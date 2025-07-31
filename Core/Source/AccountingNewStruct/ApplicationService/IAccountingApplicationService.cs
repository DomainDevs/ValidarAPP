using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Application;
using Sistran.Core.Application.AccountingServices.DTOs.Filter;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using AAPAY = Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.AccountingServices
{
    [ServiceContract]
    public interface IAccountingApplicationService
    {
        /// <summary>
        /// SaveTempApplication
        /// </summary>
        /// <param name="application"></param>
        /// <param name="sourceCode"></param>
        /// <returns>ApplicationDTO</returns>

        [OperationContract]
        ApplicationDTO SaveTempApplication(ApplicationDTO application, int sourceCode);

        /// <summary>
        /// UpdateTempApplication
        /// </summary>
        /// <param name="application"></param>
        /// <param name="sourceCode"></param>
        /// <returns>ApplicationDTO</returns>
        [OperationContract]
        ApplicationDTO UpdateTempApplication(ApplicationDTO application, int sourceCode);

        /// <summary>
        /// UpdateTempApplication
        /// </summary>
        /// <param name="application"></param>
        /// <param name="sourceCode"></param>
        /// <returns>ApplicationDTO</returns>
        [OperationContract]
        ApplicationDTO UpdateSourceCodeTempApplication(int applicationId, int sourceCode);

        /// <summary>
        /// GetTempApplicationBySourceCode
        /// </summary>
        /// <param name="imputationTypeId"></param>
        /// <param name="sourceCode"></param>
        /// <returns>ApplicationDTO</returns>
        [OperationContract]
        ApplicationDTO GetTempApplicationBySourceCode(int imputationTypeId, int sourceCode);

        /// <summary>
        /// GetDebitsAndCreditsMovementTypes
        /// </summary>
        /// <param name="application"></param>
        /// <param name="amountValue"></param>
        /// <returns>ApplicationDTO</returns>
        [OperationContract]
        ApplicationDTO GetDebitsAndCreditsMovementTypes(ApplicationDTO application, decimal amountValue);

        /// <summary>
        /// GetTempApplication
        /// </summary>
        /// <param name="tempApplication"></param>
        /// <returns>ApplicationDTO</returns>
        [OperationContract]
        ApplicationDTO GetTempApplication(ApplicationDTO tempApplication);

        /// <summary>
        /// GetTempApplication
        /// </summary>
        /// <param name="tempApplication"></param>
        /// <returns>ApplicationDTO</returns>
        [OperationContract]
        List<TempApplicationDTO> GetTempApplicationByUserId(int userId);
        ///<summary>
        /// SaveClaimsPaymentRequestItem
        /// </summary>
        /// <param name="claimsPaymentRequestItem"></param>        
        /// <param name="imputationId"></param>       
        /// <param name="exchangeRate"></param>     
        /// <returns>ApplicationDTO</returns>
        [OperationContract]
        ApplicationDTO SaveTempClaimPaymentRequestItem(ClaimsPaymentRequestTransactionItemDTO claimsPaymentRequestItem, int imputationId, decimal exchangeRate);

        /// <summary>
        /// SaveTempPaymentRequestItem
        /// </summary>
        /// <param name="paymentRequestTransactionItem"></param>
        /// <param name="applicationId"></param>
        /// <param name="exchangeRate"></param>
        /// <returns>ApplicationDTO</returns>
        [OperationContract]
        ApplicationDTO SaveTempApplicationPremiumItem(PaymentRequestTransactionItemDTO paymentRequestTransactionItem, int applicationId, decimal exchangeRate);

        /// <summary>
        /// ValidateDuplicateBrokerCheckingAccount
        /// </summary>
        /// <param name="validateParameter"></param>
        /// <returns>ApplicationDTO</returns>
        [OperationContract]
        ApplicationDTO ValidateDuplicateBrokerCheckingAccount(ValidateParameterBrokerCoinsuranceReinsuranceDTO validateParameter);

        /// <summary>
        /// ValidateDuplicateReinsuranceCheckingAccount
        /// </summary>
        /// <param name="validateParameter"></param>
        /// <returns>ApplicationDTO</returns>
        [OperationContract]
        ApplicationDTO ValidateDuplicateReinsuranceCheckingAccount(ValidateParameterBrokerCoinsuranceReinsuranceDTO validateParameter);

        /// <summary>
        /// ValidateDuplicateCoinsuranceCheckingAccount
        /// </summary>
        /// <param name="validateParameter"></param>
        /// <returns>ApplicationDTO</returns>
        [OperationContract]
        ApplicationDTO ValidateDuplicateCoinsuranceCheckingAccount(ValidateParameterBrokerCoinsuranceReinsuranceDTO validateParameter);

        /// <summary>
        /// Get temporal application by endorsement id and quota number
        /// </summary>
        /// <param name="tempApplicationId">Temporal application id</param>
        /// <param name="endorsementId">Endorsement id</param>
        /// <param name="quotaNumber">Quota number</param>
        /// <returns></returns>
        [OperationContract]
        ApplicationDTO GetTempApplicationByEndorsementIdQuotaNumber(int tempApplicationId, int endorsementId, int quotaNumber);

        ///<summary>
        /// CheckDailyAccounting
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="salesPointId"></param>
        /// <param name="beneficiaryIndividualId"></param>
        /// <param name="accountingConceptId"></param>
        /// <param name="accountingNature"></param>
        /// <returns>ApplicationDTO</returns>
        [OperationContract]
        ApplicationDTO CheckDailyAccounting(int branchId, int salesPointId, int beneficiaryIndividualId, int accountingConceptId, int accountingNature);

        /// <summary>
        /// SavePaymentOrderApplication
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SavePaymentOrderApplication(int paymentOrderId, int imputationTypeId, int userId);

        /// <summary>
        /// SaveTempJournalEntry
        /// Graba asientos de diario temporal
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntry</returns>
        [OperationContract]
        JournalEntryDTO SaveTempJournalEntry(JournalEntryDTO journalEntry);

        /// <summary>
        /// SaveImputationRequestJournalEntry
        /// Ejecuta el proceso de grabación de una imputación de Asiento de Diario
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <param name="userId"></param>
        /// <param name="tempSourceCode"></param>
        /// <returns>CollectImputation</returns>
        [OperationContract]
        CollectApplicationDTO SaveApplicationRequestJournalEntry(int tempApplicationId, int userId, int tempSourceCode);

        /// <summary>
        /// GetAccountingCompanyDefaultByUserId
        /// Obtiene la compañía por defecto del usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int GetAccountingCompanyDefaultByUserId(int userId);

        /// <summary>
        /// GetSalePointDefaultByUserIdAndBranchId
        /// Obtiene el punto de venta por default de una sucursal y usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="branchId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int GetSalePointDefaultByUserIdAndBranchId(int userId, int branchId);

        /// <summary>
        /// GetAccountingAccountByNumber
        /// Obtiene cuentas contables por número 
        /// </summary>
        /// <param name="accountingAccountNumber"></param>
        /// <param name="branchId"></param>
        /// <returns>List<AccountingAccountDTO/></returns>
        [OperationContract]
        List<AccountingAccountDTO> GetAccountingAccountByNumber(string accountingAccountNumber, int branchId);

        /// <summary>
        /// GetAccountingAccountByDescription
        /// Obtiene cuentas contables por descripción
        /// </summary>
        /// <param name="accountingAccountDescription"></param>
        /// <param name="branchId"></param>
        /// <returns>List<AccountingAccountDTO/></returns>
        [OperationContract]
        List<AccountingAccountDTO> GetAccountingAccountByDescription(string accountingAccountDescription, int branchId);

        ///<summary>
        /// GetTempAccountingTransactionItemByTempImputationId
        /// Obtiene un temporal de transacción contable dada la aplicación
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>List<TempDailyAccountingDTO/></returns>
        [OperationContract]
        List<ApplicationAccountingDTO> GetTempAccountingTransactionItemByTempApplicationId(int tempApplicationId);


        ///<summary>
        /// GetTempAccountingTransactionByTempAccountingApplicationId
        /// Obtiene un temporal de movimiento contable 
        /// </summary>
        /// <param name="tempAccountingApplicationId"></param>
        /// <returns>List<TempDailyAccountingDTO/></returns>
        [OperationContract]

        ApplicationAccountingDTO GetTempAccountingTransactionByTempAccountingApplicationId(int tempAccountingApplicationId);

        /// <summary>
        /// Guarda un listado de movimientos contables
        /// </summary>
        /// <param name="applicationAccountingTransaction">Listado de movimientos contables</param>
        /// <returns>Identificador del elemento creado</returns>
        [OperationContract]
        int SaveTempAccountingTransaction(ApplicationAccountingTransactionDTO applicationAccountingTransaction);

        /// <summary>
        /// DeleteTempApplicationAccounting
        /// Elimina un temporal de transacción contable
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteTempApplicationAccounting(int tempApplicationId);

        /// <summary>
        /// SaveBrokersCheckingAccount
        /// Graba una cuenta de agente
        /// </summary>
        /// <param name="brokersCheckingAccountTransaction"></param>
        /// <param name="applicationId"></param>
        /// <param name="accountingDate"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveBrokersCheckingAccount(BrokersCheckingAccountTransactionDTO brokersCheckingAccountTransaction, int applicationId, DateTime accountingDate);

        /// <summary>
        /// DeleteBrokersCheckingAccountItem
        /// Elimina una cuenta de agente
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <param name="brokersCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteBrokersCheckingAccountItem(int tempApplicationId, int brokersCheckingAccountItemId);

        /// <summary>
        /// UpdateTempBrokersCheckingAccountTotal
        /// </summary>
        /// <param name="tempBrokerCheckingAccountId"></param>
        /// <param name="selectedTotal"></param>
        /// <returns>BrokersCheckingAccountTransaction</returns>
        [OperationContract]
        BrokersCheckingAccountTransactionDTO UpdateTempBrokersCheckingAccountTotal(int tempBrokerCheckingAccountId, decimal selectedTotal);

        /// <summary>
        /// DeleteBrokerCheckingAccountItemChild
        /// Elimina un item de cuenta de agente
        /// </summary>
        /// <param name="tempBrokerCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteBrokerCheckingAccountItemChild(int tempBrokerCheckingAccountItemId);

        ///<summary>
        /// GetTempBrokerCheckingAccountItemByTempImputationId
        /// Obtiene un item de cuenta de agente
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>List<BrokerCheckingAccountItemDTO/></returns>
        [OperationContract]
        List<DTOs.Search.BrokerCheckingAccountItemDTO> GetTempBrokerCheckingAccountItemByTempApplicationId(int tempApplicationId);

        /// <summary>
        /// GetTempBrokerCheckingAccountItemChildByTempBrokerParentId
        ///  Obtiene un item temporal de cuenta de agente
        /// </summary>
        /// <param name="tempBrokerParentId"></param>
        /// <returns>List<BrokerCheckingAccountItemDTO/></returns>
        [OperationContract]
        List<DTOs.Search.BrokerCheckingAccountItemDTO> GetTempBrokerCheckingAccountItemChildByTempBrokerParentId(int tempBrokerParentId);

        /// <summary>
        /// SearchBrokersCheckingAccount
        /// Búsqueda de cuenta de agentes
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <returns>List<SearchAgentsItemsDTO/></returns>
        [OperationContract]
        List<SearchAgentsItemsDTO> SearchBrokersCheckingAccount(SearchParameterBrokersCheckingAccountDTO searchParameter);

        /// <summary>
        /// SaveBrokersCheckingAccountItem
        /// Registra una cuenta de agente
        /// </summary>
        /// <param name="brokerCheckingAccountItem"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveBrokersCheckingAccountItem(List<DTOs.Imputations.BrokerCheckingAccountItemDTO> brokerCheckingAccountItem);

        /// <summary>
        /// GetCurrencyDiferenceByCurrencyId
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>List<CurrencyDiferenceDTO/></returns>
        [OperationContract]
        List<CurrencyDiferenceDTO> GetCurrencyDiferenceByCurrencyId(int currencyId);

        /// <summary>
        /// GetCurrencyDifferenceByCurrencyId
        /// Obtiene el porcentaje de diferencia dada la moneda
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>decimal</returns>
        [OperationContract]
        decimal GetPercentageDifferenceByCurrencyId(int currencyId);

        /// <summary>
        /// SaveCoinsuranceCheckingAccountItems
        /// Graba cuenta de cheques de coaseguros
        /// </summary>
        /// <param name="coInsuranceCheckingAccountItems"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveCoinsuranceCheckingAccountItems(List<CoInsuranceCheckingAccountItemDTO> coInsuranceCheckingAccountItems);

        ///<summary>
        /// GetTempCoinsuranceCheckingAccountItemByTempImputationId
        /// Obtiene temporal de cuenta de cheques de coaseguros dada la imputación
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>List<CoinsuranceCheckingAccountItemDTO/></returns>
        [OperationContract]
        List<CoinsuranceCheckingAccountItemDTO> GetTempCoinsuranceCheckingAccountItemByTempApplicationId(int tempApplicationId);

        /// <summary>
        /// DeleteCoinsuranceCheckingAccountItem
        /// Actualiza cuenta de cheques de coaseguros
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <param name="coinsuranceCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteCoinsuranceCheckingAccountItem(int tempApplicationId, int coinsuranceCheckingAccountItemId);

        /// <summary>
        /// SaveReinsuranceCheckingAccount
        /// Registra cuenta de cheques de reaseguros
        /// </summary>
        /// <param name="reinsuranceCheckingAccountTransaction"></param>
        /// <param name="applicationId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveReinsuranceCheckingAccount(ReInsuranceCheckingAccountTransactionDTO reinsuranceCheckingAccountTransaction, int applicationId);

        /// <summary>
        /// DeleteReinsuranceCheckingAccountItem
        /// Eliminación de cuenta de cheques de reaseguros
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <param name="reinsuranceCheckingAccountItemId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteReinsuranceCheckingAccountItem(int tempApplicationId, int reinsuranceCheckingAccountItemId);

        ///<summary>
        /// GetTempReinsuranceCheckingAccountItemByTempImputationId
        /// Obtiene temporal cuenta de cheques de reaseguros por imputación
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>List<ReinsuranceCheckingAccountItemDTO/></returns>
        [OperationContract]
        List<DTOs.Search.ReinsuranceCheckingAccountItemDTO> GetTempReinsuranceCheckingAccountItemByTempApplicationId(int tempApplicationId);

        /// <summary>
        /// ValidateInsurancePolicyEndorsement
        /// Valida póliza, seguros y endoso
        /// </summary>
        /// <param name="policyNumber"></param>
        /// <param name="endorsementNumber"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ValidateInsurancePolicyEndorsement(string policyNumber, int endorsementNumber, int branchId, int prefixId);

        /// <summary>
        /// SaveTempInsuredLoanTransaction
        /// Graba Transacción Temporal de Préstamo de Asegurado
        /// </summary>
        /// <param name="tempInsuredLoanTransaction"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveTempInsuredLoanTransaction(InsuredLoanTransactionDTO tempInsuredLoanTransaction);

        /// <summary>
        /// DeleteTempInsuredLoanTransactionItem
        /// Elminia Transacción Temporal de Préstamo de Asegurado
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <param name="tempInsuredLoanId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteTempInsuredLoanTransactionItem(int tempApplicationId, int tempInsuredLoanId);

        /// <summary>
        /// GetTmpInsuredLoansByTempImputationId
        /// Obtiene Temporal de Préstamos de Asegurados 
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>List<InsuredLoanTransaction/></returns>
        [OperationContract]
        List<InsuredLoanTransactionDTO> GetTmpInsuredLoansByTempApplicationId(int tempApplicationId);

        /// <summary>
        /// GetInsuredLoanTransaction
        /// Ontiene Transacción de Préstamo de Asegurado
        /// </summary>
        /// <param name="insuredLoanTransaction"></param>
        /// <returns>InsuredLoanTransaction</returns>
        [OperationContract]
        InsuredLoanTransactionDTO GetInsuredLoanTransaction(InsuredLoanTransactionDTO insuredLoanTransaction);


        /// <summary>
        /// UpdateTempJournalEntry
        /// Actualiza asientos de diario temporal
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntry</returns>
        [OperationContract]
        JournalEntryDTO UpdateTempJournalEntry(JournalEntryDTO journalEntry);

        /// <summary>
        /// GetPremiumRecievableAppliedByCollectId
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="moduleId"></param>
        /// <returns>List<PremiumReceivableItemDTO/></returns>
        [OperationContract]
        List<PremiumReceivableItemDTO> GetPremiumRecievableAppliedByCollectId(int collectId, int moduleId);

        /// <summary>
        /// GetTempJournalEntryByTempId
        /// Obtiene asientos de diario temporales por su temporal
        /// </summary>
        /// <param name="tempJournalEntryId"></param>
        /// <returns>TempJournalEntryDTO</returns>
        [OperationContract]
        TempJournalEntryDTO GetTempJournalEntryByTempId(int tempJournalEntryId);

        ///<summary>
        /// GenerateCreditNoteRequest
        /// Realiza las operaciones para generar notas de crédito
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <param name="policyDocumentNumber"> </param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="userId"></param>
        ///<returns>List<JournalEntry/></returns>
        [OperationContract]
        List<JournalEntryDTO> GenerateCreditNoteRequest(JournalEntryDTO journalEntry, string policyDocumentNumber,
                                                                            string branchId, string prefixId, int userId);

        ///<summary>
        /// ValidateCreditNoteGeneration
        /// Realiza las operaciones para generar notas de crédito
        /// </summary>
        /// <param name="policyDocumentNumber"> </param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        ///<returns>bool</returns>
        [OperationContract]
        bool ValidateCreditNoteGeneration(string policyDocumentNumber, string branchId, string prefixId);

        /// <summary>
        /// SearchClaimsPaymentRequest
        /// Obtiene las solicitudes de pago de acuerdo a varios criterios de búsqueda
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <param name="typeSearch"></param>
        /// <returns>List<PaymentRequestVariousDTO/></returns>
        [OperationContract]
        List<PaymentRequestVariousDTO> SearchClaimsPaymentRequest(SearchParameterClaimsPaymentRequestDTO searchParameter, int typeSearch);

        /// <summary>
        /// DeleteClaimsPaymentRequestItem
        /// Borra una solicitud de pago de siniestro 
        /// </summary>
        /// <param name="claimsPaymentRequestId"></param>
        /// <param name="applicationId"></param>
        /// <param name="isPaymentVarious"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteTempClaimPaymentRequestItem(int claimsPaymentRequestId, int applicationId, bool isPaymentVarious);

        /// <summary>
        /// GetTempClaimsPaymentRequest
        /// Retorna los registros temporales desde pago de siniestro por el número de imputación deseada
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="isPaymentVarious"></param>
        /// <returns>List<TempPaymentRequestClaimDTO/></returns>
        [OperationContract]
        List<TempPaymentRequestClaimDTO> GetTempClaimsPaymentRequest(int applicationId, bool isPaymentVarious);

        /// <summary>
        /// DeleteTempPaymentRequestItem
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <param name="applicationId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteTempPaymentRequestItem(int paymentRequestId, int applicationId);

        /// <summary>
        /// GetTempPremiumAmountByApplicationId
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns>List<TempPaymentRequestClaimDTO></returns>
        [OperationContract]
        List<TempPaymentRequestClaimDTO> GetTempPremiumAmountByApplicationId(int applicationId);

        /// <summary>
        /// SaveTempPreLiquidation
        /// Graba las preliquidaciones en Temporales
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        [OperationContract]
        PreLiquidationDTO SaveTempPreLiquidation(PreLiquidationDTO preLiquidation);

        /// <summary>
        /// UpdateTempPreLiquidation
        /// Actualiza las preliquidaciones en Temporales
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        [OperationContract]
        PreLiquidationDTO UpdateTempPreLiquidation(PreLiquidationDTO preLiquidation);

        /// <summary>
        /// UpdatePreLiquidation
        /// Actualiza una preliquidación
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        [OperationContract]
        PreLiquidationDTO UpdatePreLiquidation(PreLiquidationDTO preLiquidation);

        /// <summary>
        /// GetTempPreLiquidation
        /// Trae preliquidaciones temporales
        /// </summary>
        /// <param name="preLiquidation"></param>
        /// <returns>PreLiquidation</returns>
        [OperationContract]
        PreLiquidationDTO GetTempPreLiquidation(PreLiquidationDTO preLiquidation);

        /// <summary>
        /// ConvertTempPreLiquidationToPreLiquidation
        /// Convierte preliquidaciones Temporales en reales
        /// </summary>
        /// <param name="tempPreLiquidationId"></param>
        /// <param name="tempApplicationId"></param>
        /// <param name="moduleId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ConvertTempPreLiquidationToPreLiquidation(int tempPreLiquidationId, int tempApplicationId, int moduleId);

        /// <summary>
        /// GetPreliquidations
        /// Obtiene preliquidaciones
        /// </summary>
        /// <param name="preliquidationsDto"></param>
        /// <returns>List<PreliquidationsDTO/></returns>
        [OperationContract]
        List<PreliquidationsDTO> GetPreliquidations(PreliquidationsDTO preliquidationsDto);

        /// <summary>
        /// CancelPreliquidation
        /// Cancela preliquidaciones
        /// </summary>
        /// <param name="preliquidationId"></param>
        /// <param name="tempApplicationId"></param>
        [OperationContract]
        /// <returns>bool</returns>
        bool CancelPreliquidation(int preliquidationId, int tempApplicationId);

        ///<summary>
        /// PremiumReceivableSearchPolicy
        /// Búsqueda de primas por cobrar de una póliza
        /// </summary>
        /// <param name="insuredId"></param>
        /// <param name="payerId"></param>
        /// <param name="agentId"></param>
        /// <param name="groupId"></param>
        /// <param name="policyId"></param>
        /// <param name="salesTicket"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        [OperationContract]
        /// <returns>List<PremiumReceivableSearchPolicyDTO/></returns>
        List<PremiumReceivableSearchPolicyDTO> PremiumReceivableSearchPolicy(string insuredId, string payerId, string agentId,
                                                      string groupId, string policyId, string policyDocumentNumber, string salesTicket,
                                                      string branchId, string prefixId, string endorsementId,
                                                       string dateFrom, string dateTo, string insuredDocumentNumber, int pageSize, int pageIndex);

        ///<summary>
        /// GetTempPremiumReceivableItemByTempImputationId
        /// Trae un item de prima por cobrar
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>List<PremiumReceivableItemDTO/></returns>
        [OperationContract]
        List<PremiumReceivableItemDTO> GetTempApplicationPremiumByApplicationId(int tempApplicationId);

        ///<summary>
        /// SaveTempPremiumRecievableTransaction
        /// Graba una prima por cobrar
        /// </summary>
        /// <param name="premiumRecievableTransaction"></param>
        /// <param name="applicationId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="userId"></param>
        /// <param name="registerDate"></param>
        /// <param name="accountingDate"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveTempPremiumRecievableTransaction(PremiumReceivableTransactionDTO premiumRecievableTransaction, int applicationId, decimal exchangeRate, int userId, DateTime registerDate, DateTime accountingDate);

        ///<summary>
        /// SaveTempApplicationPremiumComponents
        /// Graba una prima por cobrar
        /// </summary>
        /// <param name="tempApplicationPremiumDTO"></param>
        /// <returns>if could save, return true</returns>
        [OperationContract]
        bool SaveTempApplicationPremiumComponents(TempApplicationPremiumDTO tempApplicationPremiumDTO);

        /// <summary>
        /// DeleteTempPremiumRecievableTransactionItem
        /// Borrado de un item de primas x cobrar
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <param name="tempApplicationPremiumId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteTempPremiumRecievableTransactionItem(int tempApplicationId, int tempApplicationPremiumId, bool IsReversion = false);

        [OperationContract]
        List<DiscountedCommissionDTO> SearhDiscountedCommission(string policyId, string endorsementId);

        /// <summary>
        /// SaveTempUsedDepositPremiumRequest
        /// Graba temporal primas en depósito usadas
        /// </summary>
        /// <param name="tempUsedDepositPremiums"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveTempUsedDepositPremiumRequest(List<TempUsedDepositPremiumDTO> tempUsedDepositPremiums);

        /// <summary>
        /// GetDepositPremiumTransactionByPayerId
        /// Obtiene un item de primas por cobrar el pagador
        /// </summary>
        /// <param name="payerId"></param>
        /// <returns>List<DepositPremiumTransactionDTO/></returns>
        [OperationContract]
        List<DTOs.Search.DepositPremiumTransactionDTO> GetDepositPremiumTransactionByPayerId(int payerId);

        /// <summary>
        /// DeleteTempUsedDepositPremiumRequest
        /// Borra primas en depósito que están en temporales.
        /// </summary>
        /// <param name="tempApplicationPremiumId"></param>
        [OperationContract]
        void DeleteTempUsedDepositPremiumRequest(int tempApplicationPremiumId);

        /// <summary>
        /// ValidatePolicyComponents: Valida que la póliza tenga componentes.
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool ValidatePolicyComponents(int policyId, int endorsementId);

        /// <summary>
        /// GetPaymentsByCollectId
        /// Obtiene los detalles del Recibo
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>List<Payment></returns>
        [OperationContract]
        List<DTOs.Payments.PaymentDTO> GetPaymentsByCollectId(int collectId);

        /// <summary>
        /// ReverseApplicationRequest
        /// Método para reversar una imputación de recibo
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="moduleId"></param>
        /// <param name="userId"></param>
        ///<returns>bool</returns>
        [OperationContract]
        bool ReverseApplicationRequest(int collectId, int moduleId, int userId);

        /// <summary>
        /// UpdateJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntry</returns>
        [OperationContract]
        JournalEntryDTO UpdateJournalEntry(JournalEntryDTO journalEntry);

        /// <summary>
        /// TemporarySearch
        /// Búsqueda de temporales
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <returns>List<TemporaryItemSearchDTO/></returns>
        [OperationContract]
        List<TemporaryItemSearchDTO> TemporarySearch(SearchParameterTemporaryDTO searchParameter);

        /// <summary>
        /// GetImputationTypes
        /// Obtiene los tipos de imputación
        /// </summary>
        /// <returns>List<ImputationType/></returns>
        [OperationContract]
        List<ApplicationTypeDTO> GetApplicationTypes();

        ///<summary>
        /// DeletePremiumReceivableTransaction
        /// Borrado de primas x cobrar
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteTempApplicationPremiumTransaction(int tempApplicationId);

        /// <summary>
        /// RecalculatingForeignCurrencyAmount
        /// Permite actualizar la tasa de cambio a la fecha actual a una imputación temporal
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <param name="moduleId"></param>
        /// <param name="sourceId"></param>
        /// <param name="foreignCurrencyExchangeRates"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool RecalculatingForeignCurrencyAmount(int tempApplicationId, int moduleId, int sourceId, List<ForeignCurrencyExchangeRate> foreignCurrencyExchangeRates);

        /// <summary>
        /// SaveApplicationRequest
        /// Ejecuta el proceso de grabación de una imputación
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="tempApplicationId"></param>
        /// <param name="moduleId"></param>
        /// <param name="comments"></param>
        /// <param name="statusId"></param>
        /// <param name="userId"></param>
        /// <param name="tempSourceCode"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveApplicationRequest(int sourceCode, int tempApplicationId, int moduleId,
                                           string comments, int statusId, int userId, int tempSourceCode, int technicalTransaction,DateTime accountingDate);

        /// <summary>
        /// GetCompaniesByUserId
        /// Trae companías por usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List<CompanyDTO/></returns>
        [OperationContract]
        List<CompanyDTO> GetCompaniesByUserId(int userId);

        /// <summary>
        /// GetParameterMulticompany
        /// Trae parámetros de multicompanía
        /// </summary>
        /// <returns>bool</returns>
        [OperationContract]
        bool GetParameterMulticompany();

        /// <summary>
        /// UpdateTempImputationSourceCode
        /// Edita un temporal de imputación por el tipo de origen
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <param name="sourceId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int UpdateTempApplicationSourceCode(int tempApplicationId, int sourceId);

        /// <summary>
        /// Obtiene los cobros a partir de póliza y endoso
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns>List<AppPaymentPolicyDTO/></returns>
        [OperationContract]
        List<AppPaymentPolicyDTO> GetCollections(int policyId, int endorsementId);
        /// <summary>
        /// Eliminacion de temporales
        /// </summary>
        /// <param name="tempImputationId"></param>
        [OperationContract]
        bool CancelAppliationReceipt(int tempImputationId, int moduleId = 0);

        /// <summary>
        /// GetConceptCurrentAccountCode
        /// Trae código de concepto de pago
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="conceptDescription"></param>
        /// <param name="sourceId"></param>
        /// <returns>List<ConceptCurrentAccountDTO/></returns>
        [OperationContract]
        List<ConceptCurrentAccountDTO> GetConceptCurrentAccountCode(int branchId, int userId, string conceptDescription, int sourceId);

        /// <summary>
        /// GetConceptCurrentAccountDescription
        /// Trae descripción de concepto de pago
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="conceptId"></param>
        /// <param name="sourceId"></param>
        /// <returns>List<ConceptCurrentAccountDTO></returns>
        [OperationContract]
        List<ConceptCurrentAccountDTO> GetConceptCurrentAccountDescription(int branchId, int userId, int conceptId, int sourceId);

        /// <summary>
        /// SavePartialClousureAgentsRequest
        /// </summary>
        /// <param name="dateTo"></param>
        /// <param name="dateFrom"></param>
        /// <param name="userId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SavePartialClousureAgentsRequest(DateTime dateTo, DateTime dateFrom, int userId, int typeProcess);

        /// <summary>
        /// SaveCommissionPaymentOrder
        /// Permite generar una orden de pago comisiones
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
        /// <returns>List<PaymentOrder/></returns>
        [OperationContract]
        List<DTOs.AccountsPayables.PaymentOrderDTO> SaveCommissionPaymentOrder(int branchId, int companyId,
                                                            DateTime estimatedPaymentDate, int agentId, string agentName,
                                                            DateTime accountingDate, int userId, int salePointId, int processNumber);

        /// <summary>
        /// GetCommissionBalance
        /// Trae balance de comisiones
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="accountingCompanyId"></param>
        /// <param name="agentId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        ///  <returns>List<AgentCommissionBalanceItemDTO/></returns>
        [OperationContract]
        List<AgentCommissionBalanceItemDTO> GetCommissionBalance(int branchId, int accountingCompanyId, int agentId, DateTime dateFrom, DateTime dateTo);

        /// <summary>
        /// MassiveDataForGenerate
        /// Genera proceso masivo
        /// </summary>
        /// <param name="issueDate"></param>
        /// <returns>List<MassiveDataGenerateDTO/></returns>
        [OperationContract]
        List<MassiveDataGenerateDTO> MassiveDataForGenerate(DateTime issueDate);

        /// <summary>
        /// GetPendingCommission
        /// Obtiene la comision pendiente, a partir de la poliza y el endoso
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns>PendingCommissionDTO</returns>
        [OperationContract]
        PendingCommissionDTO GetPendingCommission(int policyId, int endorsementId);

        ///<summary>
        /// PremiumReceivableStatus
        /// Consulta de saldos en cuotas de póliza
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
        List<PremiumReceivableSearchPolicyDTO> PremiumReceivableStatus(string insuredId, string policyDocumentNumber,
                                                                        string branchId, string prefixId, string endorsementId,
                                                                        string policiesWithPortfolio, int pageSize, int pageIndex, string ExpirationDateFrom, string ExpirationDateTo);

        ///// <summary>
        ///// TODO: Alejandro Villagrán - Método temporal para recuotificaciones, se lo usa para mejorar el rendimiento en la búsqueda de póliza por autocomplete.
        ///// </summary>
        ///// <param name="policyDocumentNumber"></param>
        ///// <param name="prefixId"></param>
        ///// <param name="branchId"></param>
        ///// <returns>List<Policy/></returns>
        //[OperationContract]
        //List<PolicyDTO> GetPoliciesByBranchAndPrefix(string policyDocumentNumber, int prefixId, int branchId);

        /// <summary>
        /// GetPaymentOrdersCommission
        /// Permite obtener las órdenes de pago comisiones para el reporte
        /// </summary>
        /// <param name="paymentOrders"></param>
        /// <returns>List<PaymentOrdersCommissionDTO/></returns>
        [OperationContract]
        List<PaymentOrdersCommissionDTO> GetPaymentOrdersCommission(List<int> paymentOrders);

        ///<summary>
        /// GenerateCreditNoteReport
        /// Realiza las operaciones para generar notas de crédito
        /// </summary>
        /// <param name="policyDocumentNumber"> </param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        ///<returns>List<EndorsementPaymentDTO/></returns>
        [OperationContract]
        List<EndorsementPaymentDTO> GenerateCreditNoteReport(string policyDocumentNumber, string branchId, string prefixId);

        /// <summary>
        /// SaveCoinsuranceCheckingAccount
        /// Graba cuenta de cheques de coaseguros
        /// </summary>
        /// <param name="coinsuranceCheckingAccountTransaction"></param>
        /// <param name="applicationId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveCoinsuranceCheckingAccount(CoInsuranceCheckingAccountTransactionDTO coinsuranceCheckingAccountTransaction, int applicationId);

        /// <summary>
        /// Borra los temporales de aplicación
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <param name="sourceCode"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteTemporaryApplicationRequest(int tempImputationId, int imputationTypeId, int sourceCode);

        /// <summary>
        /// SaveJournalEntryImputation
        /// </summary>
        /// <param name="tempJournalEntryId"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="userId"></param>
        /// <returns>CollectImputation</returns>
        [OperationContract]
        CollectApplicationDTO SaveJournalEntryImputation(int tempJournalEntryId, int tempImputationId, int userId);

        /// <summary>
        /// SaveApplication
        /// Graba la imputación
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="tempApplicationId"></param>
        /// <param name="moduleId"></param>
        /// <param name="comments"></param>
        /// <param name="statusId"></param>
        /// <param name="userId"></param>
        /// <param name="tempSourceCode"></param>
        /// <returns>CollectImputation</returns>
        [OperationContract]
        CollectApplicationDTO SaveApplication(int sourceCode, int tempApplicationId, int moduleId,
                                                string comments, int statusId, int userId, int tempSourceCode, int technicalTransaction,DateTime accountingDate);

        /// <summary>
        /// SaveImputationRequestBill
        /// Ejecuta el proceso de grabación de una imputación
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="comments"></param>
        /// <param name="userId"></param>
        /// <param name="tempSourceCode"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveImputationRequestBillWithTransaction(int sourceCode, int tempImputationId, string comments,
                                              int userId, int tempSourceCode, int technicalTransaction,DateTime accountingDate);

        /// <summary>
        /// GetTempApplicationPremiumComponentsByTemApp
        /// </summary>
        /// <param name="tempApp"></param>
        /// <returns></returns>
        [OperationContract]
        List<TempApplicationPremiumComponentDTO> GetTempApplicationPremiumComponentsByTemApp(int tempApp);

        /// <summary>
        /// UpdTempApplicationPremiumComponents
        /// </summary>
        /// <param name="updTempApplicationPremiumComponentDTO"></param>
        /// <returns></returns>
        [OperationContract]
        List<TempApplicationPremiumComponentDTO> UpdTempApplicationPremiumComponents(UpdTempApplicationPremiumComponentDTO updTempApplicationPremiumComponentDTO);

        /// <summary>
        /// GetApplicationPremiumsByApplicationPremiumId
        /// </summary>
        /// <param name="applicationPremiumId"></param>
        /// <returns></returns>
        List<ApplicationPremiumDTO> GetApplicationPremiumsByApplicationPremiumId(int applicationPremiumId);

        /// <summary>
        /// GetApplicationPremiumsByApplicationPremiums
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ApplicationPremiumDTO> GetApplicationPremiumsByApplicationPremiums(DateTime accountingDate);

        /// <summary>
        /// GetApplicationIdByTechnicalTransaction
        /// Obtiene ApplicationId a partir de technicalTransaction
        /// </summary>
        /// <param name="updTempApplicationPremiumComponentDTO"></param>
        /// <returns></returns>
        [OperationContract]
        int GetApplicationIdByTechnicalTransaction(int technicalTransaction);

        /// <summary>
        /// Obtiene el identificador de la cuenta para un banco / moneda
        /// </summary>
        /// <param name="bankId">Identificador del banco</param>
        /// <param name="currencyId">Identificador de la moneda</param>
        /// <returns>Identificador de la cuenta</returns>
        [OperationContract]
        int GetAccountingAccountIdByBankIdCurrencyId(int bankId, int currencyId);

        [OperationContract]
        int GetAccountingAccountIdByBankIdCurrencyIdAccountNumber(int bankId, int currencyId, string accountNumer);

        /// <summary>
        /// GetApplicationByApplication
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [OperationContract]
        CollectApplicationDTO GetApplicationByApplication(ApplicationDTO application);

        /// <summary>
        /// GetApplicationByTechnicalTransaction
        /// </summary>
        /// <param name="technicalTransaction"></param>
        /// <returns></returns>
        [OperationContract]
        CollectApplicationDTO GetApplicationByTechnicalTransaction(int technicalTransaction);

        /// <summary>
        /// GetApplicationAccountByApplicationId
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [OperationContract]
        List<ApplicationAccountingDTO> GetApplicationAccountByApplicationId(int applicationId);

        /// <summary>
        /// GetApplicationAccountByApplicationId
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [OperationContract]
        List<ApplicationAccountingDTO> GetApplicationAccountsByApplicationId(int applicationId);

        /// <summary>
        /// ReverseApplicationPremiumByCollectPaymentId
        /// </summary>
        /// <param name="collectPaymentId"></param>
        /// <returns></returns>
        [OperationContract]
        bool ReverseApplicationPremiumByCollectPaymentId(int collectPaymentId);
        /// <summary>
        /// ReverseApplicationPremiumByCollectPaymentId
        /// </summary>
        /// <param name="collectPaymentId"></param>
        /// <returns></returns>
        [OperationContract]
        bool ReverseApplicationPremiumByPremiumId(int premiumId);
        /// <summary>
        /// GetTempApplicationPremiumCommissDTOs
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ApplicationPremiumCommisionDTO> GetTempApplicationPremiumCommissDTOs();
        /// <summary>
        /// GetTempApplicationPremiumCommissByTempAppId
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<DiscountedCommissionDTO> GetTempApplicationPremiumCommissByTempAppId(string policyId, string endorsementId, int tempAppPremiumId);
        /// <summary>
        /// CreateTempApplicationPremiumCommisses
        /// </summary>
        /// <param name="tempApplicationPremiumCommiss"></param>
        /// <returns></returns>
        [OperationContract]
        ApplicationPremiumCommisionDTO CreateTempApplicationPremiumCommisses(TempApplicationPremiumCommissDTO tempApplicationPremiumCommiss);
        /// <summary>
        /// UpdateTempApplicationPremiumCommisses
        /// </summary>
        /// <param name="tempApplicationPremiumCommiss"></param>
        /// <returns></returns>
        [OperationContract]
        ApplicationPremiumCommisionDTO UpdateTempApplicationPremiumCommisses(TempApplicationPremiumCommissDTO tempApplicationPremiumCommiss);
        /// <summary>
        /// DeleteTempApplicationPremiumCommisses
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteTempApplicationPremiumCommisses(int id);
        /// <summary>
        /// GetApplicationPremiumCommisions
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ApplicationPremiumCommisionDTO> GetApplicationPremiumCommisions();
        /// <summary>
        /// GetApplicationPremiumCommisionsByApplicationPremiumId
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ApplicationPremiumCommisionDTO> GetApplicationPremiumCommisionsByApplicationPremiumId(int applicationPremiumId);
        /// <summary>
        /// CreateApplicationPremiumCommision
        /// </summary>
        /// <param name="applicationPremiumCommisionDTO"></param>
        /// <returns></returns>
        [OperationContract]
        ApplicationPremiumCommisionDTO CreateApplicationPremiumCommision(ApplicationPremiumCommisionDTO applicationPremiumCommisionDTO);
        /// <summary>
        /// UpdateApplicationPremiumCommision
        /// </summary>
        /// <param name="applicationPremiumCommisionDTO"></param>
        /// <returns></returns>
        [OperationContract]
        ApplicationPremiumCommisionDTO UpdateApplicationPremiumCommision(ApplicationPremiumCommisionDTO applicationPremiumCommisionDTO);
        /// <summary>
        /// DeleteApplicationPremiumCommision
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteApplicationPremiumCommision(int id);

        [OperationContract]
        List<ApplicationAccountingAnalysisDTO> GetTempApplicationAccountingAnalysisByTempAppAccountingId(int tempAppAccountingId);

        /// <summary>
        /// GetTempApplicationAccountingCostCentersByTempAppAccountingId
        /// </summary>
        /// <param name="tempAppAccountingId"></param>
        /// <returns>List<ApplicationAccountingCostCenterDTO></returns>
        [OperationContract]
        List<ApplicationAccountingCostCenterDTO> GetTempApplicationAccountingCostCentersByTempAppAccountingId(int tempAppAccountingId);

        /// <summary>
        /// Return coinsurance percentage participation for an endorsement
        /// </summary>
        /// <param name="endorsementId">Endorsement Identifier</param>
        /// <returns></returns>
        [OperationContract]
        decimal GetParticipationPercentageByEndorsementId(int endorsementId);
        #region validacion temporales
        [OperationContract]
        TemporalPremiumDTO ValidatePremiumTemporal(PremiumFilterDTO premiumFilterDTO);
        #endregion

        #region Aplicacion Primas Portal
        /// <summary>
        /// Saves the temporary premium component.
        /// </summary>
        /// <param name="premiumRequestDTO">The premium request dto.</param>
        /// <returns></returns>
        int SaveTmpPremiumComponent(PremiumRequestDTO premiumRequestDTO);
        #endregion Aplicacion Primas Portal

        /// <summary>
        /// Save temporal application to real
        /// </summary>
        /// <param name="tempApplicationId">Identifier for temporal</param>
        /// <param name="userId">Identifier for user</param>
        /// <returns>Message response</returns>
        [OperationContract]
        MessageDTO SaveApplicationByTempApplicationIdUserId(int tempApplicationId, int userId);

        /// <summary>
        /// Get list of payment quotas
        /// </summary>
        /// <param name="searchPolicyPayment">Search Criteria</param>
        /// <returns>List of payment quotas</returns>
        [OperationContract]
        List<Integration.UndewritingIntegrationServices.DTOs.PremiumSearchPolicyDTO> GetPaymentQuotas(
            Integration.UndewritingIntegrationServices.DTOs.SearchPolicyPaymentDTO searchPolicyPayment);

        /// <summary>
        /// Get payer payment components by endorsement identfier and quota number
        /// </summary>
        /// <param name="endorsementId">Endorsement identifier</param>
        /// <param name="quotaNumber">Quota Number</param>
        /// <returns>Payer payment components</returns>
        [OperationContract]
        List<Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.PayerPaymentComponentDTO>
            GetPayerPaymentComponentsByEndorsementIdQuotaNumber(int endorsementId, int quotaNumber);
        [OperationContract]
        int CheckoutAnalysisCodeByAnalysisConceptKeyIdKeyValue(int AnalysisConceptKeyId, string KeyValue);
        /// <summary>
        /// Saves the payment authorization.
        /// </summary>
        /// <param name="ballotNumber">The ballot number.</param>
        /// <returns></returns>
        [OperationContract]
        long SavePaymentAuthorization(string ballotNumber);

        /// <summary>
        /// obtienen la applicaion temporal por el id de applicacion.
        /// </summary>
        /// <param name="searchPolicyPayment">Search Criteria</param>
        /// <returns>List of status payment quotas</returns>
        [OperationContract]
        List<Integration.UndewritingIntegrationServices.DTOs.PremiumSearchPolicyDTO> GetTempApplicationsPremiumByTempApplicationId(
            int tempApplicationId);

        /// <summary>
        /// Saves the payment authorization.
        /// </summary>
        /// <param name="ballotNumber">The ballot number.</param>
        /// <param name="technicalTransaction">The technical transaction.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        /// <exception cref="Exception"></exception>
        [OperationContract]
        long UpdatePaymenTechnicalTransactionforAuthorization(long pymentAuthorizationId, int technicalTransaction);
        
        /// <summary>
        /// Reverse application
        /// </summary>
        /// <param name="sourceId">Source id</param>
        /// <param name="moduleId">Module id</param>
        /// <param name="userId">User id</param>
        /// <returns>Message for to check if reversios was successful</returns>
        [OperationContract]
        MessageDTO ReverseApplication(int sourceId, int moduleId, int userId);

        /// <summary>
        /// Get list of application premiums by endorsement id
        /// </summary>
        /// <param name="endorsementId">Endorsement identifier</param>
        /// <returns>List of application premiums</returns>
        [OperationContract]
        List<ApplicationPremiumDTO> GetApplicationPremiumsByEndorsementId(int endorsementId);

        /// <summary>
        /// Reverse journal entry
        /// </summary>
        /// <param name="parameters">Parameteres for reversion</param>
        /// <returns>Message</returns>
        [OperationContract]
        MessageDTO ReverseJournalEntry(JournalEntryReversionParametersDTO parameters);

        /// <summary>
        /// SaveTempApplicationData
        /// </summary>
        /// <param name="applicationRequest">parameters for save application</param>
        /// <returns></returns>
        [OperationContract]
        AAPAY.PaymentOrderDTO SaveTempApplicationData(PremiumRequestDTO applicationRequest);

        /// <summary>
        /// SaveLogMassiveDataPolicy
        /// </summary>
        /// <param name="logMassiveData">parameters for save log masiive data</param>
        /// <returns></returns>
        [OperationContract]
        bool SaveLogMassiveDataPolicy(LogMassiveDataPolicyDTO logMassiveData);

        /// <summary>
        /// Check if exists payment ticket for portal payment
        /// </summary>
        /// <param name="ballotNumber">Payment ticket for portal payment</param>
        /// <returns></returns>
        [OperationContract]
        bool ExistsPaymenTechnicalTransactioByBallotNumber(string ballotNumber);
    }
}