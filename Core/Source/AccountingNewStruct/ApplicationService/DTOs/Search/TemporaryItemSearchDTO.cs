using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;




namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class TemporaryItemSearchDTO 
    {
        [DataMember]
        public int TemporaryNumber { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public string BranchName { get; set; }
        [DataMember]
        public int ImputationTypeCode { get; set; }
        [DataMember]
        public string ImputationTypeName { get; set; }
        [DataMember]
        public int UserCode { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int SourceCode { get; set; }
        [DataMember]
        public DateTime RegisterDate { get; set; }

        [DataMember]
        public int Rows { get; set; } 
    }
}
