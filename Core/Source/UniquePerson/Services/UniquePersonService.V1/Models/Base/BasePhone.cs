using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BasePhone : BaseGeneric
    {
        /// <summary>
        /// Es telefono principal
        /// </summary>
        [DataMember]
        public bool IsMain { get; set; }

        /// <summary>
        ///  Extensión
        /// </summary>
        [DataMember]
        public int? Extension { get; set; }

        /// <summary>
        /// Código del País
        /// </summary>
        [DataMember]
        public int? CountryCode { get; set; }

        /// <summary>
        /// Código de la Ciudad
        /// </summary>
        [DataMember]
        public int? CityCode { get; set; }

        /// <summary>
        /// Horario de Atención
        /// </summary>
        [DataMember]
        public string ScheduleAvailability { get; set; }
        
    }
}
