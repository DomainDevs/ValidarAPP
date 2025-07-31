// -----------------------------------------------------------------------
// <copyright file="Alliance.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniquePersonParamService.Models
{
    using Sistran.Core.Application.CommonService.Models;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de negocio para aliados.
    /// </summary>
    [DataContract]
    public class Alliance
    {
        /// <summary>
        /// Gets or sets Código del aliado
        /// </summary>
        [DataMember]
        public int AllianceId { get; set; }

        /// <summary>
        /// Gets or sets Descripción del aliado
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Indicador de consulta a DataCrédito
        /// </summary>
        [DataMember]
        public bool IsScore { get; set; }

        /// <summary>
        /// Gets or sets Indicador de multas de tránsito
        /// </summary>
        [DataMember]
        public bool IsFine { get; set; }

        /// <summary>
        /// Get or sets estado de la operación
        /// </summary>
        [DataMember]
        public string Status { get; set; }
		
		//[DataMember]
  //      public Agency Agency { get; set; }

        /// <summary>
        /// Asignación impresión especial
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        [DataMember]
        public int SpecialPrint { get; set; }
    }
}