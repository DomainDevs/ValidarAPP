#region Using

using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.AccountingClosingServices.EEProvider.Models.GeneralLedger.AccountingConcepts
{
    /// <summary>
    ///    AccountingConcept
    /// </summary>
    [DataContract]
    public class AccountingConcept
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     AccountingAccount
        /// </summary>
        [DataMember]
        public AccountingAccount AccountingAccount { get; set; }

        /// <summary>
        ///     Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     AgentEnabled
        /// </summary>
        [DataMember]
        public bool AgentEnabled { get; set; }

        /// <summary>
        ///     CoInsurancedEnabled
        /// </summary>
        [DataMember]
        public bool CoInsurancedEnabled { get; set; }


        /// <summary>
        ///     ReInsuranceEnabled
        /// </summary>
        [DataMember]
        public bool ReInsuranceEnabled { get; set; }

        /// <summary>
        ///     InsuredEnabled
        /// </summary>
        [DataMember]
        public bool InsuredEnabled { get; set; }

        /// <summary>
        ///     ItemEnabled
        /// </summary>
        [DataMember]
        public bool ItemEnabled { get; set; }

    }
}