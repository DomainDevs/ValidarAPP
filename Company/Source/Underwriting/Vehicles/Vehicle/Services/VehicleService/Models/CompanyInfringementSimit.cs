using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.VehicleServices.Models
{
    [DataContract]
    public class CompanyInfringementSimit
    {
        /// <summary>
        /// Consultó Multas?
        /// </summary>
        [DataMember]
        public bool IsFine { get; set; }

        /// <summary>
        /// Fecha Consulta Multas
        /// </summary>
        [DataMember]
        public DateTime? FineDate { get; set; }

        /// <summary>
        /// Id Grupo de Consulta Multas
        /// </summary>
        [DataMember]
        public int? GroupFineId { get; set; }

        /// <summary>
        /// Conteo de Multas.
        /// </summary>
        [DataMember]
        public List<CompanyInfringementCount> ListInfringementCount { get; set; }

        //[DataMember]
        //public bool ToPersist { get; set; }
    }
}
