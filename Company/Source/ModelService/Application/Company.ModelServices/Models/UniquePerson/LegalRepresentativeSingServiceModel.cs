// -----------------------------------------------------------------------
// <copyright file="LegalRepresentativeSingServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models.UniquePerson
{    
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contiene las propiedades de Firma Representante Legal
    /// </summary>
    [DataContract]
    public class LegalRepresentativeSingServiceModel
    {
        /// <summary>
        /// Obtiene o establece el CiaCode de Firma Representante Legal.
        /// </summary>
        [DataMember]
        public CompanyTypeServiceModel CompanyTypeServiceModel { get; set; }

        /// <summary>
        /// Obtiene o establece el BranchTypeServiceModel de Firma Representante Legal.
        /// </summary>
        [DataMember]
        public BranchTypeServiceModel BranchTypeServiceModel { get; set; }

        /// <summary>
        /// Obtiene o establece el CurrentFrom de Firma Representante Legal.
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Obtiene o establece el LegalRepresentative de Firma Representante Legal.
        /// </summary>
        [DataMember]
        public string LegalRepresentative { get; set; }

        /// <summary>
        /// Obtiene o establece el PathSignatureImg de Firma Representante Legal.
        /// </summary>
        [DataMember]
        public string PathSignatureImg { get; set; }

        /// <summary>
        /// Obtiene o establece el SignatureImg de Firma Representante Legal.
        /// </summary>
        [DataMember]
        public byte[] SignatureImg { get; set; }

        /// <summary>
        /// Obtiene o establece el UserId de Firma Representante Legal.
        /// </summary>
        [DataMember]
        public string UserId { get; set; }

        /// <summary>
        /// Obtiene o establece del ParametricServiceModel.
        /// </summary>
        [DataMember]
        public ParametricServiceModel ParametricServiceModel { get; set; }
    }
}