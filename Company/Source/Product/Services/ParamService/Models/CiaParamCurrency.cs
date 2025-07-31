namespace Sistran.Company.Application.ProductParamService.Models
{
    using Sistran.Core.Application.ProductParamService.Models.Base;
    using System.Runtime.Serialization;

    /// <summary>
    ///
    /// </summary>
    public class CiaParamCurrency: BaseParamCurrency
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int DecimalQuantity { get; set; }

    }
}
