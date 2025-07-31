using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    public class CompanyIndividualTaxExeption : BaseIndividualTaxExeption
    {


        [DataMember]
        public CompanyTaxCategory TaxCategory { get; set; }


    }
}
