
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// DepositPremiumTransaction:   Primas en Deposito
    /// </summary>
    [DataContract]
    public class DepositPremiumTransactionDTO
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
        public CollectDTO Collect { get; set; }

        /// <summary>
        /// Fecha de generacion prima en deposito   
        /// </summary>        
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Amount: Importe  
        /// </summary>        
        [DataMember]
        public AmountDTO Amount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRateDTO ExchangeRate { get; set; }

        /// <summary>
        /// LocalAmount: Importe  
        /// </summary>        
        [DataMember]
        public AmountDTO LocalAmount { get; set; }

        /// <summary>
        /// UsedAmounts: Importes usados del Importe en exceso, debe ser menor o igual al importe en exceso
        /// </summary>        
        [DataMember]
        public List<AmountDTO> UsedAmounts { get; set; }
    }
}
