using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Vehicles.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.Models
{
    /// <summary>
    /// Años de la versión del Vehiculo
    /// </summary>
    [DataContract]
    public class Year : BaseYear
    {

        /// <summary>
        /// Moneda
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }
    }
}
