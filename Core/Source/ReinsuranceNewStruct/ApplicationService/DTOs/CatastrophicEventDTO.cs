using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class CatastrophicEventDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime CurrentFrom { get; set; }

        [DataMember]
        public DateTime CurrentTo { get; set; }

        [DataMember]
        public string FullAddress { get; set; }
    }
}
