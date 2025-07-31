using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.Models.Base
{

    /// <summary>
    /// Modelo
    /// </summary>
    [DataContract]
    public class BaseModel:Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Modelo
        /// </summary>
        [DataMember]
        public string Description { get; set; }

    }
}
