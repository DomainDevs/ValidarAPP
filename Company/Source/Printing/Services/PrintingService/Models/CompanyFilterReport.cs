using Sistran.Core.Application.PrintingServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.PrintingServices.Models
{
    [DataContract]
    public class CompanyFilterReport : FilterReport
    {
        /// <summary>
        /// Adjuntar Resumen De Riesgos
        /// </summary>
        [DataMember]
        public bool AttachRisksSummary { get; set; }
    }
}