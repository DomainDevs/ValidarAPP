using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CollectionFormBusinessService.Models
{
    [DataContract]
    public class PolicyQuote
    {
        [DataMember]
        public int QuoteNumber { get; set; }

        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public Double TotalValue { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public Double QuoteValue { get; set; }

        [DataMember]
        public int IndividualPayerId { get; set; }

        [DataMember]
        public string PayerName { get; set; }
    }
}
