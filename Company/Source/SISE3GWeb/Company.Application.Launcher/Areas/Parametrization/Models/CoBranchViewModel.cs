
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using Sistran.Core.Application.ModelServices.Enums;
    using System.ComponentModel.DataAnnotations;
    public class CoBranchViewModel
    {      
        public int AddressType { get; set; }

        /// <summary>
        /// Obtiene o establece la Description 
        /// </summary>
        [StringLength(50)]
        [Display(Name = "ErrorSelectAddress", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Adress { get; set; }       
        
        public int City { get; set; }
       
        public int Country { get; set; }
       
        public int Id { get; set; }
     
        public bool IsIssue { get; set; }

        [Display(Name = "PhoneNumber", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]                                                  
        public long PhoneNumbre { get; set; }
      
        public int PhoneType { get; set; }
      
        public int State { get; set; }

        /// <summary>
        /// Obtiene o establece el Id 
        /// </summary>
        public int BranchId { get; set; }

        /// <summary>
        /// Obtiene o establece la Description 
        /// </summary>
        [StringLength(50)]
        [Display(Name = "LabelLongDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la Description corta
        /// </summary>
        [StringLength(15)]
        [Display(Name = "LabelShortDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string SmallDescription { get; set; }

        public StatusTypeService Status { get; set; }
    }
}