using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENUMSM = Sistran.Core.Application.ModelServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers
{
    public class DeductibleController : Controller
    {
        /// <summary>
        /// Obtiene las unidades
        /// </summary>
        /// <returns>Listado DeductibleUnitsServiceQueryModel</returns>
        public ActionResult GetDeductibleUnit()
        {
            DeductibleUnitsServiceQueryModel deductibleUnitsServiceModel = DelegateService.UnderwritingParamServiceWeb.GetDeductibleUnits();
            if (deductibleUnitsServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, deductibleUnitsServiceModel.DeductibleUnitServiceModels.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { deductibleUnitsServiceModel.ErrorTypeService, deductibleUnitsServiceModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Obtiene los deducibles
        /// </summary>
        /// <returns>Listado DeductibleSubjectServiceModels</returns>
        public ActionResult GetDeductibleSubject()
        {
            DeductibleSubjectsServiceQueryModel deductibleSubjectsServiceModel = DelegateService.UnderwritingParamServiceWeb.GetDeductibleSubjects();
            if (deductibleSubjectsServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, deductibleSubjectsServiceModel.DeductibleSubjectServiceModels.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { deductibleSubjectsServiceModel.ErrorTypeService, deductibleSubjectsServiceModel.ErrorDescription });
            }
        }

        public ActionResult GetRateTypesDeduct()
        {
            try
            {
                var rateType = EnumsHelper.GetItems<RateType>();
                var types = rateType.Where(x => x.Value != "2");
                return new UifJsonResult(true, types);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryRateTypes);
            }
        }
    }
}