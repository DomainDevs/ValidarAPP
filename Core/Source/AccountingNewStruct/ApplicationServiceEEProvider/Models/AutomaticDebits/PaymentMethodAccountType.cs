using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.AutomaticDebits
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
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// BankAccountType: Tipo de Cuenta del banco
        /// </summary>        
        [DataMember]
        public BankAccountType BankAccountType { get; set; }
        
        /// <summary>
        /// SmallDescriptionDebit: Codigo Debito
        /// </summary>        
        [DataMember]
        public string SmallDescriptionDebit { get; set; }

        
    }
}
