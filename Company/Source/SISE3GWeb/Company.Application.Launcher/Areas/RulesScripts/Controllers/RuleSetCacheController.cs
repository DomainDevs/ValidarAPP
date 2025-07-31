using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.RulesAndScripts.Controllers
{
    public class RuleSetCacheController : Controller
    {
        // GET: RulesAndScripts/RuleSetCache
        public ActionResult SearchRecords()
        {
            return View();
        }

        /// <summary>
        /// Obtener un listado de las version de rule set que tienen los nodos
        /// </summary>
        /// <returns>Listado de Nodo con la version actualizada.</returns>
        public ActionResult GetNodeVersions()
        {
            try
            {

                var nodeRulesetStatus = DelegateService.cacheBusinessService.GetNodeRulSet();
                return new UifJsonResult(true, nodeRulesetStatus);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error"/*App_GlobalResources.Language.ErrorSaveInsurencesObjects*/);
            }
        }


        /// <summary>
        /// Obtener un listado de las version de rule set Publicadas en general
        /// </summary>
        /// <returns>listado de las version de rule set Publicadas en general.</returns>
        public ActionResult GetPublishedVersions()
        {
            try
            {

                var versionHistory = DelegateService.cacheBusinessService.GetVersionHistory(3);
                return new UifJsonResult(true, versionHistory);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error"/*App_GlobalResources.Language.ErrorSaveInsurencesObjects*/);
            }
        }

        /// <summary>
		///  Guardar y notificar un cambio en el RuleSet
		/// </summary>
		/// <returns></returns>
		[HttpPost]
        public JsonResult RecordRuleNode(string ruleSet)
        {
            try
            {

                var versionHistory = DelegateService.cacheBusinessService.CreateVersionHistory(SessionHelper.GetUserId());
                return new UifJsonResult(true, string.Format(App_GlobalResources.Language.LabelMessagePublishRuleSet, versionHistory.Guid, versionHistory.VersionDatetime));
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message.ToString());
            }
        }

    }
}