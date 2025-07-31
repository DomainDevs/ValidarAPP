// -----------------------------------------------------------------------
// <copyright file="Status.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.AuthorizationPoliciesServices.Models
{
    using System.Runtime.Serialization;

    public class Status
    {
        /// <summary>
        /// Obtiene el ID
        /// </summary>
        /// <returns>lista de usuarios autorizadores</returns>
        [DataMember]
        public int Id { set; get; }

        /// <summary>
        /// Obtiene la Descripcion
        /// </summary>
        /// <returns>lista de usuarios autorizadores</returns>
        [DataMember]
        public string Description { set; get; }
    }
}
