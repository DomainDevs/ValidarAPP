using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Vehicles.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.Models
{
    /// <summary>
    /// Años de la versión del Vehiculo
    /// </summary>
    [DataContract]
    public class CompanyYear : BaseYear
    {

        /// <summary>
        /// Moneda
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }
    }
}
