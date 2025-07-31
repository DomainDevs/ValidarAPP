using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.CancellationMassiveEndorsement.Models
{
    public class CancellationValidation : Validation
    {
        [DataMember]
        public int PrefixId { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public DateTime CurrentFrom { get; set; }

        [DataMember]
        public decimal PolicyNumber { get; set; }

        [DataMember]
        public int CancellationType { get; set; }

    }
}
