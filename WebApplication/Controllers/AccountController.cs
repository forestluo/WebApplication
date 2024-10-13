using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Security;
using System.Web.Script.Serialization;

using NLDB.Models;
using NLDBLibrary;

namespace NLDB.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Admin()
        {
            //print Databases Dropdown
            StringBuilder sb = new StringBuilder();

            foreach (System.Configuration.ConnectionStringSettings cs in System.Configuration.ConfigurationManager.ConnectionStrings)
            {
                if (NLDBLibrary.DataInterfaceCache.CurrentDataInterfaceName == null)
                {
                    NLDBLibrary.DataInterfaceCache.CurrentDataInterfaceName = cs.Name;
                }
                string selected = "";
                if (NLDBLibrary.DataInterfaceCache.CurrentDataInterfaceName == cs.Name)
                    selected = "selected";
                sb.Append("<option value=\"" + cs.Name + "\" " + selected + ">" + cs.Name + "</option>");
            }

            ViewData["DBDropDown"] = sb.ToString();

            Dictionary<string, object> spaceUsed = NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetSpaceUsedSummary();

            var arr = Toolbox.DictionaryToObjectArrary(spaceUsed);

            //string spaceUsedSummary = String.Format("<b>Database Name:</b> {0} <b>Size:</b> {1} <b>Unallocated:</b> {2}, <b>Reserved:</b> {3} <b>Data:</b> {4} <b>Index Size:</b> {5} <b>Unused:</b> {6}", arr);

            //probably not the best way to do this:http://stackoverflow.com/questions/313281/how-can-i-get-a-jsonresult-object-as-a-string-so-i-can-modify-it
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            
            ViewData["SpaceUsedSummary"] = serializer.Serialize(arr);//Json(arr)

            return View();
        }

        // **************************************
        // URL: /Account/Login
        // **************************************

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            string username = model.UserName.Trim();
            string password = model.Password;


            if (System.Configuration.ConfigurationManager.AppSettings["superuser_name"] == username &&
                System.Configuration.ConfigurationManager.AppSettings["superuser_pass"] == password)
            {
                FormsAuthentication.SetAuthCookie(username, true);
                Session["UserLoggedIn"] = true;//Add this so we can detect later if the session has timmed out
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Basic");
                    //return RedirectToAction("Admin", "Account");
                }
            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

    }
}
