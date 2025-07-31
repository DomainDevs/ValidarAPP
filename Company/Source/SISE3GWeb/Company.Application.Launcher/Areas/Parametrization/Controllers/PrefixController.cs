using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Printing.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Company.Application.ModelServices.Models;
using Sistran.Core.Application.ModelServices.Models.Param;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class PrefixController : Controller
    {
        #region Propiedades
        /// <summary>
        /// Ramo tecnico
        /// </summary>
        private static List<CompanyPrefix> prefixes = new List<CompanyPrefix>();

        /// <summary>
        /// LIne business
        /// </summary>
        private static List<LineBusiness> lineBusinesss = null;
        List<BusinessBranchViewModel> BusinessBranchModel = new List<BusinessBranchViewModel>();
        #endregion

        #region Carga Vistas

        /// <summary>
        /// Carga la vista principal
        /// </summary>
        /// <returns>Action result</returns>
        public ActionResult Prefix()
        {
            return this.View();
        }

        /// <summary>
        /// Carga la vista parcial de busqueda avanzada
        /// </summary>
        /// <returns>Action result</returns>
        public ActionResult AdvancedSearch()
        {
            return this.PartialView();
        }

        /// <summary>
        /// Carga la vista parcial de ramo tecnico
        /// </summary>
        /// <returns>Action result</returns>
        public PartialViewResult TechnicalBranchBusiness()
        {
            return this.PartialView();
        }

        #endregion

        #region Carga Combos y listas

        /// <summary>
        /// Carga todos los ramos comerciales 
        /// </summary>
        /// <returns>Action Result</returns>
        public ActionResult GetPrefixes()
        {
            try
            {
                prefixes = DelegateService.companyCommonParamService.CompanyGetAllPrefix();
                return new UifJsonResult(true, prefixes.OrderBy(x => x.Description).ToList());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
            }
        }

        /// <summary>
        /// Carga todos los ramos tecnicos de la base de datos
        /// </summary>
        /// <returns>Action Result</returns>
        public ActionResult GetLinesBusiness()
        {
            try
            {
                if (lineBusinesss == null)
                {
                    lineBusinesss = DelegateService.commonService.GetLinesBusiness();
                }
                return new UifJsonResult(true, lineBusinesss.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLineBusiness);
            }

        }

        /// <summary>
        /// Genera archivo excel ramo comercial
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                GetPrefixAll();

                if (prefixes.Count > 0)
                {
                    string urlFile = DelegateService.companyCommonParamService.GenerateFileToPrefix(prefixes, App_GlobalResources.Language.FileNamePrefix);

                    if (!string.IsNullOrEmpty(urlFile))
                    {
                        return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                    }

                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
                }

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorThereIsNoDataToExport);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }

        /// <summary>
        /// Carga los ramos comerciales
        /// </summary>
        public void GetListBusinessBranch()
        {
            try
            {
                if (prefixes.Count == 0)
                {
                    prefixes = DelegateService.companyCommonParamService.CompanyGetAllPrefix().OrderBy(x => x.Description).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Carga los tipos de ramo comercial
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPrefixType()
        {
            try
            {
                List<Sistran.Core.Application.CommonService.Models.PrefixType> PrefixType = new List<Sistran.Core.Application.CommonService.Models.PrefixType>();
                PrefixType = DelegateService.commonService.GetPrefixType();
                return new UifJsonResult(true, PrefixType.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.GetPrefixType);
            }
        }

        #endregion        

        /// <summary>
        /// Guarda lor ramos
        /// </summary>
        /// <param name="prefixUpdate">ramos a guardar</param>
        /// <returns>Action Result</returns>
        public ActionResult SavePrefix(List<CompanyPrefix> prefixUpdate)
        {
            try
            {
                if (prefixUpdate != null)
                    prefixUpdate.ForEach(p => p.UserId = SessionHelper.GetUserId());
                List<string> prefixResponse = DelegateService.companyCommonParamService.SavePrefix(prefixUpdate);
                ParametrizationResult parametrizationResult = new ParametrizationResult();

                foreach (string item in prefixResponse)
                {

                    parametrizationResult.Message += "" + " " + item + "</br>";
                }
                return new UifJsonResult(true, parametrizationResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavePrefix);
            }
        }

        /// <summary>
        /// Gets the prefix all.
        /// </summary>
        private void GetPrefixAll()
        {
            if (prefixes.Count == 0)
            {
                prefixes = DelegateService.companyCommonParamService.CompanyGetAllPrefix().OrderBy(x => x.Description).ToList();
            }
        }
    }
}