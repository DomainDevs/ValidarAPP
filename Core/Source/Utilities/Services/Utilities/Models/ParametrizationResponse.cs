using Sistran.Core.Application.UtilitiesServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Services.UtilitiesServices.Models
{
    [DataContract]
    public class ParametrizationResponse<l> : BaseParametrizationResponse
    {
        [DataMember]
        public List<l> ReturnedList { get; set; }
    }
}
