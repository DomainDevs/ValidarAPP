using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
    [DataContract]
    public class UserDTO
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public string FormNum { get; set; }
    }
}
