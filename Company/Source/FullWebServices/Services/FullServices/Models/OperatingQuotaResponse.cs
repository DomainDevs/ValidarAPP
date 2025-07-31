using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class OperatingQuotaResponse
    {
        public enum OperatingQuotaResponse_Fields
        {
            Response_Id,
            Response_Description
        }
        [DataMember]
        public int ResponseId { get; set; }
        [DataMember]
        public string ResponseDescription { get; set; }
    }
}
