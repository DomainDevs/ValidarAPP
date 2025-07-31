using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    [DataContract]
    public class PrefixDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public string TinyDescription { get; set; }

        [DataMember]
        public bool HasDetailedCommission { get; set; }

        [DataMember]
        public int PrefixTypeCode { get; set; }

        /// <summary>
        /// Estado del modulo(modificado o eliminado)
        /// </summary>
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public int LineBusinessId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        public List<LineBusinessDTO> LineBusiness { get; set; }

        [DataMember]
        public PrefixTypeDTO PrefixType { get; set; }
    }
}
