// -----------------------------------------------------------------------
// <copyright file="DetailController.cs" company="SISTRAN">
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
    using Sistran.Core.Application.EntityServices.Models;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Helpers;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;   
    using ENUMUTIL = Sistran.Core.Services.UtilitiesServices.Enums;
    using ENTEN = Sistran.Core.Application.EntityServices.Enums;
    using MODEN = Sistran.Core.Application.ModelServices.Enums;
    using MODMO = Sistran.Core.Application.ModelServices.Models.Param;
    using UNDMO = Sistran.Core.Application.ModelServices.Models.UnderwritingParam;

    /// <summary>
    /// Controlador para Detalle
    /// </summary>
    public class DetailController : Controller
    {
        /// <summary>
        /// Parametros de entidad 
        /// </summary>
        private Application.EntityServices.Models.PostEntity postEntity = new Application.EntityServices.Models.PostEntity { EntityType = "Sistran.Core.Application.Quotation.Entities.Detail", KeyType = KeyType.Autonumber };

        /// <summary>
        /// Modelo deducible
        /// </summary>
        private List<DetailViewModel> detailViewModel = new List<DetailViewModel>();

        /// <summary>
        /// Modelo DetailsServiceModel
        /// </summary>
        private UNDMO.DetailsServiceModel detailServiceModel = new UNDMO.DetailsServiceModel();

        /// <summary>
        /// Vista principal
        /// </summary>
        /// <returns>Retorna la vista principal</returns>
        public ActionResult Detail()
        {
            return this.View();
        }
        ///pendiente
        /// <summary>
        /// Obtiene los tipos de detalle
        /// </summary>
        /// <returns>Modelo LinesBusinessServiceQueryModel</returns>
        [HttpGet]
        public ActionResult GetDetailTypes()
        {
            UNDMO.DetailTypesServiceQueryModel detailTypesServiceQueryModel =  DelegateService.UnderwritingParamServiceWeb.GetDetailTypes();
            if (detailTypesServiceQueryModel.ErrorTypeService == MODEN.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, detailTypesServiceQueryModel.DetailTypeServiceQueryModel.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { detailTypesServiceQueryModel.ErrorTypeService, detailTypesServiceQueryModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Obtiene los tipos de tasa
        /// </summary>
        /// <returns>Enum RateType</returns>
        [HttpGet]
        public ActionResult GetRateTypes()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<MODEN.RateType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryRateTypes);
            }
        }

        /// <summary>
        /// Retorna el listado de detalles
        /// </summary>
        /// <returns>Modelo detailServiceModel</returns>
        [HttpGet]
        public ActionResult GetDetails()
        {
            UNDMO.DetailsServiceModel detailServiceModel = this.GetDetailsViewModel();
            if (this.detailServiceModel.ErrorTypeService == MODEN.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, this.detailViewModel);
            }
            else
            {
                return new UifJsonResult(false, new { this.detailServiceModel.ErrorTypeService, this.detailServiceModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Metodo ejecutar las operaciones del CRUD
        /// </summary>
        /// <param name="details">Listado DetailViewModel</param>
        /// <returns>Ejecuta operaciones de CRUD en base de datos</returns>
        [HttpPost]
        public ActionResult ExecuteOperation(List<DetailViewModel> details)
        {
            string messageErrors = string.Empty;
            int add = 0;
            int edit = 0;
            int delete = 0;

            foreach (DetailViewModel item in details)
            {
                List<Field> fields = new List<Field>();
                try
                {
                    if ((ENUMUTIL.ParametrizationStatus)item.Status == ENUMUTIL.ParametrizationStatus.Delete)
                    {
                        int hasDependencies = DelegateService.UnderwritingParamServiceWeb.ValidateDetail(item.Id);

                        if (hasDependencies == 0)
                        {
                            fields.Add(new Field { Name = "DetailId", Value = item.Id.ToString() });
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

                            messageErrors += string.Format(App_GlobalResources.Language.ErrorDeleteWithDependencies, item.Description);
                        }
                    }
                    else
                    {
                        this.AssignPostEntity(item);
                        if (this.postEntity.Status == ENTEN.StatusTypeService.Create)
                        {
                            this.postEntity = DelegateService.EntityServices.Create(this.postEntity);
                            Field field = this.postEntity.Fields.First(x => x.Name == "DetailId");
                            if (field.Value != "0")
                            {
                                add += 1;
                            }
                        }
                        else if (this.postEntity.Status == ENTEN.StatusTypeService.Update)
                        {
                            DelegateService.EntityServices.Update(this.postEntity);
                            edit += 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    messageErrors = messageErrors + " " + string.Format(ex.Message + " " + item.Description + "<br/>");
                }
            }

            this.detailViewModel = new List<DetailViewModel>();
            this.GetDetailsViewModel();

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

            var result = this.detailViewModel.OrderBy(x => x.Description).ToList();
            return new UifJsonResult(true, new { message = message, data = result });
        }
        
        /// <summary>
        /// Genera archivo excel de detalle
        /// </summary>
        /// <returns>Arhivo de excel de detalle</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                this.GetDetailsViewModel();
                MODMO.ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToDetail(this.detailServiceModel.DetailServiceModel, App_GlobalResources.Language.PrechargeDetail);
                if (excelFileServiceModel.ErrorTypeService == MODEN.ErrorTypeService.Ok)
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
        /// Consulta todos los detalles en base de datos
        /// </summary>
        /// <returns>Modelo DetailsServiceModel</returns>
        private UNDMO.DetailsServiceModel GetDetailsViewModel()
        {
            this.detailServiceModel = DelegateService.UnderwritingParamServiceWeb.GetParametrizationDetails();
            if (this.detailServiceModel.ErrorTypeService == MODEN.ErrorTypeService.Ok)
            {
                this.detailViewModel = ModelAssembler.CreateDetailsViewModel(this.detailServiceModel.DetailServiceModel).OrderBy(x => x.Description).ToList();
            }

            return this.detailServiceModel;
        }

        /// <summary>
        /// Asigna los campos a la entidad
        /// </summary>
        /// <param name="item">modelo DetailViewModel</param>
        private void AssignPostEntity(DetailViewModel item)
        {
            List<Field> fields = new List<Field>();
            if (item.Id != 0)
            {
                fields.Add(new Field { Name = "DetailId", Value = item.Id.ToString() });
            }

            fields.Add(new Field { Name = "Description", Value = item.Description.ToString() });
            fields.Add(new Field { Name = "DetailTypeCode", Value = item.DetailTypeId.ToString() });
            fields.Add(new Field { Name = "Enabled", Value = item.Enabled.ToString(), Type = new FieldType { Name = "System.Boolean" } });
            if (item.RateTypeId != null)
            {
                fields.Add(new Field { Name = "RateTypeCode", Value = item.RateTypeId.ToString(), Type = new FieldType { Name = "System.Int32" } });
            }

            if (item.Rate != null)
            {
                fields.Add(new Field { Name = "Rate", Value = item.Rate.ToString(), Type = new FieldType { Name = "System.Decimal" } });
            }

            if (item.SublimitAmt != null)
            {
                fields.Add(new Field { Name = "SublimitAmount", Value = item.SublimitAmt.ToString(), Type = new FieldType { Name = "System.Decimal" } });
            }

            this.postEntity.Fields = fields;
            this.postEntity.KeyType = KeyType.Autonumber;
            this.postEntity.Status = item.Status;
        }
    }
}