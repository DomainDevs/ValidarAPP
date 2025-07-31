using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class SlipDTO
    { /// <summary>
      /// FacultativeId
      /// </summary>
        [DataMember]
        public int FacultativeId { get; set; }
        /// <summary>
        /// SlipNumber
        /// </summary>
        [DataMember]
        public string SlipNumber { get; set; }
    }
}
