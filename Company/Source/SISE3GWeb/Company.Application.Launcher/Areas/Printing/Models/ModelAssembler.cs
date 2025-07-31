// -----------------------------------------------------------------------
// <copyright file="ModelAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Desconocido</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Printing.Models
{
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Sistran.Company.Application.ModelServices.Models.UniquePerson;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.PrintingServices.Models;
    using Sistran.Core.Framework.UIF.Web.Helpers;

    /// <summary>
    /// Clase para mapear las entidades con los modelos. 
    /// </summary>
    public class ModelAssembler
    {
        /// <summary>
        /// método para mapear un PrinterModelsView a un FilterPolicy
        /// </summary>
        /// <param name="printerModel">Objecto de póliza</param>
        /// <returns>Un filtro de póliza</returns>
        public static FilterPolicy CreateFilterPolicy(PrinterModelsView printerModel)
        {
            return new FilterPolicy
            {
                BranchId = printerModel.BranchId,
                PrefixId = printerModel.PrefixId,
                PolicyNumber = Convert.ToDecimal(printerModel.PolicyNumber)
            };
        }

        #region LegalRepresentativeSing
        /// <summary>
        /// Método para mapear una lista de objetos de tipo LegalRepresentativeSing a LegalRepresentativeSingViewModel.
        /// </summary>
        /// <param name="legalRepresentativesSingServiceModel">lista de objetos de Firma representante legal.</param>
        /// <returns>lista de objetos de tipo LegalRepresentativeSingViewModel.</returns>
        public static List<LegalRepresentativeSingViewModel> GetLegalRepresentativeSings(LegalRepresentativesSingServiceModel legalRepresentativesSingServiceModel)
        {
            List<LegalRepresentativeSingViewModel> legalRepresentativeSingModelView = new List<LegalRepresentativeSingViewModel>();
            foreach (LegalRepresentativeSingServiceModel legalRepresentativeSingServiceModel in legalRepresentativesSingServiceModel.LegalRepresentativeSingServiceModel)
            {
                LegalRepresentativeSingViewModel legalRepresentativeSingViewModel = new LegalRepresentativeSingViewModel();
                
                string signatureImg = Convert.ToBase64String(legalRepresentativeSingServiceModel.SignatureImg);

                legalRepresentativeSingViewModel.CiaCode = legalRepresentativeSingServiceModel.CompanyTypeServiceModel.Id;
                legalRepresentativeSingViewModel.CiaDescription = legalRepresentativeSingServiceModel.CompanyTypeServiceModel.Description;
                legalRepresentativeSingViewModel.BranchTypeCode = legalRepresentativeSingServiceModel.BranchTypeServiceModel.Id;
                legalRepresentativeSingViewModel.BranchTypeDescription = legalRepresentativeSingServiceModel.BranchTypeServiceModel.Description;
                legalRepresentativeSingViewModel.CurrentFrom = legalRepresentativeSingServiceModel.CurrentFrom;
                legalRepresentativeSingViewModel.LegalRepresentative = legalRepresentativeSingServiceModel.LegalRepresentative;
                legalRepresentativeSingViewModel.PathSignatureImg = legalRepresentativeSingServiceModel.PathSignatureImg;
                legalRepresentativeSingViewModel.SignatureImg = signatureImg;
                legalRepresentativeSingViewModel.UserId = legalRepresentativeSingServiceModel.UserId;

                legalRepresentativeSingModelView.Add(legalRepresentativeSingViewModel);
            }

            return legalRepresentativeSingModelView;
        }

        /// <summary>
        /// Método para mapear una lista de objetos de tipo LegalRepresentativeSingViewModel a LegalRepresentativeSingServiceModel.
        /// </summary>
        /// <param name="legalRepresentativeSingViewModels">lista de objetos de tipo LegalRepresentativeSingViewModel.</param>
        /// /// <param name="statusTypeService">Estado de servicio.</param>
        /// <returns>lista de LegalRepresentativeSingServiceModel.</returns>
        /// <returns></returns>
        public static List<LegalRepresentativeSingServiceModel> MappLegalRepresentativeSingToSave(List<LegalRepresentativeSingViewModel> legalRepresentativeSingViewModels, StatusTypeService statusTypeService)
        {
            if (legalRepresentativeSingViewModels == null)
            {
                return null;
            }

            List<LegalRepresentativeSingServiceModel> listLegalRepresentativeSingServiceModel = new List<LegalRepresentativeSingServiceModel>();
            foreach (LegalRepresentativeSingViewModel model in legalRepresentativeSingViewModels)
            {
                LegalRepresentativeSingServiceModel legalRepresentativeSingServiceModel = new LegalRepresentativeSingServiceModel();
                ParametricServiceModel parametricServiceModel = new ParametricServiceModel();
                CompanyTypeServiceModel companyTypeServiceModel = new CompanyTypeServiceModel();
                BranchTypeServiceModel branchTypeServiceModel = new BranchTypeServiceModel();

                byte[] imgBytes = Convert.FromBase64String(model.SignatureImg);

                legalRepresentativeSingServiceModel.CurrentFrom = model.CurrentFrom;
                legalRepresentativeSingServiceModel.LegalRepresentative = model.LegalRepresentative;
                legalRepresentativeSingServiceModel.PathSignatureImg = SavePicture(imgBytes, model.CiaCode.ToString(), model.BranchTypeCode.ToString(), model.CurrentFrom.ToString());
                legalRepresentativeSingServiceModel.SignatureImg = imgBytes;
                legalRepresentativeSingServiceModel.UserId = SessionHelper.GetUserName();

                companyTypeServiceModel.Id = model.CiaCode;
                legalRepresentativeSingServiceModel.CompanyTypeServiceModel = companyTypeServiceModel;

                branchTypeServiceModel.Id = model.BranchTypeCode;
                legalRepresentativeSingServiceModel.BranchTypeServiceModel = branchTypeServiceModel;
                
                parametricServiceModel.StatusTypeService = statusTypeService;
                ErrorServiceModel errorServiceModel = new ErrorServiceModel();
                errorServiceModel.ErrorTypeService = ErrorTypeService.Ok;
                errorServiceModel.ErrorDescription = new List<string>();
                parametricServiceModel.ErrorServiceModel = errorServiceModel;
                legalRepresentativeSingServiceModel.ParametricServiceModel = parametricServiceModel;

                listLegalRepresentativeSingServiceModel.Add(legalRepresentativeSingServiceModel);
            }

            return listLegalRepresentativeSingServiceModel;
        }

        /// <summary>
        /// Método para guardar la imagen
        /// </summary>
        /// <param name="imgBytes">Bytes de la imagen</param>
        /// <param name="ciaCode">Tipo compañia</param>
        /// <param name="branchTypeCode">Tipo sucursal</param>
        /// <param name="currentFrom">Fecha actual</param>
        /// <returns>Ruta de la imagen guardada</returns>
        private static string SavePicture(byte[] imgBytes, string ciaCode, string branchTypeCode, string currentFrom)
        {
            string signName = string.Empty;
            currentFrom = currentFrom.Replace("/", "_");
            var date = currentFrom.Split(' ');
            currentFrom = date[0];

            signName = "Firma_Cia0" + ciaCode + "_TS0" + branchTypeCode + "_" + currentFrom + ".jpg";
            
            if (!Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["uploaded_images"]))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["uploaded_images"]);
            }

            string ruta = string.Format("{0}{1}", System.Configuration.ConfigurationManager.AppSettings["uploaded_images"], signName);
            Image image;
            using (MemoryStream ms = new MemoryStream(imgBytes))
            {
                image = Image.FromStream(ms);
                image.Save(ruta);
            }

            return ruta;
        }
        #endregion
    }
}