using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    [DataContract]
    public class UniqueUserLoginDTO
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public DateTime? ExpirationDate { get; set; }
        [DataMember]
        public int ExpirationsDays { get; set; }
        [DataMember]
        public bool? MustChangePassword { get; set; }
    }
}
