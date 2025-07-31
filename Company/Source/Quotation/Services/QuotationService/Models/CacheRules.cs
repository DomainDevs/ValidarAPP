using System.Runtime.Serialization;

namespace Sistran.Company.Application.QuotationServices.Models
{
    /// <summary>
    /// Parametrizacion productos en cache
    /// </summary>
    [DataContract]
    public class CacheRules
    {
        /// <summary>
        /// Id producto
        /// </summary>
        [DataMember]
        public int productId { get; set; }

        /// <summary>
        /// Id grupo de coberturas
        /// </summary>
        [DataMember]
        public int groupCoverageId { get; set; }        
    }
}
