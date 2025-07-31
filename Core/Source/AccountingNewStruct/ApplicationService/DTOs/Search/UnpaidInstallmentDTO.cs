using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class UnpaidInstallmentDTO 
    {
        public int PolicyId { get; set; }
        public int EndorsementNumber { get; set; }
        public string DocumentType { get; set; }
        public int DocumentNumber { get; set; }
        public string PayerName { get; set; }
        public int InstallmentNumber { get; set; }
        public decimal InstallmentAmount { get; set; }
        public decimal InstallmentPaidAmount { get; set; }

    }
}
