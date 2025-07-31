// -----------------------------------------------------------------------
// <copyright file="AssistanceTypeController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Sistran.Core.Application.CommonService.Models;
    using Model = Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;

    /// <summary>
    /// Controlador de asistencia
    /// </summary>
    public class AssistanceTypeController : Controller
    {
        #region Propiedades

        /// <summary>
        /// Tiene la lista de ramo comercial
        /// </summary>
        private static List<Prefix> prefixes = new List<Prefix>();

        /// <summary>
        /// Tiene la lista de clausulas
        /// </summary>
        private static List<Model.Clause> clause = new List<Model.Clause>();

        /// <summary>
        /// Tiene la lista de tipo de asistencia
        /// </summary>
        private static List<AssistanceTypeModel> assistanceTypeModel = new List<AssistanceTypeModel>();

        /// <summary>
        /// Tiene la lista de textos de asistencia
        /// </summary>
        private static List<AssistanceTextModel> assistanceTextModel = new List<AssistanceTextModel>();
        #endregion

        /// <summary>
        /// Inicia la vista tipo de asistencia
        /// </summary>
        /// <returns>Retorna vista</returns>
        public ActionResult AssistanceType()
        {
            return this.View();
        }

        /// <summary>
        /// Inicia la vista de busqueda avanzada tipo de asistencia
        /// </summary>
        /// <returns>Retorna vista</returns>
        public ActionResult AssistanceAdvancedSearch()
        {
            return this.PartialView();
        }

        /// <summary>
        /// Obtiene los tipos de asistencia
        /// </summary>
        /// <returns>Retorna json con la informacion</returns>
        public ActionResult GetAssistanceType()
        {
            try
            {
                List<AssistanceType> AssType = DelegateService.commonService.GetAssistanceTypes();
                assistanceTypeModel = Parametrization.Models.ModelAssembler.GetAssistanceType(AssType);
                prefixes = DelegateService.commonService.GetAllPrefix();
                for (int i = 0; i < assistanceTypeModel.Count; i++)
                {
                    assistanceTypeModel[i].EnabledDescription = assistanceTypeModel[i].Enabled == true ? App_GlobalResources.Language.LabelEnabled : App_GlobalResources.Language.Disabled;
                    assistanceTypeModel[i].Prefix = prefixes.FirstOrDefault(p => p.Id == assistanceTypeModel[i].PrefixCode);
                    assistanceTypeModel[i].PrefixDescription = prefixes.FirstOrDefault(p => p.Id == assistanceTypeModel[i].PrefixCode).Description;
                }

                return new UifJsonResult(true, assistanceTypeModel.OrderBy(x => x.AssistanceCode).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.GetAssistanceType);
            }
        }

        /// <summary>
        /// Guarda el tipo de aistencia y los textos de asistencia (Crea, actualiza)
        /// </summary>
        /// <param name="assistances">Lista de tipo de asistencia</param>
        /// <param name="assistanceText">Lista de textos de asistencia</param>
        /// <returns>Retorna json con la informacion</returns>
        public ActionResult SaveAssistanceType(List<AssistanceTypeModel> assistances, List<AssistanceText> assistanceText)
        {
            try
            {
                List<AssistanceType> assistanceType = Parametrization.Models.ModelAssembler.CreateAssistanceType(assistances);
                List<string> assistanceResponse = new List<string>();
                List<string> assistanceResponseText = new List<string>();
                if (assistanceType != null)
                {
                    assistanceResponse = DelegateService.commonService.CreateAssistanceTypes(assistanceType);
                }

                if (assistanceText != null)
                {
                    assistanceResponseText = DelegateService.commonService.CreateAssistanceText(assistanceText); 
                }

                assistanceTypeModel = Parametrization.Models.ModelAssembler.GetAssistanceType(DelegateService.commonService.GetAssistanceTypes());
                for (int i = 0; i < assistanceTypeModel.Count; i++)
                {
                    assistanceTypeModel[i].EnabledDescription = assistanceTypeModel[i].Enabled == true ? App_GlobalResources.Language.LabelEnabled : App_GlobalResources.Language.Disabled;
                    assistanceTypeModel[i].Prefix = prefixes.FirstOrDefault(p => p.Id == assistanceTypeModel[i].PrefixCode);
                    assistanceTypeModel[i].PrefixDescription = prefixes.FirstOrDefault(p => p.Id == assistanceTypeModel[i].PrefixCode).Description;
                }

                object[] result = new object[3];
                result[0] = assistanceResponse;
                result[1] = assistanceTypeModel.OrderBy(x => x.AssistanceCode).ToList();
                result[2] = assistanceResponseText;
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavePrefix);
            }
        }

        /// <summary>
        /// Obtiene tipo de asistencia por id de asistencia, descripcion de asistencia, ramo comercial
        /// </summary>
        /// <param name="assistanceCode">Codigo de asistencia</param>
        /// <param name="descripcion">Descripcion o nombre de asistencia</param>
        /// <param name="prefixCode">Codigo de ramo comercial</param>
        /// <returns>Retorna json con la informacion</returns>
        public ActionResult GetAssistanceByAssistanceCodeDescriptionPrefix(int assistanceCode, string descripcion, int prefixCode)
        {
            try
            {
                List<AssistanceTypeModel> assistanceModel = new List<AssistanceTypeModel>();
                if (assistanceCode != 0)
                {
                    assistanceModel = (from a in assistanceTypeModel where a.AssistanceCode == assistanceCode select a).ToList();
                }
                else if (prefixCode != 0)
                {
                    assistanceModel = (from a in assistanceTypeModel where a.PrefixCode == prefixCode select a).ToList();
                }
                else
                {
                    assistanceModel = (from a in assistanceTypeModel where a.Description.Contains(descripcion) select a).ToList();
                }

                return new UifJsonResult(true, assistanceModel.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.GetAssistanceType);
            }
        }

        /// <summary>
        /// Genera archivo excel de tipo de asistecia
        /// </summary>
        /// <returns>respuesta pertinente a la accion generada</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                string urlFile = DelegateService.commonService.GenerateFileToAssistanceType(ModelAssembler.CreateAssistanceType(assistanceTypeModel), App_GlobalResources.Language.FileNameAssistanceType);

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

        /// <summary>
        /// Obtiene textos de asistencia
        /// </summary>
        /// <param name="assistanceCd"> cosigo  de asistencia</param>
        /// <returns>Retorna json con la informacion</returns>
        public ActionResult GetAssistanceText(int assistanceCd)
        {
            try
            {
                List<AssistanceTextModel> assistanceText = new List<AssistanceTextModel>();
                List<AssistanceTextModel> modelassistanceText = new List<AssistanceTextModel>();
                assistanceTextModel = Parametrization.Models.ModelAssembler.GetAssistanceText(DelegateService.commonService.GetAssistanceText());
                modelassistanceText = (from a in assistanceTextModel where a.AssistanceCd == assistanceCd select a).ToList();
                clause = DelegateService.underwritingService.GetClause();
                for (int i = 0; i < modelassistanceText.Count; i++)
                {
                    if (modelassistanceText[i].AssistanceCd == assistanceCd)
                    {
                        modelassistanceText[i].EnabledDescription = modelassistanceText[i].Enable == true ? App_GlobalResources.Language.LabelEnabled : App_GlobalResources.Language.Disabled;
                        modelassistanceText[i].Clause = clause.FirstOrDefault(c => c.Id == modelassistanceText[i].ClauseCd3G);
                        modelassistanceText[i].ClauseDescription = modelassistanceText[i].Clause == null ? string.Empty : clause.FirstOrDefault(p => p.Id == modelassistanceText[i].ClauseCd3G).Name;
                    }
                }

                return new UifJsonResult(true, modelassistanceText.OrderBy(x => x.AssistanceTextId).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.GetAssistanceText);
            }
        }

        /// <summary>
        /// Obtiene las clausulas por Nombre o codigo
        /// </summary>
        /// <param name="description">Filtro para buscar clausula (Nombre o codigo)</param>
        /// <returns>Retorna json con la informacion</returns>
        public ActionResult GetClausesByIdBydescription(string description)
        {
            try
            {
                List<Model.Clause> clauses = new List<Model.Clause>();
                clauses = (from a in clause where a.Name.Contains(description) || a.Id.ToString().Contains(description) select a).ToList();
                return new UifJsonResult(true, clauses.OrderBy(x => x.Id).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClauses);
            }
        }
    }
}