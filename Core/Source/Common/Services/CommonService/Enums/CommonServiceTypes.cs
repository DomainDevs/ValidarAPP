// -----------------------------------------------------------------------
// <copyright file="CommonServiceTypes.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author></author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.CommonService.Enums
{
    using System.Runtime.Serialization;

    public enum CoveredRiskType
    {
        Vehicle = 1,
        Location = 2,
        Surety = 7,
        Transport = 8,
        Aircraft = 9,
        Aeronavigation = 100
    }

    public enum SubCoveredRiskType
    {
        Vehicle = 1,
        ThirdPartyLiability = 2,
        Property = 3,
        Liability = 4,
        Surety = 5,
        JudicialSurety = 6,
        Transport = 7,
        Lease = 8,
        Marine = 9,
        Fidelity = 10,
        FidelityNewVersion = 11,
        Aircraft = 12
    }
  

   
    public enum ClauseStatuses
    {
        [EnumMember]
        Included = 1,
        [EnumMember]
        Excluded = 2,
        [EnumMember]
        Original = 3
    }
}
