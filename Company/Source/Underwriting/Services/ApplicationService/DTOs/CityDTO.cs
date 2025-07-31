using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingApplicationService.DTOs
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