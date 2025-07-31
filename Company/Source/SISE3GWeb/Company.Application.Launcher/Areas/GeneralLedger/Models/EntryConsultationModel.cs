using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class EntryConsultationModel
    {
        /// <summary>
        /// Sucursal
        /// </summary>
        //[Required]
        [Display(ResourceType = typeof(Global), Name = "Branch")]
        public int BranchId { get; set; }

        /// <summary>
        /// Año del asiento
        /// </summary>
        [Required]
        [Range(1980, 9999)]
        [Display(ResourceType = typeof(Global), Name = "Year")]
        public int EntryYear { get; set; }

        /// <summary>
        /// Mes del Asiento
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Month")]
        public int EntryMonth { get; set; }

        /// <summary>
        /// Número de Asiento
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "JournalEntryNumber")]
        public int TransactionNumber { get; set; }

        /// <summary>
        /// Destino -solo se usa en consulta de asiento mayor
        /// </summary>
        //[Required]
        [Display(ResourceType = typeof(Global), Name = "Destination")]
        public int DestinationId { get; set; }

        /// <summary>
        /// Tipo de Comprobante -solo se usa en consulta de asiento mayor
        /// </summary>
        //[Required]
        [Display(ResourceType = typeof(Global), Name = "VoucherType")]
        public int AccountingMovementTypeId { get; set; }
    }
}