using System.Collections.Generic;
using System.Linq;
using UWMO = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class CommissionDAO
    {
        /// <summary>
        /// Calcular Comisiones
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="risks">Riesgos</param>
        /// <returns>Agencias</returns>
        public List<UWMO.IssuanceAgency> CalculateCommissions(UWMO.Policy policy, List<UWMO.Risk> risks)
        {
            if (policy.Summary != null && policy.Summary.RiskCount > 0)
            {
                List<UWMO.IssuanceCommission> commissions = new List<UWMO.IssuanceCommission>();
                foreach (UWMO.Risk risk in risks)
                {
                    foreach (UWMO.Coverage coverage in risk.Coverages)
                    {
                        if (commissions.Exists(x => x.SubLineBusiness.Id == coverage.SubLineBusiness.Id && x.SubLineBusiness.LineBusiness.Id == coverage.SubLineBusiness.LineBusiness.Id))
                        {
                            commissions.First(x => x.SubLineBusiness.Id == coverage.SubLineBusiness.Id && x.SubLineBusiness.LineBusiness.Id == coverage.SubLineBusiness.LineBusiness.Id).CalculateBase += coverage.PremiumAmount;
                        }
                        else
                        {
                            commissions.Add(new UWMO.IssuanceCommission
                            {
                                CalculateBase = coverage.PremiumAmount,
                                SubLineBusiness = coverage.SubLineBusiness
                            });
                        }
                    }
                }

                foreach (UWMO.IssuanceAgency agency in policy.Agencies)
                {
                    UWMO.IssuanceCommission oldCommission = new UWMO.IssuanceCommission();
                    if (agency.Commissions.Count > 0)
                    {
                        oldCommission = agency.Commissions[0];
                    }

                    agency.Commissions = new List<UWMO.IssuanceCommission>();

                    foreach (UWMO.IssuanceCommission commission in commissions)
                    {
                        UWMO.IssuanceCommission newCommission = new UWMO.IssuanceCommission();
                        newCommission.SubLineBusiness = commission.SubLineBusiness;
                        if (agency.Agent.AgentType != null)
                        {
                            if (agency.Code != 2039)
                            {
                                newCommission.AgentPercentage = oldCommission.AgentPercentage;
                                newCommission.Percentage = oldCommission.Percentage;
                                newCommission.PercentageAdditional = oldCommission.PercentageAdditional;
                                newCommission.CalculateBase = commission.CalculateBase;
                            }
                            else
                            {
                                newCommission.AgentPercentage = 0;
                                newCommission.Percentage = 0;
                                newCommission.PercentageAdditional = 0;
                                newCommission.CalculateBase = 0;
                            }
                        }
                        else
                        {
                            newCommission.AgentPercentage = 0;
                            newCommission.Percentage = 0;
                            newCommission.PercentageAdditional = 0;
                            newCommission.CalculateBase = 0;
                        }
                        newCommission.Amount = (newCommission.CalculateBase * (newCommission.Percentage + newCommission.PercentageAdditional)) / 100;
                        newCommission.Amount = (newCommission.Amount * agency.Participation) / 100;
                        agency.Commissions.Add(newCommission);
                    }
                }
            }

            return policy.Agencies;
        }
    }
}