using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.MassiveServices.Models
{
    /// <summary>
    /// Solicitud Agrupadora
    /// </summary>
    [DataContract]
    public class CoRequest
    {
        /// <summary>
        /// Atributo para la propiedad Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Atributo para la propiedad Sucursal
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        /// Atributo para la propiedad Ramo
        /// </summary>
        [DataMember]
        public Prefix Prefix { get; set; }

        /// <summary>
        /// Atributo para la propiedad Fecha Solicitud
        /// </summary>
        [DataMember]
        public DateTime RequestDate { get; set; }

        /// <summary>Description
        /// Atributo para la propiedad Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Atributo para la propiedad Endosos Solicitud
        /// </summary>
        [DataMember]
        public List<CoRequestEndorsement> CoRequestEndorsement { get; set; }

        /// <summary>
        /// Atributo para la propiedad Tipo de Negocio
        /// </summary>
        [DataMember]
        public BusinessType? BusinessType { get; set; }

        /// <summary>
        /// Atributo para la propiedad Grupo de Facturacion
        /// </summary>
        [DataMember]
        public BillingGroup BillingGroup { get; set; }

        /// <summary>
        /// Obtiene o setea el Tipo de Negocio Coaceguro Cedido y Aceptado 100 % compañia
        /// </summary>
        /// <value>
        /// CoInsuranceCompany
        /// </value>}
        [DataMember]
        public List<CoInsuranceCompany> InsuranceCompanies { get; set; }
    }
}