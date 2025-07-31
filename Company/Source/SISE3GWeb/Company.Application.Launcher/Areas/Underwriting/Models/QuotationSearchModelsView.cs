using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class QuotationSearchModelsView
    {
        /// <summary>
        /// Identificador
        /// </summary>        
        public int QuotationNumber { get; set; }

        /// <summary>
        /// Versión de la Cotización
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Ramo comercial
        /// </summary>
        public string PrefixCommercial { get; set; }

        /// <summary>
        /// Asegurado
        /// </summary>
        public string Insured { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Moneda de la emisión
        /// </summary>
        public string CurrencyIssuance { get; set; }

        /// <summary>
        /// Prima total
        /// </summary>
        public string TotalPremium { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Fecha Cotizacion
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Dias
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// Intermediario principal
        /// </summary>
        public string AgentPrincipal { get; set; }

        /// <summary>
        /// Producto    
        /// </summary>
        public string Product { get; set; }
        
        /// <summary>
        /// Id Operacion
        /// </summary>
        public int OperationId { get; set; }
    }
}