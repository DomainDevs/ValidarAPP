using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingApplicationService.DTOs
{
    [DataContract]
    public class StateDTO
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
        public CountryDTO Country { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool Enabled { get; set; }
    }
}