//using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class GuaranteeDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool HasApostille { get; set; }

        [DataMember]
        public bool HasPromissoryNote { get; set; }

        [DataMember]
        public SelectDTO GuaranteeType { get; set; }
    }
}
