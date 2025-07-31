using System;
using System.Collections.Generic;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.Vehicles.EEProvider.Assemblers
{
    public class EntityAssembler
    {
        internal static VehicleVersion CreateVehicleVersion(Models.Version vehicleVersion)
        {
            return new VehicleVersion(vehicleVersion.Id, vehicleVersion.Model.Id, vehicleVersion.Make.Id)
            {
                Description = vehicleVersion.Description,
                EngineCc = vehicleVersion.Engine.EngineCc,
                Horsepower = vehicleVersion.Engine.Horsepower,
                Weight = vehicleVersion.Weight,
                TonsQuantity = vehicleVersion.TonsQuantity,
                PassengerQuantity = vehicleVersion.PassengerQuantity,
                VehicleFuelCode = vehicleVersion.Fuel.Id,
                VehicleBodyCode = vehicleVersion.Body.Id,
                VehicleTypeCode = vehicleVersion.Type.Id,
                TransmissionTypeCode = vehicleVersion.TransmissionType.Id,
                TopSpeed = vehicleVersion.Engine.TopSpeed,
                DoorQuantity = vehicleVersion.DoorQuantity,
                NewVehiclePrice = vehicleVersion.NewVehiclePrice,
                IsImported = vehicleVersion.IsImported,
                LastModel = vehicleVersion.LastModel,
                CurrencyCode = vehicleVersion.Currency.Id,
                ExtendedProperties = CreateExtendedProperties(vehicleVersion.ExtendedProperties)
            };
        }
      

        private static List<Framework.DAF.ExtendedProperty> CreateExtendedProperties(List<Extensions.ExtendedProperty> extendedProperties)
        {
            List<Framework.DAF.ExtendedProperty> entityExtendedProperties = new List<Framework.DAF.ExtendedProperty>();

            if (extendedProperties != null)
            {
                foreach (Extensions.ExtendedProperty extendedProperty in extendedProperties)
                {
                    entityExtendedProperties.Add(new Framework.DAF.ExtendedProperty
                    {
                        Name = extendedProperty.Name,
                        Value = extendedProperty.Value
                    });
                }
            }

            return entityExtendedProperties;
        }
    }
}
