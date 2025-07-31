using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    public class RequestDocumentPayments
    {
        public List<Document> Documents { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
