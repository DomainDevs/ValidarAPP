//Sistran FWK
using Sistran.Core.Framework.UIF.Web.Resources;
using System.ComponentModel.DataAnnotations;


namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class MovementTypeModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ConceptSourceId
        /// </summary>
        public int ConceptSourceId { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string Description { get; set; }

    }
}