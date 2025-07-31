using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using SearchDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using ACPSDTO=Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;


namespace Sistran.Core.Application.AccountingServices
{
    [ServiceContract]
    public interface IAccountingImputationService
    {

        //  #region Imputation

        /// <summary>
        /// SaveImputationRequest
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="comments"></param>
        /// <param name="statusId"></param>
        /// <param name="userId"></param>
        /// <param name="tempSourceCode"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveImputationRequest(int sourceCode, int tempImputationId, int imputationTypeId, string comments, int statusId, int userId, int tempSourceCode);


        #region TempImputation

        /// <summary>
        /// SaveTempImputation
        /// </summary>
        /// <param name="imputation"></param>
        /// <param name="sourceCode"></param>
        /// <returns>ImputationDTO</returns>

        [OperationContract]
        ImputationDTO SaveTempImputation(ImputationDTO imputation, int sourceCode);

        /// <summary>
        /// UpdateTempImputation
        /// </summary>
        /// <param name="imputation"></param>
        /// <param name="sourceCode"></param>
        /// <returns>ImputationDTO</returns>
        [OperationContract]
        ImputationDTO UpdateTempImputation(ImputationDTO imputation, int sourceCode);

        /// <summary>
        /// DeleteTempImputation
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteTempImputation(int tempImputationId);

        /// <summary>
        /// GetTempImputationBySourceCode
        /// </summary>
        /// <param name="imputationTypeId"></param>
        /// <param name="sourceCode"></param>
        /// <returns>ImputationDTO</returns>
        [OperationContract]
        ImputationDTO GetTempImputationBySourceCode(int imputationTypeId, int sourceCode);

        /// <summary>
        /// GetTempImputationItem
        /// </summary>
        /// <param name="imputationTypeId"></param>
        /// <param name="tempImputationId"></param>
        /// <returns>TransactionTypeDTO</returns>
        [OperationContract]
        TransactionTypeDTO GetTempImputationItem(int imputationTypeId, int tempImputationId);

        /// <summary>
        /// GetDebitsAndCreditsMovementTypes
        /// </summary>
        /// <param name="imputation"></param>
        /// <param name="amountValue"></param>
        /// <returns>ImputationDTO</returns>
        [OperationContract]
        ImputationDTO GetDebitsAndCreditsMovementTypes(ImputationDTO imputation, decimal amountValue);

        /// <summary>
        /// UpdateTempImputationSourceCode
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="sourceId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int UpdateTempImputationSourceCode(int tempImputationId, int sourceId);

        /// <summary>
        /// GetTempImputation
        /// </summary>
        /// <param name="tempImputation"></param>
        /// <returns>ImputationDTO</returns>
        [OperationContract]
        ImputationDTO GetTempImputation(ImputationDTO tempImputation);

        /// <summary>
        /// GetImputationTypes
        /// </summary>
        /// <returns>List<ImputationTypeDTO/></returns>
        [OperationContract]
        List<ImputationTypeDTO> GetImputationTypes();

        /// <summary>
        /// DeleteTemporaryApplicationRequest
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="sourceCode"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteTemporaryApplicationRequest(int tempImputationId, int imputationTypeId, int sourceCode);

        /// <summary>
        /// RecalculatingForeignCurrencyAmount
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="sourceId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns></returns>
        [OperationContract]
        bool RecalculatingForeignCurrencyAmount(int tempImputationId, int imputationTypeId, int sourceId, List<SearchDTO.ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates);



        #endregion ImputationDTO

        //Primas por Cobrar
        #region PremiumRecievable

        ///<summary>
        /// SaveTempPremiumRecievableTransaction
        /// </summary>
        /// <param name="premiumRecievableTransaction"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="userId"></param>
        /// <param name="registerDate"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveTempPremiumRecievableTransaction(PremiumReceivableTransactionDTO premiumRecievableTransaction, int imputationId, decimal exchangeRate, int userId, DateTime registerDate);

        /// <summary>
        /// Permite Consultar la comision descontada por endoso
        /// </summary>
        /// <param name="endorsementId">identificador del endoso </param>
        /// <param name="policyId">id poliza</param>
        /// <returns></returns>
        [OperationContract]
        List<SearchDTO.DiscountedCommissionDTO> SearhDiscountedCommission(string endorsementId, string policyId);

        #endregion

        //Primas en Depósito
        #region DepositPremium

        #region PremiumRecievableTransaction

        ///<summary>
        /// PremiumReceivableSearchPolicy
        /// </summary>
        /// <param name="insuredId"></param>
        /// <param name="payerId"></param>
        /// <param name="agentId"></param>
        /// <param name="groupId"></param>
        /// <param name="policyDocumentNumber"></param>
        /// <param name="salesTicket"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="endorsementDocumentNumber"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>        
        /// <returns>List<PremiumReceivableSearchPolicyDTO/></returns>

