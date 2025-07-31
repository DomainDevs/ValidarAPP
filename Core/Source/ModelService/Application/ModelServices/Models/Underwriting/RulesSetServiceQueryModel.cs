// -----------------------------------------------------------------------
// <copyright file="ComponentsServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// ----------------------------------------------------------------------- 


namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    /// <summary>
    /// Listados de Reglas
    /// </summary>
    [DataContract]
    public class RulesSetServiceQueryModel : ErrorServiceModel
    {

        /// <summary>
        /// Obtiene o establece el listado de reglas 
        /// </summary>
        [DataMember]
        public List<RuleSetServiceQueryModel> RuleSetServiceQueryModel { get; set; }
    }
}

