// -----------------------------------------------------------------------
// <copyright file="DeductibleController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Helpers;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using ENUMUTIL = Sistran.Core.Services.UtilitiesServices.Enums;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.EntityServices.Models;
    using modelsEntityServicesCore = Sistran.Core.Application.EntityServices.Enums;


    /// <summary>
    /// Clase deducible
    /// </summary>
    public class DeductibleController : Controller
    {
        /// <summary>
        /// Parametros de entidad 
        /// </summary>
        private Application.EntityServices.Models.PostEntity postEntity = new Application.EntityServices.Models.PostEntity { EntityType = "Sistran.Core.Application.Quotation.Entities.Deductible", KeyType = KeyType.Autonumber };

        /// <summary>
        /// Modelo deducible
        /// </summary>
        private List<DeductibleViewModel> deductibleModel = new List<DeductibleViewModel>();

        /// <summary>
        /// Abrir pantalla principal
        /// </summary>
        /// <returns>Vista principal</returns>
        public ActionResult Deductible()
        {
            return this.View();
        }

        /// <summary>
        /// Abrir pantalla busqueda avanzada
        /// </summary>
        /// <returns>vista busqueda avanzada</returns>
        [HttpGet]
        public ActionResult AdvancedSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene listas de negocio
        /// </summary>
        /// <returns>Modelo LinesBusinessServiceQueryModel</returns>
        [HttpGet]
        public ActionResult GetLineBusiness()
        {
            LinesBusinessServiceQueryModel linesBusinessServiceModel = DelegateService.UnderwritingParamServiceWeb.GetLinesBusiness();
            if (linesBusinessServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, linesBusinessServiceModel.LineBusinessServiceModel.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { linesBusinessServiceModel.ErrorTypeService, linesBusinessServiceModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Listado de aplica sobre
        /// </summary>
        /// <returns>Modelo DeductibleSubjectsServiceQueryModel</returns>
        [HttpGet]
        public ActionResult GetDeductibleSubject()
        {
            DeductibleSubjectsServiceQueryModel deductibleSubjectsServiceModel = DelegateService.UnderwritingParamServiceWeb.GetDeductibleSubjects();
            if (deductibleSubjectsServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, deductibleSubjectsServiceModel.DeductibleSubjectServiceModels.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { deductibleSubjectsServiceModel.ErrorTypeService, deductibleSubjectsServiceModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Obtiene las unidades
        /// </summary>
        /// <returns>Listado DeductibleUnitsServiceQueryModel</returns>
        [HttpGet]
        public ActionResult GetDeductibleUnit()
        {
            DeductibleUnitsServiceQueryModel deductibleUnitsServiceModel = DelegateService.UnderwritingParamServiceWeb.GetDeductibleUnits();
            if (deductibleUnitsServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, deductibleUnitsServiceModel.DeductibleUnitServiceModels.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { deductibleUnitsServiceModel.ErrorTypeService, deductibleUnitsServiceModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Obtiene monedas
        /// </summary>
        /// <returns>Listado CurrenciesServiceQueryModel</returns>
        [HttpGet]
        public UifJsonResult GetCurrencies()
        {
            CurrenciesServiceQueryModel currenciesServiceModel = DelegateService.UnderwritingParamServiceWeb.GetCurrencies();
            if (currenciesServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, currenciesServiceModel.CurrencyServiceModel.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { currenciesServiceModel.ErrorTypeService, currenciesServiceModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Obtiene los tipos
        /// </summary>
        /// <returns>Listado enum RateType</returns>
        [HttpGet]
        public ActionResult GetRateTypes()
        {
            try
            {
                var rateTypes = EnumsHelper.GetItems<ENUMSM.RateType>();
                return new UifJsonResult(true, rateTypes);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryRateTypes);
            }
        }

        /// <summary>
        /// Obtiene todos los deducibles
        /// </summary>
        /// <returns>Listado deducibles</returns>
        [HttpGet]
        public ActionResult GetDeductibles()
        {
            try
            {
                this.GetListDeductibles();
                return new UifJsonResult(true, this.deductibleModel.OrderBy(x => x.DeductibleId).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLineBusiness);
            }
        }

        /// <summary>
        /// Obtiene deducible por filtro
        /// </summary>
        /// <param name="deductible">Modelo DeductibleViewModel</param>
        /// <returns>Listado de deducibles</returns>
        [HttpPost]
        public ActionResult GetDeductibleByDeductible(DeductibleViewModel deductible)
        {
            try
            {
                List<DeductibleViewModel> searchDeductibles = new List<DeductibleViewModel>();
                searchDeductibles = this.GetListDeductibles();
                if (deductible.LineBusinessId != 0)
                {
                    searchDeductibles = searchDeductibles.Where(x => x.LineBusinessId == deductible.LineBusinessId).ToList();
                }

                if (deductible.ApplyOnId != 0)
                {
                    searchDeductibles = searchDeductibles.Where(x => x.ApplyOnId == deductible.ApplyOnId).ToList();
                }

                if (deductible.UnitId != 0)
                {
                    searchDeductibles = searchDeductibles.Where(x => x.UnitId == deductible.UnitId).ToList();
                }

                if (deductible.Value != null)
                {
                    searchDeductibles = searchDeductibles.Where(x => decimal.Parse(x.Value) == decimal.Parse(deductible.Value)).ToList();
                }

                return new UifJsonResult(true, searchDeductibles.OrderBy(x => x.DeductibleId).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLineBusiness);
            }
        }

        /// <summary>
        /// Genera archivo excel de coberturas
        /// </summary>
        /// <returns>Archivo excel</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                this.GetListDeductibles();
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToDeductible(ModelAssembler.CreateDeductibles(this.deductibleModel), App_GlobalResources.Language.LabelDeductible);
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

        /// <summary>
        /// Metodo para guardar los deducibles
        /// </summary>
        /// <param name="deductibles">Listado DeductibleViewModel</param>
        /// <returns>Listado deducibles en base de datos</returns>
        [HttpPost]
        public ActionResult Save(List<DeductibleViewModel> deductibles)
        {
            string messageErrors = string.Empty;
            int add = 0;
            int edit = 0;
            int delete = 0;
                       
            foreach (DeductibleViewModel item in deductibles)
            {
                List<Field> fields = new List<Field>();
                try
                {
                    if ((ENUMUTIL.ParametrizationStatus)item.Status == ENUMUTIL.ParametrizationStatus.Delete)
                    {
                        int hasDependencies = DelegateService.UnderwritingParamServiceWeb.ValidateDeductible(item.DeductibleId);

                        if (hasDependencies == 0)
                        {
                            fields.Add(new Field { Name = "DeductId", Value = item.DeductibleId.ToString() });
                            this.postEntity.Fields = fields;
                            DelegateService.EntityServices.Delete(this.postEntity);
                            delete += 1;
                        }
                        else
                        {
                            if (messageErrors != string.Empty)
                            {
                                messageErrors += " ";
                            }

                            messageErrors += string.Format(App_GlobalResources.Language.ErrorDeleteWithDependencies, item.DeductibleId);
                        }
                    }
                    else
                    {
                        this.AssignPostEntity(item);
                        var defaultPostEntityStatus = (this.postEntity.Status == null) ? modelsEntityServicesCore.StatusTypeService.Create : this.postEntity.Status;

                        if (defaultPostEntityStatus == modelsEntityServicesCore.StatusTypeService.Create)
                        {
                            this.postEntity = DelegateService.EntityServices.Create(this.postEntity);
                            Field field = this.postEntity.Fields.First(x => x.Name == "DeductId");
                            if (field.Value != "0")
                            {
                                add += 1;
                            }
                        }
                        else if (defaultPostEntityStatus == modelsEntityServicesCore.StatusTypeService.Update)
                        {
                            DelegateService.EntityServices.Update(this.postEntity);
                            edit += 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    messageErrors = messageErrors + " " + string.Format(ex.Message + " " + item.DeductibleId + "<br/>");
                }
            }

            this.deductibleModel = new List<DeductibleViewModel>();
            this.GetListDeductibles();

            string message = string.Empty;
            if (add > 0)
            {
                message += string.Format(App_GlobalResources.Language.ReturnSaveAdded, add);
            }

            if (edit > 0)
            {
                if (messageErrors != string.Empty)
                {
                    messageErrors += " ";
                }

                message += string.Format(App_GlobalResources.Language.ReturnSaveEdited, edit);
            }

            if (delete > 0)
            {
                if (messageErrors != string.Empty)
                {
                    messageErrors += " ";
                }

                message += string.Format(App_GlobalResources.Language.ReturnSaveDeleted, delete);
            }

            if (messageErrors != string.Empty)
            {
                if (messageErrors != string.Empty)
                {
                    messageErrors += " ";
                }

                message += messageErrors;
            }

            var result = this.deductibleModel.OrderBy(x => x.DeductibleId).ToList();
            return new UifJsonResult(true, new { message = message, data = result });
        }

        /// <summary>
        /// Asigna los campos a la entidad
        /// </summary>
        /// <param name="item">modelo DeductibleViewModel</param>
        private void AssignPostEntity(DeductibleViewModel item)
        {
            List<Field> fields = new List<Field>();
            if (item.DeductibleId != 0)
            {
                fields.Add(new Field { Name = "DeductId", Value = item.DeductibleId.ToString() });
            }

            fields.Add(new Field { Name = "LineBusinessCode", Value = item.LineBusinessId.ToString() });
            fields.Add(new Field { Name = "RateTypeCode", Value = Convert.ToString((int)item.Type) });
            if (item.Rate != string.Empty)
            {
                fields.Add(new Field { Name = "Rate", Value = item.Rate.ToString() });
            }

            fields.Add(new Field { Name = "DeductValue", Value = item.Value.ToString() });
            fields.Add(new Field { Name = "DeductUnitCode", Value = item.UnitId.ToString() });
            fields.Add(new Field { Name = "DeductSubjectCode", Value = item.ApplyOnId.ToString(), Type = new FieldType { Name = "System.Int32" } });
            fields.Add(new Field { Name = "MinDeductValue", Value = item.Min.ToString() });
            fields.Add(new Field { Name = "MinDeductUnitCode", Value = item.UnitMinId.ToString(), Type = new FieldType { Name = "System.Int32" } });
            fields.Add(new Field { Name = "MinDeductSubjectCode", Value = item.ApplyOnMinId.ToString(), Type = new FieldType { Name = "System.Int32" } });

            if (item.ApplyOnMaxId != null && item.ApplyOnMaxId != 0)
            {
                fields.Add(new Field { Name = "MaxDeductValue", Value = item.Max.ToString() });
                fields.Add(new Field { Name = "MaxDeductUnitCode", Value = item.UnitMaxId.ToString(), Type = new FieldType { Name = "System.Int32" } });
                fields.Add(new Field { Name = "MaxDeductSubjectCode", Value = item.ApplyOnMaxId.ToString(), Type = new FieldType { Name = "System.Int32" } });
            }

            if (item.CurrencyId != null)
            {
                fields.Add(new Field { Name = "CurrencyCode", Value = item.CurrencyId.ToString(), Type = new FieldType { Name = "System.Int32" } });
            }

            fields.Add(new Field { Name = "Description", Value = item.TotalDescription.ToString() });
            this.postEntity.Fields = fields;
            this.postEntity.KeyType = KeyType.NextValue;
            this.postEntity.Status = item.Status;
        }

        /// <summary>
        /// Obtiene listado de deducibles
        /// </summary>
        /// <returns>Modelo DeductibleViewModel</returns>
        private List<DeductibleViewModel> GetListDeductibles()
        {
            if (this.deductibleModel.Count == 0)
            {
                DeductiblesServiceModel deductiblesServiceModel = DelegateService.UnderwritingParamServiceWeb.GetDeductibles();
                if (deductiblesServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    this.deductibleModel = ModelAssembler.CreateDeductible(deductiblesServiceModel.DeductibleServiceModel);
                    return this.deductibleModel.OrderBy(x => x.TotalDescription).ToList();
                }
                else
                {
                    return null;
                }
            }

            return this.deductibleModel;
        }
    }
}