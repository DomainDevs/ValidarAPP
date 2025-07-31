// -----------------------------------------------------------------------
// <copyright file="BusinessTypeDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>ETriana</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ParametrizationAplicationServices.DTO
{
    using System.Runtime.Serialization;

    /// <summary>
    /// BusinessTypeDTO. Clase negocio DTO.
    /// </summary>
    [DataContract]
    public class BusinessTypeDTO
    {
        /// <summary>
        /// Gets or sets. Identificador de negocio. 
        /// </summary>
        [DataMember]
        public int BUSINESS_TYPE_CD { get; set; }
        
        /// <summary>
        /// Gets or sets. Descripción del negocio.
        /// </summary>
        [DataMember]
        public string SMALL_DESCRIPTION { get; set; }
    }
}
