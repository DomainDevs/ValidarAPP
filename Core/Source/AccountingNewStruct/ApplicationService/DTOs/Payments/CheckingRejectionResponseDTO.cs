using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.DTOs.Payments
{
    /// <summary>
    ///     Pago.
    /// </summary>
    [DataContract]
    public class CheckingRejectionResponseDTO
    {

        /// <summary>
        /// Message
        /// </summary>
        [DataMember]
        public string Message { get; set; }


        /// <summary>
        ///BillId
        /// </summary>
        [DataMember]
        public string BillId { get; set; }

        /// <summary>
        ///ShowMessage
        /// </summary>
        [DataMember]
        public string ShowMessage { get; set; }

        /// <summary>
        ///TechnicalTransaction
        /// </summary>
        [DataMember]
        public string TechnicalTransaction { get; set; }
         
    }
}
