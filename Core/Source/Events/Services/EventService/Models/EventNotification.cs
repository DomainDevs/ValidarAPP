using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventNotification
    {
        /// <summary>
        /// Atributo para la propiedad Count.
        /// </summary>
        [DataMember]
        public int Count { get; set; }
        /// <summary>
        /// Atributo para la propiedad ResultId.
        /// </summary>
        [DataMember]
        public int ResultId { set; get; }
        /// <summary>
        /// Atributo para la propiedad RecordId.
        /// </summary>
        [DataMember]
        public int RecordId { set; get; }
        /// <summary>
        /// Atributo para la propiedad EventId.
        /// </summary>
        [DataMember]
        public int EventId { set; get; }
        /// <summary>
        /// Atributo para la propiedad Description_Error.
        /// </summary>
        [DataMember]
        public string DescriptionError { set; get; }
        /// <summary>
        /// Atributo para la propiedad Enabled_Stop.
        /// </summary>
        [DataMember]
        public bool EnabledStop { set; get; }
    }
}
