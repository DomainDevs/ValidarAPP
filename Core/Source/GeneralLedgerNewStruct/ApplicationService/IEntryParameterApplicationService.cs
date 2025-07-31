using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.GeneralLedgerServices
{
    [ServiceContract]
    public interface IEntryParameterApplicationService
    {
        #region ParameterDTO

        /// <summary>
        ///     SaveParameter : Grabar Parametro
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>ParameterDTO</returns>
        [OperationContract]
        ParameterDTO SaveParameter(ParameterDTO parameter);

        /// <summary>
        ///     UpdateParameter : Actualizar Parametro
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>ParameterDTO</returns>
        [OperationContract]
        ParameterDTO UpdateParameter(ParameterDTO parameter);

        /// <summary>
        ///     DeleteParameter : Eliminar Parametro
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [OperationContract]
        void DeleteParameter(ParameterDTO parameter);

        /// <summary>
        ///     GetParameters: Obtener Parametros
        /// </summary>
        /// <param name="module"></param>
        /// <returns>List<Concept/></returns>
        [OperationContract]
        List<ParameterDTO> GetParameters(int moduleDateId);

        #endregion

        #region AccountingRuleDTO

        /// <summary>
        ///     SaveAccountingRule : Grabar regla contable
        /// </summary>
        /// <param name="accountingRule"></param>
        /// <returns>AccountingRuleDTO</returns>
        [OperationContract]
        AccountingRuleDTO SaveAccountingRule(AccountingRuleDTO accountingRule);

        /// <summary>
        ///     UpdateAccountingRule : Actualizar regla contable
        /// </summary>
        /// <param name="accountingRule"></param>
        /// <returns>AccountingRuleDTO</returns>
        [OperationContract]
        AccountingRuleDTO UpdateAccountingRule(AccountingRuleDTO accountingRule);

        /// <summary>
        ///     DeleteAccountingRule 
        /// </summary>
        /// <param name="accountingRule"></param>
        /// <returns></returns>
        [OperationContract]
        void DeleteAccountingRule(AccountingRuleDTO accountingRule);

        /// <summary>
        ///     GetaccountingRules
        /// </summary>
        /// <param name="module"></param>
        /// <returns>List<accountingRule/></returns>
        [OperationContract]
        List<AccountingRuleDTO> GetAccountingRules(int moduleDateId);

        #endregion

        #region Entry

        /// <summary>
        /// ExecuteAccountingRulePackage 
        /// </summary>
        /// <param name="module"></param>        
        /// <param name="parameters"></param>
        /// <returns>List<ResultDTO/></returns>
        [OperationContract]
        List<ResultDTO> ExecuteAccountingRulePackage(int moduleDateId, List<ParameterDTO> parameters, string codeRulePackage= "");

        /// <summary>
        ///     SaveAccountingRulePackage 
        /// </summary>
        /// <param name="accountingRulePackage"></param>
        /// <returns>AccountingRulePackageDTO</returns>
        [OperationContract]
        AccountingRulePackageDTO SaveAccountingRulePackage(AccountingRulePackageDTO accountingRulePackage);

        /// <summary>
        ///     DeleteAccountingRulePackage 
        /// </summary>
        /// <param name="accountingRulePackage"></param>
        /// <returns></returns>
        [OperationContract]
        void DeleteAccountingRulePackage(AccountingRulePackageDTO accountingRulePackage);

        /// <summary>
        ///     GetAccountingRulePackage
        /// </summary>
        /// <param name="module"></param>
        /// <returns>List<AccountingRulePackageDTO/></returns>
        [OperationContract]
        List<AccountingRulePackageDTO> GetAccountingRulePackages(int moduleDateId, string codeRulePackage = "");

        #endregion

        #region ConditionDTO

        /// <summary>
        ///     SaveCondition : Graba Condicion Regla
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>ConditionDTO</returns>
        [OperationContract]
        ConditionDTO SaveCondition(ConditionDTO condition);


        /// <summary>
        ///     UpdateCondition : Actualiza Condicion Regla
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>ConditionDTO</returns>
        [OperationContract]
        ConditionDTO UpdateCondition(ConditionDTO condition);


        /// <summary>
        ///     DeleteCondition : Elimina Condicion Regla
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [OperationContract]
        void DeleteCondition(ConditionDTO condition);

        /// <summary>
        /// GetConditions: Obtener Condiciones de una regla
        /// </summary>
        /// <param name="accountingRule"></param>
        /// <returns> List<ConditionDTO/></returns>
        [OperationContract]
        List<ConditionDTO> GetConditions(AccountingRuleDTO accountingRule);

        #endregion

        #region ResultDTO

        /// <summary>
        /// SaveResult : Graba Resultado de la Condicion
        /// </summary>
        /// <param name="result"></param>
        /// <returns>ResultDTO</returns>
        [OperationContract]
        ResultDTO SaveResult(ResultDTO result);


        /// <summary>
        /// UpdateResult : Actualiza Resultado de la Condicion
        /// </summary>
        /// <param name="result"></param>
        /// <returns>ResultDTO</returns>
        [OperationContract]
        ResultDTO UpdateResult(ResultDTO result);

        /// <summary>
        /// DeleteResult : Elimina Resultado de la Condicion
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        [OperationContract]
        void DeleteResult(ResultDTO result);

        /// <summary>
        /// GetResult : Obtiene Resultado de la Condicion
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>ResultDTO</returns>
        [OperationContract]
        ResultDTO GetResult(ConditionDTO condition);

        #endregion

        #region AccountingAccountMaskDTO

        /// <summary>
        /// SaveAccountingAccountMask : Grabar Formato Cuenta de un resultado especifico
        /// </summary>
        /// <param name="result"></param>
        /// <param name="accountingAccountMask"></param>
        /// <returns>AccountingAccountMaskDTO</returns>
        [OperationContract]
        AccountingAccountMaskDTO SaveAccountingAccountMask(ResultDTO result, AccountingAccountMaskDTO accountingAccountMask);


        /// <summary>
        /// UpdateAccountingAccountMask
        /// </summary>
        /// <param name="result"></param>
        /// <param name="accountingAccountMask"></param>
        /// <returns>AccountingAccountMaskDTO</returns>
        [OperationContract]
        AccountingAccountMaskDTO UpdateAccountingAccountMask(ResultDTO result, AccountingAccountMaskDTO accountingAccountMask);

        /// <summary>
        /// DeleteAccountingAccountMask
        /// </summary>
        /// <param name="accountingAccountMask"></param>
        [OperationContract]
        void DeleteAccountingAccountMask(AccountingAccountMaskDTO accountingAccountMask);

        /// <summary>
        /// GetAccountingAccountMasks
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        [OperationContract]
        List<AccountingAccountMaskDTO> GetAccountingAccountMasks(ResultDTO result);

        #endregion
    }
}
