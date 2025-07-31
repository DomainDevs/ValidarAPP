using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponseProviderPaymentConcept
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int PaymentConceptId { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
