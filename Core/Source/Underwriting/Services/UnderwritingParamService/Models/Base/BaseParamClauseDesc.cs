// -----------------------------------------------------------------------
// <copyright file="ParamClauseDesc.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de negocio de Clausula
    /// </summary>
    [DataContract]
    public class BaseParamClauseDesc: Extension
    {
        /// <summary>
        /// Obtiene o establece el id de clausula
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la clausula
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la clausula es obligatoria
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }
    }
}
