using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PEM = Sistran.Company.Application.Location.PropertyServices.Models;
using CPSPROVIDER = Sistran.Company.Application.Location.PropertyServices.EEProvider;
namespace Sistran.Company.Application.PropertyTextService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.Location.PropertyServices.Models;
    using Sistran.Company.Application.PropertyTextService.EEProvider.Resources;
    using Sistran.Company.Application.PropertyTextService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Enums;
   
    using CPSE = Sistran.Company.Application.TextEndorsement.EEProvider;

    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class PropertyTextBusinessCia
    {
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyEndorsementBusinessCia" /> class.
        /// </summary>
        public PropertyTextBusinessCia()
        {
            provider = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        public CompanyPolicyResult CreateEndorsementText(CompanyEndorsement companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            try
            {
                CompanyPolicy policy = new CompanyPolicy();
                CPSE.CiaTextEndorsementEEProvider CiaTextEndorsementEEProvider = new CPSE.CiaTextEndorsementEEProvider();
                policy = CiaTextEndorsementEEProvider.CreateCiaTexts(companyEndorsement);
                List<PEM.CompanyPropertyRisk> companyProperty = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyEndorsement.PolicyId);
                
                if (companyProperty != null && companyProperty.Count > 0)
                {
                    List<CompanyRisk> risks = new List<CompanyRisk>();
                    companyProperty.AsParallel().ForAll(item =>
                    {
                        item.Risk.Policy = policy;
                        item.Risk.Status = RiskStatusType.Modified;
                        item = DelegateService.propertyModificationService.GetDataModification(item, CoverageStatusType.Modified);
                        var Property = DelegateService.propertyService.CreatePropertyTemporal(item, false);
                    });
                    risks = companyProperty.Select(x => x.Risk).ToList();
                    policy = provider.CalculatePolicyAmounts(policy, risks);
                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

                    var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                    if (message != string.Empty)
                    {
                        throw new Exception(message);
                    }
                    CPSPROVIDER.PropertyServiceEEProvider propertyServiceEEProvider = new CPSPROVIDER.PropertyServiceEEProvider();
                    return propertyServiceEEProvider.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, false);
                }
                else
                {
                    throw new Exception(Errors.ErrorVehicleNotFound);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
