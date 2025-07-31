using Sistran.Core.Application.Finances.Models.Base;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Finances.Models
{
    [DataContract]
   public class FinancesRisk: BaseFinances
        
    {
        public FinancesRisk()
        {
            Risk = new Risk();
        }

        /// <summary>
        /// Información del riesgo
        /// </summary>
        [DataMember]
        public Risk Risk { get; set; }

        /// <summary>
        /// Profesión
        /// </summary>
        public IssuanceOccupation Occupation { get; set; }
    }
}
