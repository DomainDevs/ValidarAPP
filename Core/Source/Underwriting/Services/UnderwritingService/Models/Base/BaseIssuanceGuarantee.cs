using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseIssuanceGuarantee : Extension
    {
        /// <summary>
        /// Codigo
        /// </summary>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Guarantee"/> is apostille.
        /// </summary>
        /// <value>
        ///   <c>true</c> if apostille; otherwise, <c>false</c>.
        /// </value>
		[DataMember]
        public bool Apostille { get; set; }
    }
}
