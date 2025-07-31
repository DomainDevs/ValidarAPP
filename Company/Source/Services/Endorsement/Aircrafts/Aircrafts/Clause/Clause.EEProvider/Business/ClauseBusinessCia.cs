using Sistran.Company.Application.UnderwritingServices.Models;
using System;


namespace Sistran.Company.Application.AircraftClauseService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.AircraftClauseService.EEProvider.Resources;
    using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;
    using Sistran.Company.Application.AircraftClauseService.EEProvider.Services;
    using CTSE = Sistran.Company.Application.ClauseEndorsement.EEProvider;
    using CTBEE = Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider;
    using System.Collections.Generic;
    using System.Linq;
    using Sistran.Core.Application.UnderwritingServices.Enums;

    class ClauseBusinessCia
    {

        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftEndorsementBusinessCia" /> class.
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
                    List<CompanyAircraft> companyAircrafts = null;
                    companyAircrafts = DelegateService.aircraftService.GetCompanyAircraftsByPolicyId(companyEndorsement.Endorsement.PolicyId);
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
                        //List<CompanyAircraft> aircrafts = DelegateService.aircraftService.CreateCompanyAircraftTemporal(policy.Id);
                        List<CompanyAircraft> aircrafts = DelegateService.aircraftService.GetCompanyAircraftsByTemporalId(policy.Id);

                        var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                        if (message != string.Empty)
                        {
                            throw new Exception(message);
                        }
                        CTBEE.CompanyAircraftBusinessServiceProvider companyAircraftBusinessServiceProvider = new CTBEE.CompanyAircraftBusinessServiceProvider();
                        //AircraftServiceEEProvider aircraftServiceEEProvider = new AircraftServiceEEProvider();
                        var companyPolicy = companyAircraftBusinessServiceProvider.CreateEndorsement(policy, aircrafts);
                        CompanyPolicy newCompanyPolicy = new CompanyPolicy();
                        newCompanyPolicy.Id = companyPolicy.Id;
                        newCompanyPolicy.DocumentNumber = companyPolicy.DocumentNumber;
                        newCompanyPolicy.Endorsement = companyPolicy.Endorsement;
                        newCompanyPolicy.InfringementPolicies = companyPolicy.InfringementPolicies;
                        return newCompanyPolicy;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorAircraftNotFound);
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
