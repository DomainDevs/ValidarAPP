using AutoMapper;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.ModificationService.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Vehicle.ModificationService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        internal static List<VehicleFilterIndividual> CreateVehicleFilterIndividuals(List<FilterIndividual> filtersIndividuals)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FilterIndividual, VehicleFilterIndividual>();

            });
            return config.CreateMapper().Map<List<FilterIndividual>, List<VehicleFilterIndividual>>(filtersIndividuals);
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
            catch (Exception)
            {
            }
            return Event;
        }
    }
}
