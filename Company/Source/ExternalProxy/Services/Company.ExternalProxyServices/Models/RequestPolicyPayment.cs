using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    public class RequestPolicyPayment
    {
        public string Branch { get; set; }
        public string Prefix { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime PaymentDate { get; set; }

    }
}
