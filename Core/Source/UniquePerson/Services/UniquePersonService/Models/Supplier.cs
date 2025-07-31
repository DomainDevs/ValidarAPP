using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Informacion Social
    /// </summary>
    [DataContract]
    public class Supplier : BaseSupplier
    {
        /// <summary>
        /// SupplierProfile 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public SupplierProfile SupplierProfile { get; set; }

    }
}
