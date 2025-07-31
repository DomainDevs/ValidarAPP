// -----------------------------------------------------------------------
// <copyright file="InfringementLogServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Common
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parametros de Dias de Infraccion
    /// </summary>
    [DataContract]
    public class InfringementLogServiceModel
    {
        /// <summary>
        /// Obtiene o establece Id del log
        /// </summary>
        [DataMember]
        public long daysValidateInfringementLogId { get; set; }

        /// <summary>
        /// Obtiene o establece Dias de Infracción
        /// </summary>
        [DataMember]
        public long daysValidateInfringement { get; set; }

        /// <summary>
        /// Obtiene o establece Fecha de registro
        /// </summary>
        [DataMember]
        public DateTime registrationDate { get; set; }

        /// <summary>
        /// Obtiene o establece id del usuario
        /// </summary>
        [DataMember]
        public int userId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public ParametricServiceModel ParametricServiceModel { get; set; }

    }
}
