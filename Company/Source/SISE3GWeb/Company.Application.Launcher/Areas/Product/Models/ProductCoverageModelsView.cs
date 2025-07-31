using Sistran.Company.Application.ModelServices.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class ProductCoverageModelsView
    {
        /// <summary>
        /// Identificador
        /// </summary>     
        public int Id { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>     
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSublimit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? RuleSetId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? PosRuleSetId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal SublimitPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? MainCoverageId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? MainCoveragePercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? ScriptId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SubLineBusinessDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SubLineBusinessId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LineBusinessDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int LineBusinessId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int InsuredObjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InsuredObjectIdDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPremiumMin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool NoCalculate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<DeductByCoverProductModelView> DeductiblesCoverage { get; set; }

        /// <summary>
        /// Estado del objeto
        /// </summary>        
        public StatusTypeService StatusTypeService { get; set; }
    }
}