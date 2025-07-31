using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///     Tipo de Movimiento Contable
    /// </summary>
    [DataContract]
    public class AccountingMovementTypeDTO
    {
        /// <summary>
        ///     Identificador único del modelo
        /// </summary>
        [DataMember]
        public int AccountingMovementTypeId { get; set; }

        /// <summary>
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Es Automatico
        /// </summary>
        [DataMember]
        public bool IsAutomatic { get; set; }
    }
}
