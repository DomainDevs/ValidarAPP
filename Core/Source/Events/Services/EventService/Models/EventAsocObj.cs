using System.Runtime.Serialization;


namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventAsocObj
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
        /// Atributo para la propiedad AccessId.
        /// </summary>
        [DataMember]
        public int AccessId { set; get; }
    }
}
