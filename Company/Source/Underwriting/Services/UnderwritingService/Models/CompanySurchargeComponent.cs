using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanySurchargeComponent
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public CompanyRateType RateType { get; set; }
        [DataMember]
        public decimal Rate { get; set; }

        /// <summary>
        /// Estado del registro de parametrizacion
        /// </summary>
        [DataMember]
        public int State { get; set; }
    }
}
