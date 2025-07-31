using System.Runtime.Serialization;

namespace Sistran.Core.Application.WrapperAccountingService.DTOs
{
    /// <summary>
    /// Comprobante de pago
    /// </summary>
    [DataContract]
    public class VoucherRequest
    {
        [DataMember]
        public int DocumentType { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public int PaymentMethodId { get; set; }
        [DataMember]
        public ConsignmentCashRequest ConsignmentCashRequest { get; set; }        

    }
}
