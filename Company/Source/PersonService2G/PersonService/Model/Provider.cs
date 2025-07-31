using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Model
{
    public class Provider
    {
        public int Id { get; set; }
        public int IndividualId { get; set; }
        public int ProviderTypeId { get; set; }
        public int OriginTypeId { get; set; }
        public int? ProviderDeclinedTypeId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public DateTime? DeclinationDate { get; set; }
        public string Observation { get; set; }
        public int SpecialityDefault { get; set; }
        public int SupplierProfileId { get; set; }
        public List<ProviderPaymentConcept> providerPaymentConcepts { get; set; }
    }
}
