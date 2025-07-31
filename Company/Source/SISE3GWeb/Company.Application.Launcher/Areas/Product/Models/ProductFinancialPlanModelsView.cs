using Sistran.Company.Application.ModelServices.Enums;
using System.ComponentModel.DataAnnotations;
namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class ProductFinancialPlanModelsView
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
        /// 
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Estado del objeto
        /// </summary>        
        public StatusTypeService StatusTypeService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CurrencyModelsView Currency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PaymentMethodModelsView PaymentMethod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PaymentScheduleModelsView PaymentSchedule { get; set; }
    }
}