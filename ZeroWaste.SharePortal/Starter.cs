using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using ZeroWaste.SharePortal.Controllers;
using ZeroWaste.SharePortal.Models;
using ZeroWaste.SharePortal.Utils;
using N2.Definitions.Runtime;
using N2.Engine;
using N2.Plugin;
using N2.Web.Mvc;

namespace ZeroWaste.SharePortal
{
    [AutoInitialize]
    public class Starter : IPluginInitializer
    {
        public void Initialize(IEngine engine)
        {
            RegisterRoutes(RouteTable.Routes, engine);
            RegisterControllerFactory(ControllerBuilder.Current, engine);
            RegisterViewEngines(ViewEngines.Engines);
            RegisterViewTemplates(engine);
        }

        /// <summary>
        /// Gets the engine to look in the content types view forlder for dynamic templates.
        /// </summary>
        /// <param name="engine"></param>
        private void RegisterViewTemplates(IEngine engine)
        {
            engine.RegisterViewTemplates<ContentPagesController>()
                .Add<ContentPartsController>();
        }

        public static void RegisterControllerFactory(ControllerBuilder controllerBuilder, IEngine engine)
        {
            // Registers controllers in the solution for dependency injection using the IoC container provided by N2
            engine.RegisterAllControllers();

            var controllerFactory = engine.Resolve<ControllerFactoryConfigurator>()
                .NotFound<StartPageController>(sc => sc.NotFound())
                .ControllerFactory;

            controllerBuilder.SetControllerFactory(controllerFactory);
        }

        public static void RegisterRoutes(RouteCollection routes, IEngine engine)
        {
            routes.MapContentRoute("Content", engine);
            routes.MapRoute("DefaultCss",
                "DefaultCss",
                new
                {
                    controller = "StartPage",
                    action = "DefaultCss"
                });

            var patterns = new Dictionary<Type, string>();
            patterns.Add(typeof(NewsContainer), "{action}/{year}");
            routes.Insert(0, new ContentItemRoute(engine, patterns));
        }

        public static void RegisterViewEngines(ViewEngineCollection viewEngines)
        {
            viewEngines.DecorateViewTemplateRegistration();
        }
    }
}