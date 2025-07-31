using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables
{
    [DataContract]
    public class VoucherConceptTaxDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///  Tax
        /// </summary>        
        [DataMember]
        public TaxDTO Tax { get; set; }

        /// <summary>
        ///  TaxCondition 
        /// </summary>        
        [DataMember]
        public TaxConditionDTO TaxCondition { get; set; }

        /// <summary>
        ///  TaxCategory 
        /// </summary>        
        [DataMember]
        public TaxCategoryDTO TaxCategory { get; set; }

        /// <summary>
        /// TaxeRate
        /// </summary>        
        [DataMember]
        public decimal TaxeRate { get; set; }

        /// <summary>
        /// TaxeBaseAmount 
        /// </summary>        
        [DataMember]
        public decimal TaxeBaseAmount { get; set; }

        /// <summary>
        /// TaxValue 
        /// </summary>        
        [DataMember]
        public decimal TaxValue { get; set; }
    }
      
}
