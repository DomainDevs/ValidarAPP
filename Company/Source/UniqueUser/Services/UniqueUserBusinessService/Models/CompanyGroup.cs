using System.Runtime.Serialization;

// -----------------------------------------------------------------------
// <copyright file="CompanyGroup.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan David Torres</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniqueUserBusinessService.Models
{
    /// <summary>
    /// CompanyGroup. Model of Group.
    /// </summary>
    [DataContract]
    public class CompanyGroup
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool State { get; set; }
    }
}
