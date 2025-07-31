using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;


namespace Sistran.Core.Application.TempCommonServices.Models
{
	[DataContract]
	[Serializable]
	public class Policy
	{
		[DataMember]
		public int Id { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
		public int UserId { get; set; }
		[DataMember]
		public Branch Branch { get; set; }
		[DataMember]
		public Prefix Prefix { get; set; }
		[DataMember]
		public int DocumentNumber { get; set; }
        [DataMember]
        public int EndorsmentId { get; set; }
        [DataMember]
		public Currency Currency { get; set; }
		[DataMember]
		public int BusinessType { get; set; }
		[DataMember]
		public int InsuredId { get; set; }
		[DataMember]
		public DateTime IssueDate { get; set; }
		[DataMember]
		public DateTime CurrentFrom { get; set; }
		[DataMember]
		public DateTime CurrentTo { get; set; }

		[DataMember]
		public Endorsement Endorsement { get; set; }

	}

	[DataContract]
	[Serializable]
	public class Endorsement
	{
		/// <summary>
		/// Identificador
		/// </summary>
		[DataMember]
		public int Id { get; set; }

		/// <summary>
		/// Número de Endoso
		/// </summary>
		[DataMember]
		public int Number { get; set; }

		[DataMember]
		public DateTime IssueDate { get; set; }

		/// <summary>
		/// Vigencia Inicial
		/// </summary>
		[DataMember]
		public DateTime CurrentFrom { get; set; }

		/// <summary>
		/// Vigencia Final
		/// </summary>
		[DataMember]
		public DateTime CurrentTo { get; set; }

		[DataMember]
		public List<Risk> Risks { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public int EndorsementNumber { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public int InsuredCd { get; set; }

        [DataMember]
        public string InsuredName { get; set; }

        [DataMember]
        public string OperationType { get; set; }

        [DataMember]
        public decimal Prime { get; set; }

        [DataMember]
        public decimal InsuredAmount { get; set; }

        [DataMember]
        public decimal ResponsibilityMaximumAmount { get; set; }
    }

	[DataContract]
	[Serializable]
	public class Risk
	{
		[DataMember]
		public int Id { get; set; }

		/// <summary>
		/// Número de Riesgo
		/// </summary>
		[DataMember]
		public int Number { get; set; }

		/// <summary>
		/// Asegurado principal
		/// </summary>
		[DataMember]
		public int IndividualId { get; set; }

		[DataMember]
		public string Longitude { get; set; }

		/// <summary>
		/// Latitud
		/// </summary>
		[DataMember]
		public string Latitude { get; set; }

		/// <summary>
		/// Dirección del Riego
		/// </summary>
		[DataMember]
		public string Address { get; set; }

		[DataMember]
		public List<Coverage> Coverages { get; set; }

	}

	[DataContract]
	[Serializable]
	public class Coverage
	{
		/// <summary>
		/// Código de Línea de Negocio
		/// </summary>
		[DataMember]
		public int LineBusinessId { get; set; }
		/// <summary>
		/// Código de Sub Línea de Negocio
		/// </summary>
		[DataMember]
		public int SubLineBusinessId { get; set; }
		/// <summary>
		/// Id de la Cobertura
		/// </summary>
		[DataMember]
		public int Id { get; set; }
		/// <summary>
		/// Número de cobertura
		/// </summary>
		[DataMember]
		public int Number { get; set; }
		/// <summary>
		/// Id Objeto Asegurado
		/// </summary>
		[DataMember]
		public int InsuredObjectId { get; set; }

		/// <summary>
		/// Es Factultativo
		/// </summary>
		[DataMember]
		public bool Facultative { get; set; }

		/// <summary>
		/// Límite de endoso
		/// </summary>
		[DataMember]
		public Amount LimitAmount { get; set; }

		/// <summary>
		/// Premio
		/// </summary>
		[DataMember]
		public Amount Premium { get; set; }

        #region "Campos de reaseguro"

        /// <summary>
        /// Código de Línea de Contrato de Reaseguros
        /// </summary>
        [DataMember]
        public int LineId { get; set; }

        /// <summary>
        /// Clave cúmulo de reaseguro
        /// </summary>
        [DataMember]
        public string CumulusKey { get; set; }

        /// <summary>
        /// Código de Error al asiganr Línea 
        /// </summary>
        [DataMember]
        public int ErrorId { get; set; }

        #endregion


    }


}
