using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CollectionFormBusinessService.Models
{
    [DataContract]
    public class ReportAmount
    {
        public ReportAmount(decimal value)
        {
            this.Value = value;
        }

        public ReportAmount()
        {
        }

        [DataMember]
        public decimal Value { get; set; }
    }
}
