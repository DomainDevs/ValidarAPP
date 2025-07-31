using System.Runtime.Serialization;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;

namespace Sistran.Core.Application.MassiveRenewalServices.Models
{
    [DataContract]
    [Serializable]
    public class MassiveRenewalRow
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador del masivo
        /// </summary>
        [DataMember]
        public int MassiveRenewalId { get; set; }

        /// <summary>
        /// Numero de Fila
        /// </summary>
        [DataMember]
        public int RowNumber { get; set; }

        /// <summary>
        /// Poliza
        /// </summary>
        [DataMember]
        public Risk Risk { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [DataMember]
        public MassiveLoadProcessStatus? Status { get; set; }

        /// <summary>
        /// Identificador del temporal
        /// </summary>
        [DataMember]
        public int? TemporalId { get; set; }

        /// <summary>
        /// Si se produjo errores
        /// </summary>
        [DataMember]
        public bool? HasError { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [DataMember]
        public string Observations { get; set; }

        /// <summary>
        /// SerializedRow
        /// </summary>
        [DataMember]
        public string SerializedRow { get; set; }

        /// <summary>
        /// Comisión
        /// </summary>
        [DataMember]
        public decimal TotalCommission { get; set; }

        /// <summary>
        /// Tiene Eventos?
        /// </summary>
        [DataMember]
        public bool HasEvents { get; set; }
    }
}
