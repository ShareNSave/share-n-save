using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using N2;
using N2.Collections;
using N2.Edit;
using N2.Persistence;
using N2.Web;
using N2.Web.Mvc.Html;
using N2.Web.UI.WebControls;
using Tree = N2.Web.Tree;

namespace ZeroWaste.SharePortal
{
    public static class HtmlHelpers
    {
        /// <summary>
        /// Render a list of links of content items with a separator
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TContentItem"></typeparam>
        /// <param name="helper"></param>
        /// <param name="items"></param>
        /// <param name="separator"></param>
        /// <param name="linkModifier"></param>
        /// <returns></returns>
        public static IHtmlString ContentItemLinkList<TModel, TContentItem>
            (this HtmlHelper<TModel> helper,
            IEnumerable<TContentItem> items,
            string separator = ", ",
            Action<ILinkBuilder> linkModifier = null)
            where TContentItem : ContentItem
        {
            var html = string.Join(separator, items.Select(x =>
            {
                var link = Link.To(x);
                if (linkModifier != null)
                    linkModifier(link);
                return link.ToString();
            }));
            return MvcHtmlString.Create(html);
        }

        #region Tree

        /// <summary>
        /// Build a tree that is bootstrap friendly
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="helper"></param>
        /// <param name="startFrom"></param>
        /// <param name="currentItem"></param>
        /// <param name="takeLevel"></param>
        /// <param name="filter"></param>
        /// <param name="appendCreatorNode"></param>
        /// <param name="includeRoot"></param>
        /// <param name="onHoverDropdowns"></param>
        /// <returns></returns>
        public static Tree BootstrapTree<TModel>(this HtmlHelper<TModel> helper,
            ContentItem startFrom = null,
            ContentItem currentItem = null,
            int takeLevel = 2,
            ItemFilter filter = null,
            bool appendCreatorNode = false,
            bool includeRoot = true,
            bool onHoverDropdowns = false)
        {
            // prep
            if (startFrom == null) startFrom = helper.StartPage();
            if (currentItem == null) currentItem = helper.CurrentPage();
            if (filter == null)
                filter = new NavigationFilter(helper.ViewContext.RequestContext.HttpContext.User, helper.ContentEngine().SecurityManager);

            HierarchyBuilder builder = new ParallelRootHierarchyBuilder(startFrom, takeLevel);
            builder.GetChildren = (builder.GetChildren = (item) =>
            {
                var items = item.Children.Where(filter);
                if (appendCreatorNode && item.IsPage && helper.GetControlPanelState().IsFlagSet(ControlPanelState.DragDrop))
                    items = items.AppendCreatorNode(helper.ContentEngine(), item);
                return items.ToList();
            });

            var node = builder.Build();
            if (!includeRoot)
                node.Children.RemoveAt(0);

            var tree = Tree.Using(node);
            tree.HtmlAttibutes(new { @class = "nav" });
            ClassifyAnchors(startFrom, currentItem, tree);

            return tree;
        }

        private static void ClassifyAnchors(ContentItem startsFrom, ContentItem current, Tree tree)
        {
            var ancestors = GenericFind<ContentItem, ContentItem>.ListParents(current, startsFrom, true);
            if (ancestors.Contains(startsFrom))
            {
                ancestors.Remove(startsFrom);
            }
            tree.LinkWriter((n, w) =>
            {
                var link = n.Current.Link();
                var @class = n.Current == current ? "current" : ancestors.Contains(n.Current) ? "trail" : "";
                if (n.Children.Count > 0)
                {
                    @class += " dropdown-toggle";
                    link.Attribute("data-toggle", "dropdown");
                }
                link.Class(@class).WriteTo(w);
            });
            tree.ULTagModifier((n, t) =>
            {
                if (n.Parent != null && n.Parent.Current != null)
                {
                    t.MergeAttribute("class", "dropdown-menu");
                }
            });
            tree.LITagModifier((n, t) =>
            {
                //if children, indicate with a dropdown class
                var @class = n.Children.Count > 0 ? "dropdown" : string.Empty;
                @class += " " + (n.Current == current
                                ? "active"
                                : ancestors.Contains(n.Current) ? "active trail" : "");
                t.MergeAttribute("class", @class.Trim());
            });
        }

        /// <summary>
        /// Add attibtues the root of a treeview
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static Tree HtmlAttibutes(this Tree tree, object htmlAttributes)
        {
            tree.Tag((node, builder) =>
            {
                if (builder.TagName.Equals("ul", StringComparison.InvariantCultureIgnoreCase)
                    && (node.Parent == null || node.Parent.Current == null))
                {
                    builder.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
                }
            });
            return tree;
        }

        public static Tree ULTagModifier(this Tree tree, Action<HierarchyNode<ContentItem>, TagBuilder> tagModifier)
        {
            return tree.Tag((n, t) =>
            {
                if (t.TagName.Equals("ul", StringComparison.InvariantCultureIgnoreCase))
                {
                    tagModifier(n, t);
                }
            });
        }

        public static Tree LITagModifier(this Tree tree, Action<HierarchyNode<ContentItem>, TagBuilder> tagModifier)
        {
            return tree.Tag((n, t) =>
            {
                if (t.TagName.Equals("li", StringComparison.InvariantCultureIgnoreCase))
                {
                    tagModifier(n, t);
                }
            });
        }

        #endregion

        #region PagingLinksBuilder

        /// <summary>
        /// Builds a paging links builder
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="currentPage"></param>
        /// <param name="totalPages"></param>
        /// <param name="pageUrlBuilder"></param>
        /// <returns></returns>
        public static PagingLinksBuilder PagingLinksBuilder(this HtmlHelper helper, int currentPage, int totalPages, Func<int, string> pageUrlBuilder)
        {
            return new PagingLinksBuilder(currentPage, totalPages, pageUrlBuilder);
        }

        /// <summary>
        /// Builds a paging links builder
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="pagedList"></param>
        /// <param name="pageUrlBuilder"></param>
        /// <returns></returns>
        public static PagingLinksBuilder PagingLinksBuilder<T>(this HtmlHelper helper, PagedList<T> pagedList, Func<int, string> pageUrlBuilder)
        {
            return PagingLinksBuilder(helper, pagedList.PageNumber, pagedList.TotalPages, pageUrlBuilder);
        }

        #endregion
    }
}