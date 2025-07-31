using System.Runtime.Serialization;


namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyPolicyUser
    {
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// user profile
        /// </summary>
        [DataMember]
        public int UserProfileId { get; set; }
    }
}
