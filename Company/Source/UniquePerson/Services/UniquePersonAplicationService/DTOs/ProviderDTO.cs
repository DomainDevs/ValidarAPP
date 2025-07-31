using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    using Core.Application.AuthorizationPoliciesServices.Models;

    [DataContract]
    public class ProviderDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// IndividualID Person
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Id Tipo de proveedor
        /// </summary>
        [DataMember]
        public int ProviderTypeId { get; set; }

        /// <summary>
        /// Id Tipo de origen
        /// </summary>
        [DataMember]
        public int OriginTypeId { get; set; }

        /// <summary>
        /// Id Tipo de baja
        /// </summary>
        [DataMember]
        public int? ProviderDeclinedTypeId { get; set; }

        /// <summary>
        /// Fecha de creación
        /// </summary>
        [DataMember]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Fecha de modificacion
        /// </summary>
        [DataMember]
        public DateTime? ModificationDate { get; set; }

        /// <summary>
        /// Fecha de baja
        /// </summary>
        [DataMember]
        public DateTime? DeclinationDate { get; set; }

        /// <summary>
        /// Observacion
        /// </summary>
        [DataMember]
        public string Observation { get; set; }

        /// <summary>
        /// Especialidad predeterminada
        /// </summary>
        [DataMember]
        public int? SpecialityDefault { get; set; }

        /// <summary>
        /// Listado Concepto de pago
        /// </summary>
        [DataMember]
        public List<ProviderPaymentConceptDTO> ProviderPaymentConcepts { get; set; }

        /// <summary>
        /// Especialidad de Proveedor
        /// </summary>
        [DataMember]
        public List<ProviderSpecialityDTO> ProviderSpecialities { get; set; }

        [DataMember]
        public int SupplierProfileId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int OperationId { get; set; }

        [DataMember]
        public List<GroupSupplierDTO> GroupSupplier { get; set; }

    }
}
