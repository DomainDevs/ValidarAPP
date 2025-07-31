using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventDelegation
    {
    }


    [DataContract]
    public class EventDelegationSP
    {
        /// <summary>
        /// Atributo para la propiedad DelegationId.
        /// </summary>
        [DataMember]
        public int DelegationId { set; get; }
        /// <summary>
        /// Atributo para la propiedad HierarchyId.
        /// </summary>
        [DataMember]
        public int HierarchyId { set; get; }
        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string Description { set; get; }
    }
}
