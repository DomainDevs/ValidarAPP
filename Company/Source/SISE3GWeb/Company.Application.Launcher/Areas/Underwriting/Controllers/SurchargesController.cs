// -----------------------------------------------------------------------
// <copyright file="SurchargesController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Lozano</author>
// -----------------------------------------------------------------------
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.ModelServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers
{
    public class SurchargesController : Controller
    {
        /// <summary>
        /// Lista los recargos
        /// </summary>  
        /// <returns> retorna Lista de Tasas </returns>
        /// 
  

        //[HttpPost]
        //public UifJsonResult SaveTemporarySurcharges(List<SurchargesViewModel> surchargesView)
        //{
        //    try
        //    {
        //       List<CompanySurchargeComponent> resultSave = DelegateService.underwritingService.ExecuteOperationCompanySurcharges(ModelAssembler.CreateCompanySurcharges(surchargesView));
        //        int totalCreated = resultSave.Count(p => p.State == (int)StatusTypeService.Create);
        //        int totalDeleted = surchargesView.Count(p => p.StatusTypeService == StatusTypeService.Delete);
        //        string messageCreated = null;
        //        string messageDeleted = null;
        //        if (totalCreated > 0)
        //        {
        //            messageCreated = $"{App_GlobalResources.Language.VehicleTypeCreated}: {totalCreated}";
        //        }

        //        if (totalDeleted > 0)
        //        {
        //            messageDeleted = $"{App_GlobalResources.Language.VehicleTypeDeleted}: {totalDeleted}";
        //        }

        //        return new UifJsonResult(
        //            true,
        //            new
        //            {
        //                messageCreated = messageCreated,
        //                messageDeleted = messageDeleted
        //            });
        //    }
        //    catch (Exception e)
        //    {
        //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSurcharge);
        //    }
        //}
        /// <summary>
        /// obtiene lista de recargos
        /// </summary>
        /// <returns> retorna la vista de recargos </returns>
        //private List<SurchargesViewModel> GetListSurcharges()
        //{
        //    List<SurchargesViewModel> SurchargesViewModel = new List<SurchargesViewModel>();
        //    if (SurchargesViewModel.Count == 0)
        //    {
        //        SurchargesServiceModel surchargeServiceModel = DelegateService.CompanyUnderwritingBusinessService.GetCompanySurcharges();

        //        SurchargesViewModel = ModelAssembler.CreateSurcharge(surchargeServiceModel.SurchargeServiceModel);
        //        return SurchargesViewModel.OrderBy(x => x.Description).ToList();
        //    }

        //    return SurchargesViewModel;
        //}

    }
}