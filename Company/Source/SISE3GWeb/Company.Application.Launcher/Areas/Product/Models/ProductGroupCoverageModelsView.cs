using Sistran.Company.Application.ModelServices.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class ProductGroupCoverageModelsView
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
        /// Id del objeto ramo
        /// </summary>     
        public int RiskTypeId { get; set; }

        /// <summary>
        /// Objetos del seguro asociados al grupo de cobertura
        /// </summary>
        public List<InsuredObjectModelsView> InsuredObjects { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<FormsModelsView> Form { get; set; }

        /// <summary>
        /// Estado del objeto
        /// </summary>        
        public StatusTypeService StatusTypeService { get; set; }
    }
}