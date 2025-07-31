using System.Runtime.Serialization;


namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventComparisonType
    {
        /// <summary>
        /// Atributo para la propiedad comparatorCode.
        /// </summary>
        [DataMember]
        public int ComparatorCode { set; get; }

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string Description { set; get; }

        /// <summary>
        /// Atributo para la propiedad SmallDesc.
        /// </summary>
        [DataMember]
        public string SmallDesc { set; get; }

        /// <summary>
        /// Atributo para la propiedad Symbol.
        /// </summary>
        [DataMember]
        public string Symbol { set; get; }

        /// <summary>
        /// Atributo para la propiedad TextInd.
        /// </summary>
        [DataMember]
        public bool TextInd { set; get; }

        /// <summary>
        /// Atributo para la propiedad ComboInd.
        /// </summary>
        [DataMember]
        public bool ComboInd { set; get; }

        /// <summary>
        /// Atributo para la propiedad QueryInd.
        /// </summary>
        [DataMember]
        public bool QueryInd { set; get; }

        /// <summary>
        /// Atributo para la propiedad NumValues.
        /// </summary>
        [DataMember]
        public decimal NumValues { set; get; }

    }
}
