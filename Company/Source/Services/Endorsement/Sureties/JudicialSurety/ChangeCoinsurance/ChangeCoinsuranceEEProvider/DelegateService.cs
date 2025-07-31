using Sistran.Company.Application.ChangeCoInsuranceEndorsement;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.JudicialSuretyCancellationService;
using Sistran.Company.Application.JudicialSuretyModificationService;
using Sistran.Company.Application.Sureties.JudicialSuretyServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.JudicialSuretyChangeCoinsuranceService.EEProvider
{
    public static class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IJudicialSuretyService judicialsuretyService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyService>();
        internal static IJudicialSuretyModificationServiceCompany judicialsuretyModificationService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyModificationServiceCompany>();
        internal static ICiaChangeCoinsuranceEndorsement ciaChangeCoinsuranceEndorsement = ServiceProvider.Instance.getServiceManager().GetService<ICiaChangeCoinsuranceEndorsement>();
        internal static IJudicialSuretyCancellationServiceCompany endorsementSuretyCancellationService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyCancellationServiceCompany>();

    }
}
