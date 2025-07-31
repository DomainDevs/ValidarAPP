using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class PaymentBallotResponsesDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string BallotId { get; set; }
        [DataMember]
        public decimal Total { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public string User  { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public bool ShowMessage  { get; set; }
        [DataMember]
        public int TechnicalTransaction { get; set; }
    }
}
