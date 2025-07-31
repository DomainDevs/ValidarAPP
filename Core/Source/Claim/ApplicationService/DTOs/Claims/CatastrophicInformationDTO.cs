using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class CatastrophicInformationDTO
    {
        [DataMember]
        public int CatastrophicId { get; set; }

        [DataMember]
        public string CatastropheDescription { get; set; }

        /// <summary>
        /// DateTimeFrom 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public DateTime? DateTimeFrom { get; set; }

        /// <summary>
        /// DateTimeTo 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public DateTime? DateTimeTo { get; set; }

        /// <summary>
        /// Address 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// City 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public CityDTO City { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

    }
}
