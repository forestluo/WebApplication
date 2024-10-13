using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Web.Routing;

namespace NLDB
{
    public class NLDB : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            
            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "BasicDefault", // Route name
                "Basic/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Basic", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "CorpusDefault", // Route name
                "Corpus/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Corpus", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "RuleDefault", // Route name
                "Rule/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Rule", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "ContentDefault", // Route name
                "Content/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Content", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "ApplicationDefault", // Route name
                "Application/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Application", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            //create a DataInterface instance for each connection string we've defined and cache it
            foreach (System.Configuration.ConnectionStringSettings cs in System.Configuration.ConfigurationManager.ConnectionStrings)
            {
                NLDBLibrary.DataInterfaceCache.AddDataInterface(cs.Name, cs.ConnectionString);
            }
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
