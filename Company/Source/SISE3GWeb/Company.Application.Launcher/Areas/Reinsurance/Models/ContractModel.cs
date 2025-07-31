using System.ComponentModel.DataAnnotations;

//Sistran
using Sistran.Core.Application.ReinsuranceServices.DTOs;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models
{
    public class ContractModel
    {
        public int ContractId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(15, ErrorMessage = "El campo Descripción Reducida debe tener mínimo {2} y máximo {1} caracteres", MinimumLength = 3)]
        public string SmallDescription { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "El campo Descripción debe tener mínimo {2} y máximo {1} caracteres", MinimumLength = 10)]                        
        public string Description { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string DateFrom { get; set; }
       

        [Required]
        [DataType(DataType.Date)]
        public string DateTo { get; set; }

        [Required]
        public string CurrencyId { get; set; }

        [Required]
        public ContractTypeDTO ContractType { get; set; }

        [Required]
        public int ReleaseTimeReserve { get; set; }

        public AffectationTypeDTO AffectationType  { get; set; }
        
        /// <summary>
        /// Tipo de Restablecimiento
        /// </summary>
        public ResettlementTypeDTO ReestablishmentType { get; set; }

        /// <summary>
        /// Importe Estimado de prima
        /// </summary>
        [Display(Name = "EPI")]
        public string PremiumAmount { get; set; }

        public EPITypeDTO EPIType { get; set; }

        public bool Status { get; set; }

        public string Grouper { get; set; }
              
        public string CoInsurancePercentage { get; set; }

        /// <summary>
        /// Cantidad de Riesgo/ Vidas Afectadas
        /// </summary>
        public int QuantityRisk { get; set; }

        
        [DataType(DataType.Date)]
        public string EstimatedDate { get; set; }






    }
}