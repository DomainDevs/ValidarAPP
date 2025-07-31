//-----------------------------------------------------------------------
// <copyright file="InsuredObjectsController.cs" company="Sistran">
// Copyright (c) Sistran. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
//-----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Sistran.Core.Application.CommonService.Enums;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Services.UtilitiesServices;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using UTIMODEL=Sistran.Core.Services.UtilitiesServices.Models;
    using ENUMUTIL = Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Application.EntityServices.Models;
    using Sistran.Core.Application.EntityServices.Enums;

    /// <summary>
    /// Controlador de objetos del seguro
    /// </summary>
    public class InsuredObjectController : Controller
    {
        /// <summary>
        /// Modelo objetos de seguro
        /// </summary>
        private List<InsurencesObjectsViewModel> insurencesObjects = new List<InsurencesObjectsViewModel>();

        /// <summary>
        /// Parametros de entidad 
        /// </summary> 
        private PostEntity postEntity = new PostEntity { EntityType = "Sistran.Core.Application.Quotation.Entities.InsuredObject", KeyType = KeyType.Autonumber };

        /// <summary>
        /// Carga la vista principal
        /// </summary>
        /// <returns>Action result</returns> 
        public ActionResult InsuredObject()
        {
            return this.View();
        }

        /// <summary>
        /// Carga vista emergente
        /// </summary>
        /// <returns>Action result</returns>
        public ActionResult InsuredObjectAdvancedSearch()
        {
            return this.View();
        }

        [HttpPost]

        /// <summary>
        /// Lista los objetos de seguro
        /// </summary>  
        /// <param name="description">descripcion de objetos de seguro</param>
        /// <returns> retorna Lista de objetos de seguro </returns>
        public ActionResult GetInsuredObjectByDescription(string description)
        {
            try
            {
                this.GetListInsuredObject(description);
                return new UifJsonResult(true, this.insurencesObjects.OrderBy(x => x.Description).ToList());
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrMsgRangeEntityNotFound);
            }
        }

        [HttpPost]

        /// <summary>
        /// CRUD objetos de seguro
        /// </summary>  
        /// <param name="insurencesObjects">modelo de objetos de seguro</param>
        /// <returns> retorna mwnsaje de objetos de seguro </returns>
        public ActionResult SaveInsurencesObjects(List<InsurencesObjectsViewModel> insurencesObjects)
        {
            string messageErrors = string.Empty;
            int add = 0;
            int edit = 0;
            int delete = 0;

            List<Field> fields = new List<Field>();
            foreach (InsurencesObjectsViewModel item in insurencesObjects)
            {
                try
                {
                    if ((ENUMUTIL.ParametrizationStatus)item.Status == ENUMUTIL.ParametrizationStatus.Delete)
                    {
                        int hasDependencies = DelegateService.UnderwritingParamServiceWeb.ValidateInsuredObject(item.Id);

                        if (hasDependencies == 0)
                        {
                            fields = new List<Field>();
                            fields.Add(new Field { Name = "InsuredObjectId", Value = item.Id.ToString() });
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

                            messageErrors += string.Format(App_GlobalResources.Language.ErroDeleteInformation, item.Id);
                        }
                    }
                    else
                    {
                        this.AssignPostEntity(item);
                        if (this.postEntity.Status == StatusTypeService.Create)
                        {
                            this.postEntity = DelegateService.EntityServices.Create(this.postEntity);
                            Field field = this.postEntity.Fields.First(x => x.Name == "InsuredObjectId");
                            if (field.Value != "0")
                            {
                                add += 1;
                            }
                        }
                        else if (this.postEntity.Status == StatusTypeService.Update)
                        {
                            DelegateService.EntityServices.Update(this.postEntity);
                            edit += 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    messageErrors = messageErrors + " " + string.Format(ex.Message + " " + item.Id + "<br/>");
                }
            }

            this.insurencesObjects = new List<InsurencesObjectsViewModel>();
            this.GetListInsuredObject(null);

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

            var result = this.insurencesObjects.OrderBy(x => x.Id).ToList();
            return new UifJsonResult(true, new { message = message, data = result });
        }

        /// <summary>
        /// Genera archivo excel de objetos de seguro
        /// </summary>
        /// <returns>Archivo excel</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                string descripcion = null;
                this.GetListInsuredObject(descripcion);
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToInsuredObject(ModelAssembler.CreateInsuredObjectServiceModel(this.insurencesObjects), App_GlobalResources.Language.LabelInsuredObject);
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
        /// Asigna los campos a la entidad
        /// </summary>
        /// <param name="item">modelo InsurencesObjectsViewModel</param>
        private void AssignPostEntity(InsurencesObjectsViewModel item)
        {
            List<Field> fields = new List<Field>();
            if (item.Id != 0)
            {
                fields.Add(new Field { Name = "InsuredObjectId", Value = item.Id.ToString() });
            }

            fields.Add(new Field { Name = "Description", Value = item.Description });
            fields.Add(new Field { Name = "SmallDescription", Value = item.SmallDescription });
            fields.Add(new Field { Name = "IsDeclarative", Value = item.IsDeraclarative.ToString() });

            this.postEntity.Fields = fields;
            this.postEntity.KeyType = KeyType.Autonumber;
            this.postEntity.Status = (StatusTypeService)item.Status;
        }

        /// <summary>
        /// obtiene lista de objetos de seguro
        /// </summary>
        /// <param name="description">descripcion de objetos de seguro</param>
        /// <returns> retorna la vista de objetos de seguro </returns>
        private List<InsurencesObjectsViewModel> GetListInsuredObject(string description)
        {
            if (this.insurencesObjects.Count == 0)
            {
                InsurancesObjectsServiceModel InsuredObjectsServiceModel = DelegateService.UnderwritingParamServiceWeb.GetInsuredObjectServiceModelByDescription(description);

                this.insurencesObjects = ModelAssembler.CreateInsuredObjectViewModel(InsuredObjectsServiceModel.InsuredObjectServiceModel);
                return this.insurencesObjects.OrderBy(x => x.Description).ToList();
            }

            return this.insurencesObjects;
        }
    }
}