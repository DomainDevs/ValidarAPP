using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftBusinessServices.Models
{
    public class CompanySarlaftEvent
    {
        [DataMember]
        public int EventId { get; set; }

        [DataMember]
        public bool RejectInd { get; set; }

        [DataMember]
        public bool AuthorizedInd { get; set; }

        [DataMember]
        public string Operation1Id { get; set; }

        [DataMember]
        public int GroupEventId { get; set; }

        [DataMember]
        public string DescriptionErrorMessage { get; set; }
    }
}
