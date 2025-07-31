using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventReassignmentUser
    {

        /// <summary>
        /// Atributo para la propiedad HierarEventUser.
        /// </summary>
        [DataMember]
        public int HierarEventUser { set; get; }

        /// <summary>
        /// Atributo para la propiedad EventUserId.
        /// </summary>
        [DataMember]
        public int EventUserId { set; get; }

        /// <summary>
        /// Atributo para la propiedad AuthoUserId.
        /// </summary>
        [DataMember]
        public int AuthoUserId { set; get; }

        /// <summary>
        /// Atributo para la propiedad HierarReassUser.
        /// </summary>
        [DataMember]
        public int HierarReassUser { set; get; }

        /// <summary>
        /// Atributo para la propiedad ReassUserId.
        /// </summary>
        [DataMember]
        public int ReassUserId { set; get; }

        /// <summary>
        /// Atributo para la propiedad ReassAuthoUserId.
        /// </summary>
        [DataMember]
        public int ReassAuthoUserId { set; get; }

        /// <summary>
        /// Atributo para la propiedad BeginDate.
        /// </summary>
        [DataMember]
        public DateTime BeginDate { set; get; }

        /// <summary>
        /// Atributo para la propiedad EndDate.
        /// </summary>
        [DataMember]
        public DateTime EndDate { set; get; }

        /// <summary>
        /// Atributo para la propiedad EnabledInd.
        /// </summary>
        [DataMember]
        public int EnabledInd { set; get; }

        /// <summary>
        /// Atributo para la propiedad AuthorizationId.
        /// </summary>
        [DataMember]
        public int AuthorizationId { set; get; }

        /// <summary>
        /// Atributo para la propiedad EventInitial.
        /// </summary>
        [DataMember]
        public bool EventInitial { set; get; }
    }
}
