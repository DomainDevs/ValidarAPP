using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    public class IndividualTaxExeption : BaseIndividualTaxExeption
    {
        [DataMember]
        public TaxCategory TaxCategory { get; set; }

    }
}
