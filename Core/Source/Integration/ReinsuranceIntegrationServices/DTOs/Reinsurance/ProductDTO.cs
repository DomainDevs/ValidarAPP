using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
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
