// -----------------------------------------------------------------------
// <copyright file="Parameter.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Common
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parametros de variables
    /// </summary>
    [DataContract]
    public class ParameterServiceModel
    {
        /// <summary>
        /// Obtiene o establece el id del parametro
        /// </summary>
        [DataMember]
        public int ParameterId { get; set; }

        /// <summary>
        /// Obtiene o establece descripcion del parametro
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece valor del parametro
        /// </summary>
        [DataMember]
        public int Value { get; set; }

        /// <summary>
        /// Obtiene o establece Estado (creado, modificado o eliminado)
        /// </summary>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public ParametricServiceModel ParametricServiceModel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DiscontinuityLogServiceModel DiscontinuityLogServiceModel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public InfringementLogServiceModel InfringementLogServiceModel { get; set; }
    }
}
