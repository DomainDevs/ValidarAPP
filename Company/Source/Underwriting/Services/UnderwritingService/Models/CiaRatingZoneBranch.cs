using Sistran.Company.Application.CommonServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Modelo Zona de circulación por sucursal
    /// </summary>

    [DataContract]
    public class CiaRatingZoneBranch
    {

        /// <summary>
        /// RatingZone
        /// </summary>
        [DataMember]
        public CompanyRatingZone RatingZone { get; set; }

        /// <summary>
        /// Branch
        /// </summary>
        [DataMember]
        public CompanyBranch Branch { get; set; }


    }
}
