using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponseCompany
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public int? VerifyDigit { get; set; }
        [DataMember]
        public string Role { get; set; }
        [DataMember]
        public int CompanyType { get; set; }
        [DataMember]
        public int AssociationType { get; set; }
        [DataMember]
        public List<ResponseAddress> Addresses { get; set; }
        [DataMember]
        public List<ResponseEmail> Emails { get; set; }
        [DataMember]
        public List<ResponsePhone> Phones { get; set; }
        [DataMember]
        public int EconomicActivityId { get; set; }
        [DataMember]
        public string CheckPayable { get; set; }
        [DataMember]
        public ResponseIdentificationDocument IdentificationDocument { get; set; }
    }
}
