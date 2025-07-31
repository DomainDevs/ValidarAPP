// -----------------------------------------------------------------------
// <copyright file="HierarchyServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\cvergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.AuthorizationPolicies
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="HierarchyServiceQueryModel" />
    /// </summary>
    [DataContract]
    public class HierarchyServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece la Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }
    }
}
