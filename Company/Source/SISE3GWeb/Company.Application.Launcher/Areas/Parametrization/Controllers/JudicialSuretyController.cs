using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System.Collections.Generic;
using System.Web.Mvc;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class JudicialSuretyController : Controller
    {
        public ActionResult ArticleProduct()
        {
            return this.View();
        }

        public ActionResult ArticleLine()
        {
            return this.View();
        }

        public ActionResult ArticleProductSearch(){
            return this.View();
        }
        public ActionResult ArticleLineSearch()
        {

            return this.View();
        }
        public ActionResult CourtType()
        {
            return this.View();
        }
        public ActionResult CourtTypeSearch()
        {
            return this.View();
        }
        /// <summary>
        /// Listado de productos caucion
        /// </summary>
        /// <returns>Modelo Product</returns>
        [HttpPost]
        public ActionResult LoadProducts(int prefixCode)
        {
            try
            {
                List<Application.ProductServices.Models.Product> products = DelegateService.productService.GetProducts(prefixCode);
                return new UifJsonResult(true, products);
            }
            catch
            {
                return new UifJsonResult(false, "");
            }            

        }
        /// <summary>
        /// Listado de articulos
        /// </summary>
        /// <returns>Modelo Product</returns>
        [HttpGet]
        public ActionResult LoadArticles()
        {
            try
            {
                List<CompanyArticle> articles = DelegateService.judicialSuretyService.GetCompanyArticles();
                return new UifJsonResult(true, articles);
            }
            catch
            {
                return new UifJsonResult(false, "");
            }

        }

        /// <summary>
        /// Listado de Lineas de articulo
        /// </summary>
        /// <returns>Modelo Product</returns>
        [HttpGet]
        public ActionResult LoadArticlesLine()
        {
            try
            {
                List<CompanyArticleLine> articleLine = DelegateService.judicialSuretyService.getCompanyArticleLines();
                return new UifJsonResult(true, articleLine);
            }
            catch
            {
                return new UifJsonResult(false, "");
            }

        }

        [HttpPost]
        public ActionResult LoadSearchProductArticles(string smallDescription)
        {
            try
            {
                List<CompanyProductArticle> productArticles = DelegateService.judicialSuretyService.GetCompanyProductArticlesByDescription(smallDescription);
                return new UifJsonResult(true, productArticles);
            }
            catch
            {
                return new UifJsonResult(false, "");
            }
        }
        [HttpPost]
        public ActionResult LoadSearchArticleLine(string smallDescription)
        {
            try
            {
                List<CompanyArticleLine> articleLines = DelegateService.judicialSuretyService.GetCompanyArticleLineByDescription(smallDescription);
                return new UifJsonResult(true, articleLines);
            }
            catch
            {
                return new UifJsonResult(false, "");
            }
        }

        [HttpGet]
        public ActionResult LoadProductArticles()
        {
            try
            {
                List<CompanyProductArticle> productArticles = DelegateService.judicialSuretyService.GetCompanyProductArticles();
                return new UifJsonResult(true, productArticles);
            }
            catch
            {
                return new UifJsonResult(false, "");
            }
        }

       [HttpPost]
       public ActionResult UpdateProductArticle(List<CompanyProductArticle> productArticleDelete, List<CompanyProductArticle> productArticleUpdate, List<CompanyProductArticle> productArticleInsert)
       {
            int CountDelete = 0;
            int CountInsert = 0;
            int CountUpdate = 0;
            try
            {
                if (productArticleDelete != null)
                {
                    CountDelete = DelegateService.judicialSuretyService.DeleteCompanyProductArticle(productArticleDelete).Count;
                }
                if (productArticleUpdate != null)
                {
                    CountUpdate = DelegateService.judicialSuretyService.UpdateCompanyProductArticle(productArticleUpdate).Count;
                }
                if (productArticleInsert != null)
                {
                    CountInsert =  DelegateService.judicialSuretyService.InsertCompanyProductArticle(productArticleInsert).Count;
                }

                return new UifJsonResult(true, string.Format("Actualizados:{0} \n Eliminados:{1} \n Insertados:{2} \n", CountUpdate, CountDelete, CountInsert));

            }
            catch
            {
                return new UifJsonResult(false, string.Format("Actualizados:{0} \n Eliminados:{1} \n Insertados:{2} \n", CountUpdate, CountDelete, CountInsert));
            }
       }
        [HttpPost]
        public ActionResult UpdateArticleLine(List<CompanyArticleLine> articleLineDelete, List<CompanyArticleLine> articleLineUpdate, List<CompanyArticleLine> articleLineInsert)
        {
            int CountDelete = 0;
            int CountInsert = 0;
            int CountUpdate = 0;
            try
            {
                if (articleLineDelete != null)
                {
                    CountDelete = DelegateService.judicialSuretyService.CompanyArticleLineDelete(articleLineDelete).Count;
                }
                if (articleLineUpdate != null)
                {
                    CountUpdate = DelegateService.judicialSuretyService.CompanyArticleLineUpdate(articleLineUpdate).Count;
                }
                if (articleLineInsert != null)
                {
                    CountInsert = DelegateService.judicialSuretyService.CompanyArticleLineInsert(articleLineInsert).Count;
                }
                return new UifJsonResult(true, string.Format("Actualizados:{0} \n Eliminados:{1} \n Insertados:{2} \n", CountUpdate, CountDelete, CountInsert));

            }
            catch
            {
                return new UifJsonResult(false, string.Format("Actualizados:{0} \n Eliminados:{1} \n Insertados:{2} \n", CountUpdate, CountDelete, CountInsert));
            }
        }

        /// <summary>
        /// Listado de Lineas de articulo
        /// </summary>
        /// <returns>Modelo Product</returns>
        [HttpGet]
        public ActionResult LoadCourtsType()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.judicialSuretyService.GetCourts());
            }
            catch
            {
                return new UifJsonResult(false, "");
            }

        }
        [HttpPost]
        public ActionResult UpdateCourtType(List<CompanyCourt> courtTypeDelete, List<CompanyCourt> courtTypeUpdate, List<CompanyCourt> courtTypeInsert)
        {
            int CountDelete = 0;
            int CountInsert = 0;
            int CountUpdate = 0;
            try
            {
                if (courtTypeDelete != null)
                {
                    CountDelete = DelegateService.judicialSuretyService.CompanyCourtTypeDelete(courtTypeDelete).Count;
                }
                if (courtTypeUpdate != null)
                {
                    CountUpdate = DelegateService.judicialSuretyService.CompanyCourtTypeUpdate(courtTypeUpdate).Count;
                }
                if (courtTypeInsert != null)
                {
                    CountInsert = DelegateService.judicialSuretyService.CompanyCourtTypeInsert(courtTypeInsert).Count;
                }

                return new UifJsonResult(true, string.Format("Actualizados:{0} \n Eliminados:{1} \n Insertados:{2} \n", CountUpdate, CountDelete, CountInsert));

            }
            catch
            {
                return new UifJsonResult(false, string.Format("Actualizados:{0} \n Eliminados:{1} \n Insertados:{2} \n", CountUpdate, CountDelete, CountInsert));
            }
        }
        [HttpPost]
        public ActionResult LoadSearchCourtType(string smallDescription)
        {
            try
            {
                List<CompanyCourt> courtsType = DelegateService.judicialSuretyService.GetCompanyCourtsTypeByDescription(smallDescription);
                return new UifJsonResult(true, courtsType);
            }
            catch
            {
                return new UifJsonResult(false, "");
            }
        }

    }
}