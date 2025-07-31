using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.AutomaticDebits
{
    /// <summary>
    /// PaymentMethodBankNetwork: Metodo de Pago de la Red
    /// </summary>
    [DataContract]
    public class PaymentMethodBankNetworkDTO
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
        public BankNetworkDTO BankNetwork { get; set; }

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
