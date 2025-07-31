using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventQueryType
    {
        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public int? QueryTypeCode { set; get; }

        /// <summary>
		/// Atributo para la propiedad Description.
		/// </summary>
        [DataMember]
        public string Description { set; get; }
    }
}
