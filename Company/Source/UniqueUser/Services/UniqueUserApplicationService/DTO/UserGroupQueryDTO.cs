using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Company.Application.Utilities.DTO;

// -----------------------------------------------------------------------
// <copyright file="UserGroupQueryDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan David Torres</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniqueUserApplicationServices.DTO
{
    [DataContract]
    public class UserGroupQueryDTO
    {
        [DataMember]
        public List<UserGroupDTO> UserGroupDTO { get; set; }

        [DataMember]
        public ErrorDTO ErrorDTO { get; set; }
    }
}
