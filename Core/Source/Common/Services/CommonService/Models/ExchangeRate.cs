using Sistran.Core.Application.CommonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CommonService.Models
{
    [DataContract]
    public class ExchangeRate : BaseExchangeRate
    {
        /// <summary>
        /// Moneda
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }
    }
}
