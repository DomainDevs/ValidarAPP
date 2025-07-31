// -----------------------------------------------------------------------
// <copyright file="ParametrizationDetail.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using System.Runtime.Serialization;
    /// <summary>
    /// Detalle precargado (Modelo del negocio)
    /// </summary>
    [DataContract]
    public class ParametrizationDetail: BaseParametrizationDetail
    {
       

        /// <summary>
        /// Obtiene o establece el ID de plan de pago
        /// </summary>
        public ParametrizationDetailType DetailType { get; set; }

        
    }
}
