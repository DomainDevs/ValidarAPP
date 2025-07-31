using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.Vehicles.VehicleServices.Models.Base
{
    [DataContract]
    public class BaseUse : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Uso
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
