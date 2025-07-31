// -----------------------------------------------------------------------
// <copyright file="DecisionTableController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.RulesAndScripts.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using App_GlobalResources;
    using Application.CommonService.Models;
    using Helpers;
    using Models;
    using Newtonsoft.Json;
    using Services;
    using Sistran.Core.Framework.UIF.Web.Areas.Common.Controllers;
    using UIF2.Controls.UifTable;
    using Web.Models;
    using MRules = Application.RulesScriptsServices.Models;
    

    //[SessionState(SessionStateBehavior.ReadOnly)]
    [Authorize]
    public class DecisionTableController : Controller
    {
        #region Vistas

        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public ActionResult Header()
        {
            return this.View();
        }
        [HttpGet]
        public ActionResult AdvancedSearchDt()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult Data()
        {
            return this.View();
        }

        [HttpGet]
        public ActionResult ModalConceptHeader()
        {
            return this.View();
        }

        [HttpGet]
        public ViewResult LoadFromFile()
        {
            return this.View();
        }

        #endregion

        #region  JsonResult

        /// <summary>
        /// Obtiene el listado de tablas de decision por el filtro indicado
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <param name="levels">lista de los niveles</param>
        /// <param name="isPolicie">si es politica</param>
        /// <param name="filter">filtro like de la descripcion</param>
        [HttpPost]
        public JsonResult GetDecisionTableByFilter(int? idPackage, List<int> levels, bool isPolicie, string filter, int tableId, DateTime? dateModification, bool? isPublished)
        {
            try
            {
                this.TempData = null;
                var decisionTable =
                    DelegateService.rulesEditorServices.GetDecisionTableByFilter(idPackage, levels, isPolicie, filter, tableId, dateModification, isPublished);
                return new UifJsonResult(true, decisionTable);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        /// elimina la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        [HttpPost]
        public JsonResult DeleteTableDecision(int ruleBaseId)
        {
            try
            {
                DelegateService.rulesEditorServices._DeleteTableDecision(ruleBaseId);
                return new UifJsonResult(true, Language.MessageSavedSuccessfully);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        /// obtiene la cabecera de la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        [HttpPost]
        public JsonResult GetTableDecisionHead(int ruleBaseId)
        {
            try
            {
                var conceptsCondition =
                    DelegateService.rulesEditorServices.GetConceptsConditionByRuleBaseId(ruleBaseId);
                var conceptsAction = DelegateService.rulesEditorServices.GetConceptsActionByRuleBaseId(ruleBaseId);

                return new UifJsonResult(true, new { conceptsCondition, conceptsAction });
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }


        /// <summary>
        /// obtiene la Data de la tabla de decision 
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        /// <returns></returns>
        public JsonResult GetTableDecisionBody(int ruleBaseId, UifTableParam tableParam)//// UifTableParam tableParam
        {
            int start = Convert.ToInt16(tableParam.Start);
            int length = Convert.ToInt16(tableParam.Length);
            string raw = tableParam.Raw;

            try
            {
                int filtrados = 0;

                List<RuleModelView> tableDecision;
                if (this.TempData["tabla_" + ruleBaseId] == null)
                {
                    List<MRules._Rule> tablaOriginal = DelegateService.rulesEditorServices.GetTableDecisionBody(ruleBaseId);
                    tableDecision = RuleModelView.RulesModelView(tablaOriginal);
                    this.TempData["tabla_" + ruleBaseId] = tableDecision;
                }
                else
                {
                    if (this.TempData["Errortabla_" + ruleBaseId] != null)
                    {
                        List<RuleModelView> tableDecisionError = this.TempData["Errortabla_" + ruleBaseId] as List<RuleModelView>;
                        this.TempData["Errortabla_" + ruleBaseId] = tableDecisionError;


                        tableDecision = this.TempData["tabla_" + ruleBaseId] as List<RuleModelView>;
                        this.TempData["tabla_" + ruleBaseId] = tableDecision;

                        tableDecision = this.TempData["Errortabla_" + ruleBaseId] as List<RuleModelView>;
                        this.TempData["Errortabla_" + ruleBaseId] = tableDecision;

                    }
                    else
                    {
                        tableDecision = this.TempData["tabla_" + ruleBaseId] as List<RuleModelView>;
                        this.TempData["tabla_" + ruleBaseId] = tableDecision;
                    }
                }

                tableDecision = tableDecision.Where(x => x.Status != "Deleted").ToList();
                int totalReglas = tableDecision.Count;


                tableDecision = this.FilterDt(tableDecision, tableParam.columns);
                filtrados = tableDecision.Count;

                tableDecision = tableDecision.Skip(start).Take(length).ToList();

                return new UifTableResult(tableDecision.Select(b => new
                {
                    Actions = b.Actions,
                    Conditions = b.Conditions,
                    Separator = b.Separator,
                    Rule = new { b.Rule.RuleId }
                }).ToList(), raw, totalReglas, filtrados);

            }
            catch (Exception ex)
            {
                return this.Json(new { data = new ArrayList(), raw, totalRows = 0, totalDisplay = 0, error = ExceptionHelper.GetMessage(ex.Message) }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRuleByItem(int ruleBaseId, int ruleId)
        {
            try
            {
                List<RuleModelView> tableDecisionError = this.TempData["Errortabla_" + ruleBaseId] as List<RuleModelView>;
                this.TempData["Errortabla_" + ruleBaseId] = tableDecisionError;

                if (tableDecisionError == null || tableDecisionError.Count == 0)
                {
                    List<RuleModelView> tableDecision = this.TempData["tabla_" + ruleBaseId] as List<RuleModelView>;
                    this.TempData["tabla_" + ruleBaseId] = tableDecision;
                    return new UifJsonResult(true, tableDecision.Where(b => b.Rule.RuleId == ruleId).FirstOrDefault().Rule);
                }
                else
                {
                    List<RuleModelView> tableDecision = this.TempData["Errortabla_" + ruleBaseId] as List<RuleModelView>;
                    this.TempData["Errortabla_" + ruleBaseId] = tableDecision;
                    return new UifJsonResult(true, tableDecision.Where(b => b.Rule.RuleId == ruleId).FirstOrDefault().Rule);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(true, ExceptionHelper.GetMessage(ex.Message));
            }

        }

        /// <summary>
        /// Realiza la el guardado de la cabecera de una tabla de decision
        /// </summary>
        /// <param name="ruleBase">DT a guardar</param>
        /// <param name="conceptCondition">conceptos de la condicion</param>
        /// <param name="conceptAction">conceptos de la accion</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateTableDecisionHead(MRules._RuleBase ruleBase, List<MRules._Concept> conceptCondition,
            List<MRules._Concept> conceptAction)
        {
            try
            {
                MRules._RuleBase ruleBaseCreate = DelegateService.rulesEditorServices.CreateTableDecisionHead(ruleBase, conceptCondition, conceptAction);
                this.TempData["tabla_" + ruleBase.RuleBaseId] = null;
                return new UifJsonResult(true, ruleBaseCreate);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        /// Realiza la actualizacion de la cabecera de una tabla de decision
        /// </summary>
        /// <param name="ruleBase">DT a guardar</param>
        /// <param name="conceptCondition">conceptos de la condicion</param>
        /// <param name="conceptAction">conceptos de la accion</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateTableDecisionHead(MRules._RuleBase ruleBase, List<MRules._Concept> conceptCondition,
            List<MRules._Concept> conceptAction)
        {
            try
            {
                DelegateService.rulesEditorServices.UpdateTableDecisionHead(ruleBase, conceptCondition, conceptAction);
                this.TempData["tabla_" + ruleBase.RuleBaseId] = null;
                return new UifJsonResult(true, Language.MessageSavedSuccessfully);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        /// Realiza el guardado de la DT
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        /// <param name="lastRule">Regla default</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveDecisionTable(int ruleBaseId, string lastRule)
        {
            try
            {
                List<RuleModelView> tableDecision = this.TempData["tabla_" + ruleBaseId] as List<RuleModelView>;
                this.TempData["tabla_" + ruleBaseId] = tableDecision;

                if (tableDecision != null)
                {
                    List<MRules._Rule> rules = RuleHelper.FillRules(JsonConvert.DeserializeObject<dynamic>(lastRule));
                    List<RuleModelView> ruleItem = RuleModelView.RulesModelView(rules);
                    int ruleId = 0;

                    if (tableDecision.Count(x => x.Status != "Deleted") == 0)
                    {
                        if (tableDecision.Count != 0)
                        {
                            ruleId = tableDecision.Max(x => x.Rule.RuleId);
                        }

                        ruleItem[0].Rule.RuleId = ruleId + 1;
                        ruleItem[0].Rule.Description = $"r_{ruleId + 1}";
                        ruleItem[0].Status = "Added";
                        tableDecision.Add(ruleItem[0]);
                    }
                    else
                    {
                        List<MRules._Rule> lastRules = tableDecision.Where(x => x.Status != "Deleted").Select(x => x.Rule).ToList();
                        MRules._Rule lastRuleTmp = lastRules[lastRules.Count - 1];

                        if (rules[0].Conditions.Count(x => x.Comparator == null) != lastRuleTmp.Conditions.Count(x => x.Comparator == null))
                        {
                            ruleId = tableDecision.Max(x => x.Rule.RuleId);

                            ruleItem[0].Rule.RuleId = ruleId + 1;
                            ruleItem[0].Rule.Description = $"r_{ruleId + 1}";
                            ruleItem[0].Status = "Added";
                            tableDecision.Add(ruleItem[0]);
                        }
                    }

                    List<MRules._Rule> rulesAdd = tableDecision.Where(x => x.Status == "Added").Select(x => x.Rule).ToList();
                    List<MRules._Rule> rulesEdit = tableDecision.Where(x => x.Status == "Edited").Select(x => x.Rule).ToList();
                    List<MRules._Rule> rulesDelete = tableDecision.Where(x => x.Status == "Deleted").Select(x => x.Rule).ToList();

                    if (rulesAdd.Any() || rulesEdit.Any() || rulesDelete.Any())
                    {
                        DelegateService.rulesEditorServices.SaveDecisionTable(ruleBaseId, rulesAdd, rulesEdit, rulesDelete);
                    }

                    tableDecision = tableDecision.Where(x => x.Status != "Deleted").ToList();
                    tableDecision.ForEach(x => x.Status = null);
                    this.TempData["tabla_" + ruleBaseId] = this.ReorderDt(tableDecision);

                    return new UifJsonResult(true, Language.MessageSavedSuccessfully);
                }
                else
                {
                    this.TempData["tabla_" + ruleBaseId] = this.TempData["tabla_" + ruleBaseId] as List<RuleModelView>;
                    return new UifJsonResult(false, Language.UnexpectedError);
                }
            }
            catch (Exception e)
            {
                this.TempData["tabla_" + ruleBaseId] = this.TempData["tabla_" + ruleBaseId] as List<RuleModelView>;
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        /// valida y publica una TD
        /// </summary>
        /// <param name="ruleBaseId">id dT a publicar</param>
        /// <param name="isEvent">si la Dt es un evento</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> PublishDecisionTable(int ruleBaseId, bool isEvent)
        {
            try
            {
                List<RuleModelView> tableDecision = this.TempData["tabla_" + ruleBaseId] as List<RuleModelView>;
                this.TempData["tabla_" + ruleBaseId] = tableDecision;
                foreach (var item in tableDecision)
                {
                    foreach (var item2 in item.Rule.Actions)
                    {
                        if (item2.Expression == null)
                        {
                            return new UifJsonResult(false, Language.AllActionsHaveValue);
                        }
                    }
                }
                List<int> listInvalid = await DelegateService.rulesEditorServices.PublishDecisionTable(ruleBaseId, isEvent);

                if (listInvalid.Count == 0)
                {
                    this.TempData["Errortabla_" + ruleBaseId] = null;
                    return new UifJsonResult(true, Language.PublishedCorrectly);
                }
                else
                {
                    this.TempData["Errortabla_" + ruleBaseId] = tableDecision.Where(x => listInvalid.Contains(x.Rule.RuleId)).ToList();
                    return new UifJsonResult(false, listInvalid);
                }

            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        private string UploadFile()
        {
            string path = DelegateService.commonService.GetKeyApplication("SavePathExcel") + @"\" + this.User.Identity.Name + @"\";
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (string file in this.Request.Files)
                {
                    HttpPostedFileBase hpf = this.Request.Files[file];
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

        public JsonResult LoadPreviewXmlFile()
        {
            try
            {
                var urlFile = this.UploadFile();
                var line = "";
                StringBuilder str = new StringBuilder();
                using (StreamReader reader = new StreamReader(urlFile))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        str.AppendLine(line);
                    }
                }

                return new UifJsonResult(true, new[] { Path.GetFileName(urlFile), str.ToString() });
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult LoadPreviewXlsFile(string nameXml)
        {
            try
            {
                if (!string.IsNullOrEmpty(nameXml))
                {
                    var urlFile = this.UploadFile();
                    return this.RedirectToAction("PreviewXlsFile", new { pathXml = nameXml, pathXls = Path.GetFileName(urlFile), save = false, isEvent = false });
                }
                else
                {
                    return new UifJsonResult(false, Language.XMLFileNotSelected);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public async Task<ActionResult> PreviewXlsFile(string pathXml, string pathXls, bool save, bool isEvent)
        {
            MRules._DecisionTableMappingResult decisionTableMappingResult = new MRules._DecisionTableMappingResult();
            try
            {
                if (!string.IsNullOrEmpty(pathXml) && !string.IsNullOrEmpty(pathXls))
                {
                    decisionTableMappingResult = await this.LoadDesicionTableFromFile(pathXml, pathXls, save, isEvent);

                    if (!string.IsNullOrEmpty(decisionTableMappingResult.ErrorMessage))
                    {
                        return new UifJsonResult(false, decisionTableMappingResult.ErrorMessage);
                    }
                    else if (decisionTableMappingResult.ErrorMessage != null)
                    {
                        return new UifJsonResult(false, decisionTableMappingResult.ErrorMessage);
                    }
                    else if (decisionTableMappingResult.FileExceptions != null)
                    {
                        var filenamefromPath = decisionTableMappingResult.FileExceptions.Split(new char[] { '\\' }).Last();
                        return new UifJsonResult(false, new { url = this.Url.Action("ShowExcelFile", "DecisionTable") + "?url=" + decisionTableMappingResult.FileExceptions, FileName = filenamefromPath });
                        //return new UifJsonResult(false, new { url = DelegateService.commonService.GetKeyApplication("TransferProtocol") + decisionTableMappingResult.FileExceptions });
                    }
                }

                if (save)
                {
                    var internalPath = DelegateService.commonService.GetKeyApplication("SavePathExcel") + @"\" + this.User.Identity.Name + @"\";

                    if (!System.IO.File.Exists(internalPath + pathXml))
                    {
                        System.IO.File.Delete(internalPath + pathXml);
                    }

                    if (!System.IO.File.Exists(internalPath + pathXls))
                    {
                        System.IO.File.Delete(internalPath + pathXls);
                    }

                    this.TempData["tabla_" + decisionTableMappingResult.RuleBase.RuleBaseId] = null;

                    return new UifJsonResult(true, decisionTableMappingResult);
                }
                else
                {
                    this.ViewBag.tempUrl = pathXls;
                    return this.View(decisionTableMappingResult.DataSet);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        private async Task<MRules._DecisionTableMappingResult> LoadDesicionTableFromFile(string pathXml, string pathXls, bool save, bool isEvent)
        {
            try
            {
                var externalPath = DelegateService.commonService.GetKeyApplication("ExternalFolderFiles") + @"\" + this.User.Identity.Name + @"\";
                var internalPath = DelegateService.commonService.GetKeyApplication("SavePathExcel") + @"\" + this.User.Identity.Name + @"\";

                if (!Directory.Exists(externalPath))
                {
                    Directory.CreateDirectory(externalPath);
                }

                if (!Directory.Exists(internalPath))
                {
                    Directory.CreateDirectory(internalPath);
                }

                if (!System.IO.File.Exists(externalPath + pathXml))
                {
                    System.IO.File.Copy(internalPath + pathXml, externalPath + pathXml);
                }

                if (!System.IO.File.Exists(externalPath + pathXls))
                {
                    System.IO.File.Copy(internalPath + pathXls, externalPath + pathXls);
                }

                return await DelegateService.rulesEditorServices._LoadDecisionTableFromFile(externalPath + pathXml, externalPath + pathXls, save, isEvent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult GenerateFileToExportDecisionTable(int ruleBaseId)
        {
            try
            {
                string urlFile = DelegateService.rulesEditorServices.ExportDecisionTable(ruleBaseId);
              
                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, Language.ErrorFileNotFound);
                }
                else
                {
                    var filenamefromPath = urlFile.Split(new char[] { '\\' }).Last();
                    return new UifJsonResult(true, new { Url = this.Url.Action("ShowExcelFile", "DecisionTable") + "?url=" + urlFile, FileName = filenamefromPath });
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public FileResult ShowExcelFile(string url)
        {
            var pathToTheFile = url;
            var fileStream = new FileStream(pathToTheFile, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public JsonResult descarga (string urlFile)
        {
            try
            {  string downloadsPath = Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),@"Downloads\");
               String prueba;
               prueba = Path.GetFileName(urlFile).ToString();
           
              System.Net.WebClient webClient = new System.Net.WebClient();
              webClient.DownloadFile(urlFile, downloadsPath + prueba );
              
               System.Diagnostics.Process.Start(downloadsPath + prueba);
              
               return new UifJsonResult(true, downloadsPath + prueba);

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }

        }

        [HttpPost]
        public ActionResult GenerateFileToExportDecisionTables()
        {
            try
            {
                string urlFile = DelegateService.rulesEditorServices.ExportDecisionTables();

                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, Language.ErrorFileNotFound);
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGeneratingFile);
            }
        }
        #endregion

        /// <summary>
        /// Realiza las operaciones de CRUD sobre la TD temporal
        /// </summary>
        /// <param name="ruleBaseId">id de la DT</param>
        /// <param name="rule">regla a validar</param>
        /// <param name="status">estado de la regla (add/edit/delete)</param>
        /// <returns>operacion </returns>
        [HttpPost]
        public JsonResult DecisionTableItem(int ruleBaseId, string rule, string status)
        {
            try
            {
                List<RuleModelView> tableDecision = this.TempData["tabla_" + ruleBaseId] as List<RuleModelView>;
                List<MRules._Rule> rules = RuleHelper.FillRules(JsonConvert.DeserializeObject<dynamic>(rule));
                RuleModelView ruleItem = RuleModelView.RulesModelView(rules).First();
                ruleItem.Status = status;

                if (tableDecision != null)
                {
                    if (ruleItem.Status == "Added")
                    {
                        int ruleId = 0;
                        if (tableDecision.Count != 0)
                        {
                            ruleId = tableDecision.Max(x => x.Rule.RuleId);
                        }

                        ruleItem.Rule.RuleId = ruleId + 1;
                        ruleItem.Rule.Description = $"r_{ruleId + 1}";
                        tableDecision.Add(ruleItem);
                    }
                    else if (ruleItem.Status == "Edited")
                    {
                        int ruleIndex = tableDecision.FindIndex(x => x.Rule.RuleId == ruleItem.Rule.RuleId);
                        RuleModelView lastItem = tableDecision[ruleIndex];

                        if (lastItem.Status == "Added")
                        {
                            ruleItem.Status = lastItem.Status;
                        }
                        tableDecision[ruleIndex] = ruleItem;
                    }
                    else if (ruleItem.Status == "Deleted")
                    {
                        int ruleIndex = tableDecision.FindIndex(x => x.Rule.RuleId == ruleItem.Rule.RuleId);
                        RuleModelView lastItem = tableDecision[ruleIndex];

                        if (lastItem.Status != "Added")
                        {
                            tableDecision[ruleIndex] = ruleItem;
                        }
                        else
                        {
                            tableDecision.RemoveAt(ruleIndex);
                        }
                    }

                    this.TempData["tabla_" + ruleBaseId] = this.ReorderDt(tableDecision);
                    List<RuleModelView> tableDecisionError = this.TempData["Errortabla_" + ruleBaseId] as List<RuleModelView>;
                    this.TempData["Errortabla_" + ruleBaseId] = tableDecisionError;


                    if (tableDecisionError == null || tableDecisionError.Count == 0)
                    {
                        this.TempData["tabla_" + ruleBaseId] = this.ReorderDt(tableDecision);
                        this.TempData["Errortabla_" + ruleBaseId] = null;
                        return new UifJsonResult(true, "");
                    }
                    else
                    {
                        int ruleIndex = tableDecisionError.FindIndex(x => x.Rule.RuleId == ruleItem.Rule.RuleId);
                        if (ruleIndex != -1)
                        {
                            tableDecisionError.RemoveAt(ruleIndex);
                        }
                        if (tableDecisionError.Count != 0)
                        {
                            this.TempData["Errortabla_" + ruleBaseId] = tableDecisionError;
                            return new UifJsonResult(true, "");
                        }
                        else
                        {
                            this.TempData["tabla_" + ruleBaseId] = this.TempData["tabla_" + ruleBaseId];
                            TempData["Errortabla_" + ruleBaseId] = null;
                            return new UifJsonResult(true, "");
                        }
                    }
                }
                else
                {
                    this.TempData["tabla_" + ruleBaseId] = this.TempData["tabla_" + ruleBaseId];
                    return new UifJsonResult(false, "Error cargando tabla de decisión");
                }
            }
            catch (Exception e)
            {
                this.TempData["tabla_" + ruleBaseId] = this.TempData["tabla_" + ruleBaseId];
                return new UifJsonResult(false, Language.UnexpectedError);
            }
        }


        /// <summary>
        /// Realiza el filtro de las reglas
        /// </summary>
        /// <param name="modelViews">lista de reglas a filtrar</param>
        /// <param name="filter">parametro de busqueda</param>
        /// <returns>reglas filtradas</returns>
        private List<RuleModelView> FilterDt(List<RuleModelView> modelViews, List<UifTableParamColumn> columns)
        {
            var filtered = modelViews;
            for (int i = 0; i < columns.Count; i++)
            {
                if (!String.IsNullOrEmpty(columns[i].search.value))
                {
                    if (i < modelViews[0].Conditions.Count)
                    {
                        filtered = filtered.Where(x => x.Conditions[i].ToUpper().Contains(columns[i].search.value.ToUpper())).ToList();
                    }
                    else if (i > modelViews[0].Conditions.Count)
                    {
                        filtered = filtered.Where(x => x.Actions[i - modelViews[0].Conditions.Count - 1].ToUpper().Contains(columns[i].search.value.ToUpper())).ToList();
                    }
                }
            }
            return filtered;
        }

        /// <summary>
        /// Realiza el reordenamiento de las reglas
        /// </summary>
        /// <param name="rules">reglas a ordenar</param>
        /// <returns>reglas ordenadas</returns>
        private List<RuleModelView> ReorderDt(List<RuleModelView> rules)
        {
            if (rules.Count == 0)
            {
                return rules;
            }
            List<MRules._Concept> query = (from t1 in rules[0].Rule.Conditions
                                           select new MRules._Concept
                                           {
                                               ConceptId = t1.Concept.ConceptId,
                                               Entity = new MRules.Entity { EntityId = t1.Concept.Entity.EntityId }
                                           }).ToList();

            rules.Sort(new DecisionTableOrderHelper(query));
            return rules;
        }

        /// <summary>
        /// Exporta tabla de decisiones con error en data
        /// </summary>
        /// <param name="exceptions">excepciones a exportar</param>
        /// <returns>url a descargar archivo</returns>
        public ActionResult GenerateFileToDecisionTableByExceptions(List<string[]> exceptions)
        {
            try
            {
                string urlFile = DelegateService.rulesEditorServices.GenerateFileToDecisionTableByExceptions(exceptions);
                return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
    }
}