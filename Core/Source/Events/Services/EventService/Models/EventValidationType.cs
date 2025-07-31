using System.Runtime.Serialization;


namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventValidationType
    {
        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public int ValidationTypeCode { set; get; }

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string Description { set; get; }

        /// <summary>
        /// Atributo para la propiedad ProcedureInd.
        /// </summary>
        [DataMember]
        public bool ProcedureInd { set; get; }
    }
}
