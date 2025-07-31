using AutoMapper;
using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Assemblers;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Entities.views;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using ISSENC = Sistran.Company.Application.Issuance.Entities;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Concurrent;
using Sistran.Core.Application.Utilities.Helper;
using UTILITIES = Company.UnderwritingUtilities;
using Sistran.Core.Application.Common.Entities;
using VEMO = Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;
using System.Threading;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using TEM = Sistran.Core.Application.Temporary.Entities;
using Sistran.Company.Application.Utilities.RulesEngine;
using UTILITES = Company.UnderwritingUtilities;
using Sistran.Core.Application.Utilities.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.DAOs
{
    public class ThirdPartyLiabilityDAO
    {
        /// <summary>
        /// Obtener Poliza de RC pasajeros
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>CompanyTplPolicy</returns>
        public List<CompanyTplRisk> GetCompanyThirdPartyLiabilitiesByPolicyId(int policyId)
        {
            List<CompanyTplRisk> thirdPartyLiabilityRisks = new List<CompanyTplRisk>();

            //riesgos
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Not();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.In();
            filter.ListValue();
            filter.Constant((int)RiskStatusType.Excluded);
            filter.Constant((int)RiskStatusType.Cancelled);
            filter.EndList();
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);

            return GetCompanyTPLRisksByFilter(filter);
        }

        public List<CompanyTplRisk> GetCompanyTPLRisksByFilter(ObjectCriteriaBuilder filter)
        {
            RiskThirdPartyLiabilityView view = new RiskThirdPartyLiabilityView();
            ViewBuilder builder = new ViewBuilder("RiskThirdPartyLiabilityView")
            {
                Filter = filter.GetPredicate()
            };

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }


            if (view.Risks.Count == 0)
            {
                throw new ArgumentException(Errors.ErrorRiskEmpty);
            }
            List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
            List<COMMEN.VehicleModel> vehicleModels = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();
            List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
            List<COMMEN.VehicleVersion> vehicleVersions = view.VehicleVersions.Cast<COMMEN.VehicleVersion>().ToList();
            List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
            List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
            List<ISSENC.CiaRiskVehicle> CiaRiskVehicles = view.CiaRiskVehicles.Cast<ISSENC.CiaRiskVehicle>().ToList();

            ConcurrentBag<CompanyTplRisk> companyTPLs = new ConcurrentBag<CompanyTplRisk>();

            ParallelHelper.ForEach(risks, item =>
            {
                try
                {
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.LoadDynamicProperties(item);
                    }
                    CompanyTplRisk thirdPartyLiability = new CompanyTplRisk();

                    thirdPartyLiability = ModelAssembler.CreateThirdPartyLiability(item,
                        view.RiskVehicles.Cast<ISSEN.RiskVehicle>().First(x => x.RiskId == item.RiskId),
                        view.CoRiskVehicles.Cast<ISSEN.CoRiskVehicle>().First(x => x.RiskId == item.RiskId),
                        view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == item.RiskId),
                        view.CoRisks.Cast<ISSEN.CoRisk>().First(x => x.RiskId == item.RiskId)
                        );

                    if (thirdPartyLiability.Risk.Text != null)
                    {
                        thirdPartyLiability.Risk.Text.TextBody = item.ConditionText;
                    }
                    else
                    {
                        thirdPartyLiability.Risk.Text = new CompanyText()
                        {
                            TextBody = item.ConditionText
                        };
                    }

                    thirdPartyLiability.Make.Description = vehicleMakes.FirstOrDefault(x => x.VehicleMakeCode == thirdPartyLiability.Make.Id).SmallDescription;
                    thirdPartyLiability.Model.Description = vehicleModels.FirstOrDefault(x => x.VehicleModelCode == thirdPartyLiability.Model.Id).SmallDescription;
                    thirdPartyLiability.Version.Description = vehicleVersions.FirstOrDefault(x => x.VehicleVersionCode == thirdPartyLiability.Version.Id).Description;
                    thirdPartyLiability.GallonTankCapacity = CiaRiskVehicles.First(x => x.RiskId == item.RiskId).GallonQuantity;
                    thirdPartyLiability.YearModel = (int)CiaRiskVehicles.First(x => x.RiskId == item.RiskId).YearModel;
                    thirdPartyLiability.RepoweringYear = CiaRiskVehicles.First(x => x.RiskId == item.RiskId).YearTransformVehicle;
                    thirdPartyLiability.RePoweredVehicle = CiaRiskVehicles.First(x => x.RiskId == item.RiskId).IsTransformVehicle;

                    var companyInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(thirdPartyLiability.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                    if (companyInsured == null || companyInsured.IndividualId < 1)
                    {
                        throw new Exception(Errors.ErrorInsuredMain);
                    }

                    thirdPartyLiability.Risk.MainInsured = companyInsured;
                    thirdPartyLiability.Risk.MainInsured.Name = thirdPartyLiability.Risk.MainInsured.Name + " " + thirdPartyLiability.Risk.MainInsured.Surname + " " + thirdPartyLiability.Risk.MainInsured.SecondSurname;
                    ConcurrentBag<CompanyClause> clauses = new ConcurrentBag<CompanyClause>();

                    var companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(thirdPartyLiability.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                    IssuanceCompanyName IssCompanyName = new IssuanceCompanyName();
                    IssCompanyName.NameNum = companyName.NameNum;
                    IssCompanyName.TradeName = companyName.TradeName;
                    if (companyName.Address != null)
                    {
                        IssCompanyName.Address = new IssuanceAddress
                        {
                            Id = companyName.Address.Id,
                            Description = companyName.Address.Description,
                            City = companyName.Address.City
                        };
                    }
                    if (companyName.Phone != null)
                    {
                        IssCompanyName.Phone = new IssuancePhone
                        {
                            Id = companyName.Phone.Id,
                            Description = companyName.Phone.Description
                        };
                    }
                    if (companyName.Email != null)
                    {
                        IssCompanyName.Email = new IssuanceEmail
                        {
                            Id = companyName.Email.Id,
                            Description = companyName.Email.Description
                        };
                    }
                    thirdPartyLiability.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                    if (riskBeneficiaries != null && riskBeneficiaries.Count > 0)
                    {
                        Object objlock = new object();
                        var mapper = ModelAssembler.CreateMapCompanyBeneficiary();
                        List<ISSEN.RiskBeneficiary> beneficiaries = riskBeneficiaries.Where(x => x.RiskId == item.RiskId).ToList();
                        ParallelHelper.ForEach(beneficiaries, riskBeneficiary =>
                        {
                            try
                            {
                                CompanyBeneficiary CiaBeneficiary = new CompanyBeneficiary();
                                var beneficiaryRisk = DelegateService.underwritingService.GetBeneficiariesByDescription(riskBeneficiary.BeneficiaryId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                                if (beneficiaryRisk != null)
                                {
                                    CiaBeneficiary = mapper.Map<Beneficiary, CompanyBeneficiary>(beneficiaryRisk);
                                    CiaBeneficiary.CustomerType = CustomerType.Individual;
                                    CiaBeneficiary.BeneficiaryType = new CompanyBeneficiaryType { Id = riskBeneficiary.BeneficiaryTypeCode };
                                    CiaBeneficiary.Participation = riskBeneficiary.BenefitPercentage;
                                    List<CompanyName> companyNames = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(CiaBeneficiary.IndividualId, (CustomerType)CiaBeneficiary.CustomerType);
                                    companyName = new CompanyName();
                                    if (companyNames.Exists(x => x.NameNum == 0 && x.IsMain))
                                    {
                                        companyName = companyNames.First(x => x.IsMain);
                                    }
                                    else
                                    {
                                        companyName = companyNames.First();
                                    }
                                    IssuanceCompanyName issuanceCompanyName = new IssuanceCompanyName();
                                    issuanceCompanyName.NameNum = companyName.NameNum;
                                    issuanceCompanyName.TradeName = companyName.TradeName;
                                    if (companyName.Address != null)
                                    {

                                        issuanceCompanyName.Address = new IssuanceAddress
                                        {
                                            Id = companyName.Address.Id,
                                            Description = companyName.Address.Description,
                                            City = companyName.Address.City
                                        };
                                    }
                                    if (companyName.Phone != null)
                                    {
                                        issuanceCompanyName.Phone = new IssuancePhone
                                        {
                                            Id = companyName.Phone.Id,
                                            Description = companyName.Phone.Description
                                        };
                                    }

                                    if (companyName.Email != null)
                                    {
                                        issuanceCompanyName.Email = new IssuanceEmail
                                        {
                                            Id = companyName.Email.Id,
                                            Description = companyName.Email.Description
                                        };
                                    }

                                    lock (objlock)
                                    {
                                        thirdPartyLiability.Risk.Beneficiaries.Add(CiaBeneficiary);
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                            }
                            finally
                            {
                                DataFacadeManager.Dispose();
                            }

                        });
                    }
                    List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();

                    thirdPartyLiability.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdByRiskId(endorsementRisks.First(x => x.RiskId == item.RiskId).PolicyId, thirdPartyLiability.Risk.RiskId);

                    companyTPLs.Add(thirdPartyLiability);
                }
                catch (Exception)
                {
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }


            });
            return companyTPLs.ToList();
        }

        public List<CompanyTplRisk> GetCompanyThirdPartyLiabilitiesByEndorsementId(int endorsementId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementOperation.Properties.EndorsementId, typeof(ISSEN.EndorsementOperation).Name).Equal().Constant(endorsementId);
            filter.And();
            filter.Property(ISSEN.EndorsementOperation.Properties.RiskNumber, typeof(ISSEN.EndorsementOperation).Name).IsNotNull();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ISSEN.EndorsementOperation), filter.GetPredicate()));

            List<CompanyTplRisk> thirdPartyLiabilityRisks = ModelAssembler.CreateTemporalThirdPartyLiabilities(businessCollection);

            if (thirdPartyLiabilityRisks.Count > 0)
            {
                if (thirdPartyLiabilityRisks[0].Risk.Coverages != null)
                {
                    return thirdPartyLiabilityRisks;
                }
                else
                {
                    return GetCompanyThirdPartyLiabilitiesFromTables(endorsementId);
                }
            }
            else
            {
                return GetCompanyThirdPartyLiabilitiesFromTables(endorsementId);
            }
        }

        /// <summary>
        /// Obtener Poliza de vehiculos
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>Vehiclepolicy</returns>
        private List<CompanyTplRisk> GetCompanyThirdPartyLiabilitiesFromTables(int endorsementId)
        {
            List<CompanyTplRisk> thirdPartyLiabilityRisks = new List<CompanyTplRisk>();

            //riesgos
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            RiskThirdPartyLiabilityView view = new RiskThirdPartyLiabilityView();
            ViewBuilder builder = new ViewBuilder("RiskThirdPartyLiabilityView");

            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            List<COMMEN.VehicleModel> vehicleModels = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();
            List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
            List<COMMEN.VehicleVersion> vehicleVersions = view.VehicleVersions.Cast<COMMEN.VehicleVersion>().ToList();


            if (view.Risks.Count > 0)
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                foreach (ISSEN.Risk item in view.Risks)
                {
                    DataFacadeManager.Instance.GetDataFacade().LoadDynamicProperties(item);
                    CompanyTplRisk thirdPartyLiability = new CompanyTplRisk();

                    thirdPartyLiability = ModelAssembler.CreateThirdPartyLiability(item,
                        view.RiskVehicles.Cast<ISSEN.RiskVehicle>().First(x => x.RiskId == item.RiskId),
                        view.CoRiskVehicles.Cast<ISSEN.CoRiskVehicle>().First(x => x.RiskId == item.RiskId),
                       //  view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == item.RiskId),
                       endorsementRisks.Where(x => x.RiskId == item.RiskId).First(),
                        view.CoRisks.Cast<ISSEN.CoRisk>().First(x => x.RiskId == item.RiskId)
                        );

                    thirdPartyLiability.Make.Description = vehicleMakes.Where(x => x.VehicleMakeCode == thirdPartyLiability.Make.Id).FirstOrDefault().SmallDescription;
                    thirdPartyLiability.Model.Description = vehicleModels.Where(x => x.VehicleModelCode == thirdPartyLiability.Model.Id).FirstOrDefault().SmallDescription;
                    thirdPartyLiability.Version.Description = vehicleVersions.Where(x => x.VehicleVersionCode == thirdPartyLiability.Version.Id).FirstOrDefault().Description;


                    int insuredNameNum = thirdPartyLiability.Risk.MainInsured.CompanyName.NameNum;
                    thirdPartyLiability.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(thirdPartyLiability.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);

                    CompanyName companyName = new CompanyName();
                    companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(thirdPartyLiability.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();

                    thirdPartyLiability.Risk.MainInsured.CompanyName = new IssuanceCompanyName
                    {
                        NameNum = companyName.NameNum,
                        TradeName = companyName.TradeName,
                        Address = new IssuanceAddress
                        {
                            Id = companyName.Address.Id,
                            Description = companyName.Address.Description,
                            City = companyName.Address.City
                        },
                        Phone = new IssuancePhone
                        {
                            Id = companyName.Phone.Id,
                            Description = companyName.Phone.Description
                        },
                        Email = new IssuanceEmail
                        {
                            Id = companyName.Email.Id,
                            Description = companyName.Email.Description
                        }
                    };

                    thirdPartyLiability.Risk.Beneficiaries = ModelAssembler.CreateBeneficiaries(view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().Where(x => x.RiskId == item.RiskId).ToList());

                    foreach (CompanyBeneficiary beneficiary in thirdPartyLiability.Risk.Beneficiaries)
                    {
                        int beneficiaryNameNum = beneficiary.CompanyName.NameNum;
                        List<CompanyName> companyNames = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(beneficiary.IndividualId, (CustomerType)beneficiary.CustomerType);
                        companyName = new CompanyName();

                        if (beneficiaryNameNum == 0)
                        {
                            companyName = companyNames.First(x => x.IsMain);
                        }
                        else
                        {
                            companyName = companyNames.First(x => x.NameNum == beneficiaryNameNum);
                        }
                        beneficiary.CompanyName = new IssuanceCompanyName
                        {
                            NameNum = companyName.NameNum,
                            TradeName = companyName.TradeName,
                            Address = new IssuanceAddress
                            {
                                Id = companyName.Address.Id,
                                Description = companyName.Address.Description,
                                City = companyName.Address.City
                            },
                            Phone = new IssuancePhone
                            {
                                Id = companyName.Phone.Id,
                                Description = companyName.Phone.Description
                            },
                            Email = new IssuanceEmail
                            {
                                Id = companyName.Email.Id,
                                Description = companyName.Email.Description
                            }
                        };
                    }

                    thirdPartyLiability.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(endorsementRisks[0].PolicyId, endorsementId, thirdPartyLiability.Risk.RiskId);

                    thirdPartyLiabilityRisks.Add(thirdPartyLiability);
                }
            }
            return thirdPartyLiabilityRisks;
        }

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Riesgos</returns>
        public List<CompanyTplRisk> GetThirdPartyLiabilitiesByTemporalId(int temporalId)
        {
            List<CompanyTplRisk> thirdPartyLiabilities = new List<CompanyTplRisk>();
            List<PendingOperation> pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(temporalId);

            foreach (PendingOperation pendingOperation in pendingOperations)
            {
                CompanyTplRisk companyTplRisk = COMUT.JsonHelper.DeserializeJson<CompanyTplRisk>(pendingOperation.Operation);
                companyTplRisk.Risk.Id = pendingOperation.Id;                
                thirdPartyLiabilities.Add(companyTplRisk);
            }

            return thirdPartyLiabilities;
        }


        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="temporalId">Id temporal</param>
        /// <param name="thirdPartyLiability">Modelo thirdPartyLiability</param>
        public CompanyTplRisk CreateThirdPartyLiabilityTemporal(CompanyTplRisk thirdPartyLiability, bool isMassive)
        {
            thirdPartyLiability.Risk.InfringementPolicies = ValidateAuthorizationPolicies(thirdPartyLiability);
            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";
            PendingOperation pendingOperation = new PendingOperation();
            CompanyPolicy companyPolicy = thirdPartyLiability.Risk.Policy;            

            if (thirdPartyLiability.Risk.Id == 0)
            {
                pendingOperation.CreationDate = DateTime.Now;
                pendingOperation.ParentId = thirdPartyLiability.Risk.Policy.Id;
                pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(thirdPartyLiability);

                if (isMassive && boolUseReplicatedDatabase)
                {
                    //Se guarda el JSON en la base de datos de réplica
                }
                else
                {
                    pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
                }
            }
            else
            {
                pendingOperation.ModificationDate = DateTime.Now;
                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(thirdPartyLiability.Risk.Id);
                if (pendingOperation != null)
                {

                    pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(pendingOperation.Operation);

                    if (isMassive && boolUseReplicatedDatabase)
                    {
                        //Se guarda el JSON en la base de datos de réplica
                    }
                    else
                    {
                        DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
                    }

                }
                else
                {
                    throw new Exception("Error obteniendo el Temporal");
                }
            }

            thirdPartyLiability.Risk.Id = pendingOperation.Id;
            thirdPartyLiability.Risk.Policy = companyPolicy;

            int riskId = SaveCompanyVehicleTemporalTables(thirdPartyLiability);
            if (thirdPartyLiability.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
            {
                thirdPartyLiability.Risk.RiskId = riskId;
            }

            pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(thirdPartyLiability);

            if (isMassive && boolUseReplicatedDatabase)
            {
                //Se guarda el JSON en la base de datos de réplica
                DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
            }
            else
            {
                DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
            }
            thirdPartyLiability.Risk.Id = pendingOperation.Id;

            return thirdPartyLiability;
        }
        public int SaveCompanyVehicleTemporalTables(CompanyTplRisk companyTplRisk)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer =
            new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();
            UTILITES.GetDatatables d = new UTILITES.GetDatatables();
            UTILITES.CommonDataTables dts = d.GetcommonDataTables(companyTplRisk.Risk);

            DataTable dataTable;
            NameValue[] parameters = new NameValue[18];

            DataTable dtTempRisk = dts.dtTempRisk;
            parameters[0] = new NameValue(dtTempRisk.TableName, dtTempRisk);

            DataTable dtCOTempRisk = dts.dtCOTempRisk;
            parameters[1] = new NameValue(dtCOTempRisk.TableName, dtCOTempRisk);

            DataTable dtBeneficary = dts.dtBeneficary;
            parameters[2] = new NameValue(dtBeneficary.TableName, dtBeneficary);

            DataTable dtRiskPayer = dts.dtRiskPayer;
            parameters[3] = new NameValue(dtRiskPayer.TableName, dtRiskPayer);

            DataTable dtClause = dts.dtClause;
            parameters[4] = new NameValue(dtClause.TableName, dtClause);

            DataTable dtRiskCoverage = dts.dtRiskCoverage;
            parameters[5] = new NameValue(dtRiskCoverage.TableName, dtRiskCoverage);

            DataTable dtDeduct = dts.dtDeduct;
            parameters[6] = new NameValue(dtDeduct.TableName, dtDeduct);

            DataTable dtCoverClause = dts.dtCoverClause;
            parameters[7] = new NameValue(dtCoverClause.TableName, dtCoverClause);

            DataTable dtDynamic = dts.dtDynamic;
            parameters[8] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_RISK", dtDynamic);

            DataTable dtDynamicCoverage = dts.dtDynamicCoverage;
            parameters[9] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_COVERAGE", dtDynamicCoverage);

            DataTable tdAccessory = new DataTable("INSERT_TEMP_RISK_DETAIL_ACCESSORY");
            parameters[10] = new NameValue(tdAccessory.TableName, tdAccessory);

            DataTable dtTempRiskVehicle = ModelAssembler.GetDataTableRiskVehicle(companyTplRisk);
            parameters[11] = new NameValue(dtTempRiskVehicle.TableName, dtTempRiskVehicle);

            DataTable dtCOTempRiskVehicle = ModelAssembler.GetDataTableTemRiskVehicle(companyTplRisk);
            parameters[12] = new NameValue(dtCOTempRiskVehicle.TableName, dtCOTempRiskVehicle);

            //Campos de RCP
            parameters[13] = new NameValue("@GALLON_QTY", DBNull.Value);
            parameters[14] = new NameValue("@IS_TRANSFORM_VEHICLE", false);
            parameters[15] = new NameValue("@YEAR_TRANSFORM_VEHICLE", DBNull.Value);
            parameters[16] = new NameValue("@TRANSPORT_CARGO_TYPE_CD", DBNull.Value);
            parameters[17] = new NameValue("@YEAR_MODEL", DBNull.Value);


            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {

                dataTable = pdb.ExecuteSPDataTable("TMP.SAVE_TEMPORAL_RISK_VEHICLE_TEMP", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (companyTplRisk.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
                {
                    companyTplRisk.Risk.RiskId = Convert.ToInt32(dataTable.Rows[0][0]);
                }
                return companyTplRisk.Risk.RiskId;
            }
            else
            {
                throw new ValidationException(Errors.ErrorCreateTemporalCompanyVehicle);
            }
        }
        /// <summary>
        /// Obtener Poliza de RC pasajeros
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>CompanyTplPolicy</returns>
        public List<CompanyTplRisk> GetThirdPartyLiabilityPolicyByPolicyIdEndorsementIdLicensePlate(int policyId, int endorsementId, string licensePlate)
        {

            List<CompanyTplRisk> thirdPartyLiabilityRisks = new List<CompanyTplRisk>();

            if (endorsementId == 0) //Se consulta el endoso donde la placa es is_current
            {
                endorsementId = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(policyId, licensePlate).Id;
            }

            //riesgos
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            filter.And();
            filter.Property(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name);
            filter.Equal();
            filter.Constant(licensePlate);

            //Endoso actual no se debe mostrar los riesgos excluidos ni cancelados
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.Distinct();
            filter.Constant(RiskStatusType.Cancelled);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.Distinct();
            filter.Constant(RiskStatusType.Excluded);

            RiskThirdPartyLiabilityView view = new RiskThirdPartyLiabilityView();
            ViewBuilder builder = new ViewBuilder("RiskThirdPartyLiabilityView");

            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();

            if (view.Risks.Count > 0)
            {

                foreach (ISSEN.Risk item in view.Risks)
                {
                    dataFacade.LoadDynamicProperties(item);
                    CompanyTplRisk thirdPartyLiability = new CompanyTplRisk();

                    thirdPartyLiability = ModelAssembler.CreateThirdPartyLiability(item,
                        view.RiskVehicles.Cast<ISSEN.RiskVehicle>().First(x => x.RiskId == item.RiskId),
                        view.CoRiskVehicles.Cast<ISSEN.CoRiskVehicle>().First(x => x.RiskId == item.RiskId),
                        view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == item.RiskId),
                             view.CoRisks.Cast<ISSEN.CoRisk>().First(x => x.RiskId == item.RiskId));

                    int insuredNameNum = thirdPartyLiability.Risk.MainInsured.CompanyName.NameNum;
                    thirdPartyLiability.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(thirdPartyLiability.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);


                    CompanyName companyName = new CompanyName();
                    companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(thirdPartyLiability.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();

                    thirdPartyLiability.Risk.MainInsured.CompanyName = new IssuanceCompanyName
                    {
                        NameNum = companyName.NameNum,
                        TradeName = companyName.TradeName,
                        Address = new IssuanceAddress
                        {
                            Id = companyName.Address.Id,
                            Description = companyName.Address.Description,
                            City = companyName.Address.City
                        },
                        Phone = new IssuancePhone
                        {
                            Id = companyName.Phone.Id,
                            Description = companyName.Phone.Description
                        },
                        Email = new IssuanceEmail
                        {
                            Id = companyName.Email.Id,
                            Description = companyName.Email.Description
                        }
                    };

                    thirdPartyLiability.Risk.Beneficiaries = ModelAssembler.CreateBeneficiaries(view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().Where(x => x.RiskId == item.RiskId).ToList());

                    foreach (CompanyBeneficiary beneficiary in thirdPartyLiability.Risk.Beneficiaries)
                    {
                        int beneficiaryNameNum = beneficiary.CompanyName.NameNum;
                        List<CompanyName> companyNames = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(beneficiary.IndividualId, CustomerType.Individual);
                        companyName = new CompanyName();

                        if (beneficiaryNameNum == 0)
                        {
                            companyName = companyNames.First(x => x.IsMain);
                        }
                        else
                        {
                            companyName = companyNames.First(x => x.NameNum == beneficiaryNameNum);
                        }
                        beneficiary.CompanyName = new IssuanceCompanyName
                        {
                            NameNum = companyName.NameNum,
                            TradeName = companyName.TradeName,
                            Address = new IssuanceAddress
                            {
                                Id = companyName.Address.Id,
                                Description = companyName.Address.Description,
                                City = companyName.Address.City
                            },
                            Phone = new IssuancePhone
                            {
                                Id = companyName.Phone.Id,
                                Description = companyName.Phone.Description
                            },
                            Email = new IssuanceEmail
                            {
                                Id = companyName.Email.Id,
                                Description = companyName.Email.Description
                            }
                        };
                    }

                    thirdPartyLiability.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, thirdPartyLiability.Risk.RiskId);

                    thirdPartyLiabilityRisks.Add(thirdPartyLiability);
                }

                return thirdPartyLiabilityRisks;

            }
            else
            {
                return null;
            }
        }


        public int GetCountThirdPartyLiabilityPolicyByPolicyId(int policyId)
        {
            //riesgos
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Not();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.In();
            filter.ListValue();
            filter.Constant(RiskStatusType.Excluded);
            filter.Constant(RiskStatusType.Cancelled);
            filter.EndList();
            RiskThirdPartyLiabilityView view = new RiskThirdPartyLiabilityView();
            ViewBuilder builder = new ViewBuilder("RiskThirdPartyLiabilityView");

            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();
            return view.Risks.Count();
        }

        public Sistran.Core.Application.Vehicles.Models.ServiceType GetServiceTypeByServiceTypeId(int serviceTypeId)
        {
            try
            {
                var serviceTypePrimaryKey = COMMEN.ServiceType.CreatePrimaryKey(serviceTypeId);
                var dataFacade = DataFacadeManager.Instance.GetDataFacade();
                var businessObject = dataFacade.GetObjectByPrimaryKey(serviceTypePrimaryKey);
                Sistran.Core.Application.Vehicles.Models.ServiceType serviceType = null;
                if (businessObject != null)
                {
                    serviceType = ModelAssembler.CreateServiceType((COMMEN.ServiceType)businessObject);
                }
                return serviceType;
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }
        public bool ValidateThirdPartyLiabilityCorrelativePolicy(int prefixId, int branchId, decimal policyNumber, int productId, string licensePlate)
        {
            var parameters = new NameValue[5];
            parameters[0] = new NameValue("DOCUMENT_NUMBER", policyNumber);
            parameters[1] = new NameValue("PREFIX_ID", prefixId);
            parameters[2] = new NameValue("BRANCH_ID", branchId);
            parameters[3] = new NameValue("LICENCE_PLATE", licensePlate);
            parameters[4] = new NameValue("PRODUCT_ID", productId);
            object result;
            using (var dataAccess = new DynamicDataAccess())
            {
                result = dataAccess.ExecuteSPScalar("ISS.VALIDATE_CORRELATIVE_POLICY_RCP", parameters);
            }
            var isValid = result as int?;
            return isValid.HasValue && isValid.Value == 1;
        }

        public List<PoliciesAut> ValidateAuthorizationPolicies(CompanyTplRisk companyTplRisk)
        {
            Rules.Facade facade = new Rules.Facade();
            var key = companyTplRisk.Risk.Policy.Prefix.Id + "," + (int)companyTplRisk.Risk.Policy.Product.CoveredRisk.CoveredRiskType;
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();


            facade = DelegateService.underwritingService.CreateFacadeGeneral(companyTplRisk.Risk.Policy);


            EntityAssembler.CreateFacadeRiskThirdPartyLiability(facade, companyTplRisk);
            /*Politica del riesgo*/
            policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_RISK));
            /*Politicas de cobertura*/
            if (companyTplRisk.Risk.Coverages != null)
            {
                foreach (CompanyCoverage coverage in companyTplRisk.Risk.Coverages)
                {
                    EntityAssembler.CreateFacadeCoverage(facade, coverage);
                    policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_COVERAGE));
                }
            }
            return policiesAuts;
        }

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyTplRisk> companyTplRisk)
        {
            if (companyPolicy == null)
            {
                throw new ArgumentException("Poliza Vacia");
            }
            ValidateInfringementPolicies(companyPolicy, companyTplRisk);
            if (companyPolicy?.InfringementPolicies?.Count == 0)
            {
                companyPolicy = DelegateService.underwritingService.CreateCompanyPolicy(companyPolicy);
                if (companyPolicy != null)
                {
                    int maxRiskCount = companyPolicy.Summary.RiskCount;
                    int policyId = companyPolicy.Endorsement.PolicyId;
                    int endorsementId = companyPolicy.Endorsement.Id;
                    int endorsementTypeId = (int)companyPolicy.Endorsement.EndorsementType;
                    int operationId = companyPolicy.Id;

                    EndorsementType endorsementType = (EndorsementType)endorsementTypeId;
                    try
                    {

                        TP.Parallel.ForEach(companyTplRisk, companyTpl =>
                        {
                            companyTpl.Risk.Policy = companyPolicy;
                            if (companyTpl.Risk.Number == 0 && (companyTpl.Risk.Status == RiskStatusType.Original || companyTpl.Risk.Status == RiskStatusType.Included))
                            {
                                Interlocked.Increment(ref maxRiskCount);
                            }

                            if (endorsementType == EndorsementType.EffectiveExtension)
                            {
                                companyTpl.Risk.Status = RiskStatusType.Original;
                            }
                        });

                        if (companyPolicy.Product.IsCollective)
                        {
                            ConcurrentBag<string> errors = new ConcurrentBag<string>();
                            Parallel.ForEach(companyTplRisk, ParallelHelper.DebugParallelFor(), companyTpl =>
                            {
                                try
                                {
                                    CreateCompanyTpl(companyTpl);
                                }
                                catch (Exception ex)
                                {

                                    errors.Add(ex.Message);

                                }
                                finally
                                {
                                    DataFacadeManager.Dispose();
                                }
                            });
                        }
                        else
                        {
                            ConcurrentBag<string> errors = new ConcurrentBag<string>();
                            Parallel.ForEach(companyTplRisk, ParallelHelper.DebugParallelFor(), companyTpl =>
                            {
                                try
                                {
                                    CreateCompanyTpl(companyTpl);
                                }
                                catch (Exception ex)
                                {
                                    errors.Add(ex.Message);
                                }
                                finally
                                {
                                    DataFacadeManager.Dispose();
                                }

                            });
                            if (errors != null && errors.Any())
                            {
                                throw new ValidationException(string.Join(" ", errors));
                            }
                        }
                        DelegateService.underwritingService.CreateCompanyPolicyPayer(companyPolicy);
                        try
                        {
                            DelegateService.underwritingService.DeleteTemporalByOperationId(companyPolicy.Id, 0, 0, 0);
                            try
                            {
                                DelegateService.underwritingService.SaveControlPolicy(policyId, endorsementId, operationId, (int)companyPolicy.PolicyOrigin);
                            }
                            catch (Exception)
                            {
                                EventLog.WriteEntry("Application", Errors.ErrorRegisterIntegration);
                            }
                        }
                        catch (Exception)
                        {

                            throw new ValidationException(Errors.ErrorDeleteTemp);
                        }

                    }
                    catch (Exception)
                    {

                        DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(policyId, endorsementId, endorsementType);
                        throw;
                    }
                }
                else
                {
                    DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id, companyPolicy.Endorsement.EndorsementType.Value);
                }

            }
            return companyPolicy;
        }

        private void ValidateInfringementPolicies(CompanyPolicy companyPolicy, List<CompanyTplRisk> companyTplRisk)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            infringementPolicies.AddRange(companyPolicy.InfringementPolicies);
            companyTplRisk.ForEach(x => infringementPolicies.AddRange(x.Risk.InfringementPolicies));

            companyPolicy.InfringementPolicies = DelegateService.authorizationPoliciesService.ValidateInfringementPolicies(infringementPolicies);
        }

        /// <summary>
        /// Crear Póliza RC Pasajeros
        /// </summary>
        /// <param name="companyPolicy">Póliza</param>
        /// <param name="companyTplRisk">Riesgos</param>
        /// <returns>Póliza</returns>
        public void CreateCompanyTpl(CompanyTplRisk companyTplRisk)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();

            NameValue[] parameters = new NameValue[62];
            parameters[0] = new NameValue("@ENDORSEMENT_ID", companyTplRisk.Risk.Policy.Endorsement.Id);
            parameters[1] = new NameValue("@POLICY_ID", companyTplRisk.Risk.Policy.Endorsement.PolicyId);
            parameters[2] = new NameValue("@PAYER_ID", companyTplRisk.Risk.Policy.Holder.IndividualId);
            parameters[3] = new NameValue("@VEHICLE_VERSION_CD", companyTplRisk.Version.Id);
            parameters[4] = new NameValue("@VEHICLE_MODEL_CD", companyTplRisk.Model.Id);
            parameters[5] = new NameValue("@VEHICLE_MAKE_CD", companyTplRisk.Make.Id);
            parameters[6] = new NameValue("@VEHICLE_YEAR", companyTplRisk.Year);
            parameters[7] = new NameValue("@VEHICLE_TYPE_CD", companyTplRisk.Version.Type.Id);
            parameters[8] = new NameValue("@VEHICLE_USE_CD", 1);
            parameters[9] = new NameValue("@VEHICLE_BODY_CD", companyTplRisk.Version.Body.Id);
            parameters[10] = new NameValue("@VEHICLE_PRICE", 0);
            parameters[11] = new NameValue("@IS_NEW", 0);
            parameters[12] = new NameValue("@LICENSE_PLATE", companyTplRisk.LicensePlate);
            parameters[13] = new NameValue("@ENGINE_SER_NO", companyTplRisk.EngineSerial);
            parameters[14] = new NameValue("@CHASSIS_SER_NO", companyTplRisk.ChassisSerial);
            parameters[15] = new NameValue("@VEHICLE_COLOR_CD", 0);
            parameters[16] = new NameValue("@NEW_VEHICLE_PRICE", 0);
            parameters[17] = new NameValue("@VEHICLE_FUEL_CD", 1);
            parameters[18] = new NameValue("@STD_VEHICLE_PRICE", DBNull.Value, DbType.Decimal);
            parameters[19] = new NameValue("@FLAT_RATE_PCT", companyTplRisk.Rate);
            parameters[20] = new NameValue("@DEDUCT_ID", companyTplRisk.Deductible.Id);
            parameters[21] = new NameValue("@EXCESS", false);
            parameters[22] = new NameValue("@SHUTTLE_CD", companyTplRisk.Shuttle.Id);
            parameters[23] = new NameValue("@SERVICE_TYPE_CD", companyTplRisk.ServiceType.Id);
            parameters[24] = new NameValue("@MOBILE_NUM", companyTplRisk.PhoneNumber);
            parameters[25] = new NameValue("@TONS_QTY", companyTplRisk.Tons);
            if (companyTplRisk.RateType == null)
            {
                parameters[26] = new NameValue("@RATE_TYPE_CD", 0);

            }
            else
            {
                parameters[26] = new NameValue("@RATE_TYPE_CD", (int)companyTplRisk.RateType);
            }

            parameters[27] = new NameValue("@PASSENGER_QTY", companyTplRisk.PassengerQuantity);
            parameters[28] = new NameValue("@LOAD_TYPE_CD", companyTplRisk.TypeCargoId);
            parameters[29] = new NameValue("@TRAILERS_QTY", companyTplRisk.TrailerQuantity);

            if (companyTplRisk.Risk.DynamicProperties != null && companyTplRisk.Risk.DynamicProperties.Count > 0)
            {
                DynamicPropertiesCollection dynamicCollectionRisk = new DynamicPropertiesCollection();
                for (int i = 0; i < companyTplRisk.Risk.DynamicProperties.Count(); i++)
                {
                    DynamicProperty dinamycProperty = new DynamicProperty();
                    dinamycProperty.Id = companyTplRisk.Risk.DynamicProperties[i].Id;
                    dinamycProperty.Value = companyTplRisk.Risk.DynamicProperties[i].Value;
                    dynamicCollectionRisk[i] = dinamycProperty;
                }
                byte[] serializedValuesRisk = dynamicPropertiesSerializer.Serialize(dynamicCollectionRisk);
                parameters[30] = new NameValue("@DYNAMIC_PROPERTIES", serializedValuesRisk);
            }
            else
            {
                parameters[30] = new NameValue("@DYNAMIC_PROPERTIES", DBNull.Value, DbType.Binary);
            }

            parameters[31] = new NameValue("@INSURED_ID", companyTplRisk.Risk.MainInsured.IndividualId);
            parameters[32] = new NameValue("@COVERED_RISK_TYPE_CD", (int)companyTplRisk.Risk.CoveredRiskType);
            parameters[33] = new NameValue("@RISK_STATUS_CD", (int)companyTplRisk.Risk.Status);
            parameters[34] = new NameValue("@COMM_RISK_CLASS_CD", DBNull.Value, DbType.Int32);
            parameters[35] = new NameValue("@RISK_COMMERCIAL_TYPE_CD", DBNull.Value, DbType.Int16);

            if (companyTplRisk.Risk.Text == null)
            {
                parameters[36] = new NameValue("@CONDITION_TEXT", DBNull.Value, DbType.String);
            }
            else
            {
                parameters[36] = new NameValue("@CONDITION_TEXT", companyTplRisk.Risk.Text.TextBody);
            }

            parameters[37] = new NameValue("@RATING_ZONE_CD", companyTplRisk.Risk.RatingZone.Id);
            parameters[38] = new NameValue("@COVER_GROUP_ID", companyTplRisk.Risk.GroupCoverage.Id);
            parameters[39] = new NameValue("@IS_FACULTATIVE", companyTplRisk.Risk.IsFacultative);

            if (companyTplRisk.Risk.MainInsured.CompanyName != null && companyTplRisk.Risk.MainInsured.CompanyName.NameNum > 0)
            {
                parameters[40] = new NameValue("@NAME_NUM", companyTplRisk.Risk.MainInsured.CompanyName.NameNum);
            }
            else
            {
                parameters[40] = new NameValue("@NAME_NUM", DBNull.Value, DbType.Int16);
            }

           //valores predeterminados tomados de la operacion con R1
            parameters[41] = new NameValue("@LIMITS_RC_CD", 80);
            parameters[42] = new NameValue("@LIMIT_RC_SUM", 0);
           
            parameters[43] = new NameValue("@SINISTER_PCT", DBNull.Value, DbType.Decimal);
            if (companyTplRisk.Risk.SecondInsured != null && companyTplRisk.Risk.SecondInsured.IndividualId > 0)
            {
                parameters[44] = new NameValue("@SECONDARY_INSURED_ID", companyTplRisk.Risk.SecondInsured.IndividualId);
            }
            else
            {
                parameters[44] = new NameValue("@SECONDARY_INSURED_ID", DBNull.Value, DbType.Int32);
            }

            DataTable dtBeneficiaries = new DataTable("PARAM_TEMP_RISK_BENEFICIARY");
            dtBeneficiaries.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICIARY_ID", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICIARY_TYPE_CD", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICT_PCT", typeof(decimal));
            dtBeneficiaries.Columns.Add("NAME_NUM", typeof(int));

            foreach (CompanyBeneficiary item in companyTplRisk.Risk.Beneficiaries)
            {
                DataRow dataRow = dtBeneficiaries.NewRow();
                dataRow["CUSTOMER_TYPE_CD"] = item.CustomerType;
                dataRow["BENEFICIARY_ID"] = item.IndividualId;
                dataRow["BENEFICIARY_TYPE_CD"] = item.BeneficiaryType.Id;
                dataRow["BENEFICT_PCT"] = item.Participation;

                if (item.CustomerType == CustomerType.Individual && item.CompanyName != null && item.CompanyName.NameNum == 0)
                {
                    if (item.IndividualId == companyTplRisk.Risk.MainInsured.IndividualId)
                    {
                        item.CompanyName = companyTplRisk.Risk.MainInsured.CompanyName;
                    }
                    else
                    {
                        item.CompanyName.TradeName = "Dirección Principal";
                        item.CompanyName.IsMain = true;
                        item.CompanyName.NameNum = 1;
                        List<CompanyName> companyNames = DelegateService.uniquePersonService.GetCompanyNamesByIndividualId(item.IndividualId);
                        if (companyNames == null)
                            companyNames = new List<CompanyName>();
                        if (companyNames.Count == 0)
                        {
                            CompanyName companyName = new CompanyName
                            {
                                NameNum = item.CompanyName.NameNum,
                                TradeName = item.CompanyName.TradeName,
                                Address = new Address
                                {
                                    Id = item.CompanyName.Address.Id,
                                    Description = item.CompanyName.Address.Description,
                                    City = item.CompanyName.Address.City
                                },
                                Phone = new Phone
                                {
                                    Id = item.CompanyName.Phone.Id,
                                    Description = item.CompanyName.Phone.Description
                                },
                                Email = new Email
                                {
                                    Id = item.CompanyName.Email.Id,
                                    Description = item.CompanyName.Email.Description
                                }
                            };
                            DelegateService.uniquePersonService.CreateCompaniesName(companyName, item.IndividualId);
                        }
                    }
                }
                if (item.CompanyName != null && item.CompanyName.NameNum > 0)
                {
                    dataRow["NAME_NUM"] = item.CompanyName.NameNum;
                }

                dtBeneficiaries.Rows.Add(dataRow);
            }

            parameters[45] = new NameValue("@INSERT_TEMP_RISK_BENEFICIARY", dtBeneficiaries);

            DataTable dtCoverages = new DataTable("PARAM_TEMP_RISK_COVERAGE");
            dtCoverages.Columns.Add("COVERAGE_ID", typeof(int));
            dtCoverages.Columns.Add("IS_DECLARATIVE", typeof(bool));
            dtCoverages.Columns.Add("IS_MIN_PREMIUM_DEPOSIT", typeof(bool));
            dtCoverages.Columns.Add("FIRST_RISK_TYPE_CD", typeof(int));
            dtCoverages.Columns.Add("CALCULATION_TYPE_CD", typeof(int));
            dtCoverages.Columns.Add("DECLARED_AMT", typeof(decimal));
            dtCoverages.Columns.Add("PREMIUM_AMT", typeof(decimal));
            dtCoverages.Columns.Add("LIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("SUBLIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("LIMIT_IN_EXCESS", typeof(decimal));
            dtCoverages.Columns.Add("LIMIT_OCCURRENCE_AMT", typeof(decimal));
            dtCoverages.Columns.Add("LIMIT_CLAIMANT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("ACC_PREMIUM_AMT", typeof(decimal));
            dtCoverages.Columns.Add("ACC_LIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("ACC_SUBLIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("CURRENT_FROM", typeof(DateTime));
            dtCoverages.Columns.Add("RATE_TYPE_CD", typeof(int));
            dtCoverages.Columns.Add("RATE", typeof(decimal));
            dtCoverages.Columns.Add("CURRENT_TO", typeof(DateTime));
            dtCoverages.Columns.Add("COVER_NUM", typeof(int));
            dtCoverages.Columns.Add("RISK_COVER_ID", typeof(int));
            dtCoverages.Columns.Add("COVER_STATUS_CD", typeof(int));
            dtCoverages.Columns.Add("COVER_ORIGINAL_STATUS_CD", typeof(int));
            dtCoverages.Columns.Add("CONDITION_TEXT", typeof(string));
            dtCoverages.Columns.Add("ENDORSEMENT_LIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("ENDORSEMENT_SUBLIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("FLAT_RATE_PCT", typeof(decimal));
            dtCoverages.Columns.Add("CONTRACT_AMOUNT_PCT", typeof(decimal));
            dtCoverages.Columns.Add("DYNAMIC_PROPERTIES", typeof(byte[]));
            dtCoverages.Columns.Add("SHORT_TERM_PCT", typeof(decimal));
            dtCoverages.Columns.Add("PREMIUM_AMT_DEPOSIT_PERCENT", typeof(decimal));
            dtCoverages.Columns.Add("MAX_LIABILITY_AMT", typeof(decimal));

            DataTable dtDeductibles = new DataTable("PARAM_TEMP_RISK_COVER_DEDUCT");
            dtDeductibles.Columns.Add("COVERAGE_ID", typeof(int));
            dtDeductibles.Columns.Add("RATE_TYPE_CD", typeof(int));
            dtDeductibles.Columns.Add("RATE", typeof(decimal));
            dtDeductibles.Columns.Add("DEDUCT_PREMIUM_AMT", typeof(int));
            dtDeductibles.Columns.Add("DEDUCT_VALUE", typeof(int));
            dtDeductibles.Columns.Add("DEDUCT_UNIT_CD", typeof(int));
            dtDeductibles.Columns.Add("DEDUCT_SUBJECT_CD", typeof(int));
            dtDeductibles.Columns.Add("MIN_DEDUCT_VALUE", typeof(decimal));
            dtDeductibles.Columns.Add("MIN_DEDUCT_UNIT_CD", typeof(int));
            dtDeductibles.Columns.Add("MIN_DEDUCT_SUBJECT_CD", typeof(int));
            dtDeductibles.Columns.Add("MAX_DEDUCT_VALUE", typeof(decimal));
            dtDeductibles.Columns.Add("MAX_DEDUCT_UNIT_CD", typeof(int));
            dtDeductibles.Columns.Add("MAX_DEDUCT_SUBJECT_CD", typeof(int));
            dtDeductibles.Columns.Add("CURRENCY_CD", typeof(int));
            dtDeductibles.Columns.Add("ACC_DEDUCT_AMT", typeof(decimal));
            dtDeductibles.Columns.Add("DEDUCT_ID", typeof(int));

            foreach (CompanyCoverage item in companyTplRisk.Risk.Coverages)
            {
                DataRow dataRow = dtCoverages.NewRow();
                dataRow["COVERAGE_ID"] = item.Id;
                dataRow["IS_DECLARATIVE"] = item.IsDeclarative;
                dataRow["IS_MIN_PREMIUM_DEPOSIT"] = item.IsMinPremiumDeposit;
                dataRow["FIRST_RISK_TYPE_CD"] = (int)Sistran.Core.Application.UnderwritingServices.Enums.FirstRiskType.None;
                dataRow["CALCULATION_TYPE_CD"] = item.CalculationType.Value;
                dataRow["DECLARED_AMT"] = item.DeclaredAmount;
                dataRow["PREMIUM_AMT"] = Math.Round(item.PremiumAmount, 2);
                dataRow["LIMIT_AMT"] = item.LimitAmount;
                dataRow["SUBLIMIT_AMT"] = item.SubLimitAmount;
                dataRow["LIMIT_IN_EXCESS"] = item.ExcessLimit;
                dataRow["LIMIT_OCCURRENCE_AMT"] = item.LimitOccurrenceAmount;
                dataRow["LIMIT_CLAIMANT_AMT"] = item.LimitClaimantAmount;
                dataRow["ACC_PREMIUM_AMT"] = item.AccumulatedPremiumAmount;
                dataRow["ACC_LIMIT_AMT"] = item.AccumulatedLimitAmount;
                dataRow["ACC_SUBLIMIT_AMT"] = item.AccumulatedSubLimitAmount;
                dataRow["CURRENT_FROM"] = item.CurrentFrom;
                dataRow["RATE_TYPE_CD"] = item.RateType;
                dataRow["RATE"] = (object)item.Rate ?? DBNull.Value;
                dataRow["CURRENT_TO"] = item.CurrentTo;
                dataRow["COVER_NUM"] = item.Number;
                dataRow["COVER_STATUS_CD"] = item.CoverStatus.Value;
                dataRow["MAX_LIABILITY_AMT"] = item.MaxLiabilityAmount;
                if (item.CoverageOriginalStatus.HasValue)
                {
                    dataRow["COVER_ORIGINAL_STATUS_CD"] = item.CoverageOriginalStatus.Value;
                }
                if (item.Text != null)
                {
                    dataRow["CONDITION_TEXT"] = item.Text.TextBody;
                }
                dataRow["ENDORSEMENT_LIMIT_AMT"] = item.EndorsementLimitAmount;
                dataRow["ENDORSEMENT_SUBLIMIT_AMT"] = item.EndorsementSublimitAmount;
                dataRow["FLAT_RATE_PCT"] = item.FlatRatePorcentage;
                dataRow["SHORT_TERM_PCT"] = item.ShortTermPercentage;
                dataRow["PREMIUM_AMT_DEPOSIT_PERCENT"] = item.DepositPremiumPercent;
                dataRow["MAX_LIABILITY_AMT"] = item.MaxLiabilityAmount;

                if (item.DynamicProperties != null && item.DynamicProperties.Count > 0)
                {
                    DynamicPropertiesCollection dynamicCollectionCoverage = new DynamicPropertiesCollection();
                    for (int i = 0; i < item.DynamicProperties.Count(); i++)
                    {
                        DynamicProperty dinamycProperty = new DynamicProperty();
                        dinamycProperty.Id = item.DynamicProperties[i].Id;
                        dinamycProperty.Value = item.DynamicProperties[i].Value;
                        dynamicCollectionCoverage[i] = dinamycProperty;
                    }

                    byte[] serializedValuesCoverage = dynamicPropertiesSerializer.Serialize(dynamicCollectionCoverage);
                    dataRow["DYNAMIC_PROPERTIES"] = serializedValuesCoverage;
                }

                if (item.Deductible != null)
                {
                    DataRow dataRowDeductible = dtDeductibles.NewRow();
                    dataRowDeductible["COVERAGE_ID"] = item.Id;
                    dataRowDeductible["RATE_TYPE_CD"] = item.Deductible.RateType;
                    dataRowDeductible["RATE"] = (object)item.Deductible.Rate ?? DBNull.Value;
                    dataRowDeductible["DEDUCT_PREMIUM_AMT"] = Math.Round(item.Deductible.DeductPremiumAmount, 2);
                    dataRowDeductible["DEDUCT_VALUE"] = item.Deductible.DeductValue;
                    if (item.Deductible.DeductibleUnit != null && item.Deductible.DeductibleUnit.Id != 0)
                    {
                        dataRowDeductible["DEDUCT_UNIT_CD"] = item.Deductible.DeductibleUnit.Id;
                    }
                    if (item.Deductible.DeductibleSubject != null)
                    {
                        dataRowDeductible["DEDUCT_SUBJECT_CD"] = item.Deductible.DeductibleSubject.Id;
                    }
                    if (item.Deductible.MinDeductValue.HasValue)
                    {
                        dataRowDeductible["MIN_DEDUCT_VALUE"] = item.Deductible.MinDeductValue.Value;
                    }
                    if (item.Deductible.MinDeductibleUnit != null && item.Deductible.MinDeductibleUnit.Id != 0)
                    {
                        dataRowDeductible["MIN_DEDUCT_UNIT_CD"] = item.Deductible.MinDeductibleUnit.Id;
                    }
                    if (item.Deductible.MinDeductibleSubject != null && item.Deductible.MinDeductibleSubject.Id != 0)
                    {
                        dataRowDeductible["MIN_DEDUCT_SUBJECT_CD"] = item.Deductible.MinDeductibleSubject.Id;
                    }
                    if (item.Deductible.MaxDeductValue.HasValue)
                    {
                        dataRowDeductible["MAX_DEDUCT_VALUE"] = item.Deductible.MaxDeductValue.Value;
                    }
                    if (item.Deductible.MaxDeductibleUnit != null && item.Deductible.MaxDeductibleUnit.Id != 0)
                    {
                        dataRowDeductible["MAX_DEDUCT_UNIT_CD"] = item.Deductible.MaxDeductibleUnit.Id;
                    }
                    if (item.Deductible.MaxDeductibleSubject != null && item.Deductible.MaxDeductibleSubject.Id != 0)
                    {
                        dataRowDeductible["MAX_DEDUCT_SUBJECT_CD"] = item.Deductible.MaxDeductibleSubject.Id;
                    }
                    if (item.Deductible.Currency != null)
                    {
                        dataRowDeductible["CURRENCY_CD"] = item.Deductible.Currency.Id;
                    }
                    dataRowDeductible["ACC_DEDUCT_AMT"] = item.Deductible.AccDeductAmt;
                    dataRowDeductible["DEDUCT_ID"] = item.Deductible.Id;

                    dtDeductibles.Rows.Add(dataRowDeductible);
                }

                dtCoverages.Rows.Add(dataRow);
            }

            parameters[46] = new NameValue("@INSERT_TEMP_RISK_COVERAGE", dtCoverages);
            parameters[47] = new NameValue("@INSERT_TEMP_RISK_COVER_DEDUCT", dtDeductibles);

            DataTable dtAccessories = new DataTable("PARAM_TEMP_RISK_DETAIL_ACCESSORY");
            dtAccessories.Columns.Add("SUBLIMIT_AMT", typeof(decimal));
            dtAccessories.Columns.Add("RATE_TYPE_CD", typeof(int));
            dtAccessories.Columns.Add("RATE", typeof(decimal));
            dtAccessories.Columns.Add("PREMIUM_AMT", typeof(decimal));
            dtAccessories.Columns.Add("ACC_PREMIUM_AMT", typeof(decimal));
            dtAccessories.Columns.Add("BRAND_NAME", typeof(string));
            dtAccessories.Columns.Add("MODEL", typeof(string));
            dtAccessories.Columns.Add("DETAIL_ID", typeof(int));
            dtAccessories.Columns.Add("COVERAGE_ID", typeof(int));
            dtAccessories.Columns.Add("COVER_STATUS_CD", typeof(int));

            parameters[48] = new NameValue("@INSERT_TEMP_RISK_DETAIL_ACCESSORY", dtAccessories);

            DataTable dtClauses = new DataTable("PARAM_TEMP_CLAUSE");
            dtClauses.Columns.Add("CLAUSE_ID", typeof(int));
            dtClauses.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtClauses.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
            dtClauses.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

            if (companyTplRisk.Risk.Clauses != null)
            {
                foreach (CompanyClause item in companyTplRisk.Risk.Clauses)
                {
                    DataRow dataRow = dtClauses.NewRow();
                    dataRow["CLAUSE_ID"] = item.Id;
                    dataRow["CLAUSE_STATUS_CD"] = (int)Sistran.Core.Application.CommonService.Enums.ClauseStatuses.Original;
                    dtClauses.Rows.Add(dataRow);
                }
            }

            parameters[49] = new NameValue("@INSERT_TEMP_CLAUSE", dtClauses);

            DataTable dtDynamicProperties = new DataTable("PARAM_TEMP_DYNAMIC_PROPERTIES");
            dtDynamicProperties.Columns.Add("DYNAMIC_ID", typeof(int));
            dtDynamicProperties.Columns.Add("CONCEPT_VALUE", typeof(string));

            if (companyTplRisk.Risk.DynamicProperties != null)
            {
                foreach (DynamicConcept item in companyTplRisk.Risk.DynamicProperties)
                {
                    DataRow dataRow = dtDynamicProperties.NewRow();
                    dataRow["DYNAMIC_ID"] = item.Id;
                    dataRow["CONCEPT_VALUE"] = item.Value ?? "NO ASIGNADO";
                    dtDynamicProperties.Rows.Add(dataRow);
                }
            }

            parameters[50] = new NameValue("@INSERT_TEMP_DYNAMIC_PROPERTIES", dtDynamicProperties);

            DataTable dtDynamicPropertiesCoverage = new DataTable("PARAM_TEMP_DYNAMIC_PROPERTIES");
            dtDynamicPropertiesCoverage.Columns.Add("DYNAMIC_ID", typeof(int));
            dtDynamicPropertiesCoverage.Columns.Add("CONCEPT_VALUE", typeof(string));

            parameters[51] = new NameValue("@INSERT_TEMP_DYNAMIC_PROPERTIES_COVERAGE", dtDynamicPropertiesCoverage);
            parameters[52] = new NameValue("@RISK_NUM", companyTplRisk.Risk.Number);
            parameters[53] = new NameValue("@RISK_INSP_TYPE_CD", 1);
            parameters[54] = new NameValue("@INSPECTION_ID", DBNull.Value, DbType.Int32);
            parameters[55] = new NameValue("OPERATION", JsonConvert.SerializeObject(companyTplRisk));
            ///personalizacion 
            parameters[56] = new NameValue("@GALLON_QTY", companyTplRisk.GallonTankCapacity);
            parameters[57] = new NameValue("@IS_TRANSFORM_VEHICLE", companyTplRisk.RePoweredVehicle);
            parameters[58] = new NameValue("@YEAR_TRANSFORM_VEHICLE", companyTplRisk.RepoweringYear);
            parameters[59] = new NameValue("@TRANSPORT_CARGO_TYPE_CD", companyTplRisk.TypeCargoId);
            parameters[60] = new NameValue("@YEAR_MODEL", companyTplRisk.YearModel);
            parameters[61] = new NameValue("@RETENTION", companyTplRisk.Risk.IsRetention);

            DataTable result;

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                // result = pdb.ExecuteSPDataTable("ISS.RECORD_RISK_VEHICLE", parameters);
                result = pdb.ExecuteSPDataTable("ISS.RECORD_RISK_VEHICLE", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                if (!Convert.ToBoolean(result.Rows[0][0]))
                {
                    throw new ValidationException((string)result.Rows[0][1]);
                }
            }
            else
            {
                throw new ValidationException(Errors.ErrorRecordEndorsement);
            }
        }

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        public CompanyTplRisk GetCompanyTplRiskByRiskId(int riskId)
        {
            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(riskId);

            if (pendingOperation != null)
            {
                CompanyTplRisk companyTplRisk = COMUT.JsonHelper.DeserializeJson<CompanyTplRisk>(pendingOperation.Operation);
                companyTplRisk.Risk.Id = pendingOperation.Id;
                companyTplRisk.Risk.IsPersisted = true;

                return companyTplRisk;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///  Realiza el llamado de cada uno de los métodos que crean los datatable del riesgo y ejecuta el procedimiento de almacenado
        /// </summary>
        /// <param name="companyTplRisk"></param>
        /// <returns></returns>
        public CompanyTplRisk SaveCompanyTplTemporalTables(CompanyTplRisk companyTplRisk)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();
            DataTable dataTable;

            NameValue[] parameters = new NameValue[18];

            UTILITIES.GetDatatables dts = new UTILITIES.GetDatatables();
            UTILITIES.CommonDataTables datatables = dts.GetcommonDataTables(companyTplRisk.Risk);

            DataTable dtTempRisk = datatables.dtTempRisk;
            parameters[0] = new NameValue(dtTempRisk.TableName, dtTempRisk);

            DataTable dtCOTempRisk = datatables.dtCOTempRisk;
            parameters[1] = new NameValue(dtCOTempRisk.TableName, dtCOTempRisk);

            DataTable dtBeneficary = datatables.dtBeneficary;
            parameters[2] = new NameValue(dtBeneficary.TableName, dtBeneficary);

            DataTable dtRiskPayer = datatables.dtRiskPayer;
            parameters[3] = new NameValue(dtRiskPayer.TableName, dtRiskPayer);

            DataTable dtRiskClause = datatables.dtRiskClause;
            parameters[4] = new NameValue(dtRiskClause.TableName, dtRiskClause);

            DataTable dtRiskCoverage = datatables.dtRiskCoverage; //UTILITIES.ModelAssembler.GetDataTableRisCoverage(liabilityRisk.Risk);
            parameters[5] = new NameValue(dtRiskCoverage.TableName, dtRiskCoverage);

            DataTable dtDeduct = datatables.dtDeduct;
            parameters[6] = new NameValue(dtDeduct.TableName, dtDeduct);

            DataTable dtCoverClause = datatables.dtCoverClause;
            parameters[7] = new NameValue(dtCoverClause.TableName, dtCoverClause);

            DataTable dtDynamicRisk = datatables.dtDynamic;
            parameters[8] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_RISK", dtDynamicRisk);

            DataTable dtDynamicCoverage = datatables.dtDynamicCoverage;
            parameters[9] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_COVERAGE", dtDynamicCoverage);

            //Accesorios
            DataTable dataTableAcc = new DataTable("INSERT_TEMP_RISK_DETAIL_ACCESSORY");

            dataTableAcc.Columns.Add("SUBLIMIT_AMT", typeof(decimal));
            dataTableAcc.Columns.Add("RATE_TYPE_CD", typeof(int));
            dataTableAcc.Columns.Add("RATE", typeof(decimal));
            dataTableAcc.Columns.Add("PREMIUM_AMT", typeof(decimal));
            dataTableAcc.Columns.Add("ACC_PREMIUM_AMT", typeof(decimal));
            dataTableAcc.Columns.Add("BRAND_NAME", typeof(string));
            dataTableAcc.Columns.Add("MODEL", typeof(string));
            dataTableAcc.Columns.Add("DETAIL_ID", typeof(int));
            dataTableAcc.Columns.Add("COVERAGE_ID", typeof(int));
            dataTableAcc.Columns.Add("COVER_STATUS_CD", typeof(int));
            parameters[10] = new NameValue(dataTableAcc.TableName, dataTableAcc);
            //

            DataTable dtTempRiskLocation = ModelAssembler.SetDataTableTempRiskVehicle(companyTplRisk);
            parameters[11] = new NameValue(dtTempRiskLocation.TableName, dtTempRiskLocation);

            DataTable dtTempcoRiskLocation = ModelAssembler.GetDataTableTemRiskVehicle(companyTplRisk);
            parameters[12] = new NameValue(dtTempcoRiskLocation.TableName, dtTempcoRiskLocation);

            parameters[13] = new NameValue("@GALLON_QTY", companyTplRisk.GallonTankCapacity);
            parameters[14] = new NameValue("@IS_TRANSFORM_VEHICLE", companyTplRisk.RePoweredVehicle);
            parameters[15] = new NameValue("@YEAR_TRANSFORM_VEHICLE", companyTplRisk.RepoweringYear);
            parameters[16] = new NameValue("@TRANSPORT_CARGO_TYPE_CD", companyTplRisk.TypeCargoId);
            parameters[17] = new NameValue("@YEAR_MODEL", companyTplRisk.YearModel);



            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("TMP.SAVE_TEMPORAL_RISK_VEHICLE_TEMP", parameters);

            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (companyTplRisk.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
                {
                    companyTplRisk.Risk.RiskId = Convert.ToInt32(dataTable.Rows[0][0]);
                }
                return companyTplRisk;
            }
            else
            {
                throw new ValidationException(Errors.ErrorCreateTemporalCompanyThirdpartyLiability);//ErrrRecordTemporal "error al grabar riesgo
            }
        }

        public CompanyTplRisk SavecompanyTplRiskTemporal(CompanyTplRisk companyTplRisk)
        {


            IDynamicPropertiesSerializer dynamicPropertiesSerializer =
            new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();

            DataTable dataTable;
            NameValue[] parameters = new NameValue[9];

            DataTable dtTempRisk = ModelAssembler.GetDataTableTempRISK(companyTplRisk);
            parameters[0] = new NameValue(dtTempRisk.TableName, dtTempRisk);

            DataTable dtCOTempRisk = ModelAssembler.GetDataTableCOTempRisk(companyTplRisk);
            parameters[1] = new NameValue(dtCOTempRisk.TableName, dtCOTempRisk);

            DataTable dtTempRiskVehicle = ModelAssembler.GetDataTableRiskVehicle(companyTplRisk);
            parameters[2] = new NameValue(dtTempRiskVehicle.TableName, dtTempRiskVehicle);

            DataTable dtCOTempRiskVehicle = ModelAssembler.GetDataTableTemRiskVehicle(companyTplRisk);
            parameters[3] = new NameValue(dtCOTempRiskVehicle.TableName, dtCOTempRiskVehicle);
            parameters[4] = new NameValue("@GALLON_QTY", companyTplRisk.GallonTankCapacity);
            parameters[5] = new NameValue("@IS_TRANSFORM_VEHICLE", companyTplRisk.RePoweredVehicle);
            parameters[6] = new NameValue("@YEAR_TRANSFORM_VEHICLE", companyTplRisk.RepoweringYear);
            parameters[7] = new NameValue("@TRANSPORT_CARGO_TYPE_CD", companyTplRisk.TypeCargoId);
            parameters[8] = new NameValue("@YEAR_MODEL", companyTplRisk.YearModel);

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("TMP.CIA_SAVE_TEMPORAL_RISK_VEHICLE", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (companyTplRisk.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
                {
                    companyTplRisk.Risk.RiskId = Convert.ToInt32(dataTable.Rows[0][0]);
                }
                return companyTplRisk;
            }
            else
            {
                throw new ValidationException(Errors.ErrorCreateTemporalCompanyThirdpartyLiability);
            }
        }

        public CompanyTplRisk SaveCompanyTplTemporal(CompanyTplRisk companyTplRisk)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer =
            new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();

            DataTable dataTable;
            NameValue[] parameters = new NameValue[4];

            UTILITIES.GetDatatables dts = new UTILITIES.GetDatatables();
            UTILITIES.CommonDataTables datatables = dts.GetcommonDataTables(companyTplRisk.Risk);

            DataTable dtTempRisk = datatables.dtTempRisk;
            parameters[0] = new NameValue(dtTempRisk.TableName, dtTempRisk);

            DataTable dtCOTempRisk = datatables.dtCOTempRisk;
            parameters[1] = new NameValue(dtCOTempRisk.TableName, dtCOTempRisk);

            DataTable dtTempRiskVehicle = ModelAssembler.SetDataTableTempRiskVehicle(companyTplRisk);
            parameters[2] = new NameValue(dtTempRiskVehicle.TableName, dtTempRiskVehicle);

            DataTable dtCOTempRiskVehicle = ModelAssembler.SetDataTableTempRiskVehicle(companyTplRisk);
            parameters[3] = new NameValue(dtCOTempRiskVehicle.TableName, dtCOTempRiskVehicle);

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {

                dataTable = pdb.ExecuteSPDataTable("TMP.CIA_SAVE_TEMPORAL_RISK_VEHICLE", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (companyTplRisk.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
                {
                    companyTplRisk.Risk.RiskId = Convert.ToInt32(dataTable.Rows[0][0]);
                }
                return companyTplRisk;
            }
            else
            {
                throw new ValidationException(Errors.ErrorCreateTemporalCompanyVehicle);
            }
        }
        

        public List<PoliciesAut> ValidateAuthorizationPoliciesMassive(CompanyTplRisk companyTpl, int hierarchy, List<int> ruleToValidateRisk, List<int> ruleToValidateCoverage)
        {
            if (companyTpl.Risk != null && companyTpl.Risk.Policy != null)
            {
                companyTpl.Risk.Policy.SinisterQuantity = GetCountSinister(companyTpl);
                companyTpl.Risk.Policy.HasTotalLoss = GetHasTotalLoss(companyTpl);
                companyTpl.Risk.Policy.PortfolioBalance = GetHasPortfolioBalance(companyTpl);
            }

            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();
            if (companyTpl != null && companyTpl.Risk.Policy != null)
            {
                string key = companyTpl.Risk.Policy.Prefix.Id + "," + (int)companyTpl.Risk.Policy.Product.CoveredRisk.CoveredRiskType;
                EntityAssembler.CreateFacadeGeneral(facade, companyTpl.Risk.Policy);
                facade.SetConcept(CompanyRuleConceptPolicies.UserId, companyTpl.Risk.Policy.UserId);
                EntityAssembler.CreateFacadeRiskVehicle(facade, companyTpl);
                /*Politica del riesgo*/
                if (ruleToValidateRisk.Any())
                {
                    policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPoliciesMassive(10, key, facade, FacadeType.RULE_FACADE_RISK, hierarchy, ruleToValidateRisk));
                }

                /*Politicas de cobertura*/
                if (companyTpl.Risk.Coverages != null && ruleToValidateCoverage.Any())
                {
                    foreach (CompanyCoverage coverage in companyTpl.Risk.Coverages)
                    {
                        EntityAssembler.CreateFacadeCoverage(facade, coverage);
                        policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPoliciesMassive(10, key, facade, FacadeType.RULE_FACADE_COVERAGE, hierarchy, ruleToValidateCoverage));
                    }
                }
            }
            return policiesAuts;
        }
        /// <summary>
        /// Cantidad de siniestros por poliza
        /// </summary>
        /// <param name="Policy"></param>
        /// <returns></returns>
        public int GetCountSinister(CompanyTplRisk companyTpl)
        {
            int recordCount = 0;
            if (companyTpl.Risk.Policy.DocumentNumber != 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(TEM.FianzaPolicyClaims.Properties.PolicyNum, typeof(TEM.FianzaPolicyClaims).Name);
                filter.Equal();
                filter.Constant(companyTpl.Risk.Policy.DocumentNumber);
                filter.And();
                filter.Property(TEM.FianzaPolicyClaims.Properties.BranchCode, typeof(TEM.FianzaPolicyClaims).Name);
                filter.Equal();
                filter.Constant(companyTpl.Risk.Policy.Branch.Id);
                filter.And();
                filter.Property(TEM.FianzaPolicyClaims.Properties.PrefixCode, typeof(TEM.FianzaPolicyClaims).Name);
                filter.Equal();
                filter.Constant(companyTpl.Risk.Policy.Prefix.Id);
                BusinessCollection<TEM.FianzaPolicyClaims> result = DataFacadeManager.Instance.GetDataFacade().List<TEM.FianzaPolicyClaims>(filter.GetPredicate());
                DataFacadeManager.Dispose();
                if (result.Count > 0)
                {
                    recordCount = result.Count;
                }
            }
            return recordCount;
        }
        /// <summary>
        /// Si tiene siniestros con perdida total
        /// </summary>
        /// <param name="policyNum"></param>
        /// <returns></returns>
        public bool GetHasTotalLoss(CompanyTplRisk companyTpl)
        {
            if (companyTpl.Risk.Policy.DocumentNumber != 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(TEM.VehiclePolicyClaims.Properties.PolicyNum, typeof(TEM.VehiclePolicyClaims).Name);
                filter.Equal();
                filter.Constant(companyTpl.Risk.Policy.DocumentNumber);
                filter.And();
                filter.Property(TEM.VehiclePolicyClaims.Properties.PolicyNum, typeof(TEM.VehiclePolicyClaims).Name);
                filter.Equal();
                filter.Constant(true);
                filter.And();
                filter.Property(TEM.VehiclePolicyClaims.Properties.BranchCode, typeof(TEM.VehiclePolicyClaims).Name);
                filter.Equal();
                filter.Constant(companyTpl.Risk.Policy.Branch.Id);
                filter.And();
                filter.Property(TEM.VehiclePolicyClaims.Properties.PrefixCode, typeof(TEM.VehiclePolicyClaims).Name);
                filter.Equal();
                filter.Constant(companyTpl.Risk.Policy.Prefix.Id);
                BusinessCollection<TEM.VehiclePolicyClaims> result = DataFacadeManager.Instance.GetDataFacade().List<TEM.VehiclePolicyClaims>(filter.GetPredicate());
                DataFacadeManager.Dispose();
                if (result.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Tiene carter pendiente
        /// </summary>
        /// <param name="policyNum"></param>
        /// <returns></returns>
        public decimal GetHasPortfolioBalance(CompanyTplRisk companyTpl)
        {
            decimal ValorPortafolio = 0;
            if (companyTpl.Risk.Policy.DocumentNumber != 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(TEM.CoBorrowingPolicy.Properties.PolicyNum, typeof(TEM.CoBorrowingPolicy).Name);
                filter.Equal();
                filter.Constant(companyTpl.Risk.Policy.DocumentNumber);
                filter.And();
                filter.Property(TEM.CoBorrowingPolicy.Properties.PolicyNum, typeof(TEM.CoBorrowingPolicy).Name);
                filter.Equal();
                filter.Constant(true);
                filter.And();
                filter.Property(TEM.CoBorrowingPolicy.Properties.BranchCode, typeof(TEM.CoBorrowingPolicy).Name);
                filter.Equal();
                filter.Constant(companyTpl.Risk.Policy.Branch.Id);
                filter.And();
                filter.Property(TEM.CoBorrowingPolicy.Properties.PrefixCode, typeof(TEM.CoBorrowingPolicy).Name);
                filter.Equal();
                filter.Constant(companyTpl.Risk.Policy.Prefix.Id);
                BusinessCollection<TEM.CoBorrowingPolicy> result = DataFacadeManager.Instance.GetDataFacade().List<TEM.CoBorrowingPolicy>(filter.GetPredicate());
                DataFacadeManager.Dispose();
                if (result.Count > 0)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        ValorPortafolio += result[i].PremiumAmount;
                    }
                }
            }
            return ValorPortafolio;
        }

        /// <summary>
        /// Obtener lista de versiones por marca y modelo
        /// </summary>
        /// <param name="makeId">Id marca</param>
        /// <param name="year">aÑO</param>
        /// <returns>version</returns>
        public VEMO.Version GetVersionsByMakeIdYear(int makeId, int year)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(VehicleVersionYear.Properties.VehicleMakeCode, typeof(VehicleVersionYear).Name);
            filter.Equal();
            filter.Constant(makeId);
            filter.And();
            filter.Property(VehicleVersionYear.Properties.VehicleYear, typeof(VehicleVersionYear).Name);
            filter.Equal();
            filter.Constant(year);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleVersionYear), filter.GetPredicate()));

            VehicleVersionYear vehicleVersionYear = businessCollection.Cast<VehicleVersionYear>().FirstOrDefault();

            return ModelAssembler.CreateVersion(vehicleVersionYear);

        }

        public List<Validation> GetVehicleLicensePlate(List<Validation> validations, List<ValidationLicensePlate> validationsLicensePlate)
        {
            if (validationsLicensePlate.Count > 0)
            {
                List<Validation> resultValidationsLicensePlate = new List<Validation>();
                NameValue[] parameters = new NameValue[2];
                DataTable dtLicensePlate = new DataTable("VALIDATE_LICENSE_PLATE");
                dtLicensePlate.Columns.Add("LICENSE_PLATE", typeof(string));
                dtLicensePlate.Columns.Add("ENGINE_SER_NO", typeof(string));
                dtLicensePlate.Columns.Add("CHASSIS_SER_NO", typeof(string));
                dtLicensePlate.Columns.Add("FEC_VIG_DESDE", typeof(DateTime));
                dtLicensePlate.Columns.Add("FEC_VIG_HASTA", typeof(DateTime));
                dtLicensePlate.Columns.Add("IDENTIFICATOR", typeof(int));

                foreach (ValidationLicensePlate validationLicensePlate in validationsLicensePlate)
                {
                    DataRow dataRow = dtLicensePlate.NewRow();
                    dataRow["LICENSE_PLATE"] = validationLicensePlate.LicensePlate;
                    dataRow["ENGINE_SER_NO"] = validationLicensePlate.Engine;
                    dataRow["CHASSIS_SER_NO"] = validationLicensePlate.Chassis;
                    dataRow["FEC_VIG_DESDE"] = validationLicensePlate.CurrentFrom;
                    if (validationLicensePlate.CurrentTo != null)
                    {
                        dataRow["FEC_VIG_HASTA"] = validationLicensePlate.CurrentTo;
                    }
                    else
                    {
                        dataRow["FEC_VIG_HASTA"] = DBNull.Value;
                    }
                    dataRow["IDENTIFICATOR"] = validationLicensePlate.Id;
                    dtLicensePlate.Rows.Add(dataRow);
                }

                parameters[0] = new NameValue("VALIDATE_LICENSE_PLATE", dtLicensePlate);
                parameters[1] = new NameValue("REQUEST_ID", validationsLicensePlate[0].ParameterValue);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ISS.VALIDATE_LICENSE_PLATE", parameters);
                }
                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in result.Rows)
                    {
                        resultValidationsLicensePlate.Add(new Validation
                        {
                            Id = (int)dataRow[0],
                            ErrorMessage = (string)dataRow[1]
                        });
                    }
                }
                if (validations.Count > 0)
                {
                    foreach (Validation item in resultValidationsLicensePlate)
                    {
                        if (validations.Find(x => x.Id == item.Id) != null)
                        {
                            validations.Find(x => x.Id == item.Id).ErrorMessage += " " + item.ErrorMessage;
                        }
                        else
                        {
                            Validation validation = new Validation();
                            validation.Id = item.Id;
                            validation.ErrorMessage = item.ErrorMessage;
                            validations.Add(validation);
                        }
                    }
                }
                else
                {
                    validations.AddRange(resultValidationsLicensePlate);
                }
            }

            return validations;
        }
    }
}