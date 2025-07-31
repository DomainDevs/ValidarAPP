using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventsGroup
    {
        /// <summary>
        /// Atributo para la propiedad GroupEventId
        /// </summary>
        [DataMember]
        public int GroupEventId { set; get; }

        /// <summary>
        /// Atributo para la propiedad ModuleCode.
        /// </summary>
        [DataMember]
        public int ModuleCode { set; get; }

        /// <summary>
        /// Atributo para la propiedad SubmoduleCode.
        /// </summary>
        [DataMember]
        public int SubmoduleCode { set; get; }

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string Description { set; get; }

        /// <summary>
        /// Atributo para la propiedad EnabledInd.
        /// </summary>
        [DataMember]
        public bool EnabledInd { set; get; }

        /// <summary>
        /// Atributo para la propiedad AuthorizationReport.
        /// </summary>
        [DataMember]
        public string AuthorizationReport { set; get; }

        /// <summary>
        /// Atributo para la propiedad ProcedureAuthorized.
        /// </summary>
        public string ProcedureAuthorized { set; get; }

        /// <summary>
        /// Atributo para la propiedad ProcedureReject.
        /// </summary>
        [DataMember]
        public string ProcedureReject { set; get; }
    }
}
