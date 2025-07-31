using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventLevels
    {
        /// <summary>
        /// Atributo para la propiedad LevelId.
        /// </summary>
        [DataMember]
        public int LevelId { set; get; }

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string Description { set; get; }
    }
}
