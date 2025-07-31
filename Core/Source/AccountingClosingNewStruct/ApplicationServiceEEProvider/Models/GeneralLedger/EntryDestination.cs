#region Using

using System;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.AccountingClosingServices.EEProvider.Models.GeneralLedger
{
    /// <summary>
    ///     Modelo que representa el Destino
    /// </summary>
    [DataContract]
    public class EntryDestination
    {
        /// <summary>
        ///     Identificador único del modelo
        /// </summary>
        [DataMember]
        public int DestinationId { get; set; }

        /// <summary>
        ///     Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}