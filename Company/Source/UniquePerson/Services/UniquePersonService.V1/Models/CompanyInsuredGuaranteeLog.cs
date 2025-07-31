using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyInsuredGuaranteeLog
    {

        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int GuaranteeId { get; set; }
        [DataMember]
        public int GuaranteeStatusCode { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public DateTime LogDate { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string UserName { get; set; }

    }
}
