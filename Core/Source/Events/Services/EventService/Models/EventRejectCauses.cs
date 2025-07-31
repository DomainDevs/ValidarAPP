using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventRejectCauses
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
        /// Atributo para la propiedad RejectId.
        /// </summary>
        [DataMember]
        public int RejectId { set; get; }

        /// <summary>
		/// Atributo para la propiedad Description.
		/// </summary>
        [DataMember]
        public string Description { set; get; }

    }
}
