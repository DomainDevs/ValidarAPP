using System.Collections.Generic;
using Sistran.Company.Application.Event.BusinessService.EEProvider.Assemblers;
using Sistran.Company.Application.Event.BusinessService.Models;
using Sistran.Core.Application.EventsServices.EEProvider;
using Sistran.Core.Application.EventsServices.Models;

namespace Sistran.Company.Application.Event.BusinessService.EEProvider
{
    public class CompanyEventBusinessServiceProvider : EventsServiceEEProvider, ICompanyEventBusinessService 
    {
        public List<CompanyEventGroup> GetCompanyEventsGroups()
        {
            return ModelAssembler.CreateEventGroups(GetEventsGroups());
        }

        public List<CompanyModule> GetCompanyModules()
        {
            return ModelAssembler.CreateModules(DelegateService._AuthorizationPolicies.GetModuleServiceModel());
        }

        public List<CompanySubModule> GetCompanySubModules()
        {
            return ModelAssembler.CreateSubModules(DelegateService._AuthorizationPolicies.GetSubModuleServiceModel());
        }

        public List<CompanySubModule> GetCompanySubModulesByModuleId(int moduleId)
        {
            return ModelAssembler.CreateSubModules(DelegateService._AuthorizationPolicies.GetSubModuleForItemIdModuleServiceModel(moduleId));
        }
    }
}