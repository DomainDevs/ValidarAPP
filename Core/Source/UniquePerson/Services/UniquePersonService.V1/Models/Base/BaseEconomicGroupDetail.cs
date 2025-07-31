using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.Extensions;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseEconomicGroupDetail : Extension
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
