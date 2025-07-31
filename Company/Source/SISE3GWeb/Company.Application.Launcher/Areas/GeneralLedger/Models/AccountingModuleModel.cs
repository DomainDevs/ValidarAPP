using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AccountingModuleModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int AccountingModuleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "MovementDescription")]
        public string AccountingModuleDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = "Año tope del cierre contable actual")]
        [DataType(DataType.Text)]
        [StringLength(4, ErrorMessage = "El {0} debe tener {2} caracteres de longitud", MinimumLength = 4)]
        public string MaximumYear { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Mes tope del cierre contable actual")]
        public string MaximumMonth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Mes tope del cierre contable actual")]
        public string MaximumMonthDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = "Ultimo número de asiento tipificado")]
        public int CategorizedEntryLastNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = "Año tope del cierre contable ultimo registrado")]
        [DataType(DataType.Text)]
        [StringLength(4, ErrorMessage = "El {0} debe tener {2} caracteres de longitud", MinimumLength = 4)]
        public string ClosureLastYear { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Mes tope del cierre contable ultimo registrado")]
        public string ClosureLastMonth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Mes tope del cierre contable actual")]
        public string ClosureLastMonthDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = "Año tope del cierre contable fuera de línea")]
        [DataType(DataType.Text)]
        [StringLength(4, ErrorMessage = "El {0} debe tener {2} caracteres de longitud", MinimumLength = 4)]
        public string MaximumYearOffline { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Mes tope del cierre contable  fuera de línea")]
        public string MaximumMonthOffline { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Mes tope del cierre contable actual")]
        public string MaximumMonthOfflineDescription { get; set; }
    }
}