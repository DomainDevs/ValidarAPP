using Sistran.Company.Application.ModelServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.UniquePersonService.Enums;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    using Core.Application.AuthorizationPoliciesServices.Models;

    [DataContract]
    public class IndividualTypePersonDTO
    {
        [DataMember]
        public PersonType PersonType { get; set; }       
        
        [DataMember]
        public PersonDTO Person { get; set; }
        
        [DataMember]
        public CompanyDTO Company { get; set; } 
    }
}