        [OperationContract]
        List<SearchDTO.PremiumReceivableSearchPolicyDTO> PremiumReceivableSearchPolicy(string insuredId, string payerId, string agentId, string groupId, string policyId, string policyDocumentNumber, string salesTicket, string branchId, string prefixId, string endorsementDocumentNumber, string dateFrom, string dateTo, string insuredDocumentNumber, int pageSize, int pageIndex);

        ///<summary>
        /// GetTempPremiumReceivableItemByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>        
        /// <returns>List<PremiumReceivableItemDTO/></returns>
        [OperationContract]
        List<SearchDTO.PremiumReceivableItemDTO> GetTempPremiumReceivableItemByTempImputationId(int tempImputationId);

        /// <summary>
        /// DeleteTempPremiumRecievableTransactionItem
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="tempPremiumReceivableCode"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteTempPremiumRecievableTransactionItem(int tempImputationId, int tempPremiumReceivableCode);

        #endregion

        #region DepositPremiumTransaction

        /// <summary>
        /// GetDepositPremiumTransactionByPayerId
        /// </summary>
        /// <param name="payerId"></param>
        /// <returns>List<DepositPremiumTransactionDTO/></returns>
        [OperationContract]
        List<SearchDTO.DepositPremiumTransactionDTO> GetDepositPremiumTransactionByPayerId(int payerId);

        #endregion

        #region TempUsedDepositPremium

        /// <summary>
        /// SaveTempUsedDepositPremium
        /// </summary>
        /// <param name="tempUsedDepositPremiums"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveTempUsedDepositPremiumRequest(List<SearchDTO.TempUsedDepositPremiumDTO> tempUsedDepositPremiums);

        /// <summary>
        /// HasDepositPrimes
        /// Método para indicar si se usaron primas en depósito al aplicar una prima por cobrar.
        /// </summary>
        /// <param name="tempPremiumReceivableId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool HasDepositPrimes(int tempPremiumReceivableId);

        /// <summary>
        /// DeleteTempUsedDepositPremiumRequest
        /// Método para eliminar las primas en depósito en temporales.
        /// </summary>
        /// <param name="tempPremiumReceivableId"></param>
        /// <returns></returns>
        [OperationContract]
        void DeleteTempUsedDepositPremiumRequest(int tempPremiumReceivableId);

        #endregion

        #endregion DepositPremium

        //Solicitud Pago Siniestros/Varios
        #region PaymentRequest

        /// <summary>
        /// SearchClaimsPaymentRequest
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <param name="typeSearch"></param>
        /// <returns>List<PaymentRequestVariousDTO/></returns>
        [OperationContract]
        List<SearchDTO.PaymentRequestVariousDTO> SearchClaimsPaymentRequest(SearchDTO.SearchParameterClaimsPaymentRequestDTO searchParameter, int typeSearch);

        ///<summary>
        /// SaveClaimsPaymentRequestItem
        /// </summary>
        /// <param name="claimsPaymentRequestItem"></param>        
        /// <param name="imputationId"></param>       
        /// <param name="exchangeRate"></param>     
        /// <returns>ImputationDTO</returns>
        [OperationContract]
        ImputationDTO SaveTempClaimPaymentRequestItem(ClaimsPaymentRequestTransactionItemDTO claimsPaymentRequestItem, int imputationId, decimal exchangeRate);

        ///<summary>
        /// SaveTempPaymentRequestItem
        /// </summary>
        /// <param name="paymentRequestTransactionItem"></param>        
        /// <param name="imputationId"></param>       
        /// <param name="exchangeRate"></param>     
        /// <returns>ImputationDTO</returns>
        [OperationContract]
        ImputationDTO SaveTempPaymentRequestItem(PaymentRequestTransactionItemDTO paymentRequestTransactionItem, int imputationId, decimal exchangeRate);

        ///<summary>
        /// DeleteClaimsPaymentRequestItem
        /// </summary>
        /// <param name="claimsPaymentRequestId"></param>        
        /// <param name="imputationId"></param>        
        /// <param name="isPaymentVarious"></param>        
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteTempClaimPaymentRequestItem(int claimsPaymentRequestId, int imputationId, bool isPaymentVarious);

        ///<summary>
        /// DeleteClaimsPaymentRequestItem
        /// </summary>
        /// <param name="paymentRequestId"></param>        
        /// <param name="imputationId"></param>        
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteTempPaymentRequestItem(int paymentRequestId, int imputationId);

        /// <summary>
        /// GetTempClaimsPaymentRequest
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="isPaymentVarious"></param>        
        /// <returns>List<TempPaymentRequestClaimDTO/></returns>
        [OperationContract]
        List<SearchDTO.TempPaymentRequestClaimDTO> GetTempClaimsPaymentRequest(int imputationId, bool isPaymentVarious);

