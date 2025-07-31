using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models
{
    /// <summary>
    /// Range
    /// </summary>
    
    [DataContract]
    public class Range
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }
        
        /// <summary>
        /// Description: Descripcion
        /// </summary>        
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// IsDefault
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }

        /// <summary>
        /// RangeItems
        /// </summary>
        [DataMember]
        public List<RangeItem> RangeItems { get; set; }

    }
}
