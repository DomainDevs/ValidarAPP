using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CollectionFormBusinessService.Models
{
    [DataContract]
    public class ReportPayer : ReportIndividual
    {
        [DataMember]
        public ReportPaymentSchedule PaymentSchedule { get; set; }

        /// <summary>
        /// PDValue:  valor de primas en deposito generadas
        /// </summary>
        [DataMember]
        public ReportAmount PDValue { get; set; }
    }
}
