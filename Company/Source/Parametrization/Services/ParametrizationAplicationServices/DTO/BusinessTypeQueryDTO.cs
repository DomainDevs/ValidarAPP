// -----------------------------------------------------------------------
// <copyright file="BusinessTypeQueryDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>ETriana</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ParametrizationAplicationServices.DTO
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Sistran.Company.Application.Utilities.DTO;

    /// <summary>
    /// BusinessTypeQueryTDO. Lista de Modelos DTO Negocio.
    /// </summary>
    [DataContract]
    public class BusinessTypeQueryDTO
    {
        /// <summary>
        /// Constructor de la clase. crea el Objeto BusinessTypeQueryDTOs
        /// </summary>
        public BusinessTypeQueryDTO()
        {
            this.BusinessTypeQueryDTOs = new List<BusinessTypeDTO>();
            this.Error = new Utilities.DTO.ErrorDTO();
            this.Error.ErrorDescription = new List<string>();
        }

        /// <summary>
        /// Gets or Sets.  Obtiene  los tipos de negocio cubiertos
        /// </summary>
        [DataMember]
        public List<BusinessTypeDTO> BusinessTypeQueryDTOs { get; set; }

        /// <summary>
        ///  Gets or sets. Objeto de Error.
        /// </summary>
        [DataMember]
        public ErrorDTO Error { get; set; }
    }
}
