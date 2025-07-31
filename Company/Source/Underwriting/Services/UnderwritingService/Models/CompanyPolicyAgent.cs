using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyPolicyAgent
    {
        /// <summary>
        /// <summary>
        /// IndividualId
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }
        /// <summary>
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// <summary>
        /// FullName
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// <summary>
        /// AgentId
        /// </summary>
        [DataMember]
        public int AgentId { get; set; }
    }
}
