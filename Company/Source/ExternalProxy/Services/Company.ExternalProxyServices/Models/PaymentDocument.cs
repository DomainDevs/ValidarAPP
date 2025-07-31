using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    public class PaymentDocument 
    {

        public String DocumentNumber { get; set; }
        public String DocumentType { get; set; }
        public int PaymentDays { get; set; }
        public decimal PaymentValue { get; set; }
    }
}
