using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    public class ResponseDocumentPayments
    {
        public List<PaymentDocument> Payments { get; set; }
        public bool Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
