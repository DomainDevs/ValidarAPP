using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using VEM = Sistran.Company.Application.Vehicles.VehicleServices.Models;


namespace Sistran.Company.Application.VehicleClauseService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.Vehicles.VehicleServices.Models;
    using Sistran.Company.Application.VehicleClauseService.EEProvider.Resources;
    using Sistran.Company.Application.VehicleClauseService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using CTSE = Sistran.Company.Application.ClauseEndorsement.EEProvider;
    using CVSE = Sistran.Company.Application.Vehicles.VehicleServices.EEProvider;
    using Sistran.Company.Application.Vehicles.VehicleServices.DTOs;
    using Sistran.Core.Application.Utilities.Managers;

    class ClauseBusinessCia
    {

        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleEndorsementBusinessCia" /> class.
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
                CTSE.CiaClauseEndorsementEEProvider CiaClauseEndorsementEEProvider = new CTSE.CiaClauseEndorsementEEProvider();
                policy = CiaClauseEndorsementEEProvider.CreateCiaClause(companyEndorsement);
                policy.IssueDate = companyEndorsement.IssueDate;
                if (policy != null)
                {
                    List<VEM.CompanyVehicle> companyVehicles = null;
                    if (companyEndorsement.Endorsement.TemporalId == 0)
                    {
                        companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyEndorsement.Endorsement.PolicyId);
                    }
                    else
                    {
                        companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(policy.Id);
                        companyVehicles.AsParallel().ForAll(
                      x =>
                      {
                          x.Risk.OriginalStatus = x.Risk.Status;
                          x.Risk.Status = RiskStatusType.NotModified;
                      });

                    }

                    if (companyVehicles != null && companyVehicles.Count > 0)
                    {
                        List<CompanyRisk> risks = new List<CompanyRisk>();
                        companyVehicles.AsParallel().ForAll(item =>
                        {
                            item.Risk.Policy = policy;
                            item.Risk.Status = RiskStatusType.Modified;
                            item = DelegateService.vehicleModificationService.GetDataModification(item, CoverageStatusType.Modified);
                            if (item?.Accesories?.Count > 0)
                            {

                                List<AccessoryDTO> accessoryDTOs = DelegateService.vehicleService.GetPremiumAccesory(policy.Endorsement.PolicyId, item.Risk.Number, QuoteManager.CalculateEffectiveDays(policy.CurrentFrom, policy.CurrentTo));
                                foreach (VEM.CompanyAccessory accessory in item.Accesories)
                                {
                                    accessory.AccumulatedPremium = accessoryDTOs.Where(m => m.Id == accessory.RiskDetailId).First().premium;
                                }
                            }
                            var vehicle = DelegateService.vehicleService.CreateVehicleTemporal(item, false);
                        });
                        risks = companyVehicles.Select(x => x.Risk).ToList();
                        policy = provider.CalculatePolicyAmounts(policy, risks);
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        policy.IssueDate = companyEndorsement.IssueDate;
                        List<CompanyVehicle> vehicles = DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(policy.Id);

                        var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                        if (message != string.Empty)
                        {
                            throw new Exception(message);
                        }
                        CVSE.VehicleServiceEEProvider vehicleServiceEEProvider = new CVSE.VehicleServiceEEProvider();
                        return vehicleServiceEEProvider.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, false);
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