        /// <summary>
        /// GetTempPaymentRequestByImputationId
        /// </summary>
        /// <param name="imputationId"></param>
        /// <returns>List<TempPaymentRequestClaimDTO/></returns>
        [OperationContract]
        List<SearchDTO.TempPaymentRequestClaimDTO> GetTempPaymentRequestByImputationId(int imputationId);

        #endregion PaymentRequest

        //Cuenta Corriente Agentes
        #region CurrentAccountAgents

        #region BrokersCheckingAccount

        /// <summary>
        /// SearchBrokersCheckingAccount
        /// </summary>
        /// <param name="searchParameter"></param>        
        /// <returns>List<SearchAgentsItemsDTO/></returns>
        [OperationContract]
        List<SearchDTO.SearchAgentsItemsDTO> SearchBrokersCheckingAccount(SearchDTO.SearchParameterBrokersCheckingAccountDTO searchParameter);

        ///<summary>
        /// SaveBrokersCheckingAccountItem
        /// </summary>
        /// <param name="brokerCheckingAccountItem"></param>        
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveBrokersCheckingAccountItem(List<BrokerCheckingAccountItemDTO> brokerCheckingAccountItem);

        /// <summary>
        /// UpdateTempBrokersCheckingAccountTotal
        /// </summary>
        /// <param name="tempBrokerCheckingAccountId"></param>
        /// <param name="selectedTotal"></param>
        /// <returns>BrokersCheckingAccountTransactionDTO</returns>
        [OperationContract]
        BrokersCheckingAccountTransactionDTO UpdateTempBrokersCheckingAccountTotal(int tempBrokerCheckingAccountId, decimal selectedTotal);

        ///<summary>
        /// DeleteBrokersCheckingAccountItem
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="brokersCheckingAccountItemId"></param>        
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteBrokersCheckingAccountItem(int tempImputationId, int brokersCheckingAccountItemId);


        ///<summary>
        /// SaveBrokersCheckingAccount
        /// </summary>
        /// <param name="brokersCheckingAccountTransaction"></param>        
        /// <param name="imputationId"></param>
        /// <param name="accountingDate"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveBrokersCheckingAccount(BrokersCheckingAccountTransactionDTO brokersCheckingAccountTransaction, int imputationId, DateTime accountingDate);


        /// <summary>
        /// GetTempBrokerCheckingAccountItemByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>        
        /// <returns>List<BrokerCheckingAccountItemDTO/></returns>
        [OperationContract]
        List<DTOs.Search.BrokerCheckingAccountItemDTO> GetTempBrokerCheckingAccountItemByTempImputationId(int tempImputationId);


        /// <summary>
        /// GetTempBrokerCheckingAccountItemChildByTempImputationId
        /// </summary>
        /// <param name="tempBrokerParentId"></param>
        /// <returns>PaginatedResponse</returns>
        [OperationContract]
        List<DTOs.Search.BrokerCheckingAccountItemDTO> GetTempBrokerCheckingAccountItemChildByTempBrokerParentId(int tempBrokerParentId);

        /// <summary>
        /// DeleteBrokerCheckingAccountItemChild
        /// </summary>
        /// <param name="tempBrokerCheckingAccountItemId"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteBrokerCheckingAccountItemChild(int tempBrokerCheckingAccountItemId);

        #endregion

        #region SearchAgents

        /// <summary>
        /// SearchAgentsItems
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="salePointId"></param>
        /// <param name="prefixId"></param>
        /// <param name="policyNum"></param>
        /// <param name="currencyId"></param>
        /// <param name="insuredId"></param>        
        /// <returns>List<SearchAgentsItemsDTO/></returns>
        [OperationContract]
        List<SearchDTO.SearchAgentsItemsDTO> SearchAgentsItems(int branchId, int salePointId, int prefixId, int policyNum, int currencyId,
                                            int insuredId);
        #endregion

        #region Validate

        /// <summary>
        /// ValidateDuplicateBrokerCheckingAccount
        /// </summary>
        /// <param name="validateParameter"></param>
        /// <returns>ImputationDTO</returns>
        [OperationContract]
        ImputationDTO ValidateDuplicateBrokerCheckingAccount(SearchDTO.ValidateParameterBrokerCoinsuranceReinsuranceDTO validateParameter);

        #endregion

        #endregion CurrentAccountAgents

        //Cuenta Corriente Reaseguros
        #region CurrentAccountReinsurance

        #region TempReinsuranceCheckingAccount

