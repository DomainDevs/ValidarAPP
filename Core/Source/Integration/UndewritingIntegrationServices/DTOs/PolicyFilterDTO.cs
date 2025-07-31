
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class PolicyFilterDTO
    {
        [DataMember]
        public decimal DocumentNumber { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public int PrefixId { get; set; }
    }
}