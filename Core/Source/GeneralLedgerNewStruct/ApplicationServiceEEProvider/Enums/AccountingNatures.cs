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
        /// Cr�dito
        /// </summary>
        [EnumMember]
        Credit = 1,

        /// <summary>
        /// D�bito
        /// </summary>
        [EnumMember]
        Debit = 2
    }
}