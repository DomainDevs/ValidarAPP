using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniqueUserServices.Models
{
    [DataContract]
    public class CompanyUserGroup
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int GroupId { get; set; }
    }
}
