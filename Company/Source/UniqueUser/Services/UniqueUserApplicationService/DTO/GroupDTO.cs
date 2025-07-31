using System.Runtime.Serialization;
using Sistran.Company.Application.Utilities.DTO;

// -----------------------------------------------------------------------
// <copyright file="GroupDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan David Torres</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniqueUserApplicationServices.DTO
{
    /// <summary>
    /// GroupDTO. Model of Group DTO.
    /// </summary>
    [DataContract]
    public class GroupDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool State { get; set; }
        [DataMember]
        public ErrorDTO ErrorDTO { get; set; }
    }
}
