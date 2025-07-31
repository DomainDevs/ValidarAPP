using System.Runtime.Serialization;
using System.Collections.Generic;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingRules
{
    [DataContract]
    public class Result
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
        public AccountingNatures AccountingNature { get; set; }

        /// <summary>
        ///     AccountingAccountMasks : Formatos de Cuenta Contable
        /// </summary>
        [DataMember]
        public List<AccountingAccountMask> AccountingAccountMasks { get; set; }

        /// <summary>
        ///     AccountingAccount : Cuenta Contable: Se dejara string hasta validar el modelo de AccountingAccount
        /// </summary>
        [DataMember]
        public string AccountingAccount { get; set; }

        /// <summary>
        ///     Parameter : Valor del parametro de ingreso
        /// </summary>
        [DataMember]
        public Parameter Parameter { get; set; }
    }
}