using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class EconomicGroupDetailDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int EconomicGroupId { get; set; }

        /// <summary>
        /// EconomicGroupName
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// TributaryIdType
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// TributaryIdNo
        /// </summary>
        [DataMember]
        public DateTime? DeclinedDate { get; set; }
    }
}
