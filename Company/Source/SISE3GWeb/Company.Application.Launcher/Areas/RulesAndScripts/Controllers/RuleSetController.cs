using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Framework.UIF.Web.App_GlobalResources;
using Sistran.Core.Framework.UIF.Web.Helpers.Enums;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.RulesAndScripts.Models;
using System.Threading.Tasks;
using Sistran.Core.Framework.UIF.Web.Areas.Common.Controllers;

namespace Sistran.Core.Framework.UIF.Web.Areas.RulesAndScripts.Controllers
{
    [Authorize]
    public class RuleSetController : Controller
    {
        #region ViewResult
        [HttpGet]
        public ViewResult Index()
        {
            return this.View();
        }
       

        [HttpGet]
        [ChildActionOnly]
        public ViewResult ModalRuleSet()
        {
            return this.View();
        }


        [HttpGet]
        [ChildActionOnly]
        public ViewResult ModalRule()
        {
            return this.View();
        }

        [HttpGet]
        [ChildActionOnly]
        public ViewResult ModalConditionRule()
        {
            return this.View();
        }

        [HttpGet]
        [ChildActionOnly]
        public ViewResult ModalActionRule()
        {
            return this.View();
        }

        public ActionResult AdvancedSearch()
        {
            return PartialView();
        }

        #endregion

        #region JsonResult

