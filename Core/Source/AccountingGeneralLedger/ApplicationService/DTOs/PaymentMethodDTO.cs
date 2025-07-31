using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class PaymentMethodDTO
    {
        /// <summary>
        ///     Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Ingresos
        /// </summary>
        [DataMember]
        public bool IsIncome { get; set; }

        /// <summary>
        ///     Egresos
        /// </summary>
        [DataMember]
        public bool IsOutcome { get; set; }
    }
}