        /// <summary>
        /// SearchReinsuranceCheckingAccount
        /// </summary>
        /// <param name="searchParameter"></param>        
        /// <returns>List<ReinsuranceCheckingAccountItemDTO/></returns>
        [OperationContract]
        List<SearchDTO.ReinsuranceCheckingAccountItemDTO> SearchReinsuranceCheckingAccount(SearchDTO.SearchParameterReinsuranceCheckingAccountDTO searchParameter);

        ///<summary>
        /// SaveReinsuranceCheckingAccountItems
        /// </summary>
        /// <param name="reinsuranceCheckingAccountItems"></param>        
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveReinsuranceCheckingAccountItems(List<ReinsuranceCheckingAccountItemDTO> reinsuranceCheckingAccountItems);

        ///<summary>
        /// DeleteReinsuranceCheckingAccountItem
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="reinsuranceCheckingAccountItemId"></param>        
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteReinsuranceCheckingAccountItem(int tempImputationId, int reinsuranceCheckingAccountItemId);

        ///<summary>
        /// SaveReinsuranceCheckingAccount
        /// </summary>
        /// <param name="reinsuranceCheckingAccountTransaction"></param>        
        /// <param name="imputationId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveReinsuranceCheckingAccount(ReInsuranceCheckingAccountTransactionDTO reinsuranceCheckingAccountTransaction, int imputationId);

        /// <summary>
        /// GetTempReinsuranceCheckingAccountItemByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>        
        /// <returns>List<ReinsuranceCheckingAccountItemDTO/></returns>
        [OperationContract]
        List<SearchDTO.ReinsuranceCheckingAccountItemDTO> GetTempReinsuranceCheckingAccountItemByTempImputationId(int tempImputationId);

        /// <summary>
        /// GetTempReinsuranceCheckingAccountItemChildByTempReinsuranceParentId
        /// </summary>
        /// <param name="tempReinsuranceParentId"></param>        
        /// <returns>List<ReinsuranceCheckingAccountItemDTO/></returns>
        [OperationContract]
        List<SearchDTO.ReinsuranceCheckingAccountItemDTO> GetTempReinsuranceCheckingAccountItemChildByTempReinsuranceParentId(int tempReinsuranceParentId);

        /// <summary>
        /// DeleteReinsuranceCheckingAccountItemChild
        /// </summary>
        /// <param name="tempReinsuranceCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteReinsuranceCheckingAccountItemChild(int tempReinsuranceCheckingAccountItemId);

        /// <summary>
        /// ValidateInsurancePolicyEndorsement
        /// </summary>
        /// <param name="policyNumber"></param>
        /// <param name="endorsementNumber"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ValidateInsurancePolicyEndorsement(string policyNumber, int endorsementNumber, int branchId, int prefixId);

        /// <summary>
        /// UpdateTempReinsuranceCheckingAccountTotal
        /// </summary>
        /// <param name="tempReinsuranceCheckingAccountId"></param>
        /// <param name="selectedTotal"></param>
        /// <returns>ReInsuranceCheckingAccountTransactionDTO</returns>
        [OperationContract]
        ReInsuranceCheckingAccountTransactionDTO UpdateTempReinsuranceCheckingAccountTotal(int tempReinsuranceCheckingAccountId, decimal selectedTotal);

        #endregion

        /// <summary>
        /// ValidateDuplicateReinsuranceCheckingAccount
        /// </summary>
        /// <param name="validateParameter"></param>
        /// <returns>ImputationDTO</returns>
        [OperationContract]
        ImputationDTO ValidateDuplicateReinsuranceCheckingAccount(SearchDTO.ValidateParameterBrokerCoinsuranceReinsuranceDTO validateParameter);

        #endregion

        //Contabilidad
        #region Accounting

        ///<summary>
        /// SaveTempAccountingTransaction
        /// </summary>
        /// <param name="tempDailyAccountingTransaction"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="accountingConceptId"></param>
        /// <param name="bankReconciliationId"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="receiptDate"></param>
        /// <param name="postdatedAmount"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveTempAccountingTransaction(DailyAccountingTransactionDTO tempDailyAccountingTransaction, int tempImputationId, int accountingConceptId, int bankReconciliationId, int receiptNumber, DateTime? receiptDate, decimal postdatedAmount);

        /// <summary>
        /// DeleteTempDailyAccountingTransaction
        /// </summary>
        /// <param name="tempDailyAccountingTransactionItemId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteTempDailyAccountingTransactionItem(int tempDailyAccountingTransactionItemId);

        ///<summary>
        /// GetTempAccountingTransactionItemByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>        
        /// <returns>List<TempDailyAccountingDTO/></returns>
        [OperationContract]
        List<SearchDTO.TempDailyAccountingDTO> GetTempAccountingTransactionItemByTempImputationId(int tempImputationId);

