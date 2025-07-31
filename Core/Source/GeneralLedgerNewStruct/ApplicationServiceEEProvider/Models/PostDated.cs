using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models
{
    /// <summary>
    ///     Modelo utilizado para el manejo de Posfechados
    /// </summary>
    [DataContract]
    public class PostDated
    {
        /// <summary>
        ///     Identificador único del modelo
        /// </summary>
        [DataMember]
        public int PostDatedId { get; set; }

        // Tipo de valor (Cheque o Credito)
        /// <summary>
        /// </summary>
        [DataMember]
        public PostDateTypes PostDateType { get; set; }

        /// <summary>
        ///     Importe (moneda y cambio)
        /// </summary>
        [DataMember]
        public Amount Amount { get; set; }

        /// <summary>
        ///   ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRate ExchangeRate { get; set; }

        /// <summary>
        ///   Importe Local
        /// </summary>
        [DataMember]
        public Amount LocalAmount { get; set; }

        // Número de Documento
        /// <summary>
        /// </summary>
        [DataMember]
        public int DocumentNumber { get; set; }
    }
}