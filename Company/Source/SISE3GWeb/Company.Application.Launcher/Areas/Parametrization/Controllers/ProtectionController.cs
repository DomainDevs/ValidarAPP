// -----------------------------------------------------------------------
// <copyright file="ProtectionController.cs" company="SISTRAN">
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
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;  
    using ENUMSE = Sistran.Core.Application.EntityServices.Enums;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;

    /// <summary>
    /// Clase amparos
    /// </summary>
    public class ProtectionController : Controller
    {
        /// <summary>
        /// Listado ProtectionViewModel
        /// </summary>
        private List<ProtectionViewModel> perils = new List<ProtectionViewModel>();

        /// <summary>
        /// Parametros de entidad 
        /// </summary>
        private PostEntity postEntity = new PostEntity { EntityType = "Sistran.Core.Application.Quotation.Entities.Peril", KeyType = KeyType.NextValue };
       
        /// <summary>
        /// Vista principal
        /// </summary>
        /// <returns>Retorna la vista principal</returns>
        public ActionResult Protection()
        {
            return this.View();
        }

        /// <summary>
        /// Vista busqueda avanzada
        /// </summary>
        /// <returns>Retorna la vista busqueda</returns>
        [HttpGet]
        public ActionResult ProtectionAdvancedSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene todos los amparos
        /// </summary>
        /// <returns>Listado de amparos</returns>
        public ActionResult GetProtectionsAll()
        {
            try
            {
                this.GetListPeril();
                return new UifJsonResult(true, this.perils);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProtection);
            }
        }

        /// <summary>
        /// Ejecuta las operaciones de la pantalla
        /// </summary>
        /// <param name="protectionParametrizations">Listado ProtectionViewModel</param>
        /// <returns>Mensaje y listado actualizado</returns>
        public ActionResult ExecuteOperation(List<ProtectionViewModel> protectionParametrizations)
        {
            int add = 0;
            int edit = 0;
            int delete = 0;
            string messageErrors = string.Empty;

            foreach (ProtectionViewModel item in protectionParametrizations)
            {
                try
                {
                    this.AssignPostEntity(item);
                    if (this.postEntity.Status == ENUMSE.StatusTypeService.Create)
                    {
                        this.postEntity = DelegateService.EntityServices.Create(this.postEntity);
                        add += 1;
                    }

                    if (this.postEntity.Status == ENUMSE.StatusTypeService.Delete)
                    {
                        int hasDependencies = DelegateService.UnderwritingParamServiceWeb.ValidatePeril(item.Id);

                        if (hasDependencies == 0)
                        {
                            DelegateService.EntityServices.Delete(this.postEntity);
                            delete += 1;
                        }
                        else
                        {
                            if (messageErrors != string.Empty)
                            {
                                messageErrors += " ";
                            }

                            messageErrors += string.Format(App_GlobalResources.Language.ErrorDeleteWithDependencies, item.Id);
                        }
                    }

                    if (this.postEntity.Status == ENUMSE.StatusTypeService.Update)
                    {
                        this.postEntity = DelegateService.EntityServices.Update(this.postEntity);
                        edit += 1;
                    }
                }
                catch (Exception ex)
                {
                    messageErrors = messageErrors + " " + string.Format(ex.Message + " " + item.Id + "<br/>");
                }

                this.perils = new List<ProtectionViewModel>();
                this.GetListPeril();
            }

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

            return new UifJsonResult(true, new { message = message, data = this.perils });
        }
            
        /// <summary>
        /// Obtiene amparos
        /// </summary>
        public void GetListPeril()
        {
            if (this.perils.Count == 0)
            {
                this.postEntity.Fields = null;
                List<PostEntity> entity = DelegateService.EntityServices.GetEntities(this.postEntity);
                this.perils = ModelAssembler.CreateProtections(entity).OrderBy(x => x.DescriptionLong).ToList();
            }
        }

        /// <summary>
        /// Genera Archivo excel amparos
        /// </summary>
        /// <returns>Archivo de excel</returns>
        [HttpPost]
        public ActionResult GenerateFileToExport()
        {
            try
            {
                this.GetListPeril();
                List<PerilServiceModel> perilsServiceModel = ModelAssembler.CreateProtections(this.perils);                
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToPeril(perilsServiceModel, App_GlobalResources.Language.FileNamePeril);
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
        /// <param name="item">modelo DeductibleViewModel</param>
        private void AssignPostEntity(ProtectionViewModel item)
        {
            List<Field> fields = new List<Field>();
            if (item.Id != 0)
            {
                fields.Add(new Field
                {
                    Name = "PerilCode",
                    Value = item.Id.ToString(),
                    Type = new FieldType { Name = "System.Int32" }
                });
            }

            fields.Add(new Field { Name = "Description", Value = item.DescriptionLong.ToUpper() });
            fields.Add(new Field { Name = "SmallDescription", Value = item.DescriptionShort.ToUpper() });

            this.postEntity.Fields = fields;
            this.postEntity.KeyType = KeyType.NextValue;
            this.postEntity.Status = item.Status;
        }
    }
}