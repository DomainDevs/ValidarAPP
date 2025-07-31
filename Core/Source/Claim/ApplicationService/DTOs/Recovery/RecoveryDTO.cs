using Sistran.Core.Application.ClaimServices.DTOs.Salvage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Recovery
{
    [DataContract]
    public class RecoveryDTO
    {
        /// <summary>
        /// Id 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// CreatedDate 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public DateTime? CreatedDate { get; set; }


        /// <summary>
        /// Description 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// CancellationReason 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int? CancellationReasonId { get; set; }

        /// <summary>
        /// CancellationDate 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public DateTime? CancellationDate { get; set; }

        /// <summary>
        /// Recuperator 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int RecuperatorId { get; set; }

        // <summary>
        /// RecuperatorFullName
        /// </summary>
        [DataMember]
        public string RecuperatorFullName { get; set; }

        /// <summary>
        /// DebtorDocumentNumber 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string RecuperatorDocumentNumber { get; set; }

        // <summary>
        /// RecuperatorNameDocument
        /// </summary>
        [DataMember]
        public string RecuperatorNameDocument { get; set; }

        /// <summary>
        /// RecoveryType 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int RecoveryTypeId { get; set; }

        /// <summary>
        /// RecoveryTypeDescription
        /// </summary>
        [DataMember]
        public string RecoveryTypeDescription { get; set; }

        /// <summary>
        /// PrescriptionDate 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public DateTime? PrescriptionDate { get; set; }

        /// <summary>
        /// Voucher 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string Voucher { get; set; }

        /// <summary>
        /// LossResponsible 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string LossResponsible { get; set; }

        /// <summary>
        /// AssignedCourt 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string AssignedCourt { get; set; }

        /// <summary>
        /// ExpedientNumber 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string ExpedientNumber { get; set; }

        /// <summary>
        /// PrescriptionDate 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public DateTime? AttorneyAssignmentDate { get; set; }

        /// <summary>
        /// LastReportDate 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public DateTime? LastReportDate { get; set; }

        /// <summary>
        /// DebtorId
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int? DebtorId { get; set; }

        /// <summary>
        /// DebtorFullName 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string DebtorFullName { get; set; }

        /// <summary>
        /// DebtorDocumentNumber 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string DebtorDocumentNumber { get; set; }

        /// <summary>
        /// DebtorAddress 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string DebtorAddress { get; set; }

        /// <summary>
        /// DebtorPhone 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string DebtorPhone { get; set; }

        /// <summary>
        /// Documentation 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int Documentation { get; set; }

        /// <summary>
        /// PaymentQuotas 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public List<PaymentQuotaDTO> PaymentQuotas { get; set; }

        /// <summary>
        /// TotalAmount 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// CompanyId
        /// </summary>
        [DataMember]
        public int CompanyId { get; set; }

        /// <summary>
        /// ClaimId
        /// </summary>
        [DataMember]
        public int ClaimId { get; set; }

        /// <summary>
        /// SubClaimId
        /// </summary>
        [DataMember]
        public int SubClaimId { get; set; }

        /// <summary>
        /// PrefixId
        /// </summary>
        [DataMember]
        public int PrefixId { get; set; }

        /// <summary>
        /// BranchId
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// ClaimNumber
        /// </summary>
        [DataMember]
        public int ClaimNumber { get; set; }

        /// <summary>
        /// Tomador de la póliza
        /// </summary>
        [DataMember]
        public int PolicyHolderId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int RecoveryClassId { get; set; }

        [DataMember]
        public bool DebtorIsParticipant { get; set; }

        [DataMember]
        public decimal RecoveryAmount { get; set; }

        #region PaymentPlan

        /// <summary>
        /// PaymentPlanId
        /// </summary>
        [DataMember]
        public int? PaymentPlanId { get; set; }

        /// <summary>
        /// Id de la moneda
        /// </summary>
        [DataMember]
        public int? CurrencyId { get; set; }

        [DataMember]
        public string CurrencyDescription { get; set; }

        /// <summary>
        /// id de clase de pago
        /// </summary>
        [DataMember]
        public int? PaymentClassId { get; set; }

        /// <summary>
        /// Porcentaje de impuesto
        /// </summary>
        [DataMember]
        public decimal? TaxPercentage { get; set; }

        #endregion
    }
}
