using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules
{
    [DataContract]
    public class ResultDTO
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     AccountingNature : Naturaleza Contable  
        /// </summary>
        [DataMember]
        public int AccountingNature { get; set; }

        /// <summary>
        ///     AccountingAccountMasks : Formatos de Cuenta Contable
        /// </summary>
        [DataMember]
        public List<AccountingAccountMaskDTO> AccountingAccountMasks { get; set; }

        /// <summary>
        ///     AccountingAccount : Cuenta Contable: Se dejara string hasta validar el modelo de AccountingAccount
        /// </summary>
        [DataMember]
        public string AccountingAccount { get; set; }

        /// <summary>
        ///     Parameter : Valor del parametro de ingreso
        /// </summary>
        [DataMember]
        public ParameterDTO Parameter { get; set; }
    }
}