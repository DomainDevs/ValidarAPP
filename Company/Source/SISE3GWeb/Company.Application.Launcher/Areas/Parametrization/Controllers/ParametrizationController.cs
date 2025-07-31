using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class ParametrizationController : Controller
    {

        // GET: Parametrization/Parametrization
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSubLinesBusinessByLineBusinessId()
        {
            try
            {
                List<LineBusiness> bussines = DelegateService.commonService.GetLinesBusiness();
                var model = new List<SubLineBusinessViewModel>();
                foreach (var bussinesss in bussines)
                {
                    List<SubLineBusiness> subbussines = DelegateService.commonService.GetSubLinesBusinessByLineBusinessId(bussinesss.Id);
                    foreach (var item in subbussines)
                    {
                        model.Add(new SubLineBusinessViewModel
                        {
                            LineBusinessDescription = bussinesss.Description,
                            Description = item.Description,
                            //SmallDescription = item.SmallDescription,
                            Id = item.Id,
                            //LineBusinessId = bussinesss.Id
                        });
                    }
                }
                return new UifJsonResult(true, model.OrderBy(x => x.Description));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
            }
        }

        /// <summary>
        /// Obtiene ramo comercial
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLinesBusiness()
        {
            try
            {
                List<LineBusiness> lineBussines = DelegateService.commonService.GetLinesBusiness();
                lineBussines = GetRiskTypeByLineBusiness(lineBussines);
                return new UifJsonResult(true, lineBussines.OrderBy(x => x.Description).ToList());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLineBusiness);
            }
        }

        public ActionResult GetLinesBusinessByDescription(string description)
        {
            try
            {
                LineBusiness lineBussines = DelegateService.commonService.GetLineBusinessById(description,0);
                List<LineBusiness> bussines = new List<LineBusiness>();
                bussines.Add(lineBussines);
                bussines = GetRiskTypeByLineBusiness(bussines);
                return new UifJsonResult(true, bussines.OrderBy(x => x.Description).ToList());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLineBusiness);
            }
        }

        public ActionResult GetPrefixes()
        {
            try
            {
                List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();
                return new UifJsonResult(true, prefixes.OrderBy(x => x.Description).ToList());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
            }
        }

        /// <summary>
        /// Obtiene todos los amparos
        /// </summary>
        /// <returns></returns>
        public ActionResult GetProtectionsAll()
        {
            try
            {
                List<ProtectionViewModel> perils = Parametrization.Models.ModelAssembler.CreateProtections(DelegateService.quotationService.GetPerils());
                return new UifJsonResult(true, perils);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProtection);
            }

        }

        /// <summary>
        /// Funcion para cargar tipos de riesgo por Ramo tecnico
        /// </summary>
        /// <param name="bussines"></param>
        /// <returns></returns>
        public List<LineBusiness> GetRiskTypeByLineBusiness(List<LineBusiness> bussines)
        {
            LineBusinessCoveredRiskType modelservice = new LineBusinessCoveredRiskType();
            List<LineBusinessCoveredRiskType> listModelService = new List<LineBusinessCoveredRiskType>();
            List<LineBusiness> bussinesService = new List<LineBusiness>();
            bussinesService = DelegateService.commonService.GetRiskTypeByLineBusinessId();
            for (int i = 0; i < bussines.Count; i++)
            {
               // bussines[i].ListLineBusinessCoveredrisktype = new List<LineBusinessCoveredRiskType>();
                foreach (var item2 in bussinesService)
                {
                    modelservice = new LineBusinessCoveredRiskType();
                    if (bussines[i].Id == item2.Id)
                    {
                        modelservice.IdLineBusiness = item2.Id;
                        modelservice.IdRiskType = item2.IdLineBusinessbyRiskType;
                      //  bussines[i].ListLineBusinessCoveredrisktype.Add(modelservice);
                    }
                }
            }
            return bussines;
        }


        public FileResult ShowExcelFile(string url)
        {
            var pathToTheFile = url;
            var fileStream = new System.IO.FileStream(pathToTheFile, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}