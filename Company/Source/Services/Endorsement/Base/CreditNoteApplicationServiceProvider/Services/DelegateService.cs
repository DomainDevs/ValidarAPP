using Sistran.Company.Application.Base.Endorsement.CreditNoteBusinessService;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.Base.Endorsement.CreditNoteApplicationServices.EEProvider
{
    public class DelegateService
    {
        internal static ICompanyBaseCreditNoteBusinessService baseCreditNoteBusinessService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyBaseCreditNoteBusinessService>();
    }
}
