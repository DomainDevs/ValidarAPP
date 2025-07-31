using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

//Sistran


namespace Sistran.Core.Application.AutomaticDebitServices.Models
{
    /// <summary>
    /// AutomaticDebit: Debito Automatico(Lote)
    /// </summary>
    [DataContract]
    public class AutomaticDebit
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// BankNetwork: Red 
        /// </summary>        
        [DataMember]
        public BankNetwork BankNetwork { get; set; }

        /// <summary>
        /// Description: Descripción 
        /// </summary>        
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Date: Fecha 
        /// </summary>        
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// RetriesNumber: Numero de Reintentos
        /// </summary>        
        [DataMember]
        public int RetriesNumber { get; set; }

        /// <summary>
        /// AutomaticDebitStatus: Estado del Debito Automatico
        /// </summary>        
        [DataMember]
        public AutomaticDebitStatus AutomaticDebitStatus { get; set; }

        /// <summary>
        /// User: Usuario
        /// </summary>        
        [DataMember]
        public int UserId { get; set; }
        
        /// <summary>
        /// Coupons: Cupones, Items de Debitos Automaticos
        /// </summary>        
        [DataMember]
        public List<Coupon> Coupons { get; set; }
        
        
    }
}
