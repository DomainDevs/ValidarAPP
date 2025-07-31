using System.Collections.Generic;
using CommonModels = Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Vehicles.Models;
using ServiceType = Sistran.Core.Application.Common.Entities.ServiceType;
using UNDMO = Sistran.Core.Application.UnderwritingServices.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using UNDTO = Sistran.Core.Application.UnderwritingServices.DTOs;
using System;
using Sistran.Core.Application.Extensions;

namespace Sistran.Core.Application.Vehicles.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region Make

        public static Models.Make CreateMake(VehicleMake make)
        {
            return new Models.Make
            {
                Id = make.VehicleMakeCode,
                Description = make.SmallDescription
            };
        }

        public static List<Models.Make> CreateMakes(BusinessCollection businessCollection)
        {
            List<Models.Make> makes = new List<Models.Make>();

            foreach (VehicleMake field in businessCollection)
            {
                makes.Add(ModelAssembler.CreateMake(field));
            }

            return makes;
        }      

        #endregion

        #region Type

        public static Models.Type CreateType(VehicleType type)
        {
            return new Models.Type
            {
                Id = type.VehicleTypeCode,
                Description = type.Description
            };
        }

        public static List<Models.Type> CreateTypes(BusinessCollection businessCollection)
        {
            List<Models.Type> types = new List<Models.Type>();

            foreach (VehicleType field in businessCollection)
            {
                types.Add(ModelAssembler.CreateType(field));
            }

            return types;
        }

        #endregion

        #region Color

        public static Models.Color CreateColor(VehicleColor color)
        {
            return new Models.Color
            {
                Id = color.VehicleColorCode,
                Description = color.SmallDescription
            };
        }

        public static List<Models.Color> CreateColors(BusinessCollection businessCollection)
        {
            List<Models.Color> colors = new List<Models.Color>();

            foreach (VehicleColor field in businessCollection)
            {
                colors.Add(ModelAssembler.CreateColor(field));
            }

            return colors;
        }

        #endregion

        #region Body

        public static Models.Body CreateBody(VehicleBody body)
        {
            return new Models.Body
            {
                Id = body.VehicleBodyCode,
                Description = body.SmallDescription
            };
        }

        public static List<Models.Body> CreateBodies(BusinessCollection businessCollection)
        {
            List<Models.Body> bodies = new List<Models.Body>();

            foreach (VehicleBody field in businessCollection)
            {
                bodies.Add(ModelAssembler.CreateBody(field));
            }

            return bodies;
        }

        #endregion

        #region Fuel

        public static Models.Fuel CreateFuel(VehicleFuel fuel)
        {
            return new Models.Fuel
            {
                Id = fuel.VehicleFuelCode,
                Description = fuel.SmallDescription,
            };
        }

        public static List<Models.Fuel> CreateFuels(BusinessCollection businessCollection)
        {
            List<Models.Fuel> fuels = new List<Models.Fuel>();

            foreach (VehicleFuel field in businessCollection)
            {
                fuels.Add(ModelAssembler.CreateFuel(field));
            }

            return fuels;
        }

        #endregion

        #region Year

        public static Models.Year CreateYear(VehicleVersionYear year)
        {
            return new Models.Year
            {
                Currency = new CommonModels.Currency { Id = year.CurrencyCode },
                Price = year.VehiclePrice,
                Description = year.VehicleYear
            };
        }

        public static List<Models.Year> CreateYears(BusinessCollection businessCollection)
        {
            List<Models.Year> years = new List<Models.Year>();

            foreach (VehicleVersionYear field in businessCollection)
            {
                years.Add(ModelAssembler.CreateYear(field));
            }

            return years;
        }

        #endregion

        #region ServiceType
        public static Models.ServiceType CreateServiceType(ServiceType serviceType)
        {
            return new Models.ServiceType
            {
                Id = serviceType.ServiceTypeCode,
                Description = serviceType.Description,
                SmallDescription = serviceType.SmallDescription,
                Enabled = serviceType.Enabled
            };
        }

        public static List<Models.ServiceType> CreateServiceTypes(BusinessCollection businessCollection)
        {
            List<Models.ServiceType> serviceTypes = new List<Models.ServiceType>();
            foreach (ServiceType serviceType in businessCollection)
            {
                serviceTypes.Add(CreateServiceType(serviceType));
            }
            return serviceTypes;
        }
        #endregion

        #region Model

        public static Models.Model CreateModel(VehicleModel model)
        {
            return new Models.Model
            {
                Id = model.VehicleModelCode,
                Description = model.Description,
                Make = new Vehicles.Models.Make 
                { 
                    Id = model.VehicleMakeCode
                }
            };
        }

        public static List<Models.Model> CreateModels(BusinessCollection businessCollection)
        {
            List<Models.Model> Models = new List<Models.Model>();

            foreach (VehicleModel field in businessCollection)
            {
                Models.Add(ModelAssembler.CreateModel(field));
            }

            return Models;
        }

        #endregion

        #region Version

        public static Models.Version CreateVersion(VehicleVersion version)
        {
            Models.Version versionModel = new Models.Version
            {
                Id = version.VehicleVersionCode,
                Description = version.Description,
                Model = new Models.Model 
                { 
                    Id = version.VehicleModelCode 
                },
                Make = new Models.Make
                {
                    Id = version.VehicleMakeCode
                },
                Type = new Vehicles.Models.Type
                {
                    Id = version.VehicleTypeCode
                },
                AirConditioning = version.AirConditioning.GetValueOrDefault(),
                DoorQuantity = version.DoorQuantity,
                Engine = new Models.Engine
                {
                    EngineCc = version.EngineCc,
                    EngineCylQuantity = version.EngineCylQuantity,
                    EngineType = new Models.EngineType { Id = version.EngineTypeCode ?? 0 },
                    Horsepower = version.Horsepower,
                    TopSpeed = version.TopSpeed
                },
                IaVehicleVersion = version.IaVehicleVersionCode,
                Novelty = version.NoveltyCode,
                TonsQuantity = version.TonsQuantity,
                TransmissionType = new Models.TransmissionType { Id = version.TransmissionTypeCode ?? 0 },
                Weight = version.Weight,
                PartialLossBase = version.PartialLossBaseValue,
                Currency = new CommonModels.Currency { Id = version.CurrencyCode ?? 0 },
                LastModel = version.LastModel.GetValueOrDefault(),
                NewVehiclePrice = version.NewVehiclePrice,
                Status = version.CodeStatus,
                ServiceType = new Models.ServiceType
                {
                    Id = version.ServiceId ?? 0,
                    Description = version.ServiceDescription
                },
                WeightCategory = version.WeightCategoryCode,
                Body = new Body
                {
                    Id = version.VehicleBodyCode
                },
                IsImported = version.IsImported,
                PassengerQuantity = version.PassengerQuantity ?? 0,
                ExtendedProperties = CreateExtendedProperties(version.ExtendedProperties)
            };

            if (version.VehicleFuelCode.HasValue)
            {
                versionModel.Fuel = new Vehicles.Models.Fuel { Id = version.VehicleFuelCode.Value };
            }

            return versionModel;
        }

        private static List<Extensions.ExtendedProperty> CreateExtendedProperties(List<Framework.DAF.ExtendedProperty> extendedProperties)
        {
            List<Extensions.ExtendedProperty> entityExtendedProperties = new List<Extensions.ExtendedProperty>();

            if (extendedProperties != null)
            {
                foreach (Framework.DAF.ExtendedProperty extendedProperty in extendedProperties)
                {
                    entityExtendedProperties.Add(new Extensions.ExtendedProperty
                    {
                        Name = extendedProperty.Name,
                        Value = extendedProperty.Value
                    });
                }
            }

            return entityExtendedProperties;
        }

        public static List<Models.Version> CreateVersions(BusinessCollection businessCollection)
        {
            List<Models.Version> versions = new List<Models.Version>();

            foreach (VehicleVersion field in businessCollection)
            {
                versions.Add(ModelAssembler.CreateVersion(field));
            }

            return versions;
        }

        #endregion

        #region PolicyRiskDTO
        public static UNDTO.PolicyRiskDTO CreatePolicyRiskDTOs(ISSEN.Policy policy)
        {
            return new UNDTO.PolicyRiskDTO
            {
                DocumentNumber = policy.DocumentNumber,
                PrefixId = policy.PrefixCode,
                BranchId = policy.BranchCode
            };
        }
        public static List<UNDTO.PolicyRiskDTO> CreatePolicyRiskDTOs(BusinessCollection businessCollection)
        {
            List<UNDTO.PolicyRiskDTO> policyRiskDTOs = new List<UNDTO.PolicyRiskDTO>();
            foreach (ISSEN.Policy field in businessCollection)
            {
                policyRiskDTOs.Add(CreatePolicyRiskDTOs(field));
            }
            return policyRiskDTOs;
        }
        #endregion

        #region TransmissionType

        internal static List<Models.TransmissionType> CreateTransmissionTypes(BusinessCollection businessCollection)
        {
            List<Models.TransmissionType> transmissionTypes = new List<Models.TransmissionType>();

            foreach (Common.Entities.TransmissionType field in businessCollection)
            {
                transmissionTypes.Add(ModelAssembler.TransmissionType(field));
            }

            return transmissionTypes;
        }

        private static Models.TransmissionType TransmissionType(Common.Entities.TransmissionType field)
        {
            return new Models.TransmissionType
            {
                Id = field.TransmissionTypeCode,
                Description = field.SmallDescription
            };
        } 

        #endregion
    }
}
