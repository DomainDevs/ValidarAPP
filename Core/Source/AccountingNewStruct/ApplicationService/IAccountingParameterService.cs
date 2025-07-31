using System.Collections.Generic;
using System.ServiceModel;
using System;
using Sistran.Core.Application.AccountingServices.DTOs;
using SearchDTO=Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.CancellationPolicies;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices
{
    [ServiceContract]
    public interface IAccountingParameterService
    {

        #region CollectConcept

        /// <summary>
        /// SaveCollectConcept
        /// Se guarda los conceptos de facturación
        /// <param name="collectConcept"></param>
        /// </summary>            
        /// <returns>CollectConcept</returns>
        [OperationContract]
        CollectConceptDTO SaveCollectConcept(CollectConceptDTO collectConcept);

        /// <summary>
        /// UpdateCollectConcept: Se guarda los conceptos de facturación
        /// <param name="collectConcept"></param>
        /// </summary>            
        /// <returns>CollectConcept</returns>
        [OperationContract]
        CollectConceptDTO UpdateCollectConcept(CollectConceptDTO collectConcept);

        /// <summary>
        /// DeleteCollectConcept
        /// Se elimina los conceptos de facturación
        /// <param name="collectConcept"></param>
        /// </summary>            
        /// <returns></returns>
        [OperationContract]
        void DeleteCollectConcept(CollectConceptDTO collectConcept);


        /// <summary>
        /// GetCollectConcepts
        /// Se obtiene un concepto de facturación
        /// </summary>            
        /// <returns> List<CollectConcept></returns>
        [OperationContract]
        List<CollectConceptDTO> GetCollectConcepts();


        #endregion


        #region CollectPaymentMethodType

        /// <summary>
        /// SaveCollectPaymentMethodType: Se guarda los conceptos de facturación
        /// </summary>
        /// <param name="id"></param>
        /// <param name="methodType"></param>
        /// <param name="enabledTicket"></param>
        /// <param name="collectEnabled"></param>
        /// <param name="enabledPaymentOrder"></param>
        /// <param name="enabledPaymentRequest"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveCollectPaymentMethodType(int id, int methodType, int enabledTicket, int collectEnabled,
            int enabledPaymentOrder, int enabledPaymentRequest);


        /// <summary>
        /// UpdateCollectPaymentMethodType: Se guarda los conceptos de facturación
        /// </summary>
        /// <param name="id"></param>
        /// <param name="methodType"></param>
        /// <param name="enabledTicket"></param>
        /// <param name="collectEnabled"></param>
        /// <param name="enabledPaymentOrder"></param>
        /// <param name="enabledPaymentRequest"></param>
        /// <returns>int</returns>
        [OperationContract]
        int UpdateCollectPaymentMethodType(int id, int methodType, int? enabledTicket, int? collectEnabled, int? enabledPaymentOrder, int? enabledPaymentRequest);


        /// <summary>
        /// DeleteCollectPaymentMethodType: Se elimina los conceptos de facturación
        /// </summary>         
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        void DeleteCollectPaymentMethodType(int id);


        /// <summary>
        /// GetEnablePaymentMethodType
        /// </summary>
        /// <param name="whereClause"></param>
        /// <returns>List<PaymentMethodTypeDTO></returns>
        [OperationContract]
        List<SearchDTO.PaymentMethodTypeDTO> GetEnablePaymentMethodType(bool enabledPaymentRequest, bool enabledPaymentOrder, bool enabledTicket);

        /// <summary>
        /// GetPaymentMethodType
        /// </summary>
        /// <param name="gridSetting"></param>     
        /// <returns>List<PaymentMethodTypeDTO></returns>
        [OperationContract]
        List<SearchDTO.PaymentMethodTypeDTO> GetPaymentMethodType();

        #endregion       

        #region Rejection

        /// <summary>
        /// GetRejections
        /// </summary>
        /// <returns>List<Models.Rejection></returns>
        [OperationContract]
        List<RejectionDTO> GetRejections();

        #endregion

        #region ActionTypeDTO

        ///<summary>
        /// SaveActionType
        /// </summary>
        ///<param name="actionType"> </param>
        ///<returns>ActionTypeDTO</returns>
        [OperationContract]
        ActionTypeDTO SaveActionType(ActionTypeDTO actionType);

        /// <summary>
        /// UpdateActionType
        /// </summary>
        /// <param name="actionType"> </param>
        /// <returns>ActionTypeDTO</returns>
        [OperationContract]
        ActionTypeDTO UpdateActionType(ActionTypeDTO actionType);

        /// <summary>
        /// DeleteActionType
        /// </summary>
        /// <param name="actionType"> </param>
        /// <returns></returns>
        [OperationContract]
        void DeleteActionType(ActionTypeDTO actionType);

        /// <summary>
        /// GetActionTypes
        /// </summary>
        /// <returns>List<ActionTypeDTO></returns>
        [OperationContract]
        List<ActionTypeDTO> GetActionTypes();

        #endregion


        #region CurrencyDifference

        ///<summary>
        /// SaveCurrencyDifference
        /// </summary>
        ///<param name="currencyCode"> </param>
        ///<param name="maxDifference"> </param>
        ///<param name="percentageDifference"> </param>
        ///<returns>int</returns>
        [OperationContract]
        int SaveCurrencyDifference(int currencyCode, decimal maxDifference, decimal percentageDifference);

        ///<summary>
        /// UpdateCurrencyDifference
        /// </summary>
        ///<param name="currencyCode"> </param>
        ///<param name="maxDifference"> </param>
        ///<param name="percentageDifference"> </param>
        ///<returns>int</returns>
        [OperationContract]
        int UpdateCurrencyDifference(int currencyCode, decimal maxDifference, decimal percentageDifference);

        ///<summary>
        /// DeleteCurrencyDifference
        /// </summary>
        ///<param name="currencyCode"> </param>
        ///<returns>bool</returns>
        [OperationContract]
        bool DeleteCurrencyDifference(int currencyCode);

        ///<summary>
        /// GetCurrencyDifferences
        /// </summary>
        ///<returns>List<CurrencyDifferenceDTO></returns>
        [OperationContract]
        List<SearchDTO.CurrencyDifferenceDTO> GetCurrencyDifferences();

        #endregion

        #region AccountingDate

        /// <summary>
        /// GetAccountingDate
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>DateTime</returns>
        [OperationContract]
        DateTime GetAccountingDate(int moduleId);

        #endregion

        #region Range

        /// <summary>
        /// SaveRange
        /// </summary>
        /// <param name="range"></param>
        /// <returns>RangeDTO</returns>
        [OperationContract]
        RangeDTO SaveRange(RangeDTO range);

        /// <summary>
        /// UpdateRange
        /// </summary>
        /// <param name="range"></param>
        /// <returns>RangeDTO</returns>
        [OperationContract]
        RangeDTO UpdateRange(RangeDTO range);

        /// <summary>
        /// GetRange
        /// </summary>
        /// <param name="range"></param>
        /// <returns>RangeDTO</returns>
        [OperationContract]
        RangeDTO GetRange(RangeDTO range);

        /// <summary>
        /// GetRanges
        /// </summary>
        /// <returns>List<Range></returns>
        [OperationContract]
        List<RangeDTO> GetRanges();

        /// <summary>
        /// DeleteRange
        /// </summary>
        /// <param name="range"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteRange(RangeDTO range);

        /// <summary>
        /// DeleteRangeItem
        /// </summary>
        /// <param name="rangeItem"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteRangeItem(RangeItemDTO rangeItem);

        #endregion

        #region CancellationLimit

        /// <summary>
        /// SaveCancellationLimit
        /// </summary>
        /// <param name="cancellationLimit"></param>
        /// <returns>CancellationLimitDTO</returns>
        [OperationContract]
        CancellationLimitDTO SaveCancellationLimit(CancellationLimitDTO cancellationLimit);

        /// <summary>
        /// UpdateCancellationLimit
        /// </summary>
        /// <param name="cancellationLimit"></param>
        /// <returns>CancellationLimitDTO</returns>
        [OperationContract]
        CancellationLimitDTO UpdateCancellationLimit(CancellationLimitDTO cancellationLimit);

        /// <summary>
        /// DeleteCancellationLimit
        /// </summary>
        /// <param name="cancellationLimit"></param>
        /// <returns></returns>
        [OperationContract]
        void DeleteCancellationLimit(CancellationLimitDTO cancellationLimit);

        /// <summary>
        /// GetGetCancellationLimits
        /// </summary>
        /// <returns> List<CancellationLimitDTO></returns>
        [OperationContract]
        List<CancellationLimitDTO> GetCancellationLimits();

        #endregion

        #region Exclusion

        /// <summary>
        /// SaveExclusion
        /// </summary>
        /// <param name="exclusion"></param>
        /// <returns>Exclusion</returns>
        [OperationContract]
        ExclusionDTO SaveExclusion(ExclusionDTO exclusion);

        /// <summary>
        /// DeleteExclusion
        /// </summary>
        /// <param name="exclusion"></param>
        /// <returns></returns>
        [OperationContract]
        void DeleteExclusion(ExclusionDTO exclusion);

        /// <summary>
        /// GetExclusions
        /// </summary>
        /// <param name="exclusionType"></param>
        /// <returns> List<Exclusion></returns>
        [OperationContract]
        List<ExclusionDTO> GetExclusions(int exclusionType);

        #endregion

        #region AccountBank

        #region AccountBankType
        /// <summary>
        /// GetBankAccountTypes
        /// </summary>
        /// <returns>List<BankAccountType></returns>
        [OperationContract]
        List<BankAccountTypeDTO> GetBankAccountTypes();

        #endregion

        #region AccountBankCompany

        /// <summary>
        /// SaveBankAccountCompany
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <returns>BankAccountCompany</returns>
        [OperationContract]
        BankAccountCompanyDTO SaveBankAccountCompany(BankAccountCompanyDTO bankAccountCompany);
        /// <summary>
        /// SaveBankAccountCompany
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <returns>BankAccountCompany</returns>
        [OperationContract]
        BankAccountCompanyDTO UpdateBankAccountCompany(BankAccountCompanyDTO bankAccountCompany);

        /// <summary>
        /// DeleteBankAccountCompany
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <returns></returns>
        [OperationContract]
        void DeleteBankAccountCompany(BankAccountCompanyDTO bankAccountCompany);

        /// <summary>
        /// GetBankAccountCompany
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <returns>BankAccountCompany</returns>
        [OperationContract]
        BankAccountCompanyDTO GetBankAccountCompany(BankAccountCompanyDTO bankAccountCompany);

        /// <summary>
        /// GetBankAccountCompany
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <returns>List<BankAccountCompany></returns>
        [OperationContract]
        List<BankAccountCompanyDTO> GetBankAccountCompanies();

        /// <summary>
        /// GetBankAccountCompany
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <returns>List<BankAccountCompany></returns>
        [OperationContract]
        List<BankAccountCompanyDTO> GetBankAccountCompaniesByCurrencyCode(int currencyCode);


        #endregion

        //#region AccountBankPerson


        /// <summary>
        /// SaveBankAccountPerson
        /// </summary>
        /// <param name="bankAccountPerson"></param>
        /// <returns>BankAccountPerson</returns>
        [OperationContract]
        BankAccountPersonDTO SaveBankAccountPerson(BankAccountPersonDTO bankAccountPerson);

        /// <summary>
        /// SaveBankAccountPerson
        /// </summary>
        /// <param name="bankAccountPerson"></param>
        /// <returns>BankAccountPerson</returns>
        [OperationContract]
        BankAccountPersonDTO UpdateBankAccountPerson(BankAccountPersonDTO bankAccountPerson);

        /// <summary>
        /// DeleteBankAccountPerson
        /// </summary>
        /// <param name="bankAccountPerson"></param>
        /// <returns></returns>
        [OperationContract]
        void DeleteBankAccountPerson(BankAccountPersonDTO bankAccountPerson);

        /// <summary>
        /// GetBankAccountPerson
        /// </summary>
        /// <param name="bankAccountPerson"></param>
        /// <returns>BankAccountPerson</returns>
        [OperationContract]
        BankAccountPersonDTO GetBankAccountPerson(BankAccountPersonDTO bankAccountPerson);

        /// <summary>
        /// GetBankAccountPerson
        /// </summary>
        /// <param name="bankAccountPerson"></param>
        /// <returns>List<BankAccountPerson></returns>
        [OperationContract]
        List<BankAccountPersonDTO> GetBankAccountPersons();

        #endregion

        #region PaymentMethod
        /// <summary>
        /// GetPaymentMethods
        ///     Devuelve los medios de pago
        /// </summary>
        /// <returns>List<PaymentMethod></returns>
        [OperationContract]
        List<PaymentMethodDTO> GetPaymentMethods();
        #endregion

        #region  CreditCardType

        /// <summary>
        /// GetCreditCardType
        /// </summary>
        /// <param name="CreditCardTypeId"></param>
        /// <returns></returns>
        [OperationContract]
        CreditCardTypeDTO GetCreditCardType(int CreditCardTypeId);

        /// <summary>
        /// GetCreditCardTypes
        /// </summary>
        /// <returns>List<CreditCardType></returns>
        [OperationContract]
        List<CreditCardTypeDTO> GetCreditCardTypes();


        #endregion


    }
}
