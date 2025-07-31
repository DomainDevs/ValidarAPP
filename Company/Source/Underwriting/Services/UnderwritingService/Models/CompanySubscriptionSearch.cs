using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{

    [DataContract]
    public class CompanySubscriptionSearch
    {
        /// <summary>
        /// Obtiene o establece Id del tomador
        /// </summary>
        [DataMember]
        public int? HolderId { get; set; }

        /// <summary>
        /// Obtiene o establece Id del Asegurado
        /// </summary>
        [DataMember]
        public int? InsuredId { get; set; }
        
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
        public string Plate { get; set; }

        /// <summary>
        /// Obtiene o establece Motor
        /// </summary>
        [DataMember]
        public string Engine { get; set; }

        /// <summary>
        /// Obtiene o establece Chasis
        /// </summary>
        [DataMember]
        public string Chassis { get; set; }
        
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
