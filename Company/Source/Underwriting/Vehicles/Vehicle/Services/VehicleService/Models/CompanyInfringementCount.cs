using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.VehicleServices.Models
{
    [DataContract]
    public class CompanyInfringementCount
    {
        /// <summary>
        /// Grupo de Infracción
        /// </summary>
        [DataMember]
        public int GroupInfringementCode { get; set; }

        /// <summary>
        /// Multas Ultimo Año
        /// </summary>
        [DataMember]
        public int InfringementsLastYear { get; set; }

        /// <summary>
        /// Multas Periodo 1
        /// </summary>
        [DataMember]
        public int InfringementsPeriodOne { get; set; }

        /// <summary>
        /// Multas Periodo 2
        /// </summary>
        [DataMember]
        public int InfringementsPeriodTwo { get; set; }
    }
}
