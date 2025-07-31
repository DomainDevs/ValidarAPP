using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Company.Application.Utilities.DTO;

// -----------------------------------------------------------------------
// <copyright file="GroupQueryDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan David Torres</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniqueUserApplicationServices.DTO
{
    [DataContract]
    public class GroupQueryDTO
    {
        [DataMember]
        public List<GroupDTO> GroupDTO { get; set; }

        [DataMember]
        public ErrorDTO ErrorDTO { get; set; }
    }
}
