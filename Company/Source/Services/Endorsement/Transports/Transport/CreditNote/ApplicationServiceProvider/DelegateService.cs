using Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices;
using Sistran.Company.Application.Transports.TransportApplicationService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.Base.Endorsement.CreditNoteBusinessService;

namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.EEProvider
{
    public class DelegateService
    {
       
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICompanyTransportApplicationService transportsService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyTransportApplicationService>();
        internal static ICompanyBaseCreditNoteBusinessService creditNoteServiceBase = ServiceProvider.Instance.getServiceManager().GetService<ICompanyBaseCreditNoteBusinessService>();
        internal static ICompanyCreditNoteBusinessService creditNoteService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyCreditNoteBusinessService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
    }
}
