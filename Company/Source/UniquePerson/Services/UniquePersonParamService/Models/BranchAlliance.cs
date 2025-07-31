// -----------------------------------------------------------------------
// <copyright file="BranchAlliance.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniquePersonParamService.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de negocio para sucursal de aliado
    /// </summary>
    [DataContract]
    public class BranchAlliance
    {
        /// <summary>
        /// Gets or sets Identificador de sucursal
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// Gets or sets Descripción de la sucursal
        /// </summary>
        [DataMember]
        public string BranchDescription { get; set; }

        /// <summary>
        /// Gets or sets Identificador del aliado
        /// </summary>
        [DataMember]
        public int AllianceId { get; set; }

        /// <summary>
        /// Gets or sets Nombre del aliado
        /// </summary>
        [DataMember]
        public string AllianceDescription { get; set; }

        /// <summary>
        /// Gets or sets Código de la Ciudad de la sucursal
        /// </summary>
        [DataMember]
        public int? CityCD { get; set; }

        /// <summary>
        /// Gets or sets Nombre de la Ciudad de la sucursal
        /// </summary>
        [DataMember]
        public string CityName { get; set; }

        /// <summary>
        /// Gets or sets País de la sucursal
        /// </summary>
        [DataMember]
        public int? CountryCD { get; set; }

        /// <summary>
        /// Gets or sets Nombre del país de la sucursal
        /// </summary>
        [DataMember]
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets Departamento de la sucursal
        /// </summary>
        [DataMember]
        public int? StateCD { get; set; }

        /// <summary>
        /// Gets or sets Nombre del departamento de la sucursal
        /// </summary>
        [DataMember]
        public string StateName { get; set; }

        /// <summary>
        /// Gets or sets Puntos de venta de la sucursal
        /// </summary>
        [DataMember]
        public List<AllianceBranchSalePonit> SalesPointsAlliance { get; set; }

        /// <summary>
        /// Gets or sets Estado de la operación
        /// </summary>
        [DataMember]
        public string Status { get; set; }
    }
}