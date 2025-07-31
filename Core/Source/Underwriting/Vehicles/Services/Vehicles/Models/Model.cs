using Sistran.Core.Application.Vehicles.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.Models
{
    /// <summary>
    /// Modelo
    /// </summary>
    [DataContract]
    public class Model : BaseModel
    {

        /// <summary>
        /// Marca
        /// </summary>
        [DataMember]
        public Make Make { get; set; }
    }
}
