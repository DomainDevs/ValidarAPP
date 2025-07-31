using Sistran.Core.Application.Sureties;
using Sistran.Core.Application.SuretiesEEProvider;
using System.ServiceModel;
namespace Sistran.Company.Application.Sureties.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CiaSuretiesEEProvider : SuretiesEEProvider, ISuretiesCore
    {

    }
}