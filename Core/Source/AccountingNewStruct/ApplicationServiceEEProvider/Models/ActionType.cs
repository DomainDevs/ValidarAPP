using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models
{
    /// <summary>
    /// ActionType: 
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class ActionType
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description: Descripción de la Acción
        /// </summary>        
        [DataMember]
        public string Description { get; set; }
    }
}
