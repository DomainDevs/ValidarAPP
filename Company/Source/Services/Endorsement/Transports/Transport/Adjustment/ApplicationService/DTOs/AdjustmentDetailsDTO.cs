using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.DTOs
{
    /// <summary>
    /// Modelo para detallar los endosos
    /// </summary>
    [DataContract]
    public class AdjustmentDetailsDTO
    {
        /// <summary>
        /// Depósito inicial de prima
        /// </summary>
        [DataMember]
        public Decimal PremiumDepositAmount { get; set; }

        /// <summary>
        /// Tiene prima de deposito
        /// </summary>
        [DataMember]
        public Boolean HasPremiumDeposit { get; set; }

        /// <summary>
        /// Listado de Endosos declarados
        /// </summary>
        [DataMember]
        public List<DeclarationDTO> DeclarationEndorsements { get; set; }
    }
}
