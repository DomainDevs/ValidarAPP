// -----------------------------------------------------------------------
// <copyright file="ParamCoverage.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;

    /// <summary>
    /// Covertura en 2G
    /// </summary>
    [DataContract]
    public class BaseParamCoCoverage2G: Extension
    {
        /// <summary>
        /// Obtiene o establece el identificador de la covertura
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        
        /// <summary>
        /// Obtiene o establece la descripcion de la covertura
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la linea de negocio
        /// </summary>
        [DataMember]
        public int LineBusinessId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la sublinea de negocio
        /// </summary>
        [DataMember]
        public int SubLineBusinessId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del objeto del seguro
        /// </summary>
        [DataMember]
        public int InsuredObjectId { get; set; }
    }
}
