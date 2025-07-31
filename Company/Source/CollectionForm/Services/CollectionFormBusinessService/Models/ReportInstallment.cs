using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CollectionFormBusinessService.Models
{
    [DataContract]
    public class ReportInstallment
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int InstallmentNumber { get; set; }

        [DataMember]
        public ReportAmount Amount { get; set; }

        [DataMember]
        public DateTime DueDate { get; set; }

        [DataMember]
        public ReportAmount PaidAmount { get; set; }

        [DataMember]
        public DateTime PaidDate { get; set; }
        
        [DataMember]
        public bool IsPaid { get { return Amount == PaidAmount; } set { } }

        [DataMember]
        public bool IsPartialPaid { get; set; }
    }
}
