using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NLDBLibrary;

namespace NLDB.Controllers
{
    [Authorize, HandleJsonError, SessionExpireFilter]
    public class RuleController : Controller
    {
        // GET: Rule
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
        public ActionResult GetFilterRuleCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetFilterRuleCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectFilterRule(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectFilterRule(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRegularRuleCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetRegularRuleCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectRegularRule(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectRegularRule(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ParseContentByRegularRule(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.ParseContentByRegularRule(content), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetNumericalRuleCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetNumericalRuleCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectNumericalRule(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectNumericalRule(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ParseContentByNumericalRule(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.ParseContentByNumericalRule(content), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPhraseRuleCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetPhraseRuleCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectPhraseRule(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectPhraseRule(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ParseContentByPhraseRule(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.ParseContentByPhraseRule(content), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAttributeRuleCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetAttributeRuleCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectAttributeRule(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectAttributeRule(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ParseContentByAttributeRule(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.ParseContentByAttributeRule(content), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetParseRuleCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetParseRuleCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectParseRule(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectParseRule(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CutContentIntoSentences(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.CutContentIntoSentences(content), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FilterContentByRule(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.FilterContentByRule(content), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStructRuleCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetStructRuleCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectStructRule(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectStructRule(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExtractStructs(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.ExtractStructs(content), JsonRequestBehavior.AllowGet);
        }
    }
}