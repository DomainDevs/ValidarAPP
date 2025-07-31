// -----------------------------------------------------------------------
// <copyright file="CompanyTypeServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models.UniquePerson
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Runtime.Serialization;    

    /// <summary>
    /// Contiene las propiedades de la compañia
    /// </summary>
    [DataContract]
    public class CompanyTypeServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la compañia.
        /// </summary>
        [DataMember]
        public decimal Id { get; set; }

        /// <summary>
        /// Obtiene o establece el Description de la compañia.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece del ParametricServiceModel.
        /// </summary>
        [DataMember]
        public ParametricServiceModel ParametricServiceModel { get; set; }
    }
}