using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.BAF;

//Sistran FWK
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.Exceptions;

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.DTOs.Rules;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    public class EntryParameterController : Controller
    {
        #region View

        /// <summary>
        /// Parameters
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Parameters()
        {
            try
            {
                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                ViewBag.AccountLength = Convert.ToInt32(ConfigurationManager.AppSettings["AccountingAccountLength"]);
                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }    
        }

        /// <summary>
        /// AccountingRulePackage
        /// </summary>
        /// <returnsActionResult></returns>
        public ActionResult AccountingRulePackage()
        {
            try
            {
                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                ViewBag.AccountLength = Convert.ToInt32(ConfigurationManager.AppSettings["AccountingAccountLength"]);
                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// Condition
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Condition()
        {
            try
            {
                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                ViewBag.AccountLength = Convert.ToInt32(ConfigurationManager.AppSettings["AccountingAccountLength"]);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }
        
        #endregion View

        #region DummyData

        /// <summary>
        /// DataTypeList
        /// </summary>
        /// <returns>List<DataTypeModel></returns>
        public List<DataTypeModel> DataTypeList()
        {
            List<DataTypeModel> dataTypeModels = new List<DataTypeModel>();
            dataTypeModels.Add(new DataTypeModel() { TypeId = 1, TypeDescription = "Entero" });
            dataTypeModels.Add(new DataTypeModel() { TypeId = 2, TypeDescription = "Decimal" });
            dataTypeModels.Add(new DataTypeModel() { TypeId = 3, TypeDescription = "Boolean" });
            dataTypeModels.Add(new DataTypeModel() { TypeId = 4, TypeDescription = "Texto" });

            return dataTypeModels;
        }

        /// <summary>
        /// GetResultTypes
        /// </summary>
        /// <returns>List<ResultTypeModel></returns>
        public List<ResultTypeModel> GetResultTypes()
        {
            List<ResultTypeModel> resultTypeModels = new List<ResultTypeModel>();
            resultTypeModels.Add(new ResultTypeModel() { ResultTypeId = 1, Description = Global.Condition });
            resultTypeModels.Add(new ResultTypeModel() { ResultTypeId = 2, Description = Global.Result });

            return resultTypeModels;
        }

        /// <summary>
        /// LoadResultTypes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult LoadResultTypes()
        {
            return new UifSelectResult(GetResultTypes());
        }

        #endregion DummyData

        #region Operators

        /// <summary>
        /// GetOperators
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetOperators()
        {
            List<OperatorDTO> operators = DelegateService.glAccountingApplicationService.GetOperators();

            return new UifSelectResult(operators);
        }

        #endregion Operators

        #region Methods

        #region DataType

        /// TODO: Alejandro Villagrán
        /// Pendiente datos por modelo que debe ser incluído en Core.Parametrization
        /// <summary>
        /// GetDataTypeList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetDataTypeList()
        {
            var dataTypes = DataTypeList();
            return new UifSelectResult(dataTypes);
        }

        #endregion DataType

        #region Parameter

        /// <summary>
        /// ParameterList
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>List<Parameter></returns>
        public List<ParameterDTO> ParameterList(int moduleId)
        {
            List<ParameterDTO> parameters = DelegateService.entryParameterApplicationService.GetParameters(moduleId);

            return parameters;
        }

        /// <summary>
        /// GetParametersListByModuleId
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetParametersListByModuleId(int moduleId)
        {
            List<ParameterDTO> parameters = ParameterList(moduleId);
            List<ParameterModel> parameterModels = new List<ParameterModel>();

            foreach (ParameterDTO parameter in parameters)
            {
                parameterModels.Add(new ParameterModel()
                {
                    Id = parameter.Id,
                    ModuleId = Convert.ToInt32(parameter.ModuleDateId),
                    Order = parameter.Order,
                    ParameterDescription = parameter.Description,
                    TypeId = Convert.ToInt32(parameter.DataType),
                    TypeDescription = (from DataTypeModel dataTypeItem in DataTypeList() where dataTypeItem.TypeId == Convert.ToInt32(parameter.DataType) select dataTypeItem.TypeDescription).ToList()[0]
                });
            }

            return Json(parameterModels, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetParameters
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetParameters(int moduleId)
        {
            List<ParameterDTO> parameters = ParameterList(moduleId);
            List<ParameterModel> parameterModels = new List<ParameterModel>();

            foreach (ParameterDTO parameter in parameters)
            {
                parameterModels.Add(new ParameterModel()
                {
                    Id = parameter.Id,
                    ParameterDescription = parameter.Description
                });
            }

            return new UifSelectResult(parameterModels);
        }

        /// <summary>
        /// GetResultParameters
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetResultParameters(int moduleId)
        {
            List<ParameterDTO> parameters = ParameterList(moduleId);
            List<ParameterModel> parameterModels = new List<ParameterModel>();
            List<ParameterDTO> filteredParameters = (from parameter in parameters where parameter.DataType == "2" select parameter).ToList();

            if (filteredParameters.Any())
            {
                foreach (var item in filteredParameters)
                {
                    parameterModels.Add(new ParameterModel()
                    {
                        Id = item.Id,
                        ParameterDescription = item.Description
                    });
                }
            }

            return new UifSelectResult(parameterModels);
        }

        /// <summary>
        /// GetOrderNumber
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetOrderNumber(int moduleId)
        {
            int number = 0;

            List<ParameterDTO> parameters = ParameterList(moduleId);

            if (parameters.Count > 0)
            {
                number = parameters.Max(x => x.Order);
            }

            number = number + 1;

            return Json(number, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveParameter
        /// </summary>
        /// <param name="parameterModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveParameter(ParameterModel parameterModel)
        {
            ParameterDTO parameter = new ParameterDTO();
            parameter.Id = parameterModel.Id;
            parameter.ModuleDateId = parameterModel.ModuleId;
            parameter.Order = parameterModel.Order;
            parameter.Description = parameterModel.ParameterDescription;
            parameter.DataType = Convert.ToString(parameterModel.TypeId);

            try
            {
                if (parameterModel.Id == 0)
                {
                    parameter = DelegateService.entryParameterApplicationService.SaveParameter(parameter);
                }
                else
                {
                    parameter = DelegateService.entryParameterApplicationService.UpdateParameter(parameter);
                }
            }
            catch (Exception)
            {
                parameter.Id = 0;
            }

            return Json(parameter, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteParameter
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteParameter(int parameterId)
        {
            int isDeleted = 0;


            ParameterDTO parameter = new ParameterDTO() { Id = parameterId };

            try
            {
                DelegateService.entryParameterApplicationService.DeleteParameter(parameter);
                isDeleted = 1;
            }
            catch (Exception)
            {
                isDeleted = 0;
            }

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetParameter
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="parameterId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetParameter(int moduleId, int parameterId)
        {
            List<ParameterDTO> parameters = new List<ParameterDTO>();
            ParameterModel parameterModel = new ParameterModel();

            try
            {
                parameters = ParameterList(moduleId);
                if (parameters.Count > 0)
                {
                    ParameterDTO parameter = (from ParameterDTO item in parameters where item.Id == parameterId select item).ToList()[0];

                    parameterModel.Id = parameter.Id;
                    parameterModel.ModuleId = Convert.ToInt32(parameter.ModuleDateId);
                    parameterModel.Order = parameter.Order;
                    parameterModel.ParameterDescription = parameter.Description;
                    parameterModel.TypeId = Convert.ToInt32(parameter.DataType);
                    parameterModel.TypeDescription = parameter.DataType;
                }
            }
            catch (Exception)
            {
                parameterModel.Id = 0;
            }

            return Json(parameterModel, JsonRequestBehavior.AllowGet);
        }

        #endregion Parameter

        #region AccountingRule

        /// <summary>
        /// GetAccountingRulesByModuleId
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>List<AccountingRule></returns>
        public List<AccountingRuleDTO> GetAccountingRulesByModuleId(int moduleId)
        {
            List<AccountingRuleDTO> accountingRules = DelegateService.entryParameterApplicationService.GetAccountingRules(moduleId);

            return accountingRules;
        }

        /// <summary>
        /// SaveConcept
        /// </summary>
        /// <param name="accountingRuleModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveAccountingRule(AccountingRuleModel accountingRuleModel)
        {
            AccountingRuleDTO accountingRule = new AccountingRuleDTO();
            accountingRule.Id = accountingRuleModel.AccountingRuleId;
            accountingRule.ModuleDateId = accountingRuleModel.ModuleId;
            accountingRule.Description = accountingRuleModel.AccountingRuleDescription;
            accountingRule.Observation = accountingRuleModel.AccountingRuleObservations;
            accountingRule.Conditions = new List<ConditionDTO>();

            try
            {
                if (accountingRule.Id == 0)
                {
                    accountingRule = DelegateService.entryParameterApplicationService.SaveAccountingRule(accountingRule);
                }
                else
                {
                    accountingRule = DelegateService.entryParameterApplicationService.UpdateAccountingRule(accountingRule);
                }
            }
            catch (Exception)
            {
                accountingRule.Id = 0;
            }

            return Json(accountingRule, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteConcept
        /// </summary>
        /// <param name="accountingRuleId"></param>
        /// <returns>JsonResult</returns>        
        public JsonResult DeleteAccountingRule(int accountingRuleId)
        {
            int deleted = 0;

            AccountingRuleDTO accountingRule = new AccountingRuleDTO() { Id = accountingRuleId };

            try
            {
                DelegateService.entryParameterApplicationService.DeleteAccountingRule(accountingRule);
                deleted = 1;
            }  
            catch (UnhandledException)
            {
                deleted = 0;
            }

            return Json(deleted, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetConcept
        /// </summary>
        /// <param name="accountingRuleId"></param>
        /// <param name="moduleId"></param>
        /// <returns>JsonResult</returns>
        //todo: public JsonResult GetConcept(int conceptId, int moduleId)
        public JsonResult GetAccountingRule(int accountingRuleId, int moduleId)
        {
            List<AccountingRuleDTO> accountingRules = GetAccountingRulesByModuleId(moduleId);
            AccountingRuleModel accountingRuleModel = new AccountingRuleModel();

            List<AccountingRuleDTO> query = (from AccountingRuleDTO item in accountingRules where item.Id == accountingRuleId select item).ToList();

            if (query.Count > 0)
            {
                AccountingRuleDTO accountingRule = query[0];

                accountingRuleModel.AccountingRuleId = accountingRule.Id;
                accountingRuleModel.AccountingRuleDescription = accountingRule.Description;
                accountingRuleModel.AccountingRuleObservations = accountingRule.Observation;
            }

            return Json(accountingRuleModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAccountingRulesListByModuleId
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>JsonResult</returns>
        /// todo: public JsonResult GetConceptsListByModuleId(int moduleId)
        public JsonResult GetAccountingRulesListByModuleId(int moduleId)
        {
            List<AccountingRuleDTO> accountingRules = GetAccountingRulesByModuleId(moduleId);
            List<AccountingRuleModel> accountingRuleModels = new List<AccountingRuleModel>();

            foreach (AccountingRuleDTO accountingRule in accountingRules)
            {
                accountingRuleModels.Add(new AccountingRuleModel()
                {
                    AccountingRuleId = accountingRule.Id,
                    ModuleId = Convert.ToInt32(accountingRule.ModuleDateId),
                    AccountingRuleDescription = accountingRule.Description,
                    AccountingRuleObservations = accountingRule.Observation
                });
            }

            return Json(accountingRuleModels, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LoadConceptDetails
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>JsonResult</returns>
        /// todo: public JsonResult LoadConceptDetails(int moduleId)
        public JsonResult LoadAccountingRuleDetails(int moduleId)
        {
            List<object> accountingRuleDetails = new List<object>();
            List<AccountingRuleDTO> accountingRules = GetAccountingRulesByModuleId(moduleId);

            if (accountingRules.Count > 0)
            {
                foreach (AccountingRuleDTO accountingRule in accountingRules)
                {
                    accountingRule.Conditions = new List<ConditionDTO>();
                    accountingRule.Conditions = DelegateService.entryParameterApplicationService.GetConditions(accountingRule);

                    if (accountingRule.Conditions.Count > 0)
                    {
                        foreach (var condition in accountingRule.Conditions)
                        {
                            ConditionModel conditionModel = GetConditionModel(condition.Id, accountingRule.Id, moduleId);
                            ConditionModel rightConditionModel = new ConditionModel();
                            ConditionModel leftConditionModel = new ConditionModel();
                            ResultModel resultModel = new ResultModel();

                            string rightResultDescription = " - ";
                            string leftResultDescription = " - ";
                            string resultDescription = " - ";

                            if (condition.IdRightCondition > 0)
                            {
                                rightConditionModel = GetConditionModel(condition.IdRightCondition, accountingRule.Id, moduleId);
                                rightResultDescription = "ID - " + condition.IdRightCondition + ": " + Global.Yes.ToUpper() + " " + rightConditionModel.ParameterDescription + " " + rightConditionModel.OperatorDescription + " " + rightConditionModel.Value;
                            }
                            if (condition.IdLeftCondition > 0)
                            {
                                leftConditionModel = GetConditionModel(condition.IdLeftCondition, accountingRule.Id, moduleId);
                                leftResultDescription = "ID - " + condition.IdLeftCondition + ": " + Global.Yes.ToUpper() + " " + leftConditionModel.ParameterDescription + " " + leftConditionModel.OperatorDescription + " " + leftConditionModel.Value;
                            }
                            if (condition.IdResult > 0)
                            {
                                if (condition.IdRightCondition > 0)
                                {
                                    resultModel = GetResultModel(condition);
                                    leftResultDescription = Global.Nature + ": " + resultModel.AccountingNatureDescription + " - " + Global.Account + ": " + resultModel.AccountingAccountNumber + " - " + Global.Value + ": " + resultModel.Value;
                                }
                                if (condition.IdLeftCondition > 0)
                                {
                                    resultModel = GetResultModel(condition);
                                    rightResultDescription = Global.Nature + ": " + resultModel.AccountingNatureDescription + " - " + Global.Account + ": " + resultModel.AccountingAccountNumber + " - " + Global.Value + ": " + resultModel.Value;
                                }
                                if ((condition.IdRightCondition == 0) && (condition.IdLeftCondition == 0))
                                {
                                    resultModel = GetResultModel(condition);
                                    rightResultDescription = Global.Nature + ": " + resultModel.AccountingNatureDescription + " - " + Global.Account + ": " + resultModel.AccountingAccountNumber + " - " + Global.Value + ": " + resultModel.Value;
                                }
                            }

                            accountingRuleDetails.Add(new
                            {
                                AccountingRuleDescription = accountingRule.Description,
                                ConditionDescription = "ID - " + conditionModel.ConditionId + ": " + Global.Yes.ToUpper() + " " + conditionModel.ParameterDescription + " " + conditionModel.OperatorDescription + " " + conditionModel.Value,
                                RightResultDescription = rightResultDescription,
                                LeftResultDescription = leftResultDescription,
                                ResultDescription = resultDescription
                            });
                        }
                    }
                }
            }
            return new UifTableResult(accountingRuleDetails);
        }

        #endregion AccountingRule

        #region AccountingRulePackage

        /// <summary>
        /// GetAccountingRulePackagesByModuleId
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>List<Entry></returns>
        public List<AccountingRulePackageDTO> GetAccountingRulePackagesByModuleId(int moduleId)
        {
            List<AccountingRulePackageDTO> accountingRulePackages = DelegateService.entryParameterApplicationService.GetAccountingRulePackages(moduleId);

            return accountingRulePackages;
        }

        /// <summary>
        /// SaveAccountingRulePackage
        /// </summary>
        /// <param name="accountingRulePackageModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveAccountingRulePackage(AccountingRulePackageModel accountingRulePackageModel)
        {
            AccountingRulePackageDTO accountingRulePackage = new AccountingRulePackageDTO();

            try
            {
                accountingRulePackage.Id = accountingRulePackageModel.AccountingRulePackageId;
                accountingRulePackage.ModuleDateId = accountingRulePackageModel.ModuleId;
                accountingRulePackage.Description = accountingRulePackageModel.AccountingRulePackageDescription;
                if (accountingRulePackageModel.RulePackageId > 0)
                    accountingRulePackage.RulePackageId = accountingRulePackageModel.RulePackageId;

                accountingRulePackage.AccountingRules = new List<AccountingRuleDTO>();

                if (accountingRulePackageModel.AccountingRules != null)
                {
                    foreach (var item in accountingRulePackageModel.AccountingRules)
                    {
                        AccountingRuleDTO accountingRule = new AccountingRuleDTO();
                        accountingRule.Id = item.AccountingRuleId;
                        accountingRulePackage.AccountingRules.Add(accountingRule);
                    }
                }

                accountingRulePackage = DelegateService.entryParameterApplicationService.SaveAccountingRulePackage(accountingRulePackage);
            }
            catch (Exception)
            {
                accountingRulePackage.Id = 0;
            }

            return Json(accountingRulePackage, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteEntry
        /// </summary>
        /// <param name="accountingRulePackageId"></param>
        /// <returns>JsonResult</returns>
        /// todo: public JsonResult DeleteEntry(int entryId)
        public JsonResult DeleteAccountingRulePackage(int accountingRulePackageId)
        {
            int isDeleted = 0;

            AccountingRulePackageDTO accountingRulePackage = new AccountingRulePackageDTO() { Id = accountingRulePackageId };

            try
            {
               DelegateService.entryParameterApplicationService.DeleteAccountingRulePackage(accountingRulePackage);
                isDeleted = 1;
            }
            catch (Exception)
            {
                isDeleted = 0;
            }

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAccountingRulePackageListByModuleId
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingRulePackageListByModuleId(int moduleId)
        {
            List<AccountingRulePackageDTO> accountingRulePackages = GetAccountingRulePackagesByModuleId(moduleId);
            List<AccountingRulePackageModel> entryParameterModels = new List<AccountingRulePackageModel>();

            foreach (AccountingRulePackageDTO accountingRulePackage in accountingRulePackages)
            {
                entryParameterModels.Add(new AccountingRulePackageModel()
                {
                    AccountingRulePackageId = accountingRulePackage.Id,
                    AccountingRulePackageDescription = accountingRulePackage.Description,
                    ModuleId = Convert.ToInt32(accountingRulePackage.ModuleDateId),
                    AccountingRules = null
                });
            }

            return Json(entryParameterModels, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LoadAccountingRuleModels
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>List<ConceptModel></returns>
        public List<AccountingRuleModel> LoadAccountingRuleModels(int moduleId)
        {
            List<AccountingRuleDTO> accountingRules = GetAccountingRulesByModuleId(moduleId);
            List<AccountingRuleModel> accountingRuleModels = new List<AccountingRuleModel>();

            if (accountingRules.Count > 0)
            {
                foreach (AccountingRuleDTO accountingRule in accountingRules)
                {
                    accountingRuleModels.Add(new AccountingRuleModel()
                    {
                        AccountingRuleId = accountingRule.Id,
                        ModuleId = Convert.ToInt32(accountingRule.ModuleDateId),
                        AccountingRuleDescription = accountingRule.Description,
                        AccountingRuleObservations = accountingRule.Observation
                    });
                }
            }

            return accountingRuleModels;
        }

        /// <summary>
        /// LoadConceptsListByModuleId
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult LoadAccountingRuleModelListByModuleId(int moduleId)
        {
            List<AccountingRuleModel> accountingRuleModels = LoadAccountingRuleModels(moduleId);
            return new UifSelectResult(accountingRuleModels);
        }

        /// <summary>
        /// LoadConceptList
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingRuleListByModule(int moduleId)
        {
            List<AccountingRuleModel> accountingRuleModels = LoadAccountingRuleModels(moduleId);
            return Json(accountingRuleModels, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetEntry
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="accountingRulePackageId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingRulePackage(int moduleId, int accountingRulePackageId)
        {
            List<AccountingRulePackageDTO> accountingRulePackages = new List<AccountingRulePackageDTO>();
            AccountingRulePackageModel accountingRulePackageModel = new AccountingRulePackageModel();

            try
            {
                accountingRulePackages = GetAccountingRulePackagesByModuleId(moduleId);

                if (accountingRulePackages.Count > 0)
                {
                    AccountingRulePackageDTO accountingRulePackage = (from AccountingRulePackageDTO item in accountingRulePackages where item.Id == accountingRulePackageId select item).ToList()[0];

                    accountingRulePackageModel.AccountingRulePackageId = accountingRulePackage.Id;
                    accountingRulePackageModel.ModuleId = Convert.ToInt32(accountingRulePackage.ModuleDateId);
                    accountingRulePackageModel.AccountingRulePackageDescription = accountingRulePackage.Description;
                    accountingRulePackageModel.RulePackageId = Convert.ToInt32(accountingRulePackage.RulePackageId);
                    accountingRulePackageModel.AccountingRules = new List<AccountingRuleModel>();

                    if (accountingRulePackage.AccountingRules.Count > 0)
                    {
                        foreach (AccountingRuleDTO accountingRule in accountingRulePackage.AccountingRules)
                        {
                            AccountingRuleModel accountingRuleModel = new AccountingRuleModel();
                            accountingRuleModel.AccountingRuleId = accountingRule.Id;
                            accountingRulePackageModel.AccountingRules.Add(accountingRuleModel);
                        }
                    }
                }
            }
            catch (Exception)
            {
                accountingRulePackageModel.AccountingRulePackageId = 0;
            }

            return Json(accountingRulePackageModel, JsonRequestBehavior.AllowGet);
        }

        #endregion AccountingRulePackage

        #region Conditions

        /// <summary>
        /// LoadConditionsByConcept
        /// </summary>
        /// <param name="accountingRuleId"></param>
        /// <param name="moduleId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult LoadConditionsByAccountingRule(int accountingRuleId, int moduleId)
        {
            AccountingRuleDTO accountingRule = new AccountingRuleDTO() { Id = accountingRuleId };

            List<ConditionDTO> conditions = DelegateService.entryParameterApplicationService.GetConditions(accountingRule);
            List<ConditionModel> conditionModels = new List<ConditionModel>();

            if (conditions.Count > 0)
            {
                foreach (var item in conditions)
                {
                    ConditionModel conditionModel = GetConditionModel(item.Id, accountingRuleId, moduleId);
                    conditionModels.Add(conditionModel);
                }
            }

            return Json(conditionModels, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveCondition
        /// </summary>
        /// <param name="conditionModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveCondition(ConditionModel conditionModel)
        {
            ConditionDTO newCondition = new ConditionDTO();

            try
            {
                //Si existen condiciones, se obtiene el id de la última condición para obtener el id de resultado y que este sea puesto en la nueva condicion.
                ConditionDTO lastCondition = new ConditionDTO();
                lastCondition.Id = GetLastConditionIdByAccountingRuleId(conditionModel.AccountingRuleId);

                AccountingRuleDTO accountingRule = new AccountingRuleDTO();
                accountingRule.Id = conditionModel.AccountingRuleId;

                List<ConditionDTO> conditions = DelegateService.entryParameterApplicationService.GetConditions(accountingRule);

                newCondition.Id = conditionModel.ConditionId;
                newCondition.AccountingRule = new AccountingRuleDTO();
                newCondition.AccountingRule.Id = conditionModel.AccountingRuleId;
                newCondition.Parameter = new ParameterDTO();
                newCondition.Parameter.Id = conditionModel.ParameterId;
                newCondition.Operator = (from item in DelegateService.glAccountingApplicationService.GetOperators() where item.Id == conditionModel.OperatorId select item.OperationSign).ToList()[0];
                newCondition.Value = Convert.ToDecimal(conditionModel.Value);
                newCondition.IdLeftCondition = Convert.ToInt32(conditionModel.LeftResultId);
                newCondition.IdRightCondition = Convert.ToInt32(conditionModel.RightResultId);
                newCondition.IdResult = Convert.ToInt32(conditionModel.ResultId);

                if (conditions.Count > 0)
                {
                    lastCondition = (from item in conditions where item.Id == lastCondition.Id select item).ToList()[0];

                    if (conditionModel.ConditionId > 0)
                    {
                        newCondition = DelegateService.entryParameterApplicationService.UpdateCondition(newCondition);
                    }
                    else
                    {
                        newCondition.IdResult = lastCondition.IdResult;
                        newCondition = DelegateService.entryParameterApplicationService.SaveCondition(newCondition);

                        lastCondition.IdResult = 0;
                        lastCondition.IdRightCondition = newCondition.Id;
                        DelegateService.entryParameterApplicationService.UpdateCondition(lastCondition);
                    }
                }
                else
                {
                    newCondition = DelegateService.entryParameterApplicationService.SaveCondition(newCondition);
                }
            }
            catch
            {
                newCondition.Id = 0;
            }

            return Json(newCondition, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteCondition
        /// </summary>
        /// <param name="conditionModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteCondition(ConditionModel conditionModel)
        {
            int isDeleted = 0;

            ConditionDTO condition = new ConditionDTO();
            condition.Id = conditionModel.ConditionId;
            
            try
            {
                AccountingRuleDTO accountingRule = new AccountingRuleDTO();
                accountingRule.Id = conditionModel.AccountingRuleId;

                List<ConditionDTO> conditions = DelegateService.entryParameterApplicationService.GetConditions(accountingRule);
                condition = (from conditionItem in conditions where conditionItem.Id == conditionModel.ConditionId select conditionItem).ToList()[0];

                if (conditions.Count > 0)
                {
                    //Si solo tiene una condicion
                    if (conditions.Count == 1)
                    {
                        //Si posee resultado, se elimina el resultado.
                        if (condition.IdResult > 0)
                        {
                            ResultDTO result = new ResultDTO();
                            result.Id = condition.IdResult;
                            DelegateService.entryParameterApplicationService.DeleteResult(result);
                        }

                        //se procede a borrar la condición.
                        DelegateService.entryParameterApplicationService.DeleteCondition(condition);
                    }
                    //Si tiene más de una condición.
                    else
                    {
                        //se comprueba que sea la primera condicion de la lista.
                        var conditionIndex = conditions.FindIndex(conditionItem => conditionItem.Id == conditionModel.ConditionId);

                        if (conditionIndex == 0) //inicio de las condiciones, si se elimna no afecta al resto de condiciones.
                        {
                            DelegateService.entryParameterApplicationService.DeleteCondition(condition);
                        }
                        if (conditionIndex > 0)
                        {
                            //si es la última condicion
                            if (conditionModel.ConditionId == GetLastConditionIdByAccountingRuleId(conditionModel.AccountingRuleId))
                            {
                                //se obtiene la condicion que llama a la condicion a ser eliminada y se le actualizan los campos de condicion derecha y resultado.
                                ConditionDTO previousCondition = (from conditionItem in conditions where conditionItem.IdRightCondition == conditionModel.ConditionId select conditionItem).ToList()[0];
                                previousCondition.IdRightCondition = 0;
                                previousCondition.IdResult = condition.IdResult;

                                DelegateService.entryParameterApplicationService.UpdateCondition(previousCondition);

                                //elimino el registro
                                DelegateService.entryParameterApplicationService.DeleteCondition(condition);
                            }
                            //si es una condicion intermedia
                            else
                            {
                                //se obtiene la condicion previa
                                ConditionDTO previousCondition = (from conditionItem in conditions where conditionItem.IdRightCondition == conditionModel.ConditionId select conditionItem).ToList()[0];

                                //se obtiene la condicion siguiente.
                                ConditionDTO nextCondition = (from conditionItem in conditions where conditionItem.Id == condition.IdRightCondition select conditionItem).ToList()[0];

                                //se actualizan los campos de la condición previa y la condición siguiente.
                                previousCondition.IdRightCondition = nextCondition.Id;
                                DelegateService.entryParameterApplicationService.UpdateCondition(previousCondition);

                                //elimino el registro
                                DelegateService.entryParameterApplicationService.DeleteCondition(condition);
                            }
                        }
                    }
                }
                
                isDeleted = 1;
            }
            catch (Exception)
            {
                isDeleted = 0;
            }

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetCondition
        /// </summary>
        /// <param name="conditionId"></param>
        /// <param name="accountingRuleId"></param>
        /// <param name="moduleId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCondition(int conditionId, int accountingRuleId, int moduleId)
        {
            AccountingRuleDTO accountingRule = new AccountingRuleDTO() { Id = accountingRuleId };

            ConditionModel conditionModel = new ConditionModel();

            List<ConditionDTO> conditions = DelegateService.entryParameterApplicationService.GetConditions(accountingRule);

            if (conditions.Count > 0)
            {
                var query = (from item in conditions where item.Id == conditionId select item).ToList();

                if (query.Count > 0)
                {
                    conditionModel.ConditionId = query[0].Id;
                    conditionModel.ParameterId = query[0].Parameter.Id;
                    conditionModel.ParameterDescription = (from parameterItem in ParameterList(moduleId) where parameterItem.Id == query[0].Parameter.Id select parameterItem.Description).ToList()[0];
                    conditionModel.OperatorId = (from item in DelegateService.glAccountingApplicationService.GetOperators() where item.OperationSign == query[0].Operator select item.Id).ToList()[0];
                    conditionModel.OperatorDescription = query[0].Operator;
                    conditionModel.RightResultId = query[0].IdRightCondition;
                    conditionModel.LeftResultId = query[0].IdLeftCondition;
                    conditionModel.ResultId = query[0].IdResult;
                    conditionModel.Value = Convert.ToString(query[0].Value);
                }
            }

            return Json(conditionModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetLastConditionId
        /// </summary>
        /// <param name="accountingRuleId"></param>
        /// <returns></returns>
        public JsonResult GetLastConditionId(int accountingRuleId)
        {
            return Json(GetLastConditionIdByAccountingRuleId(accountingRuleId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetLastConditionIdByAccountingRuleId
        /// </summary>
        /// <param name="accountingRuleId"></param>
        /// <returns></returns>
        public int GetLastConditionIdByAccountingRuleId(int accountingRuleId)
        {
            int lastConditionId = 0;

            try
            {
                AccountingRuleDTO accountingRule = new AccountingRuleDTO() { Id = accountingRuleId };

                List<ConditionDTO> conditions = DelegateService.entryParameterApplicationService.GetConditions(accountingRule);

                if (conditions.Count > 0)
                {
                    lastConditionId = conditions.Max(x => x.Id);
                }
            }
            catch (Exception)
            {
                lastConditionId = 0;
            }

            return lastConditionId;
        }

        /// <summary>
        /// GetRemainingConditions
        /// </summary>
        /// <param name="conditionId"></param>
        /// <param name="accountingRuleId"></param>
        /// <param name="moduleId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetRemainingConditions(int conditionId, int accountingRuleId, int moduleId)
        {
            List<ConditionModel> remainingConditions = new List<ConditionModel>();

            AccountingRuleDTO accountingRule = new AccountingRuleDTO();
            accountingRule.Id = accountingRuleId;

            List<ConditionDTO> conditions = DelegateService.entryParameterApplicationService.GetConditions(accountingRule);
            ConditionDTO selectedCondition = new ConditionDTO();

            List<ConditionDTO> query = (from item in conditions where item.Id == conditionId select item).ToList();

            if (query.Count > 0)
            {
                selectedCondition = query[0];
            }

            List<int> excludedConditions = new List<int>();
            excludedConditions.Add(selectedCondition.Id);
            excludedConditions.Add(selectedCondition.IdRightCondition);
            excludedConditions.Add(selectedCondition.IdLeftCondition);

            var newQuery = (from item in conditions where !excludedConditions.Contains(item.Id) select item).ToList();

            if (newQuery.Count > 0)
            {
                foreach (var item in newQuery)
                {
                    ConditionModel conditionModel = new ConditionModel();
                    conditionModel.ConditionId = item.Id;
                    conditionModel.ParameterId = item.Parameter.Id;
                    conditionModel.ParameterDescription = (from parameterItem in ParameterList(moduleId) where parameterItem.Id == item.Parameter.Id select parameterItem.Description).ToList()[0];
                    conditionModel.OperatorDescription = item.Operator;
                    conditionModel.Value = Convert.ToString(item.Value);
                    remainingConditions.Add(conditionModel);
                }
            }

            return Json(remainingConditions, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetConditionModel
        /// </summary>
        /// <param name="conditionId"></param>
        /// <param name="accountingRuleId"></param>
        /// <param name="moduleId"></param>
        /// <returns>ConditionModel</returns>
        public ConditionModel GetConditionModel(int conditionId, int accountingRuleId, int moduleId)
        {
            ConditionModel conditionModel = new ConditionModel();

            AccountingRuleDTO accountingRule = new AccountingRuleDTO() { Id = accountingRuleId };

            List<ConditionDTO> conditions = DelegateService.entryParameterApplicationService.GetConditions(accountingRule);

            if (conditions.Count > 0)
            {
                List<ConditionDTO> query = (from item in conditions where item.Id == conditionId select item).ToList();

                if (query.Count > 0)
                {
                    conditionModel.ConditionId = query[0].Id;
                    conditionModel.ParameterId = query[0].Parameter.Id;
                    conditionModel.ParameterDescription = (from parameterItem in ParameterList(moduleId) where parameterItem.Id == query[0].Parameter.Id select parameterItem.Description).ToList()[0];
                    conditionModel.OperatorDescription = query[0].Operator;
                    conditionModel.RightResultId = query[0].IdRightCondition;
                    conditionModel.LeftResultId = query[0].IdLeftCondition;
                    conditionModel.ResultId = query[0].IdResult;
                    conditionModel.Value = Convert.ToString(query[0].Value);
                }
            }

            return conditionModel;
        }


        /// <summary>
        /// UpdateRightResult
        /// </summary>
        /// <param name="conditionModel"></param>
        /// <param name="moduleId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateRightResult(ConditionModel conditionModel, int moduleId)
        {
            // La actualización de una condición viene ligada a la actualización del concepto, cuyo modelo contiene una lista de condiciones.
            AccountingRuleDTO accountingRule = new AccountingRuleDTO();
            accountingRule.Id = conditionModel.AccountingRuleId;
            accountingRule = (from item in DelegateService.entryParameterApplicationService.GetAccountingRules(moduleId) where item.Id == conditionModel.AccountingRuleId select item).ToList()[0];
            accountingRule.Conditions = new List<ConditionDTO>();

            List<ConditionDTO> conditions = (from item in DelegateService.entryParameterApplicationService.GetConditions(accountingRule) where item.Id == conditionModel.ConditionId select item).ToList();
            ConditionDTO newCondition = conditions[0];
            newCondition.IdRightCondition = conditionModel.RightResultId;
            accountingRule.Conditions.Add(newCondition);

            try
            {
                accountingRule = DelegateService.entryParameterApplicationService.UpdateAccountingRule(accountingRule);
            }
            catch (Exception)
            {
                accountingRule.Id = 0;
            }

            return Json(accountingRule, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// UpdateLeftResult
        /// </summary>
        /// <param name="conditionModel"></param>
        /// <param name="moduleId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateLeftResult(ConditionModel conditionModel, int moduleId)
        {
            // La actualización de una condición viene ligada a la actualización del concepto, cuyo modelo contiene una lista de condiciones.
            AccountingRuleDTO accountingRule = new AccountingRuleDTO();
            accountingRule.Id = conditionModel.AccountingRuleId;
            accountingRule = (from item in DelegateService.entryParameterApplicationService.GetAccountingRules(moduleId) where item.Id == conditionModel.AccountingRuleId select item).ToList()[0];
            accountingRule.Conditions = new List<ConditionDTO>();

            List<ConditionDTO> conditions = (from item in DelegateService.entryParameterApplicationService.GetConditions(accountingRule) where item.Id == conditionModel.ConditionId select item).ToList();
            ConditionDTO newCondition = conditions[0];
            newCondition.IdLeftCondition = conditionModel.LeftResultId;
            accountingRule.Conditions.Add(newCondition);

            try
            {
                accountingRule = DelegateService.entryParameterApplicationService.UpdateAccountingRule(accountingRule);
            }
            catch (Exception)
            {
                accountingRule.Id = 0;
            }

            return Json(accountingRule, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteResult
        /// </summary>
        /// <param name="conditionModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteResult(ConditionModel conditionModel)
        {
            ResultDTO result = new ResultDTO() { Id = conditionModel.ResultId };

            AccountingRuleDTO accountingRule = new AccountingRuleDTO() { Id = conditionModel.AccountingRuleId };

            List<ConditionDTO> conditions = (from item in DelegateService.entryParameterApplicationService.GetConditions(accountingRule) where item.Id == conditionModel.ConditionId select item).ToList();
            ConditionDTO newCondition = conditions[0];
            newCondition.IdResult = 0;

            try
            {
                //se actualiza la condición a la que está relacionada el resultado.
                DelegateService.entryParameterApplicationService.UpdateCondition(newCondition);

                //se borra el resultado
                DelegateService.entryParameterApplicationService.DeleteResult(result);                
            }
            catch (Exception)
            {
                accountingRule.Id = 0;
            }

            return Json(accountingRule, JsonRequestBehavior.AllowGet);
        }

        #endregion Conditions

        #region Result

        /// <summary>
        /// SaveResult
        /// </summary>
        /// <param name="resultModel"></param>
        /// <param name="conditionModel"></param>
        /// <param name="moduleId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveResult(ResultModel resultModel, ConditionModel conditionModel, int moduleId)
        {
            ResultDTO result = new ResultDTO();
            ResultModel newResult = new ResultModel();

            result.Id = resultModel.ResultId;
            result.AccountingNature = resultModel.AccountingNatureId;
            result.AccountingAccount = resultModel.AccountingAccountNumber;
            result.Parameter = new ParameterDTO();
            result.Parameter.Id = resultModel.ParameterId;

            try
            {
                // Cuando se añade un resultado se debe actualizar la condición.
                if (resultModel.ResultId == 0)
                {
                    result = DelegateService.entryParameterApplicationService.SaveResult(result);

                    //se actualiza la condicion
                    AccountingRuleDTO accountingRule = new AccountingRuleDTO();
                    accountingRule.Id = conditionModel.AccountingRuleId;
                    accountingRule = (from item in DelegateService.entryParameterApplicationService.GetAccountingRules(moduleId) where item.Id == conditionModel.AccountingRuleId select item).ToList()[0];
                    accountingRule.Conditions = new List<ConditionDTO>();

                    List<ConditionDTO> conditions = (from item in DelegateService.entryParameterApplicationService.GetConditions(accountingRule) where item.Id == conditionModel.ConditionId select item).ToList();
                    ConditionDTO newCondition = conditions[0];
                    newCondition.IdRightCondition = 0;
                    newCondition.IdLeftCondition = 0;
                    newCondition.IdResult = result.Id;
                    accountingRule.Conditions.Add(newCondition);

                    DelegateService.entryParameterApplicationService.UpdateAccountingRule(accountingRule);
                }
                else
                {
                    result = DelegateService.entryParameterApplicationService.UpdateResult(result);
                }

                newResult.ResultId = result.Id;
                newResult.AccountingNatureId = Convert.ToInt32(result.AccountingNature);
                newResult.AccountingNatureDescription = (int)result.AccountingNature == 1 ? Global.Credits : Global.Debits;
                newResult.AccountingAccountNumber = result.AccountingAccount;
                newResult.ParameterId = result.Parameter.Id;
                newResult.ParameterDescription = result.Parameter.Description;
            }
            catch (Exception)
            {
                newResult.ResultId = 0;
            }

            return Json(newResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetResult
        /// </summary>
        /// <param name="conditionModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetResult(ConditionModel conditionModel)
        {
            ConditionDTO condition = new ConditionDTO();
            condition.Id = conditionModel.ConditionId;
            condition.IdResult = conditionModel.ResultId;

            ResultModel resultModel = GetResultModel(condition);

            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetResultModel
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>ResultModel</returns>
        public ResultModel GetResultModel(ConditionDTO condition)
        {
            ResultDTO result = DelegateService.entryParameterApplicationService.GetResult(condition);

            ResultModel resultModel = new ResultModel();
            if (result.Id > 0)
            {
                resultModel.ResultId = result.Id;
                resultModel.AccountingNatureId = Convert.ToInt32(result.AccountingNature);
                resultModel.AccountingNatureDescription = (int)result.AccountingNature == 1 ? Global.Credits : Global.Debits;
                resultModel.AccountingAccountNumber = AssembleMaskedAccountingAccount(condition, result.AccountingAccount);//result.AccountingAccount;
                resultModel.ParameterId = result.Parameter.Id;
                resultModel.ParameterDescription = result.Parameter.Description;
            }

            return resultModel;
        }

        #endregion Result

        #region AccountingAccountMask

        /// <summary>
        /// SaveAccountingAccountMask
        /// </summary>
        /// <param name="accountingAccountMaskModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveAccountingAccountMask(AccountingAccountMaskModel accountingAccountMaskModel)
        {
            AccountingAccountMaskDTO accountingAccountMask = new AccountingAccountMaskDTO();

            try
            {
                accountingAccountMask.Id = accountingAccountMaskModel.Id;
                accountingAccountMask.Mask = accountingAccountMaskModel.Mask;
                accountingAccountMask.Parameter = new ParameterDTO();
                accountingAccountMask.Parameter.Id = accountingAccountMaskModel.ParameterId;
                accountingAccountMask.Start = accountingAccountMaskModel.Position;

                ResultDTO result = new ResultDTO();
                result.Id = accountingAccountMaskModel.ResultId;

                if (accountingAccountMaskModel.Id == 0)
                {
                    accountingAccountMask = DelegateService.entryParameterApplicationService.SaveAccountingAccountMask(result, accountingAccountMask);
                }
                else
                {
                    accountingAccountMask = DelegateService.entryParameterApplicationService.UpdateAccountingAccountMask(result, accountingAccountMask);
                }
            }
            catch
            {
                accountingAccountMask = new AccountingAccountMaskDTO();
            }

            return Json(accountingAccountMask, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteAccountingAccountMask
        /// </summary>
        /// <param name="accountingAccountMaskModel"></param>
        /// <param name="conditionId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteAccountingAccountMask(AccountingAccountMaskModel accountingAccountMaskModel, int conditionId)
        {
            bool isSuccessfully;
            string replaceMask = "";
            ResultDTO newResult = new ResultDTO();

            try
            {
                // Se obtiene el resultado
                ConditionDTO condition = new ConditionDTO();
                condition.Id = conditionId;
                condition.IdResult = accountingAccountMaskModel.ResultId;

                ResultDTO result = DelegateService.entryParameterApplicationService.GetResult(condition);

                // Se obtiene la máscara
                List<AccountingAccountMaskDTO> accountingAccountMasks = DelegateService.entryParameterApplicationService.GetAccountingAccountMasks(result);
                var mask = (from item in accountingAccountMasks where item.Id == accountingAccountMaskModel.Id select item).ToList()[0];

                replaceMask = replaceMask.PadRight(mask.Mask.Length, '0');

                AccountingAccountMaskDTO accountingAccountMask = new AccountingAccountMaskDTO();
                accountingAccountMask.Id = accountingAccountMaskModel.Id;

                DelegateService.entryParameterApplicationService.DeleteAccountingAccountMask(accountingAccountMask);

                newResult = DelegateService.entryParameterApplicationService.GetResult(condition);

                // Se actualiza el número de cuenta.
                newResult.AccountingAccount = result.AccountingAccount.Remove(mask.Start - 1, mask.Mask.Length).Insert(mask.Start - 1, replaceMask);

                newResult = DelegateService.entryParameterApplicationService.UpdateResult(newResult);

                isSuccessfully = true;
            }
            catch
            {
                isSuccessfully = false;
                newResult = new ResultDTO();
            }

            return new UifJsonResult(isSuccessfully, newResult);
        }

        /// <summary>
        /// GetAccountingAccountMasks
        /// </summary>
        /// <param name="accountingAccountMaskModel"></param>
        /// <param name="moduleId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingAccountMasks(AccountingAccountMaskModel accountingAccountMaskModel, int moduleId)
        {
            List<AccountingAccountMaskModel> accountingAccountMaskModels = new List<AccountingAccountMaskModel>();

            try
            {
                ResultDTO result = new ResultDTO();
                result.Id = accountingAccountMaskModel.ResultId;

                List<AccountingAccountMaskDTO> accountingAccountMasks = DelegateService.entryParameterApplicationService.GetAccountingAccountMasks(result);

                if (accountingAccountMasks.Count > 0)
                {
                    foreach (AccountingAccountMaskDTO item in accountingAccountMasks)
                    {
                        AccountingAccountMaskModel accountingAccountMaskItem = new AccountingAccountMaskModel();
                        accountingAccountMaskItem.Id = item.Id;
                        accountingAccountMaskItem.Mask = item.Mask;
                        accountingAccountMaskItem.ParameterId = item.Parameter.Id;
                        accountingAccountMaskItem.ParameterDescription = (from i in DelegateService.entryParameterApplicationService.GetParameters(moduleId) where i.Id == item.Parameter.Id select i).ToList()[0].Description;
                        accountingAccountMaskItem.Position = item.Start;

                        accountingAccountMaskModels.Add(accountingAccountMaskItem);
                    }
                }

            }
            catch
            {
                accountingAccountMaskModels = new List<AccountingAccountMaskModel>();
            }

            return Json(accountingAccountMaskModels, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAccountingAccountMask
        /// </summary>
        /// <param name="accountingAccountMaskModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingAccountMask(AccountingAccountMaskModel accountingAccountMaskModel)
        {
            AccountingAccountMaskModel accountingAccountMaskModels = new AccountingAccountMaskModel();

            try
            {
                ResultDTO result = new ResultDTO();
                result.Id = accountingAccountMaskModel.ResultId;

                List<AccountingAccountMaskDTO> accountingAccountMasks = new List<AccountingAccountMaskDTO>();
                accountingAccountMasks = DelegateService.entryParameterApplicationService.GetAccountingAccountMasks(result);

                if (accountingAccountMasks.Count > 0)
                {
                    var accountingAccountMaskFiltered = (from i in accountingAccountMasks where i.Id == accountingAccountMaskModel.Id select i).ToList()[0];

                    accountingAccountMaskModels.Id = accountingAccountMaskFiltered.Id;
                    accountingAccountMaskModels.Mask = accountingAccountMaskFiltered.Mask;
                    accountingAccountMaskModels.ParameterId = accountingAccountMaskFiltered.Parameter.Id;
                    accountingAccountMaskModels.Position = accountingAccountMaskFiltered.Start;
                }
            }
            catch (Exception exception)
            {
                var message = exception.Message;
                accountingAccountMaskModels = new AccountingAccountMaskModel();
            }

            return Json(accountingAccountMaskModels, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidateMaskUniqueParameter
        /// Valida si el parámetro es único.
        /// </summary>
        /// <param name="accountingAccountMaskModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateMaskUniqueParameter(AccountingAccountMaskModel accountingAccountMaskModel)
        {
            bool isValidated = false;

            try
            {
                // Se obtiene el listado de máscaras.
                ResultDTO result = new ResultDTO() { Id = accountingAccountMaskModel.ResultId };

                List<AccountingAccountMaskDTO> accountingAccountMasks = DelegateService.entryParameterApplicationService.GetAccountingAccountMasks(result);

                if (accountingAccountMasks.Count > 0)
                {
                    var accountingAccountMasksFiltered = (from item in accountingAccountMasks where item.Parameter.Id == accountingAccountMaskModel.ParameterId select item).ToList();
                    isValidated = !accountingAccountMasksFiltered.Any();
                }
                else
                {
                    isValidated = true;
                }
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }

            return Json(isValidated, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidateMaskAccountLength
        /// Valida que la máscara tenga la posición y la longitud dentro de la longitud de la cuenta contable.
        /// </summary>
        /// <param name="accountingAccountMaskModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateMaskAccountLength(AccountingAccountMaskModel accountingAccountMaskModel)
        {
            bool isValidated;

            try
            {
                int accountLength = Convert.ToInt32(ConfigurationManager.AppSettings["AccountingAccountLength"]) + 1;

                // Si la posición inicial es mayor a la longitud de la cuenta
                isValidated = accountingAccountMaskModel.Position <= accountLength;
                // Si la posición inicial más la longitud de la máscara es mayor a la longitud de la cuenta.
                if (isValidated)
                {
                    int starPosition = accountingAccountMaskModel.Position;
                    int maskLength = Convert.ToInt32(accountingAccountMaskModel.Mask.Length);

                    isValidated = starPosition + maskLength <= accountLength;
                }
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }

            return Json(isValidated, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidateUniqueMask
        /// Valida que la máscara sea única
        /// </summary>
        /// <param name="accountingAccountMaskModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateUniqueMask(AccountingAccountMaskModel accountingAccountMaskModel)
        {
            bool isValidated = false;

            try
            {
                // Se obtiene el listado de máscaras.
                ResultDTO result = new ResultDTO();
                result.Id = accountingAccountMaskModel.ResultId;

                List<AccountingAccountMaskDTO> accountingAccountMasks = new List<AccountingAccountMaskDTO>();
                accountingAccountMasks = DelegateService.entryParameterApplicationService.GetAccountingAccountMasks(result);

                if (accountingAccountMasks.Count > 0)
                {
                    var accountingAccountMasksFiltered = (from item in accountingAccountMasks where item.Mask == accountingAccountMaskModel.Mask select item).ToList();
                    isValidated = !accountingAccountMasksFiltered.Any();
                }
                else
                {
                    isValidated = true;
                }
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }

            return Json(isValidated, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidateMaskPosition
        /// Valida que la máscara y su longitud no ocupen posición ya usada por otra máscara previamente ingresada.
        /// </summary>
        /// <param name="accountingAccountMaskModel"></param>
        /// <returns></returns>
        public JsonResult ValidateMaskPosition(AccountingAccountMaskModel accountingAccountMaskModel)
        {
            bool isValidated = false;
            int starPosition = accountingAccountMaskModel.Position;
            int endPosition = starPosition + Convert.ToInt32(accountingAccountMaskModel.Mask.Length) - 1;

            try
            {
                // Se obtiene el listado de máscaras.
                ResultDTO result = new ResultDTO() { Id = accountingAccountMaskModel.ResultId };

                List<AccountingAccountMaskDTO> accountingAccountMasks = DelegateService.entryParameterApplicationService.GetAccountingAccountMasks(result);

                if (accountingAccountMasks.Count > 0)
                {
                    foreach (var item in accountingAccountMasks)
                    {
                        int itemStarPosition = item.Start;
                        int itemEndPosition = itemStarPosition + Convert.ToInt32(item.Mask.Length) - 1;

                        if (starPosition >= itemStarPosition && starPosition <= itemEndPosition)
                        {
                            isValidated = false;
                            break;
                        }
                        if (endPosition >= itemStarPosition && endPosition <= itemEndPosition)
                        {
                            isValidated = false;
                            break;
                        }
                        if (starPosition <= itemStarPosition && endPosition >= itemEndPosition)
                        {
                            isValidated = false;
                            break;
                        }
                        isValidated = true;
                    }
                }
                else
                {
                    isValidated = true;
                }
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }

            return Json(isValidated, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// AssembleMaskedAccountingAccount
        /// Devuelve la cuenta contable con las máscaras parametrizadas.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="accountingAccountNumber"></param>
        /// <returns>string</returns>
        public string AssembleMaskedAccountingAccount(ConditionDTO condition, string accountingAccountNumber)
        {
            string assembledAccount = "";

            try
            {
                assembledAccount = accountingAccountNumber;

                if (condition.IdResult > 0)
                {
                    ResultDTO result = new ResultDTO();
                    result = DelegateService.entryParameterApplicationService.GetResult(condition);

                    if (result.Id > 0)
                    {
                        List<AccountingAccountMaskDTO> accountingAccountMasks = DelegateService.entryParameterApplicationService.GetAccountingAccountMasks(result);

                        if (assembledAccount.Length < Convert.ToInt32(ConfigurationManager.AppSettings["AccountingAccountLength"]))
                            assembledAccount = assembledAccount.PadRight(Convert.ToInt32(ConfigurationManager.AppSettings["AccountingAccountLength"]), '0');

                        if (accountingAccountMasks.Count > 0)
                        {
                            foreach (var item in accountingAccountMasks)
                            {
                                assembledAccount = assembledAccount.Remove(item.Start - 1, Convert.ToInt32(item.Mask.Length)).Insert(item.Start - 1, item.Mask);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }

            return assembledAccount;
        }

        /// <summary>
        /// GetAssembledMaskedAccountingAccount
        /// </summary>
        /// <param name="conditionModel"></param>
        /// <param name="accountingAccountNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAssembledMaskedAccountingAccount(ConditionModel conditionModel, string accountingAccountNumber)
        {
            string account = "";

            ConditionDTO condition = new ConditionDTO();
            condition.Id = conditionModel.ConditionId;
            condition.IdResult = conditionModel.ResultId;

            try
            {
                account = AssembleMaskedAccountingAccount(condition, accountingAccountNumber);
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }

            return Json(account, JsonRequestBehavior.AllowGet);
        }

        #endregion AccountingAccountMask

        #endregion Methods
    }
}