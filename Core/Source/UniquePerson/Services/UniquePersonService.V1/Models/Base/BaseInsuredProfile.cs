using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    public class BaseInsuredProfile
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
