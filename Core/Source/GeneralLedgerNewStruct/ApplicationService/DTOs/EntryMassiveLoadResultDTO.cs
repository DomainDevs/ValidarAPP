using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///     Modelo para devolver los resultados del procesos masivo de asientos    
    /// </summary>
    [DataContract]
    public class EntryMassiveLoadResultDTO
    {
        /// <summary>
        ///     total de registros procesados
        /// </summary>
        [DataMember]
        public int TotalRecords { get; set; }

        /// <summary>
        ///     Registros procesados correctamente.
        /// </summary>
        [DataMember]
        public int SuccessfulRecords { get; set; }

        /// <summary>
        ///     Registros fallidos.
        /// </summary>
        [DataMember]
        public int FailedRecords { get; set; }
    }
}