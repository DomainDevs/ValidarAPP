using System.Collections.Generic;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingRules
{
    public class AccountingRulePackage 
    {
        /// <summary>
        ///     Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Descripción
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Modulo
        /// </summary>
        public int ModuleDateId { get; set; }

        /// <summary>
        ///     Conceptos
        /// </summary>
        public List<AccountingRule> AccountingRules { get; set; }

        /// <summary>
        /// Identificador del paquete de reglas asociado
        /// </summary>
        public int? RulePackageId { get; set; }
    }
}