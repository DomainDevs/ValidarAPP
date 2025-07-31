using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftBusinessServices.Models
{
    [DataContract]
    public class CompanyCoSarlaft
    {
        [DataMember]
        public int SarlaftId { get; set; }
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public decimal? Heritage { get; set; }
        [DataMember]
        public int? IdCompanyTypeCode { get; set; }
        [DataMember]
        public int? CityCode { get; set; }
        [DataMember]
        public int? StateCode { get; set; }
        [DataMember]
        public int? CountryCode { get; set; }

        [DataMember]
        public string OppositorTypeCode { get; set; }
        [DataMember]
        public int? PersonTypeCode { get; set; }

        [DataMember]
        public int? SocietyTypeCode { get; set; }

        [DataMember]
        public int? NationalityCode { get; set; }

        [DataMember]
        public int? NationalityOtherCode { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public int? ExonerationTypeCode { get; set; }

        [DataMember]
        public string MainAddressNatural { get; set; }
    }
}
