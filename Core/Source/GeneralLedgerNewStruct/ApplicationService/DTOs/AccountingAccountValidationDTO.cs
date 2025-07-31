using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    [DataContract]
    public class AccountingAccountValidationDTO
    {
        /// <summary>
        ///     indica si la validación fue exitosa
        /// </summary>
        [DataMember]
        public bool IsSucessful { get; set; }

        /// <summary>
        ///     Id que se utiliza a nivel web
        /// </summary>
        [DataMember]
        public int TypeId { get; set; }
    }
}