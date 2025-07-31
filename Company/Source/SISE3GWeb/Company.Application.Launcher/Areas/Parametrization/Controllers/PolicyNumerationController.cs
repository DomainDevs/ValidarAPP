// -----------------------------------------------------------------------
// <copyright file="PolicyNumerationController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Sistran.Core.Application.EntityServices.Models;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using ENUENT = Sistran.Core.Application.EntityServices.Enums;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using PARCPSM = Sistran.Core.Application.ModelServices.Models.CommonParam;
    using PARUPSM = Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Application.EntityServices.Enums;
    using MODPAUN = Sistran.Core.Application.ModelServices.Models.Underwriting;

    /// <summary>
    /// Controlador para la numeración de polizas
    /// </summary>
    public class PolicyNumerationController : Controller
    {
        /// <summary>
        /// Parametros de entidad 
        /// </summary>
        private PostEntity postEntity = new PostEntity { EntityType = "Sistran.Core.Application.Issuance.Entities.PolicyNumber", KeyType = KeyType.None };

        /// <summary>
        /// Llama a la vista de numeración de polizas
        /// </summary>
        /// <returns>Vista de numeración de polizas</returns>
        public ActionResult PolicyNumeration()
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
        public ActionResult GetPolicyNumerationByBranchIdPrefixId(int branchId, int prefixId)
        {
            try
            {
                PolicyNumerationViewModel policyNumerationViewModel = new PolicyNumerationViewModel();

                PolicyNumbersServiceModel policyNumbersServiceModel = DelegateService.UnderwritingParamServiceWeb.GetParamPolicyNumbersByBranchIdPrefixId(branchId, prefixId);
                if (policyNumbersServiceModel.PolicyNumberServiceModels.Count > 0)
                {
                    PolicyNumberServiceModel policyNumberServiceModel = policyNumbersServiceModel.PolicyNumberServiceModels[0];
                    policyNumerationViewModel = new PolicyNumerationViewModel()
                    {
                        LastPolicy = policyNumberServiceModel.PolicyLastNumber,
                        DueDateTo = policyNumberServiceModel.LastPolicyDate.Day + "/" + policyNumberServiceModel.LastPolicyDate.Month + "/" + policyNumberServiceModel.LastPolicyDate.Year,
                        BranchDescription = policyNumberServiceModel.Branch.Description,
                        BranchId = policyNumberServiceModel.Branch.Id,
                        PrefixDescription = policyNumberServiceModel.Prefix.PrefixDescription,
                        PrefixId = policyNumberServiceModel.Prefix.PrefixCode,
                        HasPolicy = policyNumberServiceModel.HasPolicy
                        
                    };
                    return new UifJsonResult(true, policyNumerationViewModel);
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
        /// <param name="policyNumerationViewModel">Listado PolicyNumerationViewModel</param>
        /// <returns>Listado numeraciones en base de datos</returns>
        [HttpPost]
        public ActionResult Save(PolicyNumerationViewModel policyNumerationViewModel)
        {
            List<Field> fields = new List<Field>();
            try
            {
                ////Parametros enviados
                fields.Add(new Field { Name = "BranchCode", Value = policyNumerationViewModel.BranchId.ToString() });
                fields.Add(new Field { Name = "PrefixCode", Value = policyNumerationViewModel.PrefixId.ToString() });
                fields.Add(new Field { Name = "LastPolicyNum", Value = policyNumerationViewModel.LastPolicy.ToString() });
                fields.Add(new Field { Name = "LastPolicyDate", Value = policyNumerationViewModel.DueDateTo.ToString() });
                this.postEntity.Fields = fields;               
                this.postEntity.Status = policyNumerationViewModel.StatusTypeService;
                if(policyNumerationViewModel.StatusTypeService == StatusTypeService.Create)
                {
                    ////Ejecuta la creación de la numeración
                    this.postEntity = DelegateService.EntityServices.Create(this.postEntity);
                }
                else
                {
                    ////Ejecuta la creación de la numeración
                    this.postEntity = DelegateService.EntityServices.Update(this.postEntity);
                }
               
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
        /// <param name="policyNumerationViewModel">Listado PolicyNumerationViewModel</param>
        /// <returns>Listado de numeraciones en base de datos</returns>
        [HttpPost]
        public ActionResult Delete(PolicyNumerationViewModel policyNumerationViewModel)
        {
            List<Field> fields = new List<Field>();
            try
            {
                int hasDependencies = DelegateService.UnderwritingParamServiceWeb.ValidatePolicyNumber(policyNumerationViewModel.BranchId, policyNumerationViewModel.PrefixId);

                if (hasDependencies == 0)
                {
                    ////Parametros enviados
                    fields = new List<Field>();
                    fields.Add(new Field { Name = "BranchCode", Value = policyNumerationViewModel.BranchId.ToString() });
                    fields.Add(new Field { Name = "PrefixCode", Value = policyNumerationViewModel.PrefixId.ToString() });
                    this.postEntity.Fields = fields;
                    this.postEntity.Status = ENUENT.StatusTypeService.Delete;
                    ////Ejecuta la eliminación de la numeración
                    DelegateService.EntityServices.Delete(this.postEntity);
                    return new UifJsonResult(true, new { message = App_GlobalResources.Language.NumerationDeleted });
                }
                else
                {
                    return new UifJsonResult(true, new { message = App_GlobalResources.Language.NumerationHavePolicys });
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
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToPolicyNumber(App_GlobalResources.Language.LabelTabPolicyNumeration);
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