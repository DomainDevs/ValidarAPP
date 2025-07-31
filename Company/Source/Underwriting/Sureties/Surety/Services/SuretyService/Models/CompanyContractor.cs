using Sistran.Company.Application.UnderwritingServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Sureties.SuretyServices.Models
{
    /// <summary>
    /// Afianzado
    /// </summary>
    [DataContract]
    public class CompanyContractor : CompanyIssuanceInsured
    {
        [DataMember]
        public int? SinisterCount { get; set; }

        [DataMember]
        public bool? TechnicalCard { get; set; }

        [DataMember]
        public int? AssociationTypeId { get; set; }

        [DataMember]
        public bool? IsConsortium { get; set; }
    }
}
