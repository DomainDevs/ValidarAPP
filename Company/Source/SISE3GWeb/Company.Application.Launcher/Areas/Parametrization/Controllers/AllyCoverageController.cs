using Sistran.Company.Application.ModelServices.Enums;
using Sistran.Company.Application.UnderwritingParamApplicationService.DTOs;
using Sistran.Company.Application.Utilities.DTO;
using Sistran.Company.Application.Utilities.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class AllyCoverageController : Controller
    {
        private Helpers.PostEntity entityAllyCoveragePrincipal = new Helpers.PostEntity() { EntityType = App_GlobalResources.Language.CoverageNameSpace }; 
        //private Helpers.PostEntity entityAllyCoverageAsociated = new Helpers.PostEntity() { EntityType = App_GlobalResources.Language.AllyCoverageNameSpace }; 
        private Helpers.PostEntity entityAllyCoverage = new Helpers.PostEntity() { EntityType = App_GlobalResources.Language.AllyCoverageNameSpace };
        List<CoverageQueryViewModel> li_coverage = new List<CoverageQueryViewModel>();
        List<CoverageQueryViewModel> li_coverage_total = new List<CoverageQueryViewModel>();
        List<AllyCoverageViewModel> li_allyCov_AdvSearch = new List<AllyCoverageViewModel>();
        List<AllyCoverageViewModel> li_ally_coverage = new List<AllyCoverageViewModel>();
        /// <summary>
        /// Contructor: Llamado de la vista inicial.
        /// </summary>
        /// <returns>Vista inicial de Cobertura aliada</returns>
        public ActionResult AllyCoverage()
        {
            return this.View();
        }
        public ActionResult SearchAdvancedAllyCoverage()
        {
            return PartialView();
        }

        public UifJsonResult SearchAdvObjectsAllyCoverage(AllyCoverageViewModel allyC_ModelSearch)
        {
            List<AllyCoverageViewModel> elements;
            try
            {
                this.GetListSearchAllyCoverages();
                if(allyC_ModelSearch.CoverageId != 0 && allyC_ModelSearch.AllyCoverageId != 0)//Principal y aliada
                {
                    elements = li_allyCov_AdvSearch
                        .Where(x => x.CoverageId.Equals(allyC_ModelSearch.CoverageId) 
                        && x.AllyCoverageId.Equals(allyC_ModelSearch.AllyCoverageId)).ToList();

                    //al objeto le agrega objetos: cobertura principal y cobertura aliada
                    GetListCoveragesTotal();
                    var list_coverages = this.li_coverage_total;
                    elements.ForEach((x) =>
                    {
                        x.CoverageId_object = list_coverages.Where(y => y.Id.Equals(x.CoverageId)).First();
                        x.AllyCoverageId_object = list_coverages.Where(y => y.Id.Equals(x.AllyCoverageId)).First();
                    });
                    return new UifJsonResult(true, elements);

                }
                else if (allyC_ModelSearch.CoverageId != 0)//Principal
                {
                    elements = li_allyCov_AdvSearch
                        .Where(x => x.CoverageId.Equals(allyC_ModelSearch.CoverageId)).ToList();
                }
                else//No seleccionó ninguna
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.AllyCoverageNoSelectFound);

                }
                return new UifJsonResult(true, elements);
                
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingAllyCoverageData); 
            }
        }
        public UifJsonResult CreateAllyCoverage(AllyCoverageViewModel allyC_Model)
        {
            var coverageDTO = new AllyCoverageDTO();
            try
            {
                coverageDTO = ModelAssembler.MappAllyCoverageApplication(allyC_Model);
                coverageDTO = DelegateService.CompanyUnderwritingParamApplicationService.CreateAplicationAllyCoverage(coverageDTO);
                
                if (coverageDTO.errorDTO.ErrorType == ErrorType.Ok)
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.AllyCoverageSaveSucces);
                }
                else
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.AllyCoverageErrorCreate);
                }
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.AllyCoverageErrorCreate);
            }
        }

        public UifJsonResult SaveListAllyCoverages(List<AllyCoverageViewModel> listAllyCoverages)
        {
            try
            {
                int totalCreated = 0; int totalUpdated = 0; int totalDeleted = 0;
                int totalErrorCreated = 0; int totalErrorUpdated = 0; int totalErrorDeleted = 0;

                var coverageDTO = new AllyCoverageDTO();
                var coverageDTOold = new AllyCoverageDTO();

                //Obtenemos la lista de coberturas actual
                var listacoberally = ModelAssembler.CreateAllyCoverage(
                                                       ModelAssembler.DynamicToDictionaryList(
                                                           this.entityAllyCoverage.CRUDCliente.Find(
                                                               this.entityAllyCoverage.EntityType, null, null
                                                               )
                                                           )
                                    ).OrderBy(x => x.CoverageId).ToList();

                foreach (var allycoverage in listAllyCoverages)
                {
                    switch (allycoverage.Status)
                    {
                        case StatusTypeService.Create:
                            if (allycoverage.CoverageId != 0 && allycoverage.AllyCoverageId != 0)//Principal y aliada
                            {
                                var elements = listacoberally
                                    .Where(x => x.CoverageId.Equals(allycoverage.CoverageId)
                                    && x.AllyCoverageId.Equals(allycoverage.AllyCoverageId)).ToList();

                                //al objeto le agrega objetos: cobertura principal y cobertura aliada
                                var list_coverages = this.li_coverage_total;
                                if (elements.Count() > 0)
                                {
                                    return new UifJsonResult(false, "Error proceso cobertura aliada: "+ "Ya existe cobertura aliada ("+ allycoverage.AllyCoverageId +") asociada a cobertura ("+ allycoverage.CoverageId + ")");
                                }
                             }
                            coverageDTO = ModelAssembler.MappAllyCoverageApplication(allycoverage);
                            coverageDTO = DelegateService.CompanyUnderwritingParamApplicationService.CreateAplicationAllyCoverage(coverageDTO);

                            totalCreated = (coverageDTO.errorDTO.ErrorType == ErrorType.Ok) ? totalCreated+1 : totalCreated;
                            totalErrorCreated = (coverageDTO.errorDTO.ErrorType != ErrorType.Ok) ? totalErrorCreated+1 : totalErrorCreated;
                            break;
                        case StatusTypeService.Update:
                            coverageDTO = ModelAssembler.MappAllyCoverageApplication(allycoverage);
                            coverageDTOold = ModelAssembler.MappAllyCoverageApplication(allycoverage);
                            coverageDTO = DelegateService.CompanyUnderwritingParamApplicationService.UpdateAplicationAllyCoverage(coverageDTO, coverageDTOold);

                            totalUpdated = (coverageDTO.errorDTO.ErrorType == ErrorType.Ok) ? totalUpdated+1 : totalUpdated;
                            totalErrorUpdated = (coverageDTO.errorDTO.ErrorType != ErrorType.Ok) ? totalErrorUpdated+1 : totalErrorUpdated;
                            break;
                        case StatusTypeService.Delete:
                            coverageDTO = ModelAssembler.MappAllyCoverageApplication(allycoverage);
                            coverageDTO = DelegateService.CompanyUnderwritingParamApplicationService.DeleteAplicationAllyCoverage(coverageDTO);

                            totalDeleted = (coverageDTO.errorDTO.ErrorType == ErrorType.Ok) ? totalDeleted+1 : totalDeleted;
                            totalErrorDeleted = (coverageDTO.errorDTO.ErrorType != ErrorType.Ok) ? totalErrorDeleted+1 : totalErrorDeleted;
                            break;
                    }
                }
                string responseProcess = "Coberturas aliadas ";
                if ((totalCreated + totalDeleted + totalUpdated + totalErrorCreated + totalErrorDeleted + totalErrorUpdated) > 0)
                {
                    responseProcess = (totalCreated > 0) ? responseProcess + " Registradas: " + totalCreated.ToString() : responseProcess;
                    responseProcess = (totalUpdated > 0) ? responseProcess + " Actualizadas: " + totalUpdated.ToString() : responseProcess;
                    responseProcess = (totalDeleted > 0) ? responseProcess + " Eliminadas: " + totalDeleted.ToString() : responseProcess;
                    responseProcess = ((totalErrorCreated + totalErrorUpdated + totalErrorUpdated) > 0) ? responseProcess + ", Fallidos: "+(totalErrorCreated + totalErrorUpdated + totalErrorDeleted).ToString() :responseProcess;
                }

                return new UifJsonResult(true, responseProcess);
            }
            catch(Exception ex)
            {
                return new UifJsonResult(false, "Error proceso cobertura aliada: "+ ex.Message.ToString());
            }
        }

        public UifJsonResult UpdateAllyCoverage(AllyCoverageViewModel allyC_Model, AllyCoverageViewModel allyC_ModelOld)
        {
            var coverageDTO = new AllyCoverageDTO();
            var coverageDTOold = new AllyCoverageDTO();
            try
            {
                coverageDTO = ModelAssembler.MappAllyCoverageApplication(allyC_Model);
                coverageDTOold = ModelAssembler.MappAllyCoverageApplication(allyC_ModelOld);
                coverageDTO = DelegateService.CompanyUnderwritingParamApplicationService.UpdateAplicationAllyCoverage(coverageDTO, coverageDTOold);


                if (coverageDTO.errorDTO.ErrorType == ErrorType.Ok)
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.AllyCoverageSaveSucces);
                }
                else
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.AllyCoverageErrorUpdate);
                }
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.AllyCoverageErrorUpdate);
            }
        }

        public UifJsonResult DeleteAllyCoverage(List<AllyCoverageViewModel> li_AllyCoverage)
        {
            try
            {
                var coverageDTO = new AllyCoverageDTO();
                var failProcess = new JsonResult();
                li_AllyCoverage.ForEach(x => this.DeleteAllyCoverageItem(x));
                return new UifJsonResult(true, App_GlobalResources.Language.AllyCoverageSaveSucces);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.AllyCoverageErrorUpdate);
            }
        }

        public JsonResult DeleteAllyCoverageItem(AllyCoverageViewModel allyC_Model)
        {
            var coverageDTO = new AllyCoverageDTO();
            try
            {
                coverageDTO = ModelAssembler.MappAllyCoverageApplication(allyC_Model);
                coverageDTO = DelegateService.CompanyUnderwritingParamApplicationService.DeleteAplicationAllyCoverage(coverageDTO);//CreateAplicationAllyCoverage(coverageDTO);
                return new UifJsonResult(true, App_GlobalResources.Language.AllyCoverageSaveSucces);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public UifJsonResult GetFileToAllyCoverage(string fileName)
        {
            GetAllyCoveragesTotal();
            var list_coverages = ModelAssembler.MappQueryAllyCoverageApplication(this.li_ally_coverage);

            ExcelFileDTO Result = DelegateService.CompanyUnderwritingParamApplicationService.GenerateFileAplicationToAllyCoverageList(list_coverages, fileName);//CoCoverage(App_GlobalResources.Language.FileNameCoCoverageValue);
            try
            {
                if (Result.ErrorType == ErrorType.Ok && !String.IsNullOrEmpty(Result.File))
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + Result.File);
                }
                else
                {
                    return new UifJsonResult(false, string.Join(",", App_GlobalResources.Language.ErrorExportAllyCoverageData));
                }
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExportAllyCoverageData);
            }
            
        }

        public JsonResult GetPrincipalCoverage()
        {
            try
            {
                this.GetListCoveragesPrincipal();
                return new UifJsonResult(true, this.li_coverage);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingAllyCoverageData);
            }
        }

        /// <summary>
        /// Trae la cobertura según object insure
        /// </summary>
        /// <param name="id_object_insure"></param>
        /// <returns></returns>
        public JsonResult GetCoverageAllied(int id_object_insure)
        {
            try
            {
                this.GetListCoveragesAllies(id_object_insure);
                return new UifJsonResult(true, this.li_coverage);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingAllyCoverageData);
            }
        }

        // GET: UnderWritting/AllyCoverage
        /// <summary>
        /// Carga el listado de coberturas
        /// </summary>
        public void GetListCoveragesPrincipal()
        {
            if (this.li_coverage.Count == 0)
            {
                this.li_coverage = ModelAssembler.CreateAllyCoveragePrincipal(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityAllyCoveragePrincipal.CRUDCliente.Find(
                            this.entityAllyCoveragePrincipal.EntityType,null,null
                            )
                        )
                    ).Where(x => x.IsPrimary).OrderBy(x => x.PrintDescription).ToList(); 
                }
        }

        public void GetListSearchAllyCoverages()
        {
            if (this.li_allyCov_AdvSearch.Count == 0)
            {
                this.li_allyCov_AdvSearch = ModelAssembler.CreateAllyCoverage(
                        ModelAssembler.DynamicToDictionaryList(
                            this.entityAllyCoverage.CRUDCliente.Find(
                                this.entityAllyCoverage.EntityType, null, null
                                )
                            )
                ).OrderBy(x => x.CoverageId).ToList(); 
                
            }
        }

        // GET: UnderWritting/AllyCoverage
        /// <summary>
        /// Carga el listado de Coberturas aliadas
        /// </summary>
        public void GetListCoveragesTotal()
        {
            if (this.li_coverage_total.Count == 0)
            {
                this.li_coverage_total = ModelAssembler.CreateAllyCoveragePrincipal(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityAllyCoveragePrincipal.CRUDCliente.Find(
                            this.entityAllyCoveragePrincipal.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.Id).ToList(); 
            }
        }

        // GET: UnderWritting/AllyCoverage
        /// <summary>
        /// Carga el listado de Cobertura por Object Insure
        /// </summary>
        public void GetListCoveragesAllies(int coverage_code)
        {
            if (this.li_coverage.Count == 0)
            {
                this.li_coverage = ModelAssembler.CreateAllyCoveragePrincipal(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityAllyCoveragePrincipal.CRUDCliente.Find(
                            this.entityAllyCoveragePrincipal.EntityType, null, null
                            )
                        )
                    ).Where(x => !x.IsPrimary && x.InsuredObjectId.Equals(coverage_code)).OrderBy(x => x.Id).ToList();
            }
            
        }

        public void GetAllyCoveragesTotal()
        {
            this.li_ally_coverage = ModelAssembler.CreateAllyCoverage(
                        ModelAssembler.DynamicToDictionaryList(
                            this.entityAllyCoverage.CRUDCliente.Find(
                                this.entityAllyCoverage.EntityType, null, null
                                )
                            )
                ).OrderBy(x => x.CoverageId).ToList();
            //cargamos el list model de las coberturas para asociarlas a un description
            GetListCoveragesTotal();
            var list_coverages = this.li_coverage_total;
            li_ally_coverage.ForEach((x) =>
            {
                x.CoverageId_object = list_coverages.Where(y => y.Id.Equals(x.CoverageId)).First();
                x.AllyCoverageId_object = list_coverages.Where(y => y.Id.Equals(x.AllyCoverageId)).First();

            });

        }

        /// <summary>
        /// retorna todos los grupos de coberturas de la BD
        /// </summary>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult GetAllyCoverages()
        {
            try
            {
                var allyCoverageView = new List<AllyCoverageViewModel>();
                allyCoverageView = ModelAssembler.CreateAllyCoverage(
                        ModelAssembler.DynamicToDictionaryList(
                            this.entityAllyCoverage.CRUDCliente.Find(
                                this.entityAllyCoverage.EntityType, null, null
                                )
                            )
                ).OrderBy(x => x.CoverageId).Take(50).ToList();
                //cargamos el list model de las coberturas para asociarlas a un description
                
                GetListCoveragesTotal();
                var list_coverages = this.li_coverage_total;
                allyCoverageView.ForEach((x) =>
                {
                    x.CoverageId_object = list_coverages.Where(y => y.Id.Equals(x.CoverageId)).First();
                    x.AllyCoverageId_object = list_coverages.Where(y => y.Id.Equals(x.AllyCoverageId)).First();

                });
                
                return new UifJsonResult(true, allyCoverageView);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetAllyCoverageByCoverage(CoverageQueryViewModel coverage)
        {
            try
            {
                var allyCoverageView = new List<AllyCoverageViewModel>();
                allyCoverageView = ModelAssembler.CreateAllyCoverage(
                        ModelAssembler.DynamicToDictionaryList(
                            this.entityAllyCoverage.CRUDCliente.Find(
                                this.entityAllyCoverage.EntityType, null, null
                                )
                            )
                ).Where(x => x.CoverageId== coverage.Id).OrderBy(x=> x.AllyCoverageId).ToList();

                //al objeto le agrega objetos: cobertura principal y cobertura aliada
                GetListCoveragesTotal();
                var list_coverages = this.li_coverage_total;
                allyCoverageView.ForEach((x) =>
                {
                    x.CoverageId_object = coverage;
                    x.AllyCoverageId_object = list_coverages.Where(y => y.Id.Equals(x.AllyCoverageId)).First();
                });
                return new UifJsonResult(true, allyCoverageView);
            }
            catch(Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }
    }
}