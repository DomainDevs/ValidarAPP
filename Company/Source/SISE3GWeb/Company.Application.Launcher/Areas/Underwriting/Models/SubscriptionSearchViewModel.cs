// -----------------------------------------------------------------------
// <copyright file="SubscriptionSearchViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Amezquita</author>
// -----------------------------------------------------------------------

using Sistran.Company.Application.ModelServices.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class SubscriptionSearchViewModel
    {
        /// <summary>
        /// Obtiene o establece Tomador
        /// </summary>
        [DataMember]
        [StringLength(100)]
        public string Holder { get; set; }

        /// <summary>
        /// Obtiene o establece Id del Tomador
        /// </summary>
        [DataMember]
        public int? HolderId { get; set; }
        /// <summary>
        /// Obtiene o establece Asegurado
        /// </summary>
        [DataMember]
        [StringLength(100)]
        public string Insured { get; set; }

        /// <summary>
        /// Obtiene o establece Id del Asegurado
        /// </summary>
        [DataMember]
        public int? InsuredId { get; set; }
        
        /// <summary>
        /// Obtiene o establece Agente Intermediario
        /// </summary>
        [DataMember]
        [StringLength(100)]
        public string AgentPrincipal { get; set; }

        /// <summary>
        /// Obtiene o establece Id del Intermediario Principal 
        /// </summary>
        [DataMember]
        public int? AgentPrincipalId { get; set; }

        /// <summary>
        /// Obtiene o establece Agente Intermediario
        /// </summary>
        [DataMember]
        public int? AgentAgency { get; set; }
        
        /// <summary>
        /// Obtiene o establece Numero de cotizacion
        /// </summary>
        [DataMember]
        public int? QuotationNumber { get; set; }

        /// <summary>
        /// Obtiene o establece Version
        /// </summary>
        [DataMember]
        public int? Version { get; set; }

        /// <summary>
        /// Obtiene o establece Placa
        /// </summary>
        [DataMember]
        [StringLength(10)]
        public string Plate { get; set; }

        /// <summary>
        /// Obtiene o establece Motor
        /// </summary>
        [DataMember]
        [StringLength(50)]
        public string Engine { get; set; }

        /// <summary>
        /// Obtiene o establece Chasis
        /// </summary>
        [DataMember]
        [StringLength(100)]
        public string Chassis { get; set; }

        /// <summary>
        /// Obtiene o establece Usuario
        /// </summary>
        [DataMember]
        [StringLength(100)]
        public string User { get; set; }

        /// <summary>
        /// Obtiene o establece id Usuario
        /// </summary>
        [DataMember]
        public int? UserId { get; set; }

        /// <summary>
        /// Obtiene o establece Id del Sucursal
        /// </summary>
        [DataMember]
        public int? BranchId { get; set; }

        /// <summary>
        /// Obtiene o establece Id del Ramo
        /// </summary>
        [DataMember]
        public int? PrefixId { get; set; }

        /// <summary>
        /// Obtiene o establece Numero de Poliza
        /// </summary>
        [DataMember]
        public int? PolicyNumber { get; set; }

        /// <summary>
        /// Obtiene o establece Id del Endoso
        /// </summary>
        [DataMember]
        public int? EndorsementId { get; set; }

        /// <summary>
        /// Obtiene o establece Numero de Temporario
        /// </summary>
        [DataMember]
        public int? TemporaryNumber { get; set; }

        /// <summary>
        /// Obtiene o establece Fecha Emision
        /// </summary>
        [DataMember]
        public DateTime? IssueDate { get; set; }
    }
}