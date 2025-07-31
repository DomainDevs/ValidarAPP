using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    public class CompanyPartner : BasePartner
    {
        /// <summary>
        /// Gets or sets the identification document.
        /// </summary>
        /// <value>
        /// The identification document.
        /// </value>
        [DataMember]
        public CompanyIdentificationDocument IdentificationDocument { get; set; }

    
    }
}
