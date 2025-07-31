using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.VehicleCancellationService.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.DTOs;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VEM = Sistran.Company.Application.Vehicles.VehicleServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;
namespace Sistran.Company.Application.VehicleCancellationService.EEProvider.Business
{
    /// <summary>
    /// Cancelacion Autos
    /// </summary>
    public class VehicleCancellationBusinessCia
    {

        BaseBusinessCia baseBusinessCia;
        private int coverageIdAccNoOriginal;
        private int coverageIdAccOriginal;
        public static object obj = new object();
        public int CoverageIdAccNoOriginal
        {
            get
            {
                if (coverageIdAccNoOriginal == 0)
                {
                    coverageIdAccNoOriginal = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.NonOriginalAccessories).NumberParameter.Value;
                }

                return coverageIdAccNoOriginal;
            }
        }

        public int CoverageIdAccOriginal
        {
            get
            {
                if (coverageIdAccOriginal == 0)
                {
                    coverageIdAccOriginal = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories).NumberParameter.Value;
                }

                return coverageIdAccOriginal;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleEndorsementBusinessCia" /> class.
        /// </summary>
        public VehicleCancellationBusinessCia()
        {
            baseBusinessCia = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the temporal.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement)
        {
            try
            {
                CompanyPolicy companyPolicy = new CompanyPolicy();
                if (companyEndorsement == null)
                {
                    throw new ArgumentException("");
                }

                if (companyEndorsement.TemporalId != 0)
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
                }
                else
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyEndorsement.Id);
                    companyPolicy.Id = 0;
                    companyPolicy.Endorsement.TemporalId = 0;
                    //companyPolicy.UserId = BusinessContext.Current.UserId;
                }
                companyPolicy.Endorsement = new CompanyEndorsement
                {

                    Id = companyEndorsement.Id,
                    PolicyId = companyEndorsement.PolicyId,
                    CancellationTypeId = companyEndorsement.CancellationTypeId,
                    EndorsementReasonId = companyEndorsement.EndorsementReasonId,
                    EndorsementType = EndorsementType.Cancellation,
                    Text = new CompanyText
                    {
                        TextBody = companyEndorsement.Text.TextBody,
                        Observations = companyEndorsement.Text.Observations
                    },
                    EndorsementDays = companyEndorsement.EndorsementDays,
                    CurrentTo = companyEndorsement.CurrentTo,
                    CurrentFrom = companyEndorsement.CurrentFrom
                };
                companyPolicy.CurrentTo = companyEndorsement.CurrentTo;
                companyPolicy.CurrentFrom = companyEndorsement.CurrentFrom;
                companyPolicy.Endorsement.TicketNumber = companyEndorsement.TicketNumber;
                companyPolicy.Endorsement.TicketDate = companyEndorsement.TicketDate;
                companyPolicy.IssueDate = companyEndorsement.IssueDate;
                companyPolicy.UserId = companyEndorsement.UserId;
                companyPolicy = CancellationPolicy(companyPolicy);
                return companyPolicy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Cancellations the policy.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception">
        /// </exception>
        private CompanyPolicy CancellationPolicy(CompanyPolicy companyPolicy)
        {
            List<VEM.CompanyVehicle> companyVehicles = null;
            if (companyPolicy == null)
            {
                throw new ArgumentException(Errors.ErrorPolicyEmpty);
            }
            if (companyPolicy?.Endorsement != null)
            {
                int cancellationFactor = -1;
                List<CompanyRisk> companyRisk = new List<CompanyRisk>();
                List<PoliciesAut> riskInfringementPolicies = new List<PoliciesAut>();
                if ((CancellationType)companyPolicy.Endorsement.CancellationTypeId == CancellationType.Nominative)
                {
                    cancellationFactor = 0;
                    companyPolicy.Endorsement.EndorsementType = (EndorsementType) companyPolicy.Endorsement.CancellationTypeId; 
                }
                if (companyPolicy.Id == 0)
                {
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyPolicy.Endorsement.PolicyId);
                    coverageIdAccNoOriginal = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.NonOriginalAccessories).NumberParameter.Value;
                    companyVehicles.AsParallel().ForAll(
                          x =>
                          {
                              x.Risk.OriginalStatus = x.Risk.Status;
                              x.Risk.Status = RiskStatusType.Modified;
                              if (x.Risk.Coverages.FirstOrDefault(m => m.Id == CoverageIdAccNoOriginal) != null)
                              {
                                  x.Risk.Coverages.First(m => m.Id == CoverageIdAccNoOriginal).CurrentFromOriginal = x.Risk.Coverages.First(m => m.Id == CoverageIdAccNoOriginal).CurrentFrom;
                              }
                              if (x.Accesories?.Count > 0)
                              {
                                  x.Accesories.AsParallel().ForAll(y =>
                                  {
                                      y.Premium = 0;
                                  });
                              }
                          });
                }
                else
                {
                    companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(companyPolicy.Id);
                    if (companyVehicles == null || companyVehicles.Count < 1)
                    {
                        companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyPolicy.Endorsement.PolicyId);
                        companyVehicles.AsParallel().ForAll(
                         x =>
                         {
                             x.Risk.OriginalStatus = x.Risk.Status;
                             x.Risk.Status = RiskStatusType.Modified;
                         });
                    }
                }

                var companyPolicyTemp = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                companyPolicy.Endorsement.TemporalId = companyPolicyTemp.Endorsement.TemporalId;
                companyPolicy.Id = companyPolicyTemp.Id;
                companyRisk = CreateVehicleCancelation(companyPolicy, companyVehicles, cancellationFactor);

                if (companyRisk != null && companyRisk.Any())
                {
                    riskInfringementPolicies.AddRange(companyRisk.SelectMany(x => x.InfringementPolicies).Where(m => m != null).ToList());
                    companyPolicy = baseBusinessCia.CalculatePolicyAmounts(companyPolicy, companyRisk);
                    companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                    if (companyPolicy.InfringementPolicies != null)
                    {
                        companyPolicy.InfringementPolicies.AddRange(riskInfringementPolicies);
                    }
                    return companyPolicy;
                }
                else
                {
                    throw new Exception(Errors.UnquotedRisks);
                }
            }
            else
            {
                throw new Exception(Errors.EmptyEndorsement);
            }
        }

        /// <summary>
        /// Creates the vehicle cancelation.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <param name="companyVehicles">The company vehicles.</param>
        /// <param name="cancellationFactor">The cancellation factor.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// Error Recuperando Vehiculos
        /// </exception>
        /// <exception cref="Exception"></exception>
        public List<CompanyRisk> CreateVehicleCancelation(CompanyPolicy companyPolicy, List<VEM.CompanyVehicle> companyVehicles, int cancellationFactor)
        {
            if (companyPolicy == null || companyVehicles == null || companyVehicles.Count < 1)
            {
                throw new ArgumentException(Errors.ErrorVehicleEmpty);
            }

            if (companyVehicles != null && companyVehicles.Count > 0)
            {
                List<CompanyRisk> risks = DelegateService.cancellationEndorsement.QuotateCiaCancellation(companyPolicy, cancellationFactor);
                if (risks?.Count > 0)
                {
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    companyVehicles.Where(a => a != null).AsParallel().ForAll(
                        z =>
                        {
                            try
                            {
                                /*Accesorios*/
                                if (z.Accesories?.Count > 0)
                                {
                                    List<AccessoryDTO> accessoryDTOs = DelegateService.vehicleService.GetPremiumAccesory(companyPolicy.Endorsement.PolicyId, z.Risk.Number, QuoteManager.CalculateEffectiveDays(companyPolicy.CurrentFrom, companyPolicy.CurrentTo), true);
                                    foreach (VEM.CompanyAccessory accessory in z.Accesories)
                                    {
                                        accessory.AccumulatedPremium = accessoryDTOs.Where(m => m.Id == accessory.RiskDetailId).First().premium;
                                    }
                                    coverageIdAccNoOriginal = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.NonOriginalAccessories).NumberParameter.Value;
                                    CompanyCoverage coverageAccessoryNoOriginal = z.Risk.Coverages.FirstOrDefault(x => x.Id == CoverageIdAccNoOriginal);
                                    risks.FirstOrDefault(x => x.Number == z.Risk.Number).Coverages.FirstOrDefault(x => x.Id == CoverageIdAccNoOriginal).CurrentFromOriginal = coverageAccessoryNoOriginal.CurrentFromOriginal;
                                    foreach (var item in z.Accesories)
                                    {
                                        if (!item.IsOriginal)
                                        {
                                            item.Premium = decimal.Round(item.AccumulatedPremium * cancellationFactor, QuoteManager.DecimalRound);
                                            item.AccumulatedPremium = item.AccumulatedPremium;
                                        }
                                        item.Status = (int)RiskStatusType.Excluded;
                                        item.Amount = item.Amount * cancellationFactor;
                                    }
                                }

                                CompanyRisk companyRisk = risks.FirstOrDefault(x => x.Number == z.Risk.Number);
                                if (companyRisk != null)
                                {
                                    /*Deducibles*/
                                    foreach (CompanyCoverage item in companyRisk.Coverages)
                                    {
                                        item.Deductible = z.Risk.Coverages.Where(x => x.Id == item.Id).FirstOrDefault()?.Deductible;
                                    }
                                    z.Risk.Policy = companyPolicy;
                                    z.Risk.Number = companyRisk.Number;
                                    z.Risk.Status = companyRisk.Status;
                                    z.Risk.Coverages = companyRisk.Coverages;
                                    z.Risk.Premium = companyRisk.Coverages.Where(m => m != null).Sum(x => x.PremiumAmount);
                                }
                                else
                                {
                                    errors.Add(Errors.ErrorRiskNotFound);
                                }
                            }
                            catch (Exception)
                            {

                                errors.Add(Errors.ErrorCreateTemporalCancellationVehicle);
                            }
                        }
                    );
                    ConcurrentBag<string> error = new ConcurrentBag<string>();
                    TP.Parallel.ForEach(companyVehicles.Where(z => risks.Select(x => x.Number).Contains(z.Risk.Number)), item =>
                       {
                           try
                           {
                               item = DelegateService.vehicleModificationService.GetDataModification(item, CoverageStatusType.Modified);
                               item.Rate = item.Rate * -1;

                               List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, item.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);

                               item.Risk.Coverages.AsParallel().ForAll(coverage =>
                               {
                                   try
                                   {
                                       if (coverages?.FirstOrDefault(x => x.Id == coverage.Id)?.SubLineBusiness == null)
                                       {
                                           new Exception("Ramo tecnico no encontrado: " + coverage.Id.ToString());
                                       }
                                       coverage.SubLineBusiness = coverages?.FirstOrDefault(x => x.Id == coverage.Id)?.SubLineBusiness;
                                       coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                                       coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                                   }
                                   catch (Exception ex)
                                   {

                                       error.Add(ex.Message);
                                   }
                               });
                               item.Risk.Coverages.AsParallel().ForAll(x => x.EndorsementType = EndorsementType.Cancellation);
                               CompanyVehicle vehicle = DelegateService.vehicleService.CreateVehicleTemporal(item, false);
                               if (vehicle != null && vehicle.Risk != null)
                               {
                                   item.Risk.Id = vehicle.Risk.Id;
                                   item.Risk.InfringementPolicies = vehicle.Risk.InfringementPolicies;
                               }
                           }
                           catch (Exception ex)
                           {

                               error.Add(ex.Message);
                           }

                       });
                    if (error != null && error.Count > 0)
                    {
                        throw new Exception(string.Join("-", error.ToList()));
                    }
                    return companyVehicles.Select(x => x.Risk).ToList();
                }
                else
                {
                    throw new ArgumentException(Errors.ErrorVehicleCancellation);
                }
            }
            else
            {
                throw new ArgumentException("Error Recuperando Vehiculos");
            }

        }


        public CompanyPolicy ExecuteThread(List<VEM.CompanyVehicle> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            foreach (VEM.CompanyVehicle vehicle in risksThread)
            {
                if (vehicle.Risk.Beneficiaries[0].IdentificationDocument == null)
                {
                    foreach (CompanyBeneficiary item in vehicle.Risk.Beneficiaries)
                    {
                        Beneficiary beneficiary = new Beneficiary();
                        beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                        item.IdentificationDocument = beneficiary.IdentificationDocument;
                        item.Name = beneficiary.Name;
                        item.CustomerType = beneficiary.CustomerType;
                    }
                }

                vehicle.Risk.Status = RiskStatusType.Excluded;
                vehicle.Rate = vehicle.Rate * -1;
                vehicle.Risk.Coverages = risks.First(x => x.Number == vehicle.Risk.Number).Coverages;

                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, vehicle.Risk.GroupCoverage.Id, policy.Prefix.Id);
                foreach (CompanyCoverage coverage in vehicle.Risk.Coverages)
                {
                    coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                    coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                    coverage.CoverStatus = CoverageStatusType.Excluded;
                    coverage.EndorsementType = policy.Endorsement.EndorsementType;
                    coverage.CurrentFrom = policy.CurrentFrom;
                }


                DelegateService.vehicleService.CreateVehicleTemporal(vehicle, false);

            }
            return policy;
        }

        private CompanyRisk SetDataVehicle(CompanyRisk companyRisk)
        {
            if (companyRisk != null)
            {
                companyRisk.Status = RiskStatusType.Modified;
            }
            return companyRisk;
        }
    }

}
