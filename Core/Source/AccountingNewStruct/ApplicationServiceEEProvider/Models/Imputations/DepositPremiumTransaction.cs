using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Sistran
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// DepositPremiumTransaction:   Primas en Deposito
    /// </summary>
    [DataContract]
    public class DepositPremiumTransaction
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Collect: Ingreso de  Caja
        /// </summary>        
        [DataMember]
        public Collect.Collect Collect { get; set; }

        /// <summary>
        /// Fecha de generacion prima en deposito   
        /// </summary>        
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Amount: Importe  
        /// </summary>        
        [DataMember]
        public Amount Amount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRate ExchangeRate { get; set; }

        /// <summary>
        /// LocalAmount: Importe  
        /// </summary>        
        [DataMember]
        public Amount LocalAmount { get; set; }

        /// <summary>
        /// UsedAmounts: Importes usados del Importe en exceso, debe ser menor o igual al importe en exceso
        /// </summary>        
        [DataMember]
        public List<Amount> UsedAmounts { get; set; }
    }
}
