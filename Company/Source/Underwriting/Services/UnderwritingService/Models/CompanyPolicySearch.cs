using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{

    [DataContract]
    public class CompanyPolicySearch
    {   
        /// <summary>
        /// Numero Poliza
        /// </summary>        
        [DataMember]
        public int PolicyNumber { get; set; }

        /// <summary>
        /// Endoso
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }

        /// <summary>
        /// Ramo comercial
        /// </summary>
        [DataMember]
        public string PrefixCommercial { get; set; }

        /// <summary>
        /// Tipo de Endoso
        /// </summary>
        [DataMember]
        public string EndorsementType { get; set; }

        /// <summary>
        /// Asegurado
        /// </summary>
        [DataMember]
        public string Insured { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        [DataMember]
        public string Branch { get; set; }

        /// <summary>
        /// Moneda de Emision
        /// </summary>
        [DataMember]
        public string IssueCurrency { get; set; }

        /// <summary>
        /// Total Prima
        /// </summary>
        [DataMember]
        public string TotalPremium { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [DataMember]
        public string User { get; set; }

        /// <summary>
        /// fecha Emision
        /// </summary>
        [DataMember]
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Intermediario principal
        /// </summary>
        [DataMember]
        public string AgentPrincipal { get; set; }

        /// <summary>
        /// Product
        /// </summary>
        [DataMember]
        public string Product { get; set; }
    }
}
