using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using VEM = Sistran.Company.Application.Vehicles.VehicleServices.Models;
namespace Sistran.Company.Application.VehicleTextService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.Vehicles.VehicleServices.DTOs;
    using Sistran.Company.Application.Vehicles.VehicleServices.Models;
    using Sistran.Company.Application.VehicleTextService.EEProvider.Resources;
    using Sistran.Company.Application.VehicleTextService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using Sistran.Core.Application.Utilities.Managers;
    using System.Collections.Concurrent;
    using CTSE = Sistran.Company.Application.TextEndorsement.EEProvider;
    using CVSE = Sistran.Company.Application.Vehicles.VehicleServices.EEProvider;
    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class VehicleTextBusinessCia
    {
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleEndorsementBusinessCia" /> class.
        /// </summary>
        public VehicleTextBusinessCia()
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
                CTSE.CiaTextEndorsementEEProvider CiaTextEndorsementEEProvider = new CTSE.CiaTextEndorsementEEProvider();
                policy = CiaTextEndorsementEEProvider.CreateCiaTexts(companyEndorsement);
                List<VEM.CompanyVehicle> companyVehicles = null;
                if (companyEndorsement.TemporalId != 0)
                {
                    companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(companyEndorsement.TemporalId);
                    if (companyVehicles == null || companyVehicles.Count < 1)
                    {
                        companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyEndorsement.PolicyId);
                        companyVehicles.AsParallel().ForAll(
                            x =>
                            {
                                x.Risk.Id = 0;
                                x.Risk.OriginalStatus = x.Risk.Status;
                                x.Risk.Status = RiskStatusType.NotModified;
                            });
                    }

                }
                else
                {
                    companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyEndorsement.PolicyId);
                    companyVehicles.AsParallel().ForAll(
                           x =>
                           {
                               x.Risk.Id = 0;
                               x.Risk.OriginalStatus = x.Risk.Status;
                               x.Risk.Status = RiskStatusType.NotModified;
                               if (x.Accesories?.Count > 0)
                               {
                                   x.Accesories.AsParallel().ForAll(y =>
                                       {
                                           y.Premium = 0;
                                       });
                               }


                           });


                }
                if (companyVehicles != null && companyVehicles.Count > 0)
                {
                    List<CompanyRisk> risks = new List<CompanyRisk>();
                    ConcurrentBag<string> errorsData = new ConcurrentBag<string>();
                    companyVehicles.AsParallel().ForAll(item =>
                    {
                        try
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
                        }
                        catch (Exception)
                        {
                            errorsData.Add(Errors.ErrorCreateVehicle);
                        }

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
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
