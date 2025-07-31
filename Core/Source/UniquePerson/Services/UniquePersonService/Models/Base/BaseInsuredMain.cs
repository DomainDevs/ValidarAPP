using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    public class BaseInsuredMain
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
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string FullName { get; set; }
    }
}
