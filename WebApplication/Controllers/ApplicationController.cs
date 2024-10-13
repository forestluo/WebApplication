using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NLDBLibrary;

namespace NLDB.Controllers
{
    [Authorize, HandleJsonError, SessionExpireFilter]
    public class ApplicationController : Controller
    {
        // GET: Application
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
        public ActionResult GetDictionaryCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetDictionaryCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDictionaryByID(int did)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetDictionaryByID(did), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SwitchDictionaryByContent(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SwitchDictionaryByContent(content), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDictionaryRandomly()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetDictionaryRandomly(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectDictionary(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectDictionary(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetFreqCountByContent(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetFreqCountByContent(content), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPrefixFreqCount(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetPrefixFreqCount(content), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SelectPrefixFreq(int pageIndex, int pageSize, string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectPrefixFreq(pageIndex, pageSize, content), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSuffixFreqCount(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetSuffixFreqCount(content), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SelectSuffixFreq(int pageIndex, int pageSize, string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectSuffixFreq(pageIndex, pageSize, content), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRelatedFreqCount(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetRelatedFreqCount(content), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SelectRelatedFreq(int pageIndex, int pageSize, string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectRelatedFreq(pageIndex, pageSize, content), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult IsTerminator(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.IsTerminator(content), JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public ActionResult GetPhraseRuleByID(int rid)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetPhraseRuleByID(rid), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAttributeRuleByID(int wid)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetAttributeRuleByID(wid), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEditablePhraseRuleCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetEditablePhraseRuleCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectEditablePhraseRule(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectEditablePhraseRule(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertPhraseRule(string rule,string classification,string attribute)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.InsertPhraseRule(rule,classification,attribute), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdatePhraseRule(int rid,string rule, string classification, string attribute)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.UpdatePhraseRule(rid, rule, classification, attribute), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void DeletePhraseRule(int rid, string rule)
        {
            NLDBLibrary.DataInterfaceCache.CurrentDataInterface.DeletePhraseRule(rid, rule);
        }

        [HttpGet]
        public void DeletePhraseRuleByID(int rid)
        {
            NLDBLibrary.DataInterfaceCache.CurrentDataInterface.DeletePhraseRuleByID(rid);
        }

        [HttpPost]
        public ActionResult SelectPhraseRuleByRule(string rule)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectPhraseRuleByRule(rule), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSubsentenceRandomly()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetSubsentenceRandomly(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MatchPhraseRule(string rule,string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.MatchPhraseRule(rule,content), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetExternalContentRandomly()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetExternalContentRandomly(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExtractPhrases(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.ExtractPhrases(content), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEditableParseRuleCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetEditableParseRuleCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectEditableParseRule(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectEditableParseRule(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertParseRule(string rule)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.InsertParseRule(rule), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateParseRule(int rid, string rule)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.UpdateParseRule(rid, rule), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void DeleteParseRule(int rid, string rule)
        {
            NLDBLibrary.DataInterfaceCache.CurrentDataInterface.DeleteParseRule(rid, rule);
        }

        [HttpGet]
        public void DeleteParseRuleByID(int rid)
        {
            NLDBLibrary.DataInterfaceCache.CurrentDataInterface.DeleteParseRuleByID(rid);
        }

        [HttpPost]
        public ActionResult SelectParseRuleByRule(string rule)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectParseRuleByRule(rule), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetParseRuleByID(int rid)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetParseRuleByID(rid), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ParseContentByRule(string rule,string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.ParseContentByRule(rule, content), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExtractStructs(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.ExtractStructs(content), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FMMSplitAllByStructRule(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.FMMSplitAllByStructRule(content), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult BMMSplitAllByStructRule(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.BMMSplitAllByStructRule(content), JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult MultilayerSplitAll(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.MultilayerSplitAll(content), JsonRequestBehavior.AllowGet);
        }
    }
}