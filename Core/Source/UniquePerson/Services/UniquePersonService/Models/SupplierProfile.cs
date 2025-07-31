using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Informacion Social
    /// </summary>
    [DataContract]
    public class SupplierProfile : Extension
    {
        /// <summary>
        /// Id 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// IsEnabled 
        /// </summary>
        /// <param name="Enabled"></param>
        /// <returns></returns>
        [DataMember]
        public bool IsEnabled { get; set; }

    }
}
