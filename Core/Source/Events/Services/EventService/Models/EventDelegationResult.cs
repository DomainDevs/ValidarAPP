using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventDelegationResult
    {
        /// <summary>
        /// Atributo para la propiedad RiskId.
        /// </summary>
        [DataMember]
        public string RiskId { get; set; }

        /// <summary>
        /// Atributo para la propiedad Count.
        /// </summary>
        [DataMember]
        public int Count { set; get; }

        /// <summary>
        /// Atributo para la propiedad ModuleId.
        /// </summary>
        [DataMember]
        public int ModuleId { set; get; }

        /// <summary>
        /// Atributo para la propiedad SubModuleId.
        /// </summary>
        [DataMember]
        public int SubModuleId { set; get; }

        /// <summary>
        /// Atributo para la propiedad ResultId.
        /// </summary>
        [DataMember]
        public String ResultId { set; get; }

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
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string Description { set; get; }

        /// <summary>
        /// Atributo para la propiedad DescriptionErrorMessage.
        /// </summary>
        [DataMember]
        public string DescriptionErrorMessage { set; get; }

        /// <summary>
        /// Atributo para la propiedad TypeCode.
        /// </summary>
        [DataMember]
        public int? TypeCode { set; get; }

        /// <summary>
        /// Atributo para la propiedad EventDate.
        /// </summary>
        [DataMember]
        public DateTime EventDate { set; get; }

        /// <summary>
        /// Atributo para la propiedad IdAuthorizer.
        /// </summary>
        [DataMember]
        public int IdAuthorizer { get; set; }

        /// <summary>
        /// Atributo para la propiedad IdNotifier.
        /// </summary>
        [DataMember]
        public int IdNotifier { get; set; }

        /// <summary>
        /// Atributo para la propiedad ReasonRequest.
        /// </summary>
        [DataMember]
        public string ReasonRequest { get; set; }

        /// <summary>
        /// Atributo para la propiedad IsNotification.
        /// </summary>
        [DataMember]
        public bool IsNotification { get; set; }

        /// <summary>
        /// Atributo para la propiedad IdTemporal.
        /// </summary>
        [DataMember]
        public string IdTemporal { get; set; }

        /// <summary>
        /// Atributo para propiedad IndividualId y Tipo de documento
        /// </summary>
        [DataMember]
        public string Operation2Id { get; set; }
    }
}
