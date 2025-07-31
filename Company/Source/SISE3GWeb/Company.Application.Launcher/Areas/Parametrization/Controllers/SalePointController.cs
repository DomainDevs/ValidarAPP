//-----------------------------------------------------------------------
// <copyright file="SalePointController.cs" company="Sistran">
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
    using Application.ModelServices.Models.CommonParam;
    using Sistran.Company.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.CommonService.Enums;
    //using Sistran.Core.Application.EntityServices.Enums;
    using Sistran.Core.Application.EntityServices.Models;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using ENUMUTIL = Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.CRUD.Service.Models;
    using Sistran.CRUD.Services.Models;
    /// <summary>
    /// Controlador de objetos del seguro
    /// </summary>
    public class SalePointController : Controller
    {
        /// <summary>
        /// Modelo punto de venta
        /// </summary>
        private List<SalePointViewModel> salePoint = new List<SalePointViewModel>();

        /// <summary>
        /// Parametros de entidad 
        /// </summary> 
        private PostEntity postEntity = new PostEntity { EntityType = "Sistran.Core.Application.Common.Entities.SalePoint", KeyType = KeyType.NextValue };
        private Helpers.PostEntity entityCRUD = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.SalePoint" };

        /// <summary>
        /// Carga la vista principal
        /// </summary>
        /// <returns>Action result</returns> 
        public ActionResult SalePoint()
        {
            return this.View();
        }

        /// <summary>
        /// Carga vista emergente
        /// </summary>
        /// <returns>Action result</returns>
        public ActionResult SalePointAdvancedSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Listado de sucursales
        /// </summary>
        /// <returns>Modelo BranchesServiceQueryModel</returns>
        [HttpGet]
        public ActionResult GetBranch()
        {
            BranchesServiceQueryModel branchesServiceQueryModel = DelegateService.companyCommonParamService.GetBranch();
            if (branchesServiceQueryModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, branchesServiceQueryModel.BranchServiceQueryModel.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { branchesServiceQueryModel.ErrorTypeService, branchesServiceQueryModel.ErrorDescription });
            }
        }

        [HttpPost]

        /// <summary>
        /// Lista los punto de venta
        /// </summary>  
        /// <param name="description">descripcion de punto de venta</param>
        /// <returns> retorna Lista de punto de venta </returns>
        public ActionResult GetSalePointByDescription(string description)
        {
            try
            {
                this.GetListSalePointDescription(description);
                return new UifJsonResult(true, this.salePoint.Take(50).OrderBy(x => x.Description).ToList());
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrMsgRangeEntityNotFound);
            }
        }

        [HttpPost]

        /// <summary>
        /// Lista los punto de venta
        /// </summary>  
        /// <param name="description">descripcion de punto de venta</param>
        /// <returns> retorna Lista de punto de venta </returns>
        public ActionResult GetSalePointsByBranchCode(int branchCode)
        {
            try
            {
                this.GetSalePointsByBranchId(branchCode);
                return new UifJsonResult(true, this.salePoint.OrderBy(x => x.Description).ToList());
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrMsgRangeEntityNotFound);
            }
        }



        [HttpGet]

        /// <summary>
        /// Lista los punto de venta
        /// </summary>        
        /// <returns> retorna Lista de punto de venta </returns>
        public ActionResult GetSalePoint()
        {
            try
            {
                this.GetListSalePoint();
                return new UifJsonResult(true, this.salePoint.OrderBy(x => x.Description).ToList());
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrMsgRangeEntityNotFound);
            }
        }

        [HttpPost]

        /// <summary>
        /// CRUD punto de ventas
        /// </summary>  
        /// <param name="salePoint">modelo de punto de venta</param>
        /// <returns> retorna mensaje de punto de venta </returns>
        public ActionResult SaveSalePointes(List<SalePointViewModel> salePoint)
        {
            string messageErrors = string.Empty;
            int add = 0;
            int edit = 0;
            int delete = 0;

            foreach (SalePointViewModel item in salePoint)
            {
                try
                {
                    if ((ENUMUTIL.ParametrizationStatus)item.Status == ENUMUTIL.ParametrizationStatus.Delete)
                    {
                        int hasDependencies = DelegateService.companyCommonParamService.ValidateSalePoint(item.Id, item.BranchId);

                        if (hasDependencies == 0)
                        {
                            var criteriaBuilder = new CriteriaBuilder();
                            criteriaBuilder.CreateExpression();
                            criteriaBuilder.AddRule("SalePointCode", Operations.Equal, item.Id.ToString());
                            criteriaBuilder.AddRule("BranchCode", Operations.Equal, item.BranchId.ToString());
                            var expression = criteriaBuilder.Build();
                            entityCRUD.CRUDCliente.Delete(entityCRUD.EntityType, expression);
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
                        if (this.entityCRUD.Status == StatusTypeService.Create)
                        {
                            if (!DelegateService.parametrizationAplicationService.ValidateExistIdSalesPoint(item.Id, item.BranchId))
                            {
                                entityCRUD.Fields = ModelAssembler.DynamicToDictionary(entityCRUD.CRUDCliente.Create(entityCRUD.EntityType, entityCRUD.Fields));
                                string field = entityCRUD.Fields.First(x => x.Key == "SalePointCode").Value;
                                if (field != "0")
                                {
                                    add += 1;
                                }
                            }
                            else
                            {
                                add += 2;
                            }
                        }
                        else if (this.entityCRUD.Status == StatusTypeService.Update)
                        {
                            this.entityCRUD.CRUDCliente.Update(this.entityCRUD.EntityType, this.entityCRUD.Fields);
                            
                            edit += 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    messageErrors = messageErrors + " " + string.Format(ex.Message + " " + item.Id + "<br/>");
                }
            }

            this.salePoint = new List<SalePointViewModel>();
            this.GetListSalePoint();

            string message = string.Empty;
            if (add > 0)
            {
                if (add == 2)
                    message += string.Format(App_GlobalResources.Language.ErrorExistCodeSalePoint);
                else 
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

            var result = this.salePoint.OrderBy(x => x.Id).ToList();
            return new UifJsonResult(true, new { message = message, data = result });
        }

        /// <summary>
        /// Genera archivo excel de punto de venta
        /// </summary>
        /// <returns>Archivo excel</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                this.GetListSalePoint();
                ExcelFileServiceModel excelFileServiceModel = DelegateService.companyCommonParamService.GenerateFileToSalePoint(ModelAssembler.CreateSalePointesServiceModel(this.salePoint), App_GlobalResources.Language.LabelSalePoint);
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
        /// <param name="item">modelo SalePointViewModel</param>
        private void AssignPostEntity(SalePointViewModel item)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            fields.Add("SalePointCode", item.Id != 0 ? item.Id.ToString() : "");
            fields.Add("Description", item.Description);
            fields.Add("SmallDescription", item.SmallDescription);
            fields.Add("BranchCode", item.BranchId.ToString());
            fields.Add("Enabled", item.Enabled.ToString());


            entityCRUD.Fields = fields;
            entityCRUD.Status = (StatusTypeService)item.Status;

        }

        /// <summary>
        /// obtiene lista de punto de venta
        /// </summary>
        /// <param name="description">descripcion de punto de venta</param>
        /// <returns> retorna la vista de punto de venta </returns>
        private List<SalePointViewModel> GetListSalePoint()
        {
            if (this.salePoint.Count == 0)
            {
                SalePointsServiceModel salePointServiceModel = DelegateService.companyCommonParamService.GetSalePointes();

                this.salePoint = ModelAssembler.CreateSalePointesViewModel(salePointServiceModel.SalePointServiceModel);
                return this.salePoint.OrderBy(x => x.Description).ToList();
            }

            return this.salePoint;
        }

        /// <summary>
        /// obtiene lista de punto de venta
        /// </summary>
        /// <param name="description">descripcion de punto de venta</param>
        /// <returns> retorna la vista de punto de venta </returns>
        private List<SalePointViewModel> GetListSalePointDescription(string description)
        {
            if (this.salePoint.Count == 0)
            {
                SalePointsServiceModel salePointServiceModel = DelegateService.companyCommonParamService.GetSalePointServiceModel(description);
                this.salePoint = ModelAssembler.CreateSalePointesViewModel(salePointServiceModel.SalePointServiceModel);
                return this.salePoint.Take(50).OrderBy(x => x.Description).ToList();
            }

            return this.salePoint;
        }

        /// <summary>
        /// obtiene lista de punto de venta
        /// </summary>
        /// <param name="description">descripcion de punto de venta</param>
        /// <returns> retorna la vista de punto de venta </returns>
        private List<SalePointViewModel> GetSalePointsByBranchId(int branchId)
        {
            if (this.salePoint.Count == 0)
            {
                SalePointsServiceModel salePointServiceModel = DelegateService.companyCommonParamService.GetSalePointsByBranchCode(branchId);

                this.salePoint = ModelAssembler.CreateSalePointesViewModel(salePointServiceModel.SalePointServiceModel);
                return this.salePoint.OrderBy(x => x.Description).ToList();
            }

            return this.salePoint;
        }
    }
}