using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Company.Application.UniquePersonParamService.Models;
using ACM = Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class ClaimsAssociationController : Controller
    {
        private static List<ACM.PaymentConcept> PaymentConcept = new List<ACM.PaymentConcept>();
        // GET: Parametrization/ClaimsAssociation
        public ActionResult ClaimsAssociation()
        {
            return View();
        }
        /// <summary>
        /// Obtiene la lista de sucursales
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public ActionResult GetAllAlliances()
        {
            try
            {
                List<Alliance> alliance = DelegateService.companyUniquePersonParamService.GetAllAlliances();
                List<AllianceViewModel> aliancesView = ModelAssembler.CreateAlliances(alliance);
                return new UifJsonResult(true, aliancesView.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }

        /// <summary>
        /// Busqueda de aliados por nombre
        /// </summary>
        /// <param name="description">Nombre del aliado</param>
        /// <returns>Aliado</returns>
        public ActionResult GetAllianceByDescription(string description)
        {
            try
            {
                List<Alliance> alliance = DelegateService.companyUniquePersonParamService.GetAllianceByDescription(description);
                List<AllianceViewModel> aliancesView = ModelAssembler.CreateAlliances(alliance);
                return new UifJsonResult(true, aliancesView.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }

        /// <summary>
        /// Guarda los cambio hechos en los aliados.
        /// </summary>
        /// <param name="lstAlliances">Liado de aliados (Modelo de vista)</param>
        /// <returns>Listado de aliados actualizado.</returns>
        public ActionResult SaveAlliances(List<AllianceViewModel> lstAlliances)
        {
            try
            {
                List<string> response = DelegateService.companyUniquePersonParamService.ExecuteOprationsAlliances(ModelAssembler.CreateAlliances(lstAlliances));

                List<Alliance> alliance = DelegateService.companyUniquePersonParamService.GetAllAlliances();
                List<AllianceViewModel> aliancesView = ModelAssembler.CreateAlliances(alliance);
                object[] result = new object[2];
                result[0] = response;
                result[1] = aliancesView.OrderBy(x => x.Description).ToList();
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error");
            }
        }

        /// <summary>
        /// Vista de busqueda
        /// </summary>
        /// <returns>Vista de busuqeda</returns>
        public ViewResult AllianceAdvancedSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Metodo GenerateFileToExport que genera archivo excel y lo retorna
        /// </summary>
        /// <returns>Excel de aliados</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                List<Alliance> alliance = DelegateService.companyUniquePersonParamService.GetAllAlliances();
                string urlFile = DelegateService.companyUniquePersonParamService.GenerateFileToAlliance(alliance, App_GlobalResources.Language.FileNameAlliance);
                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
        public UifJsonResult GetCoveragesByLineBusinessId(int lineBusinessId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetCoveragesByLineBusinessId(lineBusinessId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCoverage);
            }
        }

        public UifJsonResult GetEstimationsType()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetEstimationTypes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }

        public UifJsonResult GetLinesBusinessByPrefixId(int prefixId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetLinesBusinessByPrefixId(prefixId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLineBusiness);
            }
        }
        
        //public UifJsonResult GetPaymentConcepts()
        //{
        //    try
        //    {
        //        return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetPaymentConcepts());
        //    }
        //    catch (Exception)
        //    {
        //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPaymentConcept);
        //    }
        //}

        public UifJsonResult GetPaymentConceptsByCoverageIdEstimationTypeId( int coverageId, int estimationTypeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetPaymentConceptsByCoverageIdEstimationTypeId(coverageId, estimationTypeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPaymentConcept);
            }
        }

        public UifJsonResult CreatePaymentConcept(CoveragePaymentConceptDTO coveragePaymentConceptDTO)
        {
            try
            {
                DelegateService.claimApplicationService.CreatePaymentConcept(coveragePaymentConceptDTO);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateCauseCoverage);
            }
        }

        public UifJsonResult DeletePaymentConcept(int conceptId, int coverageId, int estimationTypeId)
        {
            try
            {
                DelegateService.claimApplicationService.DeletePaymentConcept(conceptId, coverageId, estimationTypeId);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteCauseCoverage);
            }
        }
    }
}