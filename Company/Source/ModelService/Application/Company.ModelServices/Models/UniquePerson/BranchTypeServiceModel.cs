// -----------------------------------------------------------------------
// <copyright file="BranchTypeServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models.UniquePerson
{
    using System.Runtime.Serialization;    
    using Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    /// Contiene las propiedades de la Sucursal
    /// </summary>
    [DataContract]
    public class BranchTypeServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la Sucursal.
        /// </summary>
        [DataMember]
        public decimal Id { get; set; }

        /// <summary>
        /// Obtiene o establece el Description de la Sucursal.
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