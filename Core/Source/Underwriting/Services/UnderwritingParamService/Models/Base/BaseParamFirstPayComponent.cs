// -----------------------------------------------------------------------
// <copyright file="ParamFirstPayComponent.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase BaseParamFirstPayComponent
    /// </summary>
    [DataContract]
    public class BaseParamFirstPayComponent: Extension
    {
        /// <summary>
        /// Obtiene o establece Id
        /// </summary>
        [DataMember]
        public int IdComponent { get; set; }

        /// <summary>
        /// Obtiene o establece Id plan financiero
        /// </summary>
        [DataMember]
        public int IdFinancialPlan { get; set; }

        /// <summary>
        /// Obtiene o establece Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
