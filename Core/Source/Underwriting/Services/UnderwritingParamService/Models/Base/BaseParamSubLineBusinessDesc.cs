// -----------------------------------------------------------------------
// <copyright file="ParamSubLineBusinessDesc.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Modelo de negocio de subramo tenico
    /// </summary>
    [DataContract]
    public class BaseParamSubLineBusinessDesc: Extension
    {
        /// <summary>
        /// Obtiene o establece el id de del subramo tecnico
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion del subramo tecnico
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
