using Sistran.Core.Application.CommonService.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts
{
    /// <summary>
    ///     Cuenta bancaria de la Persona
    /// </summary>
    [DataContract]
    public class BankAccountPerson
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Tipo de Cuenta
        /// </summary>
        [DataMember]
        public BankAccountType BankAccountType { get; set; }

        /// <summary>
        ///     Individuo
        /// </summary>
        [DataMember]
        public UniquePersonService.V1.Models.Individual Individual { get; set; }
        
        /// <summary>
        ///     Numero de Cuenta
        /// </summary>
        [DataMember]
        public string Number { get; set; }

        /// <summary>
        ///     Banco
        /// </summary>
        [DataMember]
        public Bank Bank { get; set; }

        /// <summary>
        ///     Si/No Habilitado
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        ///     Si/No Por Defecto
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }

        /// <summary>
        ///     Moneda
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }
    }
}

