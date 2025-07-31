using System.Runtime.Serialization;
using Sistran.Company.Application.Utilities.DTO;

// -----------------------------------------------------------------------
// <copyright file="UserGroupDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan David Torres</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniqueUserApplicationServices.DTO
{
    /// <summary>
    /// UserGroupDTO. Model of User Group DTO.
    /// </summary>
    [DataContract]
    public class UserGroupDTO
    {
        [DataMember]
        public int IdUser { get; set; }
        [DataMember]
        public int IdGroup { get; set; }
        [DataMember]
        public ErrorDTO ErrorDTO { get; set; }
    }
}
