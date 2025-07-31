// -----------------------------------------------------------------------
// <copyright file="ParamDeductibleDesc.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de negocio de deducible
    /// </summary>
    [DataContract]
    public class BaseParamDeductibleDesc: Extension
    {
        /// <summary>
        /// Obtiene o establece el id de deducible
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        
        /// <summary>
        /// Obtiene o establece la descripcion del deducible
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el deducible es obligatoria
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }
    }
}
