// -----------------------------------------------------------------------
// <copyright file="CompanyBasicServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>David S. Niño T.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Datos Basicos de Compañia
    /// </summary>
    [DataContract]
    public class BasicCompanyServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la Compañia
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de Documento
        /// </summary>   
        [DataMember]
        public int DocumentType { get; set; }


        /// <summary>
        /// Obtiene o establece el Numero de Documento
        /// </summary>   
        [DataMember]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Obtiene o establece el digito de Documento de la Compañia
        /// </summary>
        [DataMember]
        public int CompanyDigit { get; set; }

        /// <summary>
        /// Obtiene o establece el Código de la Compañia
        /// </summary>   
        [DataMember]
        public int CompanyCode { get; set; }

        /// <summary>
        /// Obtiene o establece la Razon Social
        /// </summary>   
        [DataMember]
        public string TradeName { get; set; }


        /// <summary>
        /// Obtiene o establece el Tipo de Asociación
        /// </summary>   
        [DataMember]
        public int? TypePartnership { get; set; }

        /// <summary>
        /// Obtiene o establece Tipo de Empresa
        /// </summary>   
        [DataMember]
        public int? CompanyTypePartnership { get; set; }

        /// <summary>
        /// Obtiene o establece el Pais de la Compañia
        /// </summary>   
        [DataMember]
        public int Country { get; set; }

        /// <summary>
        /// Obtiene o establece la Fecha última actualización
        /// </summary>   
        [DataMember]
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// Obtiene o establece Lugar de Nacimiento
        /// </summary>   
        [DataMember]
        public string UpdateBy { get; set; }

        /// <summary>
        /// Obtiene o establece si la compañia es tomador de una poliza
        /// </summary>   
        [DataMember]
        public bool Policy { get; set; }

        /// <summary>
        /// Obtiene o establece si la compañia es asegurado de una poliza
        /// </summary>   
        [DataMember]
        public bool Insured { get; set; }

        /// <summary>
        /// Obtiene o establece si la compañia es beneficiario de una poliza
        /// </summary>   
        [DataMember]
        public bool Beneficiary { get; set; }
    }
}
