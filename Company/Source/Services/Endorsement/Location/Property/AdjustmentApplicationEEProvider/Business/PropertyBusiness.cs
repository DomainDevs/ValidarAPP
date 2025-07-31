//using Sistran.Company.Application.AdjustmentApplicationService.DTO;
using Sistran.Company.Application.AdjustmentApplicationService.DTO;
using Sistran.Company.Application.AdjustmentApplicationServiceEEProvider.Assemblers;
using PROP = Sistran.Company.Application.Location.PropertyServices.DTO;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Location.PropertyServices.Models;

namespace Sistran.Company.Application.AdjustmentApplicationServiceEEProvider.Business
{
    public class PropertyBusiness
    {
        public AdjustmentDTO CreateTemporal(AdjustmentDTO adjustmentDTO)
        {
            AdjustmentDTO policy = new AdjustmentDTO();
            CompanyPolicy companyPolicy = ModelAssembler.CreateCompanyPolicy(adjustmentDTO);
            companyPolicy = DelegateService.adjustmentBusinessService.CreateEndorsementAdjustment(companyPolicy, ModelAssembler.CreateFormValues(adjustmentDTO));
            return DTOAssembler.CreateAjusmentsumary(companyPolicy);
        }

        public AdjustmentDTO GetPropertyRiskByPolicyId(int policyId, string currentFrom)
        {
            AdjustmentDTO AdjustmentEndorsement = new AdjustmentDTO();
            // list riesgos 
            List<CompanyPropertyRisk> propertyRisks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(policyId);
            //int declarationPeriodId = (int)propertyRisks[0].DeclarationPeriodId;
            //int billingPeriodId = (int)propertyRisks[0].BillingPeriodId;
            //string declarationDescription = propertyRisks[0].DeclarationDescription;
            //string billingDescription = propertyRisks[0].BillingDescription;
            List<EndorsementDTO> endorsements = new List<EndorsementDTO>();
            List<EndorsementDTO> endorsementsDaclaration = new List<EndorsementDTO>();
            DateTime currentfrom = DateTime.Parse(currentFrom);
            List<EndorsementDTO> Endorsements = ModelAssembler.ChangeEndorsementDTOs(DelegateService.propertyService.GetEndorsementByEndorsementTypeIdPolicyId((int)EndorsementType.AdjustmentEndorsement, policyId));
            if (Endorsements.Count > 0)
            {
                Endorsements = Endorsements.OrderByDescending(f => f.IdEndorsement).ToList();
                endorsements.Add(Endorsements[0]);
            }
            endorsementsDaclaration = ModelAssembler.ChangeEndorsementDTOs(DelegateService.propertyService.GetEndorsementByEndorsementTypeIdPolicyId((int)EndorsementType.DeclarationEndorsement, policyId));
            if (endorsementsDaclaration.Count > 0)
            {
                int quatityDeclaration = endorsementsDaclaration.Count;
                int Moths = currentfrom.Month - DateTime.Today.Month;
                //if (quatityDeclaration > 0)
                //{
                //    if (endorsements.Count > 0)
                //    {
                //        AdjustmentEndorsement = CalculateDays(endorsements[0].CurrentFrom.ToString(), billingPeriodId);
                //    }
                //    else
                //    {
                //        AdjustmentEndorsement = CalculateDays(currentFrom, billingPeriodId);
                //    }
                //}
                AdjustmentEndorsement.Risks = DTOAssembler.CreateAjusmentRisks(propertyRisks);
            }

            return AdjustmentEndorsement;

        }

        public AdjustmentDTO CalculateDays(string CurrentFrom, int BillingPeriodId)
        {
            AdjustmentDTO adjusment = new AdjustmentDTO();
            DateTime currentFrom = Convert.ToDateTime(CurrentFrom);
            DateTime currentTo = new DateTime();

            if (BillingPeriodId == 6)
            {
                currentTo = currentFrom.AddMonths(12);
            }
            if (BillingPeriodId == 5)
            {
                currentTo = currentFrom.AddMonths(6);
            }
            if (BillingPeriodId == 4)
            {
                currentTo = currentFrom.AddMonths(4);
            }
            if (BillingPeriodId == 3)
            {
                currentTo = currentFrom.AddMonths(3);
            }
            if (BillingPeriodId == 2)
            {
                currentTo = currentFrom.AddMonths(2);
            }
            if (BillingPeriodId == 1)
            {
                currentTo = currentFrom.AddMonths(1);
            }
            TimeSpan timeSpan = currentTo - currentFrom;
            adjusment.Days = timeSpan.Days;
            adjusment.CurrentTo = Convert.ToDateTime(currentTo);
            adjusment.CurrentFrom = Convert.ToDateTime(currentFrom);
            return adjusment;
        }

        public AdjustmentDetailsDTO GetEndorsementByEndorsementTypeDeclarationPolicyId(int policyId, int riskId)
        {
            int endorsenmentid = 0;
            AdjustmentDetailsDTO details = new AdjustmentDetailsDTO();
            List<DeclarationDTO> detailsDeclaration = new List<DeclarationDTO>();
            List<EndorsementDTO> EndorsementsDeclaration = ModelAssembler.ChangeEndorsementDTOs(DelegateService.propertyService.GetEndorsementByEndorsementTypeIdPolicyId((int)EndorsementType.DeclarationEndorsement, policyId));
            foreach (var Endorsement in EndorsementsDeclaration)
            {
                List<CompanyCoverage> companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, Endorsement.IdEndorsement, riskId);
                details.PremiumDepositAmount = companyCoverages[0].PremiumAmount;
                details.HasPremiumDeposit = companyCoverages[0].IsMinPremiumDeposit;
                DeclarationDTO Declaration = new DeclarationDTO();
                Declaration.DeclaredAmount = companyCoverages[0].DeclaredAmount;
                Declaration.Description = companyCoverages[0].EndorsementType.ToString();
                detailsDeclaration.Add(Declaration);
            }
            details.DeclarationEndorsements = detailsDeclaration;
            return details;
        }

        public EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {
            EndorsementDTO endorsement = ModelAssembler.ChangeEndorsementDTO(DelegateService.propertyService.GetTemporalEndorsementByPolicyId(policyId));
            return endorsement;
        }

        public AdjustmentDTO GetPropertyRiskByTemporalId(int temporalId, bool isMasive)
        {
            return DTOAssembler.CreateAjusmentsumary(DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, isMasive));
        }

        public List<PROP.InsuredObjectDTO> GetInsuredObjectsByRiskId(int riskId)
        {
            return DTOAssembler.CreateInsuredObjects(DelegateService.underwritingService.GetInsuredObjectsByRiskId(riskId));
        }

        public AdjustmentDTO GetAdjustmentEndorsementByPolicyId(int policyId)
        {
            AdjustmentDTO adjustment = DTOAssembler.CreateAdjustmentDTO(DelegateService.propertyService.GetNextAdjustmentEndorsementByPolicyId(policyId));
            adjustment.Days = CalculateDays(adjustment.CurrentFrom, adjustment.CurrentTo);
            return adjustment;
        }
        public int CalculateDays(DateTime from, DateTime to)
        {
            TimeSpan timeSpan = to - from;
            int days = timeSpan.Days;
            return days;
        }

        public AdjustmentDTO GetRisksByTemporalId(int temporalId, bool isMasive)
        {
            return DTOAssembler.CreateAjusmentsumary(DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, isMasive));
        }


    }
}
