using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class BaseSubscriptionPayer:Extension
    {
        /// <summary>
        /// Atributo para la propiedad TemporalId.
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
		/// Atributo para la propiedad CustomerTypeCd.
		/// </summary>
        [DataMember]
        public int CustomerTypeCd { get; set; }

        /// <summary>
		/// Atributo para la propiedad PayerId.
		/// </summary>
        [DataMember]
        public int PayerId { get; set; }

        /// <summary>
        /// Atributo para la propiedad SurchargePct.
        /// </summary>
        [DataMember]
        public decimal SurchargePct { get; set; }

        /// <summary>
		/// Atributo para la propiedad PaymentScheduleId.
		/// </summary>
        [DataMember]
        public int PaymentScheduleId { get; set; }

        /// <summary>
		/// Atributo para la propiedad PaymentMethodCd.
		/// </summary>
        [DataMember]
        public int PaymentMethodCd { get; set; }

        /// <summary>
		/// Atributo para la propiedad PaymentId.
		/// </summary>
        [DataMember]
        public int PaymentId { get; set; }

        /// <summary>
		/// Atributo para la propiedad MailAddressId.
		/// </summary>
        [DataMember]
        public int MailAddressId { get; set; }
    }
}
