using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Sistran.Core.Application.ReinsuranceServices.DTOs;
using Sistran.Core.Application.ReinsuranceServices.Enums;
using Sistran.Core.Application.ReportingServices.Models.Formats;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.App_GlobalResources;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Controllers
{
    [Authorize]
    [HandleError]
    public class ProcessController : Controller
    {


        #region Menu

        public ActionResult ClaimProcess()
        {
            try
            {
                return View("~/Areas/Reinsurance/Views/Reinsurance/Process/ReinsureClaims.cshtml");
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorClaimProcess);
            }
        }

        /// <summary>
        /// PaymentProcess
        /// Invoca a la vista de proceso de reasegurar por pagos
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PaymentProcess()
        {
            try
            {
                return View("~/Areas/Reinsurance/Views/Reinsurance/Process/ReinsurePayments.cshtml");
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorPaymentProcess);
            }
        }

        /// <summary>
        /// ReinsureProcess
        /// Invoca a la vista de proceso de reasegurar por polizas
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ReinsureProcess()
        {
            try
            {
                ViewBag.ReinsuranceDate = LoadClosingDateReinsurance();
                return View("~/Areas/Reinsurance/Views/Reinsurance/Process/ReinsurePolicies.cshtml");
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorReinsureProcess);
            }
        }

        /// <summary>
        /// ReinsurePoliciesRangeDate
        /// Invoca a la vista de proceso de reasegurar por polizas por rango de fechas
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ReinsurePoliciesRangeDate()
        {
            try
            {
                //valida que el servicio este arriba
                List<ModuleDateDTO> moduleDateDTOs = new List<ModuleDateDTO>();
                moduleDateDTOs = DelegateService.reinsuranceService.GetModuleDates();
                return View("~/Areas/Reinsurance/Views/Reinsurance/Process/ReinsurePoliciesRangeDate.cshtml");
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorReinsurePoliciesRangeDate);
            }
        }

        /// <summary>
        /// SearchReinsureProcessMassive
        /// Invoca a la vista de proceso de reasegurar por polizas por rango de fechas
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult SearchReinsureProcessMassive()
        {
            try
            {
                //valida que el servicio este arriba
                List<ModuleDateDTO> moduleDateDTOs = new List<ModuleDateDTO>();
                moduleDateDTOs = DelegateService.reinsuranceService.GetModuleDates();
                return View("~/Areas/Reinsurance/Views/Reinsurance/Process/SearchReinsureProcessMassive.cshtml");
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorSearchReinsureProcessMassive);
            }
        }

        /// <summary>
        /// FindReinsuranceProcess
        /// Invoca a la vista de proceso de buscar reaseguro
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult FindReinsuranceProcess()
        {
            try
            {
                ViewBag.ReinsuranceDate = LoadClosingDateReinsurance();
                return View("~/Areas/Reinsurance/Views/Reinsurance/Process/FindReinsurance.cshtml");
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorFindReinsuranceProcess);
            }
        }

        /// <summary>
        /// ReinsureReports
        /// Invoca a la vista de proceso de reasegurar por polizas por rango de fechas
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ReinsureReports()
        {
            try
            {
                //valida que el servicio este arriba
                List<ModuleDateDTO> moduleDateDTOs = new List<ModuleDateDTO>();
                moduleDateDTOs = DelegateService.reinsuranceService.GetModuleDates();
                return View("~/Areas/Reinsurance/Views/Reinsurance/Process/ReinsureReports.cshtml");

            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorReinsureReports);
            }
        }

        #endregion

        #region Common

        /// <summary>
        /// GetBranch
        /// Obtiene los registros de la tabla COMM.BRANCH
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetBranch()
        {
            try
            {
                List<BranchDTO> branchDTOs = new List<BranchDTO>();
                int userId = SessionHelper.GetUserId();
                branchDTOs = DelegateService.reinsuranceService.GetBranchesByUserId(userId).OrderBy(br => br.Description).ToList();
                return new UifSelectResult(branchDTOs);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetBranch);
            }
        }

        /// <summary>
        /// GetPrefix
        /// Obtiene los ramos de la tabla COMM.PREFIX
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPrefix()
        {
            try
            {
                List<PrefixDTO> prefixDTOs = new List<PrefixDTO>();
                prefixDTOs = DelegateService.reinsuranceService.GetPrefixes();
                return new UifSelectResult(prefixDTOs);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetPrefix);
            }
        }

        /// <summary>
        /// GetProductByPrefix
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetProductByPrefix(int prefixId)
        {
            try
            {
                List<ProductDTO> productDTOs = new List<ProductDTO>();
                productDTOs = DelegateService.reinsuranceService.GetProductsByPrefixId(prefixId);
                return new UifJsonResult(true, productDTOs);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetProductByPrefix);
            }
        }

        #endregion

        #region IssuanceReinsurance

        #region Search

        /// <summary>
        /// GetEndorsementByPolicyId
        /// Obtiene los endosos de una póliza dado el id
        /// </summary>
        /// <param name="branchCode"></param>
        /// <param name="prefixCode"></param>
        /// <param name="documentNumber"></param>
        /// <param name="endorsementNumber"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetEndorsementByPolicyId(int branchCode, int prefixCode, decimal documentNumber, int endorsementNumber)
        {
            try
            {
                List<EndorsementDTO> endorsementDTOs = new List<EndorsementDTO>();
                endorsementDTOs = DelegateService.reinsuranceService.GetEndorsementByPolicyId(branchCode, prefixCode, documentNumber, endorsementNumber);
                return new UifJsonResult(true, endorsementDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Global.PolicyNotExists);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Global.PolicyNotExists);
            }
        }

        public ActionResult GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(int branchCode, int prefixCode, decimal documentNumber)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.reinsuranceService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixCode, branchCode, documentNumber));
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Global.PolicyNotExists);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Global.PolicyNotExists);
            }
        }

        /// <summary>
        /// GetReinsuranceDistributionByEndorsementId
        /// Obtiene los registros generados por el proceso de reaseguros
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetReinsuranceDistributionByEndorsementId(int endorsementId)
        {
            try
            {
                List<ReinsuranceDistributionHeaderDTO> reinsuranceDistributionHeaderDTOs = new List<ReinsuranceDistributionHeaderDTO>();
                reinsuranceDistributionHeaderDTOs = DelegateService.reinsuranceService.GetReinsuranceDistributionByEndorsementId(endorsementId);
                return new UifJsonResult(true, reinsuranceDistributionHeaderDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorGetReinsuranceDistributionByEndorsementId);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetReinsuranceDistributionByEndorsementId);
            }
        }

        /// <summary>
        /// GetDistributionErrors
        /// Llena la grilla de posibles errores cuando se ejecuta un reaseguro
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetDistributionErrors(int endorsementId)
        {
            try
            {
                List<IssGetDistributionErrorsDTO> issGetDistributionErrorsDTOs = new List<IssGetDistributionErrorsDTO>();
                issGetDistributionErrorsDTOs = DelegateService.reinsuranceService.GetDistributionErrors(endorsementId);
                return new UifJsonResult(true, issGetDistributionErrorsDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorGetDistributionErrors);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetDistributionErrors);
            }
        }

        #endregion

        #region Modification

        #region Layer

        /// <summary>
        /// GetTempLayerDistribution
        /// Vista que trae las capas de reaseguro en base al endoso
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetTempLayerDistribution(int endorsementId)
        {
            try
            {
                List<TempLayerDistributionsDTO> tempLayerDistributionsDTOs = new List<TempLayerDistributionsDTO>();
                tempLayerDistributionsDTOs = DelegateService.reinsuranceService.GetTempLayerDistributionByEndorsementId(endorsementId);
                return new UifJsonResult(true, tempLayerDistributionsDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorGetTempLayerDistribution);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetTempLayerDistribution);
            }
        }

        /// <summary>
        /// ModificationReinsuranceLayerDialog
        /// Dialogo de Modificación de Reaseguros capas
        /// </summary>
        /// <param name="tempReinsuranceProcessId"></param>
        /// <param name="tempIssueLayerId"></param>
        /// <returns>JsonResult</returns>
        public ActionResult ModificationReinsuranceLayer(int endorsementId, int tempIssueLayerId)
        {
            try
            {
                List<ReinsuranceLayerIssuanceDTO> reinsuranceLayerIssuanceDTOs = new List<ReinsuranceLayerIssuanceDTO>();
                reinsuranceLayerIssuanceDTOs = DelegateService.reinsuranceService.ModificationReinsuranceLayer(endorsementId, tempIssueLayerId);
                return new UifJsonResult(true, reinsuranceLayerIssuanceDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Global.ErrorModificationReinsuranceLayer);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Global.ErrorModificationReinsuranceLayer);
            }
        }

        /// <summary>
        /// SaveTempIssueLayer
        /// Graba o actualiza una capa de reaseguros teniendo en cuenta ciertas validaciones
        /// </summary>
        /// <param name="reinsuranceLayerIssuanceDTO"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveTempIssueLayer(ReinsuranceLayerDTO reinsuranceLayerIssuanceDTO, int endorsementId)
        {
            try
            {
                List<ReinsuranceLayerIssuanceDTO> reinsuranceLayerIssuanceDTOs = new List<ReinsuranceLayerIssuanceDTO>();
                reinsuranceLayerIssuanceDTOs = DelegateService.reinsuranceService.SaveTempIssueLayerByEndorsementId(reinsuranceLayerIssuanceDTO, endorsementId);
                return new UifJsonResult(true, reinsuranceLayerIssuanceDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorSaveTempIssueLayer);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorSaveTempIssueLayer);
            }
        }

        /// <summary>
        /// DeleteTempIssueLayer
        /// Permite eliminar una capa de reaseguros solo en orden descendente 
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="tempIssueLayerId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteTempIssueLayer(int endorsementId, int tempIssueLayerId)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.DeleteTempIssueLayerByEndorsementId(endorsementId, tempIssueLayerId);
                return new UifTableResult(result);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorDeleteTempIssueLayer);
            }
        }

        #endregion

        #region Line

        /// <summary>
        /// GetTempLineCumulus
        /// Trae la línea/cumulo de reaseguros temporal
        /// </summary>
        /// <param name="tempIssueLayerId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetTempLineCumulus(int tempIssueLayerId)
        {
            try
            {
                List<TempLineCumulusIssuanceDTO> tempLineCumulusIssuanceDTOs = new List<TempLineCumulusIssuanceDTO>();
                tempLineCumulusIssuanceDTOs = DelegateService.reinsuranceService.GetTempLineeCumulusByIssuance(tempIssueLayerId);
                return new UifTableResult(tempLineCumulusIssuanceDTOs);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifTableResult(new List<ReinsuranceLayerDTO>());
            }
        }

        /// <summary>
        /// ModificationReinsuranceLineDialog
        /// Dialogo de Modificación de Reaseguros Líneas
        /// </summary>
        /// <param name="tempLayerLineId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult ModificationReinsuranceLineDialog(int tempLayerLineId)
        {
            try
            {
                ReinsuranceLineDTO reinsuranceLineDTO = new ReinsuranceLineDTO();
                reinsuranceLineDTO = DelegateService.reinsuranceService.GetTempLayerLineById(tempLayerLineId);
                return Json(reinsuranceLineDTO, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return Json(new ReinsuranceLineDTO(), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetReinsuranceLines
        /// Recupera líneas para llenar combo de reaseguro de líneas
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetReinsuranceLines()
        {
            try
            {
                List<LineDTO> lineDTOs = new List<LineDTO>();
                lineDTOs = DelegateService.reinsuranceService.GetReinsuranceLines();
                return new UifSelectResult(lineDTOs);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetReinsuranceLines);
            }
        }

        /// <summary>
        /// UpdateTempLayerLine
        /// </summary>
        /// <param name="reinsuranceLine"></param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult UpdateTempLayerLine(ReinsuranceLineDTO reinsuranceLine)
        {
            try
            {
                DelegateService.reinsuranceService.UpdateTempLayerLine(reinsuranceLine);
                return new UifJsonResult(true, null);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                // Error generado al interior de la BDD
                return new UifJsonResult(false, Language.ErrorUpdateTempLayerLine);
            }
        }

        #endregion

        #region Allocation

        /// <summary>
        /// GetTempAllocation
        /// Trae la distribución temporal por línea
        /// </summary>
        /// <param name="tempLayerLineId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetTempAllocation(int tempLayerLineId)
        {
            try
            {
                List<ReinsuranceAllocationDTO> reinsuranceAllocationDTOs = new List<ReinsuranceAllocationDTO>();
                reinsuranceAllocationDTOs = DelegateService.reinsuranceService.GetTempAllocationByLayerLineId(tempLayerLineId);
                return new UifTableResult(reinsuranceAllocationDTOs);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                // Error generado al interior del SP o de la Vista de BDD
                return new UifTableResult(new List<Object>());
            }
        }

        /// <summary>
        /// GetTotSumPrimeAllocation
        /// Trae la distribución temporal por línea
        /// </summary>
        /// <param name="tempLayerLineId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetTotSumPrimeAllocation(int tempLayerLineId)
        {
            try
            {
                List<ReinsuranceAllocationDTO> reinsuranceAllocationDTOs = new List<ReinsuranceAllocationDTO>();
                reinsuranceAllocationDTOs = DelegateService.reinsuranceService.GetTotSumPrimeAllocation(tempLayerLineId);
                return new UifTableResult(reinsuranceAllocationDTOs);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetTotSumPrimeAllocation);
            }
        }

        /// <summary>
        /// ModificationReinsuranceContractDialog
        /// Dialogo de Modificación de Reaseguros Contratos Distribución
        /// </summary>
        /// <param name="tempIssueAllocationId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ModificationReinsuranceContractDialog(int tempIssueAllocationId)
        {
            try
            {
                ReinsuranceAllocationDTO reinsuranceAllocationDTO = new ReinsuranceAllocationDTO();
                reinsuranceAllocationDTO = DelegateService.reinsuranceService.ModificationReinsuranceContractDialog(tempIssueAllocationId);
                return Json(reinsuranceAllocationDTO, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorModificationReinsuranceContractDialog);
            }
        }

        /// <summary>
        /// UpdateTempAllocation
        /// </summary>
        /// <param name="reinsuranceAllocationDTO"></param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult UpdateTempAllocation(ReinsuranceAllocationDTO reinsuranceAllocationDTO)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.UpdateTempAllocationByReinsuranceAllocation(reinsuranceAllocationDTO);
                return new UifJsonResult(result, null);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                // Error generado al interior de la BDD
                return new UifJsonResult(false, Language.ErrorUpdateTempAllocation);
            }
        }

        #endregion

        #endregion

        #region Operations


        /// <summary>
        /// ReinsuranceProcess
        /// Permite ejecutar el proceso de reasegurar una póliza/endoso o un proceso masivo, dado el id de póliza, id de endoso,
        /// tipo de proceso, fecha desde y fecha hasta
        /// btnReinsurance
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="processTypeId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>ActionResult</returns>
        public ActionResult ReinsuranceProcess(int policyId, int endorsementId, int processTypeId, string dateFrom, string dateTo)
        {
            try
            {
                ReinsuranceDTO reinsuranceDTO = new ReinsuranceDTO();
                reinsuranceDTO = DelegateService.reinsuranceService.ReinsuranceIssue(policyId, endorsementId, SessionHelper.GetUserId());
                return new UifJsonResult(true, reinsuranceDTO);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException un)
            {
                return Json(new { success = false, result = un.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// SaveReinsuranceMassiveHeader
        /// Valida fecha desde que no sea una fecha ya cerrada y devuelve el número de registros a procesar
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="typeProcess"></param>
        /// <param name="prefixes"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveReinsuranceMassiveHeader(string dateFrom, string dateTo, int typeProcess, List<PrefixDTO> prefixes)
        {
            try
            {
                ReinsuranceMassiveHeaderDTO reinsuranceMassiveHeaderDTO = new ReinsuranceMassiveHeaderDTO();
                reinsuranceMassiveHeaderDTO = DelegateService.reinsuranceService.SaveReinsuranceMassiveHeaderByTypeProcess(dateFrom, dateTo, typeProcess, prefixes, SessionHelper.GetUserId());
                if (reinsuranceMassiveHeaderDTO.Option != null)
                {
                    return new UifJsonResult(false, reinsuranceMassiveHeaderDTO);
                }
                else
                {
                    return new UifJsonResult(true, reinsuranceMassiveHeaderDTO);
                }
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// GetTypeProcessMassive
        /// Obtiene los modulos emisión/siniestros/pagos
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetTypeProcessMassive()
        {
            try
            {
                List<ModuleDTO> moduleDTOs = new List<ModuleDTO>();
                moduleDTOs = DelegateService.reinsuranceService.GetModules().OrderBy(x => x.Id).ToList();
                return new UifSelectResult(moduleDTOs);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetTypeProcessMassive);
            }
        }

        /// <summary>
        /// ReinsuranceMassiveExecute
        /// Permite lanzar el proceso de reasegurar masivo 
        /// basado en el proceso previamente creado
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="moduleId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult ReinsuranceMassiveExecute(int processId, int moduleId)
        {
            try
            {
                DelegateService.reinsuranceService.ReinsuranceMassiveByProccesIdModuleId(processId, moduleId);
                return new UifJsonResult(true, true);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Global.ErrorMassiveReinsurance);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Global.ErrorMassiveReinsurance);
            }
        }

        /// <summary>
        /// GetTempReinsuranceProcess
        /// Trae el estatus de registros procesados del proceso de reaseguro masivo 
        /// </summary>
        /// <param name="tempReinsuranceProcessId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetTempReinsuranceProcess(int tempReinsuranceProcessId)
        {
            try
            {
                List<TempReinsuranceProcessDTO> tempReinsuranceProcessDTOs = new List<TempReinsuranceProcessDTO>();
                tempReinsuranceProcessDTOs = DelegateService.reinsuranceService.GetTempReinsuranceProcessByProcessId(tempReinsuranceProcessId);
                return new UifJsonResult(true, tempReinsuranceProcessDTOs);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifTableResult(new List<ReinsuranceLayerDTO>());
            }
        }

        /// <summary>
        /// GetTempReinsuranceProcesses
        /// Trae los registros procesados del proceso de reaseguro masivo 
        /// </summary>
        /// <param name="tempReinsuranceProcessId"></param>
        /// <param name="moduleId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetTempReinsuranceProcesses(int? tempReinsuranceProcessId, int moduleId /*, UifTableParam param*/)
        {

            List<TempReinsuranceProcessDTO> tempReinsuranceProcessDTOs = new List<TempReinsuranceProcessDTO>();
            try
            {
                tempReinsuranceProcessDTOs = DelegateService.reinsuranceService.GetTempReinsuranceProcess(tempReinsuranceProcessId, moduleId).OrderBy(x => x.TempReinsuranceProcessId).ToList();
                return new UifTableResult(tempReinsuranceProcessDTOs);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (UnhandledException)
            {
                tempReinsuranceProcessDTOs.Add(new TempReinsuranceProcessDTO { StatusDescription = Global.UnhandledExceptionMsj });

                return new UifTableResult(tempReinsuranceProcessDTOs);
            }
        }

        /// <summary>
        /// GetTempReinsuranceProcessDetails
        /// Trae los registros procesados del proceso de reaseguro masivo 
        /// </summary>
        /// <param name="tempReinsuranceProcessId"></param>
        /// <param name="param"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetTempReinsuranceProcessDetails(int tempReinsuranceProcessId)
        {
            try
            {
                List<TempReinsuranceProcessDTO> tempReinsuranceProcessDTOs = new List<TempReinsuranceProcessDTO>();
                tempReinsuranceProcessDTOs = DelegateService.reinsuranceService.GetTempReinsuranceProcessDetails(tempReinsuranceProcessId).OrderBy(x => x.BranchId & x.PrefixId & x.EndorsementNumber & x.CoverageNumber).OrderBy(x => x.PolicyNumber).ToList();
                return new UifJsonResult(true, tempReinsuranceProcessDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Global.ErrorGetDistributionErrorsMassive);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Global.ErrorGetDistributionErrorsMassive);
            }
        }

        /// <summary>
        /// UpdateTempReinsuranceProcess
        /// Actualiza el estatus de registros procesados del proceso de reaseguro masivo 
        /// </summary>
        /// <param name="processId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult UpdateTempReinsuranceProcess(int processId)
        {
            try
            {
                DelegateService.reinsuranceService.UpdateTempReinsuranceProcess(processId, 0, 0, DateTime.Now, 0);
                return new UifJsonResult(true, 0);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorUpdateTempReinsuranceProcess);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorUpdateTempReinsuranceProcess);
            }
        }

        /// <summary>
        /// LoadReinsuranceLayer
        /// Permite ejecutar la carga del reaseguro 
        /// btnLoadReinsurance
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult LoadReinsuranceLayer(int endorsementId)
        {
            try
            {
                int reinsuranceLayers = 0;
                List<PrefixDTO> prefixDTOs = new List<PrefixDTO>();
                reinsuranceLayers = DelegateService.reinsuranceService.LoadReinsuranceLayer(endorsementId, SessionHelper.GetUserId(), (int)ProcessTypes.Manual, DateTime.Now, DateTime.Now, prefixDTOs);

                if (reinsuranceLayers == 0)
                {
                    return new UifJsonResult(false, null);
                }

                return new UifJsonResult(true, reinsuranceLayers);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorLoadReinsuranceLayer);
            }
        }

        /// <summary>
        /// RecalculateReinsurance
        /// Permite recalcular  el reaseguro
        /// btnRecalculate
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns>ActionResult</returns>        
        public ActionResult RecalculateReinsurance(int endorsementId, int policyId, int? processId)
        {
            try
            {
                ReinsuranceDTO reinsuranceDTO = new ReinsuranceDTO();
                PolicyDTO policyDTO = new PolicyDTO();
                policyDTO.Endorsement = new EndorsementDTO();
                policyDTO.PolicyId = policyId;
                policyDTO.Endorsement.Id = endorsementId;
                reinsuranceDTO = DelegateService.reinsuranceService.ManualIssueReinsurance(policyDTO, SessionHelper.GetUserId(), processId);

                if (reinsuranceDTO.ReinsuranceId == 0)
                {
                    return new UifJsonResult(false, null);
                }
                else
                {
                    #region CabeceraReaseguro
                    TempData["ReinsuranceId"] = reinsuranceDTO.ReinsuranceId;
                    TempData["Number"] = reinsuranceDTO.Number;
                    TempData["Movements"] = Convert.ToInt32(reinsuranceDTO.Movements);
                    TempData["ProcessDate"] = reinsuranceDTO.ProcessDate;
                    TempData["IssueDate"] = reinsuranceDTO.IssueDate;
                    TempData["ValidityFrom"] = reinsuranceDTO.ValidityFrom;
                    TempData["ValidityTo"] = reinsuranceDTO.ValidityTo;
                    TempData["IsAutomatic"] = reinsuranceDTO.IsAutomatic;
                    TempData["UserId"] = reinsuranceDTO.UserId;
                    #endregion
                    return new UifJsonResult(true, reinsuranceDTO);
                }
            }
            catch (BusinessException ex)
            {
                return Json(new { success = false, result = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// SaveReinsurance
        /// Graba el reaseguro a tablas definitivas
        /// btnSaveReinsurance
        /// </summary>
        /// <param name="tempReinsuranceProcessId"></param>
        /// <param name="endorsementId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveReinsurance(int endorsementId)
        {
            try
            {
                List<ReinsuranceDTO> reinsuranceDTOs = new List<ReinsuranceDTO>();
                ReinsuranceDTO reinsuranceDTO = new ReinsuranceDTO();
                //Carga cabecera del Reaseguro
                reinsuranceDTO.ReinsuranceId = Convert.ToInt32(TempData["ReinsuranceId"]);
                reinsuranceDTO.Number = Convert.ToInt32(TempData["Number"]);
                reinsuranceDTO.ProcessDate = Convert.ToDateTime(TempData["ProcessDate"]);
                reinsuranceDTO.IssueDate = Convert.ToDateTime(TempData["IssueDate"]);
                reinsuranceDTO.ValidityFrom = Convert.ToDateTime(TempData["ValidityFrom"]);
                reinsuranceDTO.ValidityTo = Convert.ToDateTime(TempData["ValidityTo"]);
                reinsuranceDTO.IsAutomatic = Convert.ToBoolean(TempData["IsAutomatic"]);
                reinsuranceDTO.UserId = Convert.ToInt32(TempData["UserId"]);

                switch (Convert.ToInt32(TempData["Movements"])) //Tipo de movimiento
                {
                    case 1:
                        reinsuranceDTO.Movements = Movements.Original;
                        break;
                    case 2:
                        reinsuranceDTO.Movements = Movements.Counterpart;
                        break;
                    case 3:
                        reinsuranceDTO.Movements = Movements.Adjustment;
                        break;
                }

                reinsuranceDTOs = DelegateService.reinsuranceService.SaveReinsuranceByEndorsementId(endorsementId, SessionHelper.GetUserId(), reinsuranceDTO);
                return new UifJsonResult(true, reinsuranceDTOs);

            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public UifJsonResult CreateOperatingQuotaEventsByProcessId(int processId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.reinsuranceService.CreateOperatingQuotaEventsByProcessId(processId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Global.BusinessExceptionMsj);
            }
        }

        #endregion

        #region Facultative

        /// <summary>
        /// GetAgentByName
        /// Devuelve los brokers 
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAgentByName(string query)
        {
            try
            {
                List<AgentDTO> agentDTOs = new List<AgentDTO>();
                agentDTOs = DelegateService.reinsuranceService.GetAgentByName(query);
                return Json(agentDTOs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetAgentByName);
            }
        }

        /// <summary>
        /// ModificationReinsuranceFacultativeDialog
        /// </summary>
        /// <param name="tempFacultativeCompanyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="layerNumber"></param>
        /// <param name="lineId"></param>
        /// <param name="cumulusKey"></param>
        /// <returns>JsonResult</returns>
        public ActionResult GetReinsuranceFacultative(int? tempFacultativeCompanyId, int endorsementId, int layerNumber, int lineId, string cumulusKey)
        {
            try
            {
                ReinsuranceFacultativeDTO reinsuranceFacultativeDTO = new ReinsuranceFacultativeDTO();
                reinsuranceFacultativeDTO = DelegateService.reinsuranceService.GetReinsuranceFacultative(tempFacultativeCompanyId, endorsementId, layerNumber, lineId, cumulusKey);
                return new UifJsonResult(true, reinsuranceFacultativeDTO);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Global.ErrorGetReinsuranceFacultative);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Global.ErrorGetReinsuranceFacultative);
            }
        }

        /// <summary>
        /// LoadFacultative
        /// Permite recalcular  el reaseguro
        /// btnLoadFacultative
        /// </summary>
        /// <param name="tempReinsuranceProcessId"></param>
        /// <param name="layerNumber"></param>
        /// <param name="lineId"></param>
        /// <param name="cumulusKey"></param>
        /// <param name="description"></param>
        /// <param name="sumPercentage"></param>
        /// <param name="premiumPercentage"></param>   
        /// <returns>ActionResult</returns>
        public ActionResult LoadFacultative(int tempReinsuranceProcessId, int? layerNumber, int? lineId,
                                            string cumulusKey, string description, decimal sumPercentage, decimal premiumPercentage)
        {
            try
            {
                int tempFacultativeId = 0;
                tempFacultativeId = DelegateService.reinsuranceService.LoadFacultative(tempReinsuranceProcessId, layerNumber,
                                     lineId, cumulusKey, description, sumPercentage, premiumPercentage, SessionHelper.GetUserId());
                return new UifJsonResult(true, tempFacultativeId);
            }
            catch (Exception)
            {
                // Error generado al interior del SP                
                return new UifJsonResult(false, Language.ErrorLoadFacultative);
            }
        }

        /// <summary>
        /// CalculationValue
        /// </summary>
        /// <param name="tempReinsuranceProcessId"></param>
        /// <param name="layerNumber"></param>
        /// <param name="lineId"></param>
        /// <param name="cumulusKey"></param>
        /// <param name="sumPercentage"></param>
        /// <param name="premiumPercentage"></param>
        /// <returns>ActionResult</returns>
        public ActionResult CalculationValue(int tempReinsuranceProcessId, int? layerNumber, int? lineId,
                                            string cumulusKey, double sumPercentage, double premiumPercentage)
        {
            try
            {
                List<ReinsuranceAllocationDTO> calculationValues = new List<ReinsuranceAllocationDTO>();
                decimal[] values = DelegateService.reinsuranceService.CalculationValue(tempReinsuranceProcessId, layerNumber, lineId, cumulusKey);

                calculationValues.Add(new ReinsuranceAllocationDTO
                {
                    TotalSum = Convert.ToDecimal((Convert.ToDouble(values[0]) * sumPercentage) / 100),
                    TotalPremium = Convert.ToDecimal((Convert.ToDouble(values[1]) * premiumPercentage) / 100)
                });

                return new UifJsonResult(true, calculationValues);
            }
            catch (Exception)
            {
                // Error generado al interior del SP
                return new UifJsonResult(false, Language.ErrorCalculationValue);
            }
        }

        /// <summary>
        /// GetTempFacultativeCompanies
        /// Recupera los temporales del facultativo
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="layerNumber"></param>
        /// <param name="lineId"></param>
        /// <param name="cumulusKey"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetTempFacultativeCompanies(int endorsementId, int layerNumber, int lineId, string cumulusKey)
        {
            try
            {
                List<TempFacultativeCompaniesDTO> tempFacultativeCompaniesDTOs = new List<TempFacultativeCompaniesDTO>();
                tempFacultativeCompaniesDTOs = DelegateService.reinsuranceService.GetTempFacultativeCompaniesByEndorsementId(endorsementId, layerNumber, lineId, cumulusKey);
                return new UifJsonResult(true, tempFacultativeCompaniesDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorGetTempFacultativeCompanies);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetTempFacultativeCompanies);
            }
        }

        /// <summary>
        /// SaveTempFacultativeCompany
        /// Graba el temporal de facultativo de compañias
        /// </summary>
        /// <param name="reinsuranceFacultative"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveTempFacultativeCompany(ReinsuranceFacultativeDTO reinsuranceFacultative,
                                int endorsementId, int layerNumber, int lineId, string cumulusKey)
        {
            try
            {
                List<FacultativeCompaniesDTO> facultativeCompaniesDTOs = new List<FacultativeCompaniesDTO>();
                facultativeCompaniesDTOs = DelegateService.reinsuranceService.SaveTempFacultativeCompanyByLineId(reinsuranceFacultative,
                                            endorsementId, layerNumber, lineId, cumulusKey);
                return new UifJsonResult(true, facultativeCompaniesDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorSaveTempFacultativeCompany);
            }
        }

        /// <summary>
        /// GetTempFacultativePayment
        /// </summary>
        /// <param name="levelCompanyId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTempFacultativePayment(int levelCompanyId)
        {
            try
            {
                List<PlanFacultativeDTO> planFacultativeDTOs = new List<PlanFacultativeDTO>();
                planFacultativeDTOs = DelegateService.reinsuranceService.GetTempFacultativePayment(levelCompanyId);
                return Json(planFacultativeDTOs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetTempFacultativePayment);
            }
        }

        /// <summary>
        /// SavePaymentPlanFacultative
        /// </summary>
        /// <param name="tmpFacultativeCompanyCode"></param>
        /// <param name="feeNumber"></param>
        /// <param name="PaymentDate"></param>
        /// <param name="PaymentAmount"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SavePaymentPlanFacultative(int tmpFacultativeCompanyCode, int feeNumber, string paymentDate, decimal paymentAmount)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.SavePaymentPlanFacultative(tmpFacultativeCompanyCode, feeNumber, paymentDate, paymentAmount);
                return new UifJsonResult(result, null);
            }
            catch (Exception)
            {
                // Error generado al interior del SP
                return new UifJsonResult(false, Language.ErrorSavePaymentPlanFacultative);
            }
        }
        /// <summary>
        /// DeletePlanFacultative
        /// </summary>
        /// <param name="facultativePaymentsId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeletePlanFacultative(int facultativePaymentsId, int facultativeCompanyId)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.DeletePlanFacultative(facultativePaymentsId, facultativeCompanyId);
                return new UifJsonResult(result, null);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorDeletePlanFacultative);
            }
        }

        public ActionResult GetCompanyIdByFacultativeIdAndIndividualId(int facultativeId, int individualId)
        {
            try
            {
                int result = 0;
                result = DelegateService.reinsuranceService.GetReinsuranceCompanyIdByFacultativeIdAndIndividualId(facultativeId, individualId);
                return new UifJsonResult(true, result);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorGetCompanyIdByContractLevelIdAndIndividualId);
            }
        }

        /// <summary>
        /// IssGetSlips
        /// Devuelve los slips 
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetSlips(int processId, int endorsementId)
        {
            try
            {
                List<SlipDTO> slipDTOs = new List<SlipDTO>();
                slipDTOs = DelegateService.reinsuranceService.GetSlips(processId, endorsementId);
                return new UifJsonResult(true, slipDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorLoadFacultative);
            }
        }

        public JsonResult ExpandFacultative(int processId, int endorsementId, int layerNumber, int facultativeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.reinsuranceService.ExpandFacultative(processId, endorsementId, layerNumber, facultativeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorLoadFacultative);
            }
        }

        #endregion

        #endregion

        #region ReinsuranceClaims

        public ActionResult GetClaims(int branchCode, int prefixCode, decimal policyNumber, int? claimNumber)
        {
            try
            {
                List<ClaimDTO> claimDTOs = DelegateService.reinsuranceService.GetClaims(branchCode, prefixCode, policyNumber, claimNumber);
                return new UifJsonResult(true, claimDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorGetClaims);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetClaims);
            }
        }
        
        public ActionResult GetReinsuranceClaimDistributionByClaimCodeClaimModifyCode(int claimCode, int claimModifyCode, int movementSourceId, int claimCoverageCd)
        {
            try
            {
                List<ClaimDistributionDTO> claimDistributionDTO = new List<ClaimDistributionDTO>();
                claimDistributionDTO = DelegateService.reinsuranceService.GetReinsuranceClaimDistributionByClaimCodeClaimModifyCode(claimCode, claimModifyCode, movementSourceId, claimCoverageCd);
                return new UifJsonResult(true, claimDistributionDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetReinsuranceClaims);
            }
        }

        public ActionResult ReinsuranceClaim(int claimId, int claimModifyId)
        {
            try
            {
                ReinsuranceDTO reinsuranceDTO = new ReinsuranceDTO();
                reinsuranceDTO = DelegateService.reinsuranceService.ReinsuranceClaim(claimId, claimModifyId, SessionHelper.GetUserId());
                return new UifJsonResult(true, reinsuranceDTO);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult ManualClaimReinsurance(int claimId, int claimModifyId)
        {
            try
            {
                int processId = 0;
                processId = DelegateService.reinsuranceService.ManualClaimReinsurance(claimId, claimModifyId, SessionHelper.GetUserId());
                return new UifJsonResult(true, processId);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetClaimAllocations(int processId, int movementSourceId, int claimCoverageId)
        {
            try
            {
                List<ClaimAllocationDTO> claimAllocationsDTOs = new List<ClaimAllocationDTO>();
                claimAllocationsDTOs = DelegateService.reinsuranceService.GetClaimAllocationByMovementSource(processId, movementSourceId, claimCoverageId);
                return new UifJsonResult(true, claimAllocationsDTOs);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult ModificationReinsuranceClaimLineDialog(int processId, int sourceId, int layerNumber, int lineId, string cumulusKey, string contract, int levelNumber, string movementSource, decimal amount, string variance)
        {
            try
            {

                ViewData["tempReinsuranceProcessId"] = processId;
                ViewData["tempClaimReinsSourceId"] = sourceId;
                ViewData["layerNumber"] = layerNumber;
                ViewData["lineId"] = lineId;
                ViewData["cumulusKey"] = cumulusKey;
                ViewData["contract"] = contract;
                ViewData["levelNumber"] = levelNumber;
                ViewData["movementSource"] = movementSource;
                ViewData["newAmount"] = amount.ToString().Replace(',', '.');
                ViewData["sourceAmount"] = amount.ToString().Replace(',', '.');
                ViewData["variance"] = variance;

                return PartialView("../Reinsurance/Process/ModificationReinsuranceClaimLineDialog");
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorModificationReinsuranceClaimLineDialog);
            }
        }


        public ActionResult ModificationReinsuranceClaim(int tempClaimReinsSourceId, decimal newAmount)
        {
            try
            {
                DelegateService.reinsuranceService.ModificationReinsuranceClaim(tempClaimReinsSourceId, newAmount);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorModificationReinsuranceClaimLine);
            }
        }

        public ActionResult SaveClaimReinsurance(int processId, int claimCode, int claimModifyCode)
        {
            try
            {
                int reinsuranceId = 0;
                reinsuranceId = DelegateService.reinsuranceService.SaveClaimReinsurance(processId, claimCode, claimModifyCode, SessionHelper.GetUserId());

                if (reinsuranceId > 0)
                {
                    return new UifJsonResult(true, reinsuranceId);
                }
                else
                {
                    return new UifJsonResult(false, Language.ErrorSaveClaimReinsurance);
                }
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorSaveClaimReinsurance);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorSaveClaimReinsurance);
            }
        }

        #endregion ReinsuranceClaims

        #region ReinsurancePayment

        public ActionResult GetPaymentsRequest(int branchCode, int prefixCode, int policyNumber, int claimNumber, int? paymentRequestNumber)
        {
            try
            {
                List<PaymentRequestDTO> paymentRequestDTOs = new List<PaymentRequestDTO>();
                paymentRequestDTOs = DelegateService.reinsuranceService.GetPaymentsRequest(branchCode, prefixCode, policyNumber, claimNumber, paymentRequestNumber);
                return new UifJsonResult(true, paymentRequestDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorGetRequestPayments);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetRequestPayments);
            }
        }

        public ActionResult GetReinsurancePaymentDistributionsByPaymentRequestId(int paymentRequestId, int movementSourceId, int voucherConceptCd, int claimCoverageCd)
        {
            try
            {
                List<PaymentDistributionDTO> paymentDistributionDTOs = new List<PaymentDistributionDTO>();
                paymentDistributionDTOs = DelegateService.reinsuranceService.GetReinsurancePaymentDistributionsByPaymentRequestId(paymentRequestId, movementSourceId, voucherConceptCd, claimCoverageCd);
                return new UifJsonResult(true, paymentDistributionDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetReinsurancePayments);
            }
        }

        public ActionResult ReinsurancePayment(int paymentRequestId)
        {
            try
            {
                ReinsuranceDTO reinsuranceDTO = new ReinsuranceDTO();
                reinsuranceDTO = DelegateService.reinsuranceService.ReinsurancePayment(paymentRequestId, SessionHelper.GetUserId());
                return new UifJsonResult(true, reinsuranceDTO);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult ManualPaymentReinsurance(int paymentRequestId)
        {
            try
            {
                int processId = 0;
                processId = DelegateService.reinsuranceService.ManualPaymentReinsurance(paymentRequestId, SessionHelper.GetUserId());
                return new UifJsonResult(true, processId);
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult SavePaymentReinsurance(int processId, int paymentRequestId)
        {
            try
            {
                int reinsuranceId = 0;
                reinsuranceId = DelegateService.reinsuranceService.SavePaymentReinsurance(processId, paymentRequestId, SessionHelper.GetUserId());

                if (reinsuranceId > 0)
                {
                    return new UifJsonResult(true, reinsuranceId);
                }
                else
                {
                    return new UifJsonResult(false, Language.ErrorSaveReinsurancePayment);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorSaveReinsurancePayment);
            }
        }

        public ActionResult GetPaymentAllocations(int processId, int movementSourceId, int voucherConceptCd, int claimCoverageCd)
        {
            try
            {
                List<PaymentAllocationDTO> paymentAllocationDTOs = new List<PaymentAllocationDTO>();
                paymentAllocationDTOs = DelegateService.reinsuranceService.GetPaymentAllocationByMovementSource(processId, movementSourceId, voucherConceptCd, claimCoverageCd);
                return new UifJsonResult(true, paymentAllocationDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetPaymentAllocations);
            }
        }

        public ActionResult ModificationReinsurancePaymentLineDialog(int processId, int sourceId, int lineId, string description,
                                                                     string cumulusKey, int layerNumber, string smallDescription,
                                                                     int levelNumber, decimal amount, string variance)
        {
            try
            {
                ViewData["tempReinsuranceProcessId"] = processId;
                ViewData["tempPaymentReinsSourceId"] = sourceId;
                ViewData["lineId"] = lineId;
                ViewData["description"] = description;
                ViewData["cumulusKey"] = cumulusKey;
                ViewData["layerNumber"] = layerNumber;
                ViewData["smallDescription"] = smallDescription;
                ViewData["levelNumber"] = levelNumber;
                ViewData["newAmount"] = amount.ToString().Replace(',', '.');
                ViewData["sourceAmount"] = amount.ToString().Replace(',', '.');
                ViewData["variance"] = variance;
                return PartialView("../Reinsurance/Process/ModificationReinsurancePaymentLineDialog");
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorModificationReinsurancePaymentLineDialog);
            }
        }

        public ActionResult ModificationReinsurancePayment(int tempPaymentReinsSourceId, decimal newAmount)
        {
            try
            {
                DelegateService.reinsuranceService.ModificationReinsurancePayment(tempPaymentReinsSourceId, newAmount);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorModificationReinsurancePaymentLine);
            }
        }

        public ActionResult GetPaymentLayerByPaymentRequestId(int paymentRequestId, int voucherConceptCd, int claimCoverageCd)
        {
            try
            {
                List<ReinsurancePaymentLayerDTO> reinsurancePaymentLayerDTOs = new List<ReinsurancePaymentLayerDTO>();
                reinsurancePaymentLayerDTOs = DelegateService.reinsuranceService.GetPaymentLayerByPaymentRequestId(paymentRequestId, voucherConceptCd, claimCoverageCd);
                return new UifJsonResult(true, reinsurancePaymentLayerDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorGetPaymentLayerByPayment);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetPaymentLayerByPayment);
            }
        }

        public ActionResult GetDistributionPaymentByPaymentLayerId(int paymentLayerId)
        {
            try
            {
                List<ReinsurancePaymentDistributionDTO> reinsurancePaymentDistributionDTOs = new List<ReinsurancePaymentDistributionDTO>();
                reinsurancePaymentDistributionDTOs = DelegateService.reinsuranceService.GetDistributionPaymentByPaymentLayerId(paymentLayerId);
                return new UifJsonResult(true, reinsurancePaymentDistributionDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorGetDistributionPaymentByPaymentLayer);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetDistributionPaymentByPaymentLayer);
            }
        }
        #endregion ReinsurancePayment

        #region FindReinsurance

        /// <summary>
        /// DeleteReinsurance
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <param name="endorsementNumber"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteReinsurance(decimal documentNumber, int endorsementNumber)
        {
            try
            {
                bool result = false;
                result = DelegateService.reinsuranceService.DeleteReinsurance(documentNumber, endorsementNumber);
                return new UifJsonResult(true, result);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Global.ErrorDeleteReinsurance);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Global.ErrorDeleteReinsurance);
            }
        }

        /// <summary>
        /// GetReinsuranceByEndorsement
        /// Método que implementa la busqueda reaseguros para un endoso.
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetReinsuranceByEndorsement(int endorsementId)
        {
            try
            {
                List<EndorsementReinsuranceDTO> endorsementReinsuranceDTOs = new List<EndorsementReinsuranceDTO>();
                endorsementReinsuranceDTOs = DelegateService.reinsuranceService.GetReinsuranceByEndorsementId(endorsementId);
                return new UifJsonResult(true, endorsementReinsuranceDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorGetReinsuranceByEndorsement);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetReinsuranceByEndorsement);
            }
        }

        /// <summary>
        /// GetDistributionByReinsurance
        /// Método que implementa la busqueda distribuciones para un reaseguro.
        /// </summary>
        /// <param name="layerId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetDistributionByReinsurance(int layerId)
        {
            try
            {
                List<ReinsuranceDistributionDTO> reinsuranceDistributionDTOs = new List<ReinsuranceDistributionDTO>();
                reinsuranceDistributionDTOs = DelegateService.reinsuranceService.GetDistributionByReinsuranceByLayerId(layerId);
                return new UifJsonResult(true, reinsuranceDistributionDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorGetDistributionByReinsurance);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetDistributionByReinsurance);
            }
        }
        
        public ActionResult GetClaimLayerByClaimIdClaimModifyId(int claimId, int claimModifyId, int movementSourceId, int claimCoverageCd)
        {
            try
            {
                List<ReinsuranceClaimLayerDTO> reinsuranceClaimLayerDTOs = new List<ReinsuranceClaimLayerDTO>();
                reinsuranceClaimLayerDTOs = DelegateService.reinsuranceService.GetClaimLayerByClaimIdClaimModifyId(claimId, claimModifyId, movementSourceId, claimCoverageCd);
                return new UifJsonResult(true, reinsuranceClaimLayerDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorGetClaimLayerByClaim);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetClaimLayerByClaim);
            }
        }

        public ActionResult GetDistributionClaimByClaimLayerId(int claimLayerId, int movementSourceId)
        {
            try
            {
                List<ReinsuranceClaimDistributionDTO> reinsuranceClaimDistributionDTOs = new List<ReinsuranceClaimDistributionDTO>();
                reinsuranceClaimDistributionDTOs = DelegateService.reinsuranceService.GetDistributionClaimByClaimLayerId(claimLayerId, movementSourceId);
                return new UifJsonResult(true, reinsuranceClaimDistributionDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorGetDistributionClaimByClaimLayer);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetDistributionClaimByClaimLayer);
            }
        }
        #endregion FindReinsurance


        #region ExportsProcessMassiveDetails

        /// <summary>
        /// ExportReinsuranceProcessDetail
        /// </summary>
        /// <param name="tempReinsuranceProcessId"></param>
        /// <returns>JsonResult</returns>
        public UifJsonResult ExportReinsuranceProcessDetail(int tempReinsuranceProcessId)
        {
            try
            {
                string fileName = Global.RptDistributionErrorMassive + " - " + tempReinsuranceProcessId;
                List<TempReinsuranceProcessDTO> tempReinsuranceProcess = new List<TempReinsuranceProcessDTO>();
                tempReinsuranceProcess = DelegateService.reinsuranceService.GetTempReinsuranceProcessDetails(tempReinsuranceProcessId);
                ExportToExcelReinsuranceProcessDetail(tempReinsuranceProcess, fileName + ".xls");
                return new UifJsonResult(true, fileName);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorExportReinsuranceProcessDetail);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorExportReinsuranceProcessDetail);
            }
        }

        /// <summary>
        /// ExportToExcelReinsuranceProcessDetail
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fileName"></param>
        private void ExportToExcelReinsuranceProcessDetail(List<TempReinsuranceProcessDTO> list, string fileName)
        {
            StringWriter sw = new StringWriter();
            string tab = "\t";

            sw.WriteLine("PROCESO REASEGURO MASIVO DETALLE DE ERRORES");
            // Se escribe la cabecera
            sw.WriteLine(Global.ProcessId + tab + Global.Branch + tab + Global.Prefix + tab + Global.PolicyNumber + tab +
                        Global.EndorsementNumber + tab + Global.RiskNumber + tab + Global.CoverageNumber + tab + Global.ErrorDescription);

            foreach (TempReinsuranceProcessDTO dto in list)
            {
                // Se escribe el detalle
                sw.WriteLine(string.Format("{0}" + tab + "{1}" + tab + "{2}" + tab + "{3}" + tab + "{4}" + tab +
                                           "{5}" + tab + "{6}" + tab + "{7}" + tab,
                                           dto.TempReinsuranceProcessId,
                                           dto.BranchDescription,
                                           dto.PrefixDescription,
                                           dto.PolicyNumber,
                                           dto.EndorsementNumber,
                                           dto.RiskNumber,
                                           dto.CoverageNumber,
                                           dto.ErrorDescription
                                           ));
            }

            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            Response.Write(sw);
            Response.End();
        }

        /// <summary>
        /// LoadProcessMassiveDetailsReport
        /// </summary>
        /// <param name="tempReinsuranceProcessId"></param>
        public void LoadProcessMassiveDetailsReport(int tempReinsuranceProcessId)
        {
            List<TempReinsuranceProcessDTO> tempReinsuranceProcessDTOs = new List<TempReinsuranceProcessDTO>();
            tempReinsuranceProcessDTOs = DelegateService.reinsuranceService.LoadProcessMassiveDetailsReport(tempReinsuranceProcessId);
            this.HttpContext.Session["processMassiveDetails"] = tempReinsuranceProcessDTOs;
            this.HttpContext.Session["processMassiveDetailsReportName"] = "Areas//Reinsurance//Reports//ProcessMassiveDetails.rpt";
        }

        /// <summary>
        /// ShowProcessMassiveDetailsReport
        /// Visualiza el reporte en formato pdf 
        /// </summary>
        public void ShowProcessMassiveDetailsReport()
        {
            try
            {
                bool isValid = true;

                var rptSource = System.Web.HttpContext.Current.Session["processMassiveDetails"];
                var strReportName = System.Web.HttpContext.Current.Session["processMassiveDetailsReportName"];

                if (rptSource == null)
                {
                    isValid = false;
                }

                if (isValid)
                {
                    var rd = new ReportDocument();

                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + strReportName;

                    rd.Load(strRptPath);
                    // Lena Reporte Principal
                    rd.SetDataSource(rptSource);

                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, true, "ProcessMassiveDetails");

                    // Clear all sessions value
                    Session["processMassiveDetails"] = null;
                    Session["processMassiveDetailsReportName"] = null;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        #endregion

        #region ReportsExport

        /// <summary>
        /// GetTypeReport
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetTypeReport()
        {
            try
            {
                List<ReportTypeDTO> reportTypeDTOs = new List<ReportTypeDTO>();
                reportTypeDTOs = DelegateService.reinsuranceReportService.GetReportTypes();
                return new UifSelectResult(reportTypeDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetTypeReport);
            }
        }


        #endregion

        #region Reporte de Cierres

        /// <summary>
        /// ClosureReport
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public ActionResult ClosureReport(int year, int month, int reportType)
        {
            try
            {
                string result = "";
                result = DelegateService.reinsuranceReportService.ClosureReport(year, month, reportType, SessionHelper.GetUserId());
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorClosureReport);
            }
        }


        /// <summary>
        /// GetTotalRecordsMassiveReport
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="reportType"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetTotalRecordsMassiveReport(string dateFrom, string dateTo, int reportType)
        {
            try
            {
                decimal result = 0;
                result = DelegateService.reinsuranceReportService.GetTotalRecordsMassiveReport(dateFrom, dateTo, reportType);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorPreviusProcessReports);
            }
        }

        /// <summary>
        /// ReinsuranceReports
        /// Ejecuta el proceso de generacion de datos, guardando en una tabla temporal
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public UifJsonResult ReinsuranceReports(string dateFrom, string dateTo, int reportType)
        {
            try
            {
                var result = DelegateService.reinsuranceReportService.ReinsuranceReports(dateFrom, dateTo, reportType, SessionHelper.GetUserId());
                return new UifJsonResult(true, result);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Global.ErrorStoreProcedure);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Global.ErrorStoreProcedure);
            }

        }
        /// <summary>
        /// GetMassiveReportProcess
        /// </summary>
        /// <param name="reportName"></param>
        /// <returns>JsonResult</returns>
        public ActionResult GetMassiveReportProcess(string reportName)
        {
            try
            {
                List<MassiveReportDTO> massiveReportDTOs = new List<MassiveReportDTO>();
                massiveReportDTOs = DelegateService.reinsuranceReportService.GetMassiveReportProcess(reportName, SessionHelper.GetUserId());
                return new UifJsonResult(true, massiveReportDTOs);
            }
            catch (BusinessException)
            {
                return new UifJsonResult(false, Language.ErrorGetMassiveReportProcess);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetMassiveReportProcess);
            }
        }

        /// <summary>
        /// GenerateStructureReport
        /// Crea el archivo y guarda en el servidor(metodo Asicrono no tiene pruebas Test)
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="reportTypeDescription"></param>
        /// <param name="exportFormatType"></param>
        /// <param name="recordsNumber"></param>
        /// <returns>JsonResult</returns>
        public ActionResult GenerateStructureReport(int processId, string reportTypeDescription, int exportFormatType, decimal recordsNumber)
        {
            try
            {
                var result = DelegateService.reinsuranceReportService.GenerateStructureReport(processId, reportTypeDescription, exportFormatType, recordsNumber, SessionHelper.GetUserId());
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Global.ErrorGeneratingFile);
            }
        }

        /// <summary>
        /// GetFileTypeList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetFileTypeList()
        {
            try
            {
                List<SelectDTO> fileTypes = new List<SelectDTO>();
                fileTypes.Add(new SelectDTO { Id = 2, Description = FileTypes.Excel.ToString() });
                return new UifSelectResult(fileTypes);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetFileTypeList);
            }
        }

        #endregion

        #region GeneralLedger

        /// <summary>
        /// RecordReinsuranceEntry
        /// Método de contabilización de ingreso de caja con parametrización dinámica
        /// </summary>
        /// <param name="processId"></param>
        /// <returns>string</returns>
        public string RecordReinsuranceEntry(int processId)
        {
            try
            {
                return DelegateService.reinsuranceReportService.RecordReinsuranceEntry(processId, SessionHelper.GetUserId());
            }
            catch (Exception)
            {
                return Language.ErrorRecordReinsuranceEntry;
            }
        }

        #endregion GeneralLedger

        #region Private Methods

        /// <summary>
        /// LoadClosingDateReinsurance
        /// </summary>
        /// <returns>DateTime</returns>
        private DateTime LoadClosingDateReinsurance()
        {
            ModuleDateDTO moduleDateDTO = new ModuleDateDTO();
            moduleDateDTO.Id = Convert.ToInt32(ConfigurationManager.AppSettings["ReinsuranceModule"]);
            moduleDateDTO = DelegateService.reinsuranceService.GetModuleDate(moduleDateDTO);
            DateTime closeDate = new DateTime(moduleDateDTO.LastClosingYyyy, moduleDateDTO.LastClosingMm, DateTime.DaysInMonth(moduleDateDTO.LastClosingYyyy, moduleDateDTO.LastClosingMm));
            return closeDate;
        }

        /// <summary>
        /// GetBranchDefaultByUserId
        /// Obtiene la sucursal por defecto del usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns>int</returns>
        private int GetBranchDefaultByUserId(int userId, int type)
        {
            if (type == 0)
            {
                return DelegateService.reinsuranceService.GetBranchesByUserId(userId).Where(br => br.IsDefault).ToList()[0].Id;
            }

            return DelegateService.reinsuranceService.GetBranchesByUserId(userId).Count;
        }
        #endregion

        #region QueryCumulus

        /// <summary>
        /// Inicializa la vista de Query Cumulus
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryCumulus()
        {
            return View("~/Areas/Reinsurance/Views/Reinsurance/Process/QueryCumulus.cshtml");
        }

        /// <summary>
        /// GetLineBusiness
        /// </summary>
        /// <returns>JsonResult</returns>
        public UifJsonResult GetLineBusiness()
        {
            try
            {
                List<LineBusinessDTO> lineBusinessDTOs = new List<LineBusinessDTO>();
                lineBusinessDTOs = DelegateService.reinsuranceService.GetLineBusiness();
                return new UifJsonResult(true, lineBusinessDTOs);
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
        public UifJsonResult GetSubLineBusiness(int lineBusiness)
        {
            try
            {
                List<SubLineBusinessDTO> lineBusinessDTOs = new List<SubLineBusinessDTO>();
                lineBusinessDTOs = DelegateService.reinsuranceService.GetSubLineBusiness(lineBusiness);
                return new UifJsonResult(true, lineBusinessDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetSubLineBusiness);
            }
        }

        public JsonResult GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string query)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.reinsuranceService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(query, Core.Services.UtilitiesServices.Enums.InsuredSearchType.DocumentNumber, Core.Services.UtilitiesServices.Enums.CustomerType.Individual));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetInsuredsByDescriptionInsuredSearchTypeCustomerType);
            }
        }

        public UifJsonResult GetCumulusByIndividual(int individualId, int lineBusiness, DateTime cumulusDate, bool isFuture, int subLineBusiness, int PrefixCd)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.reinsuranceService.GetCumulusByIndividual(individualId, lineBusiness, cumulusDate, isFuture, subLineBusiness, PrefixCd));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetInsuredsByDescriptionInsuredSearchTypeCustomerType);
            }
        }

        public UifJsonResult GetCumulusDetailByIndividual(int individualId, int lineBusiness, DateTime cumulusDate, bool isFuture, int subLineBusiness, int PrefixCd)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.reinsuranceService.GetCumulusDetailByIndividual(individualId, lineBusiness, cumulusDate, isFuture, subLineBusiness, PrefixCd));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetInsuredsByDescriptionInsuredSearchTypeCustomerType);
            }
        }

        public UifJsonResult GetDetailCumulusParticipantsEconomicGroup(int economicGroupId, int lineBusiness, DateTime cumulusDate, bool isFuture, int subLineBusiness, int prefixCd)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.reinsuranceService.GetDetailCumulusParticipantsEconomicGroup(economicGroupId, lineBusiness, cumulusDate, isFuture, subLineBusiness, prefixCd));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorGetInsuredsByDescriptionInsuredSearchTypeCustomerType);
            }
        }

        public UifJsonResult GenerateFileCumulusByIndividual(string filename, List<CoverageReinsuranceCumulusDTO> coverageReinsuranceCumulusDTOs)
        {
            try
            {
                string urlFile = DelegateService.reinsuranceService.GenerateFileCumulusByIndividual(filename, coverageReinsuranceCumulusDTOs);
                if (urlFile == "")
                {
                    return new UifJsonResult(false, Language.ErrorThereIsNoDataToExport);
                }
                return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, Language.ErrorExportExcel);
            }
        }

        #endregion

    }
}