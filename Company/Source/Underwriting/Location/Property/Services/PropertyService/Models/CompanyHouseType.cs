using System.Runtime.Serialization;

namespace Sistran.Company.Application.Location.PropertyServices.Models
{
    [DataContract]
    public class CompanyHouseType
    {
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// tipo de Vivienda
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Abreviatura de tipo de Vivienda
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
