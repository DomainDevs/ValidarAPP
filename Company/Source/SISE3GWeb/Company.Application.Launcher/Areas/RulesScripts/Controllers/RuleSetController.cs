using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;

using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using Level = Sistran.Core.Application.RulesScriptsServices.Models.Level;

namespace Sistran.Core.Framework.UIF.Web.Areas.RulesScripts.Controllers
{
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Company.Application.UnderwritingServices.Models;

    [Authorize]
    public class RuleSetController : Controller
    {   
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPackages()
        {
            try
            {
                List<Package> packages = DelegateService.rulesEditorServices.GetPackages().Where(p => p.Disabled == false).ToList();
                return new UifSelectResult(packages.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(new List<Package>());
            }
        }

        public JsonResult GetLevels(int? packageId)
        {
            try
            {
                List<Level> levels = DelegateService.rulesEditorServices.GetLevels((int)packageId);
                return new UifSelectResult(levels.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(new List<Level>());
            }
        }

        public JsonResult GetPrefixes()
        {
            try
            {
                List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();
                return new UifSelectResult(prefixes.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(new List<Prefix>());
            }
        }

      
        public JsonResult GetAllRuleSets(bool IsEvent)
        {
            try
            {
                List<RuleSet> ruleSetDTOs = DelegateService.rulesEditorServices.GetAllRuleSets(IsEvent);

                return Json(ruleSetDTOs, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        public JsonResult GetRuleSetDTOsByLevelId(int levelId, bool IsEvent)
        {
            try
            {
                List<RuleSet> ruleSetDTOs = DelegateService.rulesEditorServices.GetRuleSetDTOsByLevelId(levelId, IsEvent);

                return Json(ruleSetDTOs, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        public JsonResult GetRuleSetsByProductId(int? packageId, int? levelId, int? productId)
        {
            try
            {
                List<RuleSet> ruleSetDTOs = DelegateService.rulesEditorServices.GetRuleSetByPackageIdLevelIdProductId(packageId, levelId, productId);
                return Json(ruleSetDTOs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifTableResult(new List<RuleSet>());
            }
        }

        [HttpPost]
        public JsonResult GetRuleByName(string RuleName, bool IsEvent)
        {
            try
            {
                List<RuleSet> ruleSetDTOs = DelegateService.rulesEditorServices.GetAllRuleSets(IsEvent);

                ruleSetDTOs = ruleSetDTOs.Where(x => x.Description.ToUpper().Contains(RuleName.ToUpper())).ToList();

                return Json(ruleSetDTOs, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        #region Reglas        

        public ActionResult GetRuleNames(int ruleSetId)
        {
            try
            {
                List<RuleComposite> ruleName = DelegateService.rulesEditorServices.GetRuleNames(ruleSetId);
                return Json(ruleName, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifTableResult(new List<RuleName>());
            }
        }

        public ActionResult GetRuleSetComposite(int ruleSetId)
        {
            try
            {
                RuleSetComposite GetRuleSetComposite = DelegateService.rulesEditorServices.GetRuleSetComposite(ruleSetId);
                return Json(GetRuleSetComposite, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifTableResult(new List<RuleName>());
            }
        }

        #endregion

        #region Condition
        public ActionResult GetConditions(int ruleSetId, int ruleId)
        {
            try
            {
                List<Condition> condition = DelegateService.rulesEditorServices.GetConditions(ruleSetId, ruleId);
                return Json(condition, JsonRequestBehavior.AllowGet);                
            }
            catch (Exception)
            {
                return new UifTableResult(new List<Condition>());
            }
        }

        public ActionResult GetConceptComparator(int conceptId, int entityId)
        {
            try
            {
                List<ComparatorConcept> comparatorConcept = DelegateService.rulesEditorServices.GetConceptComparator(conceptId, entityId);
                return new UifSelectResult(comparatorConcept);
            }
            catch (Exception)
            {
                return new UifSelectResult(new List<ComparatorConcept>());
            }
        }

        public ActionResult GetConceptControl(int conceptId, int entityId)
        {
            try
            {
                ConceptControl conceptControl = DelegateService.rulesEditorServices.GetConceptControl(conceptId, entityId);
                return new UifJsonResult(true, conceptControl);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCommisionPercentage);
            }
        }

        public ActionResult GetDynamicConcept(int conceptId, int entityId)
        {
            try
            {
                object conceptControl = DelegateService.rulesEditorServices.GetDynamicConcept(conceptId, entityId);
                return new UifJsonResult(true, conceptControl);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCommisionPercentage);
            }
        }
        #endregion

        public ActionResult GetValueTypes()
        {
            try
            {
                List<Sistran.Core.Application.RulesScriptsServices.Models.ValueType> listAction = DelegateService.rulesEditorServices.GetValueTypes();
                return new UifSelectResult(listAction);
            }
            catch (Exception)
            {
                return new UifSelectResult(new List<Sistran.Core.Application.RulesScriptsServices.Models.ValueType>());
            }
        }

        #region Action
        public ActionResult GetActions(int ruleSetId, int ruleId)
        {
            try
            {
                List<Sistran.Core.Application.RulesScriptsServices.Models.Action> action = DelegateService.rulesEditorServices.GetActions(ruleSetId, ruleId);
                return Json(action, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifTableResult(new List<ListAction>());
            }
        }

        public ActionResult GetActionTypeCollection()
        {
            try
            {
                List<ActionTypeCode> listAction = DelegateService.rulesEditorServices.GetActionTypeCollection();

                return new UifSelectResult(listAction);
            }
            catch (Exception)
            {
                return new UifSelectResult(new List<ActionTypeCode>());
            }
        }

        public ActionResult GetFunctionTypes()
        {
            try
            {
                List<ListAction> listAction = DelegateService.rulesEditorServices.GetFunctionTypes();

                return new UifSelectResult(listAction);
            }
            catch (Exception)
            {
                return new UifSelectResult(new List<ListAction>());
            }
        }

        public ActionResult GetOperationTypes(int conceptId, int entityId)
        {
            try
            {
                List<Operator> listAction = DelegateService.rulesEditorServices.GetOperationTypes(conceptId, entityId);

                return new UifSelectResult(listAction);
            }
            catch (Exception)
            {
                return new UifSelectResult(new List<Operator>());
            }
        }

        public ActionResult GetRuleFunctions()
        {
            try
            {
                List<RuleFunction> ruleFunction = DelegateService.rulesEditorServices.GetRuleFunctions();
                return new UifSelectResult(ruleFunction);
            }
            catch (Exception)
            {
                return new UifSelectResult(new List<RuleFunction>());
            }
        }
        #endregion

        public ActionResult CreateRuleSet(RuleSetComposite ruleSetComposite)
        {
            try
            {
                if (ruleSetComposite.RuleComposites != null)
                {
                    foreach (var ruleComposites in ruleSetComposite.RuleComposites)
                    {
                        if (ruleComposites.Conditions != null)
                        {
                            foreach (var condition in ruleComposites.Conditions)
                            {
                                if (condition.ConceptValue != null)
                                {
                                    condition.ConceptValue = DelegateService.rulesEditorServices.GetConcept(condition.ConceptValue.ConceptId, condition.ConceptValue.EntityId);
                                }
                                if (condition.Concept != null)
                                {
                                    condition.Concept = DelegateService.rulesEditorServices.GetConcept(condition.Concept.ConceptId, condition.Concept.EntityId);
                                }
                            }    
                        }

                        if (ruleComposites.Actions != null)
                        {
                            foreach (var action in ruleComposites.Actions)
                            {
                                if (action.ConceptLeft != null)
                                {
                                    action.ConceptLeft = DelegateService.rulesEditorServices.GetConcept(action.ConceptLeft.ConceptId, action.ConceptLeft.EntityId);
                                }
                                if (action.ConceptRight != null)
                                {
                                    action.ConceptRight = DelegateService.rulesEditorServices.GetConcept(action.ConceptRight.ConceptId, action.ConceptRight.EntityId);
                                }
                                if (!string.IsNullOrEmpty(action.Message))
                                {
                                    action.InvokeType = InvokeType.MessageInvoke;
                                    action.AssignType = AssignType.InvokeAssign;
                                }
                                if (action.IdFuction != null)
                                {
                                    action.InvokeType = InvokeType.FunctionInvoke;
                                    action.AssignType = AssignType.InvokeAssign;
                                }
                                if (action.RuleSetId != 0)
                                {
                                    action.InvokeType = InvokeType.RuleSetInvoke;
                                    action.AssignType = AssignType.InvokeAssign;
                                }
                            }    
                        }                        
                    }
                    DelegateService.rulesEditorServices.CreateRuleSet(ruleSetComposite);
                }
                return new UifJsonResult(true, null);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, null);
            }
        }

        public PartialViewResult AdvancedSearch()
        {
            return PartialView();
        }

        public ActionResult DeleteRuleSet(int ruleSetId)
        {
            try
            {
                DelegateService.rulesEditorServices.DeleteRuleSet(ruleSetId);
                
                return new UifJsonResult(true, string.Format(App_GlobalResources.Language.MsgDeleteSuccessfull,App_GlobalResources.Language.LabelRuleSet));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

    }
}


