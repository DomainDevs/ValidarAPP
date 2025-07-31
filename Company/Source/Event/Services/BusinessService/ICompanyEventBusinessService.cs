using Sistran.Company.Application.Event.BusinessService.Models;
using Sistran.Core.Application.EventsServices;
using Sistran.Core.Application.EventsServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Event.BusinessService
{
    [ServiceContract]
    public interface ICompanyEventBusinessService : IEventServiceCore
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CompanyModule> GetCompanyModules();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CompanySubModule> GetCompanySubModules();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CompanyEventGroup> GetCompanyEventsGroups();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanySubModule> GetCompanySubModulesByModuleId(int moduleId);
    }
}