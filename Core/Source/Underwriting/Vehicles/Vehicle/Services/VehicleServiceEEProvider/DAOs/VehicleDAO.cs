using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.Assemblers;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Diagnostics;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using VEDTO = Sistran.Core.Application.Vehicles.VehicleServices.DTOs;
using Sistran.Co.Application.Data;
using System.Data;
using System;
using Sistran.Core.Application.UnderwritingServices.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.View;
using System.Threading.Tasks;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.CommonService.Enums;
using AutoMapper;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.DAOs
{
    public class VehicleDAO : Sistran.Core.Application.Vehicles.EEProvider.DAOs.VehicleDAO
    {
        /// <summary>
        /// Obtener Usos 
        /// </summary>
        /// <returns>Lista de Usos</returns>
        public List<Models.Use> GetUses()
        {
            BusinessCollection businessCollection = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(COMMEN.VehicleUse)));
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.DAOs.GetUses");
            return ModelAssembler.CreateUses(businessCollection);
        }


        /// <summary>
        /// Obtener Usos pór tipo de carroceria
        /// </summary>
        /// <param name="bodyId"></param> 
        /// <returns></returns>
        public List<Models.Use> GetUsesByBodyId(int bodyId)
        {
            BusinessCollection businessCollection = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Models.Use> uses = new List<Models.Use>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(VehicleBodyUse.Properties.VehicleBodyCode, typeof(VehicleBodyUse).Name);
            filter.Equal();
            filter.Constant(bodyId);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(COMMEN.VehicleBodyUse), filter.GetPredicate()));
            }

            List<Models.Use> listUses = GetUses();

            foreach (VehicleBodyUse item in businessCollection)
            {
                if (listUses.Exists(x => x.Id == item.VehicleUseCode))
                {
                    Models.Use use = new Models.Use()
                    {
                        Id = item.VehicleUseCode,
                        Description = listUses.First(x => x.Id == item.VehicleUseCode).Description
                    };
                    uses.Add(use);
                }

            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.DAOs.GetUses");
            return uses;
        }

        /// <summary>
        /// Obtener Precio del Vehículo
        /// </summary>
        /// <param name="makeId">Id Marca</param>
        /// <param name="modelId">Id Modelo</param>
        /// <param name="versionId">Id Version</param>
        /// <param name="year">Año</param>
        /// <returns>Precio Vehículo</returns>
        public decimal GetPriceByMakeIdModelIdVersionId(int makeId, int modelId, int versionId, int year)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            if (year == 0)
            {
                BusinessCollection businessCollection = null;
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(VehicleVersionYear.Properties.VehicleMakeCode, typeof(VehicleVersionYear).Name);
                filter.Equal();
                filter.Constant(makeId);
                filter.And();
                filter.Property(VehicleVersionYear.Properties.VehicleModelCode, typeof(VehicleVersionYear).Name);
                filter.Equal();
                filter.Constant(modelId);
                filter.And();
                filter.Property(VehicleVersionYear.Properties.VehicleVersionCode, typeof(VehicleVersionYear).Name);
                filter.Equal();
                filter.Constant(versionId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(VehicleVersionYear), filter.GetPredicate()));
                }

                List<Vehicles.Models.Year> years = Vehicles.EEProvider.Assemblers.ModelAssembler.CreateYears(businessCollection);

                years = years.OrderByDescending(x => x.Description).ToList();

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.DAOs.GetPriceByMakeIdModelIdVersionId");
                return years[0].Price;
            }
            else
            {
                VehicleVersionYear versionYear = null;
                PrimaryKey key = VehicleVersionYear.CreatePrimaryKey(versionId, modelId, makeId, year);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    versionYear = (VehicleVersionYear)daf.GetObjectByPrimaryKey(key);
                }

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.DAOs.GetPriceByMakeIdModelIdVersionId");

                if (versionYear != null)
                    return Vehicles.EEProvider.Assemblers.ModelAssembler.CreateYear(versionYear).Price;

                return 0;
            }
        }

        public List<Models.Vehicle> GetVehiclesByIndividualId(int individualId)
        {
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("LICENSE_PLATE", null);
            parameters[1] = new NameValue("INDIVIDUAL_ID", individualId);

            DataTable result = null;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("CLM.GET_VEHICLE_POLICIES", parameters);
            }

            return CreateVehicles(result.Rows);
        }

        public static List<Models.Vehicle> CreateVehicles(DataRowCollection dataRow)
        {
            List<Models.Vehicle> vehicles = new List<Models.Vehicle>();
            foreach (DataRow item in dataRow)
            {
                Models.Vehicle vehicle = new Models.Vehicle()
                {
                    Risk = new Risk
                    {
                        Policy = new UnderwritingServices.Models.Policy()
                        {
                            DocumentNumber = Convert.ToDecimal(item[0])
                        }
                    },
                    LicensePlate = Convert.ToString(item[1]),
                    Make = new Vehicles.Models.Make()
                    {
                        Description = Convert.ToString(item[2])
                    },
                    Model = new Vehicles.Models.Model()
                    {
                        Description = Convert.ToString(item[3])
                    },
                    Year = Convert.ToInt32(item[4]),
                    ChassisSerial = Convert.ToString(item[6]),
                    EngineSerial = Convert.ToString(item[7]),
                    Color = new Vehicles.Models.Color()
                    {
                        Description = Convert.ToString(item[5])
                    },
                };
                vehicles.Add(vehicle);
            }
            return vehicles;
        }

        public List<Vehicle> GetRiskVehiclesByEndorsementId(int endorsementId)
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
                List<Vehicle> companyVehicles = ModelAssembler.CreateVehicles(businessCollection);

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

        private List<Vehicle> GetCompanyVehiclesFromTables(int endorsementId)
        {
            List<Vehicle> companyVehicles = new List<Vehicle>();
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
                List<ISSEN.RiskVehicle> riskVehicles = view.RiskVehicles.Cast<ISSEN.RiskVehicle>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<COMMEN.VehicleModel> vehicleModels = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();
                List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
                List<COMMEN.VehicleVersion> vehicleVersions = view.VehicleVersions.Cast<COMMEN.VehicleVersion>().ToList();
                Parallel.ForEach(risks, ParallelHelper.DebugParallelFor(), item =>
                {
                    DataFacadeManager.Instance.GetDataFacade().LoadDynamicProperties(item);
                    Vehicle vehicle = new Vehicle();

                    vehicle = ModelAssembler.CreateVehicle(item,
                        riskVehicles.Where(x => x.RiskId == item.RiskId).First(),
                        endorsementRisks.Where(x => x.RiskId == item.RiskId).First()
                        );

                    vehicle.Make.Description = vehicleMakes.Where(x => x.VehicleMakeCode == vehicle.Make.Id).FirstOrDefault().SmallDescription;
                    vehicle.Model.Description = vehicleModels.Where(x => x.VehicleModelCode == vehicle.Model.Id).FirstOrDefault().SmallDescription;
                    vehicle.Version.Description = vehicleVersions.Where(x => x.VehicleVersionCode == vehicle.Version.Id).FirstOrDefault().Description;

                    int insuredNameNum = vehicle.Risk.MainInsured.CompanyName.NameNum;

                    vehicle.Risk.MainInsured = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(vehicle.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

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
                    vehicle.Risk.Beneficiaries = new List<Beneficiary>();
                    foreach (ISSEN.RiskBeneficiary riskBeneficiary in riskBeneficiaries.Where(x => x.RiskId == item.RiskId))
                    {
                        Beneficiary beneficiary = ModelAssembler.CreateBeneficiary(riskBeneficiary);

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
                        vehicle.Risk.Beneficiaries.Add(beneficiary);
                    }

                    vehicle.Risk.Coverages = DelegateService.underwritingService.GetCoveragesByPolicyIdEndorsementIdRiskId(endorsementRisks[0].PolicyId, endorsementId, item.RiskId);

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
                        Parallel.ForEach(vehicle.Accesories, accessory =>
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

        public List<Vehicle> GetRiskVehiclesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            List<Vehicle> vehicles = new List<Vehicle>();
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
                vehicles = ModelAssembler.CreateVehiclesByRiskVehicle(view.RiskVehicles);

                List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<COMMEN.VehicleColor> vehicleColors = view.VehicleColors.Cast<COMMEN.VehicleColor>().ToList();
                List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
                List<COMMEN.VehicleModel> vehicleModel = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();

                foreach (Vehicle vehicle in vehicles)
                {
                    ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                    filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filterSumAssured.Equal();
                    filterSumAssured.Constant(endorsementId);
                    SumAssuredView assuredView = new SumAssuredView();
                    ViewBuilder builderAssured = new ViewBuilder("SumAssuredView");
                    builderAssured.Filter = filterSumAssured.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                    decimal insuredAmount = 0;

                    foreach (ISSEN.RiskCoverage entityRiskCoverage in assuredView.RiskCoverages)
                    {
                        insuredAmount += entityRiskCoverage.LimitAmount;
                    }

                    vehicle.Risk = ModelAssembler.CreateRisk(risks.First(x => x.RiskId == vehicle.Risk.RiskId));
                    vehicle.Risk.Number = entityEndorsementRisks.First(x => x.RiskId == vehicle.Risk.RiskId).RiskNum;
                    vehicle.Risk.MainInsured = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(vehicle.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                    vehicle.Make.Description = vehicleMakes.FirstOrDefault(x => x.VehicleMakeCode == vehicle.Make.Id)?.SmallDescription;
                    vehicle.Model.Description = vehicleModel.FirstOrDefault(x => x.VehicleModelCode == vehicle.Model.Id)?.SmallDescription;
                    vehicle.Color.Description = vehicleColors.FirstOrDefault(x => x.VehicleColorCode == vehicle.Color.Id)?.SmallDescription;
                    vehicle.Risk.AmountInsured = insuredAmount;
                }
            }

            return vehicles;
        }

        public Vehicle GetVehicleByLicensePlate(string licencePlate)
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
                Vehicle vehicle = ModelAssembler.CreateVehicleByRiskVehicle((ISSEN.RiskVehicle)businessCollection.FirstOrDefault());
                return vehicle;
            }
            else
            {
                return null;
            }
        }

        public List<Vehicle> GetRisksVehicleByInsuredId(int insuredId)
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(insuredId);

            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);

            ClaimRiskVehicleView view = new ClaimRiskVehicleView();
            ViewBuilder builder = new ViewBuilder("ClaimRiskVehicleView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                vehicles = ModelAssembler.CreateVehiclesByRiskVehicle(view.RiskVehicles);

                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.Policy> policies = view.Policies.Cast<ISSEN.Policy>().ToList();
                List<COMMEN.VehicleColor> vehicleColors = view.VehicleColors.Cast<COMMEN.VehicleColor>().ToList();
                List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
                List<COMMEN.VehicleModel> vehicleModel = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                foreach (Vehicle vehicle in vehicles)
                {
                    vehicle.Risk = ModelAssembler.CreateRisk(risks.First(x => x.RiskId == vehicle.Risk.RiskId));
                    vehicle.Risk.Number = entityEndorsementRisks.First(x => x.RiskId == vehicle.Risk.RiskId).RiskNum;
                    vehicle.Make.Description = vehicleMakes.FirstOrDefault(x => x.VehicleMakeCode == vehicle.Make.Id)?.SmallDescription;
                    vehicle.Model.Description = vehicleModel.FirstOrDefault(x => x.VehicleModelCode == vehicle.Model.Id)?.SmallDescription;
                    vehicle.Color.Description = vehicleColors.FirstOrDefault(x => x.VehicleColorCode == vehicle.Color.Id)?.SmallDescription;
                    vehicle.Risk.Policy.Endorsement.Id = endorsementRisks.FirstOrDefault(X => X.RiskId == vehicle.Risk.Id).EndorsementId;
                    vehicle.Risk.Policy.Id = endorsementRisks.FirstOrDefault(X => X.RiskId == vehicle.Risk.Id).PolicyId;
                    vehicle.Risk.Policy.DocumentNumber = policies.FirstOrDefault(X => X.PolicyId == vehicle.Risk.Policy.Id).DocumentNumber;
                }
            }

            return vehicles;
        }


        public List<Vehicle> GetRisksVehicleByLicensePlate(string licensePlate)
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name);
            filter.Like();
            filter.Constant(licensePlate + "%");

            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);

            ClaimRiskVehicleView view = new ClaimRiskVehicleView();
            ViewBuilder builder = new ViewBuilder("ClaimRiskVehicleView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                vehicles = ModelAssembler.CreateVehiclesByRiskVehicle(view.RiskVehicles);

                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.Policy> policies = view.Policies.Cast<ISSEN.Policy>().ToList();
                List<COMMEN.VehicleColor> vehicleColors = view.VehicleColors.Cast<COMMEN.VehicleColor>().ToList();
                List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
                List<COMMEN.VehicleModel> vehicleModel = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                foreach (Vehicle vehicle in vehicles)
                {
                    vehicle.Risk = ModelAssembler.CreateRisk(risks.First(x => x.RiskId == vehicle.Risk.RiskId));
                    vehicle.Risk.Number = entityEndorsementRisks.First(x => x.RiskId == vehicle.Risk.RiskId).RiskNum;
                    vehicle.Make.Description = vehicleMakes.FirstOrDefault(x => x.VehicleMakeCode == vehicle.Make.Id)?.SmallDescription;
                    vehicle.Model.Description = vehicleModel.FirstOrDefault(x => x.VehicleModelCode == vehicle.Model.Id)?.SmallDescription;
                    vehicle.Color.Description = vehicleColors.FirstOrDefault(x => x.VehicleColorCode == vehicle.Color.Id)?.SmallDescription;
                    vehicle.Risk.Policy.Endorsement.Id = endorsementRisks.FirstOrDefault(X => X.RiskId == vehicle.Risk.Id).EndorsementId;
                    vehicle.Risk.Policy.Id = endorsementRisks.FirstOrDefault(X => X.RiskId == vehicle.Risk.Id).PolicyId;
                    vehicle.Risk.Policy.DocumentNumber = policies.FirstOrDefault(X => X.PolicyId == vehicle.Risk.Policy.Id).DocumentNumber;
                }
            }

            return vehicles;
        }

        public Vehicle GetRiskVehicleByRiskId(int riskId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.RiskVehicle.Properties.RiskId, typeof(ISSEN.RiskVehicle).Name);
            filter.Equal();
            filter.Constant(riskId);

            ClaimRiskVehicleView claimRiskVehicleView = new ClaimRiskVehicleView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskVehicleView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimRiskVehicleView);

            if (claimRiskVehicleView.RiskVehicles.Count > 0)
            {
                ISSEN.RiskVehicle entityRiskVehicle = claimRiskVehicleView.RiskVehicles.Cast<ISSEN.RiskVehicle>().First();
                ISSEN.Risk entityRisk = claimRiskVehicleView.Risks.Cast<ISSEN.Risk>().First(x => x.RiskId == entityRiskVehicle.RiskId);
                ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskVehicleView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == x.RiskId);
                ISSEN.Policy entityPolicy = claimRiskVehicleView.Policies.Cast<ISSEN.Policy>().First(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                return ModelAssembler.CreateClaimVehicle(entityRiskVehicle, entityRisk, entityEndorsementRisk, entityPolicy);
            }

            return null;
        }

        public List<Vehicle> GetSelectRisksVehicleByLicensePlate(string description)
        {
            try
            {
                List<Vehicle> vehicles = new List<Vehicle>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name);
                filter.Like();
                filter.Constant(description + "%");

                PolicyVehicleRiskSummaryView policyVehicleSummaryView = new PolicyVehicleRiskSummaryView();
                ViewBuilder viewBuilder = new ViewBuilder("PolicyVehicleRiskSummaryView");
                viewBuilder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, policyVehicleSummaryView);

                if (policyVehicleSummaryView.RiskVehicle.Count > 0)
                {
                    List<ISSEN.Risk> risks = policyVehicleSummaryView.Risks.Cast<ISSEN.Risk>().ToList();
                    List<ISSEN.EndorsementRisk> endorsementRisk = policyVehicleSummaryView.EndorsementRisk.Cast<ISSEN.EndorsementRisk>().ToList();

                    vehicles = ModelAssembler.CreateVehiclesByRiskVehicle(policyVehicleSummaryView.RiskVehicle);

                    foreach (Vehicle vehicle in vehicles)
                    {
                        vehicle.Risk.Policy.Endorsement.Id = endorsementRisk.FirstOrDefault(x => x.RiskId == vehicle.Risk.Id).EndorsementId;
                        vehicle.Risk.Number = endorsementRisk.FirstOrDefault(x => x.RiskId == vehicle.Risk.Id).RiskNum;
                        vehicle.Risk.MainInsured.InsuredId = risks.FirstOrDefault(X => X.RiskId == vehicle.Risk.Id).InsuredId;
                        vehicle.Risk.CoveredRiskType = (CommonService.Enums.CoveredRiskType)risks.FirstOrDefault(x => x.RiskId == vehicle.Risk.Id).CoveredRiskTypeCode;
                    }
                }

                return vehicles;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
