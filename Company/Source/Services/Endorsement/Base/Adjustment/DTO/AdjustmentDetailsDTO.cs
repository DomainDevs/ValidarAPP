using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Adjustment.DTO
{
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
