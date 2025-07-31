using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models
{
    /// <summary>
    ///     Modelo utilizado para el manejo de números de asiento
    /// </summary>
    [DataContract]
    public class EntryNumber
    {
        /// <summary>
        ///     Identificador único del modelo
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Tipo movimiento contable
        /// </summary>
        [DataMember]
        public AccountingMovementType AccountingMovementType { get; set; }

        /// <summary>
        ///     Destino asiento
        /// </summary>
        [DataMember]
        public EntryDestination EntryDestination { get; set; }

        /// <summary>
        ///     Fecha generación asiento
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        ///     Último año de generación asiento
        /// </summary>
        [DataMember]
        public int Year { get; set; }

        /// <summary>
        ///     Número de asiento
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        ///     Si el número asiento es generado para diario o de mayor
        /// </summary>
        [DataMember]
        public bool IsJournalEntry { get; set; }

    }
}
