using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{

    [DataContract]
    public class CompanyQuotationSearch
    {
        /// <summary>
        /// Identificador
        /// </summary>   
        [DataMember]
        public int QuotationNumber { get; set; }

        /// <summary>
        /// Versión de la Cotización
        /// </summary>
        [DataMember]
        public int Version { get; set; }

        /// <summary>
        /// Ramo comercial
        /// </summary>
        [DataMember]
        public string PrefixCommercial { get; set; }

        /// <summary>
        /// Asegurado
        /// </summary>
        [DataMember]
        public string Insured { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        [DataMember]
        public string Branch { get; set; }

        /// <summary>
        /// Moneda de la emisión
        /// </summary>
        [DataMember]
        public string CurrencyIssuance { get; set; }

        /// <summary>
        /// Prima total
        /// </summary>
        [DataMember]
        public string TotalPremium { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [DataMember]
        public string User { get; set; }

        /// <summary>
        /// Fecha Cotizacion
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Dias
        /// </summary>
        [DataMember]
        public int Days { get; set; }

        /// <summary>
        /// Intermediario principal
        /// </summary>
        [DataMember]
        public string AgentPrincipal { get; set; }

        /// <summary>
        /// Producto    
        /// </summary>
        [DataMember]
        public string Product { get; set; }

        /// <summary>
        /// Id Operacion
        /// </summary>
        [DataMember]
        public int OperationId { get; set; }
    }
}
