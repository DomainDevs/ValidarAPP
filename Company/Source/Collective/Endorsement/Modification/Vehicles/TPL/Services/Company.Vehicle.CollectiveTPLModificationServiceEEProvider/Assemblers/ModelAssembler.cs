using AutoMapper;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.CollectiveTPLModificationService.Models;
using Sistran.Company.Application.Vehicles.Models;
using VECO = Sistran.Core.Application.Vehicles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;

namespace Sistran.Company.Application.Vehicles.CollectiveTPLModificationServiceEEProvider.Assemblers
{
    public class ModelAssembler
    {
        internal static List<ThirdPartyLiabilityFilterIndividual> CreateVehicleFilterIndividuals(List<FilterIndividual> filtersIndividuals)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FilterIndividual, ThirdPartyLiabilityFilterIndividual>();

            });
            return config.CreateMapper().Map<List<FilterIndividual>, List<ThirdPartyLiabilityFilterIndividual>>(filtersIndividuals);
        }

        internal static EventAuthorization CreateCompanyEventAuthorizationEmision(CompanyPolicy companyPolicy, int userId)
        {
            EventAuthorization Event = new EventAuthorization();
            try
            {
                Event.OPERATION1_ID = companyPolicy.Endorsement.TicketNumber.ToString();
                Event.OPERATION2_ID = companyPolicy.Endorsement.Id.ToString();
                Event.AUTHO_USER_ID = userId;
                Event.EVENT_ID = (int)UnderwritingServices.Enums.EventTypes.SubscriptionMassive;
            }
            catch (Exception ex)
            {
            }
            return Event;
        }

        public static CompanyVersion CreateCompanyVersion(VECO.Version version)
        {
            return new CompanyVersion
            {
                AirConditioning = version.AirConditioning,
                Body = new CompanyBody { Id = version.Body.Id },
                Currency = version.Currency,
                Description = version.Description,
                DoorQuantity = version.DoorQuantity,
                Engine = new CompanyEngine { EngineCc = version.Engine.EngineCc },
                ExtendedProperties = version.ExtendedProperties,
                Fuel = new CompanyFuel { Description = version.Fuel.Description },
                IaVehicleVersion = version.IaVehicleVersion,
                Id = version.Id,
                IsImported = version.IsImported,
                LastModel = version.LastModel,
                Make = new CompanyMake { Id = version.Make.Id },
                Model = new CompanyModel { Id = version.Model.Id },
                NewVehiclePrice = version.NewVehiclePrice,
                Novelty = version.Novelty,
                PartialLossBase = version.PartialLossBase,
                PassengerQuantity = version.PassengerQuantity,
                ServiceType = new CompanyServiceType { Id = version.ServiceType.Id },
                Status = version.Status,
                TonsQuantity = version.TonsQuantity,
                TransmissionType = new CompanyTransmissionType { Id = version.TransmissionType.Id },
                Type = new Vehicles.Models.CompanyType { Id = version.Type.Id },
                VehicleAxleQuantity = version.VehicleAxleQuantity,
                Weight = version.Weight,
                WeightCategory = version.WeightCategory,

            };
        }
        internal static ComponentValueDTO CreateCompanyComponentValueDTO(CompanySummary companySummary)
        {
            var imaper = AutoMapperAssembler.CreateMapCompanyComponentValueDTO();
            return imaper.Map<CompanySummary, ComponentValueDTO>(companySummary);
        }
    }

}
