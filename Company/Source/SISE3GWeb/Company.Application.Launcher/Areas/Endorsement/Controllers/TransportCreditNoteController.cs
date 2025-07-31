using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.DTOs;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class TransportCreditNoteController : EndorsementBaseController
    {
        #region View
        public ActionResult TransportCreditNote()
        {
            return PartialView();
        }
        // GET: Endorsement/TransportCreditNote
        //public ActionResult TransportCreditNote(CreditNoteDTO creditNote)
        //{
        //    ModelState.Clear();
        //    if (creditNote != null && creditNote.PolicyId > 0)
        //    {
        //        Sistran.Company.Application.Transports.TransportApplicationService.DTOs.EndorsementDTO endorsement = new Sistran.Company.Application.Transports.TransportApplicationService.DTOs.EndorsementDTO();
        //        endorsement = DelegateService.transportAdjustmentApplicationService.GetTemporalEndorsementByPolicyId(creditNote.PolicyId);
        //        if (endorsement == null)
        //        {
        //            return View(creditNote);
        //        }
        //        else
        //        {
        //            if (endorsement.EndorsementType == EndorsementType.CreditNoteEndorsement)
        //            {
        //                creditNote.TemporalId = endorsement.TemporalId;
        //                return View(creditNote);
        //            }
        //            else
        //            {
        //                SearchViewModel searchViewModel = new SearchViewModel();
        //                searchViewModel.BranchId = creditNote.BranchId;
        //                searchViewModel.PrefixId = creditNote.PrefixId;
        //                searchViewModel.PolicyNumber = creditNote.PolicyNumber.ToString();
        //                searchViewModel.EndorsementId = creditNote.EndorsementId;
        //                searchViewModel.TemporalId = endorsement.TemporalId;
        //                return RedirectToAction("Search", "Search", searchViewModel);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //}

        #endregion

        public ActionResult GetEndorsementsByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber, bool current)
        {
            try
            {
                List<CompanyEndorsement> endorsements = DelegateService.underwritingService.GetCiaEndorsementsByFilterPolicy(branchId, prefixId, policyNumber, current);
                var selectedId = 0;

                if (endorsements != null)
                {
                    endorsements.Where(x => x.IsCurrent).AsParallel().ForAll(endorsement =>
                    {
                        endorsement.Description = App_GlobalResources.Language.ActualState;
                        selectedId = endorsement.Id;
                    });

                    return new UifJsonResult(true, new { endorsements, selectedId });
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.PolicyNotFound);
                }
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorQueryEndorsement);
            }
        }

        public ActionResult GetEndorsmentsWithPremium(int policyId)
        {
            try
            {
                List<Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.DTOs.EndorsementDTO> endorsements =
                    DelegateService.transportCreditNoteApplicationService.GetEndorsementsWithPremiumAmount(policyId);

                if (endorsements != null)
                {
                    return new UifJsonResult(true, endorsements);
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPolicyType);
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorQueryEndorsement);
            }
        }

        public ActionResult GetRiksByPolicyIdByEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                List<Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.DTOs.EndorsementRiskDTO> risks =
                    DelegateService.transportCreditNoteApplicationService.GetRisksByPolicyIdEndorsementId(policyId, endorsementId);

                if (risks != null)
                {
                    return new UifJsonResult(true, risks);
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPolicyType);
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorQueryEndorsement);
            }
        }


        public ActionResult GetEndorsementType(int policyId)
        {
            try
            {
                //List<EndorsementTypeDTO> endorsementType = DelegateService.transportCreditNoteApplicationService.GetAvaibleEndorsementsByPolicyId(policyId);
                //if (endorsementType != null)
                //{
                //    return new UifJsonResult(true, endorsementType.OrderBy(x => x.Description).ToList());
                //}
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPolicyType);
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorQueryEndorsement);
            }
        }

        public ActionResult GetTransportsByPolicyIdByEndorsementId(int policyId, int endorsementId)

        {
            try
            {
                RiskDTO endorsement = DelegateService.transportCreditNoteApplicationService.GetTransportsByPolicyIdByEndorsementId(policyId, endorsementId);
                return new UifJsonResult(true, endorsement);
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorDocumentControlRisks);
            }
        }

        public ActionResult GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            try
            {
                CreditNoteDTO coverage = DelegateService.transportCreditNoteApplicationService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId);
                coverage.CoverageDTOs = coverage.CoverageDTOs.Where(x => x.PremiumAmount > 0).ToList();
                return new UifJsonResult(true, coverage);
            }
            catch (Exception)
            { 
                return new UifSelectResult(App_GlobalResources.Language.ErrorConsultingCoverages);
            }
        }

        public ActionResult CreateTemporal(CreditNoteDTO creditNoteModel)
        {
            try
            {
                var policy = DelegateService.transportCreditNoteApplicationService.CreateTemporal(creditNoteModel);
                return new UifJsonResult(true, policy);
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }

        public ActionResult GetTemporalId(int temporalId)
        {
            try
            {
                var policy = DelegateService.transportCreditNoteApplicationService.GetTemporalById(temporalId, false);
                return new UifJsonResult(true, policy);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }

        public ActionResult GetMaximumPremiumPercetToReturn(int policyId)
        {
            try
            {
                var percent = DelegateService.transportCreditNoteApplicationService.GetMaximumPremiumPercetToReturn(policyId);
                return new UifJsonResult(true, percent);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetMaximumPremiumPercetToReturn);
            }
        }
    }

}


