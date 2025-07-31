using System.ComponentModel.DataAnnotations;

//Sistran
using Sistran.Core.Application.ReinsuranceServices.DTOs;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models
{
    public class ContractLevelModel
    {
        public int ContractLevelId { get; set; }

        [Required]
        public int LevelNumber { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public string ContractLimit { get; set; }
        
        [DataType(DataType.Currency)]
        //[Range(1, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public string Percentage { get; set; }  //% de asignación

        [DataType(DataType.Currency)]
        public string RetentionLimit { get; set; }

        //[RegularExpression(@"^[0-9]{1,5}(\.[0-9]{0,2})?$")]
        [DataType(DataType.Currency)]
        public string LinesNumber { get; set; }

        [DataType(DataType.Currency)]
        public string EventLimit { get; set; }

        public ContractDTO Contract { get; set; }

        public string AdjustmentPercentage { get; set; }  
        public string FixedRatePercentage { get; set; }
        public string  MinimumRatePercentage { get; set; }  
        public string MaximumRatePercentage { get; set; }  
        public string LifeRate { get; set; }  

        public int CalculationType { get; set; }
        public int ApplyOnType { get; set; }
        public string AnnualAddedLimit { get; set; }
        public int PremiumType { get; set; }
        public ContractTypeDTO ContractType { get; set; }
    }
}