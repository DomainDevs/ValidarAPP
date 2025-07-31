using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class NoticeSurety
    {
        /// <summary>
        /// Nombre del afianzado
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Número de documento del afianzado
        /// </summary>
        [DataMember]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// BidNumber
        /// </summary>
        [DataMember]
        public string BidNumber { get; set; }

        /// <summary>
        /// CourtNum
        /// </summary>
        [DataMember]
        public string CourtNum { get; set; }

        /// <summary>
        /// Notice
        /// </summary>
        [DataMember]
        public Notice Notice { get; set; }
    }
}
