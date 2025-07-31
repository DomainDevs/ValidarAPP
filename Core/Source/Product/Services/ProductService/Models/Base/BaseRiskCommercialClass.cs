using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.ProductServices.Models.Base
{
    /// <summary>
    /// Comercio clase
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseRiskCommercialClass:Extension
    {
        /// <summary>
        /// Propiedad para DefaultRiskCommercial
        /// </summary>
        [DataMember]
        public bool DefaultRiskCommercial { set; get; }
    }
}
