using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    public class StateDTO
    {
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public CountryDTO Country { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool Enabled { get; set; }
    }
}
