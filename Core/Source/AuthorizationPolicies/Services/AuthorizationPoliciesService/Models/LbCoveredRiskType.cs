// -----------------------------------------------------------------------
// <copyright file="AuthorizationAnswerGroup.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    [DataContract]
    public class LbCoveredRiskType
    {
        [DataMember]
        public int LineBusiness { get; set; }

        [DataMember]
        public int CoveredRiskType { get; set; }
    }
}
