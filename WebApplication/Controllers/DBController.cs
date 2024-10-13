using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NLDBLibrary;

namespace NLDB.Controllers
{
    [Authorize, HandleJsonError, SessionExpireFilter]
    public class DBController : Controller
    {
        //
        // GET: /DB/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UpdateSelectedDB(string selectedDBName)
        {
            NLDBLibrary.DataInterfaceCache.CurrentDataInterfaceName = selectedDBName;

            //Returns the Space Used Summary
            Dictionary<string, object> spaceUsed = NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetSpaceUsedSummary();
            var arr = Toolbox.DictionaryToObjectArrary(spaceUsed);

            //var data = new { name = "TestName"};
            return Json(arr, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRunningQueries()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetRunningQueries(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void KillRunningQuery(int? sessionId)
        {
            if( sessionId.HasValue )
                NLDBLibrary.DataInterfaceCache.CurrentDataInterface.KillRunningQuery(sessionId.Value);
        }

        [HttpGet]
        public ActionResult GetQueryHistory()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetQueryHistory(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetIndexFragmentation()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetIndexFragmentation(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExecuteQuery(string query)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.DBToolbox.Query(query, null), JsonRequestBehavior.AllowGet);
        }
    }
}
