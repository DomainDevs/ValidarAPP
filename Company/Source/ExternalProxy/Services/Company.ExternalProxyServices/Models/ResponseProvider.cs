using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponseProvider
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int ProviderTypeId { get; set; }
        [DataMember]
        public int OriginTypeId { get; set; }
        [DataMember]
        public int? ProviderDeclinedTypeId { get; set; }
        [DataMember]
        public DateTime CreationDate { get; set; }
        [DataMember]
        public DateTime? ModificationDate { get; set; }
        [DataMember]
        public DateTime? DeclinationDate { get; set; }
        [DataMember]
        public string Observation { get; set; }
        [DataMember]
        public int SpecialityDefault { get; set; }
        [DataMember]
        public int SupplierProfileId { get; set; }
        [DataMember]
        public List<ResponseProviderPaymentConcept> providerPaymentConcepts { get; set; }
    }
}
