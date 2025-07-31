using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class SearchClaimDTO
    {
        [DataMember]
        public int? BranchId { get; set; }

        [DataMember]
        public DateTime? ClaimDateFrom { get; set; }

        [DataMember]
        public DateTime? ClaimDateTo { get; set; }

        [DataMember]
        public int? ClaimNumber { get; set; }

        [DataMember]
        public DateTime? NoticeDateFrom { get; set; }

        [DataMember]
        public DateTime? NoticeDateTo { get; set; }

        [DataMember]
        public int? PrefixId { get; set; }

        [DataMember]
        public int? TemporaryNumber { get; set; }

        [DataMember]
        public int? UserId { get; set; }

        [DataMember]
        public int? IndividualId { get; set; }

        [DataMember]
        public int? HolderId { get; set; }

        [DataMember]
        public String DocumentNumber { get; set; }

        [DataMember]
        public bool IsMinimumSalary { get; set; }

        [DataMember]
        public decimal CurrentMinimumSalaryValue { get; set; }

    }
}
