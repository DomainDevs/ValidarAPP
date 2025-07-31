using Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.DTOs;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class TransportAdjustmentController : EndorsementBaseController
    {
        #region View
        public ActionResult TransportAdjustment(AdjustmentDTO adjustment)
        {
            ModelState.Clear();
            if (adjustment != null && adjustment.PolicyId > 0)
            {
                SearchViewModel searchViewModel;
                EndorsementDTO endorsement = DelegateService.transportAdjustmentApplicationService.GetTemporalEndorsementByPolicyId(adjustment.PolicyId);
                if (endorsement != null)
                {
                    if (endorsement.EndorsementType == EndorsementType.AdjustmentEndorsement)
                    {
                        adjustment.TemporalId = endorsement.TemporalId;
                        return View(adjustment);
                    }
                    else
                    {
                        searchViewModel = new SearchViewModel();
                        searchViewModel.BranchId = adjustment.BranchId;
                        searchViewModel.PrefixId = adjustment.PrefixId;
                        searchViewModel.PolicyNumber = adjustment.PolicyNumber.ToString();
                        searchViewModel.EndorsementId = adjustment.EndorsementId;
                        searchViewModel.TemporalId = endorsement.TemporalId;
                        //searchViewModel.ProductIsCollective = adjustment.ProductIsCollective;
                        return RedirectToAction("Search", "Search", searchViewModel);
                    }
                }
                else
                {
                    List<Application.UnderwritingServices.Models.Endorsement> endorsementsByPoliciId = DelegateService.underwritingService.GetEndorsementsContainByPolicyId(adjustment.PolicyId);
                    if (endorsementsByPoliciId.Count > 0)
                    {
                        if (endorsementsByPoliciId.Any(e => e.EndorsementType == EndorsementType.DeclarationEndorsement))
                        {
                            adjustment.TemporalId = endorsementsByPoliciId.Where(e => e.EndorsementType == EndorsementType.DeclarationEndorsement).FirstOrDefault().TemporalId;
                            if (endorsementsByPoliciId.Any(e => e.EndorsementType == EndorsementType.AdjustmentEndorsement))
                            {
                                adjustment.CurrentTo = endorsementsByPoliciId.Where(e => e.EndorsementType == EndorsementType.AdjustmentEndorsement).FirstOrDefault().CurrentTo;
                                adjustment.CurrentFrom = endorsementsByPoliciId.Where(e => e.EndorsementType == EndorsementType.AdjustmentEndorsement).FirstOrDefault().CurrentFrom;
                            }
                            else
                            {
                                adjustment.CurrentTo = endorsementsByPoliciId.Where(e => e.EndorsementType == EndorsementType.Emission).FirstOrDefault().CurrentTo;
                                adjustment.CurrentFrom = endorsementsByPoliciId.Where(e => e.EndorsementType == EndorsementType.Emission).FirstOrDefault().CurrentFrom;
                            }
                            return View(adjustment);
                        }
                        else
                        {
                            searchViewModel = new SearchViewModel();
                            searchViewModel.BranchId = adjustment.BranchId;
                            searchViewModel.PrefixId = adjustment.PrefixId;
                            searchViewModel.PolicyNumber = adjustment.PolicyNumber.ToString();
                            searchViewModel.EndorsementId = adjustment.EndorsementId;
                            //searchViewModel.ProductIsCollective = adjustment.ProductIsCollective;
                            searchViewModel.Message = App_GlobalResources.Language.ErrorGetEndorsementDeclaration;
                            return RedirectToAction("Search", "Search", searchViewModel);
                        }
                    }
                    else
                    {
                        searchViewModel = new SearchViewModel();
                        searchViewModel.BranchId = adjustment.BranchId;
                        searchViewModel.PrefixId = adjustment.PrefixId;
                        searchViewModel.PolicyNumber = adjustment.PolicyNumber.ToString();
                        searchViewModel.EndorsementId = adjustment.EndorsementId;
                        //searchViewModel.ProductIsCollective = adjustment.ProductIsCollective;
                        return RedirectToAction("Search", "Search", searchViewModel);
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult DetailAdjustment()
        {
            return View();
        }
        #endregion

        public ActionResult GetRisksByPolicyId(int policyId, string currentFrom)
        {
            try
            {
                List<TransportDTO> risks = DelegateService.transportApplicationService.GetTransportsByPolicyId(policyId);
                return new UifJsonResult(true, risks);
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorQueryEndorsement);
            }
        }

        public ActionResult GetInsuredObjectsByRiskId(int riskId)
        {
            try
            {
                List<InsuredObjectDTO> insuredObjects = new List<InsuredObjectDTO>();
                insuredObjects = DelegateService.transportAdjustmentApplicationService.GetInsuredObjectsByRiskId(riskId);
                return new UifJsonResult(true, insuredObjects);
            }
            catch (Exception)
            {
                return new UifSelectResult(false, App_GlobalResources.Language.ErrorGetInsuredObject);
            }
        }

        public ActionResult CalculateDays(string inputFrom, string inputTo, int billingPeriodId)
        {
            try
            {
                AdjustmentDTO endorsement = DelegateService.transportAdjustmentApplicationService.CalculateDays(inputFrom, inputTo, billingPeriodId);
                return new UifJsonResult(true, endorsement);
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorCalculateDays);
            }

        }

        public ActionResult GetEndorsementByEndorsementTypeDeclarationPolicyId(int policyId, int riskId)
        {
            try
            {
                AdjustmentDetailsDTO adjustmentDetails = DelegateService.transportAdjustmentApplicationService.GetEndorsementByEndorsementTypeDeclarationPolicyId(policyId, riskId);

                return new UifJsonResult(true, adjustmentDetails.DeclarationEndorsements);
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorGetCoverage);
            }

        }

        public ActionResult CreateTemporal(AdjustmentDTO adjustmentModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var policy = DelegateService.transportAdjustmentApplicationService.CreateTemporal(adjustmentModel);
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, "");
                }
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }

        public ActionResult GetRisksByTemporalId(int temporalId)
        {
            try
            {
                var policy = DelegateService.transportAdjustmentApplicationService.GetTransportsByTemporalId(temporalId, false);
                return new UifJsonResult(true, policy);

            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }

        public ActionResult GetAdjustmentEndorsementByPolicyId(int policyId)
        {
            try
            {
                AdjustmentDTO adjustment = DelegateService.transportAdjustmentApplicationService.GetAdjustmentEndorsementByPolicyId(policyId);
                return new UifJsonResult(true, adjustment);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAdjustmentEndorsementByPolicyId);
            }
        }
        
        public ActionResult CanMakeEndorsement(int policyId)
        {
            try
            {
                Dictionary<string, object> resulValidation = new Dictionary<string, object>();
                var makeEndrsement = DelegateService.transportApplicationService.CanMakeEndorsement(policyId, out resulValidation);
                resulValidation.Add("CanMakeEndorsement", makeEndrsement);
                return new UifJsonResult(true, resulValidation);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, false);
            }
        }

        public ActionResult EndorsementByRiskByInsuredObjectId(decimal policyId, decimal riskId, decimal insuredObjectId)
        {
            try
            {
                bool isAllow = DelegateService.transportApplicationService.CanMakeEndorsementByRiskByInsuredObjectId(policyId, riskId, insuredObjectId, EndorsementType.AdjustmentEndorsement);
                return new UifJsonResult(true, isAllow);
            }
            catch (Exception ex)
            {

                return new UifJsonResult(false, false);
            }

        }
    }
}