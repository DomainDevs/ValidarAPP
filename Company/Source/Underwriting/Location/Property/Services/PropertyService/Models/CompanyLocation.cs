using System.Runtime.Serialization;

namespace Sistran.Company.Application.Location.PropertyServices.Models
{
    [DataContract]
    public class CompanyLocation
    {
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Localidad
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Localidad
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Barrio
        /// </summary>
        [DataMember]
        public CompanyDistrict CompanyDistrict { get; set; }

    }
}
