using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.ReportingServices.Models.Formats
{
    /// <summary>
    /// FormatField: Campo del Formato
    /// </summary>
    [DataContract]
    public class FormatField
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
        /// Order: Orden de campo
        /// </summary>        
        [DataMember]
        public int Order { get; set; }
        
        /// <summary>
        /// Start: Posicion Inicial 
        /// </summary>        
        [DataMember]
        public int Start { get; set; }

        /// <summary>
        /// Length: Longitud
        /// </summary>        
        [DataMember]
        public int Length { get; set; }

        /// <summary>
        /// Value: Valor
        /// </summary>        
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        /// Filled: Relleno
        /// </summary>        
        [DataMember]
        public string Filled { get; set; }

        /// <summary>
        /// Align: Alineacion
        /// </summary>        
        [DataMember]
        public string Align { get; set; }

        /// <summary>
        /// RowNumber: Numero de Fila
        /// </summary>        
        [DataMember]
        public int RowNumber { get; set; }

        /// <summary>
        /// Order: Numero de Columna
        /// </summary>        
        [DataMember]
        public int ColumnNumber { get; set; }

        /// <summary>
        /// Field: Nombre del campo
        /// </summary>        
        [DataMember]
        public string Field { get; set; }


        /// <summary>
        /// Mask: Cadena de Formato del campo
        /// </summary>        
        [DataMember]
        public string Mask { get; set; }

        /// <summary>
        /// IsRequired: Campo Requerido
        /// </summary>        
        [DataMember]
        public bool IsRequired { get; set; }

    }
}
