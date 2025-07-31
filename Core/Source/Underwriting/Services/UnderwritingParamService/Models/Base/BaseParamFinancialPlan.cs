// -----------------------------------------------------------------------
// <copyright file="ParamFinancialPlan.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;
 
    /// <summary>
    /// Modelo plan financiero
    /// </summary>
    [DataContract]
    public class BaseParamFinancialPlan: Extension
    {
        /// <summary>
        /// Obtiene o establece Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece minima cuota
        /// </summary>
        [DataMember]
        public int MinQuota { get; set; }
       


    }
}
