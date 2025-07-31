// -----------------------------------------------------------------------
// <copyright file="CompanyParamRelRejectionCauses.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Company.AuthorizationPoliciesParamServices.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CompanyParamGroupPolicies
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string description { get; set; }
    }
}
