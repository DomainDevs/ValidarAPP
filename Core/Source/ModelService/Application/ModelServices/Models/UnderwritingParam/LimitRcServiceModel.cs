// -----------------------------------------------------------------------
// <copyright file="LimitRcModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio para limite Rc
    /// </summary>
    [DataContract]
    public class LimitRcServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece codigo del limite rc
        /// </summary>
        [DataMember]
        public int LimitRcCd { get; set; }

        /// <summary>
        /// Obtiene o establece limite uno
        /// </summary>
        [DataMember]
        public Decimal Limit1 { get; set; }

        /// <summary>
        /// Obtiene o establece limite dos
        /// </summary>
        [DataMember]
        public Decimal Limit2 { get; set; }

        /// <summary>
        /// Obtiene o establece limite tres
        /// </summary>
        [DataMember]
        public Decimal Limit3 { get; set; }

        /// <summary>
        /// Obtiene o establece limite unico
        /// </summary>
        [DataMember]
        public Decimal LimitUnique { get; set; }

        /// <summary>
        /// Obtiene o establece descripcion de limite
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
