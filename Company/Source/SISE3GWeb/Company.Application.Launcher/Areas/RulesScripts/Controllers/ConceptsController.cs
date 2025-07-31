using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.RulesScripts.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using RulesScriptsEnum = Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Newtonsoft.Json;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Sistran.Core.Framework.UIF.Web.Areas.RulesScripts.Controllers
{
    using System.Text;
    using Application.ModelServices.Enums;
    using Application.ModelServices.Models.Param;
    using WebGrease.Css.Extensions;

    [Authorize]
    public class ConceptsController : Controller
    {

        #region Concepts
        #region ViewResult
        /// <summary>
        /// pantalla inicial de conceptos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// retorna la vista parcial para la busqueda avanzada de conceptos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult AdvancedSearchConcepts()
        {
            return View();
        }
        [HttpGet]
        public ViewResult RangeEntityAdvancedSearch()
        {
            return View();
        }

        /// <summary>
        /// pantalla que lista los conceptos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult ListConcepts()
        {
            return PartialView();
        }

        /// <summary>
        /// pantalla que agrega-edita los conceptos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult AddEditConcept(ConceptViewModel concept)
        {
            if (concept.Id == 0)
            {
                concept.IsNull = true;
                concept.IsPersistible = true;
                concept.IsVisible = true;
            }

            return PartialView(concept);
        }

        /// <summary>
        /// pantalla incial de lista de valores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult ListEntity()
        {
            return View();
        }

        /// <summary>
        /// pantalla inicial de lista de rangos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult RangeEntity()
        {
            return View();
        }

        /// <summary>
        /// pantalla que ingresa los valores de las listas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult ListEntityValueAdd()
        {
            return PartialView();
        }

        /// <summary>
        /// pantalla que ingresa los valores de los rangos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult RangeEntityValueAdd()
        {
            return PartialView();
        }
        #endregion

        #region JsonResult
        /// <summary>
        /// obtiene los conceptos a partir del modulo y el nivel, filtra dependiento (estaticos, dinamicos,todos)
        /// </summary>
        /// <param name="IdModule">id del modulo</param>
        /// <param name="IdLevel">id del nivel</param>
        /// <param name="Filter">fitro
        ///     <value val=0>todos</value>
        ///     <value val=1>estaticos</value>
        ///     <value val=2>dinamicos</value>
        /// </param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetConceptsByIdModuleIdLevelDescription(int? IdModule, int? IdLevel, int Filter, string Description)
        {
            var listConcepts = DelegateService.conceptsService.GetConceptsByIdModuleIdLevelDescription(IdModule, IdLevel, Filter, Description);

            List<ConceptViewModel> listResult = new List<ConceptViewModel>();
            foreach (var item in listConcepts)
            {
                listResult.Add(ConceptViewModel.ConceptToViewModel(item));
            }
            if (IdModule == null)
            {
                return Json(listResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(listResult.Take(50), JsonRequestBehavior.AllowGet);
            }
            
        }

        /// <summary>
        /// Obtiene el autocomplete para la busqueda avanzada de conceptos
        /// </summary>
        /// <param name="query">query de busqueda</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetConceptsAutocomplete(string query)
        {
            List<Concept> listConcepts = DelegateService.conceptsService.GetConcepts();
            List<ConceptViewModel> listResult = new List<ConceptViewModel>();

            foreach (var item in listConcepts.Where(x => x.Description.ToLower().Contains(query.ToLower())).ToList())
            {
                listResult.Add(ConceptViewModel.ConceptToViewModel(item));
            }

            return Json(listResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// obtiene los tipos de conceptos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetConceptTypes()
        {
            var list = DelegateService.conceptsService.GetConceptTypes();
            return new UifSelectResult(list);
        }

        /// <summary>
        /// Guarda los cambios realizados para los conceptos
        /// </summary>
        /// <param name="ConceptBasicAdd">lista de conceptos basicos a crear</param>
        /// <param name="ConceptBasicEdit">lista de conceptos basicos a editar</param>
        /// <param name="ConceptBasicDelete">lista de conceptos basicos a eliminar</param>
        /// <param name="ConceptListAdd">lista de conceptos lista a crear</param>
        /// <param name="ConceptListEdit">lista de conceptos lista a editar</param>
        /// <param name="ConceptListDelete">lista de conceptos lista a eliminar</param>
        /// <param name="ConceptRangeAdd">lista de conceptos rango a crear</param>
        /// <param name="ConceptRangeEdit">lista de conceptos rango a editar</param>
        /// <param name="ConceptRangeDelete">lista de conceptos rango a eliminar</param>
        /// <param name="ConceptReferenceAdd">lista de conceptos referencia a crear</param>
        /// <param name="ConceptReferenceEdit">lista de conceptos referencia a editar</param>
        /// <param name="ConceptReferenceDelete">lista de conceptos referencia a eliminar</param>
        /// <returns></returns>
        public JsonResult SaveConcepts(
               string ConceptBasicAdd, string ConceptBasicEdit, string ConceptBasicDelete,
               string ConceptListAdd, string ConceptListEdit, string ConceptListDelete,
               string ConceptRangeAdd, string ConceptRangeEdit, string ConceptRangeDelete,
               string ConceptReferenceAdd, string ConceptReferenceEdit, string ConceptReferenceDelete)
        {
            try
            {

                var listConceptBasicAdd = JsonConvert.DeserializeObject<List<ConceptBasicViewModel>>(ConceptBasicAdd);
                var listConceptBasicEdit = JsonConvert.DeserializeObject<List<ConceptBasicViewModel>>(ConceptBasicEdit);
                var listConceptBasicDelete = JsonConvert.DeserializeObject<List<ConceptBasicViewModel>>(ConceptBasicDelete);

                var listConceptListAdd = JsonConvert.DeserializeObject<List<ConceptListViewModel>>(ConceptListAdd);
                var listConceptListEdit = JsonConvert.DeserializeObject<List<ConceptListViewModel>>(ConceptListEdit);
                var listConceptListDelete = JsonConvert.DeserializeObject<List<ConceptListViewModel>>(ConceptListDelete);

                var listConceptRangeAdd = JsonConvert.DeserializeObject<List<ConceptRangeViewModel>>(ConceptRangeAdd);
                var listConceptRangeEdit = JsonConvert.DeserializeObject<List<ConceptRangeViewModel>>(ConceptRangeEdit);
                var listConceptRangeDelete = JsonConvert.DeserializeObject<List<ConceptRangeViewModel>>(ConceptRangeDelete);

                var listConceptReferenceAdd = JsonConvert.DeserializeObject<List<ConceptReferenceViewModel>>(ConceptReferenceAdd);
                var listConceptReferenceEdit = JsonConvert.DeserializeObject<List<ConceptReferenceViewModel>>(ConceptReferenceEdit);
                var listConceptReferenceDelete = JsonConvert.DeserializeObject<List<ConceptReferenceViewModel>>(ConceptReferenceDelete);


                listConceptBasicAdd = AssignBasicType(listConceptBasicAdd);
                listConceptBasicEdit = AssignBasicType(listConceptBasicEdit);
                listConceptBasicDelete = AssignBasicType(listConceptBasicDelete);

                listConceptListAdd = AssignListType(listConceptListAdd);
                listConceptListEdit = AssignListType(listConceptListEdit);
                listConceptListDelete = AssignListType(listConceptListDelete);

                listConceptRangeAdd = AssignRangeType(listConceptRangeAdd);
                listConceptRangeEdit = AssignRangeType(listConceptRangeEdit);
                listConceptRangeDelete = AssignRangeType(listConceptRangeDelete);

                listConceptReferenceAdd = AssignReferenceType(listConceptReferenceAdd);
                listConceptReferenceEdit = AssignReferenceType(listConceptReferenceEdit);
                listConceptReferenceDelete = AssignReferenceType(listConceptReferenceDelete);


                List<BasicConcept> basicConceptsAdd = new List<BasicConcept>();
                foreach (var item in listConceptBasicAdd)
                {
                    basicConceptsAdd.Add((BasicConcept)ConceptViewModel.ConceptToModel(item));
                }

                var basicConceptsEdit = new List<BasicConcept>();
                foreach (var item in listConceptBasicEdit)
                {
                    basicConceptsEdit.Add((BasicConcept)ConceptViewModel.ConceptToModel(item));
                }

                List<ListConcept> listConceptsAdd = new List<ListConcept>();
                foreach (var item in listConceptListAdd)
                {
                    listConceptsAdd.Add((ListConcept)ConceptViewModel.ConceptToModel(item));
                }

                var listConceptsEdit = new List<ListConcept>();
                foreach (var item in listConceptListEdit)
                {
                    listConceptsEdit.Add((ListConcept)ConceptViewModel.ConceptToModel(item));
                }

                List<RangeConcept> rangeConceptadd = new List<RangeConcept>();
                foreach (var item in listConceptRangeAdd)
                {
                    rangeConceptadd.Add((RangeConcept)ConceptViewModel.ConceptToModel(item));
                }


                var rangeConceptEdit = new List<RangeConcept>();
                foreach (var item in listConceptRangeEdit)
                {
                    rangeConceptEdit.Add((RangeConcept)ConceptViewModel.ConceptToModel(item));
                }

                List<ReferenceConcept> ReferenceConceptadd = new List<ReferenceConcept>();
                foreach (var item in listConceptReferenceAdd)
                {
                    ReferenceConceptadd.Add((ReferenceConcept)ConceptViewModel.ConceptToModel(item));
                }


                var ReferenceConceptEdit = new List<ReferenceConcept>();
                foreach (var item in listConceptReferenceEdit)
                {
                    ReferenceConceptEdit.Add((ReferenceConcept)ConceptViewModel.ConceptToModel(item));
                }


                List<BasicConcept> conceptsbasicDelete = new List<BasicConcept>();
                foreach (var item in listConceptBasicDelete)
                {
                    conceptsbasicDelete.Add((BasicConcept)ConceptViewModel.ConceptToModel(item));
                }

                List<ListConcept> conceptsListDelete = new List<ListConcept>();
                foreach (var item in listConceptListDelete)
                {
                    conceptsListDelete.Add((ListConcept)ConceptViewModel.ConceptToModel(item));
                }

                List<RangeConcept> conceptsRangeDelete = new List<RangeConcept>();
                foreach (var item in listConceptRangeDelete)
                {
                    conceptsRangeDelete.Add((RangeConcept)ConceptViewModel.ConceptToModel(item));
                }

                List<ReferenceConcept> conceptsReferenceDelete = new List<ReferenceConcept>();
                foreach (var item in listConceptReferenceDelete)
                {
                    conceptsReferenceDelete.Add((ReferenceConcept)ConceptViewModel.ConceptToModel(item));
                }

                DelegateService.conceptsService.SaveConcepts(basicConceptsAdd, basicConceptsEdit, conceptsbasicDelete,
                   listConceptsAdd, listConceptsEdit, conceptsListDelete,
                   rangeConceptadd, rangeConceptEdit, conceptsRangeDelete,
                   ReferenceConceptadd, ReferenceConceptEdit, conceptsReferenceDelete);


                return new UifJsonResult(true, App_GlobalResources.Language.MessageSavedSuccessfully);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// asigna los type Enum a los concetos basicos
        /// </summary>
        /// <param name="listConceptBasic">lsita de conceptos basicos</param>
        /// <returns></returns>
        private List<ConceptBasicViewModel> AssignBasicType(List<ConceptBasicViewModel> listConceptBasic)
        {

            foreach (ConceptBasicViewModel concept in listConceptBasic)
            {
                if (concept.EntityId == 0)
                {
                    var entity = DelegateService.conceptsService.GetEntityByIdPackageIdLevel(concept.Module, concept.Level);
                    concept.EntityId = entity.EntityId;
                }
                switch (concept.BasicTypeCode)
                {
                    case RulesScriptsEnum.BasicType.Date:
                        concept.ConceptControlCode = RulesScriptsEnum.ConceptControlType.DateEditor;
                        break;
                    case RulesScriptsEnum.BasicType.Numeric:
                    case RulesScriptsEnum.BasicType.Decimal:
                        concept.ConceptControlCode = RulesScriptsEnum.ConceptControlType.NumberEditor;
                        break;
                    case RulesScriptsEnum.BasicType.Text:
                        concept.ConceptControlCode = RulesScriptsEnum.ConceptControlType.TextBox;
                        break;
                }
            }

            return listConceptBasic;
        }

        /// <summary>
        /// asigna los type Enum a los concetos lista
        /// </summary>
        /// <param name="listConceptList">lista de conceptos lista</param>
        /// <returns></returns>
        private List<ConceptListViewModel> AssignListType(List<ConceptListViewModel> listConceptList)
        {
            foreach (ConceptListViewModel concept in listConceptList)
            {
                if (concept.EntityId == 0)
                {
                    var entity = DelegateService.conceptsService.GetEntityByIdPackageIdLevel(concept.Module, concept.Level);
                    concept.EntityId = entity.EntityId;
                }

                concept.ConceptControlCode = RulesScriptsEnum.ConceptControlType.ListBox;
            }

            return listConceptList;
        }

        /// <summary>
        /// asigna los type Enum a los concetos rango
        /// </summary>
        /// <param name="listConceptRange">lista de conceptos rango</param>
        /// <returns></returns>
        private List<ConceptRangeViewModel> AssignRangeType(List<ConceptRangeViewModel> listConceptRange)
        {
            foreach (ConceptRangeViewModel concept in listConceptRange)
            {
                if (concept.EntityId == 0)
                {
                    var entity = DelegateService.conceptsService.GetEntityByIdPackageIdLevel(concept.Module, concept.Level);
                    concept.EntityId = entity.EntityId;
                }
                concept.ConceptControlCode = RulesScriptsEnum.ConceptControlType.ListBox;
            }

            return listConceptRange;
        }

        /// <summary>
        /// asigna los type Enum a los concetos referencia
        /// </summary>
        /// <param name="listConceptReference">lista de conceptos referencia</param>
        /// <returns></returns>
        private List<ConceptReferenceViewModel> AssignReferenceType(List<ConceptReferenceViewModel> listConceptReference)
        {
            foreach (ConceptReferenceViewModel concept in listConceptReference)
            {
                if (concept.EntityId == 0)
                {
                    var entity = DelegateService.conceptsService.GetEntityByIdPackageIdLevel(concept.Module, concept.Level);
                    concept.EntityId = entity.EntityId;
                }
                concept.ConceptControlCode = RulesScriptsEnum.ConceptControlType.SearchCombo;
            }

            return listConceptReference;
        }

        /// <summary>
        /// obtiene los tipos de conceptos basicos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetBasicTypes()
        {
            try
            {
                var list = DelegateService.conceptsService.GetBasicTypes();
                return new UifSelectResult(list);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// obtiene los valores para los tipos de conceptos basicos
        /// </summary>
        /// <param name="conceptSrt">concepto basico serializado</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetBasicConceptsValues(string conceptSrt)
        {
            try
            {
                var basicConcept = JsonConvert.DeserializeObject<BasicConcept>(conceptSrt);
                var conceptResult = DelegateService.conceptsService.GetBasicConceptsValues(basicConcept);
                if (conceptResult != null)
                {
                    var concept = ConceptViewModel.ConceptToViewModel(conceptResult);
                    return new UifJsonResult(true, concept);
                }
                return new UifJsonResult(true, conceptResult);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// valida si el concepto esta siendo usado 
        /// </summary>
        /// <param name="conceptSrt">concepto serializado</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult IsInUse(string conceptSrt)
        {
            try
            {
                var concept = JsonConvert.DeserializeObject<ConceptViewModel>(conceptSrt);
                var IsInUse = DelegateService.conceptsService.IsInUse(ConceptViewModel.ConceptToModel(concept));
                return new UifJsonResult(IsInUse, App_GlobalResources.Language.NoCanEditDeleteConceptUse);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// Otiene la entidad por IdEntity
        /// </summary>
        /// <param name="EntityId">Id de la entidad</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetEntity(int EntityId)
        {
            try
            {
                var Entity = DelegateService.conceptsService.GetEntity(EntityId);
                return new UifJsonResult(true, new { ModuleId = Entity.PackageId, LevelId = Entity.LevelId });
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// Descarga un excel que contiene todos los grupos de condiciones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FileConcepts()
        {
            try
            {
                string urlFile = DelegateService.conceptsService.ExportConcepts();

                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
        #endregion

        [HttpGet]
        public ActionResult ListEntityAdvancedSearch()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el Concepto por descripción
        /// </summary>
        /// <param name="description">Description de Concepto</param>
        /// <returns>Concepto</returns>
        public ActionResult GetConceptsByDescription(string description)
        {
            try
            {
                bool? respuesta = DelegateService.conceptsService.GetConceptsByDescription(description);

                if (respuesta == true)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.RepeatedConcept);
                }
                else if (respuesta == false)
                {
                    return new UifJsonResult(true, "");
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProcess);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProcess);
            }
        }

        #endregion

        #region ListEntity
        /// <summary>
        /// Obtener todas las listas de valores.
        /// </summary>
        /// <returns>lista de listas de valores</returns>
        public ActionResult GetListEntity()
        {
            try
            {
                List<ListEntity> listEntity = DelegateService.conceptsService.GetListEntity();
                List<ListEntityViewModel> listEntityResult = new List<ListEntityViewModel>();

                foreach (ListEntity item in listEntity)
                {
                    listEntityResult.Add(ListEntityViewModel.ListEntityToViewModel(item));
                }

                return new UifJsonResult(true, listEntityResult.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrMsgListEntityNotFound);
            }
        }

        [HttpPost]
        public JsonResult SaveListEntity(List<ListEntity> listEntities)
        {
            try
            {
                listEntities = DelegateService.conceptsService.ExecuteOperationListEntities(listEntities);

                int totalAdded = listEntities.Count(x => x.StatusTypeService == StatusTypeService.Create);
                int totalModified = listEntities.Count(x => x.StatusTypeService == StatusTypeService.Update);
                int totalDeleted = listEntities.Count(x => x.StatusTypeService == StatusTypeService.Delete);

                StringBuilder errorMessage = new StringBuilder();
                listEntities.Where(x => x.StatusTypeService == StatusTypeService.Error).ForEach(x => x.ErrorServiceModel.ErrorDescription.ForEach(y => errorMessage.Append("</br>" + y)));

                return new UifJsonResult(true, new ParametrizationResult { TotalAdded = totalAdded, TotalDeleted = totalDeleted, TotalModified = totalModified, Message = errorMessage.ToString() });
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        /// Obtener todas las listas de valores.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de listas de valores</returns>
        public ActionResult GetListEntitySelect()
        {
            try
            {
                List<ListEntity> listEntity = DelegateService.conceptsService.GetListEntity();
                List<ListEntityViewModel> listEntityResult = new List<ListEntityViewModel>();

                foreach (ListEntity item in listEntity)
                {
                    listEntityResult.Add(ListEntityViewModel.ListEntityToViewModel(item));
                }

                return new UifSelectResult(listEntityResult);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// Obtener las listas de valores que coinciden con la descripción.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de listas de valores</returns>
        public ActionResult GetListEntityByDescription(string Description)
        {
            try
            {
                List<ListEntity> listEntity = DelegateService.conceptsService.GetListEntityByDescription(Description);
                List<ListEntityViewModel> listEntityResult = new List<ListEntityViewModel>();

                foreach (ListEntity item in listEntity)
                {
                    listEntityResult.Add(ListEntityViewModel.ListEntityToViewModel(item));
                }

                if (listEntityResult.Count != 0)
                    return Json(listEntityResult.OrderBy(x => x.Description).ToList(), JsonRequestBehavior.AllowGet);

                return new UifJsonResult(false, App_GlobalResources.Language.ErrMsgListEntityNotFound);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// Obtener los valores de lista por código de lista de valores.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de listas de valores</returns>
        public ActionResult GetListEntityValueByListEntityCode(int listEntityCode)
        {
            try
            {
                List<ListEntity> listEntity = DelegateService.conceptsService.GetListEntityValueByListEntityCode(listEntityCode);
                List<ListEntityViewModel> listEntityResult = new List<ListEntityViewModel>();

                foreach (ListEntity item in listEntity)
                {
                    listEntityResult.Add(ListEntityViewModel.ListEntityToViewModel(item));
                }

                if (listEntityResult.Count != 0)
                    return Json(listEntityResult.OrderBy(x => x.Description).ToList(), JsonRequestBehavior.AllowGet);

                return new UifJsonResult(false, App_GlobalResources.Language.ErrMsgListEntityNotFound);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
        #endregion

        #region RangeEntity
        /// </summary>
        /// Obtener todas las rangos de valores.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public ActionResult GetRangeEntity()
        {
            try
            {
                List<RangeEntity> rangeEntity = DelegateService.conceptsService.GetRangeEntity();
                List<RangeEntityViewModel> rangeEntityResult = new List<RangeEntityViewModel>();

                foreach (RangeEntity item in rangeEntity)
                {
                    rangeEntityResult.Add(RangeEntityViewModel.RangeEntityToViewModel(item));
                }


                return new UifJsonResult(true, rangeEntityResult.OrderBy(x => x.Description).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrMsgRangeEntityNotFound);
            }
        }

        /// </summary>
        /// Obtener todas las rangos de valores.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public ActionResult GetRangeEntitySelect()
        {
            try
            {
                List<RangeEntity> rangeEntity = DelegateService.conceptsService.GetRangeEntity();
                List<RangeEntityViewModel> rangeEntityResult = new List<RangeEntityViewModel>();


                foreach (RangeEntity item in rangeEntity)
                {
                    rangeEntityResult.Add(RangeEntityViewModel.RangeEntityToViewModel(item));
                }

                return new UifSelectResult(rangeEntityResult);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// </summary>
        /// Obtener las rangos de valores que coinciden con la descripción.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public ActionResult GetRangeEntityByDescription(string Description)
        {
            try
            {
                List<RangeEntity> rangeEntity = DelegateService.conceptsService.GetRangeEntityByDescription(Description);
                List<RangeEntityViewModel> rangeEntityResult = new List<RangeEntityViewModel>();

                foreach (RangeEntity item in rangeEntity)
                {
                    rangeEntityResult.Add(RangeEntityViewModel.RangeEntityToViewModel(item));
                }

                if (rangeEntityResult.Count != 0)
                    return Json(rangeEntityResult.OrderBy(x => x.Description).ToList(), JsonRequestBehavior.AllowGet);


                return new UifJsonResult(false, App_GlobalResources.Language.ErrMsgRangeEntityNotFound);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// obtine los valores correspondientes al rango
        /// </summary>
        /// <param name="rangeEntityCode">codigo del rango</param>
        /// <returns></returns>
        public ActionResult GetRangeEntityValueByRangeEntityCode(int rangeEntityCode)
        {
            try
            {
                List<RangeEntity> rangeEntity = DelegateService.conceptsService.GetRangeEntityValueByRangeEntityCode(rangeEntityCode);
                List<RangeEntityViewModel> RangeEntityResult = new List<RangeEntityViewModel>();

                foreach (RangeEntity item in rangeEntity)
                {
                    RangeEntityResult.Add(RangeEntityViewModel.RangeEntityToViewModel(item));
                }

                if (RangeEntityResult.Count != 0)
                    return Json(RangeEntityResult.OrderBy(x => x.Description).ToList(), JsonRequestBehavior.AllowGet);

                return new UifJsonResult(false, App_GlobalResources.Language.ErrMsgRangeEntityNotFound);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        [HttpPost]
        public JsonResult SaveRangeEntity(List<RangeEntity> rangeEntities)
        {
            try
            {
                rangeEntities = DelegateService.conceptsService.ExecuteOperationRangeEntities(rangeEntities);

                int totalAdded = rangeEntities.Count(x => x.StatusTypeService == StatusTypeService.Create);
                int totalModified = rangeEntities.Count(x => x.StatusTypeService == StatusTypeService.Update);
                int totalDeleted = rangeEntities.Count(x => x.StatusTypeService == StatusTypeService.Delete);

                StringBuilder errorMessage = new StringBuilder();
                rangeEntities.Where(x => x.StatusTypeService == StatusTypeService.Error).ForEach(x => x.ErrorServiceModel.ErrorDescription.ForEach(y => errorMessage.Append("</br>" + y)));

                return new UifJsonResult(true, new ParametrizationResult { TotalAdded = totalAdded, TotalDeleted = totalDeleted, TotalModified = totalModified, Message = errorMessage.ToString() });
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }
        #endregion

        #region Reference
        /// <summary>
        /// Obtiene la listas de referencias 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetReferencesEntitySelect(int packageId, int levelId)
        {
            var listReferences = DelegateService.conceptsService.GetForeignEntities(packageId, levelId);
            return new UifSelectResult(listReferences);
        }
        #endregion
    }
}