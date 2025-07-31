using Sistran.Company.Application.UnderwritingParamApplicationService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Company.Application.Utilities.Enums;
using Sistran.Company.Application.Utilities.DTO;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class CoverageValueController : Controller
    {
        // GET: Parametrization/CoverageValue
        private Helpers.PostEntity entityPrefix = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.Prefix" };
        private List<GenericViewModel> prefixes = new List<GenericViewModel>();
        #region CargaVistas
        public ActionResult CoverageValue()
        {
            return View();
        }
        public ActionResult CoverageValueSearchAdv()
        {
            return PartialView();
        }
        #endregion

        #region CargaListasCombos
        /// <summary>
        /// Carga listado de valores de cobertura, toma los 50 primeros, listview
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCoverageValue()
        {
            CoCoverageValueQueryDTO coverageValue = new CoCoverageValueQueryDTO();
            try
            {
                coverageValue = DelegateService.CompanyUnderwritingParamApplicationService.GetApplicationCoCoverageValue();
                return new UifJsonResult(true, coverageValue.CoCoverageValue.OrderBy(x => x.Prefix.Description).Take(50).ToList());
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetListCoCoverageValue);
            }
        }

        /// <summary>
        /// GetPrefixes: metodo que consulta listado de los ramos comerciales tabla comm.prefix
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPrefixes()
        {
            try
            {
                this.GetListprefixes();
                return new UifJsonResult(true, this.prefixes);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
            }
        }

        /// <summary>
        /// GetListprefixes: Metodo que consulta listado de los ramos comerciales tbala comm.prefix
        /// </summary>
        public void GetListprefixes()
        {
            if (this.prefixes.Count == 0)
            {
                this.prefixes = ModelAssembler.CreatePrefixes(ModelAssembler.DynamicToDictionaryList(this.entityPrefix.CRUDCliente.Find(this.entityPrefix.EntityType, null, null))).OrderBy(x => x.DescriptionLong).ToList();
            }
        }

        /// <summary>
        /// GetCoverages: metodo que consulta listado de coberturas pr prefix y linebusiness
        /// </summary>
        public ActionResult GetCoverages(int idPrefix)
        {
            List<CoverageView> coverages = new List<CoverageView>();
            try
            {                  
                var result = DelegateService.CompanyUnderwritingParamApplicationService.GetApplicationCoverageByPrefixId(idPrefix);
                return new UifJsonResult(true, result);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetListCoCoverageValue);
            }
          
        }

        #endregion

        #region eventosCrud
        public ActionResult CreateCoCoverage(CoverageValueViewModel coverageValueViewModel)
        {
            try
            {
                CoCoverageValueDTO coCoverageValueDTO = new CoCoverageValueDTO();
                coCoverageValueDTO = ModelAssembler.MappCoCoverageVMApplication(coverageValueViewModel);
                coCoverageValueDTO = DelegateService.CompanyUnderwritingParamApplicationService.CreateApplicationCoCoverageValue(coCoverageValueDTO);

                if (coCoverageValueDTO.Error.ErrorType == ErrorType.Ok)
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.CoverageValueSaveSuccessfully);
                }
                else
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.CoverageValueSaveError);

                }
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.DataAlreadyExists);
            }

        }
        
         public ActionResult UpdateCoCoverageValue(CoverageValueViewModel coverageValueViewModel)
        {
            CoCoverageValueDTO coCoverageValueDTO = new CoCoverageValueDTO();
            try
            {
                coCoverageValueDTO = ModelAssembler.MappCoCoverageVMApplication(coverageValueViewModel);
                coCoverageValueDTO=DelegateService.CompanyUnderwritingParamApplicationService.UpdateApplicationCoCoverageValue(coCoverageValueDTO);
                         
                if (coCoverageValueDTO.Error.ErrorType == ErrorType.Ok)
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.CoverageValueSaveSuccessfully);
                }
                else
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.CoverageValueUpdateError);
                }
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.CoverageValueUpdateError);
            }
            
            
        }

        public ActionResult DeleteCoCoverageValue(CoverageValueViewModel coverageValueViewModel)
        {
            CoCoverageValueDTO coCoverageValueDTO = new CoCoverageValueDTO();
            try
            {
                coCoverageValueDTO = ModelAssembler.MappCoCoverageVMApplication(coverageValueViewModel);
                coCoverageValueDTO=DelegateService.CompanyUnderwritingParamApplicationService.DeleteApplicationCoCoverageValue(coCoverageValueDTO);
                if(coCoverageValueDTO.Error.ErrorType.Equals(ErrorType.BusinessFault))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.CoverageValueDeleteError);
                }
                return new UifJsonResult(true, App_GlobalResources.Language.MessageDeletedSuccessfully);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.CoverageValueDeleteError);
            }
        }
        #endregion

        #region Search
        public ActionResult GetAdvancedSearch(CoverageValueViewModel coverageValueViewModel)
        {
           
            try
            {
                CoCoverageValueQueryDTO coCoverageValueQueryDTO = new CoCoverageValueQueryDTO();
                CoCoverageValueDTO coCoverageValueDTO = new CoCoverageValueDTO();
                coCoverageValueDTO = ModelAssembler.MappCoCoverageVMApplication(coverageValueViewModel);
                var result = DelegateService.CompanyUnderwritingParamApplicationService.GetApplicationCoCoverageValueAdv(coCoverageValueDTO);
                coCoverageValueQueryDTO = result;
                if (coCoverageValueQueryDTO.CoCoverageValue.Count == 0)
                {
                    return new UifJsonResult(true, coCoverageValueQueryDTO);
                }
                return new UifJsonResult(true, coCoverageValueQueryDTO.CoCoverageValue.OrderBy(x => x.Prefix.Description).Take(50).ToList());
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetListCoCoverageValue);
            }
        }
        #endregion
        #region ExportFile
        public JsonResult ExportFile()
        {
            ExcelFileDTO Result = DelegateService.CompanyUnderwritingParamApplicationService.GenerateFileApplicationToCoCoverage(App_GlobalResources.Language.FileNameCoCoverageValue);
            try
            {
                if (Result.ErrorType == ErrorType.Ok && !String.IsNullOrEmpty(Result.File))
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + Result.File);
                }
                else
                {
                    return new UifJsonResult(false, string.Join(",", App_GlobalResources.Language.ErrorExportCity));
                }
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExportCity);
            }

        }

        #endregion}

    }

}