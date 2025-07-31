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
    public class CompanyParamRelRejectionCauses
    {
       [DataMember]
       public CompanyParamBaseEjectionCauses CompanyParamBaseEjectionCauses { get; set; }

       [DataMember]
       public CompanyParamGroupPolicies CompanyParamGroupPolicies { get; set; }
    }

}
