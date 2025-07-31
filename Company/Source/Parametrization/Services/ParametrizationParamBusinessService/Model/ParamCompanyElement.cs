// -----------------------------------------------------------------------
// <copyright file="ParamCompanyElement.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camilo jimenéz</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ParametrizationParamBusinessService.Model
{
    using System.Runtime.Serialization;

    /// <summary>
    /// ParamCompanyElement.Modelo de Elemento individual asociado.
    /// </summary>
    [DataContract]
    public class ParamCompanyElement
    {
        /// <summary>
        /// Gets or sets. Entero(llave primaria).
        /// </summary>
        [DataMember]
        public int Pk { get; set; }

        /// <summary>
        /// Gets or sets. Descripción asociada.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
