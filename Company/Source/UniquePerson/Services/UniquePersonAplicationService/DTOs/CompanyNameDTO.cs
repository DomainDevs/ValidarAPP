using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    public class CompanyNameDTO
    {
        /// <summary>
        /// datos direccion
        /// </summary>
        [DataMember]
        public int AddressID { get; set; }


        /// <summary>
        /// datos telefono
        /// </summary>
        [DataMember]
        public int PhoneID { get; set; }
        /// <summary>
        /// datos email
        /// </summary>
        [DataMember]
        public int EmailID { get; set; }
        /// <summary>
        /// Id Individuo
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int NameNum { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string TradeName { get; set; }

        /// <summary>
        /// Es Principal
        /// </summary>
        [DataMember]
        public bool IsMain { get; set; }


        /// <summary>
        /// Dato para emision
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int OperationId { get; set; }
    }
}
