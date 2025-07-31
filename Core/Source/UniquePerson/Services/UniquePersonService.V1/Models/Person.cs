using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    [DataContract]
    public class Person : BasePerson
    {
        /// <summary>
        /// role
        /// </summary>
        [DataMember]
        public Role Role { get; set; }

        /// <summary>
        /// EconomicActivity
        /// </summary>
        [DataMember]
        public EconomicActivity EconomicActivity { get; set; }

        /// <summary>
        /// IdentificationDocument
        /// </summary>
        [DataMember]
        public IdentificationDocument IdentificationDocument { get; set; }

        /// <summary>
        /// MaritalStatus
        /// </summary>
        [DataMember]
        public MaritalStatus MaritalStatus { get; set; }

        /// <summary>
        /// EducationLevel
        /// </summary>
        [DataMember]
        public EducativeLevel EducativeLevel { get; set; }

        /// <summary>
        /// SocialLayer
        /// </summary>
        [DataMember]
        public SocialLayer SocialLayer { get; set; }

        /// <summary>
        /// HouseType
        /// </summary>
        [DataMember]
        public HouseType HouseType { get; set; }

        /// <summary>
        /// PersonType
        /// </summary>
        [DataMember]
        public PersonType PersonType { get; set; }

        /// <summary>
        /// Checkpayable
        /// </summary>
        [DataMember]
        public string CheckPayable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool? DataProtection { get; set; }

        [DataMember]
        public Insured Insured { get; set; }

    }
}
