using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.AutomaticDebits
{
    /// <summary>
    /// PaymentMethodBankNetwork: Metodo de Pago de la Red
    /// </summary>
    [DataContract]
    public class PaymentMethodBankNetwork
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
        /// PaymentMethod: Metodo de Pago
        /// </summary>        
        [DataMember]
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// BankAccountCompany: Cuenta Bancaria Compañia
        /// </summary>        
        [DataMember]
        public BankAccountCompany BankAccountCompany { get; set; }
        
        /// <summary>
        /// ToGenerate: Permite Generar
        /// </summary>        
        [DataMember]
        public bool ToGenerate { get; set; }

       
        
    }
}
