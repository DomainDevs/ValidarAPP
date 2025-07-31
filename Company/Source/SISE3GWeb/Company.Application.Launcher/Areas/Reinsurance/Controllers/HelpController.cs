//Sistran FWK
using Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Controllers
{
    public class HelpController : Controller
    {

        #region Views

        #region Param

        public ActionResult Parametrization()
        {
            return View("~/Areas/Reinsurance/Views/Help/Parameters/Parametrization.cshtml");
        }


        /// <summary>
        /// Contract
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Contract()
        {
            return View("~/Areas/Reinsurance/Views/Help/Parameters/Contract.cshtml");
        }

        /// <summary>
        /// ContractLevel
        /// </summary>
        /// <returns>View</returns>
        public ActionResult ContractLevel()
        {
            return View("~/Areas/Reinsurance/Views/Help/Parameters/ContractLevel.cshtml");
        }

        /// <summary>
        /// ContractLevel
        /// </summary>
        /// <returns>View</returns>
        public ActionResult CompanyLevel()
        {
            return View("~/Areas/Reinsurance/Views/Help/Parameters/CompanyLevel.cshtml");
        }

        /// <summary>
        /// Line
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Line()
        {
            return View("~/Areas/Reinsurance/Views/Help/Parameters/Line.cshtml");
        }

        /// <summary>
        /// ContractLine
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ContractLine()
        {
            return View("~/Areas/Reinsurance/Views/Help/Parameters/ContractLine.cshtml");
        }

        /// <summary>
        /// AssociationLine
        /// </summary>
        /// <returns>View</returns>
        public ActionResult AssociationLine()
        {
            return View("~/Areas/Reinsurance/Views/Help/Parameters/AssociationLine.cshtml");
        }

        #endregion

        #region Process

        public ActionResult Processes()
        {
            return View("~/Areas/Reinsurance/Views/Help/Process/Processes.cshtml");
        }

        /// <summary>
        /// ReinsurancePolicy
        /// </summary>
        /// <returns>View</returns>
        public ActionResult ReinsurancePolicy()
        {
            return View("~/Areas/Reinsurance/Views/Help/Process/ReinsurancePolicy.cshtml");
        }

        /// <summary>
        /// ModificationLayer
        /// </summary>
        /// <returns>View</returns>
        public ActionResult ModificationLayer()
        {
            return View("~/Areas/Reinsurance/Views/Help/Process/ModificationLayer.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns>View</returns>
        public ActionResult ModificationLine()
        {
            return View("~/Areas/Reinsurance/Views/Help/Process/ModificationLine.cshtml");
        }

        /// <summary>
        /// ModificationAllocation
        /// </summary>
        /// <param name=""></param>
        /// <returns>View</returns>  
        public ActionResult ModificationAllocation()
        {
            return View("~/Areas/Reinsurance/Views/Help/Process/ModificationAllocation.cshtml");
        }

        /// <summary>
        /// FacultativeContract
        /// </summary>
        /// <returns>View</returns>   
        public ActionResult FacultativeContract()
        {
            return View("~/Areas/Reinsurance/Views/Help/Process/FacultativeContract.cshtml");
        }

        /// <summary>
        /// ReinsuranceClaim
        /// </summary>
        /// <returns>View</returns> 
        public ActionResult ReinsuranceClaim()
        {
            return View("~/Areas/Reinsurance/Views/Help/Process/ReinsuranceClaim.cshtml");
        }

        /// <summary>
        /// ModificationReinsuranceClaim
        /// </summary>
        /// <returns>View</returns> 
        public ActionResult ModificationReinsuranceClaim()
        {
            return View("~/Areas/Reinsurance/Views/Help/Process/ModificationReinsuranceClaim.cshtml");
        }

        /// <summary>
        /// ReinsurancePayment
        /// </summary>
        /// <returns>View</returns> 
        public ActionResult ReinsurancePayment()
        {
            return View("~/Areas/Reinsurance/Views/Help/Process/ReinsurancePayment.cshtml");
        }

        /// <summary>
        /// ModificationReinsurancePayment
        /// </summary>
        /// <returns>View</returns> 
        public ActionResult ModificationReinsurancePayment()
        {
            return View("~/Areas/Reinsurance/Views/Help/Process/ModificationReinsurancePayment.cshtml");
        }

        /// <summary>
        /// SearchReinsurance
        /// </summary>
        /// <returns>View</returns> 
        public ActionResult SearchReinsurance()
        {
            return View("~/Areas/Reinsurance/Views/Help/Process/SearchReinsurance.cshtml");
        }

        /// <summary>
        /// ReinsurancePolicyMassive
        /// </summary>
        /// <returns>View</returns> 
        public ActionResult ReinsurancePolicyMassive()
        {
            return View("~/Areas/Reinsurance/Views/Help/Process/ReinsurancePolicyMassive.cshtml");
        }

        /// <summary>
        /// ReinsuranceReports
        /// </summary>
        /// <returns>View</returns> 
        public ActionResult ReinsuranceReports()
        {
            return View("~/Areas/Reinsurance/Views/Help/Process/ReinsureReports.cshtml");
        }

        #endregion

        #endregion 

        #region Methods

        /// <summary>
        /// HelpSearch
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult HelpSearch(string query)
        {
            query = query.ToLower(); // Función de búsqueda no busca en mayúsculas

            List<HelpModel> helpModels = new List<HelpModel>();

            // Se coloca en minusculas para que sea la búsqueda en minusculas
            helpModels.Add(new HelpModel() { Id = 1, HelpDescription = "contrato", Url = "Contract" });
            helpModels.Add(new HelpModel() { Id = 2, HelpDescription = "nivel de contrato", Url = "ContractLevel" });
            helpModels.Add(new HelpModel() { Id = 3, HelpDescription = "compañía de nivel", Url = "CompanyLevel" });
            helpModels.Add(new HelpModel() { Id = 4, HelpDescription = "línea", Url = "Line" });
            helpModels.Add(new HelpModel() { Id = 5, HelpDescription = "contrato por línea", Url = "ContractLine" });
            helpModels.Add(new HelpModel() { Id = 6, HelpDescription = "asociación de línea", Url = "AssociationLine" });
            helpModels.Add(new HelpModel() { Id = 7, HelpDescription = "reasegurar póliza", Url = "ReinsurancePolicy" });
            helpModels.Add(new HelpModel() { Id = 8, HelpDescription = "modificación capas", Url = "ModificationLayer" });
            helpModels.Add(new HelpModel() { Id = 9, HelpDescription = "modificación líneas", Url = "ModificationLine" });
            helpModels.Add(new HelpModel() { Id = 10, HelpDescription = "distribución de contratos", Url = "ModificationAllocation" });
            helpModels.Add(new HelpModel() { Id = 11, HelpDescription = "contrato facultativo", Url = "FacultativeContract" });
            helpModels.Add(new HelpModel() { Id = 12, HelpDescription = "reasegurar siniestro", Url = "ReinsuranceClaim" });
            helpModels.Add(new HelpModel() { Id = 13, HelpDescription = "modificar reaseguro de siniestro", Url = "ModificationReinsuranceClaim" });
            helpModels.Add(new HelpModel() { Id = 14, HelpDescription = "reasegurar pago", Url = "ReinsurancePayment" });
            helpModels.Add(new HelpModel() { Id = 15, HelpDescription = "modificar reaseguro de pago", Url = "ModificationReinsurancePayment" });
            helpModels.Add(new HelpModel() { Id = 16, HelpDescription = "consultar reaseguro", Url = "SearchReinsurance" });
            helpModels.Add(new HelpModel() { Id = 17, HelpDescription = "reasegurar póliza masivo", Url = "ReinsurancePolicyMassive" });
            helpModels.Add(new HelpModel() { Id = 18, HelpDescription = "reporte inconsistencia border", Url = "ReinsuranceReports" });

            var dataFiltered = helpModels.Where(d => d.HelpDescription.Contains(query));
            if (!String.IsNullOrEmpty(query))
            {
                return Json(dataFiltered, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(helpModels, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// LoadPartial
        /// </summary>
        /// <param name="controler"></param>
        /// <returns>RedirectToRouteResult</returns>
        public RedirectToRouteResult LoadPartial(string controler)
        {
            return RedirectToAction(controler, "Help");
        }

        #endregion Methods

    }
}
