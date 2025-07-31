//System
using Sistran.Core.Application.ReinsuranceServices.DTOs;
using Sistran.Core.Application.ReinsuranceServices.Enums;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.App_GlobalResources;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Controllers
{
    /// <summary>
    /// Parametrización reaseguros
    /// </summary>
    [Authorize]
    [HandleError]
    public class ParameterController : Controller
    {
        #region ParamContracts

        #region Contract

        /// <summary>
        /// MainContract
        /// Invoca a la vista de contratos MainContract.cshtml
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainContract()
        {
            try
            {
                //valida que el servicio este arriba
                List<ModuleDateDTO> moduleDates = new List<ModuleDateDTO>();
                moduleDates = DelegateService.reinsuranceService.GetModuleDates();
                ViewBag.ContractYearNow = DateTime.Now.Year;
                return View("~/Areas/Reinsurance/Views/Reinsurance/Parameter/MainContract.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// ValidateMinYearContract
        /// </summary>
        /// <param name="year"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateMinYearContract(int year)
        {
            try
            {
                int minYearLimit = DateTime.Now.Year - 1;

                if (year < minYearLimit)
                {
                    return Json(minYearLimit, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorValidateMinYearContract);
            }
        }

        /// <summary>
        /// ContractDialog
        /// Invoca a la vista de inserción / actualización de contratos ContractDialog.cshtml
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="contractYear"></param>
        /// <param name="contractTypeId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult AddContract(int contractId, int contractYear, int contractTypeId)
        {
            try
            {
                ContractDTO contractDTO = new ContractDTO();
                contractDTO = DelegateService.reinsuranceService.AddContract(contractId, contractYear, contractTypeId);
                return PartialView("../Reinsurance/Parameter/ContractDialog", contractDTO);
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetContractById
        /// Obtiene un registro de la tabla REINS.CONTRACT dado el id del contrato
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetContractById(int contractId)
        {
            try
            {
                ContractDTO contractDTO = new ContractDTO();
                contractDTO = DelegateService.reinsuranceService.GetContractById(contractId);
                return new UifJsonResult(true, contractDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetContractById);
            }
        }

        /// <summary>
        /// GetContractByYearAndContractTypeId
        /// Obtiene los registros de la tabla REINS.CONTRACT dado el año y tipo de contrato
        /// </summary>
        /// <param name="year"></param>
        /// <param name="contractTypeId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetContractsByYearAndContractTypeId(int year, int contractTypeId)
        {
            try
            {
                List<ContractDTO> contractDTOs = new List<ContractDTO>();
                contractDTOs = DelegateService.reinsuranceService.GetContractsByYearAndContractTypeId(year, contractTypeId);
                return new UifJsonResult(true, contractDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetContractsByYearAndContractTypeId);
            }
        }

        /// <summary>
        /// GetContractTypeEnabled
        /// Obtiene los registros de la tabla REINS.CONTRACT_TYPE
        /// que estén habilitados
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetContractTypeEnabled()
        {
            try
            {
                List<ContractTypeDTO> contractTypeDTOs = new List<ContractTypeDTO>();
                contractTypeDTOs = DelegateService.reinsuranceService.GetContractTypeEnabled();
                return new UifSelectResult(contractTypeDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetContractTypeEnabled);
            }
        }

        /// <summary>
        /// GetContractFuncionalityId
        /// </summary>
        /// <param name="contractTypeId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetContractFuncionalityId(int contractTypeId)
        {
            try
            {
                List<ContractTypeDTO> contractTypeDTOs = new List<ContractTypeDTO>();
                contractTypeDTOs = DelegateService.reinsuranceService.GetContractFuncionalityId(contractTypeId);
                return new UifSelectResult(contractTypeDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetContractFuncionalityId);
            }
        }

        /// <summary>
        /// GetEnabledContracts
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetEnabledContracts()
        {
            try
            {
                List<ContractDTO> contracts = new List<ContractDTO>();
                List<ContractDTO> contractFind = new List<ContractDTO>();
                contracts = DelegateService.reinsuranceService.GetEnabledContracts();
                contractFind = contracts.Where(ct => ct.Year >= Convert.ToInt32(TempData["contractYear"])).ToList();
                return new UifSelectResult(contractFind);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetEnabledContracts);
            }
        }

        /// <summary>
        /// GetCurrentPeriodContracts
        /// </summary>
        /// <param name="year"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetCurrentPeriodContracts(int year)
        {
            try
            {
                List<ContractDTO> contractDTOs = new List<ContractDTO>();
                contractDTOs = DelegateService.reinsuranceService.GetCurrentPeriodContracts(year);
                return new UifSelectResult(contractDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetCurrentPeriodContracts);
            }
        }

        /// <summary>
        /// GetEpiTypes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetEpiTypes()
        {
            try
            {
                List<EPITypeDTO> ePITypeDTOs = new List<EPITypeDTO>();
                ePITypeDTOs = DelegateService.reinsuranceService.GetEPITypes();
                return new UifSelectResult(ePITypeDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetEpiTypes);
            }
        }

        /// <summary>
        /// GetAffectationTypes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAffectationTypes()
        {
            try
            {
                List<AffectationTypeDTO> affectationTypeDTOs = new List<AffectationTypeDTO>();
                affectationTypeDTOs = DelegateService.reinsuranceService.GetAffectationTypes();
                return new UifSelectResult(affectationTypeDTOs);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, Language.ErrorGetAffectationTypes);
            }
        }

        /// <summary>
        /// GetResettlementTypes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetResettlementTypes()
        {
            try
            {
                List<ResettlementTypeDTO> resettlementTypeDTOs = new List<ResettlementTypeDTO>();
                resettlementTypeDTOs = DelegateService.reinsuranceService.GetResettlementTypes();
                return new UifSelectResult(resettlementTypeDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetResettlementTypes);
            }
        }

        /// <summary>
        /// GetContractYear
        /// Obtiene los años para ser llenados en el combo de ejercicio para la búsqueda de contratos
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetContractYear()
        {
            try
            {
                List<SelectDTO> selectDTOs = new List<SelectDTO>();
                selectDTOs = DelegateService.reinsuranceService.GetContractYear();
                return new UifSelectResult(selectDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetContractYear);
            }
        }

        /// <summary>
        /// GetContractYearContractLine
        /// Obtiene los años para ser llenados en el combo de ejercicio para la búsqueda de contratos
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetContractYearContractLine()
        {
            try
            {
                List<SelectDTO> selectDTOs = new List<SelectDTO>();
                selectDTOs = DelegateService.reinsuranceService.GetContractYear();
                return new UifSelectResult(selectDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetContractYearContractLine);
            }
        }

        /// <summary>
        /// GetCurrency
        /// Obtiene los registros de la tabla COMM.CURRENCY
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetCurrency()
        {
            try
            {
                List<CurrencyDTO> currencies = new List<CurrencyDTO>();
                currencies = DelegateService.reinsuranceService.GetCurrencies();
                return new UifSelectResult(currencies);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetCurrency);
            }
        }

        /// <summary>
        /// SaveContract
        /// Graba o actuualiza un registro en la tabla REINS.CONTRACT
        /// </summary>
        /// <param name="contractDTO"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveContract(ContractDTO contract)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.ResultSaveContract(contract);
                return new UifJsonResult(result, contract.ContractId);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, Language.ErrorSaveContract);
            }
        }

        /// <summary>
        /// DeleteContract
        /// Borra un registro de la tabla REINS.CONTRACT dado el id del contrato
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteContract(int contractId)
        {
            try
            {
                if (ValidateBeforeDeleteContract(contractId) == 0)
                {
                    DelegateService.reinsuranceService.DeleteContract(contractId);
                }
                else
                {
                    return new UifJsonResult(false, null);
                }

                return new UifJsonResult(true, null);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorDeleteContract);
            }
        }

        /// <summary>
        /// ValidateBeforeDeleteContract
        /// Verifica si tiene registros en la tabla REINS.CONTRACT_LEVEL, si tiene no permite
        /// borrar el registro de la tabla REINS.CONTRACT
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns>int</returns>
        private int ValidateBeforeDeleteContract(int contractId)
        {
            int result = 0;
            result = DelegateService.reinsuranceService.ValidateBeforeDeleteContract(contractId);
            return result;
        }

        /// <summary>
        /// ValidateContractIssueAllocation
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public ActionResult ValidateContractIssueAllocation(int contractId)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.ValidateContractIssueAllocation(contractId);
                return new UifJsonResult(true, result);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="smallDescription"></param>
        /// <param name="contractTypeId"></param>
        /// <returns></returns>
        public ActionResult ValidateDuplicateContract(ContractDTO contractDTO)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.ValidateDuplicateContract(contractDTO);
                return new UifJsonResult(true, result);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Global.BusinessExceptionMsj);
            }
        }

        public ActionResult CopyContract(int contractId, string smallDescription, int year, string description)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.CopyContract(contractId, smallDescription, year, description);
                return new UifJsonResult(true, result);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult ValidateCompleteContract(int contractId)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.ValidateCompleteContract(contractId);
                return new UifJsonResult(true, result);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        #endregion Contract

        #region Level

        /// <summary>
        /// ContractLevelDialog
        /// Invoca a la vista de inserción / actualización de nivel de contrato ContractLevelDialog.cshtml
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="contractLevelId"></param>
        /// <param name="contractTypeId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult AddContractLevel(int contractId, int contractLevelId, int contractTypeId)
        {
            // DANC PENDIENTE
            try
            {
                //ViewBag.ContractType = contractTypeId;
                LevelDTO levelDTO = new LevelDTO();
                levelDTO = DelegateService.reinsuranceService.AddContractLevel(contractId, contractLevelId, contractTypeId);
                return PartialView("../Reinsurance/Parameter/ContractLevelDialog", levelDTO);

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetContractLevelByContractId
        /// Obtiene los registros de la tabla REINS.CONTRACT_LEVEL dado el id de contrato
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetContractLevelByContractId(string contractId)
        {
            try
            {
                List<LevelDTO> levelDTOs = new List<LevelDTO>();
                levelDTOs = DelegateService.reinsuranceService.GetContractLevelByContractId(contractId);
                return new UifJsonResult(true, levelDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetContractLevelByContractId);
            }
        }

        /// <summary>
        /// SaveContractLevel
        /// Graba o actualiza un registro en la tabla REINS.CONTRACT_LEVEL
        /// </summary>
        /// <param name="contractLevelDTO"></param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult SaveContractLevel(LevelDTO contractLevelDTO)
        {
            // DANC PENDIENTE
            try
            {
                int result = 0;
                result = DelegateService.reinsuranceService.SaveContractLevel(contractLevelDTO);
                return new UifJsonResult(true, result);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Global.BusinessExceptionMsj);
            }
            catch (UnhandledException)
            {
                return new UifJsonResult(false, Global.UnhandledExceptionMsj);
            }
        }

        /// <summary>
        /// DeleteContractLevel
        /// Borra un registro de la tabla REINS.CONTRACT_LEVEL dado el id de contrato y el id de nivel de contrato
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="contractLevelId"></param>
        /// <param name="level"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteContractLevel(int contractId, int contractLevelId, int level)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.DeleteContractLevel(contractId, contractLevelId, level);
                return new UifJsonResult(result, 0);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Global.UnhandledExceptionMsj);
            }

        }

        /// <summary>
        /// GetLevelNumberByContractId
        /// Obtiene el número de nivel campo LEVEL_NUMBER de la tabla REINS.CONTRACT_LEVEL dado el id de contrato
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns>int</returns>
        public int GetLevelNumberByContractId(int contractId)
        {
            int result = 0;
            result = DelegateService.reinsuranceService.GetLevelNumberByContractId(contractId);
            return result;
        }

        /// <summary>
        /// ValidateBeforeDeleteContractLevel
        /// Verifica si tiene registros en la tabla REINS.CONTRACT_LEVEL_COMPANY, si tiene no permite borrar el registro de la tabla 
        /// REINS.CONTRACT_LEVEL
        /// </summary>
        /// <param name="contractLevelId"></param>
        /// <returns> List<int> </returns>
        private List<int> ValidateBeforeDeleteContractLevel(int contractLevelId)
        {
            List<int> result = new List<int>();
            result = DelegateService.reinsuranceService.ValidateBeforeDeleteContractLevel(contractLevelId);
            return result;
        }

        /// <summary>
        /// GetCalculationTypes
        /// Obtiene de enum para cargar Tipos de calculo en combo 
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetCalculationTypes()
        {
            try
            {
                CurrentCulture();
                List<SelectDTO> calculationTypes = new List<SelectDTO>();
                var id = 0;
                var name = "";

                foreach (CalculationTypes value in Enum.GetValues(typeof(CalculationTypes)))
                {
                    id = (int)value;
                    if (id == 1)
                    {
                        name = Global.Percentage;
                    }
                    else if (id == 2)
                    {
                        name = Global.Mileage;
                    }
                    else if (id == 3)
                    {
                        name = Global.Amount;
                    }
                    else if (id == 4)
                    {
                        name = Global.Millon;
                    }

                    calculationTypes.Add(new SelectDTO
                    {
                        Id = (int)value,
                        Description = name
                    });
                }
                return new UifSelectResult(calculationTypes);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetCalculationTypes);
            }
        }

        /// <summary>
        /// GetApplyOnTypes
        /// Obtiene de enum para cargar Tipos ApplyOnTypes en combo 
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetApplyOnTypes()
        {
            try
            {
                CurrentCulture();
                List<SelectDTO> applyOnTypes = new List<SelectDTO>();
                int id = 0;
                string name = "";

                foreach (ApplyOnTypes value in Enum.GetValues(typeof(ApplyOnTypes)))
                {
                    id = (int)value;
                    if (id == 1)
                    {
                        name = Global.RetainedRiskSum;
                    }
                    else if (id == 2)
                    {
                        name = Global.ExcessRetention;
                    }

                    applyOnTypes.Add(new SelectDTO
                    {
                        Id = (int)value,
                        Description = name
                    });
                }

                return new UifSelectResult(applyOnTypes);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetApplyOnTypes);
            }
        }

        /// <summary>
        /// GetPremiumTypes
        /// Obtiene de enum para cargar Tipos Prima en combo 
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPremiumTypes()
        {
            try
            {
                CurrentCulture();
                List<SelectDTO> premiumTypes = new List<SelectDTO>();
                int id = 0;
                string name = "";

                foreach (PremiumTypes value in Enum.GetValues(typeof(PremiumTypes)))
                {
                    id = (int)value;
                    if (id == 1)
                    {
                        name = Global.MinimumPremium;
                    }
                    else if (id == 2)
                    {
                        name = Global.DepositPremium;
                    }

                    else if (id == 3)
                    {
                        name = Global.MinimumAndDepositPremium;
                    }

                    premiumTypes.Add(new SelectDTO
                    {
                        Id = (int)value,
                        Description = name
                    });
                }

                return new UifSelectResult(premiumTypes);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetPremiumTypes);
            }
        }

        #region LevelPayment

        /// <summary>
        /// GetLevelPaymentsByLevelId
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetLevelPaymentsByLevelId(int levelId)
        {
            try
            {
                List<LevelPaymentDTO> levelPaymentDTOs = new List<LevelPaymentDTO>();
                levelPaymentDTOs = DelegateService.reinsuranceService.GetLevelPaymentsByLevelIdByLevelId(levelId);
                return Json(levelPaymentDTOs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetLevelPaymentsByLevelId);
            }
        }

        /// <summary>
        /// GetNextLevelNumberByLevelId
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetNextLevelNumberByLevelId(int levelId)
        {
            try
            {
                int result = 0;
                result = DelegateService.reinsuranceService.GetNextLevelNumberByLevelId(levelId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetNextLevelNumberByLevelId);
            }
        }

        /// <summary>
        /// SaveLevelPayment
        /// </summary>
        /// <param name="levelPaymentModal"></param>
        /// <returns>ActionResult</returns>
        public UifJsonResult SaveLevelPayment(LevelPaymentDTO levelPaymentDTO)
        {
            try
            {
                int levelPaymentId = Convert.ToInt32(levelPaymentDTO.Id);

                if (levelPaymentId.Equals(0))
                {
                    DelegateService.reinsuranceService.SaveLevelPayment(levelPaymentDTO);
                    return new UifJsonResult(true, true);
                }
                else
                {
                    DelegateService.reinsuranceService.UpdateLevelPayment(levelPaymentDTO);
                    return new UifJsonResult(true, true);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);

            }

        }

        /// <summary>
        /// DeleteLevelPayment
        /// </summary>
        /// <param name="levelPaymentId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteLevelPayment(int levelPaymentId)
        {
            bool isSucessfully = false;
            int deleted = 0;
            string message = "";

            try
            {
                DelegateService.reinsuranceService.DeleteLevelPayment(levelPaymentId);
                isSucessfully = true;
                deleted = 1;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isSucessfully = false;
                deleted = 0;
            }

            return Json(new { success = isSucessfully, result = deleted, msg = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region LevelRestore

        /// <summary>
        /// GetLevelRestoresByLevelId
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetLevelRestoresByLevelId(int levelId)
        {
            try
            {
                List<LevelRestoreDTO> levelRestores = new List<LevelRestoreDTO>();
                levelRestores = DelegateService.reinsuranceService.GetLevelRestoresByLevelId(levelId);
                return Json(levelRestores, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetLevelRestoresByLevelId);
            }
        }

        /// <summary>
        /// GetNextLevelNumberRestoreByLevelId
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetNextNumberRestoreByLevelId(int levelId)
        {
            try
            {
                int result = 0;
                result = DelegateService.reinsuranceService.GetNextNumberRestoreByLevelId(levelId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetNextNumberRestoreByLevelId);
            }
        }

        /// <summary>
        /// SaveLevelRestore
        /// </summary>
        /// <param name="levelRestoreModal"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveLevelRestore(LevelRestoreDTO levelRestoreDTO)
        {
            try
            {
                int levelRestoreId = Convert.ToInt32(levelRestoreDTO.Id);

                if (levelRestoreId.Equals(0))
                {
                    DelegateService.reinsuranceService.SaveLevelRestore(levelRestoreDTO);
                    return new UifJsonResult(true, true);
                }
                else
                {
                    DelegateService.reinsuranceService.UpdateLevelRestore(levelRestoreDTO);
                    return new UifJsonResult(true, true);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// DeleteLevelRestore
        /// </summary>
        /// <param name="levelRestoreId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteLevelRestore(int levelRestoreId)
        {
            bool isSucessfully = false;
            int deleted = 0;
            string message = "";

            try
            {
                DelegateService.reinsuranceService.DeleteLevelRestore(levelRestoreId);
                isSucessfully = true;
                deleted = 1;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isSucessfully = false;
                deleted = 0;
            }

            return Json(new { success = isSucessfully, result = deleted, msg = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region LayerCompany
        /// <summary>
        /// ContractLevelCompanyDialog
        /// Invoca a la vista de nivel de contrato de compañía ContractLevelCompanyDialog.cshtml
        /// </summary>
        /// <param name="contractLevelId"></param>
        /// <param name="contractLevelCompanyId"></param>
        /// <param name="brokerName"></param>
        /// <param name="reinsuranceCompanyName"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetContractLevelCompany(int contractLevelId, int contractLevelCompanyId, string brokerName, string reinsuranceCompanyName, int contractTypeId)
        {
            try
            {
                CurrentCulture();
                decimal participationPercentage = GetParticipationPercentageByContractLevelId(contractLevelId);
                string resultparticipationPercentage = (100 - participationPercentage).ToString();
                resultparticipationPercentage = resultparticipationPercentage.Replace(",", ".");

                ContractLevelCompanyModel levelCompanyModel = new ContractLevelCompanyModel();

                if (brokerName == "null")
                {
                    brokerName = String.Empty;
                }
                if (reinsuranceCompanyName == "null")
                {
                    reinsuranceCompanyName = String.Empty;
                }

                /* Se verifica si pertenece al tipo de contrato Proporcinal */
                List<ContractTypeDTO> contractTypes = DelegateService.reinsuranceService.GetContractTypes();
                int contractFuncionalityCompany = Convert.ToInt16(contractTypes.Where(ct => ct.ContractTypeId == contractTypeId).Select(sl => sl.ContractFunctionality.ContractFunctionalityId).First());
                bool contractFuncionality = contractFuncionalityCompany == Convert.ToInt16(ConfigurationManager.AppSettings["ContractFuncionalityProportional"]);

                if (contractLevelCompanyId.Equals(0))
                {
                    levelCompanyModel.ContractLevelCompanyId = 0;
                    levelCompanyModel.ContractLevel = new LevelDTO { ContractLevelId = contractLevelId };
                    levelCompanyModel.AgentIndividual = 0;
                    levelCompanyModel.Percentage = resultparticipationPercentage;
                    levelCompanyModel.CommissionPercentage = 0.ToString();
                    levelCompanyModel.ReservePremiumPercentage = 0.ToString();
                    levelCompanyModel.InterestReserveRelease = 0.ToString();
                    levelCompanyModel.AdditionalCommission = 0.ToString();
                    levelCompanyModel.DragLoss = 0.ToString();
                    levelCompanyModel.ReinsurerExpenditur = 0.ToString();
                    levelCompanyModel.ProfitSharingPercentage = 0.ToString();
                    levelCompanyModel.LossCommissionPercentage = 0.ToString();
                    levelCompanyModel.DifferentialCommissionPercentage = 0.ToString();
                    levelCompanyModel.ContractFunctionalityType = contractFuncionality;
                }
                else
                {
                    LevelCompanyDTO contractLevelCompany = DelegateService.reinsuranceService.GetLevelCompanyByCompanyId(contractLevelCompanyId);
                    contractLevelCompany.Agent.FullName = brokerName;
                    contractLevelCompany.Company.Name = reinsuranceCompanyName;
                    levelCompanyModel = new ContractLevelCompanyModel
                    {
                        AgentName = contractLevelCompany.Agent.FullName,
                        CommissionPercentage = contractLevelCompany.ComissionPercentage.ToString().Replace(",", "."),
                        CompanyName = contractLevelCompany.Company.Name,
                        ContractLevel = contractLevelCompany.ContractLevel,
                        ContractLevelCompanyId = contractLevelCompany.LevelCompanyId,
                        Percentage = contractLevelCompany.GivenPercentage.ToString().Replace(",", "."),
                        ReservePremiumPercentage = contractLevelCompany.ReservePremiumPercentage.ToString().Replace(",", "."),
                        InterestReserveRelease = contractLevelCompany.InterestReserveRelease.ToString().Replace(",", "."),
                        AdditionalCommission = contractLevelCompany.AdditionalCommissionPercentage.ToString(),
                        DragLoss = contractLevelCompany.DragLossPercentage.ToString(),
                        ReinsurerExpenditur = contractLevelCompany.ReinsuranceExpensePercentage.ToString().Replace(",", "."),
                        ProfitSharingPercentage = contractLevelCompany.UtilitySharePercentage.ToString(),
                        Presentation = Convert.ToInt16(contractLevelCompany.PresentationInformationType),
                        BrokerCommission = Convert.ToInt16(contractLevelCompany.IntermediaryCommission),
                        LossCommissionPercentage = contractLevelCompany.ClaimCommissionPercentage.ToString(),
                        DifferentialCommissionPercentage = contractLevelCompany.DifferentialCommissionPercentage.ToString(),
                        ContractFunctionalityType = contractFuncionality
                    };

                    resultparticipationPercentage = (decimal.Parse(resultparticipationPercentage,
                                                    CultureInfo.InvariantCulture.NumberFormat) + decimal.Parse(levelCompanyModel.Percentage,
                                                    CultureInfo.InvariantCulture.NumberFormat)).ToString();
                    resultparticipationPercentage = resultparticipationPercentage.Replace(",", ".");
                }

                ViewBag.ParticipationPercentage = resultparticipationPercentage;

                return PartialView("~/Areas/Reinsurance/Views/Reinsurance/Parameter/ContractLevelCompanyDialog.cshtml", levelCompanyModel);


            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetContractLevelCompanyByContractLevelId
        /// Obtiene los registros de la tabla REINS.CONTRACT_LEVEL_COMPANY dado el id de nivel de contrato
        /// </summary>
        /// <param name="contractLevelId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetContractLevelCompanyByContractLevelId(int contractLevelId)
        {
            try
            {
                List<LevelCompanyDTO> levelCompanyDTOs = new List<LevelCompanyDTO>();
                levelCompanyDTOs = DelegateService.reinsuranceService.GetLevelCompaniesByLevelId(contractLevelId);
                return new UifJsonResult(true, levelCompanyDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Global.ErrorGetContractLevelCompanyByContractLevelId);
            }
        }


        /// <summary>
        /// SaveContractLevelCompany
        /// Graba o actualiza un registro en la tabla REINS.CONTRACT_LEVEL_COMPANY
        /// </summary>
        /// <param name="levelCompanyDTO"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveContractLevelCompany(LevelCompanyDTO levelCompanyDTO)
        {
            try
            {
                PresentationInformationTypes presentationInformationTypes = PresentationInformationTypes.Annual;

                if (levelCompanyDTO.PresentationInformationType == (PresentationInformationTypes)1)
                {
                    presentationInformationTypes = PresentationInformationTypes.Monthly;
                }
                if (levelCompanyDTO.PresentationInformationType == (PresentationInformationTypes)2)
                {
                    presentationInformationTypes = PresentationInformationTypes.Quarterly;
                }
                if (levelCompanyDTO.PresentationInformationType == (PresentationInformationTypes)3)
                {
                    presentationInformationTypes = PresentationInformationTypes.Biannual;
                }
                if (levelCompanyDTO.PresentationInformationType == (PresentationInformationTypes)4)
                {
                    presentationInformationTypes = PresentationInformationTypes.Annual;
                }

                if (levelCompanyDTO.LevelCompanyId.Equals(0))
                {
                    DelegateService.reinsuranceService.SaveLevelCompany(levelCompanyDTO);
                }
                else
                {
                    DelegateService.reinsuranceService.UpdateLevelCompany(levelCompanyDTO);
                }

                return new UifJsonResult(true, levelCompanyDTO.ContractLevel.ContractLevelId);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }


        /// <summary>
        /// DeleteContractLevelCompany
        /// Borra un registro de la tabla REINS.CONTRACT_LEVEL_COMPANY dado el id de nivel de contrato y el id de nivel de contrato
        /// de compañía
        /// </summary>
        /// <param name="contractLevelId"></param>
        /// <param name="contractLevelCompanyId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteContractLevelCompany(int contractLevelId, int contractLevelCompanyId)
        {
            try
            {
                DelegateService.reinsuranceService.DeleteLevelCompany(contractLevelCompanyId);
                return new UifJsonResult(true, contractLevelId);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorDeleteContractLevelCompany);
            }
        }


        /// <summary>
        /// GetReinsuranceCompanyByName
        /// Obtiene los registro de la tabla UP.COMPANY de compañìas reaseguradoras   
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetReinsuranceCompanyByName(string query)
        {
            try
            {
                List<IndividualDTO> reinsuranceCompany = new List<IndividualDTO>();
                reinsuranceCompany = DelegateService.reinsuranceService.GetReinsurerByName(query.ToUpper(), Convert.ToInt32(ConfigurationManager.AppSettings["Reinsurance"]), Convert.ToInt32(ConfigurationManager.AppSettings["ForeignReinsurance"]));
                if (reinsuranceCompany.Count > 0)
                {
                    return Json(reinsuranceCompany, JsonRequestBehavior.AllowGet);
                }

                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetReinsuranceCompanyByName);
            }
        }

        public ActionResult IsReinsurerActive(int individualId)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.IsReinsurerActive(individualId);
                return new UifJsonResult(true, result);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// GetCompanyIdByContractLevelIdAndIndividualId
        /// Obtiene el id de la compañía de la tabla REINS.CONTRACT_LEVEL_COMPANY dado el id de nivel de contrato
        /// </summary>
        /// <param name="contractLevelId"></param>
        /// <param name="individualId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetCompanyIdByContractLevelIdAndIndividualId(int contractLevelId, int individualId)
        {
            try
            {
                int result = 0;
                result = DelegateService.reinsuranceService.GetReinsuranceCompanyIdByLevelIdAndIndividualId(contractLevelId, individualId);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetCompanyIdByContractLevelIdAndIndividualId);
            }
        }

        /// <summary>
        /// Obtiene el porcentaje de participación de un nivel de contrato tabla REINS.CONTRACT_LEVEL
        /// </summary>
        /// <param name="contractLevelId"></param>
        /// <returns>decimal</returns>
        private decimal GetParticipationPercentageByContractLevelId(int contractLevelId)
        {
            decimal result = 0;
            result = DelegateService.reinsuranceService.GetParticipationPercentageByLevelId(contractLevelId);
            return result;
        }

        /// <summary>
        /// GetFrameTime
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetFrameTime()
        {
            try
            {
                List<SelectDTO> selectDTOs = new List<SelectDTO>();
                selectDTOs.Add(new SelectDTO { Id = 1, Description = Global.Monthly });
                selectDTOs.Add(new SelectDTO { Id = 2, Description = Global.Quarterly });
                selectDTOs.Add(new SelectDTO { Id = 3, Description = Global.Biannual });
                selectDTOs.Add(new SelectDTO { Id = 4, Description = Global.Annual });
                return new UifSelectResult(selectDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetFrameTime);
            }
        }

        /// <summary>
        /// GetExistsIntermediaryCommission
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetExistsIntermediaryCommission()
        {
            try
            {
                List<SelectDTO> selectDTOs = new List<SelectDTO>();
                selectDTOs.Add(new SelectDTO { Id = 1, Description = @Global.Yes });
                selectDTOs.Add(new SelectDTO { Id = 0, Description = @Global.No });
                return new UifSelectResult(selectDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetExistsIntermediaryCommission);
            }
        }

        #endregion

        #endregion

        #region ParamLines

        #region Line

        /// <summary>
        /// MainLine
        /// Invoca a la vista de contratos MainLine.cshtml
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainLine()
        {
            try
            {
                //valida que el servicio este arriba
                List<ModuleDateDTO> moduleDates = new List<ModuleDateDTO>();
                moduleDates = DelegateService.reinsuranceService.GetModuleDates();
                return View("~/Areas/Reinsurance/Views/Reinsurance/Parameter/MainLine.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetCumulusType
        /// Recupera el tipo de cúmulo
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetCumulusType()
        {
            try
            {
                List<CumulusTypeDTO> cumulusTypeDTOs = new List<CumulusTypeDTO>();
                cumulusTypeDTOs = DelegateService.reinsuranceService.GetCumulusTypesOrderByDesc();
                return new UifSelectResult(cumulusTypeDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetCumulusType);
            }
        }

        /// <summary>
        /// SaveLine
        /// Graba una nueva Línea
        /// <param name="line"></param>
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult SaveLine(LineDTO lineDTO)
        {
            try
            {
                if (lineDTO.LineId.Equals(0))
                {
                    DelegateService.reinsuranceService.SaveLine(lineDTO);
                    return new UifJsonResult(true, null);
                }
                else
                {
                    DelegateService.reinsuranceService.UpdateLine(lineDTO);
                    return new UifJsonResult(true, null);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorSaveLine);
            }
        }

        /// <summary>
        /// LineIsUsed
        /// Verifica si una linea esta en uso
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult LineIsUsed(int lineId)
        {
            try
            {
                bool result = DelegateService.reinsuranceService.LineIsUsed(lineId);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorLineIsUsed);
            }
        }


        /// <summary>
        /// DeleteLine
        /// Elimina una línea dado el lineId
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteLine(int lineId)
        {
            try
            {
                // Validar que no tenga líneas de contratos asociados
                bool result = false;
                result = DelegateService.reinsuranceService.DeleteLineByLineId(lineId);
                return new UifJsonResult(result, "");
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorDeleteLine);
            }
        }

        /// <summary>
        /// AddLine
        /// Presenta al diálogo para agregar o modificar una línea
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult AddLine(int lineId)
        {
            try
            {
                //valida que el servicio este arriba  
                LineDTO lineDTO = new LineDTO();
                lineDTO = DelegateService.reinsuranceService.AddLine(lineId);
                return PartialView("~/Areas/Reinsurance/Views/Reinsurance/Parameter/LineDialog.cshtml", lineDTO);
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetLines
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetLines()
        {
            // DANC EN ASSOCIATION LINES MIGRAR
            try
            {
                List<LineDTO> lines = new List<LineDTO>();
                lines = DelegateService.reinsuranceService.GetLines().OrderByDescending(x => x.LineId).ToList();
                return new UifSelectResult(lines);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetLines);
            }
        }

        #endregion

        #region ContractLine

        /// <summary>
        /// GetLineCumulusType
        /// Recupera la relación entre línea y el tipo de cúmulo
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetLineCumulusType()
        {
            try
            {
                // Ordenado descendentemente por el Id
                List<LineCumulusTypeDTO> lineCumulusTypeDTOs = new List<LineCumulusTypeDTO>();
                lineCumulusTypeDTOs = DelegateService.reinsuranceService.GetLineCumulusTypeOrderByLineId();
                return new UifTableResult(lineCumulusTypeDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetLineCumulusType);
            }
        }

        /// <summary>
        /// GetContractLineByLineId
        /// Recupera la línea de contrato dado el lineId
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetContractLineByLineId(int lineId)
        {
            try
            {
                List<object> contractLines = new List<object>();
                LineDTO lineDTO = DelegateService.reinsuranceService.GetContractLineByLineId(lineId);
                foreach (ContractLineDTO contractLine in lineDTO.ContractLines)
                {
                    contractLines.Add(new
                    {
                        LineId = lineDTO.LineId,
                        LineDescription = lineDTO.Description,
                        ContractLineId = contractLine.ContractLineId,
                        Priority = contractLine.Priority,
                        ContractId = contractLine.Contract.ContractId,
                        ContractDescription = contractLine.Contract.Description
                    });
                }

                return new UifJsonResult(true, contractLines);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetContractLineByLineId);
            }
        }

        /// <summary>
        /// AddContractLine
        /// Presenta el diálgo para añadir/modificar ContractLine
        /// </summary>
        /// <param name="contractLineId"></param>
        /// <param name="lineId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult AddContractLine(int contractLineId, int lineId)
        {
            try
            {
                LineDTO line = new LineDTO();
                line = DelegateService.reinsuranceService.AddContractLine(contractLineId, lineId);
                return PartialView("~/Areas/Reinsurance/Views/Reinsurance/Parameter/ContractLineDialog.cshtml", line);
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// SaveContractLine
        /// Graba un nuevo registro Contract line
        /// </summary>
        /// <param name="line"></param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult SaveContractLine(LineDTO line)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.SaveContractLineByLine(line);
                return new UifJsonResult(result, null);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorSaveContractLine);
            }
        }

        /// <summary>
        /// DeleteContractLine
        /// Elimina un nuevo registro Contract line
        /// </summary>
        /// <param name="contractLineId"></param>
        /// <param name="lineId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteContractLine(int contractLineId, int lineId)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.DeleteContractLineByLine(contractLineId, lineId);
                return new UifJsonResult(result, null);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorDeleteContractLine);
            }
        }

        #endregion

        #endregion

        #region ParamAssociationLine

        /// <summary>
        /// GetAssociationLine
        /// Recupera asociación de línea 
        /// <param name="year"></param>
        /// <param name="associationTypeId"></param>
        /// <param name="associationLineId"></param>
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAssociationLine(int year, int associationTypeId, int associationLineId)
        {
            try
            {
                //Thread.CurrentThread.CurrentCulture = new CultureInfo("es-EC");
                List<AssociationLineDTO> associationLineDTOs = new List<AssociationLineDTO>();
                associationLineDTOs = DelegateService.reinsuranceService.GetAssociationLineByTypeLineYear(year, associationTypeId, associationLineId);
                return Json(new { aaData = associationLineDTOs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetAssociationLine);
            }
        }

        /// <summary>
        /// GetLineAssociationTypes
        /// Recupera tipo de asociaciones
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetLineAssociationTypes()
        {
            try
            {
                List<LineAssociationTypeDTO> lineAssociationTypeDTOs = new List<LineAssociationTypeDTO>();
                lineAssociationTypeDTOs = DelegateService.reinsuranceService.GetLineAssociationTypes();
                return new UifSelectResult(lineAssociationTypeDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetLineAssociationTypes);
            }
        }

        /// <summary>
        /// DeleteAssociationLines
        /// Elimina Asociación de líneas por el associationLineId/associationColumnValueId
        /// </summary>
        /// <param name="associationLineId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteAssociationLines(int associationLineId)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.DeleteAssociationLines(associationLineId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorDeleteAssociationLines);
            }
        }

        /// <summary>
        /// SaveLineAssociation 
        /// Graba la asociación de líneas AssociationColumnValue y AssociationLine
        /// </summary>
        /// <param name="associationLineDto"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult SaveLineAssociation(AssociationLineDTO associationLineDTO)
        {
            try
            {
                int result = 0;
                result = DelegateService.reinsuranceService.SaveLineAssociationByAssociationLine(associationLineDTO);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorSaveLineAssociation);
            }
        }

        /// <summary>
        /// SetLineAssociation
        /// </summary>
        /// <param name="associationLineDTO"></param>
        /// <returns>LineAssociation</returns>
        public LineAssociationDTO SetLineAssociation(AssociationLineDTO associationLineDTO)
        {
            LineAssociationDTO lineAssociationDTO = new LineAssociationDTO();
            ByLineBusinessDTO byLineBusinessDTO = new ByLineBusinessDTO();
            LineBusinessDTO lineBusinessDTO = new LineBusinessDTO();
            byLineBusinessDTO.LineBusiness = new List<LineBusinessDTO>();

            foreach (SubLineBusinessDTO subLineBusinessDTO in associationLineDTO.ByLineBusinessSubLineBusiness.SubLineBusiness)
            {
                lineBusinessDTO.Id = subLineBusinessDTO.Id;
                byLineBusinessDTO.LineBusiness.Add(lineBusinessDTO);
            }

            byLineBusinessDTO.LineAssociationTypeId = associationLineDTO.AssociationTypeId;
            lineAssociationDTO.Line = new LineDTO();
            lineAssociationDTO.AssociationType = new ByLineBusinessDTO();
            lineAssociationDTO.AssociationType = byLineBusinessDTO;
            lineAssociationDTO.Line.LineId = associationLineDTO.LineId;
            lineAssociationDTO.DateFrom = Convert.ToDateTime(associationLineDTO.DateFrom);
            lineAssociationDTO.DateTo = Convert.ToDateTime(associationLineDTO.DateTo);
            lineAssociationDTO.AssociationType = byLineBusinessDTO;

            return lineAssociationDTO;
        }

        public ActionResult ValidateDuplicateLineAssociation(AssociationLineDTO associationLineDTO)
        {
            try
            {
                bool result = DelegateService.reinsuranceService.ValidateDuplicateLineAssociation(associationLineDTO);
                return new UifJsonResult(true, result);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Global.ErrorValidateDuplicateLineAssocation);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Global.ErrorValidateDuplicateLineAssocation);
            }
        }

        #region ByPrefixSubPrefix

        /// <summary>
        /// GetLineBusiness
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetLineBusiness()
        {
            try
            {
                List<LineBusinessDTO> lineBusinessDTOs = new List<LineBusinessDTO>();
                lineBusinessDTOs = DelegateService.reinsuranceService.GetLineBusiness();
                return new UifSelectResult(lineBusinessDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetLineBusiness);
            }
        }

        /// <summary>
        /// GetSubLineBusiness
        /// </summary>
        /// <param name="lineBusiness"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetSubLineBusiness(int lineBusiness)
        {
            try
            {
                List<SubLineBusinessDTO> lineBusinessDTOs = new List<SubLineBusinessDTO>();
                lineBusinessDTOs = DelegateService.reinsuranceService.GetSubLineBusiness(lineBusiness);
                return new UifSelectResult(lineBusinessDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetSubLineBusiness);
            }
        }

        /// <summary>
        /// GetCoveragesByLineBusinessIdSubLineBusinessId
        /// </summary>
        /// <param name="lineBusinessId"></param>
        /// <param name="subLineBusinessId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCoveragesByLineBusinessIdSubLineBusinessId(int lineBusinessId, int subLineBusinessId)
        {
            try
            {
                List<CoverageDTO> coverageDTOs = new List<CoverageDTO>();
                coverageDTOs = DelegateService.reinsuranceService.GetCoveragesByLineBusinessIdSubLineBusinessId(lineBusinessId, subLineBusinessId);
                return new UifTableResult(coverageDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetCoveragesByLineBusinessIdSubLineBusinessId);
            }
        }

        /// <summary>
        /// GetInsuredObjectByPrefixIdList
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetInsuredObjectByPrefixIdList(int prefixId)
        {
            try
            {
                List<InsuredObjectDTO> insuredObjectDTOs = new List<InsuredObjectDTO>();
                insuredObjectDTOs = DelegateService.reinsuranceService.GetInsuredObjectByPrefixIdList(prefixId);
                return new UifTableResult(insuredObjectDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetInsuredObjectByPrefixIdList);
            }
        }
        #endregion

        #endregion

        #region ParamAssociationLine

        /// <summary>
        /// AssociationLine
        /// Asociacion de Linea
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AssociationLine()
        {
            try
            {
                //valida que el servicio este arriba
                List<ModuleDateDTO> moduleDates = new List<ModuleDateDTO>();
                moduleDates = DelegateService.reinsuranceService.GetModuleDates();
                ViewBag.ByLineBusiness = ConfigurationManager.AppSettings["ByLineBusiness"];
                ViewBag.ByLineBusinessSubLineBusiness = ConfigurationManager.AppSettings["ByLineBusinessSubLineBusiness"];
                ViewBag.ByOperationTypePrefix = ConfigurationManager.AppSettings["ByOperationTypePrefix"];
                ViewBag.ByInsured = ConfigurationManager.AppSettings["ByInsured"];
                ViewBag.ByPrefix = ConfigurationManager.AppSettings["ByPrefix"];
                ViewBag.ByPolicy = ConfigurationManager.AppSettings["ByPolicy"];
                ViewBag.ByFacultativeIssue = ConfigurationManager.AppSettings["ByFacultativeIssue"];
                ViewBag.ByInsuredPrefix = ConfigurationManager.AppSettings["ByInsuredPrefix"];
                ViewBag.ByPrefixProduct = ConfigurationManager.AppSettings["ByPrefixProduct"];
                ViewBag.ByLineBusinessSubLineBusinessRisk = ConfigurationManager.AppSettings["ByLineBusinessSubLineBusinessRisk"];
                ViewBag.ByPrefixRisk = ConfigurationManager.AppSettings["ByPrefixRisk"];
                ViewBag.ByPolicyLineBusinessSubLineBusiness = ConfigurationManager.AppSettings["ByPolicyLineBusinessSubLineBusiness"];
                ViewBag.ByLineBusinessSubLineBusinessCoverage = ConfigurationManager.AppSettings["ByLineBusinessSubLineBusinessCoverage"];
                ViewBag.YearNow = DateTime.Now.Year;
                return View("~/Areas/Reinsurance/Views/Reinsurance/Parameter/AssociationLine.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// AssociationLineDialog
        /// Dialogo de Asociacion de Linea
        /// </summary>
        /// <param name="lineAssociationTypeId"></param>
        /// <param name="associationLineId"></param>
        /// <param name="year"></param>
        /// <returns>ActionResult</returns>
        public ActionResult AddAssociationLine(int lineAssociationTypeId, int associationLineId, int year)
        {
            try
            {
                AssociationLineDTO associationLineDTOs = new AssociationLineDTO();
                associationLineDTOs = DelegateService.reinsuranceService.AddAssociationLine(lineAssociationTypeId, associationLineId, year);
                return PartialView("~/Areas/Reinsurance/Views/Reinsurance/Parameter/AssociationLineDialog.cshtml", associationLineDTOs);

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetInsuredById
        /// Obtiene listado de personas aseguradas
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetInsuredById(int individualId)
        {
            try
            {
                InsuredDTO insuredDTO = new InsuredDTO();
                insuredDTO = DelegateService.reinsuranceService.GetInsuredByIndividualId(individualId);
                return Json(insuredDTO, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetPolicyById
        /// Obtiene listado de personas aseguradas
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPolicyById(int policyId)
        {
            try
            {
                List<PolicyDTO> policyDTOs = new List<PolicyDTO>();
                policyDTOs = null;

                if (policyDTOs.Count > 0)
                {
                    return Json(policyDTOs[0], JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPolicies(string query, string param)
        {
            try
            {
                string[] result = param.Split('/');
                string policyDocumentNumber = query;
                string branchId = result[0];
                string prefixId = result[1];
                List<PolicyDTO> policies = new List<PolicyDTO>();

                PolicyDTO policy = DelegateService.reinsuranceService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(Convert.ToInt32(prefixId), Convert.ToInt32(branchId), Convert.ToDecimal(policyDocumentNumber));
                policy.DocumentNumber.ToString();
                policies.Add(policy);
                return Json(policies, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new List<PolicyDTO>(), JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region ParamPrefixCumulus

        /// <summary>
        /// MainPrefixCumulus
        /// Invoca a la vista de ramosde cumulos MainPrefixCumulus.cshtml
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainPrefixCumulus()
        {
            try
            {
                //valida que el servicio este arriba
                List<ModuleDateDTO> moduleDates = new List<ModuleDateDTO>();
                moduleDates = DelegateService.reinsuranceService.GetModuleDates();
                return View("~/Areas/Reinsurance/Views/Reinsurance/Parameter/MainPrefixCumulus.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetPrefixsCumulus
        /// Recuperacion de Ramos de Cumulos. 
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPrefixsCumulus()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-EC");
                List<LineBusinessDTO> businessLines = DelegateService.reinsuranceService.GetLineBusiness();
                List<TypeExerciseModel> exerciseTypes = GetTypeExercise();
                List<ReinsurancePrefixDTO> reinsurancePrefixes = DelegateService.reinsuranceService.GetReinsurancePrefixes();

                if (reinsurancePrefixes.Count > 0)
                {
                    var lista = from cumulusprefix in reinsurancePrefixes
                                select new PrefixCumulusModel()
                                {
                                    PrefixCumulusId = cumulusprefix.Id,
                                    Location = cumulusprefix.IsLocation,
                                    PrefixCD = cumulusprefix.Prefix.Id,
                                    PrefixDescription = businessLines.Find(a => a.Id.Equals(cumulusprefix.Prefix.Id)).Description,
                                    PrefixCumulusCD = cumulusprefix.PrefixCumulus.Id,
                                    PrefixCumulusDescription = businessLines.Find(a => a.Id.Equals(cumulusprefix.PrefixCumulus.Id)).Description,
                                    TypeExercice = cumulusprefix.ExerciseType,
                                    TypeExerciceDescripcion = exerciseTypes.Find(t => t.TypeExerciseId.Equals(Convert.ToInt16(cumulusprefix.ExerciseType))).TypeExerciceDescription

                                };

                    return Json(new { aaData = lista, total = reinsurancePrefixes.Count }, JsonRequestBehavior.AllowGet);
                }

                List<PrefixCumulusModel> prefixsCumulus = new List<PrefixCumulusModel>();
                return Json(new { aaData = prefixsCumulus, total = prefixsCumulus.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// GetTypeExercise
        /// Recuperacion de Tipos de Ejercicios. 
        /// </summary>
        /// <returns>List<TypeExerciceModel></returns>
        public List<TypeExerciseModel> GetTypeExercise()
        {
            List<TypeExerciseModel> listTypeExercise = new List<TypeExerciseModel>();

            try
            {
                listTypeExercise.Add(new TypeExerciseModel { TypeExerciseId = 1, TypeExerciceDescription = Global.EnumTEMotherPolicy });
                listTypeExercise.Add(new TypeExerciseModel { TypeExerciseId = 2, TypeExerciceDescription = Global.EnumTELastMovementPolicy });
                listTypeExercise.Add(new TypeExerciseModel { TypeExerciseId = 3, TypeExerciceDescription = Global.EnumTEOldEndorsementSurety });
                listTypeExercise.Add(new TypeExerciseModel { TypeExerciseId = 4, TypeExerciceDescription = Global.EnumTELastDateIssueOldEndorsement });

                return listTypeExercise;
            }
            catch (Exception)
            {
                return listTypeExercise;
            }
        }

        /// <summary>
        /// GetTypeExerciceJson
        /// Recuperacion de Tipos de Ejercicios en JSON 
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetTypeExercises()
        {
            return new UifSelectResult(GetTypeExercise());
        }


        /// <summary>
        /// SaveLineBusinessCumulus
        /// Grabacion de Ramos Reaseguro Cumulo
        /// <param name="prefixcumulusmodel"></param>
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult SaveLineBusinessCumulus(ReinsurancePrefixDTO reinsurancePrefixDTO)
        {
            try
            {
                reinsurancePrefixDTO = DelegateService.reinsuranceService.SaveReinsurancePrefix(reinsurancePrefixDTO);
                return new UifJsonResult(true, reinsurancePrefixDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// UpdateLineBusinessCumulus
        /// Actualizacion de Ramos Reaseguro Cumulo
        /// <param name="prefixcumulusmodel"></param>
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateLineBusinessCumulus(ReinsurancePrefixDTO reinsurancePrefixDTO)
        {
            try
            {
                reinsurancePrefixDTO = DelegateService.reinsuranceService.UpdateReinsurancePrefix(reinsurancePrefixDTO);
                return new UifJsonResult(true, reinsurancePrefixDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// DeleteLineBusinessCumulus
        /// Eliminacion de Ramos Reaseguro Cumulo
        /// <param name="reinsurancePrefixDTO"></param>
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteLineBusinessCumulus(ReinsurancePrefixDTO reinsurancePrefixDTO)
        {
            try
            {
                DelegateService.reinsuranceService.DeleteReinsurancePrefix(reinsurancePrefixDTO);
                return new UifJsonResult(true, reinsurancePrefixDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorDeleteLineBusinessCumulus);
            }
        }

        /// <summary>
        /// LoadBranch
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult LoadBranch()
        {
            try
            {
                List<BranchDTO> branchDTOs = new List<BranchDTO>();
                branchDTOs = DelegateService.reinsuranceService.GetBranches();
                return new UifSelectResult(branchDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorLoadBranch);
            }
        }

        /// <summary>
        /// LoadPrefixes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult LoadPrefixes()
        {
            try
            {
                List<PrefixDTO> prefixDTOs = new List<PrefixDTO>();
                prefixDTOs = DelegateService.reinsuranceService.GetPrefixes();
                return new UifSelectResult(prefixDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorLoadPrefixes);
            }
        }

        /// <summary>
        /// GetOperations
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetOperations()
        {
            try
            {
                return new UifJsonResult(true, GetEnumDescription(typeof(BussinesType)));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetOperations);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// CurrentCulture
        /// </summary>
        private void CurrentCulture()
        {
            string urlReferrer = Request.UrlReferrer.ToString();

            if (urlReferrer.IndexOf("en") > 0)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-us");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-GT");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-GT");
            }
        }

        /// <summary>
        /// GetEnumDescription
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns>List<object></returns>
        private List<object> GetEnumDescription(Type enumType)
        {
            List<object> businessTypes = new List<object>();
            string key = "";
            int index = 0;

            foreach (var value in Enum.GetValues(enumType))
            {
                var query = value.GetType().GetField(value.ToString()).GetCustomAttributes(true).
                            Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)));

                index = Convert.ToInt32(value);

                if (query.Any())
                {
                    key = (value.GetType().GetField(value.ToString()).GetCustomAttributes(true).
                           Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute))).
                           FirstOrDefault() as System.ComponentModel.DescriptionAttribute).Description;
                }
                else
                {
                    key = Enum.GetName(enumType, value);
                }

                businessTypes.Add(new { Id = index, Description = key });
            }

            return businessTypes;
        }

        #endregion

        #region PriorityRetention
        /// <summary>
        /// Inicializa la vista de Priority Retention
        /// </summary>
        /// <returns></returns>
        public ActionResult PriorityRetention()
        {
            return View("~/Areas/Reinsurance/Views/Reinsurance/Parameter/PriorityRetention.cshtml");
        }

        public ActionResult GetPriorityRetentions()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.reinsuranceService.GetPriorityRetentions());
            }
            catch
            {
                return new UifJsonResult(false, Language.ErrorGettingLinesBusinessData);
            }
        }

        public ActionResult SavePriorityRetentions(List<PriorityRetentionDTO> lstPriorityRetentionAdded, List<PriorityRetentionDTO> lstPriorityRetentionModified, List<PriorityRetentionDTO> lstPriorityRetentionDelete)
        {
            try
            {
                if (lstPriorityRetentionAdded != null)
                {
                    DelegateService.reinsuranceService.SavePriorityRetentions(lstPriorityRetentionAdded);
                }
                if (lstPriorityRetentionModified != null)
                {
                    DelegateService.reinsuranceService.UpdatePriorityRetentions(lstPriorityRetentionModified);
                }
                if (lstPriorityRetentionDelete != null)
                {
                    DelegateService.reinsuranceService.DeletePriorityRetentions(lstPriorityRetentionDelete);
                }
                return new UifJsonData(true, Language.SavePriorityRetentionSuccess);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorSavePriorityRetention);
            }
        }

        public ActionResult CanPriorityRetentionUpdated(int priorityRetentionId)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.CanPriorityRetentionUpdated(priorityRetentionId);
                return new UifJsonResult(true, result);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
        #endregion

    }
}
