using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.BankAccounts
{
    /// <summary>
    ///     Tipo de Cuenta Bancaria
    /// </summary>
    [DataContract]
    public class BankAccountTypeDTO
    {
        /// <summary>
        ///     Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Si/No Habilitado
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }
    }
}

