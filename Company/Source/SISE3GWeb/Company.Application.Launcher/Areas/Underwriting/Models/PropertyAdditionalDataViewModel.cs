using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Location.PropertyServices.Models;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class PropertyAdditionalDataViewModel
    {
        /// <summary>
        /// Asegurado secundario
        /// </summary>
        public CompanyIssuanceInsured SecondaryInsured { get; set; }

        /// <summary>
        /// Id de asegurado
        /// </summary>
        public int? InsuredId { get; set; }

        /// <summary>
        /// Nombre de asegurado
        /// </summary>
        public string InsuredName { get; set; }

        /// <summary>
        /// años de construccion
        /// </summary>        
        public int ConstructionYear { get; set; }

        /// <summary>
        /// edad del riesgo
        /// </summary>     
        public int RiskAge { get; set; }

        /// <summary>
        /// Tipo de construccion
        /// </summary>
        public int ConstructionType { get; set; }

        /// <summary>
        /// Numero de piso
        /// </summary>      
        public int FloorNumber { get; set; }

        /// <summary>
        /// Tipo de riesgo
        /// </summary>
        public int RiskType { get; set; }

        /// <summary>
        /// Uso del riesgo
        /// </summary>
        public int RiskUse { get; set; }

        /// <summary>
        /// Latitud
        /// </summary>
        [Display(Name = "LabelLatitude", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"([-]?[\d]\w*([.|,]\d\w*)?)", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Longitud
        /// </summary>
        [Display(Name = "LabelLongitude", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"([-]?[\d]\w*([.|,]\d\w*)?)", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal? Longitude { get; set; }

        /// <summary>
        /// EML
        /// </summary>   
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [RegularExpression(@"([\d]\w*([.|,]\d\w*)?)", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]       
        public decimal? PML { get; set; }

        /// <summary>
        /// Bloque
        /// </summary>
        public string Square { get; set; }
        /// <summary>
        /// riesgo Principal
        /// </summary>
        public bool PrincipalRisk { get; set; }
        /// <summary>
        /// Tipo de Vivienda
        /// </summary>
        public int HomeType { get; set; }
        /// <summary>
        /// Uso de Vivienda
        /// </summary>
        public int HomeUse { get; set; }
        /// <summary>
        /// Periodo Ajuste Prima de Depósito
        /// </summary>
        public int BillingPeriodDepositPremium { get; set; }
        /// <summary>
        /// Periodo Declaración
        /// </summary>
        public DeclarationPeriod DeclarationPeriod { get; set; }

        /// <summary>
        /// Periodo AJUSTE
        /// </summary>
        public AdjustPeriod AdjustPeriod { get; set; }
    }
}
