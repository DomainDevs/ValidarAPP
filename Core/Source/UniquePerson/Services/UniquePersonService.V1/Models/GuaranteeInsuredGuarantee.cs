using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    [DataContract]
    public class GuaranteeInsuredGuarantee : BaseGeneric
    {

        /// <summary>
        /// IndividualId
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Tipo contragarantia
        /// </summary>
        [DataMember]
        public int typeId { get; set; }

    }
}
