using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Helpers.Enums;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Company.Application.Location.LiabilityServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers
{
    public class ContractObjectController : Controller
    {
        // GET: Underwriting/ContractObject
        public ActionResult ContractObject()
        {
            return PartialView();

        }


        public UifJsonResult GetRiskByPolicyId(int policyId, int prefixId)
        {
            try
            {
                switch ((PrefixTypeMinPremium)prefixId)
                {
                    case PrefixTypeMinPremium.Lease:
                    case PrefixTypeMinPremium.Surety:
                        List<CompanyContract> companyContracts = DelegateService.suretyService.GetCompanySuretiesByPolicyId(policyId);
                        return new UifJsonResult(true, companyContracts);
                        break;
                    case PrefixTypeMinPremium.JudicialSurety:
                        List<CompanyJudgement> companyJudgements = DelegateService.judicialSuretyService.GetCompanyJudicialSuretyByPolicyId(policyId);
                        return new UifJsonResult(true, companyJudgements);
                        break;
                    case PrefixTypeMinPremium.Vehicle:
                        List<CompanyVehicle> vehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(policyId);
                        return new UifJsonResult(true, vehicles);
                        break;
                    case PrefixTypeMinPremium.Liability:
                        List<CompanyLiabilityRisk> companyLiabilityRisks = DelegateService.liabilityService.GetCompanyLiebilitiesByPolicyId(policyId);
                        return new UifJsonResult(true, companyLiabilityRisks);
                        break;
                    default:
                        break;
                }
                return null;
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public UifJsonResult SaveContractObjectPolicyId(int endorsementId, int riskId, string textRisk, string textPolicy)
        {
            try
            {
                var endorsement = DelegateService.underwritingService.SaveContractObjectPolicyId(endorsementId, riskId, textRisk, textPolicy);
                return new UifJsonResult(true, endorsement);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public UifJsonResult SaveLog(EndoChangeText endoChangeText)
        {
            try
            {
                Sistran.Company.Application.UnderwritingServices.Models.EndoChangeText endoChangeText1 = DelegateService.underwritingService.SaveLog(endoChangeText);
                return new UifJsonResult(true, endoChangeText1);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}