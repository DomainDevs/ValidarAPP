using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ProductDTO
    {
        /// <summary>
        /// Id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Descripción del producto
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        [DataMember]
        public string Description { get; set; }
    }
}
