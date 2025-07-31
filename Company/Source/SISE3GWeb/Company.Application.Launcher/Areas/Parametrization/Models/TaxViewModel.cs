using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    /// <summary>
    /// Modelo de las propiedades de Impuestos
    /// </summary>    
    public class TaxViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id del Impuesto
        /// </summary>
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "CodeTax")]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el descripcion del Impuesto
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(50)]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Description")]
        public string Description { get; set; }


        /// <summary>
        /// Obtiene o establece la abreviatura del Impuesto
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(15)]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Abbreviation")]
        public string Abbreviation { get; set; }

        /// <summary>
        /// Obtiene o establece la vigencia desde
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "CurrentFromTax")]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de Tasa
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "RateType")]
        public int RateTypeTax { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de Tasa adicional
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "RateTypeAdditional")]
        public int RateTypeAdditionalTax { get; set; }


        /// <summary>
        /// Obtiene o establece el Impuesto Condición Base de Cálculo
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "ConditionBaseTax")]
        public int ConditionBaseTax { get; set; }

        /// <summary>
        /// Obtiene o establece Cálculo por lo Devengado
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "AccrualCalculation")]
        public bool AccrualCalculation { get; set; }


        /// <summary>
        /// Obtiene o establece Cálculo sobre excedente de base mínima
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "MinimumBaseCalculation")]
        public bool MinimumBaseCalculation { get; set; }

        /// <summary>
        /// Obtiene o establece Cálculo de tasa adicional sobre excedente
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "RateAdditionalCalculation")]
        public bool RateAdditionalCalculation { get; set; }


        /// <summary>
        /// Obtiene o establece el esta Habilitado del Impuesto
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Obtiene o establece el campo retención
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Retention")]
        public bool Retention { get; set; }


        /// <summary>
        /// Obtiene o establece el tipo de Tasa adicional
        /// </summary>   
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "BaseTaxWithholding")]
        public int BaseTaxWithholding { get; set; }


        /// <summary>
        /// Obtiene o establece el tipo de Tasa adicional
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "FeesApplies")]
        public List<int> FeesApplies { get; set; }


        /// <summary>
        /// Obtiene o establece el Rol
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Roles")]
        public List<int> Roles { get; set; }
    }

    /// <summary>
    /// Modelo de las propiedades de Categoria-Impuestos
    /// </summary>    
    public class CategoryTaxViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la Categoria
        /// </summary>
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "CodeTaxCategory")]
        public int IdCategory { get; set; }

        /// <summary>
        /// Obtiene o establece el Id del Impuesto
        /// </summary>
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "CodeTax")]
        public int IdTax { get; set; }

        /// <summary>
        /// Obtiene o establece el descripcion de la Categoria
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(50)]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Description")]
        public string DescriptionCategory { get; set; }
    }

    /// <summary>
    /// Modelo de las propiedades de Condiciones Impositivas-Impuestos
    /// </summary>    
    public class ConditionTaxViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la Condición
        /// </summary>
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "CodeTax")]
        public int IdCondition { get; set; }

        /// <summary>
        /// Obtiene o establece el descripcion de la Condición
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(50)]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "DescriptionCondition")]
        public string DescriptionCondition { get; set; }

        /// <summary>
        /// Obtiene o establece el estado de  Tasa Nacional de la Condición
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "NationalRate")]
        public bool NationalRate { get; set; }

        /// <summary>
        /// Obtiene o establece el estado de Independiente de la Condición
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Independent")]
        public bool Independent { get; set; }

        /// <summary>
        /// Obtiene o establece el esta Habilitado  de la Condición
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Obtiene o establece el Id del Impuesto
        /// </summary>
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "CodeTax")]
        public int IdTax { get; set; }
    }


    /// <summary>
    /// Modelo de las propiedades de Condiciones Impositivas-Impuestos
    /// </summary>    
    public class RateTaxViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id
        /// </summary>
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "CodeTax")]
        public int IdRate { get; set; }


        /// <summary>
        /// Obtiene o establece la vigencia desde
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "CurrentFromTax")]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Obtiene o establece la Tasa
        /// </summary>   
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]        
        [Display(Name = "Rate", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal Rate { get; set; }

        /// <summary>
        /// Obtiene o establece la Tasa Adicional
        /// </summary>   
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "RateAdditional", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal RateAdditional { get; set; }

        /// <summary>
        /// Obtiene o establece la Mínima Base Imponible
        /// </summary>   
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "MinimumTaxableBase", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal MinimumTaxableBase { get; set; }


        /// <summary>
        /// Obtiene o establece la Mínima Base Imponible Adicional
        /// </summary>   
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "MinimumAdditionalTaxableBase", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal MinimumAdditionalTaxableBase { get; set; }

        /// <summary>
        /// Obtiene o establece la Mínima Base Imponible Adicional
        /// </summary>   
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "Minimum", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal Minimum { get; set; }

        /// <summary>
        /// Obtiene o establece la Mínima Base Imponible Adicional
        /// </summary>   
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "AdditionalMinimum", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal AdditionalMinimum { get; set; }


        /// <summary>
        /// Obtiene o establece Impuesto Básico Incluido en Base Adicional 
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "BasicTaxIncludedAdditionalBase")]
        public bool BasicTaxIncludedAdditionalBase { get; set; }


        /// <summary>
        /// Obtiene o establece la Condición Impositiva
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "TaxCondition")]
        public int TaxCondition { get; set; }

        
        /// <summary>
        /// Obtiene o establece Condición
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Category")]
        public int TaxCategory { get; set; }

        /// <summary>
        /// Obtiene o establece Ciudad
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "City")]
        public int City { get; set; }

        /// <summary>
        /// Obtiene o establece Municipio
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "TitleState")]
        public int State { get; set; }

        /// <summary>
        /// Obtiene o establece Country
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Country")]
        public int Country { get; set; }

        /// <summary>
        /// Obtiene o establece Sucursal
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "TechnicalBranch")]
        public int LineBusiness { get; set; }

        /// <summary>
        /// Obtiene o establece Ramo tecnico
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Branch")]
        public int TechnicalBranch { get; set; }

        /// <summary>
        /// Obtiene o establece la cobertura
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Coverage")]
        public int Coverage { get; set; }

        /// <summary>
        /// Obtiene o establece el Id de Impuesto
        /// </summary>
        public int IdTax { get; set; }


        /// <summary>
        /// Obtiene o establece la actividad economica
        /// </summary>
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "EconomicActivity")]
        public string EconomicActivity { get; set; } 
        
        /// <summary>
        /// Obtiene o establece el Id de actividad economica
        /// </summary>
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "EconomicActivity")]
        public int EconomicActivityId { get; set; }

    }
    /// <summary>
    /// Modelo de las propiedades de Busqueda Avanzada de Tasas de Impuesto
    /// </summary>    
    public class RateTaxAdvancedSearchViewModel
    {
        /// <summary>
        /// Obtiene o establece Condición
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Condition")]
        public int Condition { get; set; }

        /// <summary>
        /// Obtiene o establece Condición
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Category")]
        public int CategoryAdvanced { get; set; }


        /// <summary>
        /// Obtiene o establece Municipio
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Town")]
        public int Town { get; set; }

        /// <summary>
        /// Obtiene o establece Sucursal
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Office")]
        public int Office { get; set; }


        /// <summary>
        /// Obtiene o establece Ramo tecnico
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "TechnicalBranch")]
        public int TechnicalBranch { get; set; }

        /// <summary>
        /// Obtiene o establece Ramo tecnico
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Coverages")]
        public List<int> Coverages { get; set; }

        /// <summary>
        /// Obtiene o establece el estado Habilitado
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Enabled")]
        public bool Enabled { get; set; }
    }

    public class TaxbyIndividualsViewModel
    {
        /// <summary>
        /// Obtiene o establece Condición
        /// </summary> 
        /// 
        [Display(Name = "LabelDateStart", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime? Datefrom { get; set; }

        [Display(Name = "DateUntil", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime? DateUntil { get; set; }

        [Display(Name = "OfficialBulletinDate", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime? OfficialBulletinDate { get; set; }

        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "ExtentPercentage", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? ExtentPercentage { get; set; }

       





    }


}