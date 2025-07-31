using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.Text.RegularExpressions;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class SuretyModificationController : ModificationController
    {
        public ActionResult CreateTemporal(ModificationViewModel modificationModel)
        {
            UnderwritingController underwritingController = new UnderwritingController();
            try
            {
                if (modificationModel.BusinessTypeDescription != (int)BusinessType.Accepted)
                    ModelState["modificationModel.BusinessTypeDescription"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(modificationModel);
                    if (!string.IsNullOrEmpty(CompanyEndorsement.Text.TextBody))
                        CompanyEndorsement.Text.TextBody = underwritingController.unicode_iso8859(CompanyEndorsement.Text.TextBody);
                    CompanyEndorsement.UserId = SessionHelper.GetUserId();
                    var policy = DelegateService.suretyModificationServiceCia.CreateTemporal(CompanyEndorsement, false);
                    return new UifJsonResult(true, policy);

                }
                else
                {
                    string errorMessage = GetErrorMessages();
                    return new UifJsonResult(false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                }

            }
        }
        public ActionResult CreateTexts(ModificationViewModel modificationModel, CompanyModification companyModification)

        {
            Company.Application.UnderwritingServices.Models.CompanyPolicyResult companyPolicy = null;
            UnderwritingController underwritingController = new UnderwritingController();
            var UifJsonResult = new UifJsonResult(false, null);
            if (string.IsNullOrEmpty(modificationModel.Text))
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorTextEmpty);
            }
            try
            {
                modificationModel.UserId = SessionHelper.GetUserId();
                var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(modificationModel);

                if (!string.IsNullOrEmpty(CompanyEndorsement.Text.TextBody))
                    CompanyEndorsement.Text.TextBody = underwritingController.unicode_iso8859(CompanyEndorsement.Text.TextBody);

                companyPolicy = DelegateService.suretyTextServiceCia.CreateTexts(CompanyEndorsement, companyModification);
                if (companyPolicy.Errors != null && companyPolicy.Errors.Any())
                {
                    UifJsonResult = new UifJsonResult(companyPolicy.Errors.First().StateData, companyPolicy.Errors.First().Error);
                }
                else if (companyPolicy.InfringementPolicies != null && companyPolicy.InfringementPolicies.Count > 0)
                {
                    UifJsonResult = new UifJsonResult(true, companyPolicy);
                }
                else
                {
                    UifJsonResult = new UifJsonResult(true, companyPolicy);
                }
                return UifJsonResult;

            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavingTextsComments);
                }

            }

        }


        public ActionResult CreateClauses(ModificationViewModel modificationModel, CompanyModification companyModification)
        {
            try
            {
                var UifJsonResult = new UifJsonResult(false, null);
                CompanyPolicy policy = ModelAssembler.CreateCompanyPolicy(modificationModel);
                policy.UserId = Helpers.SessionHelper.GetUserId();
                CompanyPolicyResult companyPolicy = DelegateService.suretyClauseServiceCia.CreateClauses(policy, companyModification);
                if (companyPolicy != null)
                {
                    if (companyPolicy.Errors != null && companyPolicy.Errors.Any())
                    {
                        UifJsonResult = new UifJsonResult(companyPolicy.Errors.First().StateData, companyPolicy.Errors.First().Error);
                    }
                    else
                    {
                        UifJsonResult = new UifJsonResult(true, companyPolicy);
                    }
                }
                else
                {
                    UifJsonResult = new UifJsonResult(false, App_GlobalResources.Language.SelectAllRequiredClauses);
                }
                return UifJsonResult;


            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveClauses);
                }

            }
        }

        //public ActionResult CreateEndorsement(int temporalId, decimal policyNumber, ModificationUpdateViewModel modificationUpdateViewModel)
        //{
        //    try
        //    {
        //        EndorsementBaseController endorsementBaseController = new EndorsementBaseController();
        //        if (temporalId > 0)
        //        {
        //            string message = endorsementBaseController.ValidateEndorsement(temporalId);

        //            if (message == "")
        //            {
        //                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
        //                if (policy.UserId == 0)
        //                {
        //                    policy.UserId = SessionHelper.GetUserId();
        //                }
        //                policy.Endorsement.Text.TextBody = modificationUpdateViewModel.Text;
        //                policy.Endorsement.Text.Observations = modificationUpdateViewModel.Observations;
        //                policy.Endorsement.TicketDate = modificationUpdateViewModel.RegistrationDate;
        //                policy.Endorsement.TicketNumber = modificationUpdateViewModel.RegistrationNumber;

        //                CompanyPolicyResult companyPolicyResult = CreateCompanyPolicyResult(policy);
        //                /*CreatePolicyEndorsement(policy);*/
        //                if (companyPolicyResult.Errors != null && companyPolicyResult.Errors.Any())
        //                {
        //                    return new UifJsonResult(companyPolicyResult.Errors.First().StateData, companyPolicyResult.Errors.First().Error);
        //                }
        //                else if (companyPolicyResult.InfringementPolicies != null && companyPolicyResult.InfringementPolicies.Count > 0)
        //                {
        //                    return new UifJsonResult(true, companyPolicyResult.InfringementPolicies);
        //                }
        //                else
        //                {
        //                    string additionalFinancing = "";
        //                    if (policy.PaymentPlan.PremiumFinance != null && companyPolicyResult.PromissoryNoteNumCode > 0)
        //                    {
        //                        additionalFinancing = App_GlobalResources.Language.LabelPromissoryNote + ": " + companyPolicyResult.PromissoryNoteNumCode + ". ID " + App_GlobalResources.Language.LabelUser + ": " + policy.User.UserId;
        //                    }
        //                    message = App_GlobalResources.Language.SuccessfullyEndorsementGenerated + "." + App_GlobalResources.Language.LabelNumberPolicy + ": " + policyNumber.ToString() + "." + App_GlobalResources.Language.EndorsementNumber + ": " + companyPolicyResult.EndorsementNumber.ToString() + ". Endorsement Id" + ": " + companyPolicyResult.EndorsementId.ToString() + ". " + additionalFinancing;
        //                    return new UifJsonResult(true, message);
        //                }
        //            }
        //            else
        //            {
        //                return new UifJsonResult(false, message);
        //            }
        //        }
        //        else
        //        {
        //            return new UifJsonResult(false, App_GlobalResources.Language.NoExistTemporaryEmit);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new UifJsonResult(false, ex.Message);
        //    }
        //}
    }
}