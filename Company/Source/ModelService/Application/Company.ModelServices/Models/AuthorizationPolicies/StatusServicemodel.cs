// -----------------------------------------------------------------------
// <copyright file="HierarchyServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Stiveen Niño</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies
{

    using System.Runtime.Serialization;
    /// <summary>
    /// Defines the <see cref="StatusServicemodel" />
    /// </summary>
    /// 
    [DataContract]
    public class StatusServicemodel
    {
        /// <summary>
        /// Obtiene o establece ID
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
