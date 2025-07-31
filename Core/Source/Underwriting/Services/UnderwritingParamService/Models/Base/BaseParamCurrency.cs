// -----------------------------------------------------------------------
// <copyright file="ParamCurrency.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de moneda
    /// </summary>
    [DataContract]
    public class BaseParamCurrency: Extension
    { 
        /// <summary>
        /// Obtiene o establece Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Descricion
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
