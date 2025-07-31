using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class PolicySearchModelsView
    {
        /// <summary>
        /// Numero Poliza
        /// </summary>        
        public int PolicyNumber { get; set; }

        /// <summary>
        /// Endoso
        /// </summary>
        public int Endorsement { get; set; }

        /// <summary>
        /// Ramo comercial
        /// </summary>
        public string PrefixCommercial { get; set; }

        /// <summary>
        /// Tipo de Endoso
        /// </summary>
        public string EndorsementType { get; set; }
        
        /// <summary>
        /// Asegurado
        /// </summary>
        public string Insured { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Moneda de Emision
        /// </summary>
        public string IssueCurrency { get; set; }

        /// <summary>
        /// Total Prima
        /// </summary>
        public string TotalPremium { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// fecha Emision
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Intermediario principal
        /// </summary>
        public string AgentPrincipal { get; set; }

        /// <summary>
        /// Product
        /// </summary>
        public string Product { get; set; }
    }
}