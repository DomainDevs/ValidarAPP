using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    // <summary>    
    /// DTO para recuperar los datos del log de procesos masivos
    /// </summary>
    [DataContract]
    public class MassiveEntryLogDTO : MassiveEntryDTO
    {
        /// <summary>
        ///     Fecha de proceso
        /// </summary>
        [DataMember]
        public DateTime ProcessDate { get; set; }

        /// <summary>
        ///     Indica si la operación fue exitosa
        /// </summary>
        [DataMember]
        public bool Success { get; set; }

        /// <summary>
        ///     Descripción del error
        /// </summary>
        [DataMember]
        public string ErrorDescription { get; set; }

        /// <summary>
        ///     Si está habilitado para mostrarse en los resultados
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }
    }
}