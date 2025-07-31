using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimsGeneralLedgerWorkerServices.EEProvider.Enums
{

    [DataContract]
    [Flags]
    public enum ClaimsGeneralLedgerWorkerKey
    {
        [EnumMember]
        GL_JOURNAL_ENTRY_TRANSACTION_NUMBER,
        [EnumMember]
        CLM_CLAIMS_MODULE
    }
}
