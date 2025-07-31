using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using SearchDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.DTOs;

namespace Sistran.Core.Application.AccountingServices
{
    [ServiceContract]
    public interface IAccountingPaymentService
    {

        #region PaymentDAO

        /// <summary>
        /// SavePayment - nuevo Savepayment
        /// Graba un nuevo registro en la tabla PAYMENT
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="collectId"></param>
        /// <returns>Payment</returns>
        [OperationContract]
        PaymentDTO SavePayment(PaymentDTO payment, int collectId);

        /// <summary>
        /// UpdatePayment
        /// Actualiza un registro existente en la tabla PAYMENT
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="collectCode"></param>
        /// <returns>Payment</returns>
        [OperationContract]
        PaymentDTO UpdatePayment(PaymentDTO payment, int collectCode);

        /// <summary>
        /// DeletePayment
        /// Elimina un registro de la tabla PAYMENT
        /// </summary>
        /// <param name="payment"></param>
        /// <returns>void</returns>
        [OperationContract]
        void DeletePayment(PaymentDTO payment);


        /// <summary>
        /// GetPayment
        /// Obtiene un registro de la tabla PAYMENT
        /// </summary>
        /// <param name="payment"></param>
        /// <returns>Payment</returns>
        /// 
        [OperationContract]
        PaymentDTO GetPayment(PaymentDTO payment);

        /// <summary>
        /// GetPayments
        /// Obtiene todos los registros de la tabla PAYMENT
        /// </summary>
        /// <returns>List<Models.Payment/></returns>
        [OperationContract]
        List<PaymentDTO> GetPayments();

        #endregion



        /// <summary>
        /// DetailValues
        /// </summary>
        /// <param name="collectId"></param>        
        /// <returns>List<DetailValuesDTO/></returns>
        [OperationContract]
        List<SearchDTO.DetailValuesDTO> DetailValues(int collectId);


        /// <summary>
        /// ValidateCheckBankOrTransfer
        /// Valida que no se ingrese el mismo número de transferencia, cuenta para un mismo banco
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="checkNumber"></param>
        /// <param name="accountNumber"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ValidateCheckBankOrTransfer(int bankId, string checkNumber, string accountNumber);


        /// <summary>
        /// ValidateVoucher
        /// Valida que no se ingrese el mismo número de voucher para un mismo número de tarjeta
        /// </summary>
        /// <param name="creditCardNumber"></param>
        /// <param name="voucherNumber"></param>
        /// <param name="conduitType"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ValidateVoucher(string creditCardNumber, string voucherNumber, int conduitType);

        /// <summary>
        /// ValidateDepositVoucher
        /// Valida que no se ingrese el mismo número de cheque, cuenta para un mismo banco
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="numberDoc"></param>
        /// <param name="accountNumber"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ValidateDepositVoucher(int bankId, string numberDoc, string accountNumber);


        /// <summary>
        /// GetPaymentByBankIdAndDocumentNumber
        /// Obtiene la cantidad de pagos por id de banco y número de documento, si es > 0 devuelve verdadero caso contrario falso
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="documentNumber"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool GetPaymentByBankIdAndDocumentNumber(int bankId, string documentNumber);

        /// <summary>
        /// GetCheckInformation
        /// Obtiene la información del cheque en particular de la tabla PAYMENT
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="documentNumber"></param>
        /// <returns>List<CheckInformationDTO/></returns>
        [OperationContract]
        List<SearchDTO.CheckInformationDTO> GetCheckInformation(int bankId, string documentNumber);

        /// <summary>
        /// GetCheckInformationGrid
        /// Obtiene la información de todos los cheques pagados de la tabla PAYMENT
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns> List<CheckInformationGridDTO/></returns>
        [OperationContract]
        List<SearchDTO.CheckInformationGridDTO> GetCheckInformationGrid(int paymentId);

        /// <summary>
        /// GetRejectedPaymentByBankIdAndDocumentNumber
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="documentNumber"></param>
        /// <returns>List<RejectedPaymentDTO/></returns>
        [OperationContract]
        List<SearchDTO.RejectedPaymentDTO> GetRejectedPaymentByBankIdAndDocumentNumber(int bankId, string documentNumber);

