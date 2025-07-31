// -----------------------------------------------------------------------
// <copyright file="MiniumPremiunRelationDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Wilfrido Heredia Carrera</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UnderwritingParamApplicationService.DTOs
{
    using Sistran.Company.Application.Utilities.DTO;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    /// <summary>
    /// MiniumPremiunRelationDTO
    /// </summary>
    [DataContract]
    public class MinPremiunRelationQueryDTO
    {
        [DataMember]
        public List<MinPremiunRelationDTO> MinPremiunRelationDTO { get; set; }        
        [DataMember]
        public ErrorDTO ErrorDTO { get; set; }
    }
}
