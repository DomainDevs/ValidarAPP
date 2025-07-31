using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class PaymentBallotAccountingDTO
    {
        [DataMember]
        public PaymentBallotParametersDTO PaymentBallotParameters { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// TypeId
        /// </summary>
        [DataMember]
        public int TypeId { get; set; }

        /// <summary>
        /// AccountingDate
        /// </summary>
        [DataMember]
        public DateTime AccountingDate { get; set; }
    }
}
