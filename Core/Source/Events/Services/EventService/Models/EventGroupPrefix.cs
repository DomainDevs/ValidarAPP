using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{

    [DataContract]
    public class EventGroupPrefix
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
        /// Atributo para la propiedad PrefixCode.
        /// </summary>
        [DataMember]
        public int PrefixCode { set; get; }

    }
}
