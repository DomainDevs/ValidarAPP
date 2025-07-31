using System.Runtime.Serialization;

namespace Sistran.Company.Application.Marines.MarineApplicationService.DTOs
{
    /// <summary>
    /// Objeto del Seguro
    /// </summary>
	[DataContract]
    public class InsuredObjectDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
		[DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Limite Asegurado
        /// </summary>
		[DataMember]
        public decimal InsuredLimitAmount { get; set; }

        /// <summary>
        /// Prima
        /// </summary>
		[DataMember]
        public decimal PremiumAmount { get; set; }

        /// <summary>
        /// Porcentaje de Prima en Deposito
        /// </summary>
		[DataMember]
        public decimal DepositPremiumPercentage { get; set; }

        /// <summary>
        /// Id Tipo de Tasa
        /// </summary>
		[DataMember]
        public int RateTypeId { get; set; }

        /// <summary>
        /// Tasa
        /// </summary>
		[DataMember]
        public decimal Rate { get; set; }

        /// <summary>
        /// Objeto viene seleccionado
        /// </summary>
        [DataMember]
        public bool? IsSelected { get; set; }

        /// <summary>
        /// Indica si es obligatorio por defecto
        /// </summary>
        [DataMember]
        public bool? IsMandatory { get; set; }
    }
}