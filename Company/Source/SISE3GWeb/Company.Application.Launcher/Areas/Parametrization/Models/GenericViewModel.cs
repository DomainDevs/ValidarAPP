
using System.ComponentModel.DataAnnotations;


namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class GenericViewModel
    {
        /// <summary>
        /// Descripcion Larga
        /// </summary>
        [Display(Name = "LabelLongDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string DescriptionLong { get; set; }

        /// <summary>
        /// Descripcion Corta
        /// </summary>
        [Display(Name = "LabelShortDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string DescriptionShort { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador fk
        /// </summary>
        public int IdC { get; set; }


        /// <summary>
        /// Identificador fk
        /// </summary>
        public int IdD { get; set; }

        /// <summary>
        /// IsAlphanumeric
        /// </summary>
        public bool IsAlphanumeric { get; set; }


    }
}
