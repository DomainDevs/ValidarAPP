using Newtonsoft.Json;
using Sistran.Company.Application.MassiveVehicleCancellationService.EEProvider.Assemblers;
using Sistran.Company.Application.MassiveVehicleCancellationService.EEProvider.Resources;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Vehicles.VehicleServices.DTOs;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUEN = Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using AUTHMO = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.MassiveVehicleCancellationService.EEProvider.DAOs
{
    /// <summary>
    /// Cancelacion Masiva Autos 
    /// </summary>
    public class MassiveCancellationDAO
    {
        private static readonly Object objCache = new object();
        #region propiedades
        private static List<Use> uses = new List<Use>();
        private static List<DocumentType> documentTypes = new List<DocumentType>();
        private static List<Color> colors = new List<Color>();
        private static List<Deductible> deductibles = new List<Deductible>();
        private static List<Accessory> accesoriesList = new List<Accessory>();
        private static List<Core.Application.Vehicles.Models.Type> types = new List<Core.Application.Vehicles.Models.Type>();
        private static List<RatingZone> ratingZones = new List<RatingZone>();
        private static List<BillingGroup> billingGroup = new List<BillingGroup>();

        ConcurrentBag<Row> concurrentRows = new ConcurrentBag<Row>();
        readonly ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();

        private int coverageIdAccNoOriginal;
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

        #endregion
        /// <summary>
        /// Crear Temporal endoso Cancelacion masiva Autos
        /// </summary>
        /// <param name="risks">Lista de Riesgos</param>
        /// <param name="massiveCancellationRow">Proceso Masivo</param>
        public List<AuthorizationRequest> CreateTemporalEndorsementCancellation(MassiveLoad massiveLoad, CompanyPolicy companyPolicy, List<CompanyRisk> risks, MassiveCancellationRow massiveCancellationRow)
        {
            List<UserGroupModel> userGroup = DelegateService.uniqueUserService.GetUsersGroupByUserId(massiveLoad.User.UserId).Select(x => new AUTHMO.UserGroupModel { UserId = companyPolicy.UserId, GroupId = x.GroupId }).ToList();
            List<AuthorizationRequest> authorizationRequests = new List<AuthorizationRequest>();
            if (risks != null && risks.Any())
            {
                int cancellationFactor = -1;
                if ((CancellationType)companyPolicy.Endorsement.CancellationTypeId == CancellationType.Nominative)
                {
                    cancellationFactor = 0;
                }
                try
                {
                    Row row = JsonConvert.DeserializeObject<Row>(massiveCancellationRow.SerializedRow);
                    DateTime cancelationCurrentFrom = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                    PendingOperation pendingOperation = new PendingOperation();
                    PendingOperation pendingOperationRisk = new PendingOperation();
                    companyPolicy.Product.CoveredRisk = new CompanyCoveredRisk
                    {
                        CoveredRiskType = CoveredRiskType.Vehicle,
                        SubCoveredRiskType = massiveCancellationRow.SubcoveredRiskType
                    };
                    companyPolicy.UserId = massiveLoad.User.UserId;
                    companyPolicy.Endorsement.EndorsementDays = companyPolicy.CurrentTo.Subtract(companyPolicy.CurrentFrom).Days;
                    companyPolicy.Endorsement.EndorsementDays = companyPolicy.CurrentTo.Subtract(cancelationCurrentFrom).Days;
                    companyPolicy.CurrentFrom = risks[0].Policy.CurrentFrom;
                    companyPolicy.CurrentTo = risks[0].Policy.CurrentTo;
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.Summary = new CompanySummary { RiskCount = risks.Count };
                    companyPolicy.PolicyType.Description = DelegateService.commonService.GetPolicyTypesByPrefixIdById(companyPolicy.Prefix.Id, companyPolicy.PolicyType.Id).Description;
                    List<Currency> LstCurrency = DelegateService.productService.GetCurrenciesByProductId(companyPolicy.Product.Id);
                    companyPolicy.ExchangeRate.Currency = LstCurrency.First(x => x.Id == companyPolicy.ExchangeRate.Currency.Id);
                    if (companyPolicy.PaymentPlan == null)
                    {
                        companyPolicy.PaymentPlan = DelegateService.underwritingService.GetPaymentPlanByPolicyId(risks[0].Policy.Id);
                        if (companyPolicy.PaymentPlan == null)
                        {
                            throw new ValidationException(Errors.ErrorWithThePaymentPlanOfThePolicy);
                        }
                    }
                    companyPolicy.PaymentPlan = DelegateService.underwritingService.GetPaymentPlanByPaymentPlanId(companyPolicy.PaymentPlan.Id);

                    companyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyPolicy);
                    List<CompanyVehicle> companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(risks[0].Policy.Endorsement.PolicyId);
                    
                    companyVehicles = CalculateAccesories(risks, companyVehicles, companyPolicy);

                    List<CompanyVehicle> filtercompanyVehicle = new List<CompanyVehicle>();

                    if (companyVehicles == null || !companyVehicles.Any())
                    {
                        throw new ValidationException(Errors.ErrorThePolicyPresentsDataProblems);
                    }

                    foreach (CompanyVehicle item in companyVehicles)
                    {
                        if (risks.Exists(u => u.Number == item.Risk.Number))
                        {
                            item.Risk.Policy.Endorsement = companyPolicy.Endorsement;
                            filtercompanyVehicle.Add(item);
                        }
                    }
                    companyVehicles = filtercompanyVehicle;

                    if (companyPolicy.InfringementPolicies.Any(x => x.Type == AUEN.TypePolicies.Restrictive))
                    {
                        throw new Exception(string.Format(Errors.PoliciesRestrictive + "</br>", string.Join("</br>", companyPolicy.InfringementPolicies.Where(x => x.Type == AUEN.TypePolicies.Restrictive).Select(x => "*" + x.Message).ToList())));
                    }

                    if (companyPolicy.InfringementPolicies.Any(x => x.Type == AUEN.TypePolicies.Authorization))
                    {
                        List<AUTHMO.PoliciesAut> policiesAuts = new List<AUTHMO.PoliciesAut>();
                        policiesAuts.AddRange(companyPolicy.InfringementPolicies);
                        authorizationRequests.AddRange(DelegateService.massiveService.ValidateAuthorizationPolicies(policiesAuts, massiveLoad, companyPolicy.Id));
                        massiveCancellationRow.HasEvents = true;
                    }

                    //poliza
                    List<CompanyRisk> companyRisk = DelegateService.ciaVehicleCancellationService.CreateVehicleCancelation(companyPolicy, companyVehicles, cancellationFactor);
                    if (companyRisk != null && companyRisk.Any())
                    {
                        risks = companyRisk;
                    }
                    else
                    {
                        throw new Exception(/*Errors.UnquotedRisks*/"");
                    }

                    bool returnExpenses = companyPolicy.Endorsement.CancellationTypeId == (int)CancellationType.BeginDate;
                    companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponentsByCompanyPolicy(companyPolicy, risks);
                    companyPolicy.Summary = DelegateService.underwritingService.CalculateSummaryByCompanyPolicy(companyPolicy, risks);
                    companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissionsByCompanyPolicy(companyPolicy, risks);
                    companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(new QuotaFilterDTO { PlanId = companyPolicy.PaymentPlan.Id, CurrentFrom = companyPolicy.CurrentFrom, IssueDate = companyPolicy.IssueDate, ComponentValueDTO = ModelAssembler.CreateCompanyComponentValueDTO(companyPolicy.Summary) });


                    CompanyVehicle companyVehicle;
                    massiveCancellationRow.Observations = companyPolicy.Summary.Premium.ToString();
                    massiveCancellationRow.TotalCommission = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                    massiveCancellationRow.HasEvents = (companyPolicy.InfringementPolicies != null && companyPolicy.InfringementPolicies.Count != 0);
                    massiveCancellationRow.Status = MassiveLoadProcessStatus.Events;

                    foreach (CompanyVehicle vehicle in companyVehicles)
                    {
                        companyVehicle = vehicle;
                        vehicle.Risk.Status = RiskStatusType.Excluded;
                        vehicle.Rate = vehicle.Rate * -1;
                        vehicle.Risk.CoveredRiskType = CoveredRiskType.Vehicle;
                        vehicle.Risk.LimitRc = DelegateService.underwritingService.GetCompanyLimitRcById(vehicle.Risk.LimitRc.Id);
                        vehicle.Risk.Policy = companyPolicy;
                        companyVehicle = DelegateService.vehicleService.CompanySaveCompanyVehicleTemporal(vehicle);
                    }

                    massiveCancellationRow.Risk = new Risk
                    {
                        Id = companyVehicles[0].Risk.Id,
                        Description = companyVehicles[0].LicensePlate,
                        Policy = new Policy
                        {
                            Id = companyPolicy.Id,
                            DocumentNumber = companyPolicy.DocumentNumber,
                            Summary = new Summary
                            {
                                FullPremium = companyPolicy.Summary.FullPremium
                            },
                            PolicyType = new PolicyType
                            {
                                Id = companyPolicy.PolicyType.Id,
                                Description = companyPolicy.PolicyType.Description
                            },
                            Branch = new Branch
                            {
                                Id = companyPolicy.Branch.Id
                            },
                            Prefix = new Prefix
                            {
                                Id = companyPolicy.Prefix.Id
                            }
                        }
                    };

                    companyPolicy = DelegateService.underwritingService.SaveTableTemporal(companyPolicy);

                    pendingOperation.Id = companyPolicy.Id;
                    pendingOperation.UserId = risks[0].Policy.UserId;
                    pendingOperation.OperationName = "Temporal";
                    pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyPolicy);

                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);
                    }
                    else
                    {
                        string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", COMUT.JsonHelper.SerializeObjectToJson(pendingOperation), (char)007, COMUT.JsonHelper.SerializeObjectToJson(pendingOperationRisk), (char)007, COMUT.JsonHelper.SerializeObjectToJson(massiveCancellationRow), (char)007, nameof(MassiveCancellationRow));
                        QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "UpdatePendingOperationQuee");
                    }

                }
                catch (Exception ex)
                {
                    string[] messages = ex.Message.Split('|');
                    massiveCancellationRow.HasError = true;
                    massiveCancellationRow.Observations = messages[0];
                    massiveCancellationRow.Status = MassiveLoadProcessStatus.Tariff;
                    DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            }
            else
            {
                massiveCancellationRow.HasError = true;
                massiveCancellationRow.Observations = Errors.ErrorRiskNotFound;
                massiveCancellationRow.Status = MassiveLoadProcessStatus.Tariff;
                DelegateService.massiveUnderwritingService.UpdateMassiveCancellationRows(massiveCancellationRow);
            }
            return authorizationRequests;
        }

        private List<CompanyVehicle> CalculateAccesories(List<CompanyRisk> risks , List<CompanyVehicle> companyVehicles , CompanyPolicy companyPolicy) 
        {
            if (risks?.Count > 0)
            {
                int cancellationFactor = -1;
                if ((CancellationType)companyPolicy.Endorsement.CancellationTypeId == CancellationType.Nominative)
                {
                    cancellationFactor = 0;
                }

                ConcurrentBag<string> errors = new ConcurrentBag<string>();
                companyVehicles.Where(a => a != null).AsParallel().ForAll(
                    z =>
                    {
                        try
                        {
                            /*Accesorios*/
                            if (z.Accesories?.Count > 0)
                            {
                                z.Accesories.AsParallel().ForAll(y =>
                                {
                                    y.Premium = 0;
                                });
                                
                                List<AccessoryDTO> accessoryDTOs = DelegateService.vehicleService.GetPremiumAccesory(companyPolicy.Endorsement.PolicyId, z.Risk.Number, QuoteManager.CalculateEffectiveDays(companyPolicy.CurrentFrom, companyPolicy.CurrentTo), true);
                                foreach (CompanyAccessory accessory in z.Accesories)
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

                            var companyRisk = risks.FirstOrDefault(x => x.Number == z.Risk.Number);
                            if (companyRisk != null)
                            {
                                /*Deducibles*/
                                foreach (var item in companyRisk.Coverages)
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
            }
            return companyVehicles;
        }
        #region Reportes

        /// <summary>
        /// Generar Reporte de Cancelacion Masiva Autos Por identificador y estado del cargue
        /// </summary>
        /// <param name="MassiveLoadId">The massive load identifier.</param>
        /// <param name="massiveLoadStatus">The massive load status.</param>
        /// <returns>
        /// File
        /// </returns>
        public string GenerateReportByMassiveLoadIdByMassiveLoadStatus(int MassiveLoadId, MassiveLoadStatus massiveLoadStatus)
        {
            MassiveLoadProcessStatus processStatus = MassiveLoadProcessStatus.Validation;
            switch (massiveLoadStatus)
            {
                case MassiveLoadStatus.Tariffed:
                    processStatus = MassiveLoadProcessStatus.Events;
                    break;
                case MassiveLoadStatus.Issued:
                    processStatus = MassiveLoadProcessStatus.Finalized;
                    break;
            }
            DelegateService.massiveService.LoadReportCacheList();
            LoadList();
            List<LineBusiness> lineBusiness = DelegateService.commonService.GetLineBusinessBySubCoveredRiskType(SubCoveredRiskType.Vehicle);
            List<MassiveCancellationRow> massiveCancellationRows = DelegateService.massiveUnderwritingService.GetMassiveCancellationRowsByMassiveLoadIdSubCoveredRiskType(MassiveLoadId, SubCoveredRiskType.Vehicle, processStatus, false, null);
            if (massiveCancellationRows == null || !massiveCancellationRows.Any())
            {
                return null;
            }

            MassiveEmission massiveEmission = new MassiveEmission();
            massiveEmission.Id = MassiveLoadId;
            massiveEmission.Prefix = new Prefix { Id = lineBusiness.FirstOrDefault().Id };
            List<Row> rows = new List<Row>();
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.MassiveReport;
            fileProcessValue.Key4 = (int)massiveEmission.Prefix.Id;
            fileProcessValue.Key5 = (int)SubCoveredRiskType.Vehicle;
            massiveEmission.Status = massiveLoadStatus;
            File file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);
            if (file == null)
            {
                return null;
            }

            file.Templates[0].Rows[0].Fields.ForEach(u => u.Value = "");
            string serializeFields = COMUT.JsonHelper.SerializeObjectToJson(file.Templates[0].Rows[0].Fields);
            string key = Guid.NewGuid().ToString();
            string filePath = "";
            
            int bulkExcel = Convert.ToInt32(DelegateService.commonService.GetKeyApplication("MaxMassiveExcelRow"));
            file.FileType = FileType.CSV;

            TP.Parallel.ForEach(massiveCancellationRows,
                (process) =>
                {
                    FillVehicleFields(massiveEmission, process, serializeFields);
                    if (concurrentRows != null && concurrentRows.Count != 0 && concurrentRows.Count >= bulkExcel || massiveCancellationRows.Count == 0)
                    {
                        file.Templates[0].Rows = concurrentRows.ToList();
                        file.Name = "Reporte autos_" + key + "_" + massiveEmission.Id;
                        filePath = DelegateService.utilitiesService.GenerateFile(file);
                        concurrentRows = new ConcurrentBag<Row>();
                    }
                });
            
            return filePath;
        }
        private void FillVehicleFields(MassiveEmission massiveEmission, MassiveCancellationRow massiveCancellationRow, string serializeFields)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.massiveService.GetCompanyPolicyByMassiveLoadStatusPolicy(massiveEmission.Status.Value, massiveCancellationRow.Risk.Policy);
                companyPolicy.Id = massiveCancellationRow.Risk.Policy.Id;
                if (companyPolicy != null)
                {
                    List<CompanyVehicle> vehicles = GetCompanyVehicle(massiveEmission, massiveCancellationRow, massiveCancellationRow.Risk.Id);
                    List<Field> fields = DelegateService.massiveService.GetFields(serializeFields, companyPolicy);
                    foreach (CompanyVehicle vehicle in vehicles)
                    {
                        vehicle.Risk.MainInsured.Name = vehicle.Risk.MainInsured.Name + " " + vehicle.Risk.MainInsured.Surname + " " + vehicle.Risk.MainInsured.SecondSurname;
                        fields.Find(u => u.PropertyName == FieldPropertyName.Identificator).Value = massiveCancellationRow.RowNumber.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.LoadId).Value = massiveEmission.Id.ToString();
                        fields = DelegateService.massiveService.FillInsuredFields(fields, vehicle.Risk.MainInsured);
                        fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = vehicle.Price.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskNumber).Value = vehicle.Risk.Number.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskRatingZoneDescription).Value = (vehicle.Risk.RatingZone != null && vehicle.Risk.RatingZone.Id > 0) ? ratingZones.FirstOrDefault(u => u.Id == vehicle.Risk.RatingZone.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleTypeDescription).Value = (types.Count > 0) ? types.FirstOrDefault(u => u.Id == vehicle.Version.Type.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskMakeDescription).Value = vehicle.Make.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleDescription).Value = vehicle.Model?.Description + " " + vehicle.Version?.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskColorDescription).Value = colors.FirstOrDefault(u => u.Id == vehicle?.Color?.Id)?.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskModel).Value = vehicle.Year.ToString();
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskFasecolda).Value = vehicle.Fasecolda.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskLicensePlate).Value = vehicle.LicensePlate;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskEngineDescription).Value = vehicle.EngineSerial;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskChassisDescription).Value = vehicle.ChassisSerial;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskServiceTypeDescription).Value = uses?.FirstOrDefault(u => u.Id == vehicle?.Use?.Id)?.Description ?? "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskLimitRcDescription).Value = vehicle.Risk.LimitRc.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskRate).Value = vehicle.Rate.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskPrice).Value = vehicle.Risk.AmountInsured.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.BillingGroup).Value = (companyPolicy.Request != null && companyPolicy.Request.BillingGroupId > 0) ? billingGroup.FirstOrDefault(u => u.Id == companyPolicy.Request.BillingGroupId).Description + " (" + companyPolicy.Request.BillingGroupId + ")" : "";
                        if (vehicle.GoodExperienceYear != null)
                        {
                            if (vehicle.GoodExperienceYear.GoodExpNumPrinter < 0)
                            {
                                fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = "0";
                            }
                            else
                            {
                                //if (vehicle.GoodExperienceYear.GoodExpNumPrinter >= goodExpNumPrinter)
                                //{
                                //    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = String.Format("{0} o más", goodExpNumPrinter);
                                //}
                                //else
                                //{
                                //    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = vehicle.GoodExperienceYear.GoodExpNumPrinter.ToString();
                                //}
                            }
                        }

                        //Asistencia 
                        CompanyCoverage coverageAsistance = vehicle.Risk.Coverages.FirstOrDefault(u => u.Id == 9);
                        if (coverageAsistance != null)
                        {
                            decimal assistancePremium = coverageAsistance.PremiumAmount;
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyPremium).Value =
                                (companyPolicy.Summary.Premium - assistancePremium).ToString("F2");
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExpensesWithAssistance).Value =
                                (companyPolicy.Summary.Expenses + assistancePremium).ToString("F2");
                        }

                        serializeFields = COMUT.JsonHelper.SerializeObjectToJson(fields);
                        foreach (CompanyCoverage coverage in vehicle.Risk.Coverages.OrderByDescending(u => u.Number))
                        {
                            List<Field> fieldsC = JsonConvert.DeserializeObject<List<Field>>(serializeFields);

                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageDescription).Value = coverage.Description;
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageInsuredValue).Value = coverage.EndorsementSublimitAmount.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageRate).Value = coverage.Rate.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoveragePremium).Value = coverage.PremiumAmount.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.DeductibleDescription).Value = (coverage.Deductible != null && coverage.Deductible.Description != null) ? coverage.Deductible.Description : "";
                            concurrentRows.Add(new Row
                            {
                                Number = massiveCancellationRow.RowNumber,
                                Fields = fieldsC
                            });

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exceptions.Enqueue(ex);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private List<CompanyVehicle> GetCompanyVehicle(MassiveEmission massiveEmission, MassiveCancellationRow proccess, int tempId)
        {
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();
            switch (massiveEmission.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    List<PendingOperation> pendingOperations = new List<PendingOperation>();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperations = DelegateService.utilitiesService.GetPendingOperationsByParentId(proccess.Risk.Policy.Id);
                    }
                    else
                    {
                        pendingOperations = DelegateService.utilitiesService.GetPendingOperationsByParentId(proccess.Risk.Policy.Id);
                    }
                    foreach (PendingOperation item in pendingOperations)
                    {
                        companyVehicles.Add(JsonConvert.DeserializeObject<CompanyVehicle>(item.Operation));
                    }
                    break;
                case MassiveLoadStatus.Issued:
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* without Replicated Database */
                        DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => companyVehicles.Add(JsonConvert.DeserializeObject<CompanyVehicle>(x)));
                    }
                    else
                    {
                        DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => companyVehicles.Add(JsonConvert.DeserializeObject<CompanyVehicle>(x)));
                        /* with Replicated Database */
                    }
                    break;
            }
            return companyVehicles.OrderByDescending(x => x.LicensePlate).ToList();
        }
        private string CreateAccessories(List<Accessory> accessories)
        {
            string value = "";
            if (accessories != null && accessories.Any())
            {
                foreach (Accessory accessory in accessories)
                {
                    value += (accesoriesList.Count > 0 ? accesoriesList.FirstOrDefault(u => u.Id == accessory.Id).Description : "") + " " + Convert.ToInt64(accessory.Amount) + " | ";
                }
            }
            return value;
        }

        private void LoadList()
        {
            if (uses == null || uses.Count == 0)
            {
                uses = DelegateService.vehicleService.GetUses();
            }

            if (colors.Count == 0)
            {
                colors = DelegateService.vehicleService.GetColors();
            }
            if (types.Count == 0)
            {
                types = DelegateService.vehicleService.GetTypes();
            }
            if (accesoriesList.Count == 0)
            {
                accesoriesList = DelegateService.vehicleService.GetAccessories();
            }
            documentTypes = (List<DocumentType>)DelegateService.massiveService.GetCacheList("documentTypes_", "Report");
            deductibles = (List<Deductible>)DelegateService.massiveService.GetCacheList("deductibles_", "Report");
            ratingZones = DelegateService.underwritingService.GetRatingZones();
            billingGroup = DelegateService.underwritingService.GetBillingGroup();
        }
        #endregion reportes
    }
}
