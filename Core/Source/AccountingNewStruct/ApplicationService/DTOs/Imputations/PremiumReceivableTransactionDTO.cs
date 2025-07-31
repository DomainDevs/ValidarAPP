using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// PremiumsReceivable:  Primas por cobrar
    /// </summary>
    [DataContract]
    public class PremiumReceivableTransactionDTO : TransactionTypeDTO
    {
        /// <summary>
        /// PremiumReceivableItems:Lista de polizas de la imputación
        /// </summary>        
        [DataMember]
        public List<PremiumReceivableTransactionItemDTO> PremiumReceivableItems { get; set; }
        /// <summary>
        /// PremiumReceivableItems:Lista de comisiones de la imputación
        /// </summary>        
        [DataMember]
        public List<ApplicationPremiumCommisionDTO> CommissionsDiscounted { get; set; }
    }
}
