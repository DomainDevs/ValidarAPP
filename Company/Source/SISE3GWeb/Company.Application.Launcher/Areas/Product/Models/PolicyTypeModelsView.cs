using Sistran.Company.Application.ModelServices.Enums;
using System.Collections.Generic;

namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class PolicyTypeModelsView
    {
        /// <summary>
        /// Identificador
        /// </summary>
      
        public int Id { get; set; }

        /// <summary>
        /// Tipo de poliza
        /// </summary>
      
        public string Description { get; set; }

        /// <summary>
        /// Tipo Poliza predeterminada
        /// </summary>
       
        public bool IsDefault { get; set; }

        /// <summary>
        /// Estado del objeto
        /// </summary>        
        public StatusTypeService StatusTypeService { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public List<LimitRCModelView> LimitsRC { get; set; }
    }
}