        /// <summary>
        /// Obtiene los paquetes de reglas por el filtro
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <param name="levels">id los niveles a buscar</param>
        /// <param name="withDecisionTable">incluir tablas de decision</param>
        /// <param name="isPolicie">es una politica</param>
        /// <param name="filter">like para la descripcion</param>
        /// <returns></returns> 
        [HttpPost]
        public JsonResult GetRulesByFilter(int idPackage, List<int> levels, bool withDecisionTable, bool isPolicie, string filter, bool maxRow)
        {
            try
            {
                List<_RuleSet> ruleSets = DelegateService.rulesEditorServices.GetRulesByFilter(idPackage, levels, withDecisionTable, isPolicie, filter, maxRow);
                return new UifJsonResult(true, ruleSets);
            }
            catch (Exception e)
            {
               return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));               
            }
    }

    /// <summary>
    /// Obtiene los paquetes de reglas para la busqueda avanzada
    /// </summary>
    /// <param name="ruleSet">filtro de regla</param>        
    /// <returns>lista de paquetes de reglas</returns>
    public JsonResult GetRulesByRuleSet(AdvSearchRuleModelView ruleAdv)
        {
            try
            {
                _RuleSet ruleSet = new _RuleSet() {
                    RuleSetId = ruleAdv.IdAdv,
                    Description = ruleAdv.RuleDescription,
                    Level = new _Level() { LevelId= ruleAdv.LevelIdAdv},
                    Package = new _Package() { PackageId = ruleAdv.TypePackageId},
                    CurrentFrom = Convert.ToDateTime(ruleAdv.DateCreation),
                    CurrentTo = ruleAdv.DateModification,
                    Type = Application.RulesScriptsServices.Enums.RuleBaseType.Sequence
                };
                List<_RuleSet> ruleSets = DelegateService.rulesEditorServices.GetRulesByRuleSet(ruleSet);
                return new UifJsonResult(true, ruleSets);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        /// obtiene los paquetes de reglas que son DT
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        [HttpPost]
        public JsonResult GetRulesDecisionTable(int idPackage)
        {
            try
            {
                List<_RuleSet> ruleSets = DelegateService.rulesEditorServices.GetRulesDecisionTable(idPackage);
                return new UifJsonResult(true, ruleSets);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        /// Obtiene el paquete de regla completo, con sus respectivas reglas del xml
        /// </summary>
        /// <param name="idRuleSet">id de la regla</param>
        /// <param name="deserializeXml">saber si se deserializa el XML</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRuleSetByIdRuleSet(int idRuleSet, bool deserializeXml)
        {
            try
            {
                _RuleSet rule = DelegateService.rulesEditorServices.GetRuleSetByIdRuleSet(idRuleSet, deserializeXml);
                    return new UifJsonResult(true, rule);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }


        /// <summary>
        /// obtiene el comparador del del concepto para la condicion de la regla
        /// </summary>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="idEntity">id de la entidad</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetComparatorConcept(int idConcept, int idEntity)
        {
            try
            {
                List<_Comparator> comparators = DelegateService.rulesEditorServices.GetComparatorConcept(idConcept, idEntity);
                return new UifJsonResult(true, comparators);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }


        /// <summary>
        /// obtiene los tipos de comparadores para la condicion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetConditionComparatorType()
        {
            try
            {
                List<_ComparatorType> comparatorType = DelegateService.rulesEditorServices.GetConditionComparatorType();
                return new UifJsonResult(true, comparatorType);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        /// <summary>
        /// obtiene los tipos de comparadores para la accion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetActionComparatorType()
        {
            try
            {
                List<_ComparatorType> comparatorType = DelegateService.rulesEditorServices.GetActionComparatorType();
                return new UifJsonResult(true, comparatorType);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        /// <summary>
        /// Obtine los tipos de acciones para la regla
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetActionType()
        {
            try
            {
                List<_ActionType> actionType = DelegateService.rulesEditorServices.GetActionType();
                return new UifJsonResult(true, actionType);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        /// Obtine los tipos de invocaciones para la accion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetInvokeType()
        {
            try
            {
                List<_InvokeType> invokeType = DelegateService.rulesEditorServices.GetInvokeType();
                return new UifJsonResult(true, invokeType);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        /// Obtine los tipos de operadores aritmeticos para la accion
        /// </summary>
        /// <returns></returns>
        public JsonResult GetArithmeticOperatorType()
        {
            try
            {
                List<_ArithmeticOperatorType> arithmeticOperator = DelegateService.rulesEditorServices.GetArithmeticOperatorType();
                return new UifJsonResult(true, arithmeticOperator);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        /// Valida la expresion matematica, y la setea de forma correcta
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidateExpression(string expression)
        {
            try
            {
                string expressionResult = DelegateService.rulesEditorServices.ValidateExpression(expression);
                return new UifJsonResult(true, expressionResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorExpressionNotValid);
            }
        }


        /// <summary>
        /// Obtiene las funciones de reglas que concuerden con la busqueda
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <param name="levels">id de los niveles</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRuleFunctionsByIdPackageLevels(int idPackage, List<int> levels)
        {
            try
            {
                List<_RuleFunction> functions = DelegateService.rulesEditorServices.GetRuleFunctionsByIdPackageLevels(idPackage, levels);
                return new UifJsonResult(true, functions);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        /// Obtiene los paquetes habilitados
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPackages()
        {
            try
            {
                List<_Package> packages = DelegateService.rulesEditorServices._GetPackages();
                return new UifJsonResult(true, packages);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        /// Obtiene los niveles por el paquete
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetLevelsByIdPackage(int idPackage)
        {
            try
            {
                List<_Level> levels = DelegateService.rulesEditorServices.GetLevelsByIdPackage(idPackage);

                return new UifJsonResult(true, levels);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }


        /// <summary>
        /// Obtiene las entities de la tabla positionEntity por paquete y nivel
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="levelId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetEntitiesByPackageIdLevelId(int packageId, int levelId)
        {
            try
            {
                List<Entity> entities = DelegateService.rulesEditorServices.GetEntitiesByPackageIdLevelId(packageId, levelId);
                return new UifJsonResult(true, entities);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        ///  Realiza la creacion de un paquete de reglas
        /// </summary>
        /// <param name="ruleSet">paquete de reglas a crear</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> CreateRuleSet(string ruleSet)
        {
            try
            {
                _RuleSet rulesetObj = RuleHelper.GetRuleSet(ruleSet);
                _RuleSet modelruleSet = await DelegateService.rulesEditorServices._CreateRuleSet(rulesetObj);
                return new UifJsonResult(true, modelruleSet);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        ///  Realiza la modificacion del paquete de reglas
        /// </summary>
        /// <param name="ruleSet">paquete de reglas a modificar</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateRuleSet(string ruleSet)
        {

            try
            {
                _RuleSet rulesetObj = RuleHelper.GetRuleSet(ruleSet);
                DelegateService.rulesEditorServices.UpdateRuleSet(rulesetObj);
                return new UifJsonResult(true, Language.MessageSavedSuccessfully);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        ///  Realiza la eliminacion del paquete de reglas
        /// </summary>
        /// <param name="ruleSetId">paquete de reglas a eliminar</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteRuleSet(int ruleSetId)
        {
            try
            {
                DelegateService.rulesEditorServices.DeleteRuleSet(ruleSetId);
                return new UifJsonResult(true, Language.MsgCorrectRemoval);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        public ActionResult ExportRuleSet(int ruleSetId)
        {
            try
            {
                string dateNow = DateTime.Now.ToString("dd-MM-yyyy");
                _RuleSet rule = DelegateService.rulesEditorServices.GetRuleSetByIdRuleSet(ruleSetId, true);
                string serializeObject = EncryptHelper.EncryptKey(JsonConvert.SerializeObject(rule));

                this.Response.AddHeader("content-disposition", $"attachment;filename = {rule.Description} ({dateNow}).rule");
                return this.File(Encoding.UTF8.GetBytes(serializeObject), "application/octet-stream");
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        public async Task<ActionResult> ImportRuleSet()
        {
            try
            {
                if (this.Request.Files.Count > 0)
                {
                    HttpPostedFileBase fileBase = this.Request.Files[0];
                    if (fileBase != null)
                    {
                        string[] nameFile = fileBase.FileName.Split('.');
                        if (nameFile.Last() == "rule")
                        {
                            string ruleSet = "";

                            using (StreamReader sr = new StreamReader(fileBase.InputStream))
                            {
                                while (sr.Peek() >= 0) ruleSet += sr.ReadLine();
                            }

                            _RuleSet rulesetObj = RuleHelper.GetRuleSet(EncryptHelper.DecryptKey(ruleSet));
                            if (rulesetObj.IsEvent == true)
                            {
                                return new UifJsonResult(false, Language.ErrorImporRulePolicie);
                            }
                            rulesetObj = await DelegateService.rulesEditorServices.ImportRuleSet(rulesetObj);
                            if(!string.IsNullOrEmpty(rulesetObj.FileExceptions))
                            {
                                rulesetObj.FileExceptions = DelegateService.commonService.GetKeyApplication("TransferProtocol") + rulesetObj.FileExceptions;
                            }
                            return new UifJsonResult(true, rulesetObj);
                        }
                        return new UifJsonResult(false, Language.ErrorInvalidFormat);
                    }
                    return new UifJsonResult(false, Language.ErrorUploadFile);
                }
                return new UifJsonResult(false, Language.ErrorUploadFile);
            }
            catch (Exception e)
            {
                {
                    return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
                }
            }
        }
        #endregion


        public ActionResult DateNow()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.commonService.GetDate().ToString(DateHelper.FormatDate));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }
        }

        [HttpGet]
        public ActionResult GetPublished()
        {
            try
            {
                var rateTypes = EnumsHelper.GetItems<PublishedType>();
                return new UifJsonResult(true, rateTypes);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryRateTypes);
            }
        }

        [HttpGet]
        public ActionResult GetRulesWithExceptions()
        {
            try
            {
                DelegateService.rulesEditorServices.GetRulesWithExceptions();
                return new UifJsonResult(true, "Consultar txt");

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryRateTypes);
            }
        }

        /// <summary>
        /// Exportar listado de reglas
        /// </summary>
        /// <returns>excel de reglas</returns>
        public JsonResult ExportRules()
        {
            try
            {
                string urlFile = DelegateService.rulesEditorServices.GenerateFileToRules();
                if (urlFile == "")
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorThereIsNoDataToExport);
                }
                return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExportExcel);
            }
        }
	}
}


