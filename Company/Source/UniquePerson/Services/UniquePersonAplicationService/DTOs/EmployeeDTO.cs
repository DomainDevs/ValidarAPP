using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class EmployeeDTO
    {
        /// <summary>
        /// IndividualId
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// Codigo del Empleado
        /// </summary>
        [DataMember]
        public string FileNumber { get; set; }

        /// <summary>
        /// Fecha ingreso
        /// </summary>
        [DataMember]
        public DateTime? EntryDate { get; set; }

        /// <summary>
        /// Fecha retiro
        /// </summary>
        [DataMember]
        public DateTime? EgressDate { get; set; }

        /// <summary>
        /// Fecha de baja
        /// </summary>
        [DataMember]
        public DateTime? ModificationDate { get; set; }

        /// <summary>
        /// Motivo de baja id
        /// </summary>
        [DataMember]
        public int? DeclinedTypeId { get; set; }

        /// <summary>
        /// Descripción Empleado
        /// </summary>
        [DataMember]
        public string Annotation { get; set; }

        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int OperationId { get; set; }

    }
}
