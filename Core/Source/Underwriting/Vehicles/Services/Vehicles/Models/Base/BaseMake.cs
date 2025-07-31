using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.Models.Base
{
    /// <summary>
    /// Marca
    /// </summary>
    [DataContract]
    public class BaseMake : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Marca
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
