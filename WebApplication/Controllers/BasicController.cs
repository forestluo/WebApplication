using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NLDBLibrary;

namespace NLDB.Controllers
{
    [Authorize, HandleJsonError, SessionExpireFilter]
    public class BasicController : Controller
    {
        // GET: Basic
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
        public ActionResult GetExceptionLogs()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetExceptionLogs(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetExceptionLogCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetExceptionLogCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void DeleteExceptionLog(int? eid)
        {
            if (eid.HasValue)
                NLDBLibrary.DataInterfaceCache.CurrentDataInterface.DeleteExceptionLog(eid.Value);
        }

        [HttpGet]
        public void ResetExceptionLog(int? eid)
        {
            if (eid.HasValue)
                NLDBLibrary.DataInterfaceCache.CurrentDataInterface.ResetExceptionLog(eid.Value);
        }

        [HttpGet]
        public ActionResult ExecuteQuery(string query)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.DBToolbox.Query(query, null), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStatisticInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetStatisticInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectExceptionLog(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectExceptionLog(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTextPoolInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetTextPoolInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTextPoolParsedInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetTextPoolParsedInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTextPoolLengthInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetTextPoolLengthInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDictionaryInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetDictionaryInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDictionaryEnabledInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetDictionaryEnabledInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDictionaryCountInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetDictionaryCountInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDictionaryLengthInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetDictionaryLengthInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDictionaryClassificationInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetDictionaryClassificationInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetFilterRuleInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetFilterRuleInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRegularRuleInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetRegularRuleInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetNumericalRuleInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetNumericalRuleInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAttributeRuleInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetAttributeRuleInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPhraseRuleInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetPhraseRuleInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetParseRuleInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetParseRuleInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetInnerContentInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetInnerContentInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOuterContentInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetOuterContentInfo(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetExternalContentInfo()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetExternalContentInfo(), JsonRequestBehavior.AllowGet);
        }
    }
}