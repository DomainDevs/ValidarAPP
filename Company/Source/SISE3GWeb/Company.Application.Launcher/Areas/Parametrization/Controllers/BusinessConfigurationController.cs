// -----------------------------------------------------------------------
// <copyright file="BusinessConfigurationController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;

    /// <summary>
    /// Acciones de BusinessConfiguration
    /// </summary>
    public class BusinessConfigurationController : Controller
    {
        /// <summary>
        /// Lista de ramos comerciales.
        /// </summary>
        /// <returns>Resultado de la consulta.</returns>
        public ActionResult GetPrefixes()
        {
            try
            {
                List<PrefixServiceQueryModel> listPrefixes = DelegateService.UnderwritingParamServiceWeb.GetPrefixes();
                return new UifJsonResult(true, listPrefixes);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
            }

        }

        /// <summary>
        /// Lista de solicitudes agrupadoras vigentes por ramo comercial.
        /// </summary>
        /// <returns>Resultado de la consulta.</returns>
        public ActionResult GetCurrentRequestEndorsementByPrefixCode(int prefixCode)
        {
            try
            {
                List<RequestEndorsementServiceQueryModel> listRequestEndorsement = DelegateService.UnderwritingParamServiceWeb.GetCurrentRequestEndorsementByPrefixCode(prefixCode);
                return new UifJsonResult(true, listRequestEndorsement);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRequestEndorsement);
            }

        }

        /// <summary>
        /// Lista de productos vigentes por ramo comercial.
        /// </summary>
        /// <returns>Resultado de la consulta.</returns>
        public ActionResult GetCurrentProductByPrefixCode(int prefixCode)
        {
            try
            {
                List<ProductServiceQueryModel> listProductServiceQueryModel = DelegateService.UnderwritingParamServiceWeb.GetCurrentProductByPrefixCode(prefixCode);
                return new UifJsonResult(true, listProductServiceQueryModel);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProducts);
            }

        }

        /// <summary>
        /// Lista de grupo de cobertura por producto.
        /// </summary>
        /// <returns>Resultado de la consulta.</returns>
        public ActionResult GetGroupCoverageByProductCode(int productCode)
        {
            try
            {
                List<GroupCoverageServiceQueryModel> listGroupCoverageServiceQueryModel = DelegateService.UnderwritingParamServiceWeb.GetGroupCoverageByProductCode(productCode);
                return new UifJsonResult(true, listGroupCoverageServiceQueryModel);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetGroupCoverages);
            }

        }

        /// <summary>
        /// Lista de asistencias por producto.
        /// </summary>
        /// <returns>Resultado de la consulta.</returns>
        public ActionResult GetAssistanceTypeByProductCode(int productCode)
        {
            try
            {
                List<AssistanceTypeServiceQueryModel> listAssistanceTypeServiceQueryModel = DelegateService.UnderwritingParamServiceWeb.GetAssistanceTypeByProductCode(productCode);
                return new UifJsonResult(true, listAssistanceTypeServiceQueryModel);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAssistanceType);
            }

        }

        /// <summary>
        /// Obtiene la lista de negocios y acuerdos de negocio.
        /// </summary>
        /// <returns>Lista de negocios y acuerdos de negocio obtenidos.</returns>
        public ActionResult GetBusinessConfiguration()
        {
            try
            {
                var datos = DelegateService.UnderwritingParamServiceWeb.GetBusinessConfiguration().BusinessServiceModel;
                var mapper = ModelAssembler.GetBusinessConfiguration(datos);
                List<BusinessConfigurationViewModel> businessConfigurationList = ModelAssembler.GetBusinessConfiguration(DelegateService.UnderwritingParamServiceWeb.GetBusinessConfiguration().BusinessServiceModel);
                return new UifJsonResult(true, businessConfigurationList.OrderBy(x => x.BusinessId).ToList());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBusinessConfiguration);
            }
        }

        /// <summary>
        /// Obtiene la Vista del negocio y acuerdo de negocio.
        /// </summary>
        /// <returns>Vista del negocio y acuerdo de negocio.</returns>
        public ActionResult BusinessConfiguration()
        {
            BusinessConfigurationViewModel model = new BusinessConfigurationViewModel();
            return this.View(model);
        }

        /// <summary>
        /// Obtiene la vista con el resultado de la búsqueda de la vista Acuerdos de negocio 
        /// </summary>
        /// <returns>Resultado de la búsqueda</returns>
        [HttpGet]
        public ActionResult BusinessConfigurationAdvancedSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene la Vista del negocio y acuerdo de negocio.
        /// </summary>
        /// <returns>Vista del negocio y acuerdo de negocio.</returns>
        public ActionResult SaveBusinessConfiguration(List<BusinessConfigurationViewModel> listBusinessConfigurationViewModel)
        {
            try
            {
                BusinessServiceQueryModel result = new BusinessServiceQueryModel();
                List<BusinessServiceModel> listBusinessServiceModel = new List<BusinessServiceModel>();
                listBusinessServiceModel = ModelAssembler.CreateBusinessConfiguration(listBusinessConfigurationViewModel);
                result.BusinessServiceModel = listBusinessServiceModel;
                ParametrizationResponse<BusinessServiceModel> datosResult = DelegateService.UnderwritingParamServiceWeb.SaveBusiness(result);
                string added = string.Empty;
                string edited = string.Empty;
                string deleted = string.Empty;
                string message;
                if (!string.IsNullOrEmpty(datosResult.ErrorAdded))
                {
                    datosResult.ErrorAdded = App_GlobalResources.Language.ResourceManager.GetString(datosResult.ErrorAdded);
                }

                if (!string.IsNullOrEmpty(datosResult.ErrorModify))
                {
                    datosResult.ErrorModify = App_GlobalResources.Language.ResourceManager.GetString(datosResult.ErrorModify);
                }

                if (!string.IsNullOrEmpty(datosResult.ErrorDeleted))
                {
                    datosResult.ErrorDeleted = App_GlobalResources.Language.ResourceManager.GetString(datosResult.ErrorDeleted);
                }

                if (datosResult.TotalAdded > 0)
                {
                    added = App_GlobalResources.Language.ReturnSaveAddedBusinessConfiguration;
                }
                else
                {
                    datosResult.TotalAdded = null;
                }

                if (datosResult.TotalModify > 0)
                {
                    edited = App_GlobalResources.Language.ReturnSaveEditedBusinessConfiguration;
                }
                else
                {
                    datosResult.TotalModify = null;
                }

                if (datosResult.TotalDeleted > 0)
                {
                    deleted = App_GlobalResources.Language.ReturnSaveDeletedBusinessConfiguration;
                }
                else
                {
                    datosResult.TotalDeleted = null;
                }
                message = string.Format(
                   added + edited + deleted + "{3}{4}{5}",
                   datosResult.TotalAdded.ToString() ?? string.Empty,
                   datosResult.TotalModify.ToString() ?? string.Empty,
                   datosResult.TotalDeleted.ToString() ?? string.Empty,
                   datosResult.ErrorAdded ?? string.Empty,
                   datosResult.ErrorModify ?? string.Empty,
                   datosResult.ErrorDeleted ?? string.Empty);
                var resultViewModel = ModelAssembler.GetBusinessConfiguration(datosResult.ReturnedList.OrderBy(x => x.BusinessId).ToList());
                return new UifJsonResult(true, new { message = message, data = resultViewModel });
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveBusinessConfigurations);
            }
        }

        /// <summary>
        /// Genera archivo excel negocios y acuerdos de negocios.
        /// </summary>
        /// <returns>Archivo excel con los negocios y acuerdos de negocios</returns>
        public ActionResult GenerateBusinessConfigurationFileToExport()
        {
            try
            {
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToBusiness();
                if (excelFileServiceModel.ErrorTypeService == ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
    }
}