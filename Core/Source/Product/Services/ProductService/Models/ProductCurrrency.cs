using Sistran.Core.Application.ProductServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ProductServices.Models
{
    [DataContract]
    public class ProductCurrency : BaseCurrency
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int DecimalQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }

    }
}