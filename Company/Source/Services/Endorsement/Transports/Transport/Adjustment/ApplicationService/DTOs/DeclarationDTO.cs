using System.Runtime.Serialization;

namespace Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.DTOs
{
    /// <summary>
    /// Información de endoso de declaración
    /// </summary>
    [DataContract]
    public class DeclarationDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Monto de declaración
        /// </summary>
        [DataMember]
        public decimal DeclaredAmount { get; set; }

        /// <summary>
        /// Descripción del endoso
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
