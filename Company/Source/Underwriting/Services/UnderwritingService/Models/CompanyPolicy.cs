using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Poliza Cia
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UnderwritingServices.Models.Policy" />
    [DataContract]
    public class CompanyPolicy : BasePolicy
    {
        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        [DataMember]
        public CompanySummary Summary { get; set; }

        /// <summary>
        /// Gets or sets the dynamic properties.
        /// </summary>
        /// <value>
        /// The dynamic properties.
        /// </value>
        [DataMember]
        public List<DynamicConcept> DynamicProperties { get; set; }

        [DataMember]
        public CompanyProduct Product { get; set; }

        /// <summary>
        /// Ramo Comercial
        /// </summary>
        [DataMember]
        public virtual CompanyPrefix Prefix { get; set; }

        /// <summary>
        /// Tomador
        /// </summary>
        /// <value>
        /// The payer components.
        /// </value>
        [DataMember]
        public List<CompanyPayerComponent> PayerComponents { get; set; }

        /// <summary>
        /// Gets or sets the default beneficiaries.
        /// </summary>
        /// <value>
        /// The default beneficiaries.
        /// </value>
        [DataMember]
        public Holder Holder { get; set; }

        [DataMember]
        public List<CompanyBeneficiary> DefaultBeneficiaries { get; set; }

        /// <summary>
        /// Clausulas
        /// Gets or sets the temporal type description.
        /// </summary>
        [DataMember]
        public virtual List<CompanyClause> Clauses { get; set; }

        /// <summary>
        /// Intermediarios asociados
        /// </summary>
        [DataMember]
        public virtual List<IssuanceAgency> Agencies { get; set; }

        [DataMember]
        public CompanyBillingGroup BillingGroup { get; set; }

        [DataMember]
        public Request Request { get; set; }

        [DataMember]
        public CompanyPolicyType PolicyType { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public List<PoliciesAut> InfringementPolicies { get; set; }

        /// <summary>
        /// Lista de compañias coaseguradoras
        /// </summary>
        /// <value>
        /// The endorsement.
        /// </value>
        [DataMember]
        public CompanyEndorsement Endorsement { get; set; }

        /// <summary>
        /// Gets or sets the exchange rate.
        /// </summary>
        /// <value>
        /// The exchange rate.
        /// </value>
        [DataMember]
        public List<CompanyIssuanceCoInsuranceCompany> CoInsuranceCompanies { get; set; }

        /// <summary>
        /// Planes de pago de la poliza
        /// Gets or sets the holder.
        /// </summary>
        [DataMember]
        public CompanyPaymentPlan PaymentPlan {     get; set; }

        /// <summary>
        /// Modena de la poliza
        /// </summary>
        [DataMember]
        public ExchangeRate ExchangeRate { get; set; }

        /// <value>
        /// The branch.
        /// </value>
        [DataMember]
        public CompanyBranch Branch { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        [DataMember]
        public virtual CompanyText Text { get; set; }

        /// <summary>
        /// Es Agente?
        /// </summary>
        [DataMember]
        public bool IsAgent { get; set; }
        /// <summary>
        /// <summary>
        /// Nombre Cuenta
        /// </summary>
        [DataMember]
        public string AccountName { get; set; }

     
        /// <summary>
        /// Punto de Venta Aliado
        /// </summary>
        [DataMember]
        public int AllianceSalePointId { get; set; }

        /// <summary>
        /// Número Propuesta
        /// </summary>
        [DataMember]
        public string ProposalNumber { get; set; }

        /// <summary>
        /// Id Endoso
        /// </summary>
        [DataMember]
        public int RequestEndorsement { get; set; }

        /// <summary>
        /// Lista de Componentes primer pago
        /// </summary>
        [DataMember]
        public List<CompanyPayerComponent> ListFirstPayComponent { get; set; }

        /// <summary>
        /// Mensaje de Proceso
        /// </summary>
        [DataMember]
        public string ProcessMessage { get; set; }

        /// <summary>
        /// Id Negocio
        /// </summary>
        [DataMember]
        public int BusinessId { get; set; }

        /// <summary>
        /// Agrupación Cotizaciones
        /// </summary>
        [DataMember]
        public int GroupQuoteId { get; set; }

        /// <summary>
        /// Agrupación Cotizaciones
        /// </summary>
        [DataMember]
        public Guid Guid { get; set; }

        /// <summary>
        /// Response Product Id
        /// </summary>
        [DataMember]
        public string ProductIdResponse { get; set; }

        /// <summary>
        /// Id de la poliza corraletiva
        /// </summary>
        [DataMember]
        public decimal? CorrelativePolicyNumber { get; set; }
        /// <summary>
        /// Número Póliza Externo
        /// </summary>
        [DataMember]
        public int ExternalPolicyNumber { get; set; }

        [DataMember]
        public List<PayerPayment> PayerPayments { get; set; }

        /// <summary>
        /// Número Póliza 2G
        /// </summary>
        [DataMember]
        public int Policy2G { get; set; }

        /// <summary>
        /// Cantidad de Renovacion
        /// </summary>
        [DataMember]
        public int RenewalsQuantity { get; set; }

        /// <summary>
        /// Cantidad de siniestros en los ultimos 3 años 
        /// </summary>
        [DataMember]
        public int SinisterQuantityLastYears { get; set; }

        /// <summary>
        /// Lista de Errores
        /// </summary>
        [DataMember]
        public List<ErrorBase> Errors { get; set; }

        /// <summary>
        /// Company User 
        /// </summary>
        [DataMember]
        public CompanyPolicyUser User { get; set; }

        /// <summary>
        /// Company CompanySummaryComponent 
        /// </summary>
        [DataMember]
        public CompanySummaryComponent SummaryComponent { get; set; }

        /// <summary>
        /// Company CompanySummaryComponent 
        /// </summary>
        [DataMember]
        public CompanyAcceptCoInsurance AcceptCoInsurance { get; set; }

        /// <summary>
        /// TicketNumber
        /// </summary>
        [DataMember]
        public int? TicketNumber { get; set; }

        /// <summary>
        /// TicketNumber
        /// </summary>
        [DataMember]
        public DateTime? TicketDate { get; set; }

        /// <summary>
        /// Justificación sarlaf
        /// </summary>
        [DataMember]
        public CompanyJustificationSarlaft JustificationSarlaft { get; set; }

        /// <summary>
        /// SubMassiveProcessType
        /// </summary>
        [DataMember]
        public SubMassiveProcessType? SubMassiveProcessType { get; set; }

        /// <summary>
        /// Origen de poliza
        /// </summary>
        [DataMember]
        public PolicyOrigin PolicyOrigin { get; set; }

        /// <summary>
        /// Cantidad de riesgos
        /// </summary>
        [DataMember]
        public int? TotalRisk { get; set; }

        /// <summary>
        /// Es R2?
        /// </summary>
        [DataMember]
        public bool AppSourceR2 { get; set; }
        [DataMember]
        public bool IsReinsured { get; set; }
        
    }
}