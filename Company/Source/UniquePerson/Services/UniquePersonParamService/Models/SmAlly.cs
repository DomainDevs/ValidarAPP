// -----------------------------------------------------------------------
// <copyright file="SMAlly.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UniquePersonParamService.Models
{    
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio para los aliados.
    /// </summary>
    [DataContract]
    public class SmAlly
    {
        /// <summary>
        /// Gets or sets Identificador del aliado.
        /// </summary>
        [DataMember]
        public int AllianceId { get; set; }
             

        /// <summary>
        /// Gets or sets la descripción del aliado.
        /// </summary>
        [DataMember]
        public string Description { get; set; }        
    }
}
