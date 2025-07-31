using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Vehicles.VehicleServices.Models
{
    /// <summary>
    /// Causas
    /// </summary>
    [DataContract]
    public class CompanyNotInsurableCause
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
