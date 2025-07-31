using Sistran.Core.Application.WrapperAccountingService.DTOs;
using Sistran.Core.Application.WrapperAccountingService.Exception;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Sistran.Core.Application.WrapperAccountingService
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IWrapperAccountingService
    {
        [OperationContract]
        [FaultContract(typeof(AccountingException))]
        void SaveApplication(ApplicationRequest applicationRequest);

        [OperationContract]
        Task<int> TransactionNumber(int branchId);
    
        [OperationContract]
        BillReportDTO GetBillReportInformation(int technicalTransaction);
    }
}