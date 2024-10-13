using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NLDBLibrary;

namespace NLDB.Controllers
{
    [Authorize, HandleJsonError, SessionExpireFilter]
    public class CorpusController : Controller
    {
        // GET: Corpus
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
        public ActionResult GetTextPoolCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetTextPoolCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTextPoolByID(int tid)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetTextPoolByID(tid), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ClearTextPoolByID(int tid)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.ClearTextPoolByID(tid), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTextPoolRandomly()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetTextPoolRandomly(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectTextPool(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectTextPool(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public ActionResult SwitchDictionaryByID(int did)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SwitchDictionaryByID(did), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult InsertDictionaryByID(int did)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.InsertDictionaryByID(did), JsonRequestBehavior.AllowGet);
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
        public ActionResult CutContentIntoSentences(string content)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.CutContentIntoSentences(content), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FMMAllDictionaryByID(int did)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.FMMAllDictionaryByID(did), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BMMAllDictionaryByID(int did)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.BMMAllDictionaryByID(did), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetClearableDictionaryCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetClearableDictionaryCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectClearableDictionary(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectClearableDictionary(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public ActionResult GetAmbiguityCount()
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.GetAmbiguityCount(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectAmbiguity(int pageIndex, int pageSize)
        {
            return Json(NLDBLibrary.DataInterfaceCache.CurrentDataInterface.SelectAmbiguity(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void DeleteAmbiguityByID(int id)
        {
            NLDBLibrary.DataInterfaceCache.CurrentDataInterface.DeleteAmbiguityByID(id);
        }

        [HttpGet]
        public void ClearAmbiguity()
        {
            NLDBLibrary.DataInterfaceCache.CurrentDataInterface.ClearAmbiguity();
        }

        [HttpGet]
        public void UpdateAmbiguityOperationByID(int id,int operation)
        {
            NLDBLibrary.DataInterfaceCache.CurrentDataInterface.UpdateAmbiguityOperationByID(id, operation);
        }
    }
}