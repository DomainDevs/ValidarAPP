// -----------------------------------------------------------------------
// <copyright file="CityDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camilo Jimenéz </author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationAplicationServices.DTO
{
    using System.Runtime.Serialization;
    using Sistran.Company.Application.Utilities.DTO;

    /// <summary>
    /// CityDTO. Modelo de ciudad DTO.
    /// </summary>
    [DataContract]
    public class CityDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public StateDTO State { get; set; }
        [DataMember]
        public CountryDTO Country { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public ErrorDTO ErrorDTO { get; set; }
    }
}
