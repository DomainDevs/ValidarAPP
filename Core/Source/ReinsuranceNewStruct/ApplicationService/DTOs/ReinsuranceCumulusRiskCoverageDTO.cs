using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
	public class ReinsuranceCumulusRiskCoverageDTO
	{
		/// <summary>
		/// Id 
		/// </summary>
		[DataMember]
		public int ReinsuranceCumulusRiskCoverageId { get; set; }
		/// <summary>
		/// Número del Riego
		/// </summary>
		[DataMember]
		public int RiskNumber { get; set; }
		
		/// <summary>
		/// Número de la cobertura
		/// </summary>
		[DataMember]
		public int CoverageNumber { get; set; }
	}
}
