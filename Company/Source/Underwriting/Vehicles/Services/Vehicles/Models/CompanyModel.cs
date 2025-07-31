using Sistran.Core.Application.Vehicles.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.Models
{
    /// <summary>
    /// Modelo
    /// </summary>
    [DataContract]
    public class CompanyModel : BaseModel
    {

        /// <summary>
        /// Marca
        /// </summary>
        [DataMember]
        public CompanyMake Make { get; set; }
    }
}
