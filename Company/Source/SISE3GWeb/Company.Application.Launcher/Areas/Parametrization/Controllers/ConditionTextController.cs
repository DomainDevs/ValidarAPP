
using Newtonsoft.Json.Linq;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Company.Application.ParametrizationAplicationServices.DTO;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.QuotationServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Company.Application.UnderwritingParamApplicationService.DTOs;
using Sistran.Company.Application.Utilities.DTO;
using Sistran.Company.Application.Utilities.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class ConditionTextController : Controller
    {
        //Entidades
        private Helpers.PostEntity entityCoveredRiskType = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Parameters.Entities.CoveredRiskType" };
        private Helpers.PostEntity entityLineBusiness = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.LineBusiness" };
        private Helpers.PostEntity entityPrefix = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.Prefix" };
        private Helpers.PostEntity entityCoverage = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Quotation.Entities.Coverage" };
        private Helpers.PostEntity entityConditionLevel = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Parameters.Entities.ConditionLevel" };

        private List<GenericViewModel> GenericConditionLevel = new List<GenericViewModel>();
        private List<GenericViewModel> conditionLevel = new List<GenericViewModel>();
        
        //CondTextLevelModel asdad

        #region "views"
        public ActionResult ConditionText()
        {
            return View();
        }
        #endregion

        public ActionResult GetConditionLevel()
        {
            try
            {
                this.GetConditionLevels();
                return new UifJsonResult(true, this.conditionLevel);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCountries);
            }
        }
        public ActionResult GetGenericConditionLevel(int iType)
        {
            try
            {
                this.GetGenericConditionLevels(iType);
                return new UifJsonResult(true, this.GenericConditionLevel);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCountries);
            }
        }

        /// <summary>
        /// GetListCountries: Carga el listado de paises: table comm.Country con combos genericos
        /// </summary>
        public void GetGenericConditionLevels(int iType)
        {
            GenericConditionLevel.Clear();
            if (GenericConditionLevel.Count == 0)
            {
                switch (iType)
                {
                    case 2:
                        GenericConditionLevel = ModelAssembler.CreateLineBusiness(ModelAssembler.DynamicToDictionaryList(this.entityLineBusiness.CRUDCliente.Find(this.entityLineBusiness.EntityType, null, null))).OrderBy(x => x.DescriptionLong).ToList();
                        break;
                    case 3:
                        GenericConditionLevel = ModelAssembler.CreateCoveredRiskType(ModelAssembler.DynamicToDictionaryList(this.entityCoveredRiskType.CRUDCliente.Find(this.entityCoveredRiskType.EntityType, null, null))).OrderBy(x => x.DescriptionLong).ToList();
                        break;
                    case 4:
                        GenericConditionLevel = ModelAssembler.CreatePrefixes(ModelAssembler.DynamicToDictionaryList(this.entityPrefix.CRUDCliente.Find(this.entityPrefix.EntityType, null, null))).OrderBy(x => x.DescriptionLong).ToList();
                        break;
                    case 5:
                        GenericConditionLevel = ModelAssembler.CreateCoverage(ModelAssembler.DynamicToDictionaryList(this.entityCoverage.CRUDCliente.Find(this.entityPrefix.EntityType, null, null))).OrderBy(x => x.DescriptionLong).ToList();
                        break;
                }
            }
        }

        /// <summary>
        /// GetListCountries: Carga el listado de paises: table comm.Country con combos genericos
        /// </summary>
        public void GetConditionLevels()
        {
            if (conditionLevel.Count == 0)
            {
                conditionLevel = ModelAssembler.CreateConditionLevels(ModelAssembler.DynamicToDictionaryList(this.entityConditionLevel.CRUDCliente.Find(this.entityConditionLevel.EntityType, null, null))).OrderBy(x => x.DescriptionShort).ToList();
                conditionLevel = (from c in conditionLevel
                                  where c.Id != 1 && c.Id != 4
                                  select c).ToList();
                //conditionLevel = Filtro;
            }
        }

        /// <summary>
        /// Retorna el tipo de riesgo dependiendo del nivel de condidición obtenido.
        /// </summary>
        /// <param name="listConditionText">Lista de Objetos ConditionText</param>
        /// <returns>ConditionTextQueryDTO.Lista de Objetos ConditionText</returns>
        public ConditionTextQueryDTO GetRiskType(ConditionTextQueryDTO listConditionText)
        {
            int control;
            List<GenericViewModel> listLineBusiness = new List<GenericViewModel>();
            List<GenericViewModel> listCoveredRiskType = new List<GenericViewModel>();
            List<GenericViewModel> listPrefix = new List<GenericViewModel>();
            List<GenericViewModel> listCoverage = new List<GenericViewModel>();

            listLineBusiness = ModelAssembler.CreateLineBusiness(ModelAssembler.DynamicToDictionaryList(this.entityLineBusiness.CRUDCliente.Find(this.entityLineBusiness.EntityType, null, null))).OrderBy(x => x.DescriptionLong).ToList();

            listCoveredRiskType = ModelAssembler.CreateCoveredRiskType(ModelAssembler.DynamicToDictionaryList(this.entityCoveredRiskType.CRUDCliente.Find(this.entityCoveredRiskType.EntityType, null, null))).OrderBy(x => x.DescriptionLong).ToList();

            listPrefix = ModelAssembler.CreatePrefixes(ModelAssembler.DynamicToDictionaryList(this.entityPrefix.CRUDCliente.Find(this.entityPrefix.EntityType, null, null))).OrderBy(x => x.DescriptionLong).ToList();

            listCoverage = ModelAssembler.CreateCoverage(ModelAssembler.DynamicToDictionaryList(this.entityCoverage.CRUDCliente.Find(this.entityCoverage.EntityType, null, null))).OrderBy(x => x.DescriptionLong).ToList();


            foreach (ConditionTextDTO contitionText in listConditionText.ConditionText)
            {
                if (contitionText.ConditionTextLevelType.Id != -1)
                {

                    control = contitionText.Id;
                    switch (contitionText.ConditionTextLevel.Id)
                    {
                        case 2:
                            if (listLineBusiness.Where(x => x.Id == contitionText.ConditionTextLevelType.Id).FirstOrDefault() != null)
                            {
                                contitionText.ConditionTextLevelType.Description = listLineBusiness.Where(x => x.Id == contitionText.ConditionTextLevelType.Id).FirstOrDefault().DescriptionLong ?? "";
                            }
                            break;
                        case 3:
                            if (listCoveredRiskType.Where(x => x.Id == contitionText.ConditionTextLevelType.Id).FirstOrDefault() != null)
                            {
                                contitionText.ConditionTextLevelType.Description = listCoveredRiskType.Where(x => x.Id == contitionText.ConditionTextLevelType.Id).FirstOrDefault().DescriptionLong ?? "";
                            }
                            break;
                        case 4:
                            if (listPrefix.Where(x => x.Id == contitionText.ConditionTextLevelType.Id).FirstOrDefault() != null)
                            {
                                contitionText.ConditionTextLevelType.Description = listPrefix.Where(x => x.Id == contitionText.ConditionTextLevelType.Id).FirstOrDefault().DescriptionLong ?? "";
                            }

                            break;
                        case 5:
                            if (listCoverage.Where(x => x.Id == contitionText.ConditionTextLevelType.Id).FirstOrDefault() != null)
                            {
                                contitionText.ConditionTextLevelType.Description = listCoverage.Where(x => x.Id == contitionText.ConditionTextLevelType.Id).FirstOrDefault().DescriptionLong ?? "";
                            }
                            break;
                    }
                }

            }
            return listConditionText;
        }
        /// <summary>
        /// GetListCountries: Carga el listado de textos precatalogados: table comm.Country con combos genericos
        /// </summary>
        public ActionResult GetConditiontext()
        {
            ConditionTextQueryDTO listConditionText = new ConditionTextQueryDTO();
            try
            {
                listConditionText = DelegateService.CompanyUnderwritingParamApplicationService.GetApplicationConditiontext();
                listConditionText = GetRiskType(listConditionText);
                return new UifJsonResult(true, listConditionText.ConditionText.OrderBy(x => x.Title).Take(50).ToList());
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetListCities);
            }
        }
        /// <summary>
        /// GetConditiontextByDescription: Retorna Listado de textos a partir de un texto.
        /// </summary>
        public ActionResult GetConditiontextByDescription(string description)
        {
            ConditionTextQueryDTO conditionTextDTO = new ConditionTextQueryDTO();
            try
            {
                conditionTextDTO = DelegateService.CompanyUnderwritingParamApplicationService.GetApplicationConditiontextByDescription(description);
                conditionTextDTO = GetRiskType(conditionTextDTO);
                return new UifJsonResult(true, conditionTextDTO.ConditionText.OrderBy(x => x.Title).Take(50).ToList());
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetListCities);
            }
        }

        /// <summary>
        /// GetListCountries: Carga el listado de paises: table comm.Country con combos genericos
        /// </summary>
        public ActionResult CreateConditionText(ConditionTextViewModel conditionTextViewModel)
        {
            ConditionTextDTO conditionTextDTO = new ConditionTextDTO();
            try
            {
                conditionTextDTO = ModelAssembler.MappConditionTextVMApplication(conditionTextViewModel);
                conditionTextDTO = DelegateService.CompanyUnderwritingParamApplicationService.CreateApplicationConditiontext(conditionTextDTO);
                return new UifJsonResult(true, conditionTextDTO);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetListCities);
            }
        }

        /// <summary>
        /// UpdateConditionText: Modifica Valores conditionText.
        /// </summary>
        public ActionResult UpdateConditionText(ConditionTextViewModel conditionTextViewModel)
        {
            ConditionTextDTO conditionTextDTO = new ConditionTextDTO();
            try
            {
                conditionTextDTO = ModelAssembler.MappConditionTextVMApplication(conditionTextViewModel);
                conditionTextDTO = DelegateService.CompanyUnderwritingParamApplicationService.UpdateApplicationConditiontext(conditionTextDTO);
                return new UifJsonResult(true, conditionTextDTO);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetListCities);
            }
        }
        /// <summary>
        /// DeleteConditionText. Elimina el texto precatalogado seleccionado
        /// </summary>
        /// <param name="conditionTextViewModel">View Model</param>
        /// <returns>ActionResult<returns>
        public ActionResult DeleteConditionText(ConditionTextViewModel conditionTextViewModel)
        {
            ConditionTextDTO conditionTextDTO = new ConditionTextDTO();
            try
            {
                conditionTextDTO = ModelAssembler.MappConditionTextVMApplication(conditionTextViewModel);
                return new UifJsonResult(true, DelegateService.CompanyUnderwritingParamApplicationService.DeleteApplicationConditiontext(conditionTextDTO));
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetListCities);
            }

        }
        /// <summary>
        /// ExportFileCities: Consulta listado de ciudades y genera el excel a descargar
        /// </summary>
        /// <returns></returns>
        public JsonResult GenerateFileConditionText()
        {
            ExcelFileDTO Result = DelegateService.CompanyUnderwritingParamApplicationService.GenerateFileApplicationToConditiontext("TEXTOS PRECATALOGADOS");
            try
            {
                if (Result.ErrorType == ErrorType.Ok && !String.IsNullOrEmpty(Result.File))
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + Result.File);
                }
                else
                {
                    return new UifJsonResult(false, string.Join(",", App_GlobalResources.Language.ErrorExportCity));
                }
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExportCity);
            }

        }
    }
}
