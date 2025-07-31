using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.MassiveServices.Enums
{
    [Flags]
    public enum MassiveLoadStatus
    {
        [EnumMember]
        Validating = 1,
        [EnumMember]
        Validated = 2,
        [EnumMember]
        Tariffing = 3,
        [EnumMember]
        Tariffed = 4,
        [EnumMember]
        Issuing = 5,
        [EnumMember]
        Issued = 6,
        [EnumMember]
        Querying = 7,
        [EnumMember]
        Queried = 8
    }

    [Flags]
    public enum MassiveProcessType
    {
        [EnumMember]
        Emission = 1,
        [EnumMember]
        Modification = 2,
        [EnumMember]
        Renewal = 3,
        [EnumMember]
        Cancellation = 4
    }

    [Flags]
    public enum SubMassiveProcessType
    {
        [EnumMember]
        MassiveEmissionWithRequest = 1,
        [EnumMember]
        MassiveEmissionWithoutRequest = 2,
        [EnumMember]
        CollectiveEmission = 3,
        [EnumMember]
        Inclusion = 4,
        [EnumMember]
        Exclusion = 5,
        [EnumMember]
        MassiveRenewal = 6,
        [EnumMember]
        CollectiveRenewal = 7,
        [EnumMember]
        CancellationMassive = 8
    }

}