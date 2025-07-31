using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Salvage
{
    [DataContract]
    public class Buyer 
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Phone { get; set; }


    }
}
