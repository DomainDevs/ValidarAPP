using Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers
{
    public class ChargeController : Controller
    {
        // GET: Claims/Charge
        public ActionResult Charge()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el Origen de Pago
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPaymentSourcesByChargeRequest()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetPaymentSourcesByChargeRequest(true).OrderBy(x => x.Id));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPaymentSourcesByChargeRequest);
            }
        }

        /// <summary>
        /// Obtiene los Tipos de Movimientos
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPaymentMovementTypesByPaymentSourceId(int sourceId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetMovementTypesByConceptSourceId(sourceId).OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPaymentMovementTypesByPaymentSourceId);
            }
        }

        /// <summary>
        /// Obtiene la Sucursal de Cobro
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBranches()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetBranchesByUserId(SessionHelper.GetUserId()).OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranches);
            }
        }

        /// <summary>
        /// Obtiene la fecha de registro
        /// </summary>
        /// <returns></returns>
        public ActionResult GetModuleDateByModuleTypeMovementDate()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetModuleDateByModuleTypeMovementDate(ModuleType.Claim, DateTime.Today));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetModuleDateByModuleTypeMovementDate);
            }
        }

        /// <summary>
        /// Obtiene el Ramo
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPrefixes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetPrefixes().OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
            }
        }

        /// <summary>
        /// Obtiene los tipos de personas
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPersonTypes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetPersonTypes(false).OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPersonTypes);
            }
        }


        /// <summary>
        /// Obtiene los ids de los tipos de estimación para un tipo de movimiento
        /// </summary>
        /// <param name="movementTypeId"></param>
        /// <returns></returns>
        public ActionResult GetEstimationsTypesIdByMovementTypeId(int movementTypeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetEstimationsTypesIdByMovementTypeId(movementTypeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.Error);
            }
        }

        /// <summary>
        /// Obtiene los asegurados por Nombre o Número de documento
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public JsonResult GetInsuredsByNameDocumentNumber(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(query, InsuredSearchType.DocumentNumber, CustomerType.Individual), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetInsuredsByNameDocumentNumber, JsonRequestBehavior.DenyGet);
            }
        }

        /// <summary>
        /// Obtiene los recobros o salvamentos según sea el caso
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public JsonResult GetSuppliersByNameDocumentNumber(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetSuppliersByDescriptionInsuredSearchTypeCustomerType(query, InsuredSearchType.DocumentNumber, CustomerType.Individual), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetSuppliersByNameDocumentNumber, JsonRequestBehavior.DenyGet);
            }
        }

        /// <summary>
        /// Obtiene los recobros por Id de siniestro
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public JsonResult GetRecoveriesByClaim(int? branchId, int? prefixId, string policyDocumentNumber, int claimNumber)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.recoveryApplicationService.GetRecoveriesByClaim(prefixId, branchId, policyDocumentNumber, claimNumber));
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRecoveriesByClaim);
            }
        }

        /// <summary>
        /// Obtiene el recobro por su identificador
        /// </summary>
        /// <param name="recoveryId"></param>
        /// <returns></returns>
        public UifJsonResult GetRecoveryByRecoveryId(int recoveryId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.recoveryApplicationService.GetRecoveryByRecoveryId(recoveryId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRecoveryByRecoveryId);
            }
        }

        /// <summary>
        /// Obtiene los salvamentos por Id de siniestro
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="claimNumber"></param>
        /// <returns></returns>
        public ActionResult GetSalvagesByClaim(int? branchId, int? prefixId, string policyDocumentNumber, int claimNumber)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.salvageApplicationService.GetSalvagesByClaim(prefixId, branchId, policyDocumentNumber, claimNumber));
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSalvagesByClaim);
            }
        }

        /// <summary>
        /// Obtiene los salvamentos por Id de salvamento
        /// </summary>
        /// <param name="salvageId"></param>
        /// <returns></returns>
        public UifJsonResult GetSalvageBySalvageId(int salvageId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.salvageApplicationService.GetSalvageBySalvageId(salvageId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSalvageBySalvageId);
            }
        }

        /// <summary>
        /// Obtiene conceptos de pago
        /// </summary>
        /// <param name="brachId"></param>
        /// <param name="movementTypeId"></param>
        /// <returns></returns>
        public ActionResult GetPaymentConceptsByBranchIdMovementTypeId(int branchId, int movementTypeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(branchId, movementTypeId, 0, 0).OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPaymentConceptsByBranchIdMovementTypeId);
            }
        }

        /// <summary>
        /// Guarda la solicitud de cobro
        /// </summary>
        /// <param name="chargeRequestDTO"></param>
        /// <returns></returns>
        public ActionResult SaveChargeRequest(List<ChargeRequestDTO> chargeRequestsDTO)
        {
            try
            {
                chargeRequestsDTO.ForEach(x => { x.UserId = SessionHelper.GetUserId(); x.PaymentRequestTypeId = (int)PaymentRequestType.Recovery; x.RegistrationDate = DateTime.Now; });

                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.CreateChargeRequests(chargeRequestsDTO));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveChargeRequest);
            }
        }

        public UifJsonResult GetHolderByIndividualId(int individualId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(Convert.ToString(individualId), InsuredSearchType.IndividualId, CustomerType.Individual));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetInsuredsByDescriptionInsuredSearchTypeCustomerType);
            }
        }

        public UifJsonResult GetChargeRequestByChargeRequestId(int chargeRequestId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetChargeRequestByChargeRequestId(chargeRequestId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetChargeRequestByChargeRequestId);
            }
        }

        public UifJsonResult GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetSubClaimsEstimationByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimByPrefixIdBranchIdClaimNumber);
            }
        }

    }
}