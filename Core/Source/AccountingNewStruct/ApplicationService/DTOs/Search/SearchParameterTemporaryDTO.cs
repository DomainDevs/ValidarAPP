using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System.Runtime.Serialization;




namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class SearchParameterTemporaryDTO
    {
        [DataMember]
        public BranchDTO Branch { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public ApplicationTypeDTO ImputationType { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public int TemporaryNumber { get; set; }

    }
}
