using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NLDBLibrary;

namespace NLDB.Controllers
{
    [Authorize, HandleJsonError, SessionExpireFilter]
    public class ContentController : Controller
    {
        // GET: Content
        public ActionResult Index()
        {
            foreach (System.Configuration.ConnectionStringSettings cs in System.Configuration.ConfigurationManager.ConnectionStrings)
            {
                if (NLDBLibrary.DataInterfaceCache.CurrentDataInterfaceName == null)
                {
                    NLDBLibrary.DataInterfaceCache.CurrentDataInterfaceName = cs.Name;
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult GetInnerContentCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetInnerContentCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectInnerContent(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectInnerContent(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetInnerContentByID(int cid)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetInnerContentByID(cid), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetInnerContentRandomly()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetInnerContentRandomly(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOuterContentCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetOuterContentCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectOuterContent(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectOuterContent(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOuterContentByID(int oid)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetOuterContentByID(oid), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOuterContentRandomly()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetOuterContentRandomly(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetExternalContentCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetExternalContentCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectExternalContent(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectExternalContent(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetExternalContentByID(int eid)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetExternalContentByID(eid), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetExternalContentRandomly()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetExternalContentRandomly(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SplitExternalContentByID(int eid)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SplitExternalContentByID(eid), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FMMSplitExternalContentByID(int eid)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.FMMSplitExternalContentByID(eid), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BMMSplitExternalContentByID(int eid)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.BMMSplitExternalContentByID(eid), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void ContentDeleteValue(string content)
        {
            if (content != null && content.Length > 0)
                NLDBLibrary.DataInterfaceCache.CurrentDataInterface.ContentDeleteValue(content);
        }

        [HttpPost]
        public void ContentInsertValue(string content)
        {
            if (content != null && content.Length > 0)
                NLDBLibrary.DataInterfaceCache.CurrentDataInterface.ContentInsertValue(content);
        }

        [HttpPost]
        public void ContentSetTerminator(string content)
        {
            if (content != null && content.Length > 0)
                NLDBLibrary.DataInterfaceCache.CurrentDataInterface.ContentSetTerminator(content);
        }
    }
}