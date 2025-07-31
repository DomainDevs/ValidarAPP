using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftBusinessServices.Models
{
    [DataContract]
    public class CompanySarlaftExoneration : BasePerson
    {

        [DataMember]
        public int ExonerationType { get; set; }
        [DataMember]
        public bool IsExonerated { get; set; }
        [DataMember]
        public DateTime RegistrationDate { get; set; }
        [DataMember]
        public int RoleId { get; set; }
    }
}
