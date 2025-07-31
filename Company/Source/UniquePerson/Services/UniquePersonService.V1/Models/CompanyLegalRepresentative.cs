using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{

    [DataContract]
    public class CompanyLegalRepresentative : BaseLegalRepresentative
    {

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        [DataMember]
        public CompanyCity City { get; set; }

        /// <summary>
        /// Gets or sets the identification document.
        /// </summary>
        /// <value>
        /// The identification document.
        /// </value>
        [DataMember]
        public CompanyIdentificationDocument IdentificationDocument { get; set; }

        /// <summary>
        /// Gets or sets the authorization amount.
        /// </summary>
        /// <value>
        /// The authorization amount.
        /// </value>
        [DataMember]
        public CompanyAmount AuthorizationAmount { get; set; }

    }
}
