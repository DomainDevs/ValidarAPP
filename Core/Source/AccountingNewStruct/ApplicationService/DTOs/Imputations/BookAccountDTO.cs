using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// BookAccount: Libro de Cuentas
    /// </summary>
    [DataContract]
    public class BookAccountDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// AccountNumber: Numero de Cuenta 
        /// </summary>        
        [DataMember]
        public string AccountNumber { get; set; }

        /// <summary>
        /// AccountName: Nombre de Cuenta
        /// </summary>        
        [DataMember]
        public string AccountName { get; set; }
    }
}
