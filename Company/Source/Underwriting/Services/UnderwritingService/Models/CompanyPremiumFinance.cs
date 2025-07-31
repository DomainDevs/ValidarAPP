
using Sistran.Company.Application.UnderwritingServices.Enums;
using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyPremiumFinance
    {

        /// <summary>
        /// Tasa de Financiación
        /// </summary>
        [DataMember]
        public decimal? FinanceRate { get; set; }
        /// <summary>
        /// Estado del Pagare
        /// </summary>
        [DataMember]
        public StatePay StatePay { get; set; }
        /// <summary>
        /// Fecha desde
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Fecha hasta
        /// </summary>
        [DataMember]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Codigo y Nombre del asegurado
        /// </summary>
        [DataMember]
        public CompanyInsuredPremiumFinance Insured { get; set; }


        /// <summary>
        /// Número de Cuotas
        /// </summary>
        [DataMember]
        public int NumberQuotas { get; set; }

        /// <summary>
        /// Valor de la prima
        /// </summary>
        [DataMember]
        public decimal PremiumValue { get; set; }

        /// <summary>
        ///Porcentaje a Financial
        /// </summary>
        [DataMember]
        public decimal? PercentagetoFinance { get; set; }


        /// <summary>
        ///Valor Financiado
        /// </summary>
        [DataMember]
        public decimal FinanceValue { get; set; }


        /// <summary>
        ///Valor a financiar
        /// </summary>
        [DataMember]
        public decimal? FinanceToValue { get; set; }


        /// <summary>
        ///Valor minimo a financiar
        /// </summary>
        [DataMember]
        public decimal MinimumValueToFinance { get; set; }


        /// <summary>
        /// Numero pagare
        /// </summary>
        [DataMember]
        public int PromissoryNoteNumCode { get; set; }

    }
}
