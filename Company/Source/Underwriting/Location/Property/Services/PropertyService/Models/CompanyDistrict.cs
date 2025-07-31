using System.Runtime.Serialization;

namespace Sistran.Company.Application.Location.PropertyServices.Models
{
    [DataContract]
    public class CompanyDistrict
    {
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Barrio
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Barrio
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
