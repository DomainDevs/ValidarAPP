// -----------------------------------------------------------------------
// <copyright file="BranchTypeServiceModel.cs" company="SISTRAN">
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
    /// Contiene las propiedades de tipos de Sucursal
    /// </summary>
    [DataContract]
    public class BranchTypesServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece una lista de tipos de sucursal.
        /// </summary>
        [DataMember]
        public List<BranchTypeServiceModel> BranchTypeServiceModel { get; set; }
    }
}