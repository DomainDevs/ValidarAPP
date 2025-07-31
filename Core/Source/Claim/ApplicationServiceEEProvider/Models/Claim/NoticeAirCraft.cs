using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class NoticeAirCraft
    {
        [DataMember]
        public int? MakeId { get; set; }

        [DataMember]
        public int? ModelId { get; set; }

        [DataMember]
        public int? TypeId { get; set; }

        [DataMember]
        public int? UseId { get; set; }

        [DataMember]
        public int? RegisterId { get; set; }

        [DataMember]
        public int? OperatorId { get; set; }

        [DataMember]
        public string RegisterNumer { get; set; }

        [DataMember]
        public int? Year { get; set; }

        /// <summary>
        /// Notice
        /// </summary>
        [DataMember]
        public Notice Notice { get; set; }
    }
}
