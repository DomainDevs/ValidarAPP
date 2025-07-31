using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base;
using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting
{
    /// <summary>
    /// Poliza
    /// </summary>
    [DataContract]
    public class Policy : BasePolicy
    {

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
        /// punto de venta de la poliza
        /// </summary>
        [DataMember]
        public virtual Branch Branch { get; set; }

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
        /// Endoso
        /// </summary>
        [DataMember]
        public virtual Endorsement Endorsement { get; set; }
		
    }
}