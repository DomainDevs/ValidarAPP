using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Finances.FidelityServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.Finances.FidelityServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        public static FidelityRisk CreateFidelityRisk(ISSEN.Risk risk, ISSEN.RiskFidelity riskFidelity, ISSEN.EndorsementRisk endorsementRisk)
        {
            FidelityRisk fidelityRisk = new FidelityRisk
            {
                Risk = new Risk
                {
                    Description = riskFidelity.Description,
                    RiskId = risk.RiskId,
                    CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode,
                    Number = endorsementRisk.RiskNum,
                    GroupCoverage = new GroupCoverage
                    {
                        Id = risk.CoverGroupId ?? 0,
                        CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode
                    },
                    Text = new Text
                    {
                        TextBody = risk.ConditionText
                    },
                    RiskActivity = new RiskActivity
                    {
                        Id = riskFidelity.RiskCommercialClassCode
                    },
                    Policy = new Policy
                    {
                        Id = endorsementRisk.PolicyId
                    },
                    OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
                    Status = RiskStatusType.NotModified,
                    DynamicProperties = new List<DynamicConcept>(),
                    MainInsured = new IssuanceInsured
                    {
                        IndividualId = risk.InsuredId,
                        CompanyName = new IssuanceCompanyName
                        {
                            NameNum = risk.NameNum.GetValueOrDefault(),
                            Address = new IssuanceAddress
                            {
                                Id = risk.AddressId.GetValueOrDefault()
                            }
                        }
                    },
                },
                IsDeclarative = risk.IsPersisted,
                IdOccupation = riskFidelity.OccupationCode,
                DiscoveryDate = riskFidelity.DiscoveryDate,
                Description = riskFidelity.Description
            };

            foreach (DynamicProperty item in risk.DynamicProperties)
            {
                DynamicProperty dynamicProperty = (DynamicProperty)item.Value;
                DynamicConcept dynamicConcept = new DynamicConcept();
                dynamicConcept.Id = dynamicProperty.Id;
                dynamicConcept.Value = dynamicProperty.Value;
                fidelityRisk.Risk.DynamicProperties.Add(dynamicConcept);
            }
            return fidelityRisk;
        }
    }
}
