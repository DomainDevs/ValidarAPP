//Sistran FWK
//using Sistran.Core.Framework.UIF.Web.Resources;
//using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class BranchAccountingConceptModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// BranchId
        /// </summary>
        public int BranchId { get; set; }

        /// <summary>
        /// MovementTypeId
        /// </summary>
        public int MovementTypeId { get; set; }

        /// <summary>
        /// ConceptSourceId
        /// </summary>
        public int ConceptSourceId { get; set; }

        public List<AccountingConceptModel> AccountingConceptModels { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }


        ///// <summary>
        ///// AccountingConceptId
        ///// </summary>
        //public int AccountingConceptId { get; set; }
    }

    


}