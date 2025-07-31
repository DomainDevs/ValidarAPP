using System;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class LastDateClass
    {
        [DataMember]
        public DateTime UpDate { get; set; }

        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
