using System.Runtime.Serialization;

namespace Sistran.Company.Application.Location.PropertyServices.Models
{
    [DataContract]
    public class CompanyUseHouse
    {

        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Vivienda
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Abreviatura de Vivienda
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Tipo de vivienda
        /// </summary>
        [DataMember]
        public CompanyHouseType CompanyHouseType { get; set; }
    }
}
