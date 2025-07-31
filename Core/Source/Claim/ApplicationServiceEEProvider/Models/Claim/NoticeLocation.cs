using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class NoticeLocation
    {
        /// <summary>
        /// Dirección
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Pais
        /// </summary>
        [DataMember]
        public int CountryId { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [DataMember]
        public int StateId { get; set; }

        /// <summary>
        /// Ciudad
        /// </summary>
        [DataMember]
        public int CityId { get; set; }

        [DataMember]
        public Notice Notice { get; set; }
    }
}
