using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.EventsServices.Models;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventConditionGroup
    {
        /// <summary>
        /// Atributo para la propiedad ConditionId.
        /// </summary>
        [DataMember]
        public int ConditionId { set; get; }

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string Description { set; get; }

        /// <summary>
        /// Atributo para las entidades segun el id de condicion
        /// </summary>
        [DataMember]
        public List<EventEntity> EventEntities {get; set;}
    }
}
