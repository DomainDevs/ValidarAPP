using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PEM = Sistran.Company.Application.Location.PropertyServices.Models;


namespace Sistran.Company.Application.PropertyClauseService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.Location.PropertyServices.Models;
    using Sistran.Company.Application.PropertyClauseService.EEProvider.Resources;
    using Sistran.Company.Application.PropertyClauseService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using CPSE = Sistran.Company.Application.ClauseEndorsement.EEProvider;
    using CPS = Sistran.Company.Application.Location.PropertyServices;
  



    class ClauseBusinessCia
    {

        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyEndorsementBusinessCia" /> class.
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
        public CompanyPolicyResult CreateEndorsementClause(CompanyPolicy companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            try
            {
                CompanyPolicy policy = new CompanyPolicy();
                CPSE.CiaClauseEndorsementEEProvider CiaClauseEndorsementEEProvider = new CPSE.CiaClauseEndorsementEEProvider();
                policy = CiaClauseEndorsementEEProvider.CreateCiaClause(companyEndorsement);
                policy.IssueDate = companyEndorsement.IssueDate;
                if (policy != null)
                { 
                    List<PEM.CompanyPropertyRisk> companyProperty = null;
                    companyProperty = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyEndorsement.Endorsement.PolicyId);
                    if (companyProperty != null && companyProperty.Count > 0)
                    {
                        List<CompanyRisk> risks = new List<CompanyRisk>();
                        companyProperty.AsParallel().ForAll(item =>
                        {
                            item.Risk.Policy = policy;
                            item.Risk.Status = RiskStatusType.Modified;
                            item = DelegateService.propertyModificationService.GetDataModification(item, CoverageStatusType.Modified);
                            var property = DelegateService.propertyService.CreatePropertyTemporal(item, false);
                        });
                        risks = companyProperty.Select(x => x.Risk).ToList();
                        policy = provider.CalculatePolicyAmounts(policy, risks);
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        List<CompanyPropertyRisk>  companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByTemporalId(policy.Id);

                        var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                        if (message != string.Empty)
                        {
                            throw new Exception(message);
                        }
                        CPS.EEProvider.PropertyServiceEEProvider propertyServiceEEProvider = new CPS.EEProvider.PropertyServiceEEProvider();
                        return propertyServiceEEProvider.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, false);
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorVehicleNotFound);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
