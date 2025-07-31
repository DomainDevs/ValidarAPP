// -----------------------------------------------------------------------
// <copyright file="TextPrecataloguedController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;    
    using Sistran.Company.Application.UniquePersonParamService.Models;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using MOS = Core.Application.QuotationServices.Models;
    using QUO = Company.Application.QuotationServices.Models;
    using Sistran.Core.Application.QuotationServices.Models;
   
    using Sistran.Core.Application.ModelServices.Enums;
    using PARMS = Sistran.Company.Application.ModelServices.Models.Param;
    using ENUMSM = Sistran.Company.Application.ModelServices.Enums;
    using Sistran.Company.Application.Utilities.DTO;
    using Sistran.Company.Application.Utilities.Enums;
    using Sistran.Core.Application.EntityServices.Models;
    using Sistran.Core.Application.ModelServices.Models.Param;


    /// <summary>
    /// Controlador de la vista de creación y modificación de textos pre catalogacion.
    /// Módulo de parametrización.
    /// </summary>
    public class TextPrecataloguedController : Controller
    {
        private List<TextPrecatalogued> textCatalogued = new List<TextPrecatalogued>();
        private PostEntity postEntity = new PostEntity { EntityType = "Sistran.Core.Application.Quotation.Entities.ConditionText", KeyType = KeyType.NextValue };
        /// <summary>
        /// Contructor. Llamado de la vista inicial.
        /// </summary>
        /// <returns>Vista inicial de aliados</returns>
        public ActionResult TextPrecatalogued()
        {
            return this.View();
        }

        public ActionResult DetailTextPrecatalogued()
        {
            return PartialView();
        }
        /// <summary>
        /// Metodo Para Realizar los Llamados a los niveles.
        /// </summary>
        /// <returns>Retorna resultado de listado de niveles </returns>
        public ActionResult GetLevels()
        {
            try
            {
                ConditionLevelsServiceModel parametrizationClause = DelegateService.UnderwritingParamServiceWeb.GetClausesLevelsServiceModel();
                return new UifJsonResult(true, parametrizationClause.ConditionLevelsServiceModels.OrderBy(b => b.Description));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLineBusiness);
            }
        }

        /// <summary>
        /// Obtiene ramo comercial
        /// </summary>
        /// <returns>Retorna lista de ramos</returns>
        [HttpGet]
        public ActionResult GetCommercialBranch()
        {
            try
            {
                PrefixsServiceQueryModel parametrizationClause = DelegateService.UnderwritingParamServiceWeb.GetCommercialBranch();
                return new UifJsonResult(true, parametrizationClause.PrefixServiceQueryModel.OrderBy(b => b.PrefixDescription));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLineBusiness);
            }
        }

        /// <summary>
        /// Obtiene tipo de riesgo cubierto
        /// </summary>
        /// <returns>Retorna combo tipo de riesgos</returns>
        [HttpGet]
        public ActionResult GetCoveredRiskType()
        {
            try
            {
                RiskTypesServiceModel parametrizationClause = DelegateService.UnderwritingParamServiceWeb.GetCoveredRiskType();
                return new UifJsonResult(true, parametrizationClause.RiskTypeServiceModels.OrderBy(b => b.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLineBusiness);
            }
        }

        /// <summary>
        /// Metodo permite consulta Coberturas
        /// </summary>
        /// <param name="description">Parametro descripcion</param>
        /// <returns>Retorna lista de coberturas por descripcion</returns>
        public ActionResult GetCoverage(string description)
        {
            try
            {
                CoveragesClauseServiceModel coverageParametrization = DelegateService.UnderwritingParamServiceWeb.GetCoverageByName(description);

                if (coverageParametrization.CoverageServiceModels.Count != 0)
                {
                    return new UifJsonResult(true, coverageParametrization);
                }

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingCoverages);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingCoverages);
            }
        }
        [HttpGet]
        public ActionResult GetTexPrecataloged()
        {
            try
            {
                List<QUO.TextPretacalogued> conditionTextModel = DelegateService.quotationService.GetTextPretacaloguedDto();

                List<TextPrecatalogued> llenarListTextPrecatalogued = new List<TextPrecatalogued>();
                
                if (conditionTextModel != null)
                {
                    // ConditionLevelsServiceModel parametrizationClause = DelegateService.UnderwritingParamServiceWeb.GetClausesLevelsServiceModel();
                    foreach (var item in conditionTextModel)
                    {
                        TextPrecatalogued insertar = new TextPrecatalogued();
                        insertar.TextTitle = item.TextTitle;
                        insertar.ConditionLevelCode = item.ConditionLevelCode;
                        insertar.DescriptionLevel = item.DescriptionLevel;
                        insertar.ConditionTextId = item.ConditionTextId;
                        if (item.ConditionLevelCode == 2)
                        {
                            insertar.CommercialBranch = item.ConditionLevelId;
                            insertar.DescriptionBranch = item.DescriptionBranch;
                        }
                        else if (item.ConditionLevelCode == 3)
                        {
                            insertar.CoveredRisk = item.ConditionLevelId;
                            insertar.DescriptionRiskCoverange = item.DescriptionRiskCoverange;
                        }
                        else if (item.ConditionLevelCode == 5)
                        {
                            insertar.Coverage = item.ConditionLevelId;
                            insertar.DescriptionCoverange = item.DescriptionCoverange;
                        }
                        insertar.TextBody = item.TextBody;
                        insertar.CondTextLevelId = item.CondTextLevelId;
                        
                        llenarListTextPrecatalogued.Add(insertar);
                    }
                    return new UifJsonResult(true, llenarListTextPrecatalogued.OrderBy(x => x.ConditionTextId).ToList().Take(50));

                }
            }
            catch (Exception ex )
            {

                throw ex;
            }
            return new UifJsonResult(false, null);

        }
        public ActionResult CreateTextPrecatalogued(List<QUO.TextPretacalogued> textPrecatalogueds)
        {
            string messageErrors = string.Empty;
            string message = string.Empty;
            int add = 0;
            try
            {
                try
                {
                    List<QUO.TextPretacalogued> textPrecataloguedsAux = new List<QUO.TextPretacalogued>();
                    textPrecataloguedsAux = textPrecatalogueds.Where(x => x.StatusTypeService == StatusTypeService.Create).ToList();
                    if(textPrecataloguedsAux.Count>0)
                    {
                        DelegateService.quotationService.ExecuteOpertaionTextPrecatalogued(textPrecataloguedsAux);
                        add = textPrecataloguedsAux.Count();
                        message += string.Format(App_GlobalResources.Language.ReturnSaveAdded, add);
                    }
                    
                }
                catch(Exception e)
                {                   
                    return new UifJsonResult(true, App_GlobalResources.Language.ErrorSaveTextPrecatalogued);
                }

                try
                {
                    List<QUO.TextPretacalogued> textPrecataloguedsAux = new List<QUO.TextPretacalogued>();
                    textPrecataloguedsAux = textPrecatalogueds.Where(x => x.StatusTypeService == StatusTypeService.Update).ToList();
                    if (textPrecataloguedsAux.Count > 0)
                    {
                        DelegateService.quotationService.ExecuteOpertaionTextPrecatalogued(textPrecataloguedsAux);
                        add = textPrecataloguedsAux.Count();
                        message += string.Format(App_GlobalResources.Language.ReturnSaveEdited, add);
                    }
                }
                catch (Exception e)
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.ErrorSaveTextPrecatalogued);
                }

                try
                {
                    List<QUO.TextPretacalogued> textPrecataloguedsAux = new List<QUO.TextPretacalogued>();
                    textPrecataloguedsAux = textPrecatalogueds.Where(x => x.StatusTypeService == StatusTypeService.Delete).ToList();
                    if (textPrecataloguedsAux.Count > 0)
                    {
                        DelegateService.quotationService.ExecuteOpertaionTextPrecatalogued(textPrecataloguedsAux);
                        add = textPrecataloguedsAux.Count();
                        message += string.Format(App_GlobalResources.Language.ReturnSaveDeleted, add);
                    }
                }
                catch (Exception e)
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.ErrorDeleteTextPrecatalogued);
                }
                return new UifJsonResult(true, message);
            }
            catch (Exception e)
            {
                return new UifJsonResult(true, App_GlobalResources.Language.ErrorSaveTextPrecatalogued);
            }
        }

        public JsonResult ExportFile()
        {
            ExcelFileServiceModel Result = DelegateService.quotationService.GenerateFileToText(App_GlobalResources.Language.FileNameTextCatalogued);
            try
            {
                if ((Result.ErrorTypeService == ErrorTypeService.Ok) && !String.IsNullOrEmpty(Result.FileData))
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + Result.FileData);
                }

                return new UifJsonResult(false, string.Join(",", App_GlobalResources.Language.ErrorThereIsNoDataToExport));
            }
            catch (Exception e)            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExportCity);
            }
        }

        public ActionResult GetTextPreByDescription(string description)
        {
            try
            {
                List<TextPrecatalogued> conditionTextModel = GetListTextPre(description.ToUpper());
                return new UifJsonResult(true, conditionTextModel.OrderBy(x => x.TextTitle).ToList());
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExistTextPre);
            }
        }

        private List<TextPrecatalogued> GetListTextPre(string description)
        {
            List<QUO.TextPretacalogued> conditionTextModel = DelegateService.quotationService.GetTextPretacaloguedByDescription(description);

            List<TextPrecatalogued> llenarListTextPrecatalogued = new List<TextPrecatalogued>();

            if (conditionTextModel != null)
            {               
                foreach (var item in conditionTextModel)
                {
                    TextPrecatalogued insertar = new TextPrecatalogued();
                    insertar.TextTitle = item.TextTitle;
                    insertar.ConditionLevelCode = item.ConditionLevelCode;
                    insertar.DescriptionLevel = item.DescriptionLevel;
                    insertar.ConditionTextId = item.ConditionTextId;
                    if (item.ConditionLevelCode == 2)
                    {
                        insertar.CommercialBranch = item.ConditionLevelId;
                        insertar.DescriptionBranch = item.DescriptionBranch;
                    }
                    else if (item.ConditionLevelCode == 3)
                    {
                        insertar.CoveredRisk = item.ConditionLevelId;
                        insertar.DescriptionRiskCoverange = item.DescriptionRiskCoverange;
                    }
                    else if (item.ConditionLevelCode == 5)
                    {
                        insertar.Coverage = item.ConditionLevelId;
                        insertar.DescriptionCoverange = item.DescriptionCoverange;
                    }
                    insertar.TextBody = item.TextBody;
                    insertar.CondTextLevelId = item.CondTextLevelId;

                    llenarListTextPrecatalogued.Add(insertar);
                }
                 llenarListTextPrecatalogued.OrderBy(x => x.TextTitle).ToList();

            }

            return llenarListTextPrecatalogued;
        }

    }
}