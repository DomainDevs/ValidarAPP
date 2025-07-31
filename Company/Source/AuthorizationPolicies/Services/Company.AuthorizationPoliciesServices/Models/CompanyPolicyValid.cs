// -----------------------------------------------------------------------
// <copyright file="CompanyPolicyValid.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.AuthorizationPoliciesServices.Models
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;


    [DataContract]
    public class CompanyPolicyValid
    {
        /// <summary>
        /// Valida fecha inicial
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public DateTime dateIni { get; set; }

        /// <summary>
        /// Valida fecha final
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public DateTime dateFin { get; set; }

        /// <summary>
        /// Valida id estado
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public List<int> Status { get; set; }

        /// <summary>
        /// Valida id sucursal
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int idBranch { get; set; }

        /// <summary>
        /// Valida id ramos comercial
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int idPrefix { get; set; }
    }
}
