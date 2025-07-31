using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class CityDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <returns>int</returns>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// State
        /// </summary>
        /// <returns>string</returns>
        [DataMember]
        public StateDTO State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string DANECode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

    }
}
