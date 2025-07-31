using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums
{
    /// <summary>
    /// Emun de Naturaleza contable
    /// </summary>
    [DataContract]
    [Flags]
    public enum AccountingNatures
    {
        /// <summary>
        /// Crédito
        /// </summary>
        [EnumMember]
        Credit = 1,

        /// <summary>
        /// Débito
        /// </summary>
        [EnumMember]
        Debit = 2
    }
}