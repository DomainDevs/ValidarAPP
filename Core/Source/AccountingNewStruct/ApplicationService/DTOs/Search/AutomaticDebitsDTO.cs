using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.DTOs.AutomaticDebits;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{

    [DataContract]
    public class AutomaticDebitsDTO
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public BankNetworkDTO BankNetwork { get; set; }

        [DataMember]
        public BranchDTO Branch { get; set; }

        [DataMember]
        public string ProcessDate { get; set; }

        [DataMember]
        public int StatusId { get; set; }

        [DataMember]
        public string StatusDescription { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int Rows { get; set; }
    }

}
