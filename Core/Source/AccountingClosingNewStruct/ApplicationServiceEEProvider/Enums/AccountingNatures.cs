using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingClosingServices.EEProvider.Enums
{
    /// <summary>
    /// Emun de Naturaleza contable
    /// </summary>
    [DataContract]
    [Flags]
    public enum AccountingNatures
    {
        /// <summary>
        /// D�bito
        /// </summary>
        [EnumMember]
        Credit = 1,

        /// <summary>
        /// Cr�dito
        /// </summary>
        [EnumMember]
        Debit = 2
    }
}