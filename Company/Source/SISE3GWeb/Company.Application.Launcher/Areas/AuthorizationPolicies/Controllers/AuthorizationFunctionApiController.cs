using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UniqueUserServices.Enums;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Controllers
{
    [Authorize]
    public class AuthorizationFunctionApiController : ApiController
    {
        [HttpGet]
        public IHttpActionResult CreatePolicyAuthorization([FromUri]int TemporalId)
        {
            string notification = string.Empty;
            CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(TemporalId, false);
            CompanyPolicyResult companyPolicyResult = new CompanyPolicyResult();

            if (policy != null)
            {
                companyPolicyResult = CreatePolicyEndorsement(policy);

                notification = $"{policy.Branch.Description} - {policy.Prefix.Description} - {companyPolicyResult.DocumentNumber} - {companyPolicyResult.EndorsementNumber}";

                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TemporalId.ToString(), null, companyPolicyResult.EndorsementId.ToString());

                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = policy.UserId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.Emission },
                    Message = notification,
                    Parameters = new Dictionary<string, object>
                                        {
                                            { "BranchId", policy.Branch.Id },
                                            { "PrefixId", policy.Prefix.Id },
                                            { "PolicyNumber", companyPolicyResult.DocumentNumber },
                                            { "EndorsementId", companyPolicyResult.EndorsementId },
                                            { "ProductIsCollective", policy.Product.IsCollective }
                                        }
                };

                UserPerson person = DelegateService.uniqueUserService.GetPersonByUserId(policy.UserId);

                if (person.Emails.Any())
                {
                    string strAddress = person.Emails[0].Description;

                    EmailCriteria email = new EmailCriteria
                    {
                        Addressed = new List<string> { strAddress },
                        Message = "<h3>Todas las politicas fueron autorizadas</h3>" + companyPolicyResult.DocumentNumber,
                        Subject = "Politicas autorizadas - " + TemporalId
                    };

                    DelegateService.AuthorizationPoliciesService.SendEmail(email);
                }

                DelegateService.uniqueUserService.CreateNotification(notificationUser);

                return Ok();
            }

            return NotFound();
        }

        private CompanyPolicyResult CreatePolicyEndorsement(CompanyPolicy policy)
        {
            switch (policy.Product.CoveredRisk.SubCoveredRiskType)
            {
                case SubCoveredRiskType.Vehicle:
                    return DelegateService.vehicleService.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, true);
                //case SubCoveredRiskType.ThirdPartyLiability:
                //    break;
                //case SubCoveredRiskType.Property:
                //    break;
                //case SubCoveredRiskType.Liability:
                //    break;
                case SubCoveredRiskType.Surety:
                    return CreateEndorsementSurety(policy);
                //case SubCoveredRiskType.JudicialSurety:
                //    break;
                default:
                    return null;
            }
        }

        private CompanyPolicyResult CreateEndorsementSurety(CompanyPolicy companyPolicy)
        {
            if (companyPolicy.Endorsement.EndorsementType == EndorsementType.LastEndorsementCancellation)
            {
                CompanyEndorsement companyEndorsement = new CompanyEndorsement
                {
                    Id = companyPolicy.Endorsement.Id,
                    PolicyId = companyPolicy.Endorsement.PolicyId,
                    EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId,
                    TemporalId = companyPolicy.Id,
                    TicketDate = companyPolicy.Endorsement.TicketDate,
                    TicketNumber = companyPolicy.Endorsement.TicketNumber,
                    Text = companyPolicy.Endorsement.Text
                };

                companyPolicy = DelegateService.suretyReversionEndorsement.CreateEndorsementReversion(companyEndorsement, true);

                return new CompanyPolicyResult
                {
                    DocumentNumber = companyPolicy.DocumentNumber,
                    EndorsementId = companyPolicy.Endorsement.Id,
                    EndorsementNumber = companyPolicy.Endorsement.Number
                };
            }
            else
            {
                return DelegateService.suretyService.CreateCompanyPolicy(companyPolicy.Id, (int)companyPolicy.TemporalType, true);
            }
        }
    }
}