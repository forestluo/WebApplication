using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NLDB
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            if (!isSessionActive(ctx))
            {
                if (ctx.Request.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                }

                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        Type = "LoggedOut"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                // Set the response code to 403.
                ctx.Response.StatusCode = 403;

                filterContext.HttpContext.Response.StatusCode = 403;

                ctx.Response.Redirect("~/Account/Login");
            }

            base.OnActionExecuting(filterContext);
        }

        private bool isSessionActive(HttpContext ctx)
        {
            bool sessionActive = false;

            if (ctx.Session != null && ctx.Session["UserLoggedIn"] != null)
            {
                sessionActive = true;
            }
            //this method didn't seem to work, so I'm just using the session var I set when logging in
            /*
            //see:
            //http://www.tyronedavisjr.com/2008/11/23/detecting-session-timeouts-using-a-aspnet-mvc-action-filter/
            // check if session is supported
            if (ctx.Session != null)
            {
                sessionActive = true;
                // check if a new session id was generated
                if (ctx.Session.IsNewSession)
                {
                    // If it says it is a new session, but an existing cookie exists, then it must
                    // have timed out
                    string sessionCookie = ctx.Request.Headers["Cookie"];
                    if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        sessionActive = false;

                        
                    }
                }
            }
             * */
            return sessionActive;
        }
    }
}