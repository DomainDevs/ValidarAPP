using Sistran.Company.Application.AdjustmentApplicationService.DTO;
using PROP = Sistran.Company.Application.Location.PropertyServices.DTO;

using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.AdjustmentApplicationServiceEEProvider.Assemblers
{
    public static class DTOAssembler
    {
        public static AdjustmentDTO CreateAjusmentsumary(CompanyPolicy policy)
        {
            if (policy == null)
            {
                return null;
            }
            return new AdjustmentDTO()
            {

                PolicyId = policy.Id,
                PrefixId = policy.Prefix.Id,
                BranchId = policy.Branch.Id,
                Sum = policy.Summary.AmountInsured,
                Premiun = policy.Summary.Premium,
                Expense = policy.Summary.Expenses,
                //Surcharge = policy.Summary.Surcharges,
                //Discount = policy.Summary.Discounts,
                TicketNumber = Convert.ToInt32(policy.Endorsement.TicketNumber),
                TicketDate = Convert.ToDateTime(policy.Endorsement.TicketDate),
                Tax = policy.Summary.Taxes,
                TotalPremiun = policy.Summary.FullPremium,
                Days = policy.Endorsement.EndorsementDays,
                CurrentFrom = policy.Endorsement.CurrentFrom,
                CurrentTo = policy.Endorsement.CurrentTo,
                TemporalId = policy.Endorsement.TemporalId,
                Text = policy.Endorsement.Text.TextBody,
                Observation = policy.Endorsement.Text.Observations,
                RiskCount = policy.Summary.RiskCount
            };
        }

        public static List<CompanyPropertyRisk> CreateAjusmentRisks(List<CompanyPropertyRisk> propertyRisks)
        {
            if (propertyRisks == null)
            {
                return null;
            }

            List<CompanyPropertyRisk> PropertyRisksList = new List<CompanyPropertyRisk>();
            foreach (
                var property in propertyRisks)
            {
                CompanyPropertyRisk propertyRisk = new CompanyPropertyRisk();
                propertyRisk.Risk.RiskId = property.Risk.RiskId;
                propertyRisk.Risk.Description = property.Risk.Description;
                propertyRisk.AssuranceMode = property.AssuranceMode;
                PropertyRisksList.Add(propertyRisk);
            }

            return PropertyRisksList;
        }

        public static List<PROP.InsuredObjectDTO> CreateInsuredObjects(List<InsuredObject> InsuredObjects)
        {
            List<PROP.InsuredObjectDTO> insuredObjectDTO = new List<PROP.InsuredObjectDTO>();
            if (InsuredObjects != null)
            {
                foreach (var insuredObject in InsuredObjects)
                {
                    insuredObjectDTO.Add(CreateInsuredObject(insuredObject));
                }
            }
            return insuredObjectDTO;
        }

        public static PROP.InsuredObjectDTO CreateInsuredObject(InsuredObject InsuredObject)
        {
            if (InsuredObject == null)
            {
                return null;
            }
            return new PROP.InsuredObjectDTO
            {
                Id = InsuredObject.Id,
                Description = InsuredObject.Description,
                InsuredLimitAmount = InsuredObject.Amount,
                PremiumAmount = InsuredObject.Premium,
                IsSelected = InsuredObject.IsSelected,
                IsMandatory = InsuredObject.IsMandatory,
                IsDeclarative = InsuredObject.IsDeclarative
            };
        }

        public static AdjustmentDTO CreateAdjustmentDTO(PROP.EndorsementDTO endorsement)
        {
            return new AdjustmentDTO
            {
                CurrentFrom = endorsement.CurrentFrom,
                CurrentTo = endorsement.CurrentTo
            };
        }
    }
}
