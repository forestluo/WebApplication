using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NLDB.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewData["Message"] = "NLDB使用ASP.NET MVC（C#）开发，点击页面上部的“登入”进入系统。";
            return View();
        }

        public ActionResult About()
        {
            ViewData["Message"] = "NLDB（数据库管理网站）由<a href=\"http://www.simpleteam.com/\">SimpleTeam Software</a>开发。";

            return View();
        }

    }
}
