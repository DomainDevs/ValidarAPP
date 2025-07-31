using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingClosingServices.EEProvider.Models.GeneralLedger.AccountingRules
{
    [DataContract]
    public class AccountingRulePackage 
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Modulo
        /// </summary>
        [DataMember]
        public int ModuleDateId { get; set; }

        /// <summary>
        ///     Conceptos
        /// </summary>
        [DataMember]
        public List<AccountingRule> AccountingRules { get; set; }
    }
}