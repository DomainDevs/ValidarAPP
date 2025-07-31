using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Model
{
    public class ProviderPaymentConcept
    {
        public int Id { get; set; }
        public int PaymentConceptId { get; set; }
        public string Description { get; set; }
    }
}
