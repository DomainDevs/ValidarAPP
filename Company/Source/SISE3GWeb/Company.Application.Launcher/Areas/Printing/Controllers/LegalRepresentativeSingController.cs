// -----------------------------------------------------------------------
// <copyright file="LegalRepresentativeSingController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Printing.Controllers
{
    using Sistran.Company.Application.ModelServices.Models.UniquePerson;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Framework.UIF.Web.Areas.Printing.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Contiene los procedimientos del controlador LegalRepresentativeSing
    /// </summary>
    public class LegalRepresentativeSingController : Controller
    {
        /// <summary>
        /// Una lista de BranchTypeServiceModel
        /// </summary>
        private List<BranchTypeServiceModel> branchTypes = new List<BranchTypeServiceModel>();

        /// <summary>
        /// Listado de Firma representante legal.
        /// </summary>
        private LegalRepresentativesSingServiceModel legalRepresentativesSingServiceModel = new LegalRepresentativesSingServiceModel();

        /// <summary>
        /// Metodo LegalRepresentativeSing que retorna una vista
        /// </summary>
        /// <returns>Una vista</returns>
        public ActionResult LegalRepresentativeSing()
        {
            return this.View();
        }

        /// <summary>
        /// Método para retornar una vista parcial
        /// </summary>
        /// <returns>Una vista parcial</returns>
        [HttpGet]
        public ActionResult LegalRepresentativeSingAdvancedSearch()
        {
            return this.View();
        }
        
        /// <summary>
        /// Obtiene la lista de Firma representante legal.
        /// </summary>
        /// <returns>Lista de Firma representante legal.</returns>
        public ActionResult GetAllLegalRepresentativeSing()
        {
            LegalRepresentativesSingServiceModel legalRepresentativeSingList = DelegateService.companyUniquePersonParamService.GetLstCptLegalReprSign();
            ErrorTypeService errorTypeProcess = legalRepresentativeSingList.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                List<LegalRepresentativeSingViewModel> legalRepresentativeSingListViewModel = ModelAssembler.GetLegalRepresentativeSings(legalRepresentativeSingList);
                return new UifJsonResult(true, legalRepresentativeSingListViewModel.OrderBy(x => x.LegalRepresentative).ToList());
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(legalRepresentativeSingList.ErrorDescription));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Método para obtener tipos de sucursal
        /// </summary>
        /// <returns>Un lista de tipos de sucursal</returns>
        public ActionResult GetBranchTypes()
        {
            BranchTypesServiceModel companyTypes = DelegateService.companyUniquePersonParamService.GetLstBranchTypes();
            ErrorTypeService errorTypeProcess = companyTypes.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                List<BranchTypeServiceModel> companyTypesServiceModel = companyTypes.BranchTypeServiceModel;
                return new UifJsonResult(true, companyTypesServiceModel.OrderBy(x => x.Description).ToList());
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(companyTypes.ErrorDescription));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Método para obtener tipos de compañia
        /// </summary>
        /// <returns>Un lista de tipos de compañia</returns>
        public ActionResult GetCompanyTypes()
        {
            CompanyTypesServiceModel branchTypes = DelegateService.companyUniquePersonParamService.GetLstCompanyTypes();
            ErrorTypeService errorTypeProcess = branchTypes.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                List<CompanyTypeServiceModel> branchTypesServiceModel = branchTypes.CompanyTypeServiceModel;
                return new UifJsonResult(true, branchTypesServiceModel.OrderBy(x => x.Description).ToList());
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(branchTypes.ErrorDescription));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Realiza los procesos del CRUD para los canales
        /// </summary>
        /// <param name="listAdded">Lista de LegalRepresentativeSings(canales) para ser agregados</param>
        /// <param name="listModified">Lista de LegalRepresentativeSings(canales) para ser modificados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados</returns>
        public ActionResult SaveLegalRepresentativeSing(List<LegalRepresentativeSingViewModel> listAdded, List<LegalRepresentativeSingViewModel> listModified)
        {
            try
            {
                LegalRepresentativesSingServiceModel legalRepresentativeSingServiceModel = new LegalRepresentativesSingServiceModel();
                legalRepresentativeSingServiceModel.ErrorDescription = new List<string>();
                legalRepresentativeSingServiceModel.ErrorTypeService = ErrorTypeService.Ok;

                List<LegalRepresentativeSingServiceModel> listToPersistLegalRepresentativeSingServiceModel = new List<LegalRepresentativeSingServiceModel>();
                List<LegalRepresentativeSingServiceModel> listAddedLegalRepresentativeSingServiceModel = ModelAssembler.MappLegalRepresentativeSingToSave(listAdded, StatusTypeService.Create);
                List<LegalRepresentativeSingServiceModel> listModifiedLegalRepresentativeSingServiceModel = ModelAssembler.MappLegalRepresentativeSingToSave(listModified, StatusTypeService.Update);
                
                if (listAdded != null)
                {
                    listToPersistLegalRepresentativeSingServiceModel.AddRange(listAddedLegalRepresentativeSingServiceModel);
                }

                if (listModified != null)
                {
                    listToPersistLegalRepresentativeSingServiceModel.AddRange(listModifiedLegalRepresentativeSingServiceModel);
                }

                legalRepresentativeSingServiceModel.LegalRepresentativeSingServiceModel = listToPersistLegalRepresentativeSingServiceModel;
                
                ParametrizationResponse<LegalRepresentativesSingServiceModel> legalRepresentativeSingResponse = DelegateService.companyUniquePersonParamService.CreateLegalRepresentativeSing(legalRepresentativeSingServiceModel);

                string added = string.Empty;
                string edited = string.Empty;
                string message;

                if (!string.IsNullOrEmpty(legalRepresentativeSingResponse.ErrorAdded))
                {
                    legalRepresentativeSingResponse.ErrorAdded = App_GlobalResources.Language.ResourceManager.GetString(legalRepresentativeSingResponse.ErrorAdded);
                }

                if (!string.IsNullOrEmpty(legalRepresentativeSingResponse.ErrorModify))
                {
                    legalRepresentativeSingResponse.ErrorModify = App_GlobalResources.Language.ResourceManager.GetString(legalRepresentativeSingResponse.ErrorModify);
                }

                if (legalRepresentativeSingResponse.TotalAdded > 0)
                {
                    added = App_GlobalResources.Language.ReturnSaveAddedLegalRepresentativeSing;
                }
                else
                {
                    legalRepresentativeSingResponse.TotalAdded = null;
                }

                if (legalRepresentativeSingResponse.TotalModify > 0)
                {
                    edited = App_GlobalResources.Language.ReturnSaveEditedLegalRepresentativeSing;
                }
                else
                {
                    legalRepresentativeSingResponse.TotalModify = null;
                }

                message = string.Format(
                    added + edited + "{2}{3}",
                    legalRepresentativeSingResponse.TotalAdded.ToString() ?? string.Empty,
                    legalRepresentativeSingResponse.TotalModify.ToString() ?? string.Empty,
                    legalRepresentativeSingResponse.ErrorAdded ?? string.Empty,
                    legalRepresentativeSingResponse.ErrorModify ?? string.Empty);

                this.GetListLegalRepresentativeSing();
                List<LegalRepresentativeSingViewModel> legalRepresentativeSingListViewModel = ModelAssembler.GetLegalRepresentativeSings(this.legalRepresentativesSingServiceModel);
                return new UifJsonResult(true, new { message = message, data = legalRepresentativeSingListViewModel });
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveLegalRepresentativeSing);
            }
        }

        /// <summary>
        /// Genera el archivo de Excel de firma de representante legal
        /// </summary>
        /// <returns>Archivo de Excel</returns>
        public ActionResult GenerateFileToExport()
        {
            this.GetListLegalRepresentativeSing();
            ExcelFileServiceModel excelFileServiceModel = DelegateService.companyUniquePersonParamService.GenerateFileToLegalRepresentativeSing(this.legalRepresentativesSingServiceModel, App_GlobalResources.Language.FileNameLegalRepresentativeSing);

            string urlFile = excelFileServiceModel.FileData;

            if (string.IsNullOrEmpty(urlFile))
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
            }
            else
            {
                return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
            }
        }

        /// <summary>
        /// Obtiene los formatos de impresión del aliado.
        /// </summary>
        /// <returns>Retorna Errores de la consulta</returns>
        private ErrorTypeService GetListLegalRepresentativeSing()
        {
            LegalRepresentativesSingServiceModel legalRepresentativesSingServiceModelList = new LegalRepresentativesSingServiceModel();
            ErrorTypeService errorTypeProcess = legalRepresentativesSingServiceModelList.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                this.legalRepresentativesSingServiceModel = DelegateService.companyUniquePersonParamService.GetLstCptLegalReprSign();
            }

            return legalRepresentativesSingServiceModelList.ErrorTypeService;
        }

        /// <summary>
        /// Metodo para retornar los mensajes de error.
        /// </summary>
        /// <param name="errorList">Lista de errores</param>
        /// <returns>Mensajes de error.</returns>
        private string ErrorMessages(List<string> errorList)
        {
            string errorMessages = string.Empty;
            foreach (string errorMessageItem in errorList)
            {
                errorMessages = errorMessages + errorMessageItem + " <br>";
            }

            return errorMessages;
        }
    }
}