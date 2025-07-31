// -----------------------------------------------------------------------
// <copyright file="QuotationNumerationController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Sistran.Core.Application.EntityServices.Models;
    using Sistran.Core.Application.ModelServices.Models.Param;    
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using ENUENT = Sistran.Core.Application.EntityServices.Enums;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using PARCPSM = Sistran.Core.Application.ModelServices.Models.CommonParam;
    using PARUPSM = Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using MODPAUN = Sistran.Core.Application.ModelServices.Models.Underwriting;

    /// <summary>
    /// Controlador para la numeración de cotizaciones
    /// </summary>
    public class QuotationNumerationController : Controller
    {
        /// <summary>
        /// Parametros de entidad 
        /// </summary>
        private PostEntity postEntity = new PostEntity { EntityType = "Sistran.Core.Application.Quotation.Entities.QuotationNumber", KeyType = KeyType.None };

        /// <summary>
        /// Llama a la vista de numeración de cotizaciones
        /// </summary>
        /// <returns>Vista de numeración de cotizaciones</returns>
        public ActionResult QuotationNumeration()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene la lista de sucursales
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public ActionResult GetBranchs()
        {
            try
            {
                PARCPSM.BranchesServiceQueryModel branchList = DelegateService.companyCommonParamService.GetBranch();
                return new UifJsonResult(true, branchList.BranchServiceQueryModel.OrderBy(x => x.Description).ToList());
            }
            catch (System.Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }

        /// <summary>
        /// Obtiene la lista de Ramos
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public ActionResult GetPrefixes()
        {
            try
            {
                List<MODPAUN.PrefixServiceQueryModel> prefixList = DelegateService.UnderwritingParamServiceWeb.GetPrefixes();
                return new UifJsonResult(true, prefixList.OrderBy(x => x.PrefixDescription).ToList());
            }
            catch (System.Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }

        /// <summary>
        /// Obtiene las numeraciones por sucursal y ramo
        /// </summary>
        /// <param name="branchId">Id de la sucursal</param>
        /// <param name="prefixId">Id del ramo</param>
        /// <returns>Lista de numeraciones</returns>
        public ActionResult GetQuotationNumerationByBranchIdPrefixId(int branchId, int prefixId)
        {
            try
            {
                QuotationNumerationViewModel quotationNumerationViewModel = new QuotationNumerationViewModel();

                PARUPSM.QuotationNumbersServiceModel quotationNumbersServiceModel = DelegateService.UnderwritingParamServiceWeb.GetParametrizationQuotationNumbersByBranchIdPrefixId(branchId, prefixId);
                if (quotationNumbersServiceModel.QuotationNumberServiceModels.Count > 0)
                {
                    PARUPSM.QuotationNumberServiceModel quotationNumberServiceModel = quotationNumbersServiceModel.QuotationNumberServiceModels[0];
                    quotationNumerationViewModel = new QuotationNumerationViewModel()
                    {
                        LastQuotation = quotationNumberServiceModel.QuotNumber,
                        BranchDescription = quotationNumberServiceModel.Branch.Description,
                        BranchId = quotationNumberServiceModel.Branch.Id,
                        PrefixDescription = quotationNumberServiceModel.Prefix.PrefixDescription,
                        PrefixId = quotationNumberServiceModel.Prefix.PrefixCode
                    };
                    return new UifJsonResult(true, quotationNumerationViewModel);
                }

                return new UifJsonResult(true, null);
            }
            catch (System.Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSalePoints);
            }
        }

        /// <summary>
        /// Metodo para guardar las numeraciones
        /// </summary>
        /// <param name="quotationNumerationViewModel">Listado QuotationNumerationViewModel</param>
        /// <returns>Listado numeraciones en base de datos</returns>
        [HttpPost]
        public ActionResult Save(QuotationNumerationViewModel quotationNumerationViewModel)
        {
            List<Field> fields = new List<Field>();
            try
            {
                ////Parametros enviados
                fields.Add(new Field { Name = "BranchCode", Value = quotationNumerationViewModel.BranchId.ToString() });
                fields.Add(new Field { Name = "PrefixCode", Value = quotationNumerationViewModel.PrefixId.ToString() });
                fields.Add(new Field { Name = "QuotNumber", Value = quotationNumerationViewModel.LastQuotation.ToString() });
                this.postEntity.Fields = fields;
                this.postEntity.Status = ENUENT.StatusTypeService.Create;
                ////Ejecuta la creación de la numeración
                this.postEntity = DelegateService.EntityServices.Create(this.postEntity);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavingNumeration);
            }

            return new UifJsonResult(true, new { message = App_GlobalResources.Language.NumerationSaved });
        }

        /// <summary>
        /// Metodo para eliminar las numeraciones
        /// </summary>
        /// <param name="quotationNumerationViewModel">Listado QuotationNumerationViewModel</param>
        /// <returns>Listado de numeraciones en base de datos</returns>
        [HttpPost]
        public ActionResult Delete(QuotationNumerationViewModel quotationNumerationViewModel)
        {
            List<Field> fields = new List<Field>();
            try
            {
                int hasDependencies = DelegateService.UnderwritingParamServiceWeb.ValidateQuotationNumber(quotationNumerationViewModel.BranchId, quotationNumerationViewModel.PrefixId);

                if (hasDependencies == 0)
                {
                    ////Parametros enviados
                    fields.Add(new Field { Name = "BranchCode", Value = quotationNumerationViewModel.BranchId.ToString() });
                    fields.Add(new Field { Name = "PrefixCode", Value = quotationNumerationViewModel.PrefixId.ToString() });
                    this.postEntity.Fields = fields;
                    this.postEntity.Status = ENUENT.StatusTypeService.Delete;
                    ////Ejecuta la eliminación de la numeración
                    DelegateService.EntityServices.Delete(this.postEntity);
                    return new UifJsonResult(true, new { message = App_GlobalResources.Language.NumerationDeleted });
                }
                else
                {
                    return new UifJsonResult(true, new { message = App_GlobalResources.Language.NumerationHavequotations });
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeletingNumeration);
            }
        }

        /// <summary>
        /// Genera archivo excel de numeración de póliza
        /// </summary>
        /// <returns>Arhivo de excel de numeración de póliza</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToQuotationNumber(App_GlobalResources.Language.LabelTabQuotationNumeration);
                if (excelFileServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
    }
}