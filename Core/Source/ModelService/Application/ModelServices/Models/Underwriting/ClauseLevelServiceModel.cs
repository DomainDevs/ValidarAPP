// -----------------------------------------------------------------------
// <copyright file="ClauseLevelServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo para clausulas por nivel
    /// </summary>
    [DataContract]
    public class ClauseLevelServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece Id de ClauseLevel
        /// </summary>
        [DataMember]
        public int ClauseLevelId { get; set; }

        /// <summary>
        /// Obtiene o establece Id de la clausula
        /// </summary>
        [DataMember]
        public int ClauseId { get; set; }

        /// <summary>
        /// Obtiene o establece el Id tipo nivel
        /// </summary>
        [DataMember]
        public int? ConditionLevelId { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es tipo obligatorio
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }
    }
}
