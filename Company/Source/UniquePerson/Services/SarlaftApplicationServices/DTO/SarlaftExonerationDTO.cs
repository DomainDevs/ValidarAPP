using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
    [DataContract]
    public class SarlaftExonerationtDTO
    {
        [DataMember]
        public int IndividualId { get; set; }
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