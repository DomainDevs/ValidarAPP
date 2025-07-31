
using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///    Log de Procesos Masivos 
    /// </summary>
    [DataContract]
    public class ProcessLogDTO
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        
        /// <summary>
        ///     Usuario
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        ///     Fecha de Inicio
        /// </summary>
        [DataMember]
        public DateTime StartDate { get; set; }


        /// <summary>
        ///     Fecha Fin
        /// </summary>
        [DataMember]
        public DateTime EndDate { get; set; }


        /// <summary>
        ///     Estado del Proceso
        /// </summary>
        [DataMember]
        public int ProcessLogStatus { get; set; }

        /// <summary>
        ///     Mes
        /// </summary>
        [DataMember]
        public int Month { get; set; }

        /// <summary>
        ///     Año
        /// </summary>
        [DataMember]
        public int Year { get; set; }

    }
}