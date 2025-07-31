using System.Runtime.Serialization;

namespace Sistran.Company.Application.UploadFileServices.Models
{
    [DataContract]
    public class MassiveLoadProcess
    {
        /// <summary>
        /// Obtiene o establece el identificador del cargue.
        /// </summary>
        [DataMember]
        public int MassiveLoadId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del proceso.
        /// </summary>
        [DataMember]
        public int? ProcessId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del usuario.
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la cuenta.
        /// </summary>
        [DataMember]
        public int AccountName { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del archivo.
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la hoja de excel.
        /// </summary>
        [DataMember]
        public string WorkSheetName { get; set; }

        /// <summary>
        /// Obtiene o establece número máximo de filas por hilo.
        /// </summary>
        [DataMember]
        public int MaxRowsPerThread { get; set; }

        /// <summary>
        /// Obtiene o establece el número máximo de hilos.
        /// </summary>
        [DataMember]
        public int MaxThread { get; set; }

        /// <summary>
        /// Obtiene o establece la columna inicial.
        /// </summary>
        [DataMember]
        public string BeginColumn { get; set; }

        /// <summary>
        /// Obtiene o establece la columna final.
        /// </summary>
        [DataMember]
        public string EndColumn { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del ramo
        /// </summary>
        [DataMember]
        public int FieldSet { get; set; }

        /// <summary>
        /// Obtiene o establece los valores del archivo de excel
        /// </summary>
        [DataMember]
        public string Values { get; set; }

        /// <summary>
        /// Obtiene o establece el número de la primer fila
        /// </summary>
        [DataMember]
        public int RowFirst { get; set; }

        /// <summary>
        /// Obtiene o establece el número de la ultima fila
        /// </summary>
        [DataMember]
        public int RowLast { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del temporal
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }
    }
}

