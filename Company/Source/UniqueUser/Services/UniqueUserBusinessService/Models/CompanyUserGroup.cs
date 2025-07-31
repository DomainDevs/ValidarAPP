using System.Runtime.Serialization;

// -----------------------------------------------------------------------
// <copyright file="CompanyUserGroup.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan David Torres</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniqueUserBusinessService.Models
{
    /// <summary>
    /// CompanyUserGroup. Model of User Group.
    /// </summary>
    [DataContract]
    public class CompanyUserGroup
    {
        [DataMember]
        public int IdUser { get; set; }
        [DataMember]
        public int IdGroup { get; set; }
    }
}
