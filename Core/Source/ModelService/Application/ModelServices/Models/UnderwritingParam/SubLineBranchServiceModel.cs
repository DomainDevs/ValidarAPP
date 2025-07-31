// -----------------------------------------------------------------------
// <copyright file="SubLineBranchServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo para SubRamoTecnico
    /// </summary>
    [DataContract]
    public class SubLineBranchServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece Id de SubRamoTecnico
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion SubRamoTecnico
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece pequeña descripcion SubRamoTecnico
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece modelo de Ramo Tecnico
        /// </summary>
        [DataMember]
        public LineBusinessServiceQueryModel LineBusinessQuery { get; set; }
    }
}
