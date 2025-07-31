using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.ReportingServices.Models.Formats
{
    /// <summary>
    /// FileFormat: Formato de Archivo
    /// </summary>
    [DataContract]
    public class Format
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


        /// <summary>
        /// FormatModule: Modulo  
        /// </summary>        
        [DataMember]
        public FormatModule FormatModule { get; set; }

        /// <summary>
        /// DateFrom: Fecha Desde
        /// </summary>        
        [DataMember]
        public DateTime DateFrom { get; set; } 
        /// <summary>
        /// DateTo: Fecha Hasta
        /// </summary>        
        [DataMember]
        public DateTime DateTo { get; set; }               

        /// <summary>
        /// FileType: Tipo de Archivo
        /// </summary>        
        [DataMember]
        public FileTypes FileType { get; set; }     
                
       
        
    }
}
