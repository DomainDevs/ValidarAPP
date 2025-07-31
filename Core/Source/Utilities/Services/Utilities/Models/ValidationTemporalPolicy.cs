using Sistran.Core.Services.UtilitiesServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UtilitiesServices.Models
{
    [DataContract]
    public class ValidationTemporalPolicy : Validation
    {
        [DataMember]
        public decimal DocumentNum { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public int PrefixId { get; set; }

    }
}
