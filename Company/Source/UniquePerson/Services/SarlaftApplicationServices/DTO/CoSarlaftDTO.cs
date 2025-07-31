using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
    [DataContract]
    public class CoSarlaftDTO
    {
        [DataMember]
        public int sarlaftid { get; set; }
        [DataMember]
        public int individualid { get; set; }
        
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public decimal? heritage { get; set; }
        [DataMember]
        public int? idCompanyTypeCode { get; set; }
        [DataMember]
        public int? cityCode { get; set; }
        [DataMember]
        public int? stateCode { get; set; }
        [DataMember]
        public int? countryCode { get; set; }

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