        /// <summary>
        /// GetPaymentBallotInfoByPaymentId
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>PaymentBallot</returns>
        [OperationContract]
        SearchDTO.PaymentBallotDTO GetPaymentBallotInfoByPaymentId(int paymentId);

        #region LegalPayment

        ///<summary>
        /// SaveLegalPayment
        /// </summary>
        /// <param name="legalPaymentId"></param>
        /// <param name="rejectedPaymentId"></param>
        /// <param name="legalDate"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveLegalPayment(int legalPaymentId, int rejectedPaymentId, DateTime legalDate);

        ///<summary>
        /// SaveLegalPaymentRequest
        /// Graba LegalPayment Enviando un objeto
        /// </summary>
        /// <param name="legalPaymentDto"></param>
        ///<param name="payerId"> </param>
        ///<param name="descriptionLegalize"> </param>
        ///<returns>int</returns>
        [OperationContract]
        MessageSuccessDTO SaveLegalPaymentRequest(SearchDTO.LegalPaymentDTO legalPaymentDto, int payerId, string descriptionLegalize,int branchId, int userId,DateTime accountingDate);

        #endregion

        #region RejectedPayment


        /// <summary>
        /// SaveRejectedPayment
        /// </summary>
        /// <param name="rejectedPayment"></param>
        /// <param name="userId"></param>
        /// <param name="registerDate"></param>
        /// <param name="collectId"></param>
        /// <param name="payerId"></param>
        /// <param name="description"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveRejectedPayment(RejectedPaymentDTO rejectedPayment, int userId, DateTime registerDate, int collectId, int payerId, string description, int branchId);

       
        /// <summary>
        /// GetRejectedCardVoucherInfoByPaymentId 
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>RejectedCardVoucherInfoDTO</returns>
        [OperationContract]
        SearchDTO.RejectedCardVoucherInfoDTO GetRejectedCardVoucherInfoByPaymentId(int paymentId);

        /// <summary>
        /// GetRejectedCardVoucherInfoByPaymentId 
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>RejectedCardVoucherInfoDTO</returns>
        [OperationContract]
        SearchDTO.RejectedPaymentDTO GetRejectedCheckInfoByPaymentId(int paymentId);

        /// <summary>
        /// Guardar cheque rechazado y realizar collects
        /// </summary>
        /// <param name="rejectedPayment"></param>
        /// <param name="billId"></param>
        /// <param name="payerId"></param>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="voucherRejection"></param>
        /// <param name="forCheckRejection"></param>
        /// <returns></returns>
        [OperationContract]
        MessageSuccessDTO SaveCheckingRejection(RejectedPaymentDTO rejectedPayment, int billId, int payerId, int branchId, int userId, string voucherRejection, string forCheckRejection, DateTime accountingDate);

        #endregion

        #region PaymentLog

        /// <summary>
        /// SavePaymentLog
        /// </summary>
        /// <param name="actionTypeId"></param>
        /// <param name="sourceId"></param>
        /// <param name="paymentId"></param>
        /// <param name="status"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        void SavePaymentLog(int actionTypeId, int sourceId, int paymentId, int status, int userId);

        #endregion

   

        #region CollectReports

        /// <summary>
        /// GetReportPayment
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="branch"></param>
        /// <param name="uiviewName"></param>
        /// <param name="collectCode"></param>
        /// <returns>PaginatedResponse</returns>
        [OperationContract]
        List<SearchDTO.ReportCollectDTO> GetReportPayment(int userId, int branch, string uiviewName, int collectCode);

        #endregion

        #region SearchCheck
        /// <summary>
        /// GetChecks
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <param name="bankCode"></param>
        /// <param name="accountNumber"></param>
        /// <param name="checkNumber"></param>
        /// <returns>List<CheckInformationDTO/></returns>
        [OperationContract]
        List<SearchDTO.CheckInformationDTO> GetChecks(int paymentCode, int bankCode, string accountNumber, string checkNumber);

