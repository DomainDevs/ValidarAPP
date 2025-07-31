using Sistran.Core.Application.UnderwritingServices.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Collective.Models
{
    public class AddMassiveModelView
    {
        /// <summary>
        /// IdLoad
        /// </summary>
        public int IdLoad { get; set; }

        /// <summary>
        /// NameLoad
        /// </summary>
        [Required]
        [MaxLength(60)]
        public string NameLoad { get; set; }

        /// <summary>
        /// IdTypeLoad
        /// </summary>
        [Required]
        public int IdTypeLoad { get; set; }

        /// <summary>
        /// IdBranch
        /// </summary>
        [Required]
        public int IdBranch { get; set; }

        /// <summary>
        /// SalesPoint
        /// </summary>
        public int SalesPoint { get; set; }

        /// <summary>
        /// IdPrefixCommercial
        /// </summary>
        [Required]
        public int IdPrefixCommercial { get; set; }

        /// <summary>
        /// IdCommercialProduct
        /// </summary>
        [Required]
        public int IdCommercialProduct { get; set; }

        /// <summary>
        /// MainAgentName
        /// </summary>
        [Required]
        public string MainAgentName { get; set; }

        /// <summary>
        /// IdAgencyMainAgent
        /// </summary>
        [Required]
        public int IdAgencyMainAgent { get; set; }

        /// <summary>
        /// ExcelFile
        /// </summary>
        [Required]
        public string ExcelFile { get; set; }

        /// <summary>
        /// IdExcelPage
        /// </summary>
        [Required]
        public int IdExcelPage { get; set; }

        /// <summary>
        /// IdRequest
        /// </summary>
        public string IdRequest { get; set; }
        
        /// <summary>
        /// DescriptionRequest
        /// </summary>
        public string DescriptionRequest { get; set; }

        /// <summary>
        /// CountRisk
        /// </summary>
        public int CountRisk { get; set; }

        /// <summary>
        /// CountRisk
        /// </summary>
        public int CountRiskErrors { get; set; }

        /// <summary>
        /// CountRisk
        /// </summary>
        public int CountRiskProcessed { get; set; }

        /// <summary>
        /// Tipo de negocio
        /// </summary>        
        public BusinessType BusinessType { get; set; }

        /// <summary>
        /// Estado del cargue
        /// </summary>                     
        public string State { get; set; }

        public int StateId {get; set;}

        public int TemporalId { get; set; }
    }
}