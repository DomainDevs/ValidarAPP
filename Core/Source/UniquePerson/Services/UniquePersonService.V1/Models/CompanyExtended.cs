using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Datos Adicionales de la Compañia
    /// </summary>
    [DataContract]
    public class CompanyExtended
    {
        /// <summary>
        /// Obtiene o Setea El Id Digito Verificacion
        /// </summary>
        /// <value>
        /// Digito Verificacion
        /// </value>
        [DataMember]
        public int? VerifyDigit { get; set; }

        /// <summary>
        /// Tipo de asociación
        /// </summary>
        [DataMember]
        public AssociationType AssociationType { get; set; }

    }
}
