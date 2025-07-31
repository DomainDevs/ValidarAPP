using Sistran.Core.Application.CommonService.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyCity : BaseCity
    {

        [DataMember]
        public CompanyState State { get; set; }

        [DataMember]
        public string DANECode { get; set; }
    }
}
