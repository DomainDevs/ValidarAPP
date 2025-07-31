using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class InsuranceCompanyControl
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        
        /// <summary>
        /// Identificador de la persona
        /// </summary>
        [DataMember]
        public int InsuranceCompanyId { get; set; }

        /// <summary>
        /// Accion "I" Insertar - "U" Modificar
        /// </summary>
        [DataMember]
        public string Action { get; set; }

    }
}
