using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BasePhone : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Teléfono
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Es telefono principal
        /// </summary>
        [DataMember]
        public bool IsMain { get; set; }

        /// <summary>
        /// Extension
        /// </summary>
        [DataMember]
        public int? Extension { get; set; }

        /// <summary>
        /// Extension
        /// </summary>
        [DataMember]
        public string ScheduleAvailability { get; set; }


        /// <summary>
        /// Usuario que actualiza el registro
        /// </summary>
        [DataMember]
        public string UpdateUser { get; set; }

        /// <summary>
        /// Fecha de actualizacion del registro
        /// </summary>
        [DataMember]
        public string UpdateDate { get; set; }

        /// <summary>
        /// Indicativo del pais
        /// </summary>
        [DataMember]
        public int? CountryCode { get; set; }

        /// <summary>
        /// indicativo de la ciudad
        /// </summary>
        [DataMember]
        public int? CityCode { get; set; }



    }
}
