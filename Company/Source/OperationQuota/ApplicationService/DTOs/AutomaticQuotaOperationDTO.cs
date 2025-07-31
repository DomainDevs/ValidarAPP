using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.OperationQuotaServices.DTOs
{
    [DataContract]
   public  class AutomaticQuotaOperationDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int? ParentId { get; set; }
        [DataMember]
        public int AutomaticOperationType { get; set; }
        [DataMember]
        public int? User { get; set; }
        [DataMember]
        public DateTime CreationDate { get; set; }
        [DataMember]
        public DateTime? ModificationDate { get; set; }
        [DataMember]
        public string Operation { get; set; }
    }
}
