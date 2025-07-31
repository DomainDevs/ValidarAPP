using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.ReportingServices.Models
{
    [DataContract]
    public class StoredProcedure
    {
      
        /// <summary>
        /// ProcedureName: Nombre procedimiento almacenado
        /// </summary>
        [DataMember]
        public string ProcedureName { get; set; }

        /// <summary>
        /// ProcedureParameters: Parámetros del procedimiento almacenado
        /// </summary>        
        [DataMember]
        public List<Parameter> ProcedureParameters { get; set; }
    }
}
