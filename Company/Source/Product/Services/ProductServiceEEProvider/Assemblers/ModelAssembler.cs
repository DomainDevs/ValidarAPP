namespace Sistran.Company.Application.ProductServices.EEProvider.Assemblers
{
    using AutoMapper;
    using Sistran.Company.Application.CommonServices.Models;
    using Sistran.Company.Application.Product.Entities;
    using Sistran.Company.Application.ProductServices.Models;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.ProductServices.Models;
    using System.Collections.Generic;
    public class ModelAssembler
    {
        /// <summary>
        /// Crear Lista de Cia Product
        /// </summary>
        /// <param name="coreProduct">The core product.</param>
        /// <returns></returns>
        public static List<CompanyProduct> CreateCompanyProducts(List<Product> coreProducts)
        {
            var config = new MapperConfiguration(cfg =>
              {
                  cfg.CreateMap<Product, CompanyProduct>();
                  cfg.CreateMap<CoveredRisk, CompanyCoveredRisk>();
                  cfg.CreateMap<Prefix, CompanyPrefix>();
              });
          
            return config.CreateMapper().Map<List<Product>, List<CompanyProduct>>(coreProducts);
        }


        /// <summary>
        /// Creates the company product.
        /// </summary>
        /// <param name="coreProduct">The core product.</param>
        /// <returns></returns>
        public static CompanyProduct CreateCompanyProduct(Product coreProduct)
        {
            var config = new MapperConfiguration(cfg =>
             {
                 cfg.CreateMap<Product, CompanyProduct>();
                 cfg.CreateMap<CoveredRisk, CompanyCoveredRisk>();
                 cfg.CreateMap<Prefix, CompanyPrefix>();
             });
           
            return config.CreateMapper().Map<Product, CompanyProduct>(coreProduct);
        }

        public static Product CreateProduct(CompanyProduct companyProduct)
        {
            Product product = new Product();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(companyProduct.GetType(), product.GetType());

            });
          
            return config.CreateMapper().Map<CompanyProduct, Product>(companyProduct);
        }
        #region CptProduct

        /// <summary>
        /// Crear una puntuación de Ramo
        /// </summary>
        /// <param name="cptProduct">The CptProduct.</param>
        /// <returns>Product</returns>
        //public static CompanyProduct CreateCptProduct(CptProduct cptProduct, Product productCore)
        //{
        //    CompanyProduct product = HelperAssembler.CreateObjectMappingEqualProperties<Product, CompanyProduct>(productCore);

        //    if (product != null && productCore != null && cptProduct != null)
        //    {
        //        product.IsPoliticalProduct = cptProduct.IsPoliticalProduct;
        //        product.IncentiveAmt = cptProduct.IncentiveAmount;
        //        product.IsEnabled = cptProduct.IsEnabled;
        //        product.IsScore = cptProduct.IsScore;
        //        product.IsFine = cptProduct.IsFine;
        //        product.IsFasecolda = cptProduct.IsFasecolda;
        //        product.ValidDaysTempPolicy = cptProduct.ValidDaysTempPolicy;
        //        product.ValidDaysTempQuote = cptProduct.ValidDaysTempQuote;
        //    }

        //    return product;
        //}

        /// <summary>
        /// CreateCoProduct
        /// </summary>
        /// <param name="coProduct">The coProduct.</param>
        /// <param name="productCore">The productCore.</param>
        /// <returns>Product</returns>
        public static CompanyProduct CreateCoProduct(CoProduct coProduct, Product productCore)
        {
            CompanyProduct product = HelperAssembler.CreateObjectMappingEqualProperties<Product, CompanyProduct>(productCore);

            if (product != null && productCore != null && coProduct != null)
            {
                product.CalculateMinPremium = coProduct.CalculateMinPremium.GetValueOrDefault();
            }

            return product;
        }

        #endregion CptProduct

    }
}
