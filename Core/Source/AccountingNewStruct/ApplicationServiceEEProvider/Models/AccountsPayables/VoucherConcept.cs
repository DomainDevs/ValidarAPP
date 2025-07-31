using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables
{
    [DataContract]
    public class VoucherConcept
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// AccountingConcept 
        /// </summary>        
        [DataMember]
        public GeneralLedgerServices.EEProvider.Models.AccountingConcepts.AccountingConcept AccountingConcept { get; set; }

        /// <summary>
        /// CostCenter 
        /// </summary>        
        [DataMember]
        public CostCenter CostCenter { get; set; }

        /// <summary>
        /// Amount 
        /// </summary>        
        [DataMember]
        public Amount Amount { get; set; }

        /// <summary>
        /// VoucherConceptTaxes
        /// </summary>
        [DataMember]
        public List<VoucherConceptTax> VoucherConceptTaxes { get; set; }
    }
}
