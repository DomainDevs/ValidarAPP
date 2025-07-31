using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables
{
    

    /// <summary>
    ///     Tipo de Voucher
    /// </summary>
    [DataContract]
    public class VoucherType
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
    }
}
