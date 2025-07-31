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
    public class BaseIssuanceIdentificationDocument : Extension
    {
        /// <summary>
        /// Número de documento
        /// </summary>
        [DataMember]
        public string Number { get; set; }

    }
}