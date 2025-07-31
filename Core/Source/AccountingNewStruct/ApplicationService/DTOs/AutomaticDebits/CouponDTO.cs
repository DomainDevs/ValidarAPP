using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.AutomaticDebits
{
    /// <summary>
    /// Coupon: Item Debito Automatico (Cupon)
    /// </summary>
    [DataContract]
    public class CouponDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Policy: Poliza con la cuota a generar el debito
        /// </summary>        
        [DataMember]
        public PolicyDTO Policy { get; set; }

        /// <summary>
        /// CouponStatus: Estado del Cupon
        /// </summary>        
        [DataMember]
        public CouponStatusDTO CouponStatus { get; set; }

    }
}
