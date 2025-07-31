using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models
{
    /// <summary>
    ///     AccountingAccountType: Tipo de Cuenta Contable
    /// </summary>
    [DataContract]
    public class AccountingAccountType
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}