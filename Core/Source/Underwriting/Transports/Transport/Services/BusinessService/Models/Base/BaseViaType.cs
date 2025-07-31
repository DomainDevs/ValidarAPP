using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Transports.TransportBusinessService.Models.Base
{
    [DataContract]
    public class BaseViaType : BaseGeneric
    {
        /// <summary>
        /// Esta Habilitado
        /// </summary>
        [DataMember]
        public Boolean IsEnabled { get; set; }
    }
}
