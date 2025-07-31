using Sistran.Company.Application.UnderwritingServices.Models;
using System;


namespace Sistran.Company.Application.MarineClauseService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.MarineClauseService.EEProvider.Resources;
    using Sistran.Company.Application.Marines.MarineBusinessService.Models.Base;
    using Sistran.Company.Application.MarineClauseService.EEProvider.Services;
    using CTSE = Sistran.Company.Application.ClauseEndorsement.EEProvider;
    using CTBEE = Sistran.Company.Application.Marines.MarineBusinessService.EEProvider;
    using System.Collections.Generic;
    using System.Linq;
    using Sistran.Core.Application.UnderwritingServices.Enums;

    class ClauseBusinessCia
    {

        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarineEndorsementBusinessCia" /> class.
        /// </summary>
        public ClauseBusinessCia()
        {
            provider = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        public CompanyPolicy CreateEndorsementClause(CompanyPolicy companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            try
            {
                CompanyPolicy policy = new CompanyPolicy();
                CTSE.CiaClauseEndorsementEEProvider CiaClauseEndorsementEEProvider = new CTSE.CiaClauseEndorsementEEProvider();
                policy = CiaClauseEndorsementEEProvider.CreateCiaClause(companyEndorsement);
                if (policy != null)
                {
                    List<CompanyMarine> companyMarines = null;
                    companyMarines = DelegateService.marineService.GetCompanyMarinesByPolicyId(companyEndorsement.Endorsement.PolicyId);
                    if (companyMarines != null && companyMarines.Count > 0)
                    {
                        List<CompanyRisk> risks = new List<CompanyRisk>();
                        companyMarines.AsParallel().ForAll(item =>
                        {
                            item.Risk.Policy = policy;
                            item.Risk.Status = RiskStatusType.Modified;
                            item = DelegateService.marineModificationService.GetDataModification(item, CoverageStatusType.Modified);
                            var marine = DelegateService.marineService.CreateCompanyMarineTemporal(item);
                        });
                        risks = companyMarines.Select(x => x.Risk).ToList();
                        policy = provider.CalculatePolicyAmounts(policy, risks);
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        //List<CompanyMarine> marines = DelegateService.marineService.CreateCompanyMarineTemporal(policy.Id);
                        List<CompanyMarine> marines = DelegateService.marineService.GetCompanyMarinesByTemporalId(policy.Id);

                        var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                        if (message != string.Empty)
                        {
                            throw new Exception(message);
                        }
                        CTBEE.CompanyMarineBusinessServiceProvider companyMarineBusinessServiceProvider = new CTBEE.CompanyMarineBusinessServiceProvider();
                        //MarineServiceEEProvider marineServiceEEProvider = new MarineServiceEEProvider();
                        var companyPolicy = companyMarineBusinessServiceProvider.CreateEndorsement(policy, marines);
                        CompanyPolicy newCompanyPolicy = new CompanyPolicy();
                        newCompanyPolicy.Id = companyPolicy.Id;
                        newCompanyPolicy.DocumentNumber = companyPolicy.DocumentNumber;
                        newCompanyPolicy.Endorsement = companyPolicy.Endorsement;
                        newCompanyPolicy.InfringementPolicies = companyPolicy.InfringementPolicies;
                        return newCompanyPolicy;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorMarineNotFound);
                    }
                }
                else
                {
                    throw new Exception("Poliza no encontrada.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
