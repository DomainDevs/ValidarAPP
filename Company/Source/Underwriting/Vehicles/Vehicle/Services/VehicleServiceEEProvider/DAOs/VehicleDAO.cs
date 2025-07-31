using AutoMapper;
using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.Common.Entities;
using Sistran.Company.Application.Issuance.Entities;
using Sistran.Company.Application.Product.Entities;
using Sistran.Company.Application.Quotation.Entities;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Error;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.DTOs;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Assemblers;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Entities.views;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Entities.Views;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Application.Vehicles.EEProvider.Entities.Views;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Queues;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CiaPersonModel = Sistran.Company.Application.UniquePersonServices.V1.Models;
using CiUndrwritingModel = Sistran.Company.Application.UnderwritingServices.Models;
using COCOMMEN = Sistran.Company.Application.Common.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using CSMO = Sistran.Core.Application.CommonService.Models;
using CVEMOD = Sistran.Company.Application.Vehicles.Models;
using enUnder = Sistran.Core.Application.UnderwritingServices.Enums;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Rules = Sistran.Core.Framework.Rules;
using TEM = Sistran.Core.Application.Temporary.Entities;
using UNMOD = Sistran.Core.Application.UnderwritingServices.Models;
using UTILITES = Company.UnderwritingUtilities;
using VEDAO = Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.DAOs;
using VEMOD = Sistran.Core.Application.Vehicles.Models;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Company.Application.UnderwritingServices.DTOs;
using VehicleModelsDTO = Sistran.Company.Application.Vehicles.VehicleServices.DTOs;
using Sistran.Core.Application.Utilities.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.DAOs
{
    public class VehicleDAO
    {
        /// <summary>
        /// Obtener Poliza de vehiculos
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>Vehiclepolicy</returns>
        public List<CompanyVehicle> GetCompanyVehiclesByPolicyId(int policyId)
        {
            try
            {
                List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
                filter.Equal();
                filter.Constant(policyId);
                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
                filter.Equal();
                filter.Constant(true);
                filter.And();
                filter.Not();
                filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
                filter.In();
                filter.ListValue();
                filter.Constant((int)RiskStatusType.Excluded);
                filter.Constant((int)RiskStatusType.Cancelled);
                filter.EndList();

                return GetCompanyVehiclesByFilter(filter);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetBaseException().Message, ex);
            }

        }

        /// <summary>
        /// Obtener Poliza de vehiculos
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>Vehiclepolicy</returns>
        public List<CompanyVehicle> GetVehiclesByPolicyId(int policyId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();

            List<UNMOD.Endorsement> endorsement = DelegateService.underwritingService.GetEffectiveEndorsementsByPolicyId(policyId);

            //riesgo
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);
            filter.And();
            filter.Not();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(RiskStatusType.Excluded);

            RiskVehicleView view = new RiskVehicleView();
            ViewBuilder builder = new ViewBuilder("RiskVehicleView");
            builder.Filter = filter.GetPredicate();

            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            if (view.CompanyVehicles.Count > 0)
            {
                companyVehicles.AddRange(GetRisks(policyId, view));
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.DAOs.GetVehiclePolicyByPolicyId");

            return companyVehicles;
        }

        /// <summary>
        /// Obtener vehiculos
        /// </summary>
        /// <param name="policyList">Diccionario con llave PolicyNum y valor BranchId</param>
        /// <returns>CompanyVehicle List</returns>
        public List<CompanyVehicle> GetVehiclesByPolicyNumBranchIdPrefixList(Dictionary<string, int> policyList, List<int> prefixes)
        {
            object lockobject = new object();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            decimal count = decimal.Zero;

            foreach (KeyValuePair<string, int> entry in policyList)
            {
                filter.OpenParenthesis();
                filter.Property(ISSEN.Policy.Properties.PrefixCode, typeof(ISSEN.Policy).Name);
                filter.In();
                filter.ListValue();
                foreach (int p in prefixes)
                {
                    filter.Constant(p);
                }
                filter.EndList();
                filter.And();
                filter.Property(ISSEN.Policy.Properties.BranchCode, typeof(ISSEN.Policy).Name);
                filter.Equal();
                filter.Constant(entry.Value);
                filter.And();
                filter.Property(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name);
                filter.Equal();
                filter.Constant(entry.Key);
                filter.CloseParenthesis();

                count++;
                if (count < policyList.Count())
                {
                    filter.Or();
                }

            }
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);


            PolicyVehicleView view = new PolicyVehicleView();
            ViewBuilder builder = new ViewBuilder("PolicyVehicleView");

            builder.Filter = filter.GetPredicate();
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();

            if (view.RiskVehicles.Count > 0)
            {
                foreach (ISSEN.RiskVehicle entity in view.RiskVehicles)
                {
                    companyVehicles.Add(ModelAssembler.CreateVehicle(entity));
                }
            }
            return companyVehicles;
        }
        public List<CompanyVehicle> GetRisks(int policyId, RiskVehicleView view)
        {
            List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
            List<CompanyCoRisk> coRisks = view.CoRisks.Cast<CompanyCoRisk>().ToList();
            List<ISSEN.RiskVehicle> riskVehicles = view.RiskVehicles.Cast<ISSEN.RiskVehicle>().ToList();
            List<CompanyCoRiskVehicle> coRiskVehicles = view.CoRiskVehicles.Cast<CompanyCoRiskVehicle>().ToList();
            List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
            List<ISSEN.Risk> risks = view.CompanyVehicles.Cast<ISSEN.Risk>().ToList();

            List<CompanyVehicle> vehicles = new List<CompanyVehicle>();

            foreach (ISSEN.EndorsementRisk item in endorsementRisks)
            {
                using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.LoadDynamicProperties(risks.First(x => x.RiskId == item.RiskId));
                }
                CompanyVehicle vehicle = new CompanyVehicle();

                vehicle = ModelAssembler.CreateVehicle(risks.First(x => x.RiskId == item.RiskId),
                    coRisks.First(x => x.RiskId == item.RiskId),
                    riskVehicles.First(x => x.RiskId == item.RiskId),
                    coRiskVehicles.First(x => x.RiskId == item.RiskId),
                    endorsementRisks.First(x => x.RiskId == item.RiskId));
                CompanyIssuanceInsured companyInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(vehicle.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                if (companyInsured == null || companyInsured.IndividualId < 1)
                {
                    throw new Exception(Errors.ErrorInsuredMain);
                }
                vehicle.Risk.MainInsured = companyInsured;

                CompanyName companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(vehicle.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                vehicle.Risk.MainInsured.CompanyName = new IssuanceCompanyName
                {
                    NameNum = companyName.NameNum,
                    TradeName = companyName.TradeName,
                    Address = new IssuanceAddress
                    {
                        Id = companyName.Address.Id,
                        Description = companyName.Address.Description,
                        City = companyName.Address.City
                    }
                };
                if (companyName.Phone != null)
                {
                    vehicle.Risk.MainInsured.CompanyName.Phone = new IssuancePhone
                    {
                        Id = companyName.Phone.Id,
                        Description = companyName.Phone.Description
                    };
                }
                if (companyName.Email != null)
                {
                    vehicle.Risk.MainInsured.CompanyName.Email = new IssuanceEmail
                    {
                        Id = companyName.Email.Id,
                        Description = companyName.Email.Description
                    };
                }

                vehicle.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                if (riskBeneficiaries != null && riskBeneficiaries.Count > 0)
                {
                    object objlock = new object();
                    TP.Parallel.ForEach(riskBeneficiaries.Where(x => x.RiskId == item.RiskId), riskBeneficiary =>
                    {
                        CompanyBeneficiary beneficiary = new CompanyBeneficiary();
                        beneficiary = ModelAssembler.CreateBeneficiary(riskBeneficiary);

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

                        lock (objlock)
                        {
                            beneficiary.CompanyName = new IssuanceCompanyName
                            {
                                NameNum = companyName.NameNum,
                                TradeName = companyName.TradeName
                            };
                            if (companyName.Address != null)
                            {
                                beneficiary.CompanyName.Address = new IssuanceAddress
                                {
                                    Id = companyName.Address.Id,
                                    Description = companyName.Address.Description,
                                    City = companyName.Address.City
                                };
                            }
                            if (companyName.Phone != null)
                            {
                                beneficiary.CompanyName.Phone = new IssuancePhone
                                {
                                    Id = companyName.Phone.Id,
                                    Description = companyName.Phone.Description
                                };
                            }
                            if (companyName.Email != null)
                            {
                                beneficiary.CompanyName.Email = new IssuanceEmail
                                {
                                    Id = companyName.Email.Id,
                                    Description = companyName.Email.Description
                                };
                            }
                            vehicle.Risk.Beneficiaries.Add(beneficiary);
                        }
                    });
                }
                CompanyVehicle vehicleFasecolda = GetVehicleByMakeIdModelIdVersionId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id);
                vehicle.Fasecolda = vehicleFasecolda.Fasecolda;

                //coberturas
                List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();

                companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, item.EndorsementId, item.RiskId);
                vehicle.Risk.Coverages = companyCoverages;

                //accesorios
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                filter.Equal();
                filter.Constant(item.RiskId);
                AccessoryView accessoryView = new AccessoryView();
                ViewBuilder builder = new ViewBuilder("AccessoryView");
                builder.Filter = filter.GetPredicate();
                using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.FillView(builder, accessoryView);
                }

                Sistran.Core.Application.CommonService.Models.Parameter parameter = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories);

                if (accessoryView.RiskCoverDetails != null && accessoryView.RiskCoverDetails.Count > 0)
                {
                    vehicle.Accesories = ModelAssembler.CreateAccesories(accessoryView);

                    foreach (Accessory accessory in vehicle.Accesories)
                    {
                        if (vehicle.Risk.Coverages.First(x => x.RiskCoverageId == Convert.ToInt32(accessory.AccessoryId)).Id == parameter.NumberParameter.Value)
                        {
                            accessory.IsOriginal = true;
                        }
                    }

                    vehicle.PriceAccesories = vehicle.Accesories.Where(x => !x.IsOriginal).Sum(y => y.Amount);
                }

                vehicles.Add(vehicle);
            }

            return vehicles;
        }

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="vehicle">Modelo vehicle</param>
        public CompanyVehicle CreateVehicleTemporal(CompanyVehicle companyVehicle, bool isMassive)
        {
            companyVehicle.Risk.InfringementPolicies = ValidateAuthorizationPolicies(companyVehicle);
            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";
            PendingOperation pendingOperation = new PendingOperation();
            CompanyPolicy policy = companyVehicle.Risk.Policy;
            companyVehicle.Risk.Policy = null;

            if (companyVehicle.Risk.Id == 0)
            {
                pendingOperation.CreationDate = DateTime.Now;
                pendingOperation.ParentId = policy.Id;
                pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyVehicle);

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
                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(companyVehicle.Risk.Id);
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

            companyVehicle.Risk.Id = pendingOperation.Id;
            companyVehicle.Risk.Policy = policy;
            ////****************************GUARDAR TEMPORAL*********************************//
            int riskId = SaveCompanyVehicleTemporalTables(companyVehicle);
            if (companyVehicle.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
            {
                companyVehicle.Risk.RiskId = riskId;
            }
            ////****************************************************************************//

            pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyVehicle);

            if (isMassive && boolUseReplicatedDatabase)
            {
                //Se guarda el JSON en la base de datos de réplica
                DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
            }
            else
            {
                DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
            }
            companyVehicle.Risk.Id = pendingOperation.Id;
            return companyVehicle;
        }

        public List<CompanyVehicle> GetRisksThread(int policyId, List<ISSEN.Risk> risks, RiskVehicleView view)
        {
            if (risks == null || risks.Count < 1 || view.CompanyVehicles == null || view.CompanyVehicles.Count < 1)
            {
                throw new ArgumentException(Errors.ErrorRiskEmpty);
            }
            try
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<CompanyCoRisk> coRisks = view.CoRisks.Cast<CompanyCoRisk>().ToList();
                List<ISSEN.RiskVehicle> riskVehicles = view.RiskVehicles.Cast<ISSEN.RiskVehicle>().ToList();
                List<CompanyCoRiskVehicle> coRiskVehicles = view.CoRiskVehicles.Cast<CompanyCoRiskVehicle>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<COMMEN.VehicleModel> vehicleModels = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();
                List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
                List<COMMEN.VehicleVersion> vehicleVersions = view.VehicleVersions.Cast<COMMEN.VehicleVersion>().ToList();
                List<ISSEN.RiskClause> riskClauses = view.RiskClause.Cast<ISSEN.RiskClause>().ToList();

                List<CompanyVehicle> vehicles = new List<CompanyVehicle>();

                foreach (ISSEN.Risk item in risks)
                {
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.LoadDynamicProperties(item);
                    }
                    CompanyVehicle vehicle = new CompanyVehicle();

                    vehicle = ModelAssembler.CreateVehicle(item,
                        coRisks.First(x => x.RiskId == item.RiskId),
                        riskVehicles.First(x => x.RiskId == item.RiskId),
                        coRiskVehicles.First(x => x.RiskId == item.RiskId),
                        endorsementRisks.First(x => x.RiskId == item.RiskId));

                    vehicle.Make.Description = vehicleMakes.FirstOrDefault(x => x.VehicleMakeCode == vehicle.Make.Id).SmallDescription;
                    vehicle.Model.Description = vehicleModels.FirstOrDefault(x => x.VehicleModelCode == vehicle.Model.Id).SmallDescription;
                    vehicle.Version.Description = vehicleVersions.FirstOrDefault(x => x.VehicleVersionCode == vehicle.Version.Id).Description;

                    var companyInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(vehicle.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                    if (companyInsured == null || companyInsured.IndividualId < 1)
                    {
                        throw new Exception(Errors.ErrorInsuredMain);
                    }

                    vehicle.Risk.MainInsured = companyInsured;
                    vehicle.Risk.MainInsured.Name = vehicle.Risk.MainInsured.Name + " " + vehicle.Risk.MainInsured.Surname + " " + vehicle.Risk.MainInsured.SecondSurname;
                    ConcurrentBag<CompanyClause> clauses = new ConcurrentBag<CompanyClause>();
                    //clausulas
                    if (riskClauses != null && riskClauses.Count > 0)
                    {
                        TP.Parallel.ForEach(riskClauses.Where(x => x.RiskId == item.RiskId).ToList(), riskClause =>
                        {
                            clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
                        });
                        vehicle.Risk.Clauses = clauses.ToList();
                    }
                    var companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(vehicle.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
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

                    if (riskBeneficiaries != null && riskBeneficiaries.Count > 0)
                    {
                        vehicle.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                        Object objlock = new object();
                        var mapper = ModelAssembler.CreateMapCompanyBeneficiary();
                        TP.Parallel.ForEach(riskBeneficiaries.Where(x => x.RiskId == item.RiskId), riskBeneficiary =>
                        {
                            CompanyBeneficiary CiaBeneficiary = new CompanyBeneficiary();
                            var beneficiaryRisk = DelegateService.underwritingService.GetBeneficiariesByDescription(riskBeneficiary.BeneficiaryId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                            if (beneficiaryRisk != null)
                            {
                                CiaBeneficiary = mapper.Map<UNMOD.Beneficiary, CompanyBeneficiary>(beneficiaryRisk);
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
                                    vehicle.Risk.Beneficiaries.Add(CiaBeneficiary);
                                }
                            }
                        });
                    }
                    CompanyVehicle vehicleFasecolda = GetVehicleByMakeIdModelIdVersionId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id);
                    vehicle.Fasecolda = vehicleFasecolda.Fasecolda;

                    //coberturas
                    List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();

                    companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementRisks.First(x => x.RiskId == item.RiskId).EndorsementId, item.RiskId);
                    companyCoverages.AsParallel().ForAll(x =>
                    {
                        x.CoverageOriginalStatus = x.CoverageOriginalStatus;
                        x.OriginalRate = x.Rate;
                        x.CoverageOriginalStatus = x.CoverageOriginalStatus;
                        x.CoverStatus = CoverageStatusType.NotModified;
                    }
                    );
                    vehicle.Risk.Coverages = companyCoverages;

                    //accesorios
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(ISSEN.EndorsementRiskCoverage.Properties.PolicyId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filter.Equal();
                    filter.Constant(policyId);
                    filter.And();
                    filter.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filter.Equal();
                    filter.Constant(endorsementRisks.First(x => x.RiskId == item.RiskId).EndorsementId);
                    filter.And();
                    filter.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filter.Equal();
                    filter.Constant(item.RiskId);
                    filter.And();
                    filter.Not();
                    filter.Property(ISSEN.RiskCoverDetail.Properties.CoverStatusCode, typeof(ISSEN.RiskCoverDetail).Name);
                    filter.In();
                    filter.ListValue();
                    filter.Constant((int)CoverageStatusType.Excluded);
                    filter.Constant((int)CoverageStatusType.Cancelled);
                    filter.EndList();

                    AccessoryView accessoryView = new AccessoryView();
                    ViewBuilder builder = new ViewBuilder("AccessoryView");
                    builder.Filter = filter.GetPredicate();
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.FillView(builder, accessoryView);
                    }

                    Sistran.Core.Application.CommonService.Models.Parameter parameter = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories);

                    if (accessoryView.RiskCoverDetails != null && accessoryView.RiskCoverDetails.Count > 0)
                    {
                        vehicle.Accesories = ModelAssembler.CreateAccesories(accessoryView);
                        CompanyCoverage companyCoverageBase = vehicle.Risk.Coverages.FirstOrDefault(x => x.RiskCoverageId == Convert.ToInt32(vehicle.Accesories.First().AccessoryId));
                        foreach (Accessory accessory in vehicle.Accesories)
                        {
                            CompanyCoverage companyCoverage = vehicle.Risk.Coverages.FirstOrDefault(x => x.RiskCoverageId == Convert.ToInt32(accessory.AccessoryId));
                            if (companyCoverage != null && companyCoverage.Id == parameter.NumberParameter.Value)
                            {
                                accessory.IsOriginal = true;
                            }
                        }

                        vehicle.PriceAccesories = vehicle.Accesories.Where(x => !x.IsOriginal).Sum(y => y.Amount);


                    }
                    //AddCoveragesToRisk(vehicle, policyId, endorsementId, item.RiskId);
                    vehicles.Add(vehicle);
                }

                return vehicles;

            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public List<AccessoryDTO> GetPremiumAccesory(int policyId, int riskNumber, int days, bool isCancelation = false)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRiskCoverage.Properties.PolicyId, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskNum, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filter.Equal();
            filter.Constant(riskNumber);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskCoverDetail.Properties.RiskDetailId, typeof(ISSEN.RiskCoverDetail).Name)));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskCoverDetail.Properties.PremiumAmount, typeof(ISSEN.RiskCoverDetail).Name)));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskCoverage.Properties.CurrentFrom, typeof(ISSEN.RiskCoverage).Name)));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.RiskCoverage.Properties.CurrentTo, typeof(ISSEN.RiskCoverage).Name)));
            Join join = new Join(new ClassNameTable(typeof(ISSEN.RiskCoverDetail), typeof(ISSEN.RiskCoverDetail).Name)
              , new ClassNameTable(typeof(ISSEN.EndorsementRiskCoverage), typeof(ISSEN.EndorsementRiskCoverage).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.EndorsementRiskCoverage.Properties.RiskCoverId, typeof(ISSEN.EndorsementRiskCoverage).Name)
                .Equal().Property(ISSEN.RiskCoverDetail.Properties.RiskCoverId, typeof(ISSEN.RiskCoverDetail).Name)
                .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskCoverage), typeof(ISSEN.RiskCoverage).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.EndorsementRiskCoverage.Properties.RiskCoverId, typeof(ISSEN.EndorsementRiskCoverage).Name)
                .Equal().Property(ISSEN.RiskCoverage.Properties.RiskCoverId, typeof(ISSEN.RiskCoverage).Name)
                .GetPredicate());
            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();
            List<Accessory> accessories = new List<Accessory>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    int day = QuoteManager.CalculateEffectiveDays(Convert.ToDateTime(reader[ISSEN.RiskCoverage.Properties.CurrentFrom]), Convert.ToDateTime(reader[ISSEN.RiskCoverage.Properties.CurrentTo]));
                    decimal premiumbase = Decimal.Zero;
                    if (isCancelation && days > day)
                    {
                        premiumbase = Convert.ToDecimal(reader[ISSEN.RiskCoverDetail.Properties.PremiumAmount]);
                    }
                    else
                    {
                        premiumbase = decimal.Round(Convert.ToDecimal(reader[ISSEN.RiskCoverDetail.Properties.PremiumAmount]) / day * days, QuoteManager.DecimalRound);
                    }
                    Accessory accessory = new Accessory
                    {
                        RiskDetailId = Convert.ToInt32(reader[ISSEN.RiskCoverDetail.Properties.RiskDetailId]),
                        Premium = premiumbase
                    };
                    accessories.Add(accessory);
                }
            }
            if (accessories?.Count > 0)
            {
                var data = accessories.GroupBy(a => a.RiskDetailId).Select(u => u.First());
                List<AccessoryDTO> accessoryDTOs = new List<AccessoryDTO>();
                foreach (Accessory d in data)
                {
                    accessoryDTOs.Add(new AccessoryDTO
                    {
                        Id = d.RiskDetailId,
                        premium = decimal.Round(accessories.Where(a => a.RiskDetailId == d.RiskDetailId).Sum(m => m.Premium), QuoteManager.DecimalRound)
                    });
                }
                return accessoryDTOs;
            }
            else
            {
                return null;
            }
        }
        public List<CompanyVehicle> GetCompanyVehiclesByFilter(ObjectCriteriaBuilder filter)
        {
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();
            RiskVehicleView view = new RiskVehicleView();
            ViewBuilder builder = new ViewBuilder("RiskVehicleView")
            {
                Filter = filter.GetPredicate()
            };

            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }


            if (view.CompanyVehicles.Count == 0)
            {
                throw new ArgumentException(Errors.ErrorRiskEmpty);
            }
            List<ISSEN.Risk> risks = view.CompanyVehicles.Cast<ISSEN.Risk>().ToList();
            List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
            List<CompanyCoRisk> coRisks = view.CoRisks.Cast<CompanyCoRisk>().ToList();
            List<ISSEN.RiskVehicle> riskVehicles = view.RiskVehicles.Cast<ISSEN.RiskVehicle>().ToList();
            List<CompanyCoRiskVehicle> coRiskVehicles = view.CoRiskVehicles.Cast<CompanyCoRiskVehicle>().ToList();
            List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
            List<COMMEN.VehicleModel> vehicleModels = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();
            List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
            List<COMMEN.VehicleVersion> vehicleVersions = view.VehicleVersions.Cast<COMMEN.VehicleVersion>().ToList();
            List<ISSEN.RiskClause> riskClauses = view.RiskClause.Cast<ISSEN.RiskClause>().ToList();

            List<LimitRc> limitsRc = DelegateService.underwritingService.GetLimitsRc();

            ConcurrentBag<CompanyVehicle> vehicles = new ConcurrentBag<CompanyVehicle>();
            ParallelHelper.ForEach(risks, item =>
            {
                try
                {
                    using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.LoadDynamicProperties(item);
                    }
                    CompanyVehicle vehicle = new CompanyVehicle();

                    vehicle = ModelAssembler.CreateVehicle(item,
                        coRisks.First(x => x.RiskId == item.RiskId),
                        riskVehicles.First(x => x.RiskId == item.RiskId),
                        coRiskVehicles.First(x => x.RiskId == item.RiskId),
                        endorsementRisks.First(x => x.RiskId == item.RiskId));

                    if (vehicle.Risk.LimitRc != null)
                    {
                        vehicle.Risk.LimitRc.Description = limitsRc.First(l => l.Id == vehicle.Risk.LimitRc.Id).Description;
                    }

                    if (vehicle.Risk.Text != null)
                    {
                        vehicle.Risk.Text.TextBody = item.ConditionText;
                    }
                    else
                    {
                        vehicle.Risk.Text = new CompanyText()
                        {
                            TextBody = item.ConditionText
                        };
                    }

                    vehicle.Make.Description = vehicleMakes.FirstOrDefault(x => x.VehicleMakeCode == vehicle.Make.Id).SmallDescription;
                    vehicle.Model.Description = vehicleModels.FirstOrDefault(x => x.VehicleModelCode == vehicle.Model.Id).SmallDescription;
                    vehicle.Version.Description = vehicleVersions.FirstOrDefault(x => x.VehicleVersionCode == vehicle.Version.Id).Description;

                    CompanyIssuanceInsured companyInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(vehicle.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                    if (companyInsured == null || companyInsured.IndividualId < 1)
                    {
                        throw new Exception(Errors.ErrorInsuredMain);
                    }

                    vehicle.Risk.MainInsured = companyInsured;
                    vehicle.Risk.MainInsured.Name = vehicle.Risk.MainInsured.Name + " " + vehicle.Risk.MainInsured.Surname + " " + vehicle.Risk.MainInsured.SecondSurname;
                    ConcurrentBag<CompanyClause> clauses = new ConcurrentBag<CompanyClause>();
                    //clausulas
                    if (riskClauses != null && riskClauses.Count > 0)
                    {
                        TP.Parallel.ForEach(riskClauses.Where(x => x.RiskId == item.RiskId).ToList(), riskClause =>
                        {
                            clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
                        });
                        vehicle.Risk.Clauses = clauses.ToList();
                    }
                    CompanyName companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(vehicle.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
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
                    vehicle.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                    if (riskBeneficiaries != null && riskBeneficiaries.Count > 0)
                    {
                        vehicle.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                        Object objlock = new object();
                        IMapper mapper = ModelAssembler.CreateMapCompanyBeneficiary();
                        List<ISSEN.RiskBeneficiary> beneficiaries = riskBeneficiaries.Where(x => x.RiskId == item.RiskId).ToList();
                        ParallelHelper.ForEach(beneficiaries, riskBeneficiary =>
                        {
                            try
                            {
                                CompanyBeneficiary CiaBeneficiary = new CompanyBeneficiary();
                                Beneficiary beneficiaryRisk = DelegateService.underwritingService.GetBeneficiariesByDescription(riskBeneficiary.BeneficiaryId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                                if (beneficiaryRisk != null)
                                {
                                    CiaBeneficiary = mapper.Map<UNMOD.Beneficiary, CompanyBeneficiary>(beneficiaryRisk);
                                    CiaBeneficiary.CustomerType = CustomerType.Individual;
                                    CiaBeneficiary.BeneficiaryType = new CompanyBeneficiaryType { Id = riskBeneficiary.BeneficiaryTypeCode };
                                    CiaBeneficiary.Participation = riskBeneficiary.BenefitPercentage;
                                    beneficiaryRisk.IndividualType = beneficiaryRisk.IndividualType;
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

                                    //CiaBeneficiary.CompanyName = new IssuanceCompanyName
                                    //{
                                    //    NameNum = companyName.NameNum,
                                    //    TradeName = companyName.TradeName,
                                    //    Address = new IssuanceAddress
                                    //    {
                                    //        Id = companyName.Address.Id,
                                    //        Description = companyName.Address.Description,
                                    //        City = companyName.Address.City
                                    //    },
                                    //    Phone = new IssuancePhone
                                    //    {
                                    //        Id = companyName.Phone.Id,
                                    //        Description = companyName.Phone.Description
                                    //    },
                                    //    Email = new IssuanceEmail
                                    //    {
                                    //        Id = companyName.Email.Id,
                                    //        Description = companyName.Email.Description
                                    //    }
                                    //};

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
                                        vehicle.Risk.Beneficiaries.Add(CiaBeneficiary);
                                    }
                                }

                            }
                            catch (Exception)
                            {
                            }
                            finally
                            {
                                DataFacadeManager.Dispose();
                            }

                        });
                    }
                    CompanyVehicle vehicleFasecolda = GetVehicleByMakeIdModelIdVersionId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id);
                    vehicle.Fasecolda = vehicleFasecolda.Fasecolda;

                    //coberturas
                    List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();

                    //No se filtra por ID del endoso ya que en la tabla ENDO_RISK_COVERAGE se guardan los riesgos afectados con el último EndorsementId y no se actualizan los demás riesgos de la póliza
                    vehicle.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdByRiskId(endorsementRisks.First(x => x.RiskId == item.RiskId).PolicyId, vehicle.Risk.RiskId);

                    //accesorios
                    ObjectCriteriaBuilder filterAccessory = new ObjectCriteriaBuilder();
                    filterAccessory.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filterAccessory.Equal();
                    filterAccessory.Constant(item.RiskId);

                    AccessoryView accessoryView = new AccessoryView();
                    builder = new ViewBuilder("AccessoryView")
                    {
                        Filter = filterAccessory.GetPredicate()
                    };

                    using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.FillView(builder, accessoryView);
                    }

                    Sistran.Core.Application.CommonService.Models.Parameter parameter = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories);

                    if (accessoryView.RiskCoverDetails != null && accessoryView.RiskCoverDetails.Count > 0)
                    {
                        vehicle.Accesories = ModelAssembler.CreateAccesories(accessoryView);

                        foreach (Accessory accessory in vehicle.Accesories)
                        {
                            CompanyCoverage companyCoverage = vehicle.Risk.Coverages.FirstOrDefault(x => x.RiskCoverageId == Convert.ToInt32(accessory.AccessoryId));
                            if (companyCoverage != null && companyCoverage.Id == parameter.NumberParameter.Value)
                            {
                                accessory.IsOriginal = true;
                            }
                        }
                        vehicle.PriceAccesories = vehicle.Accesories.Where(x => !x.IsOriginal).Sum(y => y.Amount);
                    }
                    vehicles.Add(vehicle);
                }
                catch (Exception)
                {
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            });
            return vehicles.ToList();
        }

        /// <summary>
        /// Obtener Poliza de vehiculo 
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <param name="licensePlate">Placa de vehiculo</param>
        /// <returns>Vehiclepolicy</returns>
        public List<CompanyVehicle> GetVehiclesByPolicyIdEndorsementIdLicensePlate(int policyId, int endorsementId, string licensePlate, int riskId, bool riskCancelledAndExcluded)
        {
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();

            if (endorsementId == 0) //Se consulta el endoso donde la placa es is_current
            {
                endorsementId = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(policyId, licensePlate).Id;
            }

            try
            {

                //riesgo
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
                filter.Equal();
                filter.Constant(endorsementId);
                filter.And();
                if (riskId > 0)
                {
                    filter.Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name);
                    filter.Equal();
                    filter.Constant(riskId);
                }
                else
                {
                    filter.Property(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name);
                    filter.Equal();
                    filter.Constant(licensePlate);
                }

                //Endoso actual no se debe mostrar los riesgos excluidos ni cancelados
                if (riskCancelledAndExcluded)
                {
                    filter.And();
                    filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
                    filter.Distinct();
                    filter.Constant(enUnder.RiskStatusType.Cancelled);
                    filter.And();
                    filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
                    filter.Distinct();
                    filter.Constant(enUnder.RiskStatusType.Excluded);
                }

                RiskVehicleView view = new RiskVehicleView();
                ViewBuilder builder = new ViewBuilder("RiskVehicleView");
                builder.Filter = filter.GetPredicate();

                using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.FillView(builder, view);
                }

                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<CompanyCoRisk> coRisks = view.CoRisks.Cast<CompanyCoRisk>().ToList();
                List<ISSEN.RiskVehicle> riskVehicles = view.RiskVehicles.Cast<ISSEN.RiskVehicle>().ToList();
                List<CompanyCoRiskVehicle> coRiskVehicles = view.CoRiskVehicles.Cast<CompanyCoRiskVehicle>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<COMMEN.VehicleModel> vehicleModels = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();
                List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
                List<COMMEN.VehicleVersion> vehicleVersions = view.VehicleVersions.Cast<COMMEN.VehicleVersion>().ToList();
                List<ISSEN.RiskClause> riskClauses = view.RiskClause.Cast<ISSEN.RiskClause>().ToList();
                List<ISSEN.Risk> risks = view.CompanyVehicles.Cast<ISSEN.Risk>().ToList();
                List<CompanyVehicle> vehicles = new List<CompanyVehicle>();

                foreach (ISSEN.Risk item in risks)
                {
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.LoadDynamicProperties(item);
                    }
                    CompanyVehicle vehicle = new CompanyVehicle();

                    vehicle = ModelAssembler.CreateVehicle(item,
                        coRisks.First(x => x.RiskId == item.RiskId),
                        riskVehicles.First(x => x.RiskId == item.RiskId),
                        coRiskVehicles.First(x => x.RiskId == item.RiskId),
                        endorsementRisks.First(x => x.RiskId == item.RiskId));

                    vehicle.Make.Description = vehicleMakes.FirstOrDefault(x => x.VehicleMakeCode == vehicle.Make.Id).SmallDescription;
                    vehicle.Model.Description = vehicleModels.FirstOrDefault(x => x.VehicleModelCode == vehicle.Model.Id).SmallDescription;
                    vehicle.Version.Description = vehicleVersions.FirstOrDefault(x => x.VehicleVersionCode == vehicle.Version.Id).Description;

                    var companyInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(vehicle.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                    if (companyInsured == null || companyInsured.IndividualId < 1)
                    {
                        throw new Exception(Errors.ErrorInsuredMain);
                    }

                    vehicle.Risk.MainInsured = companyInsured;
                    vehicle.Risk.MainInsured.Name = vehicle.Risk.MainInsured.Name + " " + vehicle.Risk.MainInsured.Surname + " " + vehicle.Risk.MainInsured.SecondSurname;
                    ConcurrentBag<CompanyClause> clauses = new ConcurrentBag<CompanyClause>();
                    //clausulas
                    if (riskClauses != null && riskClauses.Count > 0)
                    {
                        TP.Parallel.ForEach(riskClauses.Where(x => x.RiskId == item.RiskId).ToList(), riskClause =>
                        {
                            clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
                        });
                        vehicle.Risk.Clauses = clauses.ToList();
                    }
                    var companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(vehicle.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
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

                    if (riskBeneficiaries != null && riskBeneficiaries.Count > 0)
                    {
                        vehicle.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                        Object objlock = new object();
                        var mapper = ModelAssembler.CreateMapCompanyBeneficiary();
                        TP.Parallel.ForEach(riskBeneficiaries.Where(x => x.RiskId == item.RiskId), riskBeneficiary =>
                        {
                            CompanyBeneficiary CiaBeneficiary = new CompanyBeneficiary();
                            var beneficiaryRisk = DelegateService.underwritingService.GetBeneficiariesByDescription(riskBeneficiary.BeneficiaryId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                            if (beneficiaryRisk != null)
                            {
                                CiaBeneficiary = mapper.Map<UNMOD.Beneficiary, CompanyBeneficiary>(beneficiaryRisk);
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
                                    vehicle.Risk.Beneficiaries.Add(CiaBeneficiary);
                                }
                            }
                        });
                    }
                    CompanyVehicle vehicleFasecolda = GetVehicleByMakeIdModelIdVersionId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id);
                    vehicle.Fasecolda = vehicleFasecolda.Fasecolda;

                    //coberturas
                    List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();

                    companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementRisks.First(x => x.RiskId == item.RiskId).EndorsementId, item.RiskId);
                    companyCoverages.AsParallel().ForAll(x =>
                    {
                        x.CoverageOriginalStatus = x.CoverageOriginalStatus;
                        x.OriginalRate = x.Rate;
                        x.CoverageOriginalStatus = x.CoverageOriginalStatus;
                        x.CoverStatus = CoverageStatusType.NotModified;
                    }
                    );
                    vehicle.Risk.Coverages = companyCoverages;

                    //accesorios
                    ObjectCriteriaBuilder filter_ = new ObjectCriteriaBuilder();
                    filter_.Property(ISSEN.EndorsementRiskCoverage.Properties.PolicyId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filter_.Equal();
                    filter_.Constant(policyId);
                    filter_.And();
                    filter_.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filter_.Equal();
                    filter_.Constant(endorsementRisks.First(x => x.RiskId == item.RiskId).EndorsementId);
                    filter_.And();
                    filter_.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filter_.Equal();
                    filter_.Constant(item.RiskId);

                    AccessoryView accessoryView = new AccessoryView();
                    ViewBuilder builder_ = new ViewBuilder("AccessoryView");
                    builder_.Filter = filter_.GetPredicate();
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.FillView(builder_, accessoryView);
                    }

                    Sistran.Core.Application.CommonService.Models.Parameter parameter = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories);

                    if (accessoryView.RiskCoverDetails != null && accessoryView.RiskCoverDetails.Count > 0)
                    {
                        vehicle.Accesories = ModelAssembler.CreateAccesories(accessoryView);

                        foreach (Accessory accessory in vehicle.Accesories)
                        {
                            CompanyCoverage companyCoverage = vehicle.Risk.Coverages.FirstOrDefault(x => x.RiskCoverageId == Convert.ToInt32(accessory.AccessoryId));
                            if (companyCoverage != null && companyCoverage.Id == parameter.NumberParameter.Value)
                            {
                                accessory.IsOriginal = true;
                            }
                        }

                        vehicle.PriceAccesories = vehicle.Accesories.Where(x => !x.IsOriginal).Sum(y => y.Amount);
                    }
                    //AddCoveragesToRisk(vehicle, policyId, endorsementId, item.RiskId);
                    vehicles.Add(vehicle);
                }

                return vehicles;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Obtener Poliza de vehiculo 
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <param name="licensePlateList">Lista de placas de vehiculos</param>
        /// <returns>Vehiclepolicy</returns>
        public List<CompanyVehicle> GetVehiclesByPolicyIdEndorsementIdLicensePlateList(int policyId, int endorsementId, List<string> licensePlateList)
        {
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();
            //riesgo
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
            filter.In();
            filter.ListValue();
            foreach (string s in licensePlateList)
            {
                filter.Constant(s);
            }
            filter.EndList();

            //Endoso actual no se debe mostrar los riesgos excluidos ni cancelados
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.Distinct();
            filter.Constant(enUnder.RiskStatusType.Cancelled);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.Distinct();
            filter.Constant(enUnder.RiskStatusType.Excluded);

            RiskVehicleView view = new RiskVehicleView();
            ViewBuilder builder = new ViewBuilder("RiskVehicleView");
            builder.Filter = filter.GetPredicate();

            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            if (view.CompanyVehicles.Count > 0)
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Risk> risks = view.CompanyVehicles.Cast<ISSEN.Risk>().ToList();
                List<CompanyCoRisk> coRisks = view.CoRisks.Cast<CompanyCoRisk>().ToList();
                List<ISSEN.RiskVehicle> riskVehicles = view.RiskVehicles.Cast<ISSEN.RiskVehicle>().ToList();
                List<CompanyCoRiskVehicle> coRiskVehicles = view.CoRiskVehicles.Cast<CompanyCoRiskVehicle>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<COMMEN.VehicleModel> vehicleModels = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();
                List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
                List<COMMEN.VehicleVersion> vehicleVersions = view.VehicleVersions.Cast<COMMEN.VehicleVersion>().ToList();

                foreach (ISSEN.Risk item in risks)
                {
                    using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.LoadDynamicProperties(item);
                    }
                    CompanyVehicle vehicle = new CompanyVehicle();

                    vehicle = ModelAssembler.CreateVehicle(item,
                        coRisks.Where(x => x.RiskId == item.RiskId).First(),
                        riskVehicles.Where(x => x.RiskId == item.RiskId).First(),
                        coRiskVehicles.Where(x => x.RiskId == item.RiskId).First(),
                        endorsementRisks.Where(x => x.RiskId == item.RiskId).First());

                    vehicle.Make.Description = vehicleMakes.Where(x => x.VehicleMakeCode == vehicle.Make.Id).FirstOrDefault().SmallDescription;
                    vehicle.Model.Description = vehicleModels.Where(x => x.VehicleModelCode == vehicle.Model.Id).FirstOrDefault().SmallDescription;
                    vehicle.Version.Description = vehicleVersions.Where(x => x.VehicleVersionCode == vehicle.Version.Id).FirstOrDefault().Description;

                    vehicle.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                    foreach (ISSEN.RiskBeneficiary riskBeneficiary in riskBeneficiaries.Where(x => x.RiskId == item.RiskId))
                    {
                        vehicle.Risk.Beneficiaries.Add(ModelAssembler.CreateBeneficiary(riskBeneficiary));
                    }

                    CompanyVehicle vehicleFasecolda = GetVehicleByMakeIdModelIdVersionId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id);
                    vehicle.Fasecolda = vehicleFasecolda.Fasecolda;

                    //coberturas
                    List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();

                    companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, item.RiskId);
                    vehicle.Risk.Coverages = companyCoverages;

                    //accesorios
                    filter = new ObjectCriteriaBuilder();
                    filter.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filter.Equal();
                    filter.Constant(item.RiskId);
                    AccessoryView accessoryView = new AccessoryView();
                    builder = new ViewBuilder("AccessoryView");
                    builder.Filter = filter.GetPredicate();
                    using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.FillView(builder, accessoryView);
                    }
                    CSMO.Parameter parameter = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories);

                    if (accessoryView.RiskCoverDetails != null && accessoryView.RiskCoverDetails.Count > 0)
                    {
                        vehicle.Accesories = ModelAssembler.CreateAccesories(accessoryView);

                        foreach (Accessory accessory in vehicle.Accesories)
                        {
                            if (vehicle.Risk.Coverages.First(x => x.RiskCoverageId == Convert.ToInt32(accessory.AccessoryId)).Id == parameter.NumberParameter.Value)
                            {
                                accessory.IsOriginal = true;
                            }
                        }

                        vehicle.PriceAccesories = vehicle.Accesories.Where(x => !x.IsOriginal).Sum(y => y.Amount);
                    }
                    IMapper mapper = ModelAssembler.CreateMapPersonInsured();
                    vehicle.Risk.MainInsured = mapper.Map<CiaPersonModel.CompanyInsured, CiUndrwritingModel.CompanyIssuanceInsured>(DelegateService.uniquePersonService.GetCompanyInsuredByIndividualId(vehicle.Risk.MainInsured.IndividualId));
                    AddCoveragesToRisk(vehicle, policyId, endorsementId, item.RiskId);
                    companyVehicles.Add(vehicle);
                }
            }
            return companyVehicles;
        }

        /// <summary>
        /// Obtener Vehículo por Placa
        /// </summary>
        /// <param name="licencePlate">Placa</param>
        /// <returns>Vehículo</returns>
        public CompanyVehicle GetVehicleByLicensePlate(string licencePlate)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name, licencePlate);

            string[] sort = new[] { "-" + ISSEN.RiskVehicle.Properties.RiskId };
            BusinessCollection businessCollection = null;
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                IDataReader result = daf.SelectObjects(typeof(ISSEN.RiskVehicle), filter.GetPredicate(), sort);
                businessCollection = new BusinessCollection(result);
            }

            if (businessCollection != null && businessCollection.Count > 0)
            {
                CompanyVehicle vehicle = ModelAssembler.CreateVehicle((ISSEN.RiskVehicle)businessCollection[0]);
                CompanyVehicle vehicleFasecolda = GetVehicleByMakeIdModelIdVersionId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id);
                vehicle.Fasecolda = vehicleFasecolda.Fasecolda;
                return vehicle;
            }
            else
            {
                return null;
            }
        }

        public List<CompanyVehicle> GetCompanyVehiclesByEndorsementId(int endorsementId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementOperation.Properties.EndorsementId, typeof(ISSEN.EndorsementOperation).Name).Equal().Constant(endorsementId);
            filter.And();
            filter.Property(ISSEN.EndorsementOperation.Properties.RiskNumber, typeof(ISSEN.EndorsementOperation).Name).IsNotNull();
            BusinessCollection businessCollection = null;
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                IDataReader result = daf.SelectObjects(typeof(ISSEN.EndorsementOperation), filter.GetPredicate());
                businessCollection = new BusinessCollection(result);
            }
            if (businessCollection != null && businessCollection.Any())
            {
                List<CompanyVehicle> companyVehicles = ModelAssembler.CreateVehicles(businessCollection);

                if (companyVehicles.Count > 0)
                {
                    if (companyVehicles[0].Risk.Coverages != null)
                    {
                        return companyVehicles;
                    }
                    else
                    {
                        return GetCompanyVehiclesFromTables(endorsementId);
                    }
                }
                else
                {
                    return GetCompanyVehiclesFromTables(endorsementId);
                }
            }
            else
            {
                return null;
            }
        }

        public List<CompanyVehicle> GetCompanyVehiclesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            switch (moduleType)
            {
                case ModuleType.Emission:
                    return GetCompanyVehiclesByEndorsementId(endorsementId);
                case ModuleType.Claim:
                    List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
                    filter.Equal();
                    filter.Constant(endorsementId);
                    filter.And();
                    filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
                    filter.Distinct();
                    filter.Constant(RiskStatusType.Excluded);

                    ClaimRiskVehicleView view = new ClaimRiskVehicleView();
                    ViewBuilder builder = new ViewBuilder("ClaimRiskVehicleView");
                    builder.Filter = filter.GetPredicate();

                    DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                    if (view.Risks.Count > 0)
                    {
                        companyVehicles = ModelAssembler.CreateCompanyClaimVehiclesByRiskVehicle(view.RiskVehicles);

                        List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                        List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                        List<COMMEN.VehicleColor> vehicleColors = view.VehicleColors.Cast<COMMEN.VehicleColor>().ToList();
                        List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
                        List<COMMEN.VehicleModel> vehicleModel = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();

                        foreach (CompanyVehicle companyVehicle in companyVehicles)
                        {
                            ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                            filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                            filterSumAssured.Equal();
                            filterSumAssured.Constant(endorsementId);
                            CompanyVehiclesSumAssuredView assuredView = new CompanyVehiclesSumAssuredView();
                            ViewBuilder builderAssured = new ViewBuilder("SumAssuredView");
                            builderAssured.Filter = filterSumAssured.GetPredicate();
                            DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                            decimal insuredAmount = 0;

                            foreach (ISSEN.RiskCoverage entityRiskCoverage in assuredView.RiskCoverages)
                            {
                                insuredAmount += entityRiskCoverage.LimitAmount;
                            }

                            companyVehicle.Risk = ModelAssembler.CreateRisk(risks.First(x => x.RiskId == companyVehicle.Risk.RiskId));
                            companyVehicle.Risk.Number = entityEndorsementRisks.First(x => x.RiskId == companyVehicle.Risk.RiskId).RiskNum;
                            companyVehicle.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(companyVehicle.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                            companyVehicle.Make.Description = vehicleMakes.FirstOrDefault(x => x.VehicleMakeCode == companyVehicle.Make.Id)?.SmallDescription;
                            companyVehicle.Model.Description = vehicleModel.FirstOrDefault(x => x.VehicleModelCode == companyVehicle.Model.Id)?.SmallDescription;
                            companyVehicle.Color.Description = vehicleColors.FirstOrDefault(x => x.VehicleColorCode == companyVehicle.Color.Id)?.SmallDescription;
                            companyVehicle.Risk.AmountInsured = insuredAmount;
                        }
                    }
                    
                    return companyVehicles;

                default:
                    return new List<CompanyVehicle>();
            }
        }

        /// <summary>
        /// Obtener Vehículo por Marca, Modelo y Versión
        /// </summary>
        /// <param name="makeId">Id Marca</param>
        /// <param name="modelId">Id Modelo</param>
        /// <param name="versionId">Id Versión</param>
        /// <returns>Vehículo</returns>
        public CompanyVehicle GetVehicleByMakeIdModelIdVersionId(int makeId, int modelId, int versionId)
        {
            PrimaryKey key = COCOMMEN.CoVehicleVersionFasecolda.CreatePrimaryKey(versionId, modelId, makeId);
            COCOMMEN.CoVehicleVersionFasecolda fasecoldaEntity = null;
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                BusinessObject result = daf.GetObjectByPrimaryKey(key);
                fasecoldaEntity = (COCOMMEN.CoVehicleVersionFasecolda)result;
            }

            if (fasecoldaEntity != null)
            {
                CompanyVehicle vehicle = new CompanyVehicle
                {
                    Fasecolda = new Fasecolda
                    {
                        Make = fasecoldaEntity.FasecoldaMakeId.Trim(),
                        Model = fasecoldaEntity.FasecoldaModelId.Trim(),
                        Description = fasecoldaEntity.FasecoldaMakeId.Trim() + fasecoldaEntity.FasecoldaModelId.Trim()
                    },
                    Make = new CompanyMake
                    {
                        Id = makeId
                    },
                    Model = new CompanyModel
                    {
                        Id = modelId
                    },
                    Version = new CVEMOD.CompanyVersion
                    {
                        Id = versionId
                    }
                };

                VEDAO.VehicleDAO vehicleDAO = new VEDAO.VehicleDAO();
                IMapper imapper = ModelAssembler.CreateMapCompanyVersion();
                vehicle.Version = imapper.Map<VEMOD.Version, CVEMOD.CompanyVersion>(vehicleDAO.GetVersionByVersionIdModelIdMakeId(vehicle.Version.Id, vehicle.Model.Id, vehicle.Make.Id));
                return vehicle;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Vehículo por Código Fasecolda
        /// </summary>
        /// <param name="fasecoldaCode">Código Fasecolda</param>
        /// <returns>Vehículo</returns>
        public CompanyVehicle GetVehicleByFasecoldaCode(string fasecoldaCode, int year)
        {
            if (fasecoldaCode.Length > 3)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COCOMMEN.CoVehicleVersionFasecolda.Properties.FasecoldaMakeId, typeof(COCOMMEN.CoVehicleVersionFasecolda).Name);
                filter.Equal();
                filter.Constant(fasecoldaCode.Substring(0, 3));
                filter.And();
                filter.Property(COCOMMEN.CoVehicleVersionFasecolda.Properties.FasecoldaModelId, typeof(COCOMMEN.CoVehicleVersionFasecolda).Name);
                filter.Equal();
                filter.Constant(fasecoldaCode.Substring(3));

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COCOMMEN.CoVehicleVersionFasecolda), filter.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    COCOMMEN.CoVehicleVersionFasecolda fasecoldaEntity = (COCOMMEN.CoVehicleVersionFasecolda)businessCollection[0];

                    CompanyVehicle vehicle = new CompanyVehicle
                    {
                        Fasecolda = new Fasecolda
                        {
                            Make = fasecoldaEntity.FasecoldaMakeId.Trim(),
                            Model = fasecoldaEntity.FasecoldaModelId.Trim(),
                            Description = fasecoldaEntity.FasecoldaMakeId.Trim() + fasecoldaEntity.FasecoldaModelId.Trim()
                        },
                        Make = new CompanyMake
                        {
                            Id = fasecoldaEntity.VehicleMakeCode
                        },
                        Model = new CompanyModel
                        {
                            Id = fasecoldaEntity.VehicleModelCode
                        },
                        Version = new CVEMOD.CompanyVersion
                        {
                            Id = fasecoldaEntity.VehicleVersionCode
                        }
                    };
                    IMapper imappers = ModelAssembler.CreateMapCompanyFasecolda();

                    VEDAO.VehicleDAO vehicleDAO = new VEDAO.VehicleDAO();
                    Make getMakeByVehicleMakeCd = vehicleDAO.GetMakeByVehicleMakeCd(vehicle.Make.Id);
                    vehicle.Make = imappers.Map<Make, CompanyMake>(getMakeByVehicleMakeCd);
                    vehicle.Model = imappers.Map<Model, CompanyModel>(vehicleDAO.GetModelByMakeIdModelId(vehicle.Make.Id, vehicle.Model.Id));
                    IMapper imapper = ModelAssembler.CreateMapCompanyVersion();

                    vehicle.Version = imapper.Map<VEMOD.Version, CVEMOD.CompanyVersion>(vehicleDAO.GetVersionByVersionIdModelIdMakeId(vehicle.Version.Id, vehicle.Model.Id, vehicle.Make.Id));
                    if (year > 0)
                    {
                        vehicle.Price = vehicleDAO.GetPriceByMakeIdModelIdVersionId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id, year);
                    }
                    return vehicle;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public int GetCountVehiclePolicyByPolicyId(int policyId)
        {
            //riesgo
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
            RiskVehicleView view = new RiskVehicleView();
            ViewBuilder builder = new ViewBuilder("RiskVehicleView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();

            return view.CompanyVehicles.Count;
        }

        /// <summary>
        /// Obtener Poliza de vehiculos(sin riesgos)
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="endorsementId">Id endoso</param>
        /// <returns>Vehiclepolicy</returns>
        public CompanyPolicy GetVehiclePolicyWithOutRiskByPolicyId(int policyId, int endorsementId)
        {
            return DelegateService.underwritingService.GetTemporalPolicyByPolicyIdEndorsementId(policyId, endorsementId);
        }

        /// <summary>
        /// Obtener Vehículo 2G por Placa, No. Motor y No. Chasis 
        /// </summary>
        /// <param name="licensePlate">Propiedad clave LicensePlate.</param>
        /// <param name="engineSerNo">Propiedad clave EngineSerNo.</param>
        /// <param name="chassisSerNo">Propiedad clave ChassisSerNo.</param>
        /// <returns>Vehículo</returns>
        public bool IsVehicleByLicensePlateEngineChassis(string licensePlate, string engineSerNo, string chassisSerNo)
        {
            bool exist = false;
            PrimaryKey key = CoVehicle2G.CreatePrimaryKey(licensePlate, engineSerNo, chassisSerNo);

            CoVehicle2G vehicle2GEntity = (CoVehicle2G)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (vehicle2GEntity != null)
            {
                exist = true;
            }
            else
            {
                exist = false;
            }
            return exist;
        }

        /// <summary>
        /// Obtener Vehículo por Marca, Modelo, Versión y anio
        /// </summary>
        /// <param name="makeId">Id Marca</param>
        /// <param name="modelId">Id Modelo</param>
        /// <param name="versionId">Id Versión</param>
        /// /// <param name="year">anio a buscar</param>
        /// <returns>Vehículo</returns>
        public CompanyVehicle GetVehicleByMakeIdModelIdVersionIdAndYear(int makeId, int modelId, int versionId, int year)
        {
            CompanyVehicle vehicle = null;
            PrimaryKey key = VehicleVersionYear.CreatePrimaryKey(versionId, modelId, makeId, year);
            VehicleVersionYear versionYear = (VehicleVersionYear)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (versionYear != null)
            {
                vehicle = new CompanyVehicle
                {
                    Make = new CompanyMake
                    {
                        Id = makeId
                    },
                    Model = new CompanyModel
                    {
                        Id = modelId
                    },
                    Version = new CVEMOD.CompanyVersion
                    {
                        Id = versionId
                    },
                    Year = year
                };
            }
            return vehicle;
        }

        public List<PoliciesAut> ValidateAuthorizationPolicies(CompanyVehicle companyVehicle)
        {
            if (companyVehicle.Risk != null && companyVehicle.Risk.Policy != null)
            {
                companyVehicle.Risk.Policy.SinisterQuantity = GetCountSinister(companyVehicle);
                companyVehicle.Risk.Policy.HasTotalLoss = GetHasTotalLoss(companyVehicle);
                companyVehicle.Risk.Policy.PortfolioBalance = GetHasPortfolioBalance(companyVehicle);
            }

            Rules.Facade facade = new Rules.Facade();
            facade.SetConcept(RuleConceptPolicies.UserId, companyVehicle.Risk.Policy.UserId);

            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();
            if (companyVehicle != null && companyVehicle.Risk.Policy != null)
            {
                string key = companyVehicle.Risk.Policy.Prefix.Id + "," + (int)companyVehicle.Risk.Policy.Product.CoveredRisk.CoveredRiskType;
                EntityAssembler.CreateFacadeGeneral(facade, companyVehicle.Risk.Policy);
                EntityAssembler.CreateFacadeRiskVehicle(facade, companyVehicle);
                /*Politica del riesgo*/
                policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_RISK));
                /*Politicas de cobertura*/
                if (companyVehicle.Risk.Coverages != null)
                {
                    foreach (CompanyCoverage coverage in companyVehicle.Risk.Coverages)
                    {
                        EntityAssembler.CreateFacadeCoverage(facade, coverage);
                        policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_COVERAGE));
                    }
                }
            }
            return policiesAuts;
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesMassive(CompanyVehicle companyVehicle, int hierarchy, List<int> ruleToValidateRisk, List<int> ruleToValidateCoverage)
        {
            if (companyVehicle.Risk != null && companyVehicle.Risk.Policy != null)
            {
                companyVehicle.Risk.Policy.SinisterQuantity = GetCountSinister(companyVehicle);
                companyVehicle.Risk.Policy.HasTotalLoss = GetHasTotalLoss(companyVehicle);
                companyVehicle.Risk.Policy.PortfolioBalance = GetHasPortfolioBalance(companyVehicle);
            }

            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();
            if (companyVehicle != null && companyVehicle.Risk.Policy != null)
            {
                string key = companyVehicle.Risk.Policy.Prefix.Id + "," + (int)companyVehicle.Risk.Policy.Product.CoveredRisk.CoveredRiskType;
                EntityAssembler.CreateFacadeGeneral(facade, companyVehicle.Risk.Policy);
                facade.SetConcept(CompanyRuleConceptPolicies.UserId, companyVehicle.Risk.Policy.UserId);
                EntityAssembler.CreateFacadeRiskVehicle(facade, companyVehicle);
                /*Politica del riesgo*/
                if (ruleToValidateRisk.Any())
                {
                    policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPoliciesMassive(10, key, facade, FacadeType.RULE_FACADE_RISK, hierarchy, ruleToValidateRisk));
                }

                /*Politicas de cobertura*/
                if (companyVehicle.Risk.Coverages != null && ruleToValidateCoverage.Any())
                {
                    foreach (CompanyCoverage coverage in companyVehicle.Risk.Coverages)
                    {
                        EntityAssembler.CreateFacadeCoverage(facade, coverage);
                        policiesAuts.AddRange(DelegateService.authorizationPoliciesService.ValidateAuthorizationPoliciesMassive(10, key, facade, FacadeType.RULE_FACADE_COVERAGE, hierarchy, ruleToValidateCoverage));
                    }
                }
            }
            return policiesAuts;
        }

        /// <summary>
        /// Metodo para devolver riesgos desde el esquema report
        /// </summary>
        /// <param name="prefixId">ramo</param>
        /// <param name="branchId"> sucursal</param>
        /// <param name="documentNumber"> numero de poliza</param>
        /// <param name="endorsementType"> endorsement</param>
        /// <returns>modelo de vehicles</returns>
        public List<CompanyVehicle> GetVehiclesByPrefixBranchDocumentNumberEndorsementType(int prefixId, int branchId, decimal documentNumber, EndorsementType endorsementType)
        {
            List<CompanyVehicle> vehicles = new List<CompanyVehicle>();
            NameValue[] parameters = new NameValue[5];
            parameters[0] = new NameValue("@PREFIX_ID", prefixId);
            parameters[1] = new NameValue("@BRANCH_ID", branchId);
            parameters[2] = new NameValue("@DOCUMENT_NUM", documentNumber);
            parameters[3] = new NameValue("@ENDORSEMENT_TYPE_ID", endorsementType);
            parameters[4] = new NameValue("@ONLY_POLICY", 0);


            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("REPORT.REPORT_GET_OPERATION", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {

                foreach (DataRow arrayItem in result.Rows)
                {
                    vehicles.Add(JsonConvert.DeserializeObject<CompanyVehicle>(arrayItem[0].ToString()));

                }
            }
            return vehicles;
        }

        public bool GetCityExempt(int Branch)
        {
            List<CompanyVehicle> vehicles = new List<CompanyVehicle>();
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@BRANCH_CD", Branch);

            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("COMM.GET_CITY_EXEMPT", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        public CompanyVehicle GetCompanyVehicleByRiskId(int riskId)
        {
            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(riskId);
            if (pendingOperation != null)
            {
                CompanyVehicle companyVehicle = COMUT.JsonHelper.DeserializeJson<CompanyVehicle>(pendingOperation.Operation);
                companyVehicle.Risk.Id = pendingOperation.Id;
                companyVehicle.Risk.IsPersisted = true;

                return companyVehicle;
            }
            else
            {
                return null;
            }
        }

        public List<CompanyVehicle> GetCompanyRisksVehicleByInsuredId(int insuredId)
        {
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(insuredId);

            ClaimRiskVehicleView view = new ClaimRiskVehicleView();
            ViewBuilder builder = new ViewBuilder("ClaimRiskVehicleView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                companyVehicles = ModelAssembler.CreateCompanyClaimVehiclesByRiskVehicle(view.RiskVehicles);
                
                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.Policy> policies = view.Policies.Cast<ISSEN.Policy>().ToList();
                List<COMMEN.VehicleColor> vehicleColors = view.VehicleColors.Cast<COMMEN.VehicleColor>().ToList();
                List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
                List<COMMEN.VehicleModel> vehicleModel = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                foreach (CompanyVehicle companyVehicle in companyVehicles)
                {
                    companyVehicle.Risk = ModelAssembler.CreateRisk(risks.First(x => x.RiskId == companyVehicle.Risk.RiskId));
                    companyVehicle.Risk.Number = entityEndorsementRisks.First(x => x.RiskId == companyVehicle.Risk.RiskId).RiskNum;
                    companyVehicle.Make.Description = vehicleMakes.FirstOrDefault(x => x.VehicleMakeCode == companyVehicle.Make.Id)?.SmallDescription;
                    companyVehicle.Model.Description = vehicleModel.FirstOrDefault(x => x.VehicleModelCode == companyVehicle.Model.Id)?.SmallDescription;
                    companyVehicle.Color.Description = vehicleColors.FirstOrDefault(x => x.VehicleColorCode == companyVehicle.Color.Id)?.SmallDescription;
                    companyVehicle.Risk.Policy.Endorsement.Id = endorsementRisks.FirstOrDefault(X => X.RiskId == companyVehicle.Risk.Id).EndorsementId;
                    companyVehicle.Risk.Policy.Id = endorsementRisks.FirstOrDefault(X => X.RiskId == companyVehicle.Risk.Id).PolicyId;
                    companyVehicle.Risk.Policy.DocumentNumber = policies.FirstOrDefault(X => X.PolicyId == companyVehicle.Risk.Policy.Id).DocumentNumber;
                }
            }

            return companyVehicles;
        }

        public List<CompanyVehicle> GetCompanyRisksVehicleByLicensePlate(string licensePlate)
        {
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name);
            filter.Like();
            filter.Constant(licensePlate + "%");

            ClaimRiskVehicleView view = new ClaimRiskVehicleView();
            ViewBuilder builder = new ViewBuilder("ClaimRiskVehicleView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                companyVehicles = ModelAssembler.CreateCompanyClaimVehiclesByRiskVehicle(view.RiskVehicles);

                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.Policy> policies = view.Policies.Cast<ISSEN.Policy>().ToList();
                List<COMMEN.VehicleColor> vehicleColors = view.VehicleColors.Cast<COMMEN.VehicleColor>().ToList();
                List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
                List<COMMEN.VehicleModel> vehicleModel = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                foreach (CompanyVehicle companyVehicle in companyVehicles)
                {
                    companyVehicle.Risk = ModelAssembler.CreateRisk(risks.First(x => x.RiskId == companyVehicle.Risk.RiskId));
                    companyVehicle.Risk.Number = entityEndorsementRisks.First(x => x.RiskId == companyVehicle.Risk.RiskId).RiskNum;
                    companyVehicle.Make.Description = vehicleMakes.FirstOrDefault(x => x.VehicleMakeCode == companyVehicle.Make.Id)?.SmallDescription;
                    companyVehicle.Model.Description = vehicleModel.FirstOrDefault(x => x.VehicleModelCode == companyVehicle.Model.Id)?.SmallDescription;
                    companyVehicle.Color.Description = vehicleColors.FirstOrDefault(x => x.VehicleColorCode == companyVehicle.Color.Id)?.SmallDescription;
                    companyVehicle.Risk.Policy.Endorsement.Id = endorsementRisks.FirstOrDefault(X => X.RiskId == companyVehicle.Risk.Id).EndorsementId;
                    companyVehicle.Risk.Policy.Id = endorsementRisks.FirstOrDefault(X => X.RiskId == companyVehicle.Risk.Id).PolicyId;
                    companyVehicle.Risk.Policy.DocumentNumber = policies.FirstOrDefault(X => X.PolicyId == companyVehicle.Risk.Policy.Id).DocumentNumber;
                }
            }

            return companyVehicles;
        }

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Riesgos</returns>
        public List<CompanyVehicle> GetCompanyVehiclesByTemporalId(int temporalId)
        {
            ConcurrentBag<CompanyVehicle> companyVehicles = new ConcurrentBag<CompanyVehicle>();
            List<PendingOperation> pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(temporalId);
            TP.Parallel.ForEach(pendingOperations, pendingOperation =>
            {
                var companyVehicle = JsonConvert.DeserializeObject<CompanyVehicle>(pendingOperation.Operation);
                companyVehicle.Risk.Id = pendingOperation.Id;
                companyVehicles.Add(companyVehicle);
            });

            return companyVehicles.ToList();
        }

        /// <summary>
        /// Obtener Poliza de vehiculos
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>Vehiclepolicy</returns>
        private List<CompanyVehicle> GetCompanyVehiclesFromTables(int endorsementId)
        {
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            RiskVehicleView view = new RiskVehicleView();
            ViewBuilder builder = new ViewBuilder("RiskVehicleView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            if (view.CompanyVehicles.Count > 0)
            {
                List<ISSEN.Risk> risks = view.CompanyVehicles.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<CompanyCoRisk> coRisks = view.CoRisks.Cast<CompanyCoRisk>().ToList();
                List<ISSEN.RiskVehicle> riskVehicles = view.RiskVehicles.Cast<ISSEN.RiskVehicle>().ToList();
                List<CompanyCoRiskVehicle> coRiskVehicles = view.CoRiskVehicles.Cast<CompanyCoRiskVehicle>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<COMMEN.VehicleModel> vehicleModels = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();
                List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
                List<COMMEN.VehicleVersion> vehicleVersions = view.VehicleVersions.Cast<COMMEN.VehicleVersion>().ToList();
                Parallel.ForEach(risks, ParallelHelper.DebugParallelFor(), item =>
                 {
                     DataFacadeManager.Instance.GetDataFacade().LoadDynamicProperties(item);
                     CompanyVehicle vehicle = new CompanyVehicle();

                     vehicle = ModelAssembler.CreateVehicle(item,
                         coRisks.Where(x => x.RiskId == item.RiskId).First(),
                         riskVehicles.Where(x => x.RiskId == item.RiskId).First(),
                         coRiskVehicles.Where(x => x.RiskId == item.RiskId).First(),
                         endorsementRisks.Where(x => x.RiskId == item.RiskId).First()
                         );

                     vehicle.Make.Description = vehicleMakes.Where(x => x.VehicleMakeCode == vehicle.Make.Id).FirstOrDefault().SmallDescription;
                     vehicle.Model.Description = vehicleModels.Where(x => x.VehicleModelCode == vehicle.Model.Id).FirstOrDefault().SmallDescription;
                     vehicle.Version.Description = vehicleVersions.Where(x => x.VehicleVersionCode == vehicle.Version.Id).FirstOrDefault().Description;

                     int insuredNameNum = vehicle.Risk.MainInsured.CompanyName.NameNum;

                     vehicle.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(vehicle.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);

                    CompanyName companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(vehicle.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                    vehicle.Risk.MainInsured.CompanyName = new IssuanceCompanyName
                    {
                        NameNum = companyName.NameNum,
                        TradeName = companyName.TradeName,
                        Address = new IssuanceAddress
                        {
                            Id = companyName.Address.Id,
                            Description = companyName.Address.Description,
                            City = companyName.Address.City
                        }
                    };
                    if (companyName.Phone != null)
                    {
                        vehicle.Risk.MainInsured.CompanyName.Phone = new IssuancePhone
                        {
                            Id = companyName.Phone.Id,
                            Description = companyName.Phone.Description
                        };
                    }
                    if (companyName.Email != null)
                    {
                        vehicle.Risk.MainInsured.CompanyName.Email = new IssuanceEmail
                        {
                            Id = companyName.Email.Id,
                            Description = companyName.Email.Description
                        };
                    }

                    vehicle.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                    foreach (ISSEN.RiskBeneficiary riskBeneficiary in riskBeneficiaries.Where(x => x.RiskId == item.RiskId))
                    {
                        CompanyBeneficiary beneficiary = new CompanyBeneficiary();
                        beneficiary = ModelAssembler.CreateBeneficiary(riskBeneficiary);

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
                            }
                        };
                        if (companyName.Phone != null)
                        {
                            beneficiary.CompanyName.Phone = new IssuancePhone
                            {
                                Id = companyName.Phone.Id,
                                Description = companyName.Phone.Description
                            };
                        }
                        if (companyName.Email != null)
                        {
                            beneficiary.CompanyName.Email = new IssuanceEmail
                            {
                                Id = companyName.Email.Id,
                                Description = companyName.Email.Description
                            };
                        }
                        vehicle.Risk.Beneficiaries.Add(beneficiary);
                    }

                     CompanyVehicle vehicleFasecolda = GetVehicleByMakeIdModelIdVersionId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id);
                     vehicle.Fasecolda = vehicleFasecolda.Fasecolda;

                     vehicle.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(endorsementRisks[0].PolicyId, endorsementId, item.RiskId);

                     //accesorios
                     filter = new ObjectCriteriaBuilder();
                     filter.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                     filter.Equal();
                     filter.Constant(item.RiskId);

                     AccessoryView accessoryView = new AccessoryView();
                     builder = new ViewBuilder("AccessoryView");
                     builder.Filter = filter.GetPredicate();

                     DataFacadeManager.Instance.GetDataFacade().FillView(builder, accessoryView);

                     Sistran.Core.Application.CommonService.Models.Parameter parameter = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories);

                     if (accessoryView.RiskCoverDetails != null && accessoryView.RiskCoverDetails.Count > 0)
                     {
                         vehicle.Accesories = ModelAssembler.CreateAccesories(accessoryView);
                         TP.Parallel.ForEach(vehicle.Accesories, accessory =>
                         {
                             if (vehicle.Risk.Coverages.First(x => x.RiskCoverageId == Convert.ToInt32(accessory.AccessoryId)).Id == parameter.NumberParameter.Value)
                             {
                                 accessory.IsOriginal = true;
                             }
                         });

                         vehicle.PriceAccesories = vehicle.Accesories.Where(x => !x.IsOriginal).Sum(y => y.Amount);
                     }
                     DataFacadeManager.Dispose();
                     lock (companyVehicles)
                     {
                         companyVehicles.Add(vehicle);
                     }
                 });
            }

            return companyVehicles;
        }

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyVehicle> companyVehicles)
        {
            if (companyPolicy == null || companyVehicles == null || companyVehicles.Count < 1)
            {
                throw new ArgumentException("Poliza y Vehiculo Vacios");
            }

            if (companyVehicles.All(x => x.Risk.Status == RiskStatusType.NotModified))
            {
                throw new ArgumentException(Errors.RiskNotModified);
            }
            ValidateInfringementPolicies(companyPolicy, companyVehicles);
            if (companyPolicy?.InfringementPolicies?.Count == 0)
            {
                VehicleModelsDTO.CompanyPolicyDTO companyPolicyDto = new VehicleModelsDTO.CompanyPolicyDTO { id = companyPolicy.Endorsement.PolicyId, EndorsmentId = companyPolicy.Endorsement.Id, DocumentNumber = companyPolicy.DocumentNumber };
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
                        TP.Parallel.ForEach(companyVehicles, companyVehicle =>
                        {
                            companyVehicle.Risk.Policy = companyPolicy;

                            if (companyVehicle.Risk.Number == 0 && (companyVehicle.Risk.Status == RiskStatusType.Original || companyVehicle.Risk.Status == RiskStatusType.Included))
                            {
                                Interlocked.Increment(ref maxRiskCount);
                                companyVehicle.Risk.Number = maxRiskCount;
                            }
                        });

                        if (companyPolicy.PolicyOrigin == PolicyOrigin.Collective)
                        {
                            ConcurrentBag<string> errors = new ConcurrentBag<string>();
                            Parallel.ForEach(companyVehicles, ParallelHelper.DebugParallelFor(), companyVehicle =>
                            {
                                try
                                {
                                    CreateRisk(companyVehicle);
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
                        else
                        {
                            ConcurrentBag<string> errors = new ConcurrentBag<string>();
                            Parallel.ForEach(companyVehicles, ParallelHelper.DebugParallelFor(), companyVehicle =>
                            {
                                try
                                {
                                    if (companyVehicle.Risk.Status == RiskStatusType.NotModified)
                                    {
                                        throw new ArgumentException(Errors.RiskNotModified);
                                    }
                                    CreateRisk(companyVehicle);
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
                        try
                        {
                            DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(policyId, endorsementId, endorsementType);
                            companyPolicy.Endorsement.PolicyId = companyPolicyDto.id;
                            companyPolicy.Endorsement.Id = companyPolicyDto.EndorsmentId;
                            companyPolicy.DocumentNumber = companyPolicyDto.DocumentNumber;
                        }
                        catch (Exception)
                        {
                            companyPolicy.Endorsement.PolicyId = companyPolicyDto.id;
                            companyPolicy.Endorsement.Id = companyPolicyDto.EndorsmentId;
                            companyPolicy.DocumentNumber = companyPolicyDto.DocumentNumber;
                        }
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

        public void CreateRisk(CompanyVehicle companyVehicle)
        {
            if (companyVehicle == null || companyVehicle.Risk == null)
            {
                throw new ArgumentException(Errors.ErrorSaveRiskVehicle);
            }
            //if (companyVehicle.Risk.Status == RiskStatusType.NotModified)
            //{
            //    throw new ArgumentException(Errors.RiskNotModified);
            //}

            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();

            NameValue[] parameters = new NameValue[62];
            parameters[0] = new NameValue("@ENDORSEMENT_ID", companyVehicle.Risk.Policy.Endorsement.Id);
            parameters[1] = new NameValue("@POLICY_ID", companyVehicle.Risk.Policy.Endorsement.PolicyId);
            parameters[2] = new NameValue("@PAYER_ID", companyVehicle.Risk.Policy.Holder.IndividualId);
            parameters[3] = new NameValue("@VEHICLE_VERSION_CD", companyVehicle.Version.Id);
            parameters[4] = new NameValue("@VEHICLE_MODEL_CD", companyVehicle.Model.Id);
            parameters[5] = new NameValue("@VEHICLE_MAKE_CD", companyVehicle.Make.Id);
            parameters[6] = new NameValue("@VEHICLE_YEAR", companyVehicle.Year);
            parameters[7] = new NameValue("@VEHICLE_TYPE_CD", companyVehicle.Version.Type.Id);
            parameters[8] = new NameValue("@VEHICLE_USE_CD", companyVehicle.Use.Id);
            parameters[9] = new NameValue("@VEHICLE_BODY_CD", companyVehicle.Version.Body.Id);
            parameters[10] = new NameValue("@VEHICLE_PRICE", companyVehicle.Price);
            parameters[11] = new NameValue("@IS_NEW", companyVehicle.IsNew);
            parameters[12] = new NameValue("@LICENSE_PLATE", companyVehicle.LicensePlate);
            parameters[13] = new NameValue("@ENGINE_SER_NO", companyVehicle.EngineSerial);
            parameters[14] = new NameValue("@CHASSIS_SER_NO", companyVehicle.ChassisSerial);
            parameters[15] = new NameValue("@VEHICLE_COLOR_CD", companyVehicle.Color.Id);
            parameters[16] = new NameValue("@NEW_VEHICLE_PRICE", companyVehicle.NewPrice);
            if (companyVehicle.Version.Fuel.Id > 0)
            {
                parameters[17] = new NameValue("@VEHICLE_FUEL_CD", companyVehicle.Version.Fuel.Id);
            }
            else
            {
                parameters[17] = new NameValue("@VEHICLE_FUEL_CD", 1);
            }
            parameters[18] = new NameValue("@STD_VEHICLE_PRICE", companyVehicle.StandardVehiclePrice);
            parameters[19] = new NameValue("@FLAT_RATE_PCT", companyVehicle.Rate);
            parameters[20] = new NameValue("@DEDUCT_ID", DBNull.Value);
            parameters[21] = new NameValue("@EXCESS", false);
            parameters[22] = new NameValue("@SHUTTLE_CD", DBNull.Value);
            if (companyVehicle?.ServiceType?.Id == null || companyVehicle?.ServiceType?.Id == 0)
            {
                companyVehicle.ServiceType.Id = 1;
            }

            parameters[23] = new NameValue("@SERVICE_TYPE_CD", companyVehicle.ServiceType.Id);
            parameters[24] = new NameValue("@MOBILE_NUM", DBNull.Value);
            parameters[25] = new NameValue("@TONS_QTY", companyVehicle.Version.TonsQuantity ?? 0);
            parameters[26] = new NameValue("@RATE_TYPE_CD", DBNull.Value);
            parameters[27] = new NameValue("@PASSENGER_QTY", companyVehicle.PassengerQuantity);
            parameters[28] = new NameValue("@LOAD_TYPE_CD", DBNull.Value);
            parameters[29] = new NameValue("@TRAILERS_QTY", companyVehicle.TrailersQuantity);

            if (companyVehicle.Risk.DynamicProperties != null && companyVehicle.Risk.DynamicProperties.Count > 0)
            {
                DynamicPropertiesCollection dynamicCollectionRisk = new DynamicPropertiesCollection();
                List<DynamicConcept> dynamicPropertiesRisk = companyVehicle.Risk.DynamicProperties.Where(x => x.EntityId == RuleConceptRisk.Id).Distinct().ToList();
                for (int i = 0; i < dynamicPropertiesRisk.Count(); i++)
                {
                    DynamicProperty dinamycProperty = new DynamicProperty();
                    dinamycProperty.Id = dynamicPropertiesRisk[i].Id;
                    dinamycProperty.Value = dynamicPropertiesRisk[i].Value;
                    dynamicCollectionRisk[i] = dinamycProperty;
                }
                byte[] serializedValuesRisk = dynamicPropertiesSerializer.Serialize(dynamicCollectionRisk);
                parameters[30] = new NameValue("@DYNAMIC_PROPERTIES", serializedValuesRisk);
            }
            else
            {
                parameters[30] = new NameValue("@DYNAMIC_PROPERTIES", DBNull.Value);
            }

            parameters[31] = new NameValue("@INSURED_ID", companyVehicle.Risk.MainInsured.IndividualId);
            parameters[32] = new NameValue("@COVERED_RISK_TYPE_CD", (int)companyVehicle.Risk.CoveredRiskType);
            parameters[33] = new NameValue("@RISK_STATUS_CD", (int)companyVehicle.Risk.Status);
            parameters[34] = new NameValue("@COMM_RISK_CLASS_CD", DBNull.Value);
            parameters[35] = new NameValue("@RISK_COMMERCIAL_TYPE_CD", DBNull.Value);

            if (companyVehicle.Risk.Text == null)
            {
                parameters[36] = new NameValue("@CONDITION_TEXT", "--");
            }
            else
            {
                parameters[36] = new NameValue("@CONDITION_TEXT", companyVehicle.Risk.Text.TextBody);
            }

            parameters[37] = new NameValue("@RATING_ZONE_CD", companyVehicle.Risk.RatingZone.Id);
            parameters[38] = new NameValue("@COVER_GROUP_ID", companyVehicle.Risk.GroupCoverage.Id);
            parameters[39] = new NameValue("@IS_FACULTATIVE", false);

            if (companyVehicle.Risk.MainInsured.CompanyName != null && companyVehicle.Risk.MainInsured.CompanyName.NameNum > 0)
            {
                parameters[40] = new NameValue("@NAME_NUM", companyVehicle.Risk.MainInsured.CompanyName.NameNum);
            }
            else
            {
                parameters[40] = new NameValue("@NAME_NUM", DBNull.Value);
            }

            parameters[41] = new NameValue("@LIMITS_RC_CD", companyVehicle.Risk.LimitRc.Id);
            if (companyVehicle.Risk.LimitRc != null && companyVehicle.Risk.LimitRc.LimitSum > 0)
                parameters[42] = new NameValue("@LIMIT_RC_SUM", companyVehicle.Risk.LimitRc.LimitSum);
            else
                parameters[42] = new NameValue("@LIMIT_RC_SUM", 0, DbType.Int32);

            parameters[43] = new NameValue("@SINISTER_PCT", DBNull.Value);
            if (companyVehicle.Risk.SecondInsured != null && companyVehicle.Risk.SecondInsured.IndividualId > 0)
            {
                parameters[44] = new NameValue("@SECONDARY_INSURED_ID", companyVehicle.Risk.SecondInsured.IndividualId);
            }
            else
            {
                parameters[44] = new NameValue("@SECONDARY_INSURED_ID", DBNull.Value);
            }

            DataTable dtBeneficiaries = new DataTable("PARAM_TEMP_RISK_BENEFICIARY");
            dtBeneficiaries.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICIARY_ID", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICIARY_TYPE_CD", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICT_PCT", typeof(decimal));
            dtBeneficiaries.Columns.Add("NAME_NUM", typeof(int));

            foreach (CompanyBeneficiary item in companyVehicle.Risk.Beneficiaries)
            {
                DataRow dataRow = dtBeneficiaries.NewRow();
                dataRow["CUSTOMER_TYPE_CD"] = item.CustomerType;
                dataRow["BENEFICIARY_ID"] = item.IndividualId;
                dataRow["BENEFICIARY_TYPE_CD"] = item.BeneficiaryType?.Id ?? 1;
                dataRow["BENEFICT_PCT"] = item.Participation;

                if (item.CustomerType == CustomerType.Individual && item.CompanyName != null && item.CompanyName.NameNum == 0)
                {
                    if (item.IndividualId == companyVehicle.Risk.MainInsured.IndividualId)
                    {
                        item.CompanyName = companyVehicle.Risk.MainInsured.CompanyName;
                    }
                    else
                    {
                        item.CompanyName.TradeName = "Dirección Principal";
                        item.CompanyName.IsMain = true;
                        item.CompanyName.NameNum = 1;
                        List<CompanyName> companyNames = DelegateService.uniquePersonService.GetCompanyNamesByIndividualId(item.IndividualId);
                        if (companyNames == null)
                            companyNames = new List<CompanyName>();
                        if (companyNames?.Count == 0)
                        {
                            CompanyName companyName = new CompanyName
                            {
                                NameNum = item.CompanyName.NameNum,
                                TradeName = item.CompanyName.TradeName
                            };
                            if (item.CompanyName.Address != null)
                            {

                                companyName.Address = new Address
                                {
                                    Id = item.CompanyName.Address.Id,
                                    Description = item.CompanyName.Address.Description,
                                    City = item.CompanyName.Address.City
                                };
                            }
                            if (item.CompanyName.Phone != null)
                            {
                                companyName.Phone = new Phone
                                {
                                    Id = item.CompanyName.Phone.Id,
                                    Description = item.CompanyName.Phone.Description
                                };
                            }

                            if (item.CompanyName.Email != null)
                            {
                                companyName.Email = new Email
                                {
                                    Id = item.CompanyName.Email.Id,
                                    Description = item.CompanyName.Email.Description
                                };
                            }
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

            companyVehicle.Risk.Coverages = companyVehicle.Risk.Coverages.OrderBy(x => x.Id).ToList();


            foreach (CompanyCoverage item in companyVehicle.Risk.Coverages)
            {
                DataRow dataRow = dtCoverages.NewRow();
                dataRow["RISK_COVER_ID"] = item.Id;
                dataRow["COVERAGE_ID"] = item.Id;
                dataRow["IS_DECLARATIVE"] = item.IsDeclarative;
                dataRow["IS_MIN_PREMIUM_DEPOSIT"] = item.IsMinPremiumDeposit;
                dataRow["FIRST_RISK_TYPE_CD"] = (int)Sistran.Core.Application.UnderwritingServices.Enums.FirstRiskType.None;
                dataRow["CALCULATION_TYPE_CD"] = item.CalculationType.Value;
                dataRow["DECLARED_AMT"] = Math.Round(item.DeclaredAmount, 2);
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
                if (item.Rate == null)
                {
                    dataRow["RATE"] = DBNull.Value;
                }
                else
                {
                    dataRow["RATE"] = Math.Round((double)item.Rate, QuoteManager.PremiumRoundValue);
                }
                dataRow["CURRENT_TO"] = item.CurrentTo;
                dataRow["COVER_NUM"] = item.Number;
                if (item.CoverStatus.HasValue)
                {
                    dataRow["COVER_STATUS_CD"] = item.CoverStatus.Value;
                }
                else
                {
                    dataRow["COVER_STATUS_CD"] = CoverageStatusType.Original;
                }
                if (item.CoverageOriginalStatus.HasValue)
                {
                    dataRow["COVER_ORIGINAL_STATUS_CD"] = item.CoverageOriginalStatus.Value;
                }
                if (item.Text != null)
                {
                    dataRow["CONDITION_TEXT"] = item.Text.TextBody;
                }
                else
                {
                    dataRow["CONDITION_TEXT"] = DBNull.Value;
                }
                dataRow["ENDORSEMENT_LIMIT_AMT"] = item.EndorsementLimitAmount;
                dataRow["ENDORSEMENT_SUBLIMIT_AMT"] = item.EndorsementSublimitAmount;
                dataRow["FLAT_RATE_PCT"] = item.FlatRatePorcentage;
                dataRow["SHORT_TERM_PCT"] = item.ShortTermPercentage;
                dataRow["PREMIUM_AMT_DEPOSIT_PERCENT"] = 0; //Se asigna 0 por defecto para autos
                dataRow["MAX_LIABILITY_AMT"] = item.MaxLiabilityAmount;

                if (item.DynamicProperties != null && item.DynamicProperties.Count > 0)
                {
                    DynamicPropertiesCollection dynamicCollectionCoverage = new DynamicPropertiesCollection();
                    List<DynamicConcept> dinamicConceptCoverage = item.DynamicProperties.Where(x => x.EntityId == RuleConceptCoverage.Id).ToList();
                    for (int i = 0; i < dinamicConceptCoverage.Count(); i++)
                    {
                        DynamicProperty dinamycProperty = new DynamicProperty();
                        dinamycProperty.Id = dinamicConceptCoverage[i].Id;
                        dinamycProperty.Value = dinamicConceptCoverage[i].Value;
                        dynamicCollectionCoverage[i] = dinamycProperty;
                    }
                    byte[] serializedValuesCoverage = dynamicPropertiesSerializer.Serialize(dynamicCollectionCoverage);
                    dataRow["DYNAMIC_PROPERTIES"] = serializedValuesCoverage;
                }
                else
                {
                    dataRow["DYNAMIC_PROPERTIES"] = DBNull.Value;
                }
                if (item.Deductible != null)
                {
                    DataRow dataRowDeductible = dtDeductibles.NewRow();
                    dataRowDeductible["COVERAGE_ID"] = item.Id;
                    dataRowDeductible["RATE_TYPE_CD"] = item.Deductible.RateType;
                    dataRowDeductible["RATE"] = (object)item.Deductible.Rate ?? DBNull.Value;
                    dataRowDeductible["DEDUCT_PREMIUM_AMT"] = item.Deductible.DeductPremiumAmount;
                    dataRowDeductible["DEDUCT_VALUE"] = item.Deductible.DeductValue;
                    if (item.Deductible.DeductibleUnit != null && item.Deductible.DeductibleUnit.Id != 0)
                    {
                        dataRowDeductible["DEDUCT_UNIT_CD"] = item.Deductible.DeductibleUnit.Id;
                    }
                    else
                    {
                        dataRowDeductible["DEDUCT_UNIT_CD"] = 0;
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

            if (companyVehicle.Accesories != null && companyVehicle.Accesories.Count > 0)
            {
                CSMO.Parameter parameterOriginal = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories);
                CSMO.Parameter parameterNoOriginal = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.NonOriginalAccessories);

                for (int i = 0; i < companyVehicle.Accesories.Count(); i++)
                {
                    DataRow dataRow = dtAccessories.NewRow();
                    dataRow["SUBLIMIT_AMT"] = companyVehicle.Accesories[i].Amount;
                    dataRow["RATE_TYPE_CD"] = companyVehicle.Accesories[i].RateType;
                    dataRow["RATE"] = Math.Round(companyVehicle.Accesories[i].Rate, QuoteManager.PremiumRoundValue);
                    dataRow["PREMIUM_AMT"] = Math.Round(companyVehicle.Accesories[i].Premium, 2);
                    dataRow["ACC_PREMIUM_AMT"] = companyVehicle.Accesories[i].AccumulatedPremium;
                    dataRow["BRAND_NAME"] = companyVehicle.Accesories[i].Make ?? "";
                    dataRow["MODEL"] = companyVehicle.Accesories[i].Description ?? "";
                    dataRow["DETAIL_ID"] = companyVehicle.Accesories[i].Id;
                    if (companyVehicle.Accesories[i].IsOriginal)
                    {
                        dataRow["COVERAGE_ID"] = parameterOriginal.NumberParameter.Value;
                    }
                    else
                    {
                        dataRow["COVERAGE_ID"] = parameterNoOriginal.NumberParameter.Value;
                    }
                    dataRow["COVER_STATUS_CD"] = companyVehicle.Accesories[i].Status;
                    dtAccessories.Rows.Add(dataRow);
                }
            }
            parameters[48] = new NameValue("@INSERT_TEMP_RISK_DETAIL_ACCESSORY", dtAccessories);

            DataTable dtClauses = new DataTable("PARAM_TEMP_CLAUSE");
            dtClauses.Columns.Add("CLAUSE_ID", typeof(int));
            dtClauses.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtClauses.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
            dtClauses.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

            if (companyVehicle.Risk.Clauses != null)
            {
                foreach (CompanyClause item in companyVehicle.Risk.Clauses)
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

            if (companyVehicle.Risk.DynamicProperties != null)
            {
                foreach (DynamicConcept item in companyVehicle.Risk.DynamicProperties.Where(x => x.EntityId == RuleConceptRisk.Id).Distinct().ToList())
                {
                    DataRow dataRow = dtDynamicProperties.NewRow();
                    dataRow["DYNAMIC_ID"] = item.Id;
                    dataRow["CONCEPT_VALUE"] = item.Value ?? "NO ASIGNADO";
                    dtDynamicProperties.Rows.Add(dataRow);
                }
            }
            DataTable dtDynamicPropertiesCoverage = new DataTable("PARAM_TEMP_DYNAMIC_PROPERTIES");
            dtDynamicPropertiesCoverage.Columns.Add("DYNAMIC_ID", typeof(int));
            dtDynamicPropertiesCoverage.Columns.Add("CONCEPT_VALUE", typeof(string));
            if (companyVehicle.Risk.Coverages.Where(z => z.DynamicProperties != null).Any())
            {
                foreach (DynamicConcept item in companyVehicle.Risk.Coverages.Where(z => z.DynamicProperties != null).SelectMany(x => x.DynamicProperties).Where(x => x.EntityId == RuleConceptCoverage.Id).Distinct().ToList())
                {
                    DataRow dataRow = dtDynamicPropertiesCoverage.NewRow();
                    dataRow["DYNAMIC_ID"] = item.Id;
                    dataRow["CONCEPT_VALUE"] = item.Value ?? "NO ASIGNADO";
                    dtDynamicPropertiesCoverage.Rows.Add(dataRow);
                }
            }

            parameters[50] = new NameValue("@INSERT_TEMP_DYNAMIC_PROPERTIES", dtDynamicProperties);
            parameters[51] = new NameValue("@INSERT_TEMP_DYNAMIC_PROPERTIES_COVERAGE", dtDynamicPropertiesCoverage);
            parameters[52] = new NameValue("@RISK_NUM", companyVehicle.Risk.Number);
            parameters[53] = new NameValue("@RISK_INSP_TYPE_CD", 1);
            parameters[54] = new NameValue("@INSPECTION_ID", DBNull.Value);
            parameters[55] = new NameValue("@OPERATION", JsonConvert.SerializeObject(companyVehicle));
            //Campos de RCP
            parameters[56] = new NameValue("@GALLON_QTY", DBNull.Value);
            parameters[57] = new NameValue("@IS_TRANSFORM_VEHICLE", false);
            parameters[58] = new NameValue("@YEAR_TRANSFORM_VEHICLE", DBNull.Value);
            parameters[59] = new NameValue("@TRANSPORT_CARGO_TYPE_CD", DBNull.Value);
            parameters[60] = new NameValue("@YEAR_MODEL", DBNull.Value);
            parameters[61] = new NameValue("@RETENTION", false);

            DataTable result;

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                try
                {
                    result = pdb.ExecuteSPDataTable("ISS.RECORD_RISK_VEHICLE", parameters);
                }
                catch (Exception ex)
                {
                    throw new BusinessException(ExceptionManager.GetMessage(ex, "ErrorCreateCompanyVehicle"), ex);
                }
            }

            if (result != null && result.Rows.Count > 0)
            {
                string error = result.Rows[0][0].ToString();
                if (!string.IsNullOrEmpty(error) && !Convert.ToBoolean(result.Rows[0][0]))
                {

                    throw new Exception(error);
                }
            }
            else
            {
                throw new ValidationException(Errors.ErrorRecordEndorsement);
            }
        }

        private void ValidateInfringementPolicies(CompanyPolicy companyPolicy, List<CompanyVehicle> companyVehicles)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            infringementPolicies.AddRange(companyPolicy.InfringementPolicies);

            if (companyVehicles != null && companyVehicles.Any())
            {
                companyVehicles.ForEach(x =>
            {
                if (x.Risk.InfringementPolicies != null && x.Risk.InfringementPolicies.Count > 0)
                {
                    infringementPolicies.AddRange(x.Risk.InfringementPolicies);
                }
            });
            }
            companyPolicy.InfringementPolicies = DelegateService.authorizationPoliciesService.ValidateInfringementPolicies(infringementPolicies);
        }

        /// <summary>
        /// Polizas asociadas a individual
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<CompanyVehicle> GetVehiclesByIndividualId(int individualId)
        {
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);

            RiskVehicleView view = new RiskVehicleView();
            ViewBuilder builder = new ViewBuilder("RiskVehicleView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            DataFacadeManager.Dispose();
            if (view.CompanyVehicles.Count > 0)
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Risk> risks = view.CompanyVehicles.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.Policy> policies = view.Policies.Cast<ISSEN.Policy>().ToList();
                List<ISSEN.RiskVehicle> riskVehicles = view.RiskVehicles.Cast<ISSEN.RiskVehicle>().ToList();
                List<COMMEN.VehicleModel> vehicleModels = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();
                List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
                List<COMMEN.VehicleVersion> vehicleVersions = view.VehicleVersions.Cast<COMMEN.VehicleVersion>().ToList();
                List<COMMEN.VehicleColor> vehicleColor = view.VehicleColors.Cast<COMMEN.VehicleColor>().ToList();
                List<COMMEN.VehicleUse> vehicleUse = view.VehicleUses.Cast<COMMEN.VehicleUse>().ToList();
                List<COMMEN.VehicleType> vehicleType = view.VehicleTypes.Cast<COMMEN.VehicleType>().ToList();

                foreach (ISSEN.Risk item in risks)
                {
                    DataFacadeManager.Instance.GetDataFacade().LoadDynamicProperties(item);
                    DataFacadeManager.Dispose();
                    CompanyVehicle vehicle = new CompanyVehicle();
                    ISSEN.EndorsementRisk endorsementRisk = endorsementRisks.Where(x => x.RiskId == item.RiskId).First();
                    vehicle = ModelAssembler.CreateVehicle(riskVehicles.Where(x => x.RiskId == item.RiskId).First(), policies.Where(x => x.PolicyId == endorsementRisk.PolicyId).First());
                    vehicle.Risk.Policy.Endorsement = new CompanyEndorsement() { Id = endorsementRisk.EndorsementId };
                    vehicle.Risk.RiskId = endorsementRisk.RiskId;
                    vehicle.Make.Description = vehicleMakes.Where(x => x.VehicleMakeCode == vehicle.Make.Id).FirstOrDefault().SmallDescription;
                    vehicle.Model.Description = vehicleModels.Where(x => x.VehicleModelCode == vehicle.Model.Id).FirstOrDefault().SmallDescription;
                    vehicle.Version.Description = vehicleVersions.Where(x => x.VehicleVersionCode == vehicle.Version.Id).FirstOrDefault().Description;
                    vehicle.Color.Description = vehicleColor.Where(x => x.VehicleColorCode == vehicle.Color.Id).FirstOrDefault().SmallDescription;
                    vehicle.Use.Description = vehicleUse.Where(x => x.VehicleUseCode == vehicle.Use.Id).FirstOrDefault().SmallDescription;
                    vehicle.Version.Type.Description = vehicleType.Where(x => x.VehicleTypeCode == vehicle.Version.Type.Id).FirstOrDefault().Description;
                    companyVehicles.Add(vehicle);
                }
            }
            return companyVehicles.GroupBy(b => new { branchId = b.Risk.Policy.Branch.Id, prefixId = b.Risk.Policy.Prefix.Id, documentNumber = b.Risk.Policy.DocumentNumber }).Select(b => b.LastOrDefault()).ToList(); ;

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

        public void AddBeneficiariesClauses(List<CompanyClause> riskClauses)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COCOMMEN.CoBeneficiaryClause.Properties.BeneficiaryTypeId, Core.Application.Utilities.Configuration.KeySettings.OnerousBeneficiaryTypeId);
            filter.And();
            filter.PropertyEquals(COCOMMEN.CoBeneficiaryClause.Properties.LevelId, EmissionLevel.Risk);
            filter.And();
            filter.Property(COCOMMEN.CoBeneficiaryClause.Properties.BeneficiaryId).IsNull();
            BusinessCollection<COCOMMEN.CoBeneficiaryClause> coBeneficiaryClauses = DataFacadeManager.Instance.GetDataFacade().List<COCOMMEN.CoBeneficiaryClause>(filter.GetPredicate());
            DataFacadeManager.Dispose();
            if (coBeneficiaryClauses.Count > 0)
            {
                List<int> beneficiaryClausesId = coBeneficiaryClauses.Cast<COCOMMEN.CoBeneficiaryClause>()
                    .Where(x => x.ClauseId.HasValue).Select(x => x.ClauseId.Value).ToList();
                if (beneficiaryClausesId.Any())
                {
                    IMapper imapper = ModelAssembler.CreateMapCompanyClause();
                    List<CompanyClause> clauseBeneficiary = imapper.Map<List<UNMOD.Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByClauseIds(beneficiaryClausesId).ToList());
                    riskClauses.AddRange(clauseBeneficiary);
                }
            }
        }
        /// <summary>
        /// Gets the vehicle by endorsement identifier.
        /// </summary>
        /// <param name="riskId">The risk identifier.</param>
        /// <returns></returns>
        public CompanyVehicle GetVehicleByRiskId(int riskId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.RiskVehicle.Properties.RiskId, typeof(ISSEN.RiskVehicle).Name);
            filter.Equal();
            filter.Constant(riskId);
            ISSEN.RiskVehicle vehicle = (ISSEN.RiskVehicle)new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ISSEN.RiskVehicle), filter.GetPredicate())).FirstOrDefault();
            DataFacadeManager.Dispose();
            if (vehicle != null)
            {
                CompanyVehicle companyVehicle = ModelAssembler.CreateVehicle(vehicle);
                return companyVehicle;
            }
            else
            {
                return null;
            }
        }  
		
		public CompanyVehicle GetCompanyRiskVehicleByRiskId(int riskId)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.RiskVehicle.Properties.RiskId, typeof(ISSEN.RiskVehicle).Name);
            filter.Equal();
            filter.Constant(riskId);
            var vehicle = (ISSEN.RiskVehicle)new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ISSEN.RiskVehicle), filter.GetPredicate())).FirstOrDefault();

            if (vehicle != null)
            {
                PrimaryKey primaryKey = ISSEN.Risk.CreatePrimaryKey(riskId);
                ISSEN.Risk entityRisk = (ISSEN.Risk)DataFacadeManager.GetObject(primaryKey);

                var companyVehicle = ModelAssembler.CreateVehicle(vehicle);
                companyVehicle.Risk = ModelAssembler.CreateRisk(entityRisk);

                return companyVehicle;
            }
            else
            {
                return null;
            }
        }

        private void AddCoveragesToRisk(CompanyVehicle vehicleRisk, int policyId, int endorsementId, int riskId)
        {
            vehicleRisk.Risk = vehicleRisk.Risk ?? new CompanyRisk();
            vehicleRisk.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId);
        }

        public void DeletePendingOperation(int id)
        {
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@OPERATION_ID", id);
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                pdb.ExecuteSPNonQuery("ISS.DELETE_PENDING_OPERATIONS", parameters);
            }
        }

        /// <summary>
        /// Obtener lista de Causas
        /// </summary>
        /// <returns>Lista de causas</returns>
        public List<Models.CompanyNotInsurableCause> GetNotInsurableCauses()
        {
            BusinessCollection businessCollection = null;
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(COCOMMEN.CiaNonInsurableCauses)));
            }
            return ModelAssembler.CreateCauses(businessCollection);
        }
        public Result<List<CompanyValidationPlate>, ErrorModel> GetValidationPlate()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COCOMMEN.CiaVehicleEnabled)));
                List<CompanyValidationPlate> paramValidationPlate = ModelAssembler.CreateValidationPlates(businessCollection);
                return new ResultValue<List<CompanyValidationPlate>, ErrorModel>(paramValidationPlate);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add("Error");
                return new ResultError<List<CompanyValidationPlate>, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, Utilities.Enums.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="vehicle">Modelo vehicle</param>
        public CompanyVehicle CompanySaveVehicleTemporal(CompanyVehicle companyVehicle)
        {
            PendingOperation pendingOperation = new PendingOperation();
            CompanyPolicy policy = companyVehicle.Risk.Policy;
            companyVehicle.Risk.Policy = null;
            companyVehicle.NewPrice = companyVehicle.NewPrice == 0 ? 1 : companyVehicle.NewPrice;
            if (companyVehicle.Risk.Id == 0)
            {
                pendingOperation.ParentId = policy.Id;
                pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyVehicle);
                pendingOperation.UserId = policy.UserId;
                pendingOperation.UserName = policy.User?.AccountName;
                pendingOperation.OperationName = policy.TemporalTypeDescription;
                pendingOperation.AdditionalInformation = companyVehicle.Risk.RiskId.ToString();
                pendingOperation.IsMassive = policy.PolicyOrigin != PolicyOrigin.Individual ? true : false;
                pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
                companyVehicle.Risk.Id = pendingOperation.Id;
                pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyVehicle);
                DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
            }
            else
            {
                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(companyVehicle.Risk.Id);
                if (pendingOperation != null)
                {
                    pendingOperation.ParentId = policy.Id;
                    pendingOperation.IsMassive = policy.PolicyOrigin != PolicyOrigin.Individual ? true : false;
                    pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyVehicle);
                    pendingOperation.AdditionalInformation = companyVehicle.Risk.RiskId.ToString();
                    DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
                }
                else
                {
                    throw new Exception("Error obteniendo el Temporal");
                }
            }

            companyVehicle.Risk.Id = pendingOperation.Id;
            companyVehicle.Risk.Policy = policy;

            return companyVehicle;
        }

        public int SaveCompanyVehicleTemporalTables(CompanyVehicle companyVehicle)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer =
            new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();
            UTILITES.GetDatatables d = new UTILITES.GetDatatables();
            UTILITES.CommonDataTables dts = d.GetcommonDataTables(companyVehicle.Risk);

            DataTable dataTable;
            NameValue[] parameters = new NameValue[18];

            DataTable dtTempRisk = dts.dtTempRisk;
            foreach (DataRow row in dtTempRisk.Rows)
            {
                if (companyVehicle.Inspection != null && companyVehicle.Inspection.InspectionType > 0)
                {
                    row["RISK_INSP_TYPE_CD"] = companyVehicle.Inspection.InspectionType;
                }
                else
                {
                    row["RISK_INSP_TYPE_CD"] = 1;
                }
            }
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

            DataTable tdAccessory = ModelAssembler.GetDataTableAccessory(companyVehicle);
            parameters[10] = new NameValue(tdAccessory.TableName, tdAccessory);

            DataTable dtTempRiskVehicle = ModelAssembler.GetDataTableRiskVehicle(companyVehicle);
            parameters[11] = new NameValue(dtTempRiskVehicle.TableName, dtTempRiskVehicle);

            DataTable dtCOTempRiskVehicle = ModelAssembler.GetDataTableTemRiskVehicle(companyVehicle);
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
                if (companyVehicle.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
                {
                    companyVehicle.Risk.RiskId = Convert.ToInt32(dataTable.Rows[0][0]);
                }
                return companyVehicle.Risk.RiskId;
            }
            else
            {
                throw new ValidationException(Errors.ErrorCreateTemporalCompanyVehicle);
            }
        }

        public CompanyVehicle SaveCompanyVehicleTemporal(CompanyVehicle companyVehicle)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer =
            new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();

            DataTable dataTable;
            NameValue[] parameters = new NameValue[4];

            DataTable dtTempRisk = ModelAssembler.GetDataTableTempRISK(companyVehicle);
            parameters[0] = new NameValue(dtTempRisk.TableName, dtTempRisk);

            DataTable dtCOTempRisk = ModelAssembler.GetDataTableCOTempRisk(companyVehicle);
            parameters[1] = new NameValue(dtCOTempRisk.TableName, dtCOTempRisk);

            DataTable dtTempRiskVehicle = ModelAssembler.GetDataTableRiskVehicle(companyVehicle);
            parameters[2] = new NameValue(dtTempRiskVehicle.TableName, dtTempRiskVehicle);

            DataTable dtCOTempRiskVehicle = ModelAssembler.GetDataTableTemRiskVehicle(companyVehicle);
            parameters[3] = new NameValue(dtCOTempRiskVehicle.TableName, dtCOTempRiskVehicle);

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {

                dataTable = pdb.ExecuteSPDataTable("TMP.CIA_SAVE_TEMPORAL_RISK_VEHICLE", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (companyVehicle.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
                {
                    companyVehicle.Risk.RiskId = Convert.ToInt32(dataTable.Rows[0][0]);
                }
                return companyVehicle;
            }
            else
            {
                throw new ValidationException(Errors.ErrorCreateTemporalCompanyVehicle);//ErrrRecordTemporal "error al grabar riesgo
            }
        }

        public CompanyMinPremium GetMinimumPremium(int prefixId, EndorsementType? endorsementTypeId, int productId, int groupCoverageId, bool filterEntoType = true)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(MinPremiumRelation.Properties.PrefixCode, typeof(MinPremiumRelation).Name);
                filter.Equal();
                filter.Constant(prefixId);
                filter.And();
                if (filterEntoType)
                {
                    filter.Property(MinPremiumRelation.Properties.EndoTypeCode, typeof(MinPremiumRelation).Name);
                    filter.Equal();
                    filter.Constant(endorsementTypeId);
                    filter.And();
                }
                filter.Property(MinPremiumRelation.Properties.Key1, typeof(MinPremiumRelation).Name);
                filter.Equal();
                filter.Constant(productId);
                filter.And();
                filter.Property(MinPremiumRelation.Properties.Key2, typeof(MinPremiumRelation).Name);
                filter.Equal();
                filter.Constant(groupCoverageId);

                MinPremiumRelation entityMinPremiumRelation = (MinPremiumRelation)DataFacadeManager.GetObjects(typeof(MinPremiumRelation), filter.GetPredicate()).FirstOrDefault();
                DataFacadeManager.Dispose();
                if (entityMinPremiumRelation != null)
                    return ModelAssembler.CreateCompanyMinPremium(entityMinPremiumRelation);
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetBaseException().Message, ex);
            }
        }
        public List<CompanyCiaGroupCoverage> GetGroupCoverage(int productId, int groupCoverageId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(CiaGroupCoverage.Properties.ProductId, typeof(MinPremiumRelation).Name);
                filter.Equal();
                filter.Constant(productId);
                filter.And();
                filter.Property(CiaGroupCoverage.Properties.CoverGroupId, typeof(MinPremiumRelation).Name);
                filter.Equal();
                filter.Constant(groupCoverageId);

                BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(CiaGroupCoverage), filter.GetPredicate());
                DataFacadeManager.Dispose();
                return ModelAssembler.CreateCompanyCiaGroupCoverage(businessCollection);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetBaseException().Message, ex);
            }

        }

        /// <summary>
        /// Trae la lista de servicios 
        /// </summary>
        /// <param name="Service">Modelo Service</param>
        public List<CompanyServiceType> GetServiceType()
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(COMMEN.ServiceType), filter.GetPredicate());
                DataFacadeManager.Dispose();
                return ModelAssembler.CreateCompanyService(businessCollection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetBaseException().Message, ex);
            }
        }

        public void CreateVehicleTemporalQueue(CompanyVehicle companyVehicle, string modelRow, string modelName)
        {
            PendingOperation pendingOperation = new PendingOperation();

            pendingOperation.CreationDate = DateTime.Now;
            pendingOperation.ParentId = companyVehicle.Risk.Policy.Id;
            pendingOperation.IsMassive = companyVehicle.Risk.Policy.PolicyOrigin != PolicyOrigin.Individual ? true : false;
            companyVehicle.Risk.Policy = null;
            pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyVehicle);

            string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}", COMUT.JsonHelper.SerializeObjectToJson(pendingOperation), (char)007, modelRow, (char)007, modelName);
            IQueue queue = new BaseQueueFactory().CreateQueue("CreatePendingOperationQueue", routingKey: "CreatePendingOperationQueue", serialization: "JSON");
            queue.PutOnQueue(pendingOperationJson);
        }

        public void UpdateVehicleTemporalQueue(CompanyVehicle companyVehicle, string modelRow, string modelName)
        {
            if (string.IsNullOrEmpty(modelRow) && string.IsNullOrEmpty(modelRow))
            {
                IQueue queue = new BaseQueueFactory().CreateQueue("UpdatePendingOperationQueue", routingKey: "UpdatePendingOperationQueue", serialization: "JSON");
                queue.PutOnQueue(JsonConvert.SerializeObject(companyVehicle));
            }
            else
            {
                string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}", JsonConvert.SerializeObject(companyVehicle), (char)007, modelRow, (char)007, modelName);
                // IQueue queue = new BaseQueueFactory().CreateQueue("UpdatePendingOperationWithRowQueue", routingKey: "CreatePendingOperationWithRowQueue", serialization: "JSON");
                IQueue queue = new BaseQueueFactory().CreateQueue("UpdatePendingOperationWithRowQueue", routingKey: "UpdatePendingOperationWithRowQueue", serialization: "JSON");
                queue.PutOnQueue(pendingOperationJson);
            }
        }

        public void CreateRiskQueue(CompanyVehicle companyVehicle, string modelRow, string modelName)
        {
            string issuanceRiskJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(companyVehicle), (char)007, modelRow, (char)007, nameof(CompanyVehicle), (char)007, modelName);
            IQueue queue = new BaseQueueFactory().CreateQueue("CreateRiskQueue", routingKey: "CreateRiskQueue", serialization: "JSON");
            queue.PutOnQueue(issuanceRiskJson);
        }

        /// <summary>
        /// Cantidad de siniestros por poliza
        /// </summary>
        /// <param name="Policy"></param>
        /// <returns></returns>
        public int GetCountSinister(CompanyVehicle companyVehicle)
        {
            int recordCount = 0;
            if (companyVehicle.Risk.Policy.DocumentNumber != 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(TEM.FianzaPolicyClaims.Properties.PolicyNum, typeof(TEM.FianzaPolicyClaims).Name);
                filter.Equal();
                filter.Constant(companyVehicle.Risk.Policy.DocumentNumber);
                filter.And();
                filter.Property(TEM.FianzaPolicyClaims.Properties.BranchCode, typeof(TEM.FianzaPolicyClaims).Name);
                filter.Equal();
                filter.Constant(companyVehicle.Risk.Policy.Branch.Id);
                filter.And();
                filter.Property(TEM.FianzaPolicyClaims.Properties.PrefixCode, typeof(TEM.FianzaPolicyClaims).Name);
                filter.Equal();
                filter.Constant(companyVehicle.Risk.Policy.Prefix.Id);
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
        public bool GetHasTotalLoss(CompanyVehicle companyVehicle)
        {
            if (companyVehicle.Risk.Policy.DocumentNumber != 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(TEM.VehiclePolicyClaims.Properties.PolicyNum, typeof(TEM.VehiclePolicyClaims).Name);
                filter.Equal();
                filter.Constant(companyVehicle.Risk.Policy.DocumentNumber);
                filter.And();
                filter.Property(TEM.VehiclePolicyClaims.Properties.PolicyNum, typeof(TEM.VehiclePolicyClaims).Name);
                filter.Equal();
                filter.Constant(true);
                filter.And();
                filter.Property(TEM.VehiclePolicyClaims.Properties.BranchCode, typeof(TEM.VehiclePolicyClaims).Name);
                filter.Equal();
                filter.Constant(companyVehicle.Risk.Policy.Branch.Id);
                filter.And();
                filter.Property(TEM.VehiclePolicyClaims.Properties.PrefixCode, typeof(TEM.VehiclePolicyClaims).Name);
                filter.Equal();
                filter.Constant(companyVehicle.Risk.Policy.Prefix.Id);
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
        public decimal GetHasPortfolioBalance(CompanyVehicle companyVehicle)
        {
            decimal ValorPortafolio = 0;
            if (companyVehicle.Risk.Policy.DocumentNumber != 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(TEM.CoBorrowingPolicy.Properties.PolicyNum, typeof(TEM.CoBorrowingPolicy).Name);
                filter.Equal();
                filter.Constant(companyVehicle.Risk.Policy.DocumentNumber);
                filter.And();
                filter.Property(TEM.CoBorrowingPolicy.Properties.PolicyNum, typeof(TEM.CoBorrowingPolicy).Name);
                filter.Equal();
                filter.Constant(true);
                filter.And();
                filter.Property(TEM.CoBorrowingPolicy.Properties.BranchCode, typeof(TEM.CoBorrowingPolicy).Name);
                filter.Equal();
                filter.Constant(companyVehicle.Risk.Policy.Branch.Id);
                filter.And();
                filter.Property(TEM.CoBorrowingPolicy.Properties.PrefixCode, typeof(TEM.CoBorrowingPolicy).Name);
                filter.Equal();
                filter.Constant(companyVehicle.Risk.Policy.Prefix.Id);
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

        public IssuanceIdentificationDocument GetIdentificationDocumentByInsured(int insuredId, int customerTypeCode)
        {
            IssuanceInsured insured = DelegateService.underwritingService.GetInsuredByIndividualId(insuredId);
            return insured?.IdentificationDocument;
        }
        /// <summary>
        /// Consulta si la placa, motor o chasis ya existe en una póliza
        /// </summary>
        /// <param name="licensePlate">Placa</param>
        /// <param name="engineNumber">Motor</param>
        /// <param name="chassisNumber">Chasis</param>
        /// <param name="productId">Id Producto</param>
        /// <returns>Mensaje</returns>
        public string ExistsRiskByLicensePlateEngineNumberChassisNumberCompany(string licensePlate, string engineNumber, string chassisNumber, int policiId, DateTime currentFrom)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            VEDAO.VehicleDAO coreVehicleDao = new VEDAO.VehicleDAO();
            var or = 0;
            if (licensePlate != "TL")
            {
                filter.Property(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name).Equal().Constant(licensePlate);
                //filter.Or(); 
                or = 1;

            }
            if (engineNumber != "NA")
            {
                if (or == 1) { filter.Or(); }
                filter.Property(ISSEN.RiskVehicle.Properties.EngineSerNo, typeof(ISSEN.RiskVehicle).Name).Equal().Constant(engineNumber);
                or = 1;
                //filter.Or();
            }
            if (chassisNumber != "NA")
            {
                if (or == 1) { filter.Or(); }
                filter.Property(ISSEN.RiskVehicle.Properties.ChassisSerNo, typeof(ISSEN.RiskVehicle).Name).Equal().Constant(chassisNumber);

            }

            filter.And();


            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name).Equal().Constant(1);
            filter.And();
            filter.Not();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.In();
            filter.ListValue();
            filter.Constant((int)RiskStatusType.Excluded);
            filter.Constant((int)RiskStatusType.Cancelled);
            filter.EndList();
            filter.And();
            filter.Property(ISSEN.Policy.Properties.CurrentTo, typeof(ISSEN.Policy).Name).Greater().Constant(currentFrom);

            if (policiId > 0)
            {
                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
                filter.Distinct();
                filter.Constant(policiId);
            }

            ExistsVehicleView view = new ExistsVehicleView();
            ViewBuilder builder = new ViewBuilder("ExistsVehicleView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            DataFacadeManager.Dispose();

            if (view.RiskVehicles.Count > 0)
            {
                var risksVehicles = view.RiskVehicles.Cast<ISSEN.RiskVehicle>();
                var message = "";

                var vehicleEngine = risksVehicles.FirstOrDefault(x => x.EngineSerNo == engineNumber);
                if (vehicleEngine != null)
                {
                    ISSEN.Policy policy = view.Policies.Cast<ISSEN.Policy>().SingleOrDefault(X => X.PolicyId ==
                    (view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().SingleOrDefault(y => y.RiskId == vehicleEngine.RiskId).PolicyId));
                    return new VEDAO.VehicleDAO().CreateMessageExist(vehicleEngine, policy);
                }

                var vehiclePlate = risksVehicles.FirstOrDefault(x => x.LicensePlate == licensePlate);
                if (vehiclePlate != null)
                {
                    ISSEN.Policy policy = view.Policies.Cast<ISSEN.Policy>().SingleOrDefault(X => X.PolicyId ==
                   (view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().SingleOrDefault(y => y.RiskId == vehiclePlate.RiskId).PolicyId));
                    return new VEDAO.VehicleDAO().CreateMessageExist(vehiclePlate, policy);
                }

                var vehicleChasis = risksVehicles.FirstOrDefault(x => x.ChassisSerNo == chassisNumber);
                if (vehicleChasis != null)
                {
                    ISSEN.Policy policy = view.Policies.Cast<ISSEN.Policy>().SingleOrDefault(X => X.PolicyId ==
                   (view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().SingleOrDefault(y => y.RiskId == vehicleChasis.RiskId).PolicyId));
                    return new VEDAO.VehicleDAO().CreateMessageExist(vehicleChasis, policy);
                }

                return message;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Creacion IssuanceName
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public IssuanceCompanyName CreateCompanyName(CompanyName companyName)
        {
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
            return issuanceCompanyName;
        }

        public int GetSummaryRisk(CompanyEndorsement endorsement)
        {
            List<CompanyVehicle> vehicles = new List<CompanyVehicle>();
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("@POLICY_ID", endorsement.PolicyId);
            parameters[1] = new NameValue("@ENDORSEMENT_ID", endorsement.Id);
            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("ISS.GET_SUMMARY_RISK_DATA", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {

                return result.Rows.Count;
            }
            else
            {
                return 0;
            }
        }
    }
}