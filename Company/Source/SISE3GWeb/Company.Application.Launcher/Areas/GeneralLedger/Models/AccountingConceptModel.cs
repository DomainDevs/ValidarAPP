//Sistran FWK
using Sistran.Core.Framework.UIF.Web.Resources;
using System.ComponentModel.DataAnnotations;


namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AccountingConceptModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id de la cuenta contable
        /// </summary>
        public int AccountingAccountId { get; set; }


        /// <summary>
        /// Description
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string Description { get; set; }

        
        /// <summary>
        /// AgentEnabled
        /// </summary>
        public bool AgentEnabled { get; set; }

        /// <summary>
        /// CoInsurancedEnabled
        /// </summary>
        public bool CoInsurancedEnabled { get; set; }
   
        /// <summary>
        /// ReInsuranceEnabled
        /// </summary>
        public bool ReInsuranceEnabled { get; set; }
     
        /// <summary>
        /// InsuredEnabled
        /// </summary>
        public bool InsuredEnabled { get; set; }

        /// <summary>
        /// InsuredEnabled
        /// </summary>
        public bool ItemEnabled { get; set; }

    }
}