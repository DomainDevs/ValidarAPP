using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.Vehicles.VehicleApplicationService.DTOs;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using COMM = Sistran.Core.Application.Common.Entities;

namespace Sistran.Company.Application.Vehicles.VehicleApplicationService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        //Core.Application.Vehicles.Models.Version
        //VersionExtendedModel
        #region MyRegion
        //internal static VersionExtendedModel CreateVehicleVersonModel(VehicleVersionDTO vehicleVersionDTO)
        //{
        //    Core.Application.Vehicles.Models.Version versionModel = new Core.Application.Vehicles.Models.Version
        //    {
        //        Id = vehicleVersionDTO.Id,
        //        Description = vehicleVersionDTO.Description,
        //        Model = new Core.Application.Vehicles.Models.Model
        //        {
        //            Id = vehicleVersionDTO.VehicleModelServiceQueryModel
        //        },
        //        Make = new Core.Application.Vehicles.Models.Make
        //        {
        //            Id = vehicleVersionDTO.VehicleMakeServiceQueryModel
        //        },
        //        Type = new Core.Application.Vehicles.Models.Type
        //        {
        //            Id = vehicleVersionDTO.VehicleTypeServiceQueryModel
        //        },

        //        DoorQuantity = vehicleVersionDTO.DoorQuantity,
        //        Engine = new Core.Application.Vehicles.Models.Engine
        //        {
        //            EngineCc = vehicleVersionDTO.EngineQuantity,

        //            Horsepower = vehicleVersionDTO.HorsePower,
        //            TopSpeed = vehicleVersionDTO.MaxSpeedQuantity
        //        },

        //        TonsQuantity = vehicleVersionDTO.TonsQuantity,
        //        TransmissionType = new Core.Application.Vehicles.Models.TransmissionType { Id = vehicleVersionDTO.VehicleTransmissionTypeServiceQueryModel ?? 0 },
        //        Weight = vehicleVersionDTO.Weight,
        //        PassengerQuantity = vehicleVersionDTO.PassengerQuantity ?? 0,
        //        Currency = new Currency { Id = vehicleVersionDTO.Currency ?? 0 },
        //        LastModel = vehicleVersionDTO.LastModel,
        //        IsImported = vehicleVersionDTO.IsImported,
        //        NewVehiclePrice = vehicleVersionDTO.Price,

        //        Body = new Body
        //        {
        //            Id = vehicleVersionDTO.VehicleBodyServiceQueryModel
        //        }

        //    };

        //    if (vehicleVersionDTO.VehicleFuelServiceQueryModel.HasValue)
        //    {
        //        versionModel.Fuel = new Core.Application.Vehicles.Models.Fuel { Id = vehicleVersionDTO.VehicleFuelServiceQueryModel.Value };
        //    }

        //    return versionModel;
        //}
        #endregion

        internal static Models.CompanyVersion CreateVehicleVersonModel(VehicleVersionDTO vehicleVersionDTO)
        {
            Models.CompanyVersion versionModel = new Models.CompanyVersion
            {
                Id = vehicleVersionDTO.Id,
                Description = vehicleVersionDTO.Description,
                Model = new Models.CompanyModel
                {
                    Id = vehicleVersionDTO.VehicleModelServiceQueryModel
                },
                
                Make = new Models.CompanyMake
                {
                    Id = vehicleVersionDTO.VehicleMakeServiceQueryModel
                },
           
                
                Type = new Models.CompanyType
                {
                    Id = vehicleVersionDTO.VehicleTypeServiceQueryModel
                },
                

                DoorQuantity = vehicleVersionDTO.DoorQuantity,
                Engine = new Models.CompanyEngine
                {
                    EngineCc = vehicleVersionDTO.EngineQuantity,
                    Horsepower = vehicleVersionDTO.HorsePower,
                    TopSpeed = vehicleVersionDTO.MaxSpeedQuantity
                },
                

                TonsQuantity = vehicleVersionDTO.TonsQuantity,
                TransmissionType = new Models.CompanyTransmissionType
                {
                    Id = vehicleVersionDTO.VehicleTransmissionTypeServiceQueryModel ?? 0
                },
                
                Weight = vehicleVersionDTO.Weight,
                PassengerQuantity = vehicleVersionDTO.PassengerQuantity ,
                Currency = new Currency { Id = vehicleVersionDTO.Currency ?? 0 },
                LastModel = vehicleVersionDTO.LastModel,
                IsImported = vehicleVersionDTO.IsImported,
                NewVehiclePrice = vehicleVersionDTO.Price,
                Body = new Models.CompanyBody
                {
                    Id = vehicleVersionDTO.VehicleBodyServiceQueryModel
                },
                
               IsElectronicPolicy = vehicleVersionDTO.IsElectronicPolicy 
            };

            if (vehicleVersionDTO.VehicleFuelServiceQueryModel.HasValue)
            {
                versionModel.Fuel = new Models.CompanyFuel
                {
                    Id = vehicleVersionDTO.VehicleFuelServiceQueryModel.Value
                };
                
            }
            

            return versionModel;
        }
    }
}
