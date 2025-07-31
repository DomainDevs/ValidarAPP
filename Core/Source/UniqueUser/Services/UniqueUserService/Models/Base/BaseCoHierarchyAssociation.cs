using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniqueUserServices.Models.Base
{
    [DataContract]
    public class BaseCoHierarchyAssociation : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// ExclusionayInd
        /// </summary>
        [DataMember]
        public bool ExclusionayInd { get; set; }

        /// <summary>
        /// EnabledInd
        /// </summary>
        [DataMember]
        public bool EnabledInd { get; set; }

        /// <summary>
        /// LimitInsuredAmt
        /// </summary>
        [DataMember]
        public decimal? LimitInsuredAmt { get; set; }
    }
}
