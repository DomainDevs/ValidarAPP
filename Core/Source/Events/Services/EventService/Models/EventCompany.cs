using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventCompany
    {

        /// <summary>
        /// Atributo para la propiedad groupEventId.
        /// </summary>
        [DataMember]
        public EventsGroup EventsGroup { set; get; }

        /// <summary>
        /// Atributo para la propiedad eventId.
        /// </summary>
        [DataMember]
        public int EventId { set; get; }

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string Description { set; get; }

        /// <summary>
        /// Atributo para la propiedad ValidationTypeCode.
        /// </summary>
        [DataMember]
        public EventValidationType ValidationType { set; get; }

        /// <summary>
        /// Atributo para la propiedad ProcedureName.
        /// </summary>
        [DataMember]
        public string ProcedureName { set; get; }
       
        /// <summary>
        /// Atributo para la propiedad ConditionId.
        /// </summary>
        [DataMember]
        public EventConditionGroup EventConditionGroup { set; get; }

        /// <summary>
        /// Atributo para la propiedad EnabledStop.
        /// </summary>
        [DataMember]
        public bool EnabledStop { set; get; }
      
        /// <summary>
        /// Atributo para la propiedad EnabledAuthorize.
        /// </summary>
        [DataMember]
        public bool EnabledAuthorize { set; get; }
        
        /// <summary>
        /// Atributo para la propiedad DescriptionErrorMessage.
        /// </summary>
        [DataMember]
        public string DescriptionErrorMessage { set; get; }
       
        /// <summary>
        /// Atributo para la propiedad Enabled.
        /// </summary>
        [DataMember]
        public bool Enabled { set; get; }
      
        /// <summary>
        /// Atributo para la propiedad TypeCode.
        /// </summary>
        [DataMember]
        public int? TypeCode { set; get; }
    }
}
