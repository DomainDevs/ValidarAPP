
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.AccountingServices.DTOs;
using SearchDTO=Sistran.Core.Application.AccountingServices.DTOs.Search;

namespace Sistran.Core.Application.AccountingServices
{
    [ServiceContract]
    public interface IAccountingCollectService
    {


        #region CollectImputation

        /// <summary>
        /// Guarda una aplicación
        /// </summary>
        /// <param name="collectImputation">Aplicación</param>
        /// <param name="collectControlId">Identificador de la caja</param>
        /// <param name="mustToApply">Indica si debe convertir la aplicación temporal a permanente</param>
        /// <returns>Aplicación</returns>
        [OperationContract]
        CollectApplicationDTO SaveCollectImputation(CollectApplicationDTO collectImputation , int collectControlId, bool mustToApply);

        /// <summary>
        /// UpdateCollectImputation
        /// </summary>
        /// <param name="collectImputation"></param>        
        /// <returns>CollectApplicationDTO</returns>
        [OperationContract]
        CollectApplicationDTO UpdateCollectImputation(CollectApplicationDTO collectImputation);


        /// <summary>
        /// GetCollectImputations
        /// </summary>
        /// <param name="collectImputation"></param>
        /// <returns>List<CollectApplicationDTO/></returns>
        [OperationContract]
        List<CollectApplicationDTO> GetCollectImputations(CollectApplicationDTO collectImputation);


        #endregion

        #region Collect


        /// <summary>
        /// SaveRegularizationCollect
        /// 
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="collectControlId"></param>
        /// <param name="sourcePaymentId"></param>
        /// <returns>Collect</returns>
        [OperationContract]
        CollectDTO SaveRegularizationCollect(CollectDTO collect, int collectControlId, int sourcePaymentId, int brachId );

        /// <summary>
        /// UpdateCollect
        /// 
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="collectControlId"></param>        
        [OperationContract]
        CollectDTO UpdateCollect(CollectDTO collect, int collectControlId);


        /// <summary>
        /// CancelCollect
        /// Cancela o anula un recibo
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="collectControlId"></param>
        /// <param name="authorizationUserId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int CancelCollect(int collectId, int collectControlId, int authorizationUserId);


        /// <summary>
        /// GetCollect: Obtener Recibo
        /// </summary>
        /// <param name="collect"></param>
        /// <returns>Collect</returns>
        [OperationContract]
        CollectDTO GetCollect(CollectDTO collect);

        /// <summary>
        /// GetCollect: Obtener Recibo por sourcecode (technicaltransaction)
        /// </summary>
        /// <param name="collect"></param>
        /// <returns>Collect</returns>
        [OperationContract]
        CollectDTO GetCollectByTechnicalTransaction(int technicalTransaction);

        #endregion Collect



        #region PolicySearch

        /// <summary>
        /// GetPoliciesByCollectId
        /// Obtiene las pólizas 
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>List<CollectItemPolicyDTO/></returns>
        [OperationContract]
        List<SearchDTO.CollectItemPolicyDTO> GetPoliciesByCollectId(int collectId);
        #endregion

        #region CollectItem


        /// <summary>
        /// GetPoliciesByCollectItemId
        /// Obtiene las pólizas 
        /// </summary>
        /// <param name="collectItemId"></param>
        /// <returns>List<CollectItemPolicyDTO/></returns>
        [OperationContract]
        List<SearchDTO.CollectItemPolicyDTO> GetPoliciesByCollectItemId(int collectItemId);


        /// <summary>
        /// GetCollectItemsWithoutPaymentTicket
        /// </summary>
        /// <param name="collectControlId"></param>
        /// <returns>List<CollectItemWithoutPaymentTicketDTO/></returns>
        [OperationContract]
        List<SearchDTO.CollectItemWithoutPaymentTicketDTO> GetCollectItemsWithoutPaymentTicket(int collectControlId);


        #endregion

        #region BalanceInquiries
        /// <summary>
        /// BankBallotToDeposit
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="user"></param>
        /// <param name="date"></param>        
        /// <returns>PaginatedResponse</returns>
        [OperationContract]
        List<SearchDTO.BalanceInquieriesDTO> GetBalanceInquiries(int branch, int userId, string date);

        /// <summary>
        /// GetUserInquiries
        /// </summary>
        /// <param name="branch"></param>
        /// <returns>List<User/></returns>
        [OperationContract]
        List<int> GetUserInquiries(int branch);

        #endregion

        #region IncomeChangeConcept

        /// <summary>
        /// GetReceiptForExchangeConcept
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="accountingDate"></param>
        /// <returns>IncomeChangeConceptDTO</returns>
        [OperationContract]
        SearchDTO.IncomeChangeConceptDTO GetReceiptForExchangeConcept(int collectId, System.DateTime accountingDate);

        /// <summary>
        /// UpdateIncomeChangeConcept
        /// </summary>
        /// <param name="collectId"></param>
        /// <param name="collectControlId"></param>
        /// <param name="collectConceptId"></param>
        /// <returns>Collect</returns>
        [OperationContract]
        CollectDTO UpdateIncomeChangeConcept(int collectId, int collectControlId, int collectConceptId);

        #endregion

        #region DailyClosingCash

        /// <summary>
        /// GetBranchesOpenStatus
        /// </summary>
        /// <returns>List<Branch/></returns>
        [OperationContract]
        List<SearchDTO.BranchDTO> GetBranchesOpenStatus();

        /// <summary>
        /// ValidateCheckCardDeposited
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <returns>decimal</returns>
        [OperationContract]
        decimal ValidateCheckCardDeposited(int branchId, int userId);

        /// <summary>
        /// ValidateCashDeposited
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="currencyId"></param>
        /// <returns>decimal</returns>
        [OperationContract]
        decimal ValidateCashDeposited(int branchId, int userId, int currencyId);

        /// <summary>
        /// GetRegisterDateCollectControl
        /// </summary>
        /// <param name="collectControlId"></param>
        /// <returns>string</returns>
        [OperationContract]
        string GetRegisterDateCollectControl(int collectControlId);

        /// <summary>
        /// GetCashierByBranchId
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>List<User/></returns>
        [OperationContract]
        List<int> GetCashierByBranchId(int branchId);

        #endregion


        #region SearchCollects


        /// <summary>
        /// SearchCollects
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="collectConceptId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="userId"></param>
        /// <param name="collectId"></param>
        /// <param name="imputationType"></param>   
        /// <param name="journalEntryStatusId"></param>   
        /// <returns>List<SearchCollectDTO/></returns>
        [OperationContract]
        List<SearchDTO.SearchCollectDTO> SearchCollects(int branchId, int collectConceptId, string startDate, string endDate, int userId, int collectId, int imputationType, int journalEntryStatusId);


        /// <summary>
        /// GetReceiptApplicationInformation
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>List<SearchCollectDTO/></returns>
        [OperationContract]
        List<SearchDTO.SearchCollectDTO> GetReceiptApplicationInformation(int collectId);

        /// <summary>
        /// obtiene información de aplicación de recibo por technicalTransaction
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>List<SearchCollectDTO/></returns>
        [OperationContract]
        List<SearchDTO.SearchCollectDTO> GetReceiptApplicationTechnicalTransaction(int technicalTransaction);

        /// <summary>
        /// GetCollectIdByJournalEntryId
        /// Obtiene Id del recibo a partir del id de la imputación de asiento diario
        /// </summary>
        /// <param name="journalEntryId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int GetCollectIdByJournalEntryId(int journalEntryId);
        #endregion


        #region AccountingCompany

        /// <summary>
        /// GetAccountingCompany
        /// </summary>
        /// <returns>List<ParametrizationModels.Individuals.Company/></returns>
        [OperationContract]
        List<DTOs.CompanyDTO> GetAccountingCompany();

        #endregion


        #region MassiveProcess

        /// <summary>
        /// GetMassiveProcess
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List<MassiveProcessDTO/></returns>
        [OperationContract]
        List<SearchDTO.MassiveProcessDTO> GetMassiveProcess(int userId);

        ///<summary>
        /// SaveCollectMassiveProcess
        /// </summary>
        /// <param name="collectMassiveProcess"></param>
        /// <returns>CollectMassiveProcessDTO</returns>
        [OperationContract]
        SearchDTO.CollectMassiveProcessDTO SaveCollectMassiveProcess(SearchDTO.CollectMassiveProcessDTO collectMassiveProcess);

        /// <summary>
        /// UpdateCollectMassiveProcess
        /// </summary>
        /// <param name="collectMassiveProcess"></param>
        /// <returns>CollectMassiveProcessDTO</returns>
        [OperationContract]
        SearchDTO.CollectMassiveProcessDTO UpdateCollectMassiveProcess(SearchDTO.CollectMassiveProcessDTO collectMassiveProcess);


        #endregion

        /// <summary>
        /// GetCollectByCollectId
        /// </summary>
        /// <returns>Collect</returns>
        [OperationContract]
        CollectDTO GetCollectByCollectId(int collectId);


        [OperationContract]
        int GetTechnicalTransactionByPaymentId(int paymentId);

        /// <summary>
        /// Get avaible currencies
        /// </summary>
        /// <returns>Currencies</returns>
        [OperationContract]
        List<DTOs.Search.CurrencyDTO> GetAvaibleCurrencies();

        /// <summary>
        /// Get avaible banks by currency id
        /// </summary>
        /// <param name="currencyId">Currency identifier</param>
        /// <returns>Banks</returns>
        [OperationContract]
        List<BankDTO> GetAvaibleBanksByCurrencyId(int currencyId);

        /// <summary>
        /// Get avaible bank accounts
        /// </summary>
        /// <param name="currencyId">Currency Identifier</param>
        /// <param name="bankId">Bank identifier</param>
        /// <returns>Bank accounts</returns>
        [OperationContract]
        List<DTOs.BankAccounts.BankAccountCompanyDTO> GetAvaibleAccountsByCurrencyIdBankId(int currencyId, int bankId);
    }
}
