using System;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;


namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    public class BranchAccountingConceptController : Controller
    {
        #region View

        /// <summary>
        /// MainBranchAccountingConceptController
        /// Llamada a View
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainBranchAccountingConcept()
        {
            try
            {

                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// GetBranchAccountingConceptsByBranch
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetBranchAccountingConceptsByBranch(int branchId)
        {
            BranchDTO branch = new BranchDTO();
            branch.Id = branchId;

            var branchAccountingConcepts = DelegateService.glAccountingApplicationService.GetBranchAccountingConceptByBranch(branch);

            var jsonData = from item in branchAccountingConcepts
                           select new
                           {
                               Id = item.Id,
                               AccountingConcept = DelegateService.glAccountingApplicationService.GetAccountingConcept(item.AccountingConcept),
                               MovementType = DelegateService.glAccountingApplicationService.GetMovementType(item.MovementType),
                               BranchId = item.Branch.Id

                           };


            return new UifTableResult(jsonData);
        }

        /// <summary>
        /// GetUserBranchAccountingConceptByUserByBranch
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="branchId"></param>
        /// <param name="conceptSourceId"></param>
        /// <param name="movementTypeId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetUserBranchAccountingConceptByUserByBranch(int userId, int branchId, int conceptSourceId, int movementTypeId)
        {
            BranchDTO branch = new BranchDTO() { Id = branchId };

            var userBranchAccountingConcepts = DelegateService.glAccountingApplicationService.GetUserBranchAccountingConceptByUserByBranch(userId, branch);

            var jsonData = from item in userBranchAccountingConcepts
                           where item.BranchAccountingConcept.MovementType.ConceptSource.Id == conceptSourceId && item.BranchAccountingConcept.MovementType.Id == movementTypeId
                           select new
                           {
                               UserBranchId = item.Id,
                               BranchAccountingConceptId = item.BranchAccountingConcept.Id,
                               AccountingConcept = DelegateService.glAccountingApplicationService.GetAccountingConcept(item.BranchAccountingConcept.AccountingConcept),
                               MovementType = DelegateService.glAccountingApplicationService.GetMovementType(item.BranchAccountingConcept.MovementType),
                               UserId = item.UserId,
                               AccountingAccountNumber = item.BranchAccountingConcept.AccountingConcept.AccountingAccount.Number
                           };

            return new UifTableResult(jsonData);
        }

        /// <summary>
        /// GetMovementTypesByConceptSourceId
        /// </summary>
        /// <param name="conceptSourceId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetMovementTypesByConceptSourceId(int conceptSourceId)
        {
            ConceptSourceDTO conceptSource = new ConceptSourceDTO() { Id = conceptSourceId };
            return new UifSelectResult(DelegateService.glAccountingApplicationService.GetMovementTypesByConceptSource(conceptSource));
        }


        /// <summary>
        /// SaveBranchAccountingConcept
        /// Guardar dos tablas de relacion con sucursal y usuarios
        /// </summary>
        /// <param name="branchAccountingConceptModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveBranchAccountingConcept(BranchAccountingConceptModel branchAccountingConceptModel)
        {

            bool isSucessfully = false;
            int saved = 0;
            int saved2 = 0;
            var message = "";
            BranchAccountingConceptDTO branchAccountingConcept = new BranchAccountingConceptDTO();

            UserBranchAccountingConceptDTO userBranchAccountingConcept = new UserBranchAccountingConceptDTO();

            try
            {
                branchAccountingConcept.Id = branchAccountingConceptModel.Id;
                branchAccountingConcept.Branch = new BranchDTO();
                branchAccountingConcept.Branch.Id = branchAccountingConceptModel.BranchId;
                branchAccountingConcept.MovementType = new MovementTypeDTO();
                branchAccountingConcept.MovementType.Id = branchAccountingConceptModel.MovementTypeId;
                branchAccountingConcept.MovementType.ConceptSource = new ConceptSourceDTO();
                branchAccountingConcept.MovementType.ConceptSource.Id = branchAccountingConceptModel.ConceptSourceId;
                if (branchAccountingConceptModel.AccountingConceptModels != null)
                {
                    for (int i = 0; i < branchAccountingConceptModel.AccountingConceptModels.Count; i++)
                    {
                        branchAccountingConcept.AccountingConcept = new AccountingConceptDTO();
                        branchAccountingConcept.AccountingConcept.Id = branchAccountingConceptModel.AccountingConceptModels[i].Id;

                        branchAccountingConcept = DelegateService.glAccountingApplicationService.SaveBranchAccountingConcept(branchAccountingConcept);
                        saved = branchAccountingConcept.Id;

                        if (branchAccountingConcept.Id > 0)
                        {
                            userBranchAccountingConcept.Id = 0;
                            userBranchAccountingConcept.BranchAccountingConcept = branchAccountingConcept;
                            userBranchAccountingConcept.UserId = branchAccountingConceptModel.UserId;

                            userBranchAccountingConcept = DelegateService.glAccountingApplicationService.SaveUserBranchAccountingConcept(userBranchAccountingConcept);
                            branchAccountingConcept.Id = 0;
                        }
                    }
                    isSucessfully = true;
                    saved2 = userBranchAccountingConcept.Id;
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
                isSucessfully = false;
                saved = 0;
                saved2 = 0;
            }
            return Json(new { success = isSucessfully, result = saved, result2 = saved2, msg = message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteBranchAccountingConcept
        /// </summary>
        /// <param name="userBranchAccountingConceptId"></param>
        /// <param name="branchAccountingConceptId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteBranchAccountingConcept(int userBranchAccountingConceptId, int branchAccountingConceptId)
        {
            bool isDeletedBranch = false;
            bool isDeletedUser = false;
            int deleted = 0;
            var message = "";

            try
            {
                UserBranchAccountingConceptDTO userBranchAccountingConcept = new UserBranchAccountingConceptDTO();
                userBranchAccountingConcept.Id = userBranchAccountingConceptId;

                isDeletedBranch = DelegateService.glAccountingApplicationService.DeleteUserBranchAccountingConcept(userBranchAccountingConcept);
                deleted = 1;
                if (isDeletedBranch)
                {
                    BranchAccountingConceptDTO branchAccountingConcept = new BranchAccountingConceptDTO();
                    branchAccountingConcept.Id = branchAccountingConceptId;
                    isDeletedUser = DelegateService.glAccountingApplicationService.DeleteBranchAccountingConcept(branchAccountingConcept);
                    if (isDeletedUser)
                    {
                        deleted = deleted + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isDeletedBranch = false;
                deleted = 0;
            }

            return Json(new { success = isDeletedBranch, result = deleted, msg = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}