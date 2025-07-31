// -----------------------------------------------------------------------
// <copyright file="Product2GController.cs" company="SISTRAN">
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
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
   
    /// <summary>
    /// controlador de productos 2G
    /// </summary>
    public class Product2GController : Controller
    {
        #region Propiedades

        /// <summary>
        /// Tiene la lista de productos 2G
        /// </summary>
        private static List<Product2GModel> product2GModel = new List<Product2GModel>();

        /// <summary>
        /// Tiene la lista de ramo comercial
        /// </summary>
        private static List<Prefix> prefixes = new List<Prefix>();

        ///// <summary>
        ///// Lista de productos
        ///// </summary>
        //private static List<Product> prodctModels = new List<Product>();
        #endregion

        /// <summary>
        /// Inicia la vista tipo de Productos 2G
        /// </summary>
        /// <returns>Retorna vista</returns>
        public ActionResult Product2G()
        {
            return this.View();
        }

        /// <summary>
        /// Inicia la vista tipo de busqueda avanzada de productos 2G
        /// </summary>
        /// <returns>Retorna vista</returns>
        public ActionResult Product2GAdvancedSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene lso prductos 2G
        /// </summary>
        /// <returns>Retorna json con la informacion</returns>
        public ActionResult GetProduct2G()
        {
            try
            {

                product2GModel = null; //Parametrization.Models.ModelAssembler.GetProduct2g(DelegateService.underwritingService.GetProduct2g());

                prefixes = DelegateService.commonService.GetAllPrefix();
                for (int i = 0; i < product2GModel.Count; i++)
                {
                    product2GModel[i].Prefix = prefixes.FirstOrDefault(p => p.Id == product2GModel[i].PrefixCode);
                    product2GModel[i].PrefixDescription = prefixes.FirstOrDefault(p => p.Id == product2GModel[i].PrefixCode).Description;
                }

                return new UifJsonResult(true, product2GModel.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.GetProduct2G);
            }
        }

        /// <summary>
        /// Obtiene productos 2G por codigo de ramo comercial
        /// </summary>
        /// <param name="prefixCode">Codigo de ramo comercial</param>
        /// <returns>Retorna json con la informacion</returns>
        public ActionResult GetProduct2GByPrefixCode(int prefixCode)
        {
            try
            {
                List<Product2GModel> modelProduct2G = new List<Product2GModel>();
                modelProduct2G = (from a in product2GModel where a.PrefixCode == prefixCode select a).ToList();
                for (int i = 0; i < modelProduct2G.Count; i++)
                {
                    modelProduct2G[i].Prefix = prefixes.FirstOrDefault(p => p.Id == modelProduct2G[i].PrefixCode);
                    modelProduct2G[i].PrefixDescription = prefixes.FirstOrDefault(p => p.Id == modelProduct2G[i].PrefixCode).Description;
                }

                return new UifJsonResult(true, modelProduct2G.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.GetProduct2G);
            }
        }

        /// <summary>
        /// Guarda los productos 2G (Crea, actualiza, elimina)
        /// </summary>
        /// <param name="product2GUpdate">Lista de productos</param>
        /// <returns>Retorna json con la informacion</returns>
        public ActionResult SaveProduct2G(List<Product2GModel> product2GUpdate)
        {
            try
            {
                List<Product2GModel> product2G = Parametrization.Models.ModelAssembler.CreateProduct2g(product2GUpdate);
                //List<string> product2GResponse = DelegateService.UnderwritingServiceCore.CreateProduct2g(product2G);
                //product2GModel = Parametrization.Models.ModelAssembler.GetProduct2g(DelegateService.UnderwritingServiceCore.GetProduct2g());
                prefixes = DelegateService.commonService.GetAllPrefix();
                for (int i = 0; i < product2GModel.Count; i++)
                {
                    product2GModel[i].Prefix = prefixes.FirstOrDefault(p => p.Id == product2GModel[i].PrefixCode);
                    product2GModel[i].PrefixDescription = prefixes.FirstOrDefault(p => p.Id == product2GModel[i].PrefixCode).Description;
                }

                object[] result = new object[2];
                //result[0] = product2GResponse;
                //result[1] = product2GModel;
                return new UifJsonResult(true, result);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveProduct2G);
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
                this.GetProduct2G();
                string urlFile = "";//DelegateService.underwritingService.GenerateFileToProducto2G(ModelAssembler.CreateProduct2g(product2GModel), App_GlobalResources.Language.FileNameProduct2G);

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
    }
}