        /// <summary>
        /// GetAccountingAccountByDescription
        /// </summary>
        /// <param name="accountingAccountDescription"></param>
        /// <param name="branchId"></param>
        /// <returns>List<AccountingAccountDTO/></returns>
        [OperationContract]
        List<SearchDTO.AccountingAccountDTO> GetAccountingAccountByDescription(string accountingAccountDescription, int branchId);

        /// <summary>
        /// GetAccountingAccountByNumber
        /// </summary>
        /// <param name="accountingAccountNumber"></param>
        /// <param name="branchId"></param>
        /// <returns>List<AccountingAccountDTO/></returns>
        [OperationContract]
        List<SearchDTO.AccountingAccountDTO> GetAccountingAccountByNumber(string accountingAccountNumber, int branchId);



        #endregion

        //Cuenta Corriente Coaseguros
        #region CurrentAccountCoinsurance

        /// <summary>
        /// SaveReinsuranceCheckingAccountItems
        /// </summary>
        /// <param name="coInsuranceCheckingAccountItems"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveCoinsuranceCheckingAccountItems(List<CoInsuranceCheckingAccountItemDTO> coInsuranceCheckingAccountItems);

        /// <summary>
        /// DeleteCoinsuranceCheckingAccountItem
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="coinsuranceCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteCoinsuranceCheckingAccountItem(int tempImputationId, int coinsuranceCheckingAccountItemId);



        /// <summary>
        /// SaveCoinsuranceCheckingAccount
        /// </summary>
        /// <param name="coinsuranceCheckingAccountTransaction"></param>
        /// <param name="imputationId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveCoinsuranceCheckingAccount(CoInsuranceCheckingAccountTransactionDTO coinsuranceCheckingAccountTransaction,
                                            int imputationId);


        /// <summary>
        /// DeleteCoinsuranceCheckingAccountItemChild
        /// </summary>
        /// <param name="tempCoinsuranceCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteCoinsuranceCheckingAccountItemChild(int tempCoinsuranceCheckingAccountItemId);


        /// <summary>
        /// UpdateTempCoinsuranceCheckingAccountTotal
        /// </summary>
        /// <param name="tempCoinsuranceCheckingAccountId"></param>
        /// <param name="selectedTotal"></param>
        /// <returns>CoInsuranceCheckingAccountTransactionDTO</returns>
        [OperationContract]
        CoInsuranceCheckingAccountTransactionDTO UpdateTempCoinsuranceCheckingAccountTotal(
            int tempCoinsuranceCheckingAccountId, decimal selectedTotal);

        ///<summary>
        /// GetTempCoinsuranceCheckingAccountItemByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>        
        /// <returns>List<CoinsuranceCheckingAccountItemDTO/></returns>
        [OperationContract]
        List<SearchDTO.CoinsuranceCheckingAccountItemDTO> GetTempCoinsuranceCheckingAccountItemByTempImputationId(int tempImputationId);

        /// <summary>
        /// GetTempCoinsuranceCheckingAccountItemChildByTempCoinsuranceParentId
        /// </summary>
        /// <param name="tempCoinsuranceParentId"></param>        
        /// <returns>List<CoinsuranceCheckingAccountItemDTO/></returns>
        [OperationContract]
        List<SearchDTO.CoinsuranceCheckingAccountItemDTO> GetTempCoinsuranceCheckingAccountItemChildByTempCoinsuranceParentId(int tempCoinsuranceParentId);

        /// <summary>
        /// SearchCoinsuranceCheckingAccount
        /// </summary>
        /// <param name="searchParameter"></param>        
        /// <returns></returns>
        [OperationContract]
        List<SearchDTO.CoinsuranceCheckingAccountItemDTO> SearchCoinsuranceCheckingAccount(SearchDTO.SearchParameterCoinsuranceCheckingAccountDTO searchParameter);

        /// <summary>
        /// ValidateDuplicateCoinsuranceCheckingAccount
        /// </summary>
        /// <param name="validateParameter"></param>
        /// <returns>ImputationDTO</returns>
        [OperationContract]
        ImputationDTO ValidateDuplicateCoinsuranceCheckingAccount(SearchDTO.ValidateParameterBrokerCoinsuranceReinsuranceDTO validateParameter);

        #endregion

        // Asegurados
        #region  InsuredLoand
        /// <summary>
        /// SaveTempInsuredLoanTransaction: Graba Transacción Temporal de Préstamo de Asegurado
        /// </summary>
        /// <param name="tempInsuredLoanTransaction"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveTempInsuredLoanTransaction(InsuredLoanTransactionDTO tempInsuredLoanTransaction);

        /// <summary>
        /// DeleteTempInsuredLoanTransactionItem: Elminia Transacción Temporal de Préstamo de Asegurado
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="tempInsuredLoanId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteTempInsuredLoanTransactionItem(int tempImputationId, int tempInsuredLoanId);

        /// <summary>
        /// GetTmpInsuredLoansByTempImputationId : Obtiene Temporal de Préstamos de Asegurados 
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>List<InsuredLoanTransaction/></returns>
        [OperationContract]
        List<InsuredLoanTransactionDTO> GetTmpInsuredLoansByTempImputationId(int tempImputationId);

        /// <summary>
        /// GetInsuredLoanTransaction: Ontiene Transacción de Préstamo de Asegurado
        /// </summary>
        /// <param name="insuredLoanTransaction"></param>
        /// <returns>InsuredLoanTransaction</returns>
        [OperationContract]
        InsuredLoanTransactionDTO GetInsuredLoanTransaction(InsuredLoanTransactionDTO insuredLoanTransaction);

        /// <summary>
        /// SaveInsuredLoanTransaction: Graba Transacción de Préstamo de Asegurado
        /// </summary>
        /// <param name="insuredLoanTransaction"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveInsuredLoanTransaction(InsuredLoanTransactionDTO insuredLoanTransaction);


        #endregion

        // #endregion ImputationTypes

        #region Various

        #region CurrencyDifference

        /// <summary>
        /// GetCurrencyDiferenceByCurrencyId
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>List<CurrencyDiferenceDTO/></returns>
        [OperationContract]
        List<SearchDTO.CurrencyDiferenceDTO> GetCurrencyDiferenceByCurrencyId(int currencyId);

        #endregion CurrencyDifference

        #region Company

        /// <summary>
        /// GetCompaiesByUserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List<CompanyDTO/></returns>
        [OperationContract]
        List<CompanyDTO> GetCompaniesByUserId(int userId);

        /// <summary>
        /// GetParameterMulticompany
        /// </summary>
        /// <returns>bool</returns>
        [OperationContract]
        bool GetParameterMulticompany();

        /// <summary>
        /// GetCurrencyDifferenceByCurrencyId
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        [OperationContract]
        decimal GetPercentageDifferenceByCurrencyId(int currencyId);

        #endregion Company

        #region CurrentAccount

        /// <summary>
        /// GetConceptCurrentAccountDescription
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="conceptId"></param>
        /// <param name="sourceId"></param>
        /// <returns>List<ConceptCurrentAccountDTO/></returns>
        [OperationContract]
        List<SearchDTO.ConceptCurrentAccountDTO> GetConceptCurrentAccountDescription(int branchId, int userId, int conceptId, int sourceId);

        /// <summary>
        /// GetConceptCurrentAccountCode
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="conceptDescription"></param>
        /// <param name="sourceId"></param>
        /// <returns>List<ConceptCurrentAccountDTO/></returns>
        [OperationContract]
        List<SearchDTO.ConceptCurrentAccountDTO> GetConceptCurrentAccountCode(int branchId, int userId, string conceptDescription, int sourceId);

        #endregion CurrentAccount


        /// <summary>
        /// ValidatePolicyComponents: Valida que la póliza tenga componentes.
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        bool ValidatePolicyComponents(int policyId, int endorsementId);

        #endregion Various

        #region PreLiquidation

        ///<summary>
        /// SaveTempPreLiquidation
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        [OperationContract]
        PreLiquidationDTO SaveTempPreLiquidation(PreLiquidationDTO preLiquidation);


        ///<summary>
        /// UpdateTempPreLiquidation
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        [OperationContract]
        PreLiquidationDTO UpdateTempPreLiquidation(PreLiquidationDTO preLiquidation);


        ///<summary>
        /// UpdatePreLiquidation
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        [OperationContract]
        PreLiquidationDTO UpdatePreLiquidation(PreLiquidationDTO preLiquidation);


        ///<summary>
        /// ConvertTempPreLiquidationToPreLiquidation
        /// </summary>
        /// <param name="tempPreLiquidationId"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ConvertTempPreLiquidationToPreLiquidation(int tempPreLiquidationId, int tempImputationId, int imputationTypeId);


        /// <summary>
        /// GetPreliquidations
        /// </summary>
        /// <param name="preliquidationsDto"></param>        
        /// <returns>List<PreliquidationsDTO/></returns>
        [OperationContract]
        List<SearchDTO.PreliquidationsDTO> GetPreliquidations(SearchDTO.PreliquidationsDTO preliquidationsDto);


        /// <summary>
        /// CancelPreliquidation
        /// </summary>
        /// <param name="preliquidationId"></param>
        /// <param name="tempImputationId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool CancelPreliquidation(int preliquidationId, int tempImputationId);



        /// <summary>
        /// GetTempPreLiquidation
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidationDTO</returns>
        [OperationContract]
        PreLiquidationDTO GetTempPreLiquidation(PreLiquidationDTO preLiquidation);


        #endregion PreLiquidation

