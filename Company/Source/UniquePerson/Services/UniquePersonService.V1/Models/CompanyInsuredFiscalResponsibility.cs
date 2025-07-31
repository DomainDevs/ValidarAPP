using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyInsuredFiscalResponsibility
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int InsuredId { get; set; }

        [DataMember]
        public int FiscalResponsabilityId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string FiscalResponsabilityDescription { get; set; }

        [DataMember]
        public string Code { get; set; }
    }
}
