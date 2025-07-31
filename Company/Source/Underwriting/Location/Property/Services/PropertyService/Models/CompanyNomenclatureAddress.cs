using Sistran.Core.Application.Locations.Models;
using Sistran.Core.Application.Locations.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Location.PropertyServices.Models
{
    [DataContract]
    public class CompanyNomenclatureAddress : BaseNomenclatureAddress
    {
        [DataMember]
        public CompanyRouteType Type { get; set; }

        [DataMember]
        public CompanyApartmentOrOffice ApartmentOrOffice { get; set; }

        [DataMember]
        public Suffix Suffix1 { get; set; }

        [DataMember]
        public Suffix Suffix2 { get; set; }
        



    }
}
