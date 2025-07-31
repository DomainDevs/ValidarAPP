using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables
{
    [DataContract]
    public class VoucherConceptDTO
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
        public AccountingConceptDTO AccountingConcept { get; set; }

        /// <summary>
        /// CostCenter 
        /// </summary>        
        [DataMember]
        public CostCenterDTO CostCenter { get; set; }

        /// <summary>
        /// Amount 
        /// </summary>        
        [DataMember]
        public AmountDTO Amount { get; set; }

        /// <summary>
        /// VoucherConceptTaxes
        /// </summary>
        [DataMember]
        public List<VoucherConceptTaxDTO> VoucherConceptTaxes { get; set; }
    }
}
