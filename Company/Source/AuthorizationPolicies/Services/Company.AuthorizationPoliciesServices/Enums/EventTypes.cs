using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.AuthorizationPoliciesServices.Enums
{
    [Flags]
    public enum EventTypes
    {
        /// <summary>
        /// Prima
        /// </summary>
        [EnumMember]
        Subscription = 1,
        /// <summary>
        /// Prima
        /// </summary>
        [EnumMember]
        Endorsement = 2,
        /// <summary>
        /// Gastos
        /// </summary>
        [EnumMember]
        Printing = 3,
        /// <summary>
        /// Impuestos
        /// </summary>
        [EnumMember]
        Release = 4
    }

    public enum Reject
    {
        RECHAZAR = 0
    }

    public enum Authorize
    {
        AUTORIZAR = 0
    }
}
