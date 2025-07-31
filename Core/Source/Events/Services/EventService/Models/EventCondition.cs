using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventCondition
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
        /// Atributo para la propiedad DelegationId.
        /// </summary>
        [DataMember]
        public int DelegationId { set; get; }

        /// <summary>
        /// Atributo para la propiedad vEntityId.
        /// </summary>
        [DataMember]
        public int EntityId { set; get; }

        /// <summary>
        /// Atributo para la propiedad ConditionQuantity.
        /// </summary>
        [DataMember]
        public int ConditionQuantity { set; get; }

        /// <summary>
        /// Atributo para la propiedad EventQuantity.
        /// </summary>
        [DataMember]
        public int EventQuantity { set; get; }

        /// <summary>
		/// Atributo para la propiedad ComparatorCode.
		/// </summary>
        [DataMember]
        public int ComparatorCode { set; get; }

        /// <summary>
        /// Atributo para la propiedad ConditionValue.
        /// </summary>
        [DataMember]
        public string ConditionValue { set; get; }
    }
}
