// -----------------------------------------------------------------------
// <copyright file="ParamBasicCompany.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>David S. Niño T.</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniquePersonParamService.Models
{
    using Sistran.Core.Application.Utilities.Error;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Datos Basicos de Compañia
    /// </summary>
    [DataContract]
    public class ParamBasicCompany
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


        /// <summary>
        /// Objeto que obtiene la Información Basica de compañia.
        /// </summary>
        /// <param name="ParamBasicCompany">Objeto ParamBasicCompany.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamBasicCompany, ErrorModel> GetParamBasicCompany(int IndividualId, int DocumentType, string DocumentNumber,
            int CompanyDigit, int CompanyCode, string TradeName, int? TypePartnership, int? CompanyTypePartnership, int Country,
            DateTime? LastUpdate, string UpdateBy, bool Policy, bool Insured, bool Beneficiary)
        {
            return new ResultValue<ParamBasicCompany, ErrorModel>(new ParamBasicCompany()
            {

                IndividualId = IndividualId,
                DocumentType = DocumentType,
                DocumentNumber = DocumentNumber,
                CompanyDigit = CompanyDigit,
                CompanyCode = CompanyCode,
                TradeName = TradeName,
                TypePartnership = TypePartnership,
                CompanyTypePartnership = CompanyTypePartnership,
                Country = Country,
                LastUpdate = LastUpdate,
                UpdateBy = UpdateBy,
                Policy = Policy,
                Insured = Insured,
                Beneficiary = Beneficiary

            });
        }
    }
}
