using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyMeasurementType
    {
        /// <summary>
        /// Obtiene o Setea el Identificador
        /// </summary>
        /// <value>
        /// Identificador
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o Setea Descripcion
        /// </summary>
        /// <value>
        /// Descripcion
        /// </value>
        [DataMember]
        public string Description { get; set; }
    }
}
