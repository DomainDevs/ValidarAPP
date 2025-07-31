// -----------------------------------------------------------------------
// <copyright file="CityQueryDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camilo Jimenéz </author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationAplicationServices.DTO
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Sistran.Company.Application.Utilities.DTO;

    [DataContract]
    public class CityQueryDTO
    {
        [DataMember]
        public List<CityDTO> CityDTO { get; set; }

        [DataMember]
        public ErrorDTO ErrorDTO { get; set; }
    }
}
