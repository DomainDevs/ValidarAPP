using System.Runtime.Serialization;

namespace Sistran.Core.Application.WrapperAccountingService.Exception
{
    [DataContract]
    public class AccountingException
    {
        [DataMember]
        public string Reason { get; set; }
    }
}
