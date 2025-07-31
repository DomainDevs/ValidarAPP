using System.Runtime.Serialization;

namespace Sistran.Company.Application.Endorsement.CreditNoteApplicationService.DTOs
{
    [DataContract]
    public class CoverageDTO
    {
        /// <summary>
        /// Monto de prima
        /// </summary>
        [DataMember]
        public decimal PremiumAmount { get; set; }

        /// <summary>
        /// Monto de prima a devolver
        /// </summary>
        [DataMember]
        public decimal PremiumtoReturn { get; set; }

        /// <summary>
        /// Identificador de la cobertura
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
