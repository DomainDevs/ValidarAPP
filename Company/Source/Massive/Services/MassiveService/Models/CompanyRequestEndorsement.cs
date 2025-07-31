using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.ProductServices.Models;

namespace Sistran.Company.Application.MassiveServices.Models
{
    [DataContract]
    public class CompanyRequestEndorsement
    {
        /// <summary>
        /// Atributo para la propiedad Id
        /// </summary> 
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Atributo para la propiedad EndorsementType
        /// </summary> 
        [DataMember]
        public EndorsementType? EndorsementType { get; set; }
        
        /// <summary>
        /// Atributo para la propiedad DocumentNum
        /// </summary> 
        [DataMember]
        public int DocumentNumber { get; set; }
        
        /// <summary>
        /// Atributo para la propiedad EndorsementDate
        /// </summary> 
        [DataMember]
        public DateTime EndorsementDate { get; set; }
        
        /// <summary>
        /// Atributo para la propiedad Holder
        /// </summary> 
        [DataMember]
        public Holder Holder { get; set; }

        /// <summary>
        /// Atributo para la propiedad CurrentFrom
        /// </summary> 
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Atributo para la propiedad CurrentTo
        /// </summary> 
        [DataMember]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Atributo para la propiedad MonthlPayDay
        /// </summary> 
        [DataMember]
        public short? MonthPayerDay { get; set; }
        
        /// <summary>
        /// Atributo para la propiedad IssueExpensesAmt
        /// </summary> 
        [DataMember]
        public decimal IssueExpensesAmount { get; set; }
        
        /// <summary>
        /// Atributo para la propiedad UserId
        /// </summary> 
        [DataMember]
        public int? UserId { get; set; }
        
        /// <summary>
        /// Atributo para la propiedad IsOpenEffect
        /// </summary> 
        [DataMember]
        public bool IsOpenEffect { get; set; }
        
        /// <summary>
        /// Atributo para la propiedad PaymentPlan
        /// </summary> 
        [DataMember]
        public PaymentPlan PaymentPlan { get; set; }

        /// <summary>Annotations
        /// Atributo para la propiedad Annotations
        /// </summary> 
        [DataMember]
        public string Annotations { get; set; }

        /// <summary>
        /// Atributo para la propiedad Product
        /// </summary> 
        [DataMember]
        public Product Product { get; set; }

        /// <summary>
        /// Atributo para la propiedad Prefix
        /// </summary> 
        [DataMember]
        public Prefix Prefix { get; set; }

        /// <summary>
        /// Atributo para la propiedad PolicyType
        /// </summary> 
        [DataMember]
        public PolicyType PolicyType { get; set; }

        /// <summary>
        /// Atributo para la propiedad AmtGift
        /// </summary> 
        [DataMember]
        public decimal? GiftAmount { get; set; }

        /// <summary>
        /// Atributo para la propiedad CoRequestAgent
        /// </summary> 
        [DataMember]
        public List<IssuanceAgency> Agencies { get; set; }
    }
}