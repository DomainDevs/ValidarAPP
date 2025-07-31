using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.ReportingServices.Models.Formats
{
    /// <summary>
    /// FormatDetail: Detalle del Formato de Archivo
    /// </summary>
    [DataContract]
    public class FormatDetail
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Format 
        /// </summary>        
        [DataMember]
        public Format Format { get; set; }    


        /// <summary>
        /// FormatType: Tipo de Formato
        /// </summary>        
        [DataMember]
        public FormatTypes FormatType { get; set; }


        /// <summary>
        /// Separator: Separador
        /// </summary>        
        [DataMember]
        public string Separator { get; set; }
                               
        /// <summary>
        /// Fields: Campos del Formato
        /// </summary>        
        [DataMember]
        public List<FormatField> Fields { get; set; }


        
        
    }
}
