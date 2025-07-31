using System.Runtime.Serialization;

using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments
{
    

    

    /// <summary>
    ///     Pago en Transferencia
    /// </summary>
    [DataContract]
    public class Transfer : Check
    {
        /// <summary>
        ///     Cuenta y banco Receptor
        /// </summary>
        [DataMember]
        public BankAccountPerson ReceivingAccount { get; set; }
		
    }
}
