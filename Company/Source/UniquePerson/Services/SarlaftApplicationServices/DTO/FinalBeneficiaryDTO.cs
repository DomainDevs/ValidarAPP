using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
    [DataContract]
    public class FinalBeneficiaryDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string IdCardNumero { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int DocumentTypeId { get; set; }
        [DataMember]
        public string TradeName { get; set; }
        [DataMember]
        public int SarlaftId { get; set; }


    }
}
