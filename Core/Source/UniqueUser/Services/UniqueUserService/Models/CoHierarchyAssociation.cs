using Sistran.Core.Application.UniqueUserServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniqueUserServices.Models
{
    [DataContract]
    public class CoHierarchyAssociation : BaseCoHierarchyAssociation
    {
        /// <summary>
        /// Module
        /// </summary>
        [DataMember]
        public Module Module { get; set; }

        /// <summary>
        /// SubModule
        /// </summary>
        [DataMember]
        public SubModule SubModule { get; set; }
    }
}
