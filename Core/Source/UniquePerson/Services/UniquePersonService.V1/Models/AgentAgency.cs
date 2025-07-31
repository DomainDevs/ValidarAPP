// -----------------------------------------------------------------------
// <copyright file="AgentAgency.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class AgentAgency
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
        /// Gets or sets Identificador del aliado
        /// </summary>
        [DataMember]
        public int AllianceId { get; set; }

        /// <summary>
        /// Gets or sets Indicador de impresión
        /// </summary>
        [DataMember]
        public bool IsSpecialImpression { get; set; }

        /// <summary>
        /// Gets or sets Acción a realizar (CRUD)
        /// </summary>
        [DataMember]
        public string Status { get; set; }
    }
}
