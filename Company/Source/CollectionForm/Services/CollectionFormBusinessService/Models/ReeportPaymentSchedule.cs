using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CollectionFormBusinessService.Models
{
    [DataContract]
    public class ReportPaymentSchedule
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int InstallmentsCount { get; set; }

        [DataMember]
        public List<ReportInstallment> Installments { get; set; }
    }
}
