using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventDataType
    {
        /// <summary>
        /// Atributo para la propiedad dataTypeCode.
        /// </summary>
        [DataMember]
        public int? DataTypeCode { set; get; }

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string Description { set; get; }

        /// <summary>
        /// Atributo para la propiedad NumericInd.
        /// </summary>
        [DataMember]
        public bool NumericInd { set; get; }

        /// <summary>
        /// Atributo para la propiedad SqlDataType.
        /// </summary>
        [DataMember]
        public string SqlDataType { set; get; }
    }
}
