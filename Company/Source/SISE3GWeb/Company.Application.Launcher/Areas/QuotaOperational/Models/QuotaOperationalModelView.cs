using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.QuotaOperational.Models
{
    public class QuotaOperationalModelView
    {

        /// <summary>
        /// Nombre tomador
        /// </summary>
        [Display(Name = "LabelHolder", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(134)]
        public string HolderName { get; set; }

        /// <summary>
        /// Id tomador
        /// </summary>
        public int IdTomador { get; set; }

        /// <summary>
        /// Ramo tecnico
        /// </summary>
        public string LineBusiness { get; set; }

        /// <summary>
        /// Cupo Operativo
        /// </summary>
        public int OperationalQuotaValue { get; set; }
        
        /// <summary>
        /// Fecha Hasta
        /// </summary>
        public DateTime DateEnd { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        public int Money { get; set; }

        public string moneyName { get; set; }

        /// <summary>
        /// Hidden individualId
        /// </summary>
        public int individualId { get; set; }

        public int CurrencyId { get; set; }

        public decimal amountValue { get; set; }

        public int lineBusinessId { get; set; }

        public int InsuredId { get; set; }

    }
}