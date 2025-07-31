using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models
{
    /// <summary>
    /// Rejection: Motivo de Rechazo
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class Rejection
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description: Descripción del Motivo de Rechazo
        /// </summary>        
        [DataMember]
        public string Description { get; set; }
    }
}
