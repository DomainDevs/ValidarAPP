using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyJustificationSarlaft
    {
        /// <summary>
        /// identificador
        /// </summary>
        [DataMember]
        public int JustificationReasonCode { get; set; }

        /// <summary>
        /// descripción 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// SmallDescription 
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Enabled 
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }
    }
}
