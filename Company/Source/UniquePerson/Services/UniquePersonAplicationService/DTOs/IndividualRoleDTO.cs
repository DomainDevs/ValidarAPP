using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class IndividualRoleDTO
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int RoleId { get; set; }
    }
}
