using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class PaymentTicketBallotDTO
    {
        /// <summary>
        /// PaymentTicketBallotId
        /// </summary>
        [DataMember]
        public int PaymentTicketBallotId { get; set; }
    }
}
