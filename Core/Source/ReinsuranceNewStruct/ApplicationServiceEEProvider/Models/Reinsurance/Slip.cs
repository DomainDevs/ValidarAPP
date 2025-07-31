using Sistran.Core.Application.CommonService.Models;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class Slip
    {

        /// <summary>
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
