using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Finances.FidelityServices.Models.Base;
using System.Runtime.Serialization;


namespace Sistran.Company.Application.Finances.FidelityServices.Models
{
    /// <summary>
    /// Riesgo de Manejo
    /// </summary>
    [DataContract]
    public class CompanyFidelityRisk : BaseFidelityRisk
    {
        [DataMember]
        public CompanyRisk Risk { get; set; }

        public CompanyFidelityRisk()
        {
        }
    }
}
