using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniqueUserServices.Models.Base
{
    [DataContract]
    public class BaseUserAgency
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public DateTime? DateDeclined { get; set; }
    }
}