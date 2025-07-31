using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyAssistanceType
    {
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// tipo de Asistencia
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Abreviatura ipo de Asistencia
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
