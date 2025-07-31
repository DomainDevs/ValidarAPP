// -----------------------------------------------------------------------
// <copyright file="SMAgentAgency.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniquePersonParamService.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class SmAgentAgency
    {
        /// <summary>
        /// Gets or sets Identificador de la persona (Agente)
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Gets or sets Identificador de la agencia por agente
        /// </summary>
        [DataMember]
        public int AgencyAgencyId { get; set; }

        /// <summary>
        /// Gets or sets la descripción de la agencia.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets el código de la sucursal de la agencia.
        /// </summary>
        [DataMember]
        public int BranchCode { get; set; }

        /// <summary>
        /// Gets or sets el código de la agencia.
        /// </summary>
        [DataMember]
        public int AgentCode { get; set; }

        /// <summary>
        /// Gets or sets la fecha de baja de la agencia.
        /// </summary>
        [DataMember]
        public DateTime? DeclinedDate { get; set; }

        /// <summary>
        /// Gets or sets el código del tipo de baja del agente.
        /// </summary>
        [DataMember]
        public int? AgentDeclinedTypeCode { get; set; }


    }
}
