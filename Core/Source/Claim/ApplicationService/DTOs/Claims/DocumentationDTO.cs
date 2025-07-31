using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class DocumentationDTO
    {
        /// <summary>
        /// Claim
        /// </summary>
        /// <returns>int</returns>
        [DataMember]
        public int Claim { get; set; }

        /// <summary>
        /// ClaimNotice
        /// </summary>
        /// <returns>int</returns>
        [DataMember]
        public int ClaimNotice { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        /// <returns>string</returns>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// ClaimNotice
        /// </summary>
        /// <returns>bool</returns>
        [DataMember]
        public bool IsRequired { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <returns>int</returns>
        [DataMember]
        public int Id { get; set; }
    }
}
