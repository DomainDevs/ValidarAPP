using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    public class ModificationViewModel : EndorsementViewModel
    {
        /// <summary>
        /// Texto Precatalogado
        /// </summary>      
        public string TextPrecataloged { get; set; }
        /// <summary>
        /// Plan de pagos Precatalogado
        /// </summary>   
        public PaymentPlan PaymentPlan { get; set; }

        /// <summary>
        /// Plan de pagos Precatalogado
        /// </summary>   
        public List<CompanyClause> Clauses { get; set; }

        /// <summary>
        /// Placa
        /// </summary>       
        public string DescriptionRisk { get; set; }

        /// <summary>
        /// SubCoveredRiskType
        /// </summary>       
        public int SubCoveredRiskType { get; set; }

        /// <summary>
        /// SubCoveredRiskType
        /// </summary>  
        public bool IsCollective { get; set; }

        [Range(1, 300)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorModificationType")]
        public int ModificationTypeId { get; set; }
    }
}