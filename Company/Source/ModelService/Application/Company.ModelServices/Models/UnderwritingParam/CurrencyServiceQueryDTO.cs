// <copyright file="CurrencyServiceQueryDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan David Moreno</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models.UnderwritingParam
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de moneda
    /// </summary>
    [DataContract]
    public class CurrencyServiceQueryDTO 
    {
        /// <summary>
        /// Obtiene o establece el Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }        
    }
}
