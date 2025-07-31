using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.AccountingServices.DTOs;
using ACPS = Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using SearchDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;

namespace Sistran.Core.Application.AccountingServices
{
    [ServiceContract]
    public interface IAccountingAccountsPayableService
    {
        #region CheckBookControl


        /// <summary>
        /// SaveCheckBookControl
        /// </summary>
        /// <param name="checkBookControl"></param>
        /// <returns>CheckBookControl</returns>
        [OperationContract]
        ACPS.CheckBookControlDTO SaveCheckBookControl(ACPS.CheckBookControlDTO checkBookControl);


        /// <summary>
        /// GetCheckBookControlsByAccountBankId
        /// </summary>  
        /// <param name="accountBankId"></param>     
        /// <returns>List<CheckBookControl/></returns>
        [OperationContract]
        List<ACPS.CheckBookControlDTO> GetCheckBookControlsByAccountBankId(int accountBankId);

        /// <summary>
        /// GetCheckBookControlsByBankIdBranchId
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="branchId"></param>
        /// <returns>PaginatedResponse</returns>
        [OperationContract]
        List<SearchDTO.CheckBookControlDTO> GetCheckBookControlsByBankIdBranchId(int bankId, int branchId);

        /// <summary>
        /// GetCheckBookControlActiveByAccountBankId
        /// bcardenas
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <param name="isAutomatic"></param>
        /// <returns>List<CheckBookControl/></returns>
        [OperationContract]
        List<ACPS.CheckBookControlDTO> GetCheckBookControlActiveByAccountBankId(int accountBankId, int isAutomatic);
        #endregion

        //#region PaymentOrder

        /// <summary>
        /// SaveTempPaymentOrder        
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns>PaymentOrder</returns>
        [OperationContract]
        ACPS.PaymentOrderDTO SaveTempPaymentOrder(ACPS.PaymentOrderDTO paymentOrder);

        /// <summary>
        /// UpdateTempPaymentOrder        
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns>PaymentOrder</returns>
        [OperationContract]
        ACPS.PaymentOrderDTO UpdateTempPaymentOrder(ACPS.PaymentOrderDTO paymentOrder);


        /// <summary>
        /// GetTempPaymentOrderById        
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <returns>PaymentOrder</returns>
        [OperationContract]
        ACPS.PaymentOrderDTO GetTempPaymentOrderById(int paymentOrderId);

        /// <summary>
        /// SavePaymentOrderImputationRequest
        /// </summary>
        /// <param name="tempPaymentOrderId"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SavePaymentOrderImputationRequest(int tempPaymentOrderId, int tempImputationId, int imputationTypeId, int userId);

        /// <summary>
        /// GetTempPaymentOrderByTempId
        /// </summary>
        /// <param name="tempPaymentOrderId"></param>
        /// <returns>TempPaymentOrderDTO</returns>
        [OperationContract]
        SearchDTO.TempPaymentOrderDTO GetTempPaymentOrderByTempId(int tempPaymentOrderId);

        /// <summary>
        /// GetBankAccountByBeneficiaryId
        /// </summary>
        /// <param name="benficiaryId"></param>        
        /// <returns>List<BeneficiaryBankAccountsDTO/></returns>
        [OperationContract]
        List<SearchDTO.BeneficiaryBankAccountsDTO> GetBankAccountByBeneficiaryId(string benficiaryId);

        /// <summary>
        /// GetPaymentOrderByPaymentSourceIdPayDate
        /// </summary>
        /// <param name="paymentSourceId"></param>
        /// <param name="payDate"></param>
        /// <param name="currencyId"></param>
        /// <param name="paymentOrderId"></param>
        /// <returns> List<TempPaymentOrderDTO/></returns>
        [OperationContract]
        List<SearchDTO.TempPaymentOrderDTO> GetPaymentOrderByPaymentSourceIdPayDate(int paymentSourceId, DateTime payDate, int currencyId, int paymentOrderId);


        /// <summary>
        /// SearchPaymentOrders
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <returns>List<PaymentOrderDTO/></returns>
        [OperationContract]        
        List<SearchDTO.PaymentOrderDTO> SearchPaymentOrders(SearchDTO.SearchParameterPaymentOrdersDTO searchParameter);


        /// <summary>
        /// CancellationPaymentOrder
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="userId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool CancellationPaymentOrder(int paymentOrderId, int tempImputationId, int userId);

        /// <summary>
        /// UpdatePaymentOrder
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool UpdatePaymentOrder(ACPS.PaymentOrderDTO paymentOrder);

        /// <summary>
        /// GetPaymentOrder
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <returns></returns>
        [OperationContract]
        ACPS.PaymentOrderDTO GetPaymentOrder(int paymentOrderId);

        [OperationContract]
        IndividualDTO GetIndividualByIndividualId(int individualId);
        //#endregion

        #region CheckPaymentOrder

        ///<summary>
        /// SaveCheckPaymentOrderRequest
        /// </summary>
        /// <param name="checkPaymentOrder"></param>
        /// <param name="checkBookControlId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveCheckPaymentOrderRequest(ACPS.CheckPaymentOrderDTO checkPaymentOrder, int checkBookControlId);

        ///<summary>
        /// UpdateCheckPaymentOrder
        /// </summary>
        /// <param name="checkPaymentOrder"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool UpdateCheckPaymentOrder(ACPS.CheckPaymentOrderDTO checkPaymentOrder);

        #endregion

        #region PrintCheck

        /// <summary>
        /// GetPrintCheck
        /// </summary>
        /// <param name="searchParameterCheckPaymentOrder"></param>
        /// <returns>List<PrintCheckDTO/></returns>
        [OperationContract]
        List<SearchDTO.PrintCheckDTO> GetPrintCheck(SearchDTO.SearchParameterCheckPaymentOrderDTO searchParameterCheckPaymentOrder);

        #endregion

        #region Transfer

        /// <summary>
        /// SearchTransferPaymentOrders
        /// </summary>
        /// <param name="searchParameter"></param>        
        /// <returns>List<PaymentOrderDTO/></returns>
        [OperationContract]
        List<SearchDTO.PaymentOrderDTO> SearchTransferPaymentOrders(SearchDTO.SearchParameterPaymentOrdersDTO searchParameter);

        ///<summary>
        /// SaveTransferRequest
        /// </summary>
        /// <param name="transferPaymentOrder"></param>
        /// <returns>TransferPaymentOrder</returns>
        [OperationContract]
        ACPS.TransferPaymentOrderDTO SaveTransferRequest(ACPS.TransferPaymentOrderDTO transferPaymentOrder);

        ///<summary>
        /// HasTransferFormat
        /// </summary>
        /// <param name="bankCode"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool HasTransferFormat(int bankCode);

        /// <summary>
        /// GenerateTransferFile
        /// </summary>
        /// <param name="transferPaymentOrder"></param>
        /// <param name="path"></param>
        /// <returns>string</returns>
        [OperationContract]
        string GenerateTransferFile(ACPS.TransferPaymentOrderDTO transferPaymentOrder, string path);

        #endregion

        #region OtherPaymentRequest

        /// <summary>
        /// SaveOtherPaymentRequest
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns>PaymentRequest</returns>
        [OperationContract]
        ACPS.PaymentRequestDTO SavePaymentRequest(ACPS.PaymentRequestDTO paymentRequest);

        /// <summary>
        /// UpdatePaymentRequest
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns>PaymentRequest</returns>
        [OperationContract]
        ACPS.PaymentRequestDTO UpdatePaymentRequest(ACPS.PaymentRequestDTO paymentRequest);

        /// <summary>
        /// GetPaymentRequest
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns>PaymentRequest</returns>
        [OperationContract]
        ACPS.PaymentRequestDTO GetPaymentRequest(ACPS.PaymentRequestDTO paymentRequest);



        /// <summary>
        /// OtherPaymentRequestReport
        /// Reporte para solicitud de pagos varios
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <returns>OtherPaymentsRequestReportHeaderDTO</returns>
        [OperationContract]
        SearchDTO.OtherPaymentsRequestReportHeaderDTO OtherPaymentRequestReport(int paymentRequestId);

        /// <summary>
        /// GetCostCenterByAccountingAccountId
        /// Obtiene los centros de gastos asociados a una cuenta contable parametrizados para un concepto
        /// de pago
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>List<CostCenterDTO/></returns>
        [OperationContract]
        List<SearchDTO.CostCenterDTO> GetCostCenterByAccountingAccountId(int accountingAccountId);

        /// <summary>
        /// GetPaymentRequestNumber
        /// </summary>
        /// <param name="paymentRequestNumber"></param>
        /// <returns>PaymentRequest</returns>
        [OperationContract]
        ACPS.PaymentRequestNumberDTO GetPaymentRequestNumber(ACPS.PaymentRequestNumberDTO paymentRequestNumber);

        #endregion

        #region CancelCheck

        /// <summary>
        /// GetCancelCheckPaymentOrder
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <param name="checkNumber"></param>
        /// <returns>List<CancelCheckDTO/></returns>
        [OperationContract]
        List<SearchDTO.CancelCheckDTO> GetCancelCheckPaymentOrder(int accountBankId, int checkNumber);

        ///<summary>
        /// SaveCancelCheckRequest
        /// </summary>
        /// <param name="checkPaymentOrder"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveCancelCheckRequest(ACPS.CheckPaymentOrderDTO checkPaymentOrder);

#endregion

        #region TransferCancellation

        ///<summary>
        /// CancelTransferBank
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="userCancelId"></param>
        /// <param name="typeOperation"></param>
        /// <returns>PaymentOrder</returns>
        [OperationContract]
        ACPS.PaymentOrderDTO CancelTransferBank(int paymentOrderId, int userCancelId, int typeOperation);

        #endregion

        #region AccountingCompany

        ///<summary>
        /// SaveAccountingCompany
        /// </summary>
        /// <param name="company"></param>
        /// <returns>Company</returns>
        [OperationContract]
        CompanyDTO SaveAccountingCompany(CompanyDTO company);


        /// <summary>
        /// UpdateAccountingCompany
        /// </summary>
        /// <param name="company"></param>
        /// <returns>Company</returns>
        [OperationContract]
        CompanyDTO UpdateAccountingCompany(CompanyDTO company);


        /// <summary>
        /// DeleteAccountingCompany
        /// </summary>
        /// <param name="accountingCompanyId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteAccountingCompany(int accountingCompanyId);


        /// <summary>
        /// GetAccountingCompany
        /// </summary>
        /// <param name="company"></param>
        /// <returns>Company</returns>
        [OperationContract]
        CompanyDTO GetAccountingCompany(CompanyDTO company);


        /// <summary>
        /// GetAccountingCompanies
        /// </summary>
        /// <returns>List<CommonService.Models.Individuals.Company/></returns>
        [OperationContract]
        List<CompanyDTO> GetAccountingCompanies();

        #endregion


        /// <summary>
        /// GetVoucherTypes
        ///     Obtiene una lista de los tipos de Voucher
        /// </summary>
        /// <returns>List<VoucherType/></returns>
        [OperationContract]
        List<ACPS.VoucherTypeDTO> GetVoucherTypes();

        /// <summary>
        /// GetTempVoucherByRangeVoucherId
        /// </summary>
        /// <param name="voucherIdBegin"></param>
        /// <param name="voucherIdLast"></param>
        /// <returns>List<Voucher/></returns>
        [OperationContract]
        List<ACPS.VoucherDTO> GetTempVoucherByRangeVoucherId(int voucherIdBegin, int voucherIdLast);

        /// <summary>
        /// DeleteTempVoucherByRangeVoucherId
        /// </summary>
        /// <param name="voucherIdBegin"></param>
        /// <param name="voucherIdLast"></param>
        [OperationContract]
        void DeleteTempVoucherByRangeVoucherId(int voucherIdBegin, int voucherIdLast);


        /// <summary>
        /// DeleteTempVoucherConceptByTempPaymentRequest
        /// </summary>
        /// <param name="tempPaymentRequestId"></param>
        [OperationContract]
        void DeleteTempVoucherConceptByTempPaymentRequest(int tempPaymentRequestId);


        #region PaymentOrderAuthorization

        /// <summary>
        /// GetPaymentOrderAuthorization
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="paymentMethodId"></param>
        /// <param name="estimatedPaymentDate"></param>
        /// <param name="userId"></param>
        /// <returns>List<PaymentOrder></returns>
        [OperationContract]
        List<ACPS.PaymentOrderDTO> GetPaymentOrderAuthorization(int branchId, int paymentMethodId, DateTime estimatedPaymentDate, int userId);


        /// <summary>
        /// SavePaymentOrderAuthorization
        /// Registra el nivel autorización de las órdenes de pago 
        /// </summary>
        /// <param name="paymentOrder"></param>        
        /// <returns></returns>
        [OperationContract]
        void SavePaymentOrderAuthorization(ACPS.PaymentOrderDTO paymentOrder);

        #endregion
    }
}
