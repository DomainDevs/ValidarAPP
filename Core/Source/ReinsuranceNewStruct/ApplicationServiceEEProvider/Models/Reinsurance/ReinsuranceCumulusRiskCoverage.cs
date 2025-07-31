using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
	public class ReinsuranceCumulusRiskCoverage
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
