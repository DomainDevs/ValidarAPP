using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.GeneralLedgerServices.Enums
{
    /// <summary>
    ///     Id de tipos Posfechados
    /// </summary>
    [DataContract]
    public enum PostDateTypes
    {
        /// <summary>
        ///     Cheque
        /// </summary>
        [EnumMember] Check = 1,

        /// <summary>
        ///     Crédito
        /// </summary>
        [EnumMember] Credit = 2
    }
}
