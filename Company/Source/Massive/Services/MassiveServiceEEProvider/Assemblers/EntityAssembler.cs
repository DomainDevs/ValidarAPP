using Sistran.Company.Application.MassiveServices.EEProvider.Entities;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Company.Application.Request.Entities;
using COMMEN = Sistran.Company.Application.Common.Entities;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.MassiveServices.EEProvider.Assemblers
{
    public static class EntityAssembler
    {
        public static CoRequest CreateCoRequest(Models.CompanyRequest companyRequest)
        {
            return new CoRequest()
            {
                BillingGroupCode = companyRequest.BillingGroup.Id,
                BranchCode = companyRequest.Branch.Id,
                Description = companyRequest.Description,
                PrefixCode = companyRequest.Prefix.Id,
                RequestDate = companyRequest.RequestDate
            };
        }

        public static CoRequestAgent CreateCoRequest(IssuanceAgency agency)
        {
            return new CoRequestAgent()
            {
                AgentAgencyId = agency.Id,
                IndividualId = agency.Agent.IndividualId,
                ParticipationPercentage = agency.Participation,
                IsPrimary = agency.IsPrincipal
            };
        }

        
        
        public static CoRequestEndorsementAgent CreateCoRequestEndorsementAgent(int requestAgentId, int requestEndorsmentId)
        {
            return new CoRequestEndorsementAgent(requestAgentId, requestEndorsmentId)
            {
                RequestAgentId = requestAgentId,
                RequestEndorsementId = requestEndorsmentId
            };
        }

        #region CoRequestEndorsementAgent
        /// <summary>
        /// Crea un Entity.CoRequestEndorsement a partir de un model.CoRequestEndorsement
        /// </summary>
        /// <param name="coRequestEndorsement">Models.coRequestEndorsement</param>
        /// <param name="userId">Identificador del user</param>
        /// <returns></returns>
        public static CoRequestEndorsement CreateCoRequestEndorsement(Models.CompanyRequestEndorsement companyRequestEndorsement)
        {
            return new CoRequestEndorsement()
            {
                AmtGift = companyRequestEndorsement.GiftAmount,
                Annotations = companyRequestEndorsement.Annotations,
                CurrentFrom = companyRequestEndorsement.CurrentFrom,
                CurrentTo = companyRequestEndorsement.CurrentTo,
                DocumentNum = companyRequestEndorsement.DocumentNumber,
                EndorsementDate = companyRequestEndorsement.EndorsementDate,
                EndoTypeCode = (int)companyRequestEndorsement.EndorsementType,
                IsOpenEffect = companyRequestEndorsement.IsOpenEffect,
                IssueExpensesAmount = companyRequestEndorsement.IssueExpensesAmount,
                MonthlyPayDay = companyRequestEndorsement.MonthPayerDay,
                PaymentScheduleId = companyRequestEndorsement.PaymentPlan != null ? companyRequestEndorsement.PaymentPlan.Id : 0,
                PolicyHolderId = companyRequestEndorsement.Holder.IndividualId,
                PolicyTypeCode = companyRequestEndorsement.PolicyType.Id,
                PrefixCode = companyRequestEndorsement.Prefix.Id,
                ProductId = companyRequestEndorsement.Product.Id,
                UserId = companyRequestEndorsement.UserId
            };
        }

        #endregion

        #region CreateCoRequestCoinsuranceAssigned
        /// <summary>
        /// Crea un Entity.CoRequestCoinsuranceAssigned a partir de un requestAgentId y requestEndorsmentId
        /// </summary>
        /// <param name="requestAgentId">Identificador del agente</param>
        /// <param name="requestEndorsmentId">Identificador del endoso</param>
        /// <returns></returns>
        public static Core.Application.Request.Entities.CoRequestCoinsuranceAssigned CreateCoRequestCoinsuranceAssigned(IssuanceCoInsuranceCompany coInsuranceCompany, int requestId, int requestEndorsmentId)
        {

            return new Core.Application.Request.Entities.CoRequestCoinsuranceAssigned(requestEndorsmentId, requestId, coInsuranceCompany.Id)
            {

                ExpensesPercentage = coInsuranceCompany.ExpensesPercentage,
                InsuranceCompanyId = coInsuranceCompany.Id,
                PartCiaPercentage = coInsuranceCompany.ParticipationPercentage
            };
        }
        #endregion
        #region CoRequestCoinsuranceAccepted
        /// <summary>
        /// Crea un Entity.CoRequestCoinsuranceAssigned a partir de un requestAgentId y requestEndorsmentId
        /// </summary>
        /// <param name="requestAgentId">Identificador del agente</param>
        /// <param name="requestEndorsmentId">Identificador del endoso</param>
        /// <returns></returns>
        public static Core.Application.Request.Entities.CoRequestCoinsuranceAccepted CreateCoRequestCoinsuranceAccepted(IssuanceCoInsuranceCompany coInsuranceCompany, int requestId, int requestEndorsmentId)
        {

            return new Core.Application.Request.Entities.CoRequestCoinsuranceAccepted(requestEndorsmentId, requestId, coInsuranceCompany.Id)
            {

                ExpensesPercentage = coInsuranceCompany.ExpensesPercentage,
                InsuranceCompanyId = coInsuranceCompany.Id,
                PartCiaPercentage = coInsuranceCompany.ParticipationPercentage,
                PolicyNumMain = coInsuranceCompany.PolicyNumber,
                AnnexNumMain = coInsuranceCompany.EndorsementNumber,
                PartMainPercentage = coInsuranceCompany.ParticipationPercentageOwn
            };
        }
        #endregion

    }
}
