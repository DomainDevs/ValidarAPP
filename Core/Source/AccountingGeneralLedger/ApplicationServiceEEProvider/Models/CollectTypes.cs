using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Models
{
        [DataContract]
        [Flags]
        public enum CollectTypes
        {
            [EnumMember]
            Incoming = 1,
            [EnumMember]
            Outgoing = 2,
            [EnumMember]
            DailyAccount = 3,
        }

}
