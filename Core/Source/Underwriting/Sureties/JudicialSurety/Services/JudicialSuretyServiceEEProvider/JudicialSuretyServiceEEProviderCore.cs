using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.EEProvider.DAOs;
using System.ServiceModel;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Core.Application.Sureties.JudicialSuretyServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class JudicialSuretyServiceEEProviderCore : SuretiesEEProvider.SuretiesEEProvider, IJudicialSuretyCore
    {
        /// <summary>
        /// Metodo que obtiene el listado de Juzgados
        /// </summary>
        /// <returns></returns>
        public List<Court> GetCourts()
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                return judgementDAO.GetCourt();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.Article> GetArticlesByProductId(int productId)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                return judgementDAO.GetArticlesByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Article> GetArticles()
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                return judgementDAO.GetArticle();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Judgement> GetJudicialSuretiesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                return judgementDAO.GetJudicialSuretiesByEndorsementIdModuleType(endorsementId, moduleType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Judgement GetJudicialSuretyByRiskIdModuleType(int riskId, ModuleType moduleType)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                return judgementDAO.GetJudicialSuretyByRiskIdModuleType(riskId, moduleType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Judgement> GetJudicialSuretiesBySureryId(int suretyId)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                return judgementDAO.GetJudicialSuretiesBySureryIdModuleType(suretyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Judgement> GetJudicialSuretiesByDescription(string description)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                return judgementDAO.GetJudicialSuretiesByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<ProductArticle> GetProductArticles()
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                return judgementDAO.GetProductArticles();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<ProductArticle> GetProductArticlesByDescription(string smallDescription)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                return judgementDAO.GetProductArticlesByDescription(smallDescription);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<ProductArticle> DeleteProductArticle(List<ProductArticle> productArticleDelete)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                List<ProductArticle> productArticles = new List<ProductArticle>();
                foreach (ProductArticle productArticle in productArticleDelete)
                {
                    productArticles.Add(judgementDAO.DeleteProductArticle(productArticle));
                }
                return productArticles;


            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<ProductArticle> UpdateProductArticle(List<ProductArticle> productArticleUpdate)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                List<ProductArticle> productArticles = new List<ProductArticle>();
                foreach (ProductArticle productArticle in productArticleUpdate)
                {
                     productArticles.Add(judgementDAO.UpdateProductArticle(productArticle));
                }
                return productArticles;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<ProductArticle> InsertProductArticle(List<ProductArticle> productArticleInsert)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                List<ProductArticle> productArticles = new List<ProductArticle>();
                foreach (ProductArticle productArticle in productArticleInsert)
                {
                    productArticles.Add(judgementDAO.InsertProductArticle(productArticle));
                }
                return productArticles;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<ArticleLine> getArticleLines()
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();

                return judgementDAO.getArticleLines();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
        public List<ArticleLine> GetArticleLineByDescription(string smallDescrption)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();

                return judgementDAO.GetArticleLineByDescription(smallDescrption);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<ArticleLine> ArticleLineDelete(List<ArticleLine> articleLines)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                List<ArticleLine> productArticles = new List<ArticleLine>();
                foreach (ArticleLine articleLine in articleLines)
                {
                    productArticles.Add(judgementDAO.ArticleLineDelete(articleLine));
                }
                return productArticles;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<ArticleLine> ArticleLineUpdate(List<ArticleLine> articleLineUpdate)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                List<ArticleLine> productArticles = new List<ArticleLine>();
                foreach (ArticleLine articleLine in articleLineUpdate)
                {
                    productArticles.Add(judgementDAO.ArticleLineUpdate(articleLine));
                }
                return productArticles;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<ArticleLine> ArticleLineInsert(List<ArticleLine> articleLineInsert)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                List<ArticleLine> productArticles = new List<ArticleLine>();
                foreach (ArticleLine articleLine in articleLineInsert)
                {
                    productArticles.Add(judgementDAO.ArticleLineInsert(articleLine));
                }
                return productArticles;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<Court> GetCourtsTypeByDescription(string smallDescription)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                return judgementDAO.GetCourtsTypeByDescription(smallDescription);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Court> CourtTypeDelete(List<Court> courtTypeDelete)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                List<Court> courts = new List<Court>();
                foreach (Court court in courtTypeDelete)
                {
                    courts.Add(judgementDAO.CourtTypeDelete(court));
                }
                return courts;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<Court> CourtTypeInsert(List<Court> courtTypeInsert)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                List<Court> courts = new List<Court>();
                foreach (Court court in courtTypeInsert)
                {
                    courts.Add(judgementDAO.CourtTypeInsert(court));
                }
                return courts;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<Court> CourtTypeUpdate(List<Court> courtTypeUpdate)
        {
            try
            {
                JudgementDAO judgementDAO = new JudgementDAO();
                List<Court> courts = new List<Court>();
                foreach (Court court in courtTypeUpdate)
                {
                    courts.Add(judgementDAO.CourtTypeUpdate(court));
                }
                return courts;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

    }
}