        /// <summary>
        /// GetInternalBallotInformation
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>List<DepositCheckInformationDTO/></returns>
        [OperationContract]
        List<SearchDTO.DepositCheckInformationDTO> GetInternalBallotInformation(int paymentCode);

        /// <summary>
        /// GetDepositInformation
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>List<CheckInformationDTO/></returns>
        [OperationContract]
        List<SearchDTO.DepositCheckInformationDTO> GetDepositInformation(int paymentCode);

        /// <summary>
        /// GetRejectedInformation
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>List<RejectedPaymentDTO/></returns>
        [OperationContract]
        List<SearchDTO.RejectedPaymentDTO> GetRejectedInformation(int paymentCode);

        /// <summary>
        /// GetRegularizedInformation
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>List<CheckInformationDTO/></returns>
        [OperationContract]
        List<SearchDTO.RegularizedPaymentDTO> GetRegularizedInformation(int paymentCode);

        /// <summary>
        /// GetLegalInformation
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>List<LegalPaymentDTO/></returns>
        [OperationContract]
        List<SearchDTO.LegalPaymentDTO> GetLegalInformation(int paymentCode);

        /// <summary>
        /// Graba regularizacion y contabilidad
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="billControlId"></param>
        /// <param name="sourcePaymentId"></param>
        /// <param name="branchId"></param>
        /// <param name="accountingDate"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        [OperationContract]
        MessageSuccessDTO SaveRegularizationCollect(CollectDTO collect, int billControlId, int sourcePaymentId, int branchId, DateTime accountingDate, int userId);

        #endregion

        #region GetChecksUpdated

        /// <summary>
        /// GetChecksUpdated
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="checkNumber"></param>
        /// <param name="accountNumber"></param>
        /// <param name="technicalTransaction"></param>
        /// <param name="branchCode"></param>
        /// <returns>List<CheckInformationDTO/></returns>
        [OperationContract]
        List<SearchDTO.CheckInformationDTO> GetChecksUpdated(int bankCode, string checkNumber, string accountNumber, int technicalTransaction, int branchCode);

        /// <summary>
        /// SaveChangeCheck
        /// Cambia el estado del cheque original a canjeado e
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="paymentIdSource"></param>
        /// <param name="collectCode"></param>
        /// <param name="userId"></param>
        /// <param name="payerId"></param>
        /// <param name="changeDescription"></param>
        /// <returns>int</returns>
        [OperationContract]
        MessageSuccessDTO SaveChangeCheck(PaymentDTO payment, int paymentIdSource, int collectCode, int userId, int payerId, string changeDescription, DateTime accountingDate);

        #endregion

        #region CardVoucher

        /// <summary>
        /// GetCardVoucher
        /// </summary>
        /// <param name="creditCardTypeCode"></param>
        /// <param name="voucher"></param>
        /// <param name="documentNumber"></param>
        /// <param name="technicalTransaction"></param>
        /// <param name="branchCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="status"></param>
        /// <returns>List<CardVoucherDTO/></returns>
        [OperationContract]
        List<SearchDTO.CardVoucherDTO> GetCardVoucher(int creditCardTypeCode, string voucher, long documentNumber, int technicalTransaction, int branchCode, DateTime startDate, DateTime endDate, int status);

        /// <summary>
        /// GetInformationPayment
        /// </summary> 
        /// <param name="paymentCode"></param>
        /// <returns>PaginatedResponse</returns>
        [OperationContract]
        List<SearchDTO.InformationPaymentDTO> GetInformationPayment(int paymentCode);

        /// <summary>
        /// GetTaxInformationByPaimentId
        /// </summary> 
        /// <param name="paymentId"></param>
        /// <returns>List<TaxInformationDTO/></returns>
        [OperationContract]
        List<SearchDTO.TaxInformationDTO> GetTaxInformationByPaimentId(int paymentId);

        #endregion

        #region CheckingRejection

        /// <summary>
        /// GetCheckInformationByDocumentNumber
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <returns>List<InformationPaymentDTO/></returns>
        [OperationContract]
        List<SearchDTO.CheckInformationDTO> GetCheckInformationByDocumentNumber(string documentNumber);

        #endregion

        #region PaymentTax

        /// <summary>
        /// SavePaymentTax
        /// </summary>
        /// <param name="paymentTax"></param>
        /// <param name="paymentId"></param>
        /// <returns>PaymentTax</returns>
        [OperationContract]
        PaymentTaxDTO SavePaymentTax(PaymentTaxDTO paymentTax, int paymentId);

        /// <summary>
        /// DeletePaymentTax
        /// Elimina un registro de la tabla PAYMENT
        /// </summary>
        /// <param name="paymentTax"></param>
        [OperationContract]
        void DeletePaymentTax(PaymentTaxDTO paymentTax);

        #endregion

        #region StatusPayment
        /// <summary>
        /// GetStatusPaymentByMethodTypeId
        /// </summary> 
        /// <param name="methodTypeId"></param>
        /// <returns>List<StatusPaymentDTO/></returns>
        [OperationContract]
        List<SearchDTO.StatusPaymentDTO> GetStatusPaymentByMethodTypeId(int methodTypeId);

        /// <summary>
        /// UpdateStatusPayment
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="status"></param>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        [OperationContract]
        void UpdateStatusPayment(int methodId, int status, string descripcion);

        /// <summary>
        /// DeleteStatusPayment
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [OperationContract]
        void DeleteStatusPayment(int methodId, int status);
        #endregion

        #region GetChecksDepositingPending

        /// <summary>
        /// GetChecksDepositingPending
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="branchCode"></param>
        /// <returns>List<CheckInformationDTO/></returns>
        [OperationContract]
        List<SearchDTO.CheckInformationDTO> GetChecksDepositingPending(int bankCode, DateTime startDate, DateTime endDate, int branchCode);

        /// <summary>
        /// GetReportChecksDepositingPending
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="issuingBankCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="branchCode"></param>
        /// <returns>List<CheckInformationDTO/></returns>
        [OperationContract]
        List<SearchDTO.CheckInformationDTO> GetReportChecksDepositingPending(int userId, int issuingBankCode, string startDate, string endDate, int branchCode);

        /// <summary>
        /// GetReportCardsDepositingPending
        /// Obtiene la cabecera y detalle de los cheques pendientes de depositar para la impresión
        /// </summary>
        /// <param name="creditCardTypeCode"></param>
        /// <param name="voucher"></param>
        /// <param name="documentNumber"></param>
        /// <param name="collectCode"></param>
        /// <param name="branchCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="status"></param>
        /// <returns>PaginatedResponse</returns>
        [OperationContract]
        List<SearchDTO.CardVoucherDTO> GetReportCardsDepositingPending(string creditCardTypeCode, string voucher, string documentNumber, string collectCode, string branchCode, string startDate, string endDate, string status);

        #endregion

        #region ReprintCollect

        /// <summary>
        /// ItemsCollect
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool ItemsCollect(int collectId);

        #endregion

        #region TempPaymentRequestClaim

        /// <summary>
        /// SaveTempPaymentRequest
        /// </summary>
        /// <param name="paymentRequestInfo"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveTempPaymentRequest(PaymentRequestDTO paymentRequestInfo);

        /// <summary>
        /// ConvertTempPaymentRequestToPaymentRequest
        /// </summary>
        /// <param name="tempPaymentRequestId"></param>
        /// <returns>PaymentRequestDTO</returns>
        [OperationContract]
        PaymentRequestDTO ConvertTempPaymentRequestToPaymentRequest(int tempPaymentRequestId);

        #endregion

        /// <summary>
        /// GetTaxCard
        /// </summary>
        /// <param name="creditCardTypeId"></param>
        /// <param name="branchId"></param>
        /// <returns>Payment</returns>
        [OperationContract]
        PaymentDTO GetTaxCreditCard(int creditCardTypeId, int branchId);


        /// <summary>
        /// Get bill report information
        /// </summary>
        /// <param name="technicalTransaction">Technical transaction</param>
        /// <returns>Bill report information</returns>
        [OperationContract]
        BillReportDTO GetBillReport(int technicalTransaction);
    }
}
