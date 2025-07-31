using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Services.UtilitiesServices.Models
{
    [DataContract]
    public class EndorsementControl
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int UserId { get; set; }
    }
}
