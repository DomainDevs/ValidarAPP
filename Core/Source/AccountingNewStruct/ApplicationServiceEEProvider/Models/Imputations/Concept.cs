using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// Concept:   Concepto
    /// </summary>
    [DataContract]
    public class Concept
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Description 
        /// </summary> 
        [DataMember]
        public string Description { get; set; }

        
    }
}
