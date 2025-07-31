namespace Sistran.Core.Framework.UIF.Web.Areas.RulesScripts.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Enums = Sistran.Core.Application.RulesScriptsServices.Enums;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Newtonsoft.Json;
    using Sistran.Core.Application.RulesScriptsServices.Models;
    using Sistran.Core.Framework.UIF.Web.App_GlobalResources;
    using Sistran.Core.Framework.UIF.Web.Areas.RulesScripts.Models;
    using Sistran.Core.Framework.UIF.Web.Helpers;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Framework.UIF2.Controls.UifSelect;

    [Authorize]
    public class DecisionTableController : Controller
    {
        private static Random ramdom = new Random();
        public ActionResult Index()
        {
            return View("ListDecisionTables");
        }

        /// <summary>
        /// retorna la vista parcial para la busqueda avanzada de tablas de decision
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult AdvancedSearchDt()
        {
            return View();
        }


        public ActionResult HeadTableDecision(RuleBase ruleBase)
        {
            try
            {
                DecisionTableViewModel decisionTable = new DecisionTableViewModel { RuleBase = ruleBase };
                return View(decisionTable);
            }
            catch (Exception)
            {
                return View();
            }
        }

        public ActionResult BodyTableDecision(RuleBase RuleBase)
        {
            try
            {
                DecisionTableComposite decisionTableComposite = new DecisionTableComposite();
                decisionTableComposite.RuleBase = RuleBase;
                return View(decisionTableComposite);
            }
            catch (Exception)
            {
                return View();
            }
        }

        public ActionResult CreateTableDecision(DecisionTableViewModel decisionTable)
        {
            try
            {
                if (decisionTable.RuleBase.Description != null
                    && decisionTable.ruleComposite.Actions != null
                    && decisionTable.ruleComposite.Conditions != null
                    && decisionTable.RuleBase.LevelId != 0
                    && decisionTable.RuleBase.PackageId != 0)
                {
                    var conditions = decisionTable.ruleComposite.Conditions.Where(x => x.ConceptControlCode == 0).ToList();
                    var actions = decisionTable.ruleComposite.Actions.Where(x => x.ConceptControlCode == 0).ToList();

                    for (int i = 0; i < conditions.Count; i++)
                    {
                        var concept = DelegateService.rulesEditorServices.GetConcept(conditions[i].ConceptId, conditions[i].EntityId);
                        conditions[i].ConceptControlCode = concept.ConceptControlCode;
                        conditions[i].ConceptTypeCode = concept.ConceptTypeCode;

                    }
                    for (int i = 0; i < actions.Count; i++)
                    {
                        var concept = DelegateService.rulesEditorServices.GetConcept(actions[i].ConceptId, actions[i].EntityId);
                        actions[i].ConceptControlCode = concept.ConceptControlCode;
                        actions[i].ConceptTypeCode = concept.ConceptTypeCode;
                    }

                    DelegateService.rulesEditorServices.CreateTableDecision(decisionTable.RuleBase, decisionTable.ruleComposite.Conditions, decisionTable.ruleComposite.Actions);
                }
                return new UifJsonResult(true, null);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, null);
            }
        }

        public ActionResult CreateTableDecisionRow(DecisionTableComposite RulesAdd, DecisionTableComposite RulesEdit, DecisionTableComposite RulesDelete)
        {
            try
            {
                var isCreate = false;
                if (RulesEdit.RulesComposite != null)
                {
                    foreach (var composite in RulesEdit.RulesComposite)
                    {
                        foreach (var condition in composite.Conditions)
                        {
                            if (condition.Concept.ConceptTypeCode == Enums.ConceptType.Basic && condition.Concept.ConceptControlCode == Enums.ConceptControlType.DateEditor && !string.IsNullOrEmpty(condition.Value))
                            {
                                condition.Value = DateTime.ParseExact(condition.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss");
                            }
                        }

                        foreach (var action in composite.Actions)
                        {
                            if (action.ConceptLeft.ConceptTypeCode == Enums.ConceptType.Basic && action.ConceptLeft.ConceptControlCode == Enums.ConceptControlType.DateEditor && !string.IsNullOrEmpty(action.ValueRight))
                            {
                                action.ValueRight = DateTime.ParseExact(action.ValueRight, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss");
                            }
                        }
                    }
                }

                if (RulesAdd.RulesComposite != null)
                {
                    foreach (var composite in RulesAdd.RulesComposite)
                    {
                        foreach (var condition in composite.Conditions)
                        {
                            if (condition.Concept.ConceptTypeCode == Enums.ConceptType.Basic && condition.Concept.ConceptControlCode == Enums.ConceptControlType.DateEditor && !string.IsNullOrEmpty(condition.Value))
                            {
                                condition.Value = DateTime.ParseExact(condition.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss");
                            }
                        }

                        foreach (var action in composite.Actions)
                        {
                            if (action.ConceptLeft.ConceptTypeCode == Enums.ConceptType.Basic && action.ConceptLeft.ConceptControlCode == Enums.ConceptControlType.DateEditor && !string.IsNullOrEmpty(action.ValueRight))
                            {
                                action.ValueRight = DateTime.ParseExact(action.ValueRight, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss");
                            }
                        }
                    }
                }

                isCreate = DelegateService.rulesEditorServices.CreateTableDecisionRow(RulesAdd, RulesEdit, RulesDelete);

                return new UifJsonResult(isCreate, null);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, null);
            }
        }

        public ActionResult DeleteTableDecision(int Id)
        {
            try
            {
                var isDelete = false;

                if (Id != 0)
                {
                    isDelete = DelegateService.rulesEditorServices.DeleteTableDecision(Id);
                }
                return new UifJsonResult(isDelete, null);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, null);
            }
        }

        public ActionResult PostTableDecision(int id)
        {
            try
            {
                var isPost = false;

                if (id != 0)
                {
                    isPost = DelegateService.rulesEditorServices.PostDecisionTable(id);
                }
                return new UifJsonResult(isPost, null);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, null);
            }
        }

        public ViewResult LoadFromFile()
        {
            return View();
        }

        #region Datos
        public JsonResult GetDecisionTableByDescription(string Description)
        {
            List<RuleBase> ruleBases = DelegateService.rulesEditorServices.GetDecisionTableList().Where(x => x.Description.ToLower().Contains(Description.ToLower())).ToList();
            return Json(ruleBases, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDecisionTablelist()
        {
            try
            {
                List<RuleBase> ruleBases = DelegateService.rulesEditorServices.GetDecisionTableList();
                return Json(ruleBases, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        public JsonResult GetDecisionTablelistByLevelId(int? module, int? level, bool? Published)
        {
            try
            {
                List<RuleBase> ruleBases = DelegateService.rulesEditorServices.GetDecisionTableList();

                if (module.HasValue)
                {
                    ruleBases = ruleBases.Where(x => x.PackageId == module).ToList();
                }
                if (level.HasValue)
                {
                    ruleBases = ruleBases.Where(x => x.LevelId == level).ToList();
                }

                if (Published.HasValue)
                {
                    ruleBases = ruleBases.Where(x => x.IsPublished == Published).ToList();
                }

                return Json(ruleBases, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCommisionPercentage);
            }
        }

        public JsonResult GetActionConcept(int id)
        {
            try
            {
                List<Concept> concepts = DelegateService.rulesEditorServices.GetActionConcept(id);
                return new UifJsonResult(true, concepts);
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message );
            }
        }

        public JsonResult GetConditionConcept(int id)
        {
            try
            {
                List<Concept> concepts = DelegateService.rulesEditorServices.GetConditionConcept(id);
                return new UifJsonResult(true, concepts);
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message );
            }
        }

        public JsonResult GetDecisionTableData(int id)
        {
            try
            {
                DecisionTableComposite concepts = DelegateService.rulesEditorServices.GetDecisionTableComposite(id);
                var jsonResult = new UifJsonResult(true, concepts);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
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

        [HttpPost]
        public ActionResult GetDataFromFilter(int entityId, List<ConditionFilter> filter, int? Level)
        {
            try
            {
                //validar el modulo del guion donde se ejecuta
                string dataFromFilter = DelegateService.rulesEditorServices.GetDataFromFilter(entityId, filter);

                List<ConceptViewModel> Concepts = new List<ConceptViewModel>();
                if (entityId == 315)
                {
                    Concepts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ConceptViewModel>>(dataFromFilter);
                    Concepts = Concepts.Where(x => !x.Descripción.ToUpper().Contains("(NO USAR)")).ToList();
                    if (Level != null)
                    {
                        var idList = DelegateService.rulesEditorServices.GetLevelsEntity((Enums.Level) Level);
                        dataFromFilter = JsonConvert.SerializeObject(Concepts.Where(r => idList.Contains(r.IdEntidad)).OrderBy(c => c.Descripción));
                    }
                    else
                    {
                        dataFromFilter = JsonConvert.SerializeObject(Concepts.OrderBy(x=>x.Descripción).ToList());
                    }

                }
                return Json(dataFromFilter, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.Error);
            }
        }

        public ActionResult GetComparators()
        {
            try
            {
                List<Comparator> comparator = DelegateService.rulesEditorServices.GetComparators();

                return new UifJsonResult(true, comparator);
            }
            catch (Exception)
            {
                return new UifSelectResult(new List<Comparator>());
            }
        }

        public ActionResult GetOperationTypes(int conceptId, int entityId)
        {
            try
            {
                List<Operator> _operator = DelegateService.rulesEditorServices.GetOperationTypes(conceptId, entityId);

                return new UifJsonResult(true, _operator);
            }
            catch (Exception)
            {
                return new UifSelectResult(new List<Comparator>());
            }
        }
        #endregion

        #region ValidacionTablasDecision

        /// <summary>
        /// realiza la validacion de las reglas
        /// </summary>
        /// <param name="RulesValidator"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public JsonResult ValidateTableDecision(List<Models.Rule> RulesValidator)
        {

            //valida si existen otros valores "default"
            Models.Rule lastRule = RulesValidator.Last();
            int countConditionLast = lastRule.Conditions.Where(x => x.valueOperator == null && x.symbolOperator == "Ind").Count();
            RulesValidator.Remove(RulesValidator.Last());

            var IndValues = RulesValidator.Select(x => x.Conditions.Where(y => y.valueOperator == null && y.symbolOperator == "Ind").Count() == countConditionLast).ToList();
            if (IndValues.Where(x => x == true).Count() > 0)
            {
                var Checked = IndValues.Select((item, index) => new { item, index }).Where(x => x.item == true).Select(x => x.index).ToList();
                var validList = RulesValidator.Select((item, index) => new { index = index, item = item }).Where(x => Checked.Contains(x.index)).Select(x => x.item.idRule).ToList();

                return new UifJsonResult(false, validList);
            }

            if (RulesValidator.Count() > 0)
            {
                //Valida del resto de condiciones
                RulesValidator = SetRange(RulesValidator);

                var listResult = GroupingToValidate(RulesValidator, 0);

                if (listResult.Count == 0)
                {
                    return new UifJsonResult(true, Language.ValidDecisionTable);
                }
                else
                {
                    return new UifJsonResult(false, listResult);
                }
            }
            else
            {
                return new UifJsonResult(true, Language.ValidDecisionTable);
            }
        }

        /// <summary>
        /// valida los diferentes grupos de condiciones a partir de la condicion que agrupa
        /// </summary>
        /// <param name="rulesValidator">reglas a validar</param>
        /// <param name="condition">condicion agrupadora</param>
        /// <returns></returns>
        public List<int> GroupingToValidate(List<Models.Rule> RulesValidator, int condition)
        {
            List<int> validateList = new List<int>();

            if (condition + 1 == RulesValidator[0].Conditions.Count())
            {
                validateList.AddRange(ValidateIndividualList(RulesValidator, condition));
            }
            else
            {
                var group = RulesValidator.GroupBy(x => x.Conditions[condition].symbolOperator.Trim() + " " + (x.Conditions[condition].value == null ? "" : x.Conditions[condition].value.Trim())).ToList();
                var representantGroup = group.Select(x => x.First()).ToList();
                var listTmp = ValidateIndividualList(representantGroup, condition);

                var validList = representantGroup.Select((item, index) => new { index = index, item = item }).Where(x => listTmp.Contains(x.item.idRule)).Select(x => x.index).ToList();

                //for (int i = 0; i < group.Count; i++)
                Parallel.For(0, group.Count, i =>
                {
                    if (!validList.Contains(i))
                    {
                        validateList.AddRange(GroupingToValidate(group[i].ToList(), condition + 1));
                    }
                    else
                    {
                        validateList.AddRange(group[i].Select(x => x.idRule).ToList());
                    }
                });
            }

            return validateList;
        }

        /// <summary>
        /// Realiza el agrupamiento de las reglas, por condicion 
        /// </summary>
        /// <param name="rulesValidator">reglas a validar</param>
        /// <param name="condition">condicion agrupadora</param>
        /// <returns></returns>
        private List<int> ValidateIndividualList(List<Models.Rule> rulesValidator, int condition)
        {
            List<int> validateList = new List<int>();
            List<Range> listRanges = new List<Range>();

            for (int i = 0; i < rulesValidator.Count; i++)
            {
                var ranges = SetRange(rulesValidator[i].Conditions[condition]);
                if (ranges[1] != null)
                {
                    ranges[1].UmaxValue = ranges[0].UmaxValue;
                    ranges[1].UminValue = ranges[0].UminValue;
                }
                rulesValidator[i].Conditions[condition].range1 = ranges[0];
                rulesValidator[i].Conditions[condition].range2 = ranges[1];


                if (listRanges.Count == 0)
                {
                    listRanges.Add(rulesValidator[i].Conditions[condition].range1);
                    if (rulesValidator[i].Conditions[condition].range2 != null)
                    {
                        listRanges.Add(rulesValidator[i].Conditions[condition].range2);
                    }
                }
                else
                {
                    var listRangesResult = ValidateRanges(listRanges, rulesValidator[i].Conditions[condition].range1, rulesValidator[i].Conditions[condition].range2);

                    if (listRanges.Union(listRangesResult).Except(listRanges.Intersect(listRangesResult)).Count() == 0)
                    {
                        validateList.Add(rulesValidator[i].idRule);
                    }
                    else
                    {
                        listRanges = listRangesResult;
                    }
                }
            }
            return validateList;
        }

        /// <summary>
        /// Valida los dos rangos posibles contra la lista de rangos asignados
        /// </summary>
        /// <param name="listRanges">lista de rangos</param>
        /// <param name="range1">rango a validar</param>
        /// <param name="range2">rango a valida2 (aplica solo para diferente)</param>
        /// <returns></returns>
        private List<Range> ValidateRanges(List<Range> listRanges, Range range1, Range range2)
        {
            List<Range> listRangesResult = new List<Range>();
            listRangesResult.AddRange(listRanges);

            //si la lista ya tiene el universal
            if (listRangesResult.Where(x => x.type == "Ind").Count() > 0)
            {
                return listRangesResult;
            }

            //si el rango nuevo es el universal
            if (range1.type == "Ind")
            {
                listRangesResult.Clear();
                listRangesResult.Add(range1);
                return listRangesResult;
            }

            listRangesResult = ValidateSingleRange(listRanges, range1);

            if (range2 != null)
            {
                listRangesResult = ValidateSingleRange(listRangesResult, range2);
            }

            return listRangesResult;
        }

        /// <summary>
        /// valida un rango contra una lista de rangos
        /// </summary>
        /// <param name="listRanges">lista de rangos ya asociados</param>
        /// <param name="range">rango a validar</param>
        /// <returns></returns>
        private List<Range> ValidateSingleRange(List<Range> listRanges, Range range)
        {
            List<Range> listRangesResult = new List<Range>();
            listRangesResult.AddRange(listRanges);

            //si el nuevo rango es sencillo
            if (range.type == "=")
            {
                if (listRangesResult.Where(x => Convert.ToDouble(x.minValue) == Convert.ToDouble(range.minValue) && Convert.ToDouble(x.maxValue) == Convert.ToDouble(range.maxValue)).Count() == 0)
                {
                    listRangesResult.Add(range);
                }
                return listRangesResult;
            }

            //si el nuevo rango es complejo
            foreach (var rangeItem in listRanges)
            {
                //si el rango en la iteracion es un rango sencillo
                if (rangeItem.type == "=")
                {
                    if (IsInRange(Convert.ToDouble(rangeItem.minValue), range))
                    {
                        listRangesResult.Remove(rangeItem);
                        if (listRangesResult.Where(x => Convert.ToDouble(x.minValue) == Convert.ToDouble(range.minValue) && Convert.ToDouble(x.maxValue) == Convert.ToDouble(range.maxValue)).Count() == 0)
                        {
                            listRangesResult.Add(range);
                        }
                    }
                    else
                    {
                        //si tienen el mismo limite inferior o si tienen el mismo limite superior
                        if (Convert.ToDouble(rangeItem.minValue) == Convert.ToDouble(range.minValue) || Convert.ToDouble(rangeItem.maxValue) == Convert.ToDouble(range.maxValue))
                        {
                            listRangesResult.Remove(rangeItem);
                            range.isInclusive = true;
                            if (!range.type.Contains("="))
                            {
                                range.type += "=";
                            }
                        }

                        if (listRangesResult.Where(x => Convert.ToDouble(x.minValue) == Convert.ToDouble(range.minValue) && Convert.ToDouble(x.maxValue) == Convert.ToDouble(range.maxValue)).Count() == 0)
                        {
                            listRangesResult.Add(range);
                        }
                    }
                }

                //si los dos rangos son complejos
                else
                {
                    //si los rangos forman el rango universal
                    if (IsInRange(Convert.ToDouble(rangeItem.minValue), range) || IsInRange(Convert.ToDouble(rangeItem.maxValue), range) || IsInRange(Convert.ToDouble(range.minValue), rangeItem) || IsInRange(Convert.ToDouble(range.maxValue), rangeItem))
                    {
                        if (rangeItem.type.Contains(">") && range.type.Contains("<") || rangeItem.type.Contains("<") && range.type.Contains(">"))
                        {
                            listRangesResult.Clear();
                            listRangesResult.Add(new Range()
                            {
                                name = range.name,
                                type = "Ind",
                                UmaxValue = range.UmaxValue,
                                UminValue = range.UminValue,
                                maxValue = range.UmaxValue,
                                minValue = range.UminValue,
                                isInclusive = true,
                            });
                            return listRangesResult;
                        }
                    }


                    //Si el rango de la iteracion es complejo
                    //los dos rangos a comparar tienen limite -infinito
                    if (Convert.ToDouble(rangeItem.minValue) == Convert.ToDouble(rangeItem.UminValue) && Convert.ToDouble(range.minValue) == Convert.ToDouble(range.UminValue))
                    {
                        if (IsInRange(Convert.ToDouble(rangeItem.maxValue), range))
                        {
                            listRangesResult.Remove(rangeItem);
                            if (listRangesResult.Where(x => Convert.ToDouble(x.minValue) == Convert.ToDouble(range.minValue) && Convert.ToDouble(x.maxValue) == Convert.ToDouble(range.maxValue)).Count() == 0)
                            {
                                listRangesResult.Add(range);
                            }
                        }
                    }
                    //los dos rangos a comparar tienen limite +infinito
                    else if (Convert.ToDouble(rangeItem.maxValue) == Convert.ToDouble(rangeItem.UmaxValue) && Convert.ToDouble(range.maxValue) == Convert.ToDouble(range.UmaxValue))
                    {
                        if (IsInRange(Convert.ToDouble(rangeItem.minValue), range))
                        {
                            listRangesResult.Remove(rangeItem);
                            if (listRangesResult.Where(x => Convert.ToDouble(x.minValue) == Convert.ToDouble(range.minValue) && Convert.ToDouble(x.maxValue) == Convert.ToDouble(range.maxValue)).Count() == 0)
                            {
                                listRangesResult.Add(range);
                            }
                        }
                    }
                    else
                    {
                        //si el item a comparar tiene -infinito
                        if (Convert.ToDouble(rangeItem.minValue) == Convert.ToDouble(rangeItem.UminValue))
                        {
                            //si los limites no infinitos son una interseccion 
                            if (IsInRange(Convert.ToDouble(rangeItem.maxValue), range) || IsInRange(Convert.ToDouble(range.minValue), rangeItem))
                            {
                                listRangesResult.Clear();
                                listRangesResult.Add(new Range()
                                {
                                    name = range.name,
                                    type = range.type,
                                    UmaxValue = range.UmaxValue,
                                    UminValue = range.UminValue,
                                    maxValue = range.UmaxValue,
                                    minValue = range.UminValue,
                                    isInclusive = false
                                });
                                return listRangesResult;
                            }
                            else
                            {
                                if (listRangesResult.Where(x => Convert.ToDouble(x.minValue) == Convert.ToDouble(range.minValue) && Convert.ToDouble(x.maxValue) == Convert.ToDouble(range.maxValue)).Count() == 0)
                                {
                                    listRangesResult.Add(range);
                                }
                            }
                        }
                        //si el item a comparar tiene +infinito
                        else if (Convert.ToDouble(rangeItem.maxValue) == Convert.ToDouble(rangeItem.UmaxValue))
                        {
                            if (IsInRange(Convert.ToDouble(rangeItem.minValue), range) || IsInRange(Convert.ToDouble(range.maxValue), rangeItem))
                            {
                                listRangesResult.Clear();
                                listRangesResult.Add(new Range()
                                {
                                    name = range.name,
                                    type = range.type,
                                    UmaxValue = range.UmaxValue,
                                    UminValue = range.UminValue,
                                    maxValue = range.UmaxValue,
                                    minValue = range.UminValue,
                                    isInclusive = false
                                });
                                return listRangesResult;
                            }
                            else
                            {
                                if (listRangesResult.Where(x => Convert.ToDouble(x.minValue) == Convert.ToDouble(range.minValue) && Convert.ToDouble(x.maxValue) == Convert.ToDouble(range.maxValue)).Count() == 0)
                                {
                                    listRangesResult.Add(range);
                                }
                            }
                        }
                    }
                }
            }
            return listRangesResult;
        }

        /// <summary>
        /// asigna el valor de los rangos para una condicion
        /// </summary>
        /// <param name="condition">condicion a evaluar</param>
        /// <returns></returns>
        private Range[] SetRange(Models.Condition condition)
        {
            if (condition.range1.name != "TextBox")
            {
                switch (condition.symbolOperator)
                {
                    case "Ind":
                        condition.range1.maxValue = condition.range1.UmaxValue;
                        condition.range1.minValue = condition.range1.UminValue;
                        condition.range1.type = condition.symbolOperator;
                        condition.range1.isInclusive = false;
                        break;
                    case "=":
                        condition.range1.maxValue = Convert.ToDouble(condition.value);
                        condition.range1.minValue = Convert.ToDouble(condition.value);
                        condition.range1.type = condition.symbolOperator;
                        condition.range1.isInclusive = true;
                        break;
                    case ">":
                        condition.range1.maxValue = condition.range1.UmaxValue;
                        condition.range1.minValue = Convert.ToDouble(condition.value);
                        condition.range1.type = condition.symbolOperator;
                        condition.range1.isInclusive = false;
                        break;
                    case ">=":
                        condition.range1.maxValue = condition.range1.UmaxValue;
                        condition.range1.minValue = Convert.ToDouble(condition.value);
                        condition.range1.type = condition.symbolOperator;
                        condition.range1.isInclusive = true;
                        break;
                    case "<":
                        condition.range1.maxValue = Convert.ToDouble(condition.value);
                        condition.range1.minValue = condition.range1.UminValue;
                        condition.range1.type = condition.symbolOperator;
                        condition.range1.isInclusive = false;
                        break;
                    case "<=":
                        condition.range1.maxValue = Convert.ToDouble(condition.value);
                        condition.range1.minValue = condition.range1.UminValue;
                        condition.range1.type = condition.symbolOperator;
                        condition.range1.isInclusive = true;
                        break;
                    case "<>":
                        condition.range2 = new Range()
                        {
                            name = condition.range1.name,
                            type = condition.range1.type
                        };

                        condition.range1.maxValue = Convert.ToDouble(condition.value);
                        condition.range1.minValue = condition.range1.UminValue;
                        condition.range1.type = "<";
                        condition.range1.isInclusive = false;

                        condition.range2.maxValue = condition.range1.UmaxValue;
                        condition.range2.minValue = Convert.ToDouble(condition.value);
                        condition.range2.type = ">";
                        condition.range2.isInclusive = false;
                        break;
                }
            }
            Range[] ranges = new Range[] { condition.range1, condition.range2 };
            return ranges;
        }

        /// <summary>
        /// valida el control y/o los posibles valores y asigna un rango por condicion
        /// el cual es asignado a cada una de las condiciones por regla
        /// 
        /// el rango es un intervalo Cerrado:
        ///     [a,b] los extermos hacen parte del intervalo
        /// </summary>
        /// <param name="rulesValidator"></param>
        /// <returns></returns>
        private List<Models.Rule> SetRange(List<Models.Rule> rulesValidator)
        {
            var firstRule = rulesValidator.First();

            foreach (var item in firstRule.Conditions)
            {
                dynamic conceptControl = DelegateService.rulesEditorServices.GetConceptControl(item.idConcept, item.idEntity);

                var range = new Range();
                range.name = conceptControl.Description;

                switch ((int)conceptControl.ConceptControlCode)
                {
                    case 1: //TextBoxEditor
                        range.type = typeof(string).Name;
                        range.UmaxValue = null;
                        range.UminValue = null;
                        range.maxValue = null;
                        range.minValue = null;
                        break;
                    case 2: // NumberEditor
                        range.type = typeof(long).Name;
                        range.UmaxValue = decimal.MaxValue;
                        range.UminValue = decimal.MinValue;
                        break;
                    case 3: // DateEditor
                        range.type = typeof(DateTime).Name;
                        range.UmaxValue = 3649535;
                        range.UminValue = 0;
                        break;
                    case 4: // ListBox
                        range.type = typeof(int).Name;
                        var listBox = (IEnumerable<dynamic>)conceptControl.ListListEntityValues;
                        range.UmaxValue = listBox.Max(x => Convert.ToInt32(x.ListValueCode));
                        range.UminValue = listBox.Min(x => Convert.ToInt32(x.ListValueCode));
                        break;
                    case 5: // SearchCombo
                        range.type = typeof(int).Name;
                        var GetDataFromFilter = DelegateService.rulesEditorServices.GetDataFromFilter(conceptControl.ForeignEntity, null);
                        var list = (List<dynamic>)JsonConvert.DeserializeObject<List<dynamic>>(GetDataFromFilter);
                        range.UmaxValue = Convert.ToInt32(list.Max(x => Convert.ToInt32(x.Id)));
                        range.UminValue = Convert.ToInt32(list.Min(x => Convert.ToInt32(x.Id)));
                        break;
                }

                foreach (List<Models.Condition> condition in rulesValidator.Select(x => x.Conditions.Where(y => y.idConcept == item.idConcept && y.idEntity == item.idEntity).ToList()).ToList())
                {
                    condition.ForEach(x => x.range1 = new Range() { type = range.type, maxValue = range.maxValue, minValue = range.minValue, name = range.name, UmaxValue = range.UmaxValue, UminValue = range.UminValue });
                    if (range.type == typeof(DateTime).Name)
                    {
                        condition.Where(x => x.value != null).ToList().ForEach(x => x.value = ConvertDateToInt(x.value.ToString()));
                    }
                }
            }

            return rulesValidator;
        }

        /// <summary>
        /// Convierte una valor de fecha yyyy-MM-dd, en un valor numerico
        /// </summary>
        /// <param name="date">fecha en formato yyyy-MM-dd</param>
        /// <returns></returns>
        private string ConvertDateToInt(string date)
        {
            var dateInit = new DateTime();
            var dateEnd = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            var days = dateEnd - dateInit;

            return days.TotalDays.ToString();
        }

        /// <summary>
        /// valida si el valor pertenece al rango
        /// x (E) Range ??
        /// </summary>
        /// <param name="value">valor a evaluar</param>
        /// <param name="range">rango</param>
        /// <returns></returns>
        private bool IsInRange(double value, Range range)
        {
            switch (range.type)
            {
                case "<":
                    if (value < Convert.ToDouble(range.maxValue))
                    {
                        return true;
                    }
                    break;
                case ">":
                    if (value > Convert.ToDouble(range.minValue))
                    {
                        return true;
                    }
                    break;
                case "<=":
                    if (value <= Convert.ToDouble(range.maxValue))
                    {
                        return true;
                    }
                    break;
                case ">=":
                    if (value >= Convert.ToDouble(range.minValue))
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        #endregion

        #region LoadFiles
        public JsonResult LoadPreviewXMLFile()
        {
            try
            {
                var urlFile = UploadFile();
                var line = "";
                StringBuilder str = new StringBuilder();
                using (StreamReader reader = new StreamReader(urlFile))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        str.AppendLine(line);
                    }
                }
                return new UifJsonResult(true, new string[] { Path.GetFileName(urlFile), str.ToString() });
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult LoadPreviewXLSFile(string nameXml)
        {
            try
            {
                if (!string.IsNullOrEmpty(nameXml))
                {
                    var urlFile = UploadFile();
                    return RedirectToAction("PreviewXLSFile", new { pathXml = nameXml, pathXls = Path.GetFileName(urlFile), save = false });
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.XMLFileNotSelected);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult PreviewXLSFile(string pathXml, string pathXls, bool save)
        {
            try
            {
                DecisionTableMappingResult decisionTableMappingResult = new DecisionTableMappingResult();
                if (!string.IsNullOrEmpty(pathXml) && !string.IsNullOrEmpty(pathXls))
                {
                    decisionTableMappingResult = LoadDesicionTableFromFile(pathXml, pathXls, save);
                }

                if (save)
                {
                    var internalPath = DelegateService.commonService.GetKeyApplication("SavePathExcel") + @"\" + User.Identity.Name + @"\";

                    if (!System.IO.File.Exists(internalPath + pathXml))
                    {
                        System.IO.File.Delete(internalPath + pathXml);
                    }
                    if (!System.IO.File.Exists(internalPath + pathXls))
                    {
                        System.IO.File.Delete(internalPath + pathXls);
                    }

                    return new UifJsonResult(true, decisionTableMappingResult);
                }
                else
                {
                    ViewBag.tempUrl = pathXls;
                    return View(decisionTableMappingResult.DataSet);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }


        private DecisionTableMappingResult LoadDesicionTableFromFile(string pathXml, string pathXls, bool save)
        {
            try
            {
                var externalPath = DelegateService.commonService.GetKeyApplication("ExternalFolderFiles") + @"\" + SessionHelper.GetUserName() + @"\";
                var internalPath = DelegateService.commonService.GetKeyApplication("SavePathExcel") + @"\" + User.Identity.Name + @"\";

                if (!System.IO.File.Exists(externalPath + pathXml))
                {
                    System.IO.File.Copy(internalPath + pathXml, externalPath + pathXml);
                }
                if (!System.IO.File.Exists(externalPath + pathXls))
                {
                    System.IO.File.Copy(internalPath + pathXls, externalPath + pathXls);
                }

                return DelegateService.rulesEditorServices.LoadDecisionTableFromFile(externalPath + pathXml, externalPath + pathXls, save);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private string UploadFile()
        {
            string path = DelegateService.commonService.GetKeyApplication("ExternalFolderFiles") + @"\" + User.Identity.Name + @"\";
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                    path += Guid.NewGuid() + Path.GetExtension(hpf.FileName);

                    hpf.SaveAs(path);
                }

                return path;
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                throw ex;
            }
        }
        #endregion

        public FileContentResult ExportDecisionTable(RuleBase RuleBase)
        {
            string DateNow = DateTime.Now.ToString("dd/MM/yyyy");
            Response.AddHeader("content-disposition", "attachement;filename = " + RuleBase.Description + "_" + DateNow + ".xlsx");
            try
            {
                MemoryStream stream = new MemoryStream();//crea el flujo de datos (memoria)
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    #region ExcelEmpty
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet();

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = RuleBase.Description };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());
                    #endregion

                    #region Variables
                    int columnIndex = 0;
                    int rowIndex = 1;

                    Fonts fonts = new Fonts(new Font(new FontSize() { Val = 12 })); //establece las fuentes
                    Fills fills = new Fills(new Fill(new PatternFill() { PatternType = PatternValues.Solid })); //establece los colores
                    Borders borders = new Borders(new Border()); //establece los bordes
                    CellFormats cellFormats = new CellFormats(new CellFormat());
                    CellFormat cellFormat = new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 };
                    cellFormat.Append(new Alignment()
                    {
                        Horizontal = HorizontalAlignmentValues.Center,
                        Vertical = VerticalAlignmentValues.Center
                    });
                    cellFormats.Append(cellFormat);


                    MergeCells mergeCells = new MergeCells(); //establece las celdas combinadas del documento
                    worksheetPart.Worksheet.InsertAfter(mergeCells, worksheetPart.Worksheet.Elements<SheetData>().First());
                    #endregion

                    #region Head
                    Row row = new Row();
                    //Se obtienen las condiciones
                    List<Concept> conditions = DelegateService.rulesEditorServices.GetConditionConcept(RuleBase.RuleBaseId);
                    foreach (Concept condition in conditions)
                    {
                        row.Append(GenerateExcelFile.CreateCell(condition.Description, CellValues.String, rowIndex, columnIndex, 1));

                        if (condition.ConceptControlCode == Enums.ConceptControlType.SearchCombo || condition.ConceptControlCode == Enums.ConceptControlType.ListBox)
                        {
                            mergeCells.Append(GenerateExcelFile.MergeCell(rowIndex, columnIndex, rowIndex, columnIndex + 2));
                            columnIndex += 3;
                        }
                        else
                        {
                            mergeCells.Append(GenerateExcelFile.MergeCell(rowIndex, columnIndex, rowIndex, columnIndex + 1));
                            columnIndex += 2;
                        }
                    }

                    row.Append(GenerateExcelFile.CreateCell("-->", CellValues.String, rowIndex, columnIndex, 1));
                    columnIndex++;
                    //Se obtienen las acciones
                    List<Concept> actions = DelegateService.rulesEditorServices.GetActionConcept(RuleBase.RuleBaseId);
                    foreach (Concept action in actions)
                    {
                        row.Append(GenerateExcelFile.CreateCell(action.Description, CellValues.String, rowIndex, columnIndex, 1));
                        if (action.ConceptControlCode == Enums.ConceptControlType.SearchCombo || action.ConceptControlCode == Enums.ConceptControlType.ListBox)
                        {
                            mergeCells.Append(GenerateExcelFile.MergeCell(rowIndex, columnIndex, rowIndex, columnIndex + 2));
                            columnIndex += 3;
                        }
                        else
                        {
                            mergeCells.Append(GenerateExcelFile.MergeCell(rowIndex, columnIndex, rowIndex, columnIndex + 1));
                            columnIndex += 2;
                        }
                    }

                    sheetData.AppendChild(row);
                    #endregion

                    #region Data
                    DecisionTableComposite dataComposite = DelegateService.rulesEditorServices.GetDecisionTableComposite(RuleBase.RuleBaseId);
                    foreach (RuleComposite ruleComposite in dataComposite.RulesComposite)
                    {
                        Row row2 = new Row();
                        foreach (var condition in ruleComposite.Conditions)
                        {
                            row2.Append(GenerateExcelFile.CreateCell(condition.Comparator == null ? "" : condition.Comparator.Symbol, CellValues.String, 0, 0, 1));

                            if (condition.Concept.ConceptControlCode == Enums.ConceptControlType.SearchCombo || condition.Concept.ConceptControlCode == Enums.ConceptControlType.ListBox)
                            {
                                row2.Append(GenerateExcelFile.CreateCell(condition.Value ?? "", CellValues.String, 0, 0, 1));
                            }
                            row2.Append(GenerateExcelFile.CreateCell(condition.DescriptionValue, CellValues.String, 0, 0, 1));
                        }
                        row2.Append(GenerateExcelFile.CreateCell("-->", CellValues.String, 0, 0, 1));

                        foreach (var action in ruleComposite.Actions)
                        {
                            row2.Append(GenerateExcelFile.CreateCell(action.Operator == null ? "" : action.Operator.Symbol, CellValues.String, 0, 0, 1));
                            if (action.ConceptLeft.ConceptControlCode == Enums.ConceptControlType.SearchCombo || action.ConceptLeft.ConceptControlCode == Enums.ConceptControlType.ListBox)
                            {
                                row2.Append(GenerateExcelFile.CreateCell(action.ValueRight ?? "", CellValues.String, 0, 0, 1));
                            }
                            row2.Append(GenerateExcelFile.CreateCell(action.Expression, CellValues.String, 0, 0, 1));
                        }
                        sheetData.AppendChild(row2);
                    }

                    #endregion

                    WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                    stylePart.Stylesheet = new Stylesheet(fonts, fills, borders, cellFormats);
                    stylePart.Stylesheet.Save();

                    worksheetPart.Worksheet.Save();
                }
                return File(stream.ToArray(), "Excel");
            }
            catch (Exception)
            {
                var bytes = new Byte[0];
                return File(bytes, "Excel");
            }
        }
    }
}