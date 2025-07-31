//Sistran FWK
using Sistran.Core.Framework.UIF.Web.Resources;

using System.ComponentModel.DataAnnotations;


namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AnalysisConceptKeyModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// AnalysisConceptId
        /// </summary>
        public int AnalysisConceptId { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// ColumnName
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "ColumnName")]
        public string ColumnName { get; set; }

        /// <summary>
        /// TableName
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "TableName")]
        public string TableName { get; set; }
    }
}