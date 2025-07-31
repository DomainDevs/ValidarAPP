// -----------------------------------------------------------------------
// <copyright file="LegalRepresentativesSingServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models.UniquePerson
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contiene las propiedades de Firma Representante Legal
    /// </summary>
    [DataContract]
    public class LegalRepresentativesSingServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece una lista de Firma Representante Legal.
        /// </summary>
        [DataMember]
        public List<LegalRepresentativeSingServiceModel> LegalRepresentativeSingServiceModel { get; set; }
    }
}
