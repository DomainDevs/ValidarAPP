// -----------------------------------------------------------------------
// <copyright file="LineBusinessController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using ENUMUD = Sistran.Core.Application.UnderwritingServices.Enums;

    /// <summary>
    /// Ramo técnico
    /// </summary>
    public class LineBusinessController : Controller
    {
        #region Technical Brnach Main
        /// <summary>
        /// llamado a vista de ramo tecnico
        /// </summary>
        /// <returns>Vista de ramo técnico</returns>
        public ActionResult LineBusiness()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene el ramo técnico por Id
        /// </summary>
        /// <param name="id">Id de ramo técnico</param>
        /// <returns>Ramo técnico</returns>
        public ActionResult GetLinesBusinessById(int id)
        {
            try
            {
                LineBusinessServiceModel lineBussines = DelegateService.UnderwritingParamServiceWeb.GetBusinesLinesServiceModelByLineBusinessId(id);

                if (lineBussines == null)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundProcess);
                }
                else
                {
                    return new UifJsonResult(true, lineBussines);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProcess);
            }
        }
        
        /// <summary>
        /// Obtiene el ramo técnico por Id
        /// </summary>
        /// <param name="description">Description de rtamo técnico</param>
        /// <param name="id">Id de ramo técnico</param>
        /// <returns>Ramo técnico</returns>
        public ActionResult GetLineBusinessByDescriptionById(string description, int id)
        {
            try
            {
                bool? respuesta = DelegateService.UnderwritingParamServiceWeb.GetLineBusinessByDescriptionById(description, id);

                if (respuesta == true)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorExistBranchTechnical);
                }
                else if(respuesta == false)
                {
                    return new UifJsonResult(true, "");
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProcess);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProcess);
            }
        }

        /// <summary>
        /// Obtiene el ramo técnico por Id
        /// </summary>
        /// <param name="description">Description de rtamo técnico</param>
        /// <param name="id">Id de ramo técnico</param>
        /// <returns>Ramo técnico</returns>
        public ActionResult GetLinesBusinessByDescription(string description, int id)
        {
            try
            {
                List<LineBusinessServiceModel> lineBussineses = DelegateService.UnderwritingParamServiceWeb.GetLineBusinessServiceModelByDescriptionById(description, id);

                if (lineBussineses == null || lineBussineses.Count == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundProcess);
                }
                else
                {
                    return new UifJsonResult(true, lineBussineses);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProcess);
            }
        }

        /// <summary>
        /// Obtiene el ramo técnico por Id
        /// </summary>
        /// <param name="description">Description de rtamo técnico</param>
        /// <param name="coveredRiskType">Tipo de riesgo cubierto</param>
        /// <returns>Ramo técnico</returns>
        public ActionResult GetLineBusinessAdvancedSearch(string description, int coveredRiskType)
        {
            try
            {
                List<LineBusinessServiceModel> lineBussineses = DelegateService.UnderwritingParamServiceWeb.GetLineBusinessServiceModelByAdvancedSearch(description, coveredRiskType);

                if (lineBussineses == null || lineBussineses.Count == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundProcess);
                }
                else
                {
                    return new UifJsonResult(true, lineBussineses);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProcess);
            }
        }

        /// <summary>
        /// Guarda el ramo técnico
        /// </summary>
        /// <param name="linebusinesModel">Ramo técnico</param>
        /// <param name="listInsuranceObjects"> Objetos del seguro asignados</param>
        /// <param name="protectionAssigned">Amparos asignados</param>
        /// <param name="clausesAssigned">Cláusulas asignadas</param>
        /// <returns>Respuesta de guardado</returns>
        public ActionResult SaveLineBusiness(LineBusinessViewModel linebusinesModel)
        {
            try
            {
                LineBusinessServiceModel businessLineServiceModel = Models.ModelAssembler.CreateLineBusinessServiceModel(linebusinesModel);
                businessLineServiceModel.StatusTypeService = linebusinesModel.Update ? ENUMSM.StatusTypeService.Update : ENUMSM.StatusTypeService.Create;
                businessLineServiceModel = DelegateService.UnderwritingParamServiceWeb.ExecuteOperationsLineBusinessServiceModel(businessLineServiceModel);
                if (businessLineServiceModel.ErrorServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    return new UifJsonResult(true, new { message = App_GlobalResources.Language.ReturnSavedLineBusiness });
                }
                else
                {
                    return new UifJsonResult(false, string.Join(",", businessLineServiceModel.ErrorServiceModel.ErrorDescription));
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateLineBusiness);
            }
        }

        /// <summary>
        /// Guarda los Objetos del seguro
        /// </summary>
        /// <param name="idLineBusiness">Id Ramo técnico</param>
        /// <param name="insuranceObjects">Objetos del seguro asignados</param>
        /// <returns>Respuesta de guardado</returns>
        public ActionResult SaveInsuredObject(int idLineBusiness, List<InsurencesObjectsViewModel> insuranceObjects)
        {
            try
            {
                if (insuranceObjects == null)
                {
                    insuranceObjects = new List<InsurencesObjectsViewModel>();
                }

                List <InsuredObjectServiceModel> insuranceObjectsServiceModel = Models.ModelAssembler.CreateInsuranceObject(insuranceObjects);
                insuranceObjectsServiceModel = DelegateService.UnderwritingParamServiceWeb.ExecuteOperationsInsuredObject(idLineBusiness, insuranceObjectsServiceModel);
                if (insuranceObjectsServiceModel.Count > 0)
                {
                    if (insuranceObjectsServiceModel[0].ErrorServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                    {
                        return new UifJsonResult(true, new { message = App_GlobalResources.Language.ReturnSavedInsuredObject });
                    }
                    else
                    {
                        return new UifJsonResult(false, string.Join(",", insuranceObjectsServiceModel[0].ErrorServiceModel.ErrorDescription));
                    }
                }
                else
                {
                    return new UifJsonResult(true, new { message = App_GlobalResources.Language.ReturnSavedInsuredObject});
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.SaveInsuredObject);
            }
        }

        /// <summary>
        /// Guarda los Amparos
        /// </summary>
        /// <param name="idLineBusiness">Id Ramo técnico</param>
        /// <param name="perils">Amparos asignados</param>
        /// <returns>Respuesta de guardado</returns>
        public ActionResult SavePeril(int idLineBusiness, List<ProtectionViewModel> perils)
        {
            try
            {
                if (perils == null)
                {
                    perils = new List<ProtectionViewModel>();
                }

                List<PerilServiceModel> perilsServiceModel = Models.ModelAssembler.CreatePeril(perils);
                perilsServiceModel = DelegateService.UnderwritingParamServiceWeb.ExecuteOperationsPeril(idLineBusiness, perilsServiceModel);
                if (perilsServiceModel.Count > 0)
                {
                    if (perilsServiceModel[0].ErrorServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                    {
                        return new UifJsonResult(true, new { message = App_GlobalResources.Language.ReturnSavedPerils });
                    }
                    else
                    {
                        return new UifJsonResult(false, string.Join(",", perilsServiceModel[0].ErrorServiceModel.ErrorDescription));
                    }
                }
                else
                {
                    return new UifJsonResult(true, new { message = App_GlobalResources.Language.ReturnSavedPerils });
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveProtection);
            }
        }

        /// <summary>
        /// Guarda las Cláusulas
        /// </summary>
        /// <param name="idLineBusiness">Id Ramo técnico</param>
        /// <param name="clauses">Cláusulas asignados</param>
        /// <returns>Respuesta de guardado</returns>
        public ActionResult SaveClause(int idLineBusiness, List<LineBusinessClauseViewModel> clauses)
        {
            try
            {
                if (clauses == null)
                {
                    clauses = new List<LineBusinessClauseViewModel>();
                }

                List<ClauseLevelServiceModel> clausesServiceModel = Models.ModelAssembler.CreateClause(clauses);
                clausesServiceModel = DelegateService.UnderwritingParamServiceWeb.ExecuteOperationsClause(idLineBusiness, clausesServiceModel);

                if (clausesServiceModel.Count > 0)
                {
                    if (clausesServiceModel[0].ErrorServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                    {
                        return new UifJsonResult(true, new { message = App_GlobalResources.Language.ReturnSavedClauses });
                    }
                    else
                    {
                        return new UifJsonResult(false, string.Join(",", clausesServiceModel[0].ErrorServiceModel.ErrorDescription));
                    }
                }
                else
                {
                    return new UifJsonResult(true, new { message = App_GlobalResources.Language.ReturnSavedClauses });
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveClauses);
            }
        }

        /// <summary>
        /// Elimina el ramo técnico
        /// </summary>
        /// <param name="idLineBusiness">Id del ramo técnico</param>
        /// <returns>Respuesta de eliminación</returns>
        public ActionResult DeleteLineBusiness(int idLineBusiness)
        {
            try
            {
                LineBusinessServiceModel parametrizationLineBusiness = new LineBusinessServiceModel()
                {
                    Id = idLineBusiness
                };
                parametrizationLineBusiness.StatusTypeService = ENUMSM.StatusTypeService.Delete;
                parametrizationLineBusiness = DelegateService.UnderwritingParamServiceWeb.DeleteLineBusiness(parametrizationLineBusiness);
                if (parametrizationLineBusiness.ErrorServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    return new UifJsonResult(true, new { message = App_GlobalResources.Language.ReturnDeleteLineBusiness });
                }
                else
                {
                    return new UifJsonResult(false, new { message = string.Join(",", parametrizationLineBusiness.ErrorServiceModel.ErrorDescription) });
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeletingLineBusiness);
            }
        }
        #endregion

        #region Search Advanced Technical Brnach

        /// <summary>
        /// llamado a vista de busqueda avanzada de ramo tecnico
        /// </summary>
        /// <returns>Vista de búsqueda avanzada</returns>
        public PartialViewResult SearchAdvanced()
        {
            return this.PartialView();
        }
        #endregion

        /// <summary>
        /// Obtiene los amparos disponibles
        /// </summary>
        /// <returns>Lista de amparos</returns>
        public ActionResult GetPerils()
        {
            try
            {
                PerilsServiceQueryModel perils = DelegateService.UnderwritingParamServiceWeb.GetPerilsServiceQueryModel();
                return new UifJsonResult(true, perils.PerilServiceQueryModels);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchTechnicalBranch);
            }
        }

        /// <summary>
        /// Obtiene los objetos del seguro disponibles
        /// </summary>
        /// <returns>Lista de objetos del seguro</returns>
        public ActionResult GetInsuranceObjects()
        {
            try
            {
                InsuredObjectsServiceQueryModel insObjects = DelegateService.UnderwritingParamServiceWeb.GetInsuredObjectsServiceQueryModels();
                return new UifJsonResult(true, insObjects.InsuredObjectServiceQueryModels);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchTechnicalBranch);
            }
        }

        /// <summary>
        /// Consulta todas las clausulas diponibles
        /// </summary>
        /// <returns>Lista de cláusulas</returns>
        public ActionResult GetClauses()
        {
            try
            {
                ClausesServiceQueryModel clauses = DelegateService.UnderwritingParamServiceWeb.GetClausesSQByEmissionLevelConditionLevelId(ENUMUD.EmissionLevel.Coverage, 4);
                return new UifJsonResult(true, clauses.ClauseServiceModels.OrderBy(x => x.Description));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(true, App_GlobalResources.Language.ErrorGetClauses);
            }
        }
        
        #region Protections Technical Brnach
        /// <summary>
        /// llamado a vista de Amparos para ramo tecnico
        /// </summary>
        /// <returns>Vista de amparos</returns>
        public PartialViewResult Protection()
        {
            return this.PartialView();
        }
        #endregion

        #region RiskType Technical Brnach
        /// <summary>
        /// Llamdo a vista de Tipos de riesgo
        /// </summary>
        /// <returns>Vista de tipos de riesgo cubiertos</returns>
        public PartialViewResult RiskType()
        {
            return this.PartialView();
        }
        
        /// <summary>
        /// action que carga los tipos de riesgo para el ramo tecnico
        /// </summary>
        /// <returns>Lista de tipos de riesgo cubiertos</returns>
        public ActionResult GetRiskTypeLineBusiness()
        {
            try
            {             
                CoveredRiskTypesQueryServiceModel coveredrisks = DelegateService.UnderwritingParamServiceWeb.GetAllGroupCoverages();
                if (coveredrisks.CoveredRiskTypeQueryServiceModels.Count != 0)
                {
                    return new UifJsonResult(true, coveredrisks.CoveredRiskTypeQueryServiceModels.OrderBy(x => x.Description));
                }
                else
                {
                    return new UifJsonResult(false, null);
                }
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCovered_Risks_Types);
            }
        }
        #endregion
        
        /// <summary>
        /// Genera archivo excel ramo técnico
        /// </summary>
        /// <returns>Ruta del archivo</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateLineBusinessServiceModel(App_GlobalResources.Language.FileNameTechnicalBranch);
                if (excelFileServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
    }
}