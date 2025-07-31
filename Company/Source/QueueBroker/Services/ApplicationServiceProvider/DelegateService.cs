using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.UniquePersonListRiskBusinessService;
using Sistran.Company.Application.MassiveVehicleParamBusinessService;

namespace Sistran.Company.Application.QueueBrokerServiceEEProvider
{
    public class DelegateService
    {
        //internal static ICompanyMassiveVehicleParamBusinessService paraMassiveUnderwriting = ServiceProvider.Instance.getServiceManager().GetService<ICompanyMassiveVehicleParamBusinessService>();
        internal static IUniquePersonListRiskBusinessService _UniquePersonListRiskBusinessService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonListRiskBusinessService>();
    }
}
