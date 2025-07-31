#region Using

using System;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.AccountingClosingServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum AccountingAccountApplications
    {
        [EnumMember] Accounting = 1,
        [EnumMember] Ledger = 2,
        [EnumMember] Others = 3,
        [EnumMember] AccountingLedger = 4,
    }
}