using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.AutomaticDebits
{
    /// <summary>
    /// Coupon: Item Debito Automatico (Cupon)
    /// </summary>
    [DataContract]
    public class Coupon
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
        public Policy Policy { get; set; }

        /// <summary>
        /// CouponStatus: Estado del Cupon
        /// </summary>        
        [DataMember]
        public CouponStatus CouponStatus { get; set; }

    }
}
