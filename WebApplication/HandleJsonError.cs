﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace NLDB
{
    public class HandleJsonError : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.HttpContext.Request.IsAjaxRequest() || exceptionContext.Exception == null) return;

            exceptionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            exceptionContext.Result = new JsonResult
            {
                Data = new
                {
                    exceptionContext.Exception.Message,
                    exceptionContext.Exception.StackTrace
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            //ErrorSignal.FromCurrentContext().Raise(exceptionContext.Exception);
            exceptionContext.ExceptionHandled = true;
        }
    }

}