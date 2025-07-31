using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    [DataContract]
    public class PriorityRetentionDetailDTO
    {
        [DataMember]
        public int Id { get; set; }
		/// <summary>
		/// Atributo para la propiedad PriorityRetentionId.
		/// </summary>
		[DataMember]
        public int PriorityRetentionId { get; set; }
        /// <summary>
		/// Atributo para la propiedad PolicyId.
		/// </summary>
		[DataMember] 
        public int PolicyId;
        /// <summary>
        /// Atributo para la propiedad EndorsementId.
        /// </summary>
        [DataMember] 
        public int EndorsementId;
        /// <summary>
        /// Atributo para la propiedad IssueDate.
        /// </summary>
        [DataMember] 
        public DateTime IssueDate;
        /// <summary>
        /// Atributo para la propiedad IndividualId.
        /// </summary>
        [DataMember] 
        public int IndividualId;
        /// <summary>
        /// Atributo para la propiedad LineBusinessId.
        /// </summary>
        [DataMember]
        public int PrefixCd;
        /// <summary>
        /// Atributo para la propiedad PriorityRetentionAmount.
        /// </summary>
        [DataMember]
        public decimal PriorityRetentionAmount;
        /// <summary>
        /// Atributo para la propiedad CumulusAmount.
        /// </summary>
        [DataMember]
        public decimal RetentionCumulus;
        /// <summary>
        /// Atributo para la propiedad CurrentPriorityRetentionAmount.
        /// </summary>
        [DataMember]
        public decimal CurrentPriorityRetentionAmount;
        /// <summary>
        /// Atributo para la propiedad ProcessDate.
        /// </summary>
        [DataMember] 
        public DateTime ProcessDate;
    }
}
