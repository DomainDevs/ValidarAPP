using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingApplicationService.DTOs
{
    [DataContract]
    public class CiaRatingZoneBranchDTO
    {
        [DataMember]
      
        public int BranchCode { get; set; }

        /// <summary>
        /// RatingZone
        /// </summary>
        [DataMember]
        public int RatingZone { get; set; }
    }
}
