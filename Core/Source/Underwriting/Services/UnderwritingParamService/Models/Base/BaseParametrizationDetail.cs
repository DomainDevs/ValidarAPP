// -----------------------------------------------------------------------
// <copyright file="ParametrizationDetail.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Detalle precargado (Modelo del negocio)
    /// </summary>
    [DataContract]
    public class BaseParametrizationDetail: Extension
    {
        /// <summary>
        /// Obtiene o establece el ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion
        /// </summary>
        public string Description { get; set; }

        
        /// <summary>
        /// Obtiene o establece un valor que indica si está habilitado
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Obtiene o establece los tipos de tasa
        /// </summary>
        [DataMember]
        public ModelServices.Enums.RateType? RateType { get; set; }        

        /// <summary>
        /// Obtiene o establece el valor de la tasa
        /// </summary>
        [DataMember]
        public decimal? Rate { get; set; }
        
        /// <summary>
        /// Obtiene o establece el valor del sublimite
        /// </summary>
        [DataMember]
        public decimal? SublimitAmt { get; set; }
    }
}
