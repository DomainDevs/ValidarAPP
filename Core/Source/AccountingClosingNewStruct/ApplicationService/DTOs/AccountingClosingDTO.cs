using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingClosingServices.DTOs
{
    [DataContract]
    public class AccountingClosingDTO 
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ModuleId { get; set; }

        [DataMember]
        public DateTime? StartDate { get; set; }

        [DataMember]
        public DateTime? EnDate { get; set; }

        [DataMember]
        public bool isProgress { get; set; }
    }
}
