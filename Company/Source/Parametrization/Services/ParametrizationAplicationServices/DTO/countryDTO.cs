// -----------------------------------------------------------------------
// <copyright file="CountryDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camilo Jimenéz </author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationAplicationServices.DTO
{
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

    [DataContract]
    public class CountryDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}