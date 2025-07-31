using Sistran.Company.Application.ModelServices.Enums;
using System.ComponentModel.DataAnnotations;
namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class ProductPrefixModelsView
    {

        /// <summary>
        /// Id del objeto ramo
        /// </summary>     
        public int Id { get; set; }

        /// <summary>
        /// Descripcion del ramo
        /// </summary>     
        public string Description { get; set; }

        /// <summary>
        /// Estado del objeto
        /// </summary>        
        public StatusTypeService StatusTypeService { get; set; }
    }
}