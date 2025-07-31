// Sistran



using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReportingServices.Models
{
    [DataContract]
    public class MassiveReport
    {
        /// <summary>
        /// Id: identificador de proceso
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description: Descripción corta del reporte
        /// </summary>        
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// GenerationDate: Fecha y hora de generación del reporte
        /// </summary>        
        [DataMember]
        public DateTime GenerationDate { get; set; }

        /// <summary>
        /// User: Usuario genera el reporte
        /// </summary>        
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// StartDate: Fecha y hora de inicio de la generación del reporte
        /// </summary>        
        [DataMember]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// EndDate: Fecha y hora de finalización de la generación del reporte
        /// </summary>        
        [DataMember]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// UrlFile: Url del reporte generado
        /// </summary>        
        [DataMember]
        public string UrlFile { get; set; }

        /// <summary>
        /// Success: Proceso terminado 
        /// </summary>
        [DataMember]
        public bool Success { get; set; }

        /// <summary>
        /// ModuleId : Modo aplicado, Reaseguros, Accounting, General Ledger
        /// </summary>
        [DataMember]
        public int ModuleId { get; set; }

        /// <summary>
        /// RecordsNumber :Total de Registros procesados
        /// </summary>
        [DataMember]
        public int RecordsNumber { get; set; }

        /// <summary>
        /// RecordsProcessed : Número de registros procesado
        /// </summary>
        [DataMember]
        public int RecordsProcessed { get; set; }

        /// <summary>
        /// Order : Orden de los registros
        /// </summary>
        [DataMember]
        public string Order { get; set; }


        /// <summary>
        /// Progress : Progreso del reporte
        /// </summary>
        [DataMember]
        public string Progress { get; set; }


        /// <summary>
        /// Elapsed
        /// </summary>
        [DataMember]
        public string Elapsed { get; set; }
        
    }
}
