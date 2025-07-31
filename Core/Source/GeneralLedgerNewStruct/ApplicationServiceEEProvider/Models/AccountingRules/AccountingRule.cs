using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingRules
{ 
    [DataContract]
    public class AccountingRule
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
        ///     Observation: Observacion
        /// </summary>
        [DataMember]
        public string Observation { get; set; }

        /// <summary>
        ///     Modulo
        /// </summary>
        [DataMember]
        public int ModuleDateId { get; set; }

        /// <summary>
        ///     Condiciones
        /// </summary>
        [DataMember]
        public List<Condition> Conditions { get; set; }
    }
}