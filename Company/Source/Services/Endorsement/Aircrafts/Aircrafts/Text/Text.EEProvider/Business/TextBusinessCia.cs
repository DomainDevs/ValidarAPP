using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.AircraftTextService.EEProvider.Services;

namespace Sistran.Company.Application.AircraftTextService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;
    using Sistran.Company.Application.AircraftTextService.EEProvider.Resources;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using CTSE = Sistran.Company.Application.TextEndorsement.EEProvider;
    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class AircraftTextBusinessCia
    {
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftEndorsementBusinessCia" /> class.
        /// </summary>
        public AircraftTextBusinessCia()
        {
            provider = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        public CompanyPolicy CreateEndorsementText(CompanyEndorsement companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            try
            {
                CompanyPolicy policy = new CompanyPolicy();
                CTSE.CiaTextEndorsementEEProvider CiaTextEndorsementEEProvider = new CTSE.CiaTextEndorsementEEProvider();
                policy = CiaTextEndorsementEEProvider.CreateCiaTexts(companyEndorsement);
                List<CompanyAircraft> companyAircrafts = null;
                companyAircrafts = DelegateService.aircraftService.GetCompanyAircraftsByPolicyId(companyEndorsement.PolicyId);
                if (companyAircrafts != null && companyAircrafts.Count > 0)
                {
                    List<CompanyRisk> risks = new List<CompanyRisk>();
                    companyAircrafts.AsParallel().ForAll(item =>
                    {
                        item.Risk.Policy = policy;
                        item.Risk.Status = RiskStatusType.Modified;
                        item = DelegateService.aircraftModificationService.GetDataModification(item, CoverageStatusType.Modified);
                        var aircraft = DelegateService.aircraftService.CreateCompanyAircraftTemporal(item);
                    });
                    risks = companyAircrafts.Select(x => x.Risk).ToList();
                    policy = provider.CalculatePolicyAmounts(policy, risks);
                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                    List<CompanyAircraft> aircrafts = DelegateService.aircraftService.GetCompanyAircraftsByTemporalId(policy.Id);

                    var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                    if (message != string.Empty)
                    {
                        throw new Exception(message);
                    }
                    /// CVSE.AircraftServiceEEProvider aircraftServiceEEProvider = new CVSE.AircraftServiceEEProvider();
                    return DelegateService.aircraftService.CreateEndorsement(policy, aircrafts);
                    
                }
                else
                {
                    throw new Exception(Errors.ErrorAircraftNotFound);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