        #region JournalEntry

        /// <summary>
        /// TemporarySearch
        /// </summary>
        /// <param name="searchParameter"></param>        
        /// <returns>List<TemporaryItemSearchDTO/></returns>
        [OperationContract]
        List<SearchDTO.TemporaryItemSearchDTO> TemporarySearch(SearchDTO.SearchParameterTemporaryDTO searchParameter);

        /// <summary>
        /// SaveTempJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntryDTO</returns>
        [OperationContract]
        JournalEntryDTO SaveTempJournalEntry(JournalEntryDTO journalEntry);


        /// <summary>
        /// UpdateTempJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntry</returns>
        [OperationContract]
        JournalEntryDTO UpdateTempJournalEntry(JournalEntryDTO journalEntry);


        /// <summary>
        /// UpdateJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntry</returns>
        [OperationContract]
        JournalEntryDTO UpdateJournalEntry(JournalEntryDTO journalEntry);



        /// <summary>
        /// GetTempJournalEntryById
        /// </summary>
        /// <param name="journalEntryId"></param>
        /// <returns>JournalEntry</returns>
        [OperationContract]
        JournalEntryDTO GetTempJournalEntryById(int journalEntryId);


        /// <summary>
        /// GetTempJournalEntryByTempId
        /// </summary>
        /// <param name="tempJournalEntryId"></param>
        /// <returns>TempJournalEntryDTO</returns>
        [OperationContract]
        SearchDTO.TempJournalEntryDTO GetTempJournalEntryByTempId(int tempJournalEntryId);

        #endregion JournalEntry

        #region PartialClousureAgents

        /// <summary>
        /// SavePartialClousureAgentsRequest
        /// </summary>
        /// <param name="dateTo"></param>
        /// <param name="dateFrom"></param>
        /// <param name="userId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SavePartialClousureAgentsRequest(DateTime dateTo, DateTime dateFrom, int userId, int typeProcess);

        #endregion PartialClousureAgents

        #region CommissionPaymentOrderAgents

        /// <summary>
        /// SaveCommissionPaymentOrder
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="companyId"></param>
        /// <param name="estimatedPaymentDate"></param>
        /// <param name="agentId"></param>
        /// <param name="agentName"></param>
        /// <param name="accountingDate"></param>
        /// <param name="userId"></param>
        /// <param name="salePointId"></param>
        /// <param name="processNumber"></param>
        /// <returns>List<Models.AccountsPayables.PaymentOrder/></returns>
        [OperationContract]
        List<ACPSDTO.PaymentOrderDTO> SaveCommissionPaymentOrder(int branchId, int companyId, DateTime estimatedPaymentDate, int agentId, string agentName, DateTime accountingDate, int userId, int salePointId, int processNumber);

        /// <summary>
        /// GetPaymentOrdersCommission
        /// </summary>
        /// <param name="paymentOrders"></param>
        /// <returns>List<PaymentOrdersCommissionDTO/></returns>
        [OperationContract]
        List<SearchDTO.PaymentOrdersCommissionDTO> GetPaymentOrdersCommission(List<int> paymentOrders);

        /// <summary>
        /// GetAccountingCompanyDefaultByUserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int GetAccountingCompanyDefaultByUserId(int userId);


        /// <summary>
        /// GetSalePointDefaultByUserIdAndBranchId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="branchId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int GetSalePointDefaultByUserIdAndBranchId(int userId, int branchId);

        /// <summary>
        /// GetCurrencyDefaultByAccountingConceptId
        /// </summary>
        /// <param name="accountingConceptId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int GetCurrencyDefaultByAccountingConceptId(int accountingConceptId);

        #endregion CommissionPaymentOrderAgents

        #region CommissionBalance

        /// <summary>
        /// GetCommissionBalance
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="accountingCompanyId"></param>
        /// <param name="agentId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>List<AgentCommissionBalanceItemDTO/></returns>
        [OperationContract]
        List<SearchDTO.AgentCommissionBalanceItemDTO> GetCommissionBalance(int branchId, int accountingCompanyId, int agentId, DateTime dateFrom, DateTime dateTo);

        #endregion CommissionBalance

        #region Commissions

        /// <summary>
        /// GetPendingCommission
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns>PendingCommissionDTO </returns>
        [OperationContract]
        SearchDTO.PendingCommissionDTO GetPendingCommission(int policyId, int endorsementId);

        #endregion Commissions

        #region PremiumReceivableValidation

        ///<summary>
        /// CheckPremiumReceivable
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="paymentNum"></param>
        /// <param name="payerIndividualId"></param>
        /// <returns>ImputationDTO</returns>
        [OperationContract]
        ImputationDTO CheckPremiumReceivable(int policyId, int endorsementId, int paymentNum, int payerIndividualId);

        /// <summary>
        /// ItemHasCancelationEndorsement
        /// </summary>
        ///<param name="policyId"> </param>
        ///<returns>bool</returns>
        [OperationContract]
        bool ItemHasCancelationEndorsement(int policyId);

        #endregion PremiumReceivableValidation

        #region DailyAccountingValidation

        ///<summary>
        /// CheckDailyAccounting
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="salesPointId"></param>
        /// <param name="beneficiaryIndividualId"></param>
        /// <param name="accountingConceptId"></param>
        /// <param name="accountingNature"></param>
        /// <returns>ImputationDTO</returns>
        [OperationContract]
        ImputationDTO CheckDailyAccounting(int branchId, int salesPointId, int beneficiaryIndividualId, int accountingConceptId, int accountingNature);

        #endregion DailyAccountingValidation

        #region RecievablePremiumPaymentStatus

        ///<summary>
        /// PremiumReceivableStatus
        /// </summary>
        /// <param name="insuredId"></param>
        /// <param name="policyDocumentNumber"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="policiesWithPortfolio"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns>List<PremiumReceivableSearchPolicyDTO/></returns>
        [OperationContract]
        List<SearchDTO.PremiumReceivableSearchPolicyDTO> PremiumReceivableStatus(string insuredId, string policyDocumentNumber, string branchId, string prefixId, string endorsementId, string policiesWithPortfolio, int pageSize, int pageIndex, string ExpirationDateFrom, string ExpirationDateTo);

        #endregion RecievablePremiumPaymentStatus

        #region CreditNotes

        ///<summary>
        /// GenerateCreditNoteRequest
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <param name="policyDocumentNumber"> </param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="userId"></param>
        ///<returns>List<JournalEntry/></returns>
        [OperationContract]
        List<JournalEntryDTO> GenerateCreditNoteRequest(JournalEntryDTO journalEntry, string policyDocumentNumber, string branchId, string prefixId, int userId);

        ///<summary>
        /// GenerateCreditNoteReport
        /// </summary>
        /// <param name="policyDocumentNumber"> </param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        ///<returns>List<EndorsementPaymentDTO/></returns>
        [OperationContract]
        List<SearchDTO.EndorsementPaymentDTO> GenerateCreditNoteReport(string policyDocumentNumber, string branchId, string prefixId);

        ///<summary>
        /// ValidateCreditNoteGeneration
        /// </summary>
        /// <param name="policyDocumentNumber"> </param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        ///<returns>bool</returns>
        [OperationContract]
        bool ValidateCreditNoteGeneration(string policyDocumentNumber, string branchId, string prefixId);

        #endregion CreditNotes

        #region PaymentOrder

        /// <summary>
        /// SavePaymentOrderImputation
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SavePaymentOrderImputation(int paymentOrderId, int imputationTypeId, int userId);

        #endregion PaymentOrder

        #region ReverseImputation

        /// <summary>
        /// ReverseImputationRequest
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="userId"></param>
        ///<returns>bool</returns>
        [OperationContract]
        bool ReverseImputationRequest(int collectId, int imputationTypeId, int userId);


        /// <summary>
        /// GetPremiumRecievableAppliedByCollectId
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="imputationTypeId"></param>        
        ///<returns>List<PremiumReceivableItemDTO/></returns>
        [OperationContract]
        List<SearchDTO.PremiumReceivableItemDTO> GetPremiumRecievableAppliedByCollectId(int collectId, int imputationTypeId);


        #endregion ReverseImputation

        #region MassiveProcessData

        /// <summary>
        /// MassiveDataForGenerate
        /// </summary>
        /// <param name="issueDate"></param>        
        /// <returns>List<MassiveDataGenerateDTO/></returns>
        [OperationContract]
        List<SearchDTO.MassiveDataGenerateDTO> MassiveDataForGenerate(DateTime issueDate);

        #endregion MassiveProcessData

        #region Collection


        /// <summary>
        /// Obtiene los cobros a partir de póliza y endoso
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SearchDTO.AppPaymentPolicyDTO> GetCollections(int policyId, int endorsementId);


        /// <summary>
        /// GetPaymentsByCollectId
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>List<Payment></returns>
        [OperationContract]
        List<DTOs.Payments.PaymentDTO> GetPaymentsByCollectId(int collectId);

        #endregion Collection

        #region Refinancing

        /// <summary>        
        /// Método temporal para recuotificaciones, se lo usa para mejorar el rendimiento en la búsqueda de póliza por autocomplete.
        /// </summary>
        /// <param name="policyDocumentNumber"></param>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        [OperationContract]
        List<PolicyDTO> GetPoliciesByBranchAndPrefix(string policyDocumentNumber, int prefixId, int branchId);

        #endregion Refinancing

    }
}
