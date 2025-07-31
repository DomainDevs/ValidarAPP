using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ParametrizationParamBusinessService;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.UnderwritingServices
{
    /// <summary>
    /// Invocacion Servicios
    /// </summary>
    public class DelegateBaseService
    {

        internal static IParametrizationParamBusinessService ParametrizationParamBusinessService = ServiceProvider.Instance.getServiceManager().GetService<IParametrizationParamBusinessService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
    }
}