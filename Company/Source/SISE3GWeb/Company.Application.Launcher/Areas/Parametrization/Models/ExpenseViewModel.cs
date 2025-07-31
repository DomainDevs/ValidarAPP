using Sistran.Core.Application.EntityServices.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class ExpenseViewModel
    {
        public int id { get; set; }

        [StringLength(30, MinimumLength = 3, ErrorMessage = "El Id es demasiado corto, minimo 3 digitos.")]
        public string IdExpense { get; set; }

        [Required]
        [StringLength(15)]
        [RegularExpression(@"^[0-9A-ZñÑáéíóúÁÉÍÓÚ' \-_\&\.\(\)\[\]]*$", ErrorMessage = "Caracter no permitido")]
        public string Description { get; set; }

        [Required]
        [StringLength(3)]
        [RegularExpression(@"^[0-9A-ZñÑáéíóúÁÉÍÓÚ'\-_\&\.\(\)\[\]]*$", ErrorMessage = "Caracter no permitido")]
        public string Abbreviation { get; set; }

        public bool Mandatory { get; set; }

        [Required]
        public string ExecutionType { get; set; }

        public bool InitiallyIncluded { get; set; }

        public int RateType { get; set; }

        public string RateTypeName { get; set; }
        
        public int? RuleSet { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[0-9A-ZñÑáéíóúÁÉÍÓÚ': \-_\&\.\(\)\[\]]*$", ErrorMessage = "Caracter no permitido")]
        public string RuleSetName { get; set; }

        public decimal Rate { get; set; }

        public int ComponentClass { get; set; }

        public int ComponentType { get; set; }

        public int StatusTypeService { get; set; }

    }
}