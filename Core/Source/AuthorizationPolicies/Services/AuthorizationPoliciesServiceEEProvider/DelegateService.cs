using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.RulesScriptsServices;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Application.UniqueUserServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider
{
    public static class DelegateService
    {
        internal static readonly ICommonServiceCore CommonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static readonly IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>(); 
        internal static readonly IUniquePersonServiceCore UniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
        internal static readonly IUniqueUserServiceCore UniqueUserService = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserServiceCore>();
        internal static readonly IRulesService RuleService = ServiceProvider.Instance.getServiceManager().GetService<IRulesService>();
        internal static readonly IConceptsService ConceptService = ServiceProvider.Instance.getServiceManager().GetService<IConceptsService>();
    }
}
