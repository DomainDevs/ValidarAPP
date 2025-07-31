using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReportingServices.Models.Formats
{
    /// <summary>
    /// FormatModule: Modulos
    /// </summary>
    [DataContract]
    public class FormatModule
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
