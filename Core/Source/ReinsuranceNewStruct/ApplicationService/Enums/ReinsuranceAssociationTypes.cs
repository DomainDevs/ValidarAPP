using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.Enums
{
    [DataContract]
    [Flags]
    public enum ReinsuranceAssociationTypes
    {
        [EnumMember]
        ByLineBusiness = 1,
        [EnumMember]
        ByLineBusinessSubLineBusiness = 2,
        [EnumMember]
        ByOperationTypePrefix = 3 ,
        [EnumMember]
        ByInsured = 4,
        [EnumMember]
        ByPrefix = 5,
        [EnumMember]
        ByPolicy = 6,
        [EnumMember]
        ByFacultativeIssue = 7,
        [EnumMember]
        ByInsuredPrefix = 8,
        [EnumMember]
        ByPrefixProduct = 9,
        [EnumMember]
        ByLineBusinessSubLineBusinessRisk = 10,
        [EnumMember]
        ByPrefixRisk = 11,
        [EnumMember]
        ByPolicyLineBusinessSubLineBusiness = 12,
        [EnumMember]
        ByLineBusinessSubLineBusinessCoverage = 13        
    }
}
