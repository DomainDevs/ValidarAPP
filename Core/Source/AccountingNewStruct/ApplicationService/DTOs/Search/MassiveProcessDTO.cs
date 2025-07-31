using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class MassiveProcessDTO 
    {
        [DataMember]
        public int ProcessId { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime BeginDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int StateId { get; set; }
        [DataMember]
        public string StateDescription { get; set; }
        [DataMember]
        public string ErrorDescription { get; set; }
        [DataMember]
        public int SuccessfulRecords { get; set; }
        [DataMember]
        public int FailedRecords { get; set; }
        [DataMember]
        public int TotalRecords { get; set; }
        [DataMember]
        public decimal PorcentageAdvance { get; set; }
        [DataMember]
        public int Rows { get; set; }
    }
}
