using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Poliza
    /// </summary>
    [DataContract]
    public class Policy : BasePolicy
    {
        /// <summary>
        /// Texto asociadas a la poliza
        /// </summary>
        [DataMember]
        public virtual Text Text { get; set; }

        /// <summary>
        /// Clausulas
        /// </summary>
        [DataMember]
        public virtual List<Clause> Clauses { get; set; }

        /// <summary>
        /// Intermediarios asociados
        /// </summary>
        [DataMember]
        public virtual List<IssuanceAgency> Agencies { get; set; }

        /// <summary>
        /// Ramo Comercial
        /// </summary>
        [DataMember]
        public virtual Prefix Prefix { get; set; }

        /// <summary>
        /// Producto
        /// </summary>
        [DataMember]
        public virtual Product Product { get; set; }

        /// <summary>
        /// punto de venta de la poliza
        /// </summary>
        [DataMember]
        public virtual Branch Branch { get; set; }

        /// <summary>
        /// Lista de compañias coaseguradoras
        /// </summary>
        [DataMember]
        public virtual List<IssuanceCoInsuranceCompany> CoInsuranceCompanies { get; set; }

        /// <summary>
        /// Tomador
        /// </summary>
        [DataMember]
        public virtual Holder Holder { get; set; }

        /// <summary>
        /// Planes de pago de la poliza
        /// </summary>
        [DataMember]
        public PaymentPlan PaymentPlan { get; set; }

        /// <summary>
        /// Modena de la poliza
        /// </summary>
        [DataMember]
        public ExchangeRate ExchangeRate { get; set; }

        /// <summary>
        /// Tipo de Poliza
        /// </summary>
        [DataMember]
        public virtual PolicyType PolicyType { get; set; }

        /// <summary>
        /// Solicitud Agrupadora
        /// </summary>
        [DataMember]
        public virtual Request Request { get; set; }

        /// <summary>
        /// Grupo de Facturación
        /// </summary>
        [DataMember]
        public virtual BillingGroup BillingGroup { get; set; }

        /// <summary>
        /// Lista de beneficiarios
        /// </summary>
        [DataMember]
        public virtual List<Beneficiary> DefaultBeneficiaries { get; set; }

        /// <summary>
        /// Componentes
        /// </summary>
        [DataMember]
        public virtual List<PayerComponent> PayerComponents { get; set; }

        /// <summary>
        /// Obtiene o establece las propiedades Dinamicas
        /// </summary>
        [DataMember]
        public virtual List<DynamicConcept> DynamicProperties { get; set; }

        /// <summary>
        /// Suma de Componentes
        /// </summary>
        [DataMember]
        public virtual Summary Summary { get; set; }

        /// <summary>
        /// Endoso
        /// </summary>
        [DataMember]
        public virtual Endorsement Endorsement { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }
		
        /// <summary>
        /// Trae los Riesgos Generados para la poliza.
        /// </summary>
        [DataMember]
        public Risk Risk { get; set; }
    }
}