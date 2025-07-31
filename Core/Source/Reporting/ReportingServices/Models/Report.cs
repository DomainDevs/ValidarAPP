// Sistran


using Sistran.Core.Application.ReportingServices.Models.Formats;

using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.ReportingServices.Models
{
    /// <summary>
    /// Reporte
    /// </summary>
    [DataContract]    
    public class Report
    {
       
        /// <summary>
        /// Name: Nombre del reporte
        /// </summary>        
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Description: Descripción del reporte
        /// </summary>        
        [DataMember]
        public string Description { get; set; }

         /// <summary>
        /// Format: Formato
        /// </summary>
        [DataMember]
        public Format Format { get; set; }

       
        /// <summary>
        /// User: Usuario genera el reporte
        /// </summary>        
        [DataMember]
        public int UserId { get; set; }

        
        /// <summary>
        /// Filter: Filtros del reporte
        /// </summary>
        [DataMember]
        public string Filter { get; set; }

        /// <summary>
        /// Parameters: Parámetros y/o fórmulas del reporte
        /// </summary>        
        [DataMember]
        public List<Parameter> Parameters { get; set; }


        /// <summary>
        /// Procedimiento Almacenado Asociado
        /// </summary>
        [DataMember]
        public StoredProcedure StoredProcedure { get; set; }

       
        /// <summary>
        /// ExportType: Tipo formato exportación del reporte
        /// </summary>
        [DataMember]
        public ExportTypes ExportType { get; set; }

        /// <summary>
        /// IsAsync: Modo de ejecución
        /// </summary>
        [DataMember]
        public bool IsAsync { get; set; } 

       
    }
}
