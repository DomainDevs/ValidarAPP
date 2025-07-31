// -----------------------------------------------------------------------
// <copyright file="ParamPeril.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de negocio de Amparo 
    /// </summary>
    [DataContract]
    public class BaseParamPeril: Extension
    {
        /// <summary>
        /// Obtiene o establece el id del amparo
        /// </summary>
        [DataMember]
        public int Id { get; set; }
      
        /// <summary>
        /// Obtiene o establece la descripcion del amparo
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        
        /// <summary>
        /// Obtiene o establece la descripcion abreviada del amparo
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
