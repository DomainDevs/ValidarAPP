using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.BankAccounts
{
    /// <summary>
    ///     Cuenta bancaria de la Persona
    /// </summary>
    [DataContract]
    public class BankAccountPersonDTO
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
        public BankAccountTypeDTO BankAccountType { get; set; }

        /// <summary>
        ///     Individuo
        /// </summary>
        [DataMember]
        public IndividualDTO Individual { get; set; }
        
        /// <summary>
        ///     Numero de Cuenta
        /// </summary>
        [DataMember]
        public string Number { get; set; }

        /// <summary>
        ///     Banco
        /// </summary>
        [DataMember]
        public BankDTO Bank { get; set; }

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
        public CurrencyDTO Currency { get; set; }
    }
}

