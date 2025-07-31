using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.MassiveServices.Models
{
    [DataContract]
    public class MassiveCompany
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public IndividualType IndividualType { get; set; }

        [DataMember]
        public CustomerType CustomerType { get; set; }

        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public int EconomicActivityId { get; set; }

        [DataMember]
        public int DocumentTypeId { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public int CompanyTypeId { get; set; }

        [DataMember]
        public Country CountryOrigin { get; set; }

        [DataMember]
        public int PaymentId { get; set; }

        [DataMember]
        public int PaymentMethodId { get; set; }

        [DataMember]
        public int AddressId { get; set; }

        [DataMember]
        public int AddressTypeId { get; set; }

        [DataMember]
        public string AddressDescription { get; set; }

        [DataMember]
        public City AddressCity { get; set; }

        [DataMember]
        public int PhoneId { get; set; }

        [DataMember]
        public int PhoneTypeId { get; set; }

        [DataMember]
        public string PhoneDescription { get; set; }

        [DataMember]
        public int EmailId { get; set; }

        [DataMember]
        public int EmailTypeId { get; set; }

        [DataMember]
        public string EmailDescription { get; set; }
       


    }
}
