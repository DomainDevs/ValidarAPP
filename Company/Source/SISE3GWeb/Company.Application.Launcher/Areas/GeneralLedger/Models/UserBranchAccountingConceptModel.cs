//Sistran FWK
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class UserBranchAccountingConceptModel
    {

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// BranchAccountingConceptId
        /// </summary>
        public int BranchAccountingConceptId { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }

        //public BranchAccountingConceptModel branchAccountingConceptModel { get; set; }

        //public List<BranchAccountingConceptModel> BranchAccountingConceptModels { get; set; }
    }
}