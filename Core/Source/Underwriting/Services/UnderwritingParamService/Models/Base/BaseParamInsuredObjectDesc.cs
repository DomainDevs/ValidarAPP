// -----------------------------------------------------------------------
// <copyright file="ParamInsuredObjectDesc.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;

    /// <summary>
    /// MOD-B objeto del seguro
    /// </summary>
    [DataContract]
    public class BaseParamInsuredObjectDesc: Extension
    {
        /// <summary>
        /// Obtiene o establece el id de objeto del seguro
        /// </summary>
        [DataMember]
        public int Id { get; set; } 

        /// <summary>
        /// Obtiene o establece la descripcion del objeto del seguro
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el objeto del seguro es obligatoria
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }
    }
}
