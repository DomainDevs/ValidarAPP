using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class ReasonDTO
    {
        /// <summary>
        /// Id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// EstimationTypeStatus
        /// <param name="EstimationTypeStatus"></param>
        /// </summary>
        [DataMember]
        public int EstimationTypeStatusId { get; set; }

        /// <summary>
        /// PrefixId
        /// </summary>
        [DataMember]
        public int PrefixId { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }
    }
}
