using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class InsuredFiscalResponsibilityDTO
    {
       [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int InsuredCode { get; set; }

        [DataMember]
        public int FiscalResponsibilityId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string FiscalResponsibilityDescription { get; set; }

        [DataMember]
        public string Code { get; set; }
    }
}
