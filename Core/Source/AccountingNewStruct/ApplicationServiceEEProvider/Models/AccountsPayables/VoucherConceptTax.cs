using Sistran.Core.Application.TaxServices.Models;
using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables
{
    [DataContract]
    public class VoucherConceptTax
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
        public Tax Tax { get; set; }

        /// <summary>
        ///  TaxCondition 
        /// </summary>        
        [DataMember]
        public TaxCondition TaxCondition { get; set; }

        /// <summary>
        ///  TaxCategory 
        /// </summary>        
        [DataMember]
        public TaxCategory TaxCategory { get; set; }

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
