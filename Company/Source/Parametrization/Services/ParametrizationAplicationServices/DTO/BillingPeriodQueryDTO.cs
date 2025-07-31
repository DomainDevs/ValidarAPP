// -----------------------------------------------------------------------
// <copyright file="BillingPeriodQueryDTO.cs" company="SISTRAN">
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
    /// BillingPeriodQueryDTO. Lista de Objetos de Negocio.
    /// </summary>
    [DataContract]
    public class BillingPeriodQueryDTO
    {
        /// <summary>
        /// Constructor de la clase. crea el Objeto BillingPeriodQueryDTOs
        /// </summary>
        public BillingPeriodQueryDTO()
        {
            this.BillingPeriodQueryDTOs = new List<BillingPeriodDTO>();
            this.Error = new Utilities.DTO.ErrorDTO();
            this.Error.ErrorDescription = new List<string>();
        }

        /// <summary>
        /// Gets or sets. Obtiene  los periodos de facturacion  
        /// </summary>
        [DataMember]
        public List<BillingPeriodDTO> BillingPeriodQueryDTOs { get; set; }

        /// <summary>
        /// Gets or sets. Objeto de Error.
        /// </summary>
        [DataMember]
        public ErrorDTO Error { get; set; }
    }
}
