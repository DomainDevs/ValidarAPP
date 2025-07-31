using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{

    [DataContract]
    public class InsuredGuaranteeOthers : BaseInsuredGuarantee
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String DescriptionOthers { get; set; }

    }
}
