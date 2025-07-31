using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    public class ResponsePolicyPayment
    {
        public int PaymentDays { get; set; }
        public decimal PaymentValue { get; set; }
        public bool Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
