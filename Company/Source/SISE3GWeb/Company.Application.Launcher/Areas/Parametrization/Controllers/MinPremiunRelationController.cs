// -----------------------------------------------------------------------
// <copyright file="MiniumPremiunController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Wilfrido Heredia Carrera</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using Sistran.Company.Application.UnderwritingParamApplicationService.DTOs;
    using Sistran.Company.Application.Utilities.Enums;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;

    /// <summary>
    /// Ramo técnico
    /// </summary>
    public class MinPremiunRelationController : Controller
    {
        private static List<GenericViewModel> prefixes = new List<GenericViewModel>();
        private static List<GenericViewModel> branches = new List<GenericViewModel>();
        private static List<GenericViewModel> endorsementType = new List<GenericViewModel>();
        private static List<GenericViewModel> currencies = new List<GenericViewModel>();
        private static List<GenericViewModel> products = new List<GenericViewModel>();
        private static List<GenericViewModel> clave = new List<GenericViewModel>();
        private List<GenericViewModel> PoblateDropDownBoxFromEntityType(string entityType, Dictionary<string, string> filtro = null)
        {
            var postEntity = new Helpers.PostEntity() { EntityType = entityType };
            var dynamic = postEntity.CRUDCliente.Find(postEntity.EntityType, null, null);
            var dictionaryList = (filtro != null ? ModelAssembler.DynamicToDictionaryList(dynamic, filtro) : ModelAssembler.DynamicToDictionaryList(dynamic));
            var listEntityGeneric = ModelAssembler.CreateEntities(dictionaryList, entityType);
            return listEntityGeneric.OrderBy(x => x.DescriptionLong).ToList();
        }
        public ActionResult MinPremiunRelation()
        {
            return View();
        }
        public ActionResult GetPrefix()
        {
            if (prefixes.Count == 0)
            {
                prefixes = PoblateDropDownBoxFromEntityType("Sistran.Core.Application.Common.Entities.Prefix");
            }
            return new UifJsonResult(true, prefixes);
        }
        public ActionResult GetBranch()
        {
            var list = branches;
            if (branches.Count == 0)
            {
                branches = PoblateDropDownBoxFromEntityType("Sistran.Core.Application.Common.Entities.Branch");
                branches.Add(new GenericViewModel() { Id = 0, DescriptionLong = "Todas las Sucursales" });
                branches = branches.OrderBy(x => x.Id).ToList();
            }
            return new UifJsonResult(true, branches);
        }
        public ActionResult GetEndorsementType()
        {
            if (endorsementType.Count == 0)
            {
                endorsementType = PoblateDropDownBoxFromEntityType("Sistran.Core.Application.Parameters.Entities.EndorsementType");
            }
            return new UifJsonResult(true, endorsementType);
        }
        public ActionResult GetCurrency()
        {
            if (currencies.Count == 0)
            {
                currencies = PoblateDropDownBoxFromEntityType("Sistran.Core.Application.Common.Entities.Currency");
            }
            return new UifJsonResult(true, currencies);
        }
        public ActionResult GetProductType(int PrefixId)
        {
            var filtro = new Dictionary<string, string>();
            filtro.Add("PrefixCode", PrefixId.ToString());
            products = new List<GenericViewModel>();
            products = PoblateDropDownBoxFromEntityType("Sistran.Core.Application.Product.Entities.Product", filtro);

            return new UifJsonResult(true, products);
        }

        /// <summary>
        /// Obtiene informacion para cargar combo Clave dependiendo del ramo: autos o surety
        /// </summary>
        /// <param name="PrefixId"></param>
        /// <param name="PrefixDescription"></param>
        /// <returns></returns>
        public ActionResult GetClave(int productId, int PrefixId)
        {
            clave = new List<GenericViewModel>();

            if (PrefixId == (int)Helpers.Enums.PrefixTypeMinPremium.Vehicle)
            {
                var response = DelegateService.CompanyUnderwritingParamApplicationService.GetCoverageByPrefixId(productId);
                response = response.OrderBy(x => x.Id).ToList();
                response.ForEach(x => clave.Add(new GenericViewModel { Id = x.Id, DescriptionLong = x.Description }));
            }

            if (PrefixId == (int)Helpers.Enums.PrefixTypeMinPremium.Surety)
            {
                var response = DelegateService.CompanyUnderwritingParamApplicationService.GetAllMinRange();
                response = response.OrderBy(x => x.Id).ToList();
                response.ForEach(x => clave.Add(new GenericViewModel { Id = x.Id, DescriptionLong = x.Description }));
            }

            return new UifJsonResult(true, clave);
        }


        public ActionResult CreateMinPremiunRelation(MinPremiunRelationViewModel viewModel)
        {
            var result = new UifJsonResult(false, App_GlobalResources.Language.MinPremiunRelationSaveError);
            if (viewModel.Prefix.Id == (int)Helpers.Enums.PrefixTypeMinPremium.Surety || viewModel.Prefix.Id == (int)Helpers.Enums.PrefixTypeMinPremium.Vehicle)
            {
                var dto = ModelAssembler.MappMinPremiunRelationityViewModelToApplication(viewModel);

                dto = DelegateService.CompanyUnderwritingParamApplicationService.CreateApplicationMinPremiunRelation(dto);

                if (dto.ErrorDTO.ErrorType == ErrorType.Ok)
                {
                    viewModel = ModelAssembler.MappMinPremiunRelationityDtoToViewModel(dto);
                    result = new UifJsonResult(true, App_GlobalResources.Language.MinPremiunRelationSaveSuccessfully);
                }
            }
            else
            {
                result = new UifJsonResult(false, App_GlobalResources.Language.MinPremiunRelationPrefixWrong);
            }
            return result;
        }
        public ActionResult UpdateMinPremiunRelation(MinPremiunRelationViewModel viewModel)
        {
            var result = new UifJsonResult(false, App_GlobalResources.Language.MinPremiunRelationUpdateError);
            if (viewModel.Prefix.Id == (int)Helpers.Enums.PrefixTypeMinPremium.Surety || viewModel.Prefix.Id == (int)Helpers.Enums.PrefixTypeMinPremium.Vehicle)
            {

                var dto = ModelAssembler.MappMinPremiunRelationityViewModelToApplication(viewModel);

                dto = DelegateService.CompanyUnderwritingParamApplicationService.UpdateApplicationMinPremiunRelation(dto);

                if (dto.ErrorDTO.ErrorType == ErrorType.Ok)
                {
                    viewModel = ModelAssembler.MappMinPremiunRelationityDtoToViewModel(dto);
                    result = new UifJsonResult(true, App_GlobalResources.Language.MinPremiunRelationUpdateSuccessfully);
                }
            }
            else
            {
                result = new UifJsonResult(false, App_GlobalResources.Language.MinPremiunRelationPrefixWrong);
            }
            return result;
        }
        public ActionResult DeleteMinPremiunRelation(MinPremiunRelationViewModel viewModel)
        {
            var result = new UifJsonResult(false, App_GlobalResources.Language.MinPremiunRelationDeleteError);
            var dto = ModelAssembler.MappMinPremiunRelationityViewModelToApplication(viewModel);

            var dtoResult = DelegateService.CompanyUnderwritingParamApplicationService.DeleteApplicationMinPremiunRelation(dto);

            if (dtoResult.ErrorDTO.ErrorType == ErrorType.Ok)
            {
                result = new UifJsonResult(true, App_GlobalResources.Language.MinPremiunRelationDeleteSuccessfully);
            }

            return result;
        }
        public ActionResult GetAllMinPremiunRelation()
        {
            var result = new UifJsonResult(false, App_GlobalResources.Language.MinPremiunRelationQueryError);

            try
            {

                var dto = DelegateService.CompanyUnderwritingParamApplicationService.GetApplicationMinPremiunRelation();

                if (dto.ErrorDTO.ErrorType == ErrorType.Ok)
                {
                    var list = new List<MinPremiunRelationViewModel>();
                    dto.MinPremiunRelationDTO.ForEach(x => list.Add(ModelAssembler.MappMinPremiunRelationityDtoToViewModel(x)));

                    list = list.OrderBy(x => x.Prefix.Description).ThenBy(x => x.Id).Take(50).ToList();

                    result = new UifJsonResult(true, list.ToArray());
                }
            }
            catch
            {

            }

            return result;
        }
        public ActionResult GetMinPremiunRelationByPrefixAndProduct(int PrefixId, string ProductName)
        {
            var result = new UifJsonResult(false, App_GlobalResources.Language.MinPremiunRelationQueryError);

            try
            {

                var dto = DelegateService.CompanyUnderwritingParamApplicationService.GetApplicationMinPremiunRelationByPrefixIdAndProductName(PrefixId, ProductName);

                if (dto.ErrorDTO.ErrorType == ErrorType.Ok)
                {
                    var list = new List<MinPremiunRelationViewModel>();
                    dto.MinPremiunRelationDTO.ForEach(x => list.Add(ModelAssembler.MappMinPremiunRelationityDtoToViewModel(x)));

                    list = list.OrderBy(x => x.Prefix.Description).ThenBy(x => x.Id).Take(50).ToList();

                    result = new UifJsonResult(true, list.ToArray());
                }
            }
            catch
            {

            }

            return result;
        }
        /// <summary>
        /// Genera archivo excel prima minima
        /// </summary>
        /// <returns>Ruta del archivo</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                ExcelFileServiceModel excelFileServiceModel = new ExcelFileServiceModel()
                {
                    FileData = DelegateService.CompanyUnderwritingParamApplicationService.GenerateFileToMinPremiunRelation(App_GlobalResources.Language.FileNameMinPremiumRelation),
                    ErrorTypeService = ENUMSM.ErrorTypeService.Ok
                };
                if (excelFileServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;

                    var filenamefromPath = urlFile.Split(new char[] { '\\' }).Last();
                    return new UifJsonResult(true, new { Url = this.Url.Action("ShowExcelFile", "Parametrization") + "?url=" + urlFile, FileName = filenamefromPath });
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