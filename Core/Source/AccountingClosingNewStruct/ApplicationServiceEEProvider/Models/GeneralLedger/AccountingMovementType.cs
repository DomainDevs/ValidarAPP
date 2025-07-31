#region Using

using System;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.AccountingClosingServices.EEProvider.Models.GeneralLedger
{
    /// <summary>
    ///     Tipo de Movimiento Contable
    /// </summary>
    [DataContract]
    public class AccountingMovementType
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