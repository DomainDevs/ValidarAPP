using Sistran.Core.Application.Vehicles.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.Models
{
    /// <summary>
    /// Motor
    /// </summary>
    [DataContract]
    public class CompanyEngine : BaseEngine
    {
        /// <summary>
        /// propiedad EngineType
        /// </summary>
        [DataMember]
        public CompanyEngineType EngineType { get; set; }

    }
}
