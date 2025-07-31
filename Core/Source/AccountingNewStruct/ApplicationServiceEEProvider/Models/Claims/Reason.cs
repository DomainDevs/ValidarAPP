using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims
{
    /// <summary>
    /// Razon
    /// </summary>
    [DataContract]
    public class Reason
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [DataMember]
        public Status Status { get; set; }
    }
}
