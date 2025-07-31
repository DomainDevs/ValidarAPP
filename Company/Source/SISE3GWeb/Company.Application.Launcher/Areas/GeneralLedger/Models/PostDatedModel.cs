using System;
using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;


namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class PostDatedModel
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        // Tipo de valor
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Type")]
        public int PostDateTypeId { get; set; }

        /// <summary>
        // Tipo de valor
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Type")]
        public string PostDateTypeDescription { get; set; }

        /// <summary>
        // Número de valor
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "DocumentNumber")]
        public int DocumentNumber { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Currency")]
        public int CurrencyId { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Currency")]
        public string CurrencyDescription { get; set; }

        /// <summary>
        /// Cambio
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Exchange")]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Importe moneda de emision
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "IssueAmount")]
        public Decimal IssueAmount { get; set; }

        /// <summary>
        /// Importe moneda local
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "LocalAmount")]
        public Decimal LocalAmount { get; set; }
    }
}