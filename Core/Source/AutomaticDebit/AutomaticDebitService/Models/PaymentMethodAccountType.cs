
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using System.Runtime.Serialization;

//Sistran

namespace Sistran.Core.Application.AutomaticDebitServices.Models
{
    /// <summary>
    /// PaymentMethodBankNetwork: Metodo de Pago de la Red
    /// </summary>
    [DataContract]
    public class PaymentMethodAccountType
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// PaymentMethod: Metodo de Pago
        /// </summary>        
        [DataMember]
        public PaymentMethodDTO PaymentMethod { get; set; }

        /// <summary>
        /// BankAccountType: Tipo de Cuenta del banco
        /// </summary>        
        [DataMember]
        public BankAccountTypeDTO BankAccountType { get; set; }
        
        /// <summary>
        /// SmallDescriptionDebit: Codigo Debito
        /// </summary>        
        [DataMember]
        public string SmallDescriptionDebit { get; set; }

        
    }
}
