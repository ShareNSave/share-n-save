using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using N2;
using N2.Engine;
using N2.Web;
using N2.Web.Mvc;

namespace ZeroWaste.SharePortal.Utils
{
    public class ContentItemRoute : ContentRoute
    {
        private readonly IEngine engine;
        private readonly Dictionary<Type, string> patterns;
        private readonly IRouteHandler routeHandler;
        private readonly IControllerMapper controllerMapper;
        private readonly Route innerRoute;

        public ContentItemRoute(IEngine engine, Dictionary<Type, string> patterns)
            : this(engine, patterns, null, null, null)
        {
        }

        public ContentItemRoute(IEngine engine, Dictionary<Type, string> patterns, IRouteHandler routeHandler, IControllerMapper controllerMapper, Route innerRoute)
            : base(engine, routeHandler, controllerMapper, innerRoute)
        {
            this.engine = engine;
            this.patterns = patterns;
            this.routeHandler = routeHandler ?? new MvcRouteHandler();
            this.controllerMapper = controllerMapper ?? engine.Resolve<IControllerMapper>();
            this.innerRoute = innerRoute ?? new Route("{controller}/{action}",
                new RouteValueDictionary(new { action = "Index" }),
                new RouteValueDictionary(),
                new RouteValueDictionary(new { this.engine }),
                this.routeHandler);
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData routeData = base.GetRouteData(httpContext);

            if (routeData != null)
            {
                var currentPage = routeData.CurrentPage();
                if (currentPage != null && patterns.Keys.Contains(currentPage.GetType().BaseType))
                {
                    routeData = null;
                }
            }

            if (routeData == null && patterns.Count > 0)
            {
                foreach (var pattern in patterns)
                {
                    IList<ContentItem> pages = N2.Find.Items.Where.Type.Eq(pattern.Key)
                        .Filters(new N2.Collections.PublishedFilter())
                        .Select();
                    string currentUrl = httpContext.Request.Url.AbsolutePath;
                    var page = pages.FirstOrDefault(x => currentUrl.StartsWith(x.Url));
                    if (page != null)
                    {
                        routeData = GetRouteDataForPathQuery(httpContext.Request, page.Url);
                        if (routeData != null)
                        {
                            string[] queryStrings = currentUrl.Substring(page.Url.Length).Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                            string[] parameters = pattern.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                            Regex regex = new Regex("{([0-9a-zA-z^{}]*)}");
                            for (var i = 0; i < queryStrings.Length; i++)
                            {
                                string p = parameters[i];
                                if (regex.IsMatch(p))
                                {
                                    p = p.Substring(1, p.Length - 2);
                                }
                                string v = queryStrings[i];
                                routeData.Values[p] = v;
                            }
                            break;
                        }
                    }
                }
            }

            if (routeData == null)
                routeData = base.GetRouteData(httpContext);

            return routeData;
        }

        private RouteData GetRouteDataForPathQuery(HttpRequestBase request, string baseUrl)
        {
            //On a multi-lingual site with separate domains per language,
            //the full url (with host) should be passed to UrlParser.ResolvePath():
            string host = (request.Url.IsDefaultPort) ? request.Url.Host : request.Url.Authority;
            string hostAndRawUrl = String.Format("{0}://{1}{2}", request.Url.Scheme, host, baseUrl);//Url.ToAbsolute(request.AppRelativeCurrentExecutionFilePath));
            PathData td = engine.UrlParser.FindPath(hostAndRawUrl);

            var page = td.CurrentPage;

            var actionName = td.Action;
            if (string.IsNullOrEmpty(actionName))
                actionName = request.QueryString["action"] ?? "Index";

            if (!string.IsNullOrEmpty(request.QueryString[PathData.PageQueryKey]))
            {
                int pageId;
                if (int.TryParse(request.QueryString[PathData.PageQueryKey], out pageId))
                {
                    td.CurrentPage = page = engine.Persister.Get(pageId);
                }
            }

            ContentItem part = null;
            if (!string.IsNullOrEmpty(request.QueryString[PathData.PartQueryKey]))
            {
                // part in query string is used to render a part
                int partId;
                if (int.TryParse(request.QueryString[PathData.PartQueryKey], out partId))
                    td.CurrentItem = part = engine.Persister.Get(partId);
            }

            if (page == null && part == null)
                return null;
            else if (page == null)
                page = part.ClosestPage();

            var controllerName = controllerMapper.GetControllerName((part ?? page).GetContentType());

            if (controllerName == null)
                return null;

            if (actionName == null || !controllerMapper.ControllerHasAction(controllerName, actionName))
                return null;

            var data = new RouteData(this, routeHandler);

            foreach (var defaultPair in innerRoute.Defaults)
                data.Values[defaultPair.Key] = defaultPair.Value;
            foreach (var tokenPair in innerRoute.DataTokens)
                data.DataTokens[tokenPair.Key] = tokenPair.Value;

            RouteExtensions.ApplyCurrentItem(data, controllerName, actionName, page, part);
            data.DataTokens[ContentEngineKey] = engine;

            var wrc = engine.Resolve<IWebContext>();
            if (wrc != null && wrc.RequestItems["CurrentPage"] == null)
            {
                wrc.RequestItems["CurrentPage"] = page;
            }

            return data;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return base.GetVirtualPath(requestContext, values);
        }
    }
}