// -----------------------------------------------------------------------
// <copyright file="ParametrizationRateType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;

    /// <summary>
    /// Tipo de detalle (Modelo del negocio)
    /// </summary>
    [DataContract]
    public class BaseParametrizationRateType: Extension
    {
        /// <summary>
        /// Obtiene o establece el ID 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion 
        /// </summary>
        public string Description { get; set; }
    }
}
