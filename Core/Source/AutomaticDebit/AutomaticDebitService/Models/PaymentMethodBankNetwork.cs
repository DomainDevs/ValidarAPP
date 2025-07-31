using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
//Sistran

using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AutomaticDebitServices.Models
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
        public PaymentMethodDTO PaymentMethod { get; set; }

        /// <summary>
        /// BankAccountCompany: Cuenta Bancaria Compañia
        /// </summary>        
        [DataMember]
        public BankAccountCompanyDTO BankAccountCompany { get; set; }
        
        /// <summary>
        /// ToGenerate: Permite Generar
        /// </summary>        
        [DataMember]
        public bool ToGenerate { get; set; }

       
        
    }
}
