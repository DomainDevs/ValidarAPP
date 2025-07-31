// -----------------------------------------------------------------------
// <copyright file="AllianceBranchSalePonit.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UniquePersonParamService.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de negocio para punto de venta - sucursal de aliado
    /// </summary>    
    [DataContract]
    public class AllianceBranchSalePonit
    {
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
        /// Gets or sets Identificador de la sucursal
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// Gets or sets Nombre de la sucursal
        /// </summary>
        [DataMember]
        public string BranchDescription { get; set; }

        /// <summary>
        /// Gets or sets Identificador del punto de venta
        /// </summary>
        [DataMember]
        public int SalePointId { get; set; }

        /// <summary>
        /// Gets or sets Nombre del punto de venta
        /// </summary>
        [DataMember]
        public string SalePointDescription { get; set; }

        /// <summary>
        /// Get or sets estado de la operación
        /// </summary>
        [DataMember]
        public string Status { get; set; }
    }
}