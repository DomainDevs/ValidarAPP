using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePerson.IntegrationService.Models
{
    [DataContract]
    public class ReinsurerDTO
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int ReinsuredCD { get; set; }
        [DataMember]
        public DateTime EnteredDate { get; set; }
        [DataMember]
        public DateTime? ModifyDate { get; set; }
        [DataMember]
        public DateTime? DeclinedDate { get; set; }
        [DataMember]
        public int? DeclaredTypeCD { get; set; }
        [DataMember]
        public string Annotations { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
    }
}
