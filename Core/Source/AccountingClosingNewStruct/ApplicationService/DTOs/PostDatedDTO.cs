using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingClosingServices.DTOs
{
    /// <summary>
    ///     Modelo utilizado para el manejo de Posfechados
    /// </summary>
    [DataContract]
    public class PostDatedDTO
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
        public int PostDateType { get; set; }

        /// <summary>
        ///     Importe (moneda y cambio)
        /// </summary>
        [DataMember]
        public AmountDTO Amount { get; set; }

        /// <summary>
        ///   ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRateDTO ExchangeRate { get; set; }

        /// <summary>
        ///   Importe Local
        /// </summary>
        [DataMember]
        public AmountDTO LocalAmount { get; set; }

        // Número de Documento
        /// <summary>
        /// </summary>
        [DataMember]
        public int DocumentNumber { get; set; }
    }
}