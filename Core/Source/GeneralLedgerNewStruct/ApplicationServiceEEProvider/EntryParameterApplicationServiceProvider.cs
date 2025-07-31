using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules;
using Sistran.Core.Application.GeneralLedgerServices.Assemblers;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingRules;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider
{
    public class EntryParameterApplicationServiceProvider : IEntryParameterApplicationService
    {
        #region Interfaz

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Interfaz

        #region Public Methods

        #region Parameter

        /// <summary>
        /// SaveParameter : Grabar Parametro
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Parameter</returns>
        public ParameterDTO SaveParameter(ParameterDTO parameter)
        {
            try
            {

                ParameterDAO parameterDAO = new ParameterDAO();
                return DTOAssembler.ToDTO(parameterDAO.SaveParameter(ModelDTOAssembler.ToModel(parameter)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateParameter : Actualizar Parámetro
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Parameter</returns>
        public ParameterDTO UpdateParameter(ParameterDTO parameter)
        {
            try
            {
                ParameterDAO parameterDAO = new ParameterDAO();
                return DTOAssembler.ToDTO(parameterDAO.UpdateParameter(ModelDTOAssembler.ToModel(parameter)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteParameter : Eliminar Parametro
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public void DeleteParameter(ParameterDTO parameter)
        {
            try
            {
                ParameterDAO parameterDAO = new ParameterDAO();
                parameterDAO.DeleteParameter(parameter.Id);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetParameters: Obtener Parametros
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <returns>List<Concept></returns>
        public List<ParameterDTO> GetParameters(int moduleDateId)
        {
            try
            {
                ParameterDAO parameterDAO = new ParameterDAO();
                return (from Parameter parameter in parameterDAO.GetParameters() where parameter.ModuleDateId == moduleDateId select parameter).ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Parameter

        #region AccountingRule

        /// <summary>
        /// SaveAccountingRule
        /// </summary>
        /// <param name="accountingRule"></param>
        /// <returns>AccountingRule</returns>
        public AccountingRuleDTO SaveAccountingRule(AccountingRuleDTO accountingRule)
        {
            try
            {
                // Se graba la cabecera.
                AccountingRuleDAO accountingRuleDAO = new AccountingRuleDAO();
                ConditionDAO conditionDAO = new ConditionDAO(); 
                var newAccountingRule = accountingRuleDAO.SaveAccountingRule(ModelDTOAssembler.ToModel(accountingRule));

                if (accountingRule.Conditions.Count > 0)
                {
                    foreach (var condition in accountingRule.Conditions)
                    {
                        condition.AccountingRule = new AccountingRuleDTO();
                        condition.AccountingRule.Id = newAccountingRule.Id;
                        conditionDAO.SaveCondition(ModelDTOAssembler.ToModel(condition));
                    }
                }

                return DTOAssembler.ToDTO(newAccountingRule);
            }
            catch (BusinessException ex)
            {
                throw new UnhandledException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateAccountingRule
        /// </summary>
        /// <param name="accountingRule"></param>
        /// <returns>AccountingRule</returns>
        public AccountingRuleDTO UpdateAccountingRule(AccountingRuleDTO accountingRule)
        {
            try
            {
                ConditionDAO conditionDAO = new ConditionDAO();

                AccountingRuleDAO accountingRuleDAO = new AccountingRuleDAO();
                var updatedAccountingRule = accountingRuleDAO.UpdateAccountingRule(ModelDTOAssembler.ToModel(accountingRule));

                var conditions = GetConditions(accountingRule);

                if (accountingRule.Conditions.Count > 0)
                {
                    foreach (var condition in accountingRule.Conditions)
                    {
                        condition.AccountingRule = new AccountingRuleDTO();
                        condition.AccountingRule.Id = updatedAccountingRule.Id;

                        // Se comprueba si la condición ya está grabada para actualizara, caso contrario la graba.
                        if (conditions.Any(x => x.Id == condition.Id))
                        {
                            conditionDAO.UpdateCondition(ModelDTOAssembler.ToModel(condition));
                        }
                        else
                        {
                            conditionDAO.SaveCondition(ModelDTOAssembler.ToModel(condition));
                        }
                    }
                }

                return DTOAssembler.ToDTO(updatedAccountingRule);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteAccountingRule
        /// </summary>
        /// <param name="accountingRule"></param>
        public void DeleteAccountingRule(AccountingRuleDTO accountingRule)
        {
            try
            {
                AccountingRuleDAO accountingRuleDAO = new AccountingRuleDAO();
                ConditionDAO conditionDAO = new ConditionDAO();

                //Se eliminan las condiciones relacionadas al concepto.
                ObjectCriteriaBuilder criteriaBuilde = new ObjectCriteriaBuilder();
                criteriaBuilde.PropertyEquals(GENERALLEDGEREN.Condition.Properties.AccountingRuleId, accountingRule.Id);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.Condition), criteriaBuilde.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.Condition conditionEntity in businessCollection.OfType<GENERALLEDGEREN.Condition>())
                    {
                        conditionDAO.DeleteCondition(conditionEntity.ConditionId);
                    }
                }
                accountingRuleDAO.DeleteAccountingRule(accountingRule.Id);
            }
            catch (BusinessException ex)
            {
                throw new UnhandledException(ex.ExceptionMessages.ToString());
            }
        }

        /// <summary>
        /// GetAccountingRules
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <returns>List<AccountingRule></returns>
        public List<AccountingRuleDTO> GetAccountingRules(int moduleDateId)
        {
            try
            {
                List<AccountingRule> accountingRuleDTOs = new List<AccountingRule>();
                   AccountingRuleDAO accountingRuleDAO = new AccountingRuleDAO();
                accountingRuleDTOs = (from AccountingRule accountingRule in accountingRuleDAO.GetAccountingRules() where accountingRule.ModuleDateId == moduleDateId select accountingRule).ToList();
                return DTOAssembler.ToDTOs(accountingRuleDTOs).ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// ExecuteAccountingRulePackage
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <param name="parameters"></param>
        /// <returns>List<Result></returns>
        public List<ResultDTO> ExecuteAccountingRulePackage(int moduleDateId, List<ParameterDTO> parameters, string codeRulePackage= "")
        {
            int order = 1;

            try
            {
                foreach (var parameter in parameters)
                {
                    parameter.Order = order;
                    order++;
                }

                List<ResultDTO> results = new List<ResultDTO>();

                // Leo el asiento.
                List<AccountingRulePackageDTO> accountingRulePackages;
                if(codeRulePackage != null && codeRulePackage !="" && codeRulePackage.Length > 0)
                    accountingRulePackages = GetAccountingRulePackages(moduleDateId, codeRulePackage);
                else
                    accountingRulePackages = GetAccountingRulePackages(moduleDateId);

                if (accountingRulePackages.Count > 0)
                {
                    foreach (AccountingRulePackageDTO accountingRulePackage in accountingRulePackages)
                    {
                        foreach (var accountingRule in accountingRulePackage.AccountingRules)
                        {
                            ResultDTO result = new ResultDTO();

                            // Evalúo las condiciones del concepto
                            result = ExecuteRule(parameters, accountingRule.Id, moduleDateId);

                            if (result.Parameter != null)
                            {
                                // Asigno el valor al resultado
                                var evaluationParameters = GetParameters(moduleDateId);

                                //Se valida primero que exista la parametrización antes de agregar los valores 

                                var parameter = (from item in evaluationParameters where item.Id == result.Parameter.Id select item).ToList();
                                var parameterOrder = parameter.Count > 0 ? parameter[0].Order : -1;

                                var value = (from item in parameters where item.Order == parameterOrder select item).ToList();

                                if (parameter.Count > 0 && value.Count > 0)
                                {
                                    result.Parameter.Value = value[0].Value;
                                    result.Parameter.Description = parameter[0].Description;
                                    // Armo la cuenta contable.
                                    result.AccountingAccount = UnmaskAccount(result, parameters, moduleDateId);
                                }
                                else
                                {
                                    result = new ResultDTO();
                                }
                            }

                            results.Add(result);
                        }
                    }
                }

                //se filtran los resultados que tengan un id distinto a 0
                var filteredResults = (from item in results where item.Id > 0 select item).ToList();

                return filteredResults;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// ExecuteAccountingRulePackage
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <param name="accountingRulePackage"></param>
        /// <param name="parameters"></param>
        /// <returns>List<Result></returns>
        public List<ResultDTO> ExecuteAccountingRulePackageOriginal(int moduleDateId, AccountingRulePackageDTO accountingRulePackage, List<ParameterDTO> parameters)
        {
            List<ResultDTO> results = new List<ResultDTO>();

            // Leo el asiento.
            var accountingRulePackages = GetAccountingRulePackages(moduleDateId);
            var newaccountingRulePackage = new AccountingRulePackageDTO();

            if (accountingRulePackages.Count > 0)
            {
                newaccountingRulePackage = (from item in accountingRulePackages where item.Id == accountingRulePackage.Id select item).ToList()[0];
            }

            if (newaccountingRulePackage.AccountingRules.Count > 0)
            {
                foreach (var concept in newaccountingRulePackage.AccountingRules)
                {
                    ResultDTO result = new ResultDTO();

                    // Evalúo las condiciones del concepto
                    result = ExecuteRule(parameters, concept.Id, moduleDateId);

                    if (result.Parameter != null)
                    {
                        // Asigno el valor al resultado
                        var evaluationParameters = GetParameters(moduleDateId);
                        var parameter = (from item in evaluationParameters where item.Id == result.Parameter.Id select item).ToList()[0];
                        var value = (from item in parameters where item.Order == parameter.Order select item).ToList()[0].Value;
                        result.Parameter.Value = value;
                        result.Parameter.Description = parameter.Description;
                        // Armo la cuenta contable.
                        result.AccountingAccount = UnmaskAccount(result, parameters, moduleDateId);
                    }

                    results.Add(result);
                }
            }

            return results;
        }

        /// <summary>
        /// SaveAccountingRulePackage
        /// </summary>
        /// <param name="accountingRulePackage"></param>
        /// <returns>AccountingRulePackage</returns>
        public AccountingRulePackageDTO SaveAccountingRulePackage(AccountingRulePackageDTO accountingRulePackage)
        {
            try
            {
                AccountingRulePackageDAO accountingRulePackageDAO = new AccountingRulePackageDAO();
                RulePackageRuleDAO rulePackageRuleDAO = new RulePackageRuleDAO(); 
                AccountingRulePackageDTO newAccountingRulePackage;

                if (accountingRulePackage.Id > 0)
                {
                    //se actualiza la cabecera del paquete de reglas
                    newAccountingRulePackage = DTOAssembler.ToDTO(accountingRulePackageDAO.UpdateAccountingRulePackage(ModelDTOAssembler.ToModel(accountingRulePackage)));

                    //se eliminan las relaciones originales.
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(GENERALLEDGEREN.RulePackageRule.Properties.AccountingRulePackageId, accountingRulePackage.Id);

                    BusinessCollection rulePackageRuleEntitiesCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.RulePackageRule), filter.GetPredicate()));

                    if (rulePackageRuleEntitiesCollection.Count > 0)
                    {
                        foreach (GENERALLEDGEREN.RulePackageRule rulePackageRuleEntity in rulePackageRuleEntitiesCollection.OfType<GENERALLEDGEREN.RulePackageRule>())
                        {
                            rulePackageRuleDAO.DeleteRulePackageRule(rulePackageRuleEntity.RulePackageRuleId);
                        }
                    }
                }
                else
                {
                    //se graba la cabecera del paquete de reglas
                    newAccountingRulePackage = DTOAssembler.ToDTO(accountingRulePackageDAO.SaveAccountingRulePackage(ModelDTOAssembler.ToModel(accountingRulePackage)));
                }

                // Grabo los conceptos asociados
                if (accountingRulePackage.AccountingRules.Count > 0)
                {
                    foreach (AccountingRuleDTO accountingRule in accountingRulePackage.AccountingRules)
                    {
                        rulePackageRuleDAO.SaveRulePackageRule(newAccountingRulePackage.Id, accountingRule.Id);
                    }
                }

                return newAccountingRulePackage;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteAccountingRulePackage
        /// </summary>
        /// <param name="accountingRulePackage"></param>
        public void DeleteAccountingRulePackage(AccountingRulePackageDTO accountingRulePackage)
        {
            try
            {
                RulePackageRuleDAO rulePackageRuleDAO = new RulePackageRuleDAO();

                AccountingRulePackageDAO accountingRulePackageDAO = new AccountingRulePackageDAO();

                //Se eliminan los conceptos relacionados.
                ObjectCriteriaBuilder rulePackageFilter = new ObjectCriteriaBuilder();
                rulePackageFilter.PropertyEquals(GENERALLEDGEREN.RulePackageRule.Properties.AccountingRulePackageId, accountingRulePackage.Id);

                BusinessCollection rulePackageRelations = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.RulePackageRule), rulePackageFilter.GetPredicate()));

                if (rulePackageRelations.Count > 0)
                {
                    foreach (GENERALLEDGEREN.RulePackageRule rulePackageRuleEntity in rulePackageRelations.OfType<GENERALLEDGEREN.RulePackageRule>())
                    {
                        rulePackageRuleDAO.DeleteRulePackageRule(rulePackageRuleEntity.RulePackageRuleId);
                    }
                }

                //Se elimina la cabecera
                accountingRulePackageDAO.DeleteAccountingRulePackage(accountingRulePackage.Id);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAccountingRulePackage
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <returns>List<AccountingRulePackage></returns>
        public List<AccountingRulePackageDTO> GetAccountingRulePackages(int moduleDateId, string codeRulePackage = "" )
        {
            List<AccountingRulePackageDTO> accountingRulePackages = new List<AccountingRulePackageDTO>();

            try
            {
                AccountingRulePackageDAO accountingRulePackageDAO = new AccountingRulePackageDAO();
                AccountingRuleDAO accountingRuleDAO = new AccountingRuleDAO();
                ObjectCriteriaBuilder accountingRulePackageFilter = new ObjectCriteriaBuilder();
                accountingRulePackageFilter.PropertyEquals(GENERALLEDGEREN.AccountingRulePackage.Properties.ModuleCode, moduleDateId);

                if (codeRulePackage != "" && codeRulePackage.Length > 0)
                {
                    accountingRulePackageFilter.And();
                    accountingRulePackageFilter.PropertyEquals(GENERALLEDGEREN.AccountingRulePackage.Properties.RulePackageCode, codeRulePackage);
                }

                BusinessCollection accountingRulePackageEntityCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AccountingRulePackage), accountingRulePackageFilter.GetPredicate()));

                if (accountingRulePackageEntityCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.AccountingRulePackage accountingRulePackageEntity in accountingRulePackageEntityCollection.OfType<GENERALLEDGEREN.AccountingRulePackage>())
                    {
                        AccountingRulePackageDTO accountingRulePackage = new AccountingRulePackageDTO();
                        accountingRulePackage.Id = accountingRulePackageEntity.AccountingRulePackageId;
                        accountingRulePackage = DTOAssembler.ToDTO(accountingRulePackageDAO.GetAccountingRulePackage(ModelDTOAssembler.ToModel(accountingRulePackage)));
                        accountingRulePackage.AccountingRules = new List<AccountingRuleDTO>();

                        ObjectCriteriaBuilder rulesRelationFilter = new ObjectCriteriaBuilder();
                        rulesRelationFilter.PropertyEquals(GENERALLEDGEREN.RulePackageRule.Properties.AccountingRulePackageId, accountingRulePackage.Id);

                        BusinessCollection rulesCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.RulePackageRule), rulesRelationFilter.GetPredicate()));

                        if (rulesCollection.Count > 0)
                        {
                            foreach (GENERALLEDGEREN.RulePackageRule rulePackageRelationEntity in rulesCollection.OfType<GENERALLEDGEREN.RulePackageRule>())
                            {
                                AccountingRuleDTO accountingRule = new AccountingRuleDTO();
                                accountingRule.Id = Convert.ToInt32(rulePackageRelationEntity.AccountingRuleId);
                                accountingRule = DTOAssembler.ToDTO(accountingRuleDAO.GetAccountingRule(ModelDTOAssembler.ToModel(accountingRule)));
                                accountingRulePackage.AccountingRules.Add(accountingRule);
                            }
                        }
                        accountingRulePackages.Add(accountingRulePackage);
                    }
                }
                return accountingRulePackages;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion AccountingRule

        #region EvaluateRules

        /// <summary>
        /// ExecuteRule
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="accountingRuleId"></param>
        /// <param name="module"></param>
        /// <returns>Result</returns>
        public ResultDTO ExecuteRule(List<ParameterDTO> parameters, int accountingRuleId, int moduleDateId)
        {
            AccountingRuleDTO accountingRule = new AccountingRuleDTO();
            accountingRule.Id = accountingRuleId;

            ResultDTO result = new ResultDTO();

            var conditions = GetConditions(accountingRule).OrderBy(m => m.Id).ToList();

            if (conditions.Count > 0)
            {
                result = EvaluateRule(conditions[0], (parameters).ToList(),accountingRule, moduleDateId);
            }

            return (result);
        }

        /// <summary>
        /// EvaluateRule
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <param name="accountingRule"></param>
        /// <param name="moduleDateId"></param>
        /// <returns>Result</returns>
        public ResultDTO EvaluateRule(ConditionDTO condition, List<ParameterDTO> parameters, AccountingRuleDTO accountingRule, int moduleDateId)
        {
            var evaluationParameters = GetParameters(moduleDateId);
            var result = new ResultDTO();
            var accountingParameterItemList = new List<ParameterDTO>();
            var parameter = from ParameterDTO item in evaluationParameters where item.Id == condition.Parameter.Id select item;
            if (parameter.Any())
            {
                accountingParameterItemList = (from ParameterDTO item in parameters where item.Order == parameter.ToList()[0].Order select item).ToList();
            }

            if (accountingParameterItemList.Count > 0)
            {
                result = EvaluateConditions(condition, parameters, accountingParameterItemList, accountingRule, moduleDateId);
            }

            return result;
        }

        /// <summary>
        /// ValidateOperation
        /// </summary>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="operationSign"></param>
        /// <returns></returns>
        public bool ValidateOperation(string parameter1, string parameter2, string operationSign)
        {

            bool isValidated = false;

            switch (operationSign)
            {
                case "=":
                    isValidated = (Convert.ToDecimal(parameter1, CultureInfo.InvariantCulture) == Convert.ToDecimal(parameter2, CultureInfo.InvariantCulture));
                    break;
                case ">":
                    isValidated = (Convert.ToDecimal(parameter1, CultureInfo.InvariantCulture) > Convert.ToDecimal(parameter2, CultureInfo.InvariantCulture));
                    break;
                case ">=":
                    isValidated = (Convert.ToDecimal(parameter1, CultureInfo.InvariantCulture) >= Convert.ToDecimal(parameter2, CultureInfo.InvariantCulture));
                    break;
                case "<":
                    isValidated = (Convert.ToDecimal(parameter1, CultureInfo.InvariantCulture) < Convert.ToDecimal(parameter2, CultureInfo.InvariantCulture));
                    break;
                case "<=":
                    isValidated = (Convert.ToDecimal(parameter1, CultureInfo.InvariantCulture) <= Convert.ToDecimal(parameter2, CultureInfo.InvariantCulture));
                    break;
                case "<>":
                    isValidated = EvalBoolParameter(parameter1, parameter2);
                    break;
                case "!=":
                    isValidated = EvalBoolParameter(parameter1, parameter2);
                    break;
            }

            return isValidated;
        }

        #endregion EvaluateRules

        #region Condition

        /// <summary>
        /// SaveCondition : Graba Condición Regla
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>Condition</returns>
        public ConditionDTO SaveCondition(ConditionDTO condition)
        {
            try
            {
                ConditionDAO conditionDAO = new ConditionDAO();
                return DTOAssembler.ToDTO(conditionDAO.SaveCondition(ModelDTOAssembler.ToModel(condition)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateCondition : Actualiza Condición Regla
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>Condition</returns>
        public ConditionDTO UpdateCondition(ConditionDTO condition)
        {
            try
            {
                ConditionDAO conditionDAO = new ConditionDAO();
                return DTOAssembler.ToDTO(conditionDAO.UpdateCondition(ModelDTOAssembler.ToModel(condition)));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteCondition : Elimina Condicion Regla
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public void DeleteCondition(ConditionDTO condition)
        {
            try
            {
                ConditionDAO conditionDAO = new ConditionDAO();
                conditionDAO.DeleteCondition(condition.Id);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetConditions: Obtener Condiciones de un Concepto
        /// </summary>
        /// <param name="accountingRule"></param>
        /// <returns>List<Condition/></returns>
        public List<ConditionDTO> GetConditions(AccountingRuleDTO accountingRule)
        {
            List<ConditionDTO> conditions = new List<ConditionDTO>();

            try
            {
                ConditionDAO conditionDAO = new ConditionDAO();

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.Condition.Properties.AccountingRuleId, accountingRule.Id);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.Condition), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.Condition conditionEntity in businessCollection.OfType<GENERALLEDGEREN.Condition>())
                    {
                        ConditionDTO condition = new ConditionDTO();
                        condition.Id = conditionEntity.ConditionId;
                        condition = DTOAssembler.ToDTO(conditionDAO.GetCondition(ModelDTOAssembler.ToModel(condition)));
                        conditions.Add(condition);
                    }
                }

                return conditions;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Condition

        #region Result

        /// <summary>
        /// SaveResult : Graba Resultado de la Condición
        /// </summary>
        /// <param name="result"></param>
        /// <returns>Result</returns>
        public ResultDTO SaveResult(ResultDTO result)
        {
            try
            {
                ParameterDAO parameterDAO = new ParameterDAO();
                ResultDAO resultDAO = new ResultDAO();
                result = DTOAssembler.ToDTO(resultDAO.SaveResult(ModelDTOAssembler.ToModel(result)));
                result.Parameter = DTOAssembler.ToDTO(parameterDAO.GetParameter(ModelDTOAssembler.ToModel(result.Parameter)));

                return result;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateResult : Actualiza Resultado de la Condición
        /// </summary>
        /// <param name="result"></param>
        /// <returns>Result</returns>
        public ResultDTO UpdateResult(ResultDTO result)
        {
            try
            {
                ParameterDAO parameterDAO = new ParameterDAO();
                ResultDAO resultDAO = new ResultDAO();

                result = DTOAssembler.ToDTO(resultDAO.UpdateResult(ModelDTOAssembler.ToModel(result)));
                result.Parameter = DTOAssembler.ToDTO(parameterDAO.GetParameter(ModelDTOAssembler.ToModel(result.Parameter)));

                return result;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteResult : Elimina Resultado de la Condición
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public void DeleteResult(ResultDTO result)
        {
            try
            {
                ResultDAO resultDAO = new ResultDAO();

                // Si tiene máscaras asociadas, se eliminan las mismas.
                List<AccountingAccountMaskDTO> masks = GetAccountingAccountMasks(result);

                if (masks.Any())
                {
                    foreach (var item in masks)
                    {
                        DeleteAccountingAccountMask(item);
                    }
                }

                // Una vez eliminadas las máscaras, se elimina el registro.
                resultDAO.DeleteResult(result.Id);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetResult : Obtiene Resultado de la Condición
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>Result</returns>
        public ResultDTO GetResult(ConditionDTO condition)
        {
            ParameterDAO parameterDAO = new ParameterDAO();
            ConditionDAO conditionDAO = new ConditionDAO();
            ResultDAO resultDAO = new ResultDAO();

            condition = DTOAssembler.ToDTO(conditionDAO.GetCondition(ModelDTOAssembler.ToModel(condition)));
            ResultDTO result = new ResultDTO();
            result.Id = condition.IdResult;

            try
            {
                if (result.Id > 0)
                {
                    result = DTOAssembler.ToDTO(resultDAO.GetResult(ModelDTOAssembler.ToModel(result)));
                    result.AccountingAccountMasks = GetAccountingAccountMasks(result);
                    result.Parameter = DTOAssembler.ToDTO(parameterDAO.GetParameter(ModelDTOAssembler.ToModel(result.Parameter)));
                }

                return result;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Result

        #region AccountingAccountMask

        /// <summary>
        /// SaveAccountingAccountMask
        /// </summary>
        /// <param name="result"></param>
        /// <param name="accountingAccountMask"></param>
        /// <returns>AccountingAccountMask</returns>
        public AccountingAccountMaskDTO SaveAccountingAccountMask(ResultDTO result, AccountingAccountMaskDTO accountingAccountMask)
        {
            try
            {
                AccountingAccountMaskDAO accountingAccountMaskDAO = new AccountingAccountMaskDAO();
                return DTOAssembler.ToDTO(accountingAccountMaskDAO.SaveAccountingAccountMask(ModelDTOAssembler.ToModel(accountingAccountMask), result.Id));
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// UpdateAccountingAccountMask
        /// </summary>
        /// <param name="result"></param>
        /// <param name="accountingAccountMask"></param>
        /// <returns>AccountingAccountMask</returns>
        public AccountingAccountMaskDTO UpdateAccountingAccountMask(ResultDTO result, AccountingAccountMaskDTO accountingAccountMask)
        {
            try
            {
                AccountingAccountMaskDAO accountingAccountMaskDAO = new AccountingAccountMaskDAO();
                return DTOAssembler.ToDTO(accountingAccountMaskDAO.UpdateAccountingAccountMask(ModelDTOAssembler.ToModel(accountingAccountMask), result.Id));
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// DeleteAccountingAccountMask
        /// </summary>
        /// <param name="accountingAccountMask"></param>
        public void DeleteAccountingAccountMask(AccountingAccountMaskDTO accountingAccountMask)
        {
            try
            {
                AccountingAccountMaskDAO accountingAccountMaskDAO = new AccountingAccountMaskDAO();
                accountingAccountMaskDAO.DeleteAccountingAccountMask(accountingAccountMask.Id);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetAccountingAccountMasks
        /// </summary>
        /// <param name="result"></param>
        /// <returns>List<AccountingAccountMask/></returns>
        public List<AccountingAccountMaskDTO> GetAccountingAccountMasks(ResultDTO result)
        {
            List<AccountingAccountMaskDTO> masks = new List<AccountingAccountMaskDTO>();

            try
            {
                AccountingAccountMaskDAO accountingAccountMaskDAO = new AccountingAccountMaskDAO();
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingAccountMask.Properties.ResultId, result.Id);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AccountingAccountMask), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.AccountingAccountMask accountingAccountMaskEntity in businessCollection.OfType<GENERALLEDGEREN.AccountingAccountMask>())
                    {
                        AccountingAccountMaskDTO accountingAccountMask = new AccountingAccountMaskDTO();
                        accountingAccountMask.Id = accountingAccountMaskEntity.AccountingAccountMaskId;
                        accountingAccountMask = DTOAssembler.ToDTO(accountingAccountMaskDAO.GetAccountingAccountMask(ModelDTOAssembler.ToModel(accountingAccountMask)));
                        masks.Add(accountingAccountMask);
                    }
                }

                return masks;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// UnmaskAccount
        /// </summary>
        /// <param name="result"></param>
        /// <param name="parameters"></param>
        /// <param name="moduleDateId"></param>
        /// <returns>string</returns>
        public string UnmaskAccount(ResultDTO result, List<ParameterDTO> parameters, int moduleDateId)
        {
            string unmaskedAccount = "";

            try
            {
                var evaluationParameters = GetParameters(moduleDateId);
                unmaskedAccount = result.AccountingAccount;

                // Se comprueba que la cuenta tenga máscaras asociadas.
                if (result.AccountingAccountMasks.Any())
                {
                    foreach (var maskItem in result.AccountingAccountMasks)
                    {
                        var parameter = (from item in evaluationParameters where item.Id == maskItem.Parameter.Id select item).ToList()[0];
                        var value = (from item in parameters where item.Order == parameter.Order select item).ToList()[0].Value;
                        if (value.Length < maskItem.Mask.Length)
                        {
                            value = value.PadLeft(maskItem.Mask.Length, '0');
                        }

                        unmaskedAccount = unmaskedAccount.Remove(maskItem.Start - 1, Convert.ToInt32(maskItem.Mask.Length)).Insert(maskItem.Start - 1, value);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return unmaskedAccount;
        }

        #endregion AccountingAccountMask

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// EvalBoolParameter
        /// </summary>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <returns></returns>
        private bool EvalBoolParameter(string parameter1, string parameter2)
        {
            if (Convert.ToDecimal(parameter1, CultureInfo.InvariantCulture) != Convert.ToDecimal(parameter2, CultureInfo.InvariantCulture))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// EvaluateConditions
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <param name="accountingParameterItemList"></param>
        /// <param name="accountingRule"></param>
        /// <param name="moduleDateId"></param>
        /// <returns></returns>
        private ResultDTO EvaluateConditions(ConditionDTO condition, List<ParameterDTO> parameters, List<ParameterDTO> accountingParameterItemList, AccountingRuleDTO accountingRule, int moduleDateId)
        {
            ResultDTO result = new ResultDTO();

            ConditionDTO newCondition = new ConditionDTO();

            if (ValidateOperation(Convert.ToString(accountingParameterItemList.ToList()[0].Value), Convert.ToString(condition.Value, CultureInfo.InvariantCulture), Convert.ToString(condition.Operator)))
            {
                if (condition.IdRightCondition > 0)
                {
                    if (GetConditions(accountingRule).Count > 0)
                    {
                        newCondition = (from ConditionDTO item in GetConditions(accountingRule) where item.Id == condition.IdRightCondition select item).ToList()[0];
                    }

                    result = EvaluateRule(newCondition, parameters, accountingRule, moduleDateId);
                }
                else
                {
                    if (condition.IdResult > 0)
                    {
                        result = GetResult(condition);
                    }
                }
            }
            else
            {
                if ((condition.IdLeftCondition > 0) && (GetConditions(accountingRule).Count > 0))
                {
                    newCondition = (from ConditionDTO item in GetConditions(accountingRule) where item.Id == condition.IdLeftCondition select item).ToList()[0];
                    result = EvaluateRule(newCondition, parameters, accountingRule, moduleDateId);
                }
            }

            return result;
        }

        #endregion Private Methods
    }
}
