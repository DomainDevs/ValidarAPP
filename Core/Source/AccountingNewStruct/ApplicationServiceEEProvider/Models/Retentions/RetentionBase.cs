
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Retentions
{
    /// <summary>
    /// RetentionBase: Base de Retencion
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class RetentionBase
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description: Descripción 
        /// </summary>        
        [DataMember]
        public string Description { get; set; }
       
             
    }
}
