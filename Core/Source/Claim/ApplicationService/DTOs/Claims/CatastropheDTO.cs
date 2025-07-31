using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class CatastropheDTO
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
    }
}
