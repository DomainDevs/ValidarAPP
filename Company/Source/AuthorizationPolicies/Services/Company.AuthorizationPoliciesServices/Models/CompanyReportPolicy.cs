// -----------------------------------------------------------------------
// <copyright file="CompanyPolicy.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.AuthorizationPoliciesServices.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class CompanyReportPolicy
    {
        /// <summary>
        /// Obtiene id de politica
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int idPolicies { get; set; }

        /// <summary>
        /// Obtiene descripcion de politica
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string descriptionPolicy { get; set; }

        /// <summary>
        /// Obtiene usuario de politica
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string userPolicy { get; set; }

        /// <summary>
        /// Obtiene fecha de politica
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public DateTime datePolicy { get; set; }

        /// <summary>
        /// Obtiene ramo comercial
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string prefix { get; set; }

        /// <summary>
        /// Obtiene numero de politica
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public long numberPolicy { get; set; }

        /// <summary>
        /// Obtiene tipo endozo 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string typeEndosment { get; set; }

        /// <summary>
        /// Obtiene usuario
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string user { get; set; }

        /// <summary>
        /// Obtiene fecha de autorizacion
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public DateTime? dateAuthorization { get; set; }

        /// <summary>
        /// Obtiene sucursal
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string branch { get; set; }

        /// <summary>
        /// Obtiene grupo de politicas
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string groupPolicies { get; set; }

        /// <summary>
        /// Obtiene estado de politica
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string statusPolicy { get; set; }

        /// <summary>
        /// Obtiene tiempo de espera
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int waitingTime { get; set; }
    }
}
