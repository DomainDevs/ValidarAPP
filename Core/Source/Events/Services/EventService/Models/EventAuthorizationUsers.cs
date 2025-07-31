using System.Runtime.Serialization;


namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventAuthorizationUsers
    {
        /// <summary>
        /// Atributo para la propiedad GroupEventId.
        /// </summary>
        [DataMember]
        public int GroupEventId { set; get; }
        /// <summary>
        /// Atributo para la propiedad EventId.
        /// </summary>
        [DataMember]
        public int EventId { set; get; }
        /// <summary>
        /// Atributo para la propiedad DelegationId.
        /// </summary>
        [DataMember]
        public int DelegationId { set; get; }
        /// <summary>
        /// Atributo para la propiedad UserId.
        /// </summary>
        [DataMember]
        public int UserId { set; get; }
    }
}
