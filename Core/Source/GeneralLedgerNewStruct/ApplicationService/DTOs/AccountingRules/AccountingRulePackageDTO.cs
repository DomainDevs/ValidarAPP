using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules
{
    [DataContract]
    public class AccountingRulePackageDTO 
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
        public List<AccountingRuleDTO> AccountingRules { get; set; }

        /// <summary>
        /// Identificador del paquete de reglas asociado
        /// </summary>
        [DataMember]
        public int? RulePackageId { get; set; }
    }
}