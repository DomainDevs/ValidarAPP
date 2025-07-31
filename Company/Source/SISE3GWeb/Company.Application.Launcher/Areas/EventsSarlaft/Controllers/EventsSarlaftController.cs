using Sistran.Core.Framework.UIF.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using EVEDto = Sistran.Company.Application.Event.ApplicationService.DTOs;
using System.IO;
using System.Runtime.InteropServices;
using ExcelInterop = Microsoft.Office.Interop.Excel;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Company.Application.SarlaftApplicationServices.DTO;

namespace Sistran.Core.Framework.UIF.Web.Areas.EventsSarlaft.Controllers
{
    public class EventsSarlaftController : Controller
    {
        //GET: EventsSarlaft/EventsSarlaft
        public ActionResult EventsSarlaft()
        {
            return View();
        }

        public ActionResult GroupEventsAdvanceSearch()
        {
            return PartialView();
        }

        public ActionResult EntitiesAdvanceSearch()
        {
            return PartialView();
        }

        #region GRUPO DE EVENTOS
        public UifJsonResult GetModules()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetModules());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetModule);
            }
        }

        public UifJsonResult GetSubModulesByModuleId(int moduleId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetSubModulesByModuleId(moduleId));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSubModule);
            }
        }

        public UifJsonResult GetSubModules()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetSubModules());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSubModule);
            }
        }

        public UifJsonResult GetEventGroups()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetCompanyEventsGroups());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetEventGroup);
            }
        }

        public UifJsonResult CreateGroupEvent(EVEDto.EventGroupDTO eventGroup)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.CreateGroupEvent(eventGroup));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error CreateGroupEvent");
            }
        }

        public UifJsonResult DeleteGroupEvent(EVEDto.EventGroupDTO eventGroup)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.DeleteGroupEvent(eventGroup));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error CreateGroupEvent");
            }
        }

        public UifJsonResult UpdateGroupEvent(EVEDto.EventGroupDTO eventGroup)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.UpdateGroupEvent(eventGroup));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error CreateGroupEvent");
            }
        }
        #endregion

        #region ENTIDADES
        public JsonResult GetNameEntity(string query)
        {
            try
            {
                return Json(DelegateService.EventApplicationService.GetNameEntityByDescription(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.Error, JsonRequestBehavior.DenyGet);
            }
        }

        public UifJsonResult GetQueryTypesCode()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetQueryTypes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "GetQueryTypesCode");
            }
        }

        public UifJsonResult GetLeveles()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetLevels());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "GetLeveles");
            }
        }

        public UifJsonResult GetEventEntities()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetCompanyEntities());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetEventEntities");
            }
        }

        public JsonResult GetTables(string query)
        {
            try
            {
                List<EVEDto.GenericListDTO> tables;

                if (!query.Contains('.'))
                {
                    tables = DelegateService.EventApplicationService.GetTablesNames(query, "");
                    tables.AddRange(DelegateService.EventApplicationService.GetTablesNames("", query));
                }
                else
                {
                    var str = query.Split('.');
                    string schema = str[0];
                    string table = "";
                    for (int i = 0; i < str[1].Length; i++)
                    {
                        table += str[i];
                    }
                    tables = DelegateService.EventApplicationService.GetTablesNames(schema, table);
                }

                return Json(tables, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.Error, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetStoreProcedures(string query)
        {
            try
            {
                List<EVEDto.GenericListDTO> storedProcedures;

                if (!query.Contains('.'))
                {
                    storedProcedures = DelegateService.EventApplicationService.GetStoreProceduresBySPName(query, "");
                    storedProcedures.AddRange(DelegateService.EventApplicationService.GetStoreProceduresBySPName("", query));
                }
                else
                {
                    var str = query.Split('.');
                    string schema = str[0];
                    string table = "";
                    for (int i = 0; i < str[1].Length; i++)
                    {
                        table += str[i];
                    }
                    storedProcedures = DelegateService.EventApplicationService.GetStoreProceduresBySPName(schema, table);
                }

                return Json(storedProcedures, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.Error, JsonRequestBehavior.DenyGet);
            }
        }

        public UifJsonResult GetValidationTypes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetValidationTypes());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetValidationTypes");
            }
        }

        public UifJsonResult GetDataTypes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetDataTypes());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetDataTypes");
            }
        }

        public UifJsonResult GetCodeTableByTable(int tableId, string tableName, string column)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetColumnsTableByTableIdTableNameColumn(tableId, tableName, column));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetCodeTableByTable");
            }
        }

        public UifJsonResult DeleteEntity(int entityId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.DeleteEntity(entityId));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetDataTypes");
            }
        }

        public UifJsonResult CreateEntity(EVEDto.EntityDTO entity)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.CreateEntity(entity));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetDataTypes");
            }
        }

        #endregion

        #region GRUPO DE CONDICIONES
        public UifJsonResult GetConditionsGroups()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetConditionGroups());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetConditionsGroups");
            }
        }

        public UifJsonResult GetDependencesByConditionId(int conditionId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetDependenciesByConditionId(conditionId));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetConditionsGroups");
            }
        }

        public UifJsonResult GetEntitiesByConditionsGroupId(int conditionsGroupId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetEntitiesByConditionGroupId(conditionsGroupId));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetEntitiesByConditionsGroupId");
            }

        }

        public JsonResult CreateCondition(EVEDto.ConditionGroupDTO conditionGroup)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.CreateConditionGroup(conditionGroup));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "CreateCondition");
            }
        }

        public JsonResult CreateAssignEntity(EVEDto.EventConditionDTO eventCondition)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.CreateAssignEntity(eventCondition));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "CreateAssignEntity");
            }
        }

        public JsonResult DeleteCondition(int conditionId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.DeleteCondition(conditionId));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "DeleteCondition");
            }
        }

        public UifJsonResult UpdateDependencies(EVEDto.DependenciesDTO dependencies)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.UpdateDependencies(dependencies));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "CreateAssignEntity");
            }
        }

        public UifJsonResult CreateDependencies(EVEDto.DependenciesDTO dependencies)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.CreateDependencies(dependencies));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "CreateAssignEntity");
            }
        }

        public UifJsonResult DeleteDependencies(EVEDto.DependenciesDTO dependencies)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.DeleteDependencies(dependencies));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "CreateAssignEntity");
            }
        }
        #endregion

        #region EVENTOS
        public UifJsonResult GetPrefixes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.commonService.GetPrefixes());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetPrefixes");
            }
        }

        public UifJsonResult GetEventsByEventIdStateIdPrefixId(int groupEventId, int statusEventId, int prefixId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetEventsByEventIdStateIdPrefixId(groupEventId, statusEventId, prefixId));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetEventsByIdEventGroupStateIdPrefix");
            }
        }

        public UifJsonResult GetAccesses()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetAccess());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetEventsByIdEventGroupStateIdPrefix");
            }
        }

        public UifJsonResult GetPrefixesByGroupIdEventId(int groupId, int eventId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetPrefixesByGroupIdEventId(groupId, eventId));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetPrefixesByGroupIdEventId");
            }
        }

        public UifJsonResult GetAccessesByEventIdGroupId(int groupId, int eventId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetAccessByEventIdGroupId(groupId, eventId));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetAccessesByEventIdGroupId");
            }
        }

        public UifJsonResult GetDelegationsByGroupIdEventId(int groupId, int eventId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetDelegationsByGroupIdEventId(groupId, eventId));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetDelegationsByGroupIdEventId");
            }
        }

        public UifJsonResult GetDelegationUsersByGroupIdEventIdHierarchyId(int groupId, int eventId, int hierarchyId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetDelegationUsersByGroupIdEventIdHierarchyId(groupId, eventId, hierarchyId));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetDelegationUsersByGroupIdEventIdHierarchyId");
            }
        }

        public UifJsonResult GetConditionsByGroupIdEventIdHierarchyId(int groupId, int eventId, int hierarchyId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetConditionsByGroupIdEventIdHierarchyId(groupId, eventId, hierarchyId));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetConditionsByGroupIdEventIdHierarchyId");
            }
        }

        public PartialViewResult GetEventConditionsViewByGroupIdEventIdHierarchyId(int groupId, int eventId, int hierarchyId)
        {
            try
            {
                //var ListEntitiesCondition = DelegateService.EventApplicationService.GetEventConditionsByGroupIdEventIdHierarchyId(groupId, eventId, hierarchyId);
                var evento = DelegateService.eventsService.GetEventByIdEventGroupIdEvent(groupId, eventId);
                var ListEntitiesCondition = DelegateService.eventsService.GetEntitiesByIdConditionsGroup(evento.EventConditionGroup.ConditionId).OrderBy(x => x.Description).Select(x => x.Description).ToList();
                ViewBag.IdGroup = groupId;
                ViewBag.IdEvent = eventId;
                ViewBag.IdHierarchy = hierarchyId;

                ViewBag.ListEntitiesCondition = ListEntitiesCondition;

                return PartialView("~/Areas/EventsSarlaft/Views/EventsSarlaftModal/ConditionsSeccion.cshtml");
            }
            catch (System.Exception)
            {
                return PartialView("~/Areas/EventsSarlaft/Views/EventsSarlaftModal/ConditionsSeccion.cshtml");
            }
        }

        public ActionResult GetEventConditionsByGroupIdEventIdHierarchyId(int groupId, int eventId, int hierarchyId)
        {
            try
            {
                var list = DelegateService.eventsService.GetEventConditionsByIdGroupIdEventIdHierarchy(groupId, eventId, hierarchyId).ToArray().ToList();

                var evento = DelegateService.eventsService.GetEventByIdEventGroupIdEvent(groupId, eventId);
                var cabeceras = DelegateService.eventsService.GetEntitiesByIdConditionsGroup(evento.EventConditionGroup.ConditionId).OrderBy(x => x.Description).Select(x => x.Description).ToList();

                string str = "";
                str += "[";
                for (int y = 0; y < list.Count(); y++)
                {
                    object[] item = (object[])list[y];
                    if (item.Length == cabeceras.Count() + 1)
                    {
                        str += "{";
                        for (int i = 0; i < item.Length; i++)
                        {
                            if (i == 0)
                                str += " \"IdCondition\" : \" " + item[i] + " \" ";
                            else
                                str += " \"" + cabeceras[i - 1] + "\" : \" " + item[i] + " \"";

                            if (i != item.Length - 1)
                                str += ",";
                        }
                        str += "},";
                        if (y == list.Count() - 1)
                            str = str.Substring(0, str.Length - 1);
                    }
                }
                str += "]";

                return Content(str, "application/json");
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "GetEventConditionsByGroupIdEventIdHierarchyId");
            }



        }

        public UifJsonResult GetOperatorConditionByEntityIdQueryTypeId(int entityId, int queryTypeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetOperatorConditionByEntityIdQueryTypeId(entityId, queryTypeId));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetOperatorConditionByEntityIdQueryTypeId");
            }
        }

        public UifJsonResult GetValuesByGroupIdEntityIdEventIdOperatorId(int groupId, int entityId, int eventId, List<EVEDto.GenericListDTO> anotherDependences)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.GetValuesByGroupIdEntityIdEventIdOperatorId(groupId, entityId, eventId, anotherDependences));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetValuesByGroupIdEntityIdEventIdOperatorId");
            }
        }

        public UifJsonResult CreateEvent(EVEDto.EventDTO events, List<EVEDto.GenericListDTO> prefixes, List<EVEDto.GenericListDTO> executions, List<EVEDto.GenericListDTO> rejectCauses)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.CreateCompanyEvent(events, prefixes, executions, rejectCauses));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "GetOperatorConditionByEntityIdQueryTypeId");
            }
        }

        public UifJsonResult UpdateEvent(EVEDto.EventDTO events, List<EVEDto.GenericListDTO> prefixes, List<EVEDto.GenericListDTO> executions, List<EVEDto.GenericListDTO> rejectCauses)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.UpdateCompanyEvent(events, prefixes, executions, rejectCauses));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "UpdateEvent");
            }
        }

        public UifJsonResult DeleteEvent(int groupEventId, int eventId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.DeleteEvent(groupEventId, eventId));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "DeleteEvent");
            }
        }

        public UifJsonResult UpdateDelegationUser(EVEDto.DelegationUserDTO user, int groupId, int eventId, int hierarchyId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.UpdateDelegationUser(user, groupId, eventId, hierarchyId));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "DeleteEvent");
            }
        }

        public UifJsonResult DeleteConditionEntity(EVEDto.ConditionDTO condition)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EventApplicationService.DeleteConditionEntity(condition));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, "DeleteConditionEntity");
            }
        }
        #endregion

        #region EXPORTAR DATA
        public UifJsonResult ExportData(String data, int typeFile)
        {
            ExcelInterop.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetFiles);
            }

            try
            {
                ExcelInterop.Workbook xlWorkBook;
                ExcelInterop.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (ExcelInterop.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet = gerateWorksheet(typeFile, xlWorkSheet, data);

                String externalDirectoryPath = DelegateService.commonService.GetKeyApplication("ExternalFolderFiles") + @"\" + SessionHelper.GetUserName() + @"\";

                if (!System.IO.Directory.Exists(externalDirectoryPath))
                {
                    System.IO.Directory.CreateDirectory(externalDirectoryPath);
                }

                string file = externalDirectoryPath + xlWorkSheet.Name + ".xls";

                if (!System.IO.Directory.Exists(file))
                {
                    System.IO.File.Delete(file);
                }

                xlWorkBook.SaveAs(file, ExcelInterop.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, ExcelInterop.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);

                return new UifJsonResult(true, "http:" + file);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGenerateProcess);
            }
        }

        private ExcelInterop.Worksheet gerateWorksheet(int typeFile, ExcelInterop.Worksheet xlWorkSheet, string serializedData)
        {
            int position = 2;
            switch (typeFile)
            {
                case 1:
                    xlWorkSheet.Name = "GRUPO_DE_EVENTOS_" + DateTime.Now.ToString("dd_MM_yyyy");
                    xlWorkSheet.Cells[1, 1] = "ID GRUPO DE EVENTOS";
                    xlWorkSheet.Cells[1, 2] = "DESCRIPCION";
                    xlWorkSheet.Cells[1, 3] = "ID MODULO";
                    xlWorkSheet.Cells[1, 4] = "DESCRIPCION MODULO";
                    xlWorkSheet.Cells[1, 5] = "ID SUBMODULO";
                    xlWorkSheet.Cells[1, 6] = "DESCRIPCION SUBMODULO";
                    xlWorkSheet.Cells[1, 7] = "HABILITADO";

                    List<EVEDto.EventGroupDTO> EventGroups = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EVEDto.EventGroupDTO>>(serializedData);

                    foreach (EVEDto.EventGroupDTO eventGroup in EventGroups)
                    {
                        xlWorkSheet.Cells[position, 1] = eventGroup.Id;
                        xlWorkSheet.Cells[position, 2] = eventGroup.GroupEventDescription;
                        xlWorkSheet.Cells[position, 3] = eventGroup.ModuleId;
                        xlWorkSheet.Cells[position, 4] = eventGroup.NameModule;
                        xlWorkSheet.Cells[position, 5] = eventGroup.SubmoduleId;
                        xlWorkSheet.Cells[position, 6] = eventGroup.NameSubmodule;
                        xlWorkSheet.Cells[position, 7] = eventGroup.Enabled;
                        position++;
                    }
                    break;
                case 2:
                    xlWorkSheet.Name = "ENTIDADES_" + DateTime.Now.ToString("dd_MM_yyyy");
                    xlWorkSheet.Cells[1, 1] = "ID ENTIDAD";
                    xlWorkSheet.Cells[1, 2] = "DESCRIPCION";
                    xlWorkSheet.Cells[1, 3] = "TIPO DE SENTENCIA";
                    xlWorkSheet.Cells[1, 4] = "TABLA ORIGEN";
                    xlWorkSheet.Cells[1, 5] = "CAMPO FUENTE";
                    xlWorkSheet.Cells[1, 6] = "DESCRIPCION ORIGEN";
                    xlWorkSheet.Cells[1, 7] = "TABLA JOIN";
                    xlWorkSheet.Cells[1, 8] = "CAMPO ORIGEN JOIN";
                    xlWorkSheet.Cells[1, 9] = "CAMPO DESTINO JOIN";
                    xlWorkSheet.Cells[1, 10] = "PARAMETRO WHERE";
                    xlWorkSheet.Cells[1, 11] = "NIVEL";
                    xlWorkSheet.Cells[1, 12] = "TIPO DE VALIDACION";
                    xlWorkSheet.Cells[1, 13] = "PROCEDIMIENTO DE VALIDACION";
                    xlWorkSheet.Cells[1, 14] = "TABLA DE VALIDACION";
                    xlWorkSheet.Cells[1, 15] = "CLAVE DE VALIDACION";
                    xlWorkSheet.Cells[1, 16] = "TIPO DE CLAVE DE VALIDACION";
                    xlWorkSheet.Cells[1, 17] = "CLAVE 1";
                    xlWorkSheet.Cells[1, 18] = "CLAVE 2";
                    xlWorkSheet.Cells[1, 19] = "CLAVE 3";
                    xlWorkSheet.Cells[1, 20] = "CLAVE 4";
                    xlWorkSheet.Cells[1, 21] = "CAMPO DE VALIDACIONCLAVE 1";
                    xlWorkSheet.Cells[1, 22] = "TIPO CAMPO DE VALIDACION";
                    xlWorkSheet.Cells[1, 23] = "WHERE DEL CAMPO DE VALIDACION";

                    List<EVEDto.EntityDTO> entities = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EVEDto.EntityDTO>>(serializedData);

                    foreach (EVEDto.EntityDTO entity in entities)
                    {
                        xlWorkSheet.Cells[position, 1] = entity.Id;
                        xlWorkSheet.Cells[position, 2] = entity.EntityDescription;
                        xlWorkSheet.Cells[position, 3] = "lista";
                        xlWorkSheet.Cells[position, 4] = entity.SourceTable;
                        xlWorkSheet.Cells[position, 5] = entity.SourceCode;
                        xlWorkSheet.Cells[position, 6] = entity.SourceDescription;
                        xlWorkSheet.Cells[position, 7] = entity.JoinTable;
                        xlWorkSheet.Cells[position, 8] = entity.JoinSourceField;
                        xlWorkSheet.Cells[position, 9] = entity.JoinTargetField;
                        xlWorkSheet.Cells[position, 10] = entity.ConditionJoinWhere;
                        xlWorkSheet.Cells[position, 11] = entity.ValidationTypeDescription;
                        xlWorkSheet.Cells[position, 12] = entity.ValidationKeyField;
                        xlWorkSheet.Cells[position, 13] = entity.Key1Field;
                        xlWorkSheet.Cells[position, 14] = entity.Key2Field;
                        xlWorkSheet.Cells[position, 15] = entity.Key3Field;
                        xlWorkSheet.Cells[position, 16] = entity.Key4Field;
                        xlWorkSheet.Cells[position, 17] = entity.ValidationField;
                        xlWorkSheet.Cells[position, 18] = "tipo campo de validacion";
                        xlWorkSheet.Cells[position, 19] = entity.ConditionWhere;
                        position++;
                    }
                    break;
                case 3:
                    xlWorkSheet.Name = "GRUPO_DE_CONDICIONES_" + DateTime.Now.ToString("dd_MM_yyyy");
                    xlWorkSheet.Cells[1, 1] = "ID GRUPO";
                    xlWorkSheet.Cells[1, 2] = "DESCRIPCION";
                    xlWorkSheet.Cells[1, 3] = "ENTIDADES";

                    List<EVEDto.ConditionGroupDTO> conditionGroups = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EVEDto.ConditionGroupDTO>>(serializedData);

                    foreach (EVEDto.ConditionGroupDTO conditionGroup in conditionGroups)
                    {
                        xlWorkSheet.Cells[position, 1] = conditionGroup.Id;
                        xlWorkSheet.Cells[position, 2] = conditionGroup.Description;
                        xlWorkSheet.Cells[position, 3] = conditionGroup.RelatedEntities;
                        position++;
                    }
                    break;
                case 4:
                    xlWorkSheet.Name = "EVENTOS_" + DateTime.Now.ToString("dd_MM_yyyy");
                    xlWorkSheet.Cells[1, 1] = "ID GRUPO";
                    xlWorkSheet.Cells[1, 2] = "DESCRIPCION";
                    xlWorkSheet.Cells[1, 3] = "EVENTO";
                    xlWorkSheet.Cells[1, 4] = "ESTADO";

                    List<EVEDto.EventDTO> events = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EVEDto.EventDTO>>(serializedData);

                    foreach (EVEDto.EventDTO eventDTO in events)
                    {
                        xlWorkSheet.Cells[position, 1] = eventDTO.Id;
                        xlWorkSheet.Cells[position, 2] = eventDTO.Description;
                        xlWorkSheet.Cells[position, 3] = eventDTO.GroupEventDescription;
                        xlWorkSheet.Cells[position, 4] = eventDTO.Enabled == true ? "SI" : "NO";
                        position++;
                    }
                    break;
                case 5:

                    break;
                default:
                    break;
            }

            return xlWorkSheet;
        }
        #endregion

    }
}