using Sistran.Core.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.Sureties.JudicialSuretyServices
{
    [ServiceContract]
    public interface IJudicialSuretyCore : ISuretiesCore
    {
        [OperationContract]
        List<Court> GetCourts();

        [OperationContract]
        List<Article> GetArticles();

        [OperationContract]
        List<Models.Article> GetArticlesByProductId(int productId);

        [OperationContract]
        List<Judgement> GetJudicialSuretiesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType);

        [OperationContract]
        Judgement GetJudicialSuretyByRiskIdModuleType(int riskId, ModuleType moduleType);

        [OperationContract]
        List<Judgement> GetJudicialSuretiesBySureryId(int suretyId);
        [OperationContract]
        List<Judgement> GetJudicialSuretiesByDescription(string description);
        [OperationContract]
        List<ProductArticle> GetProductArticles();
        [OperationContract]
        List<ProductArticle> GetProductArticlesByDescription(string smallDescription);
        [OperationContract]
        List<ProductArticle> DeleteProductArticle(List<ProductArticle> productArticleDelete);
        [OperationContract]
        List<ProductArticle> UpdateProductArticle(List<ProductArticle> productArticleUpdate);
        [OperationContract]
        List<ProductArticle> InsertProductArticle(List<ProductArticle> productArticleInsert);
        [OperationContract]
        List<ArticleLine> getArticleLines();
        [OperationContract]
        List<ArticleLine> GetArticleLineByDescription(string smallDescrption);
        [OperationContract]
        List<ArticleLine> ArticleLineDelete(List<ArticleLine> articleLineDelete);
        [OperationContract]
        List<ArticleLine> ArticleLineUpdate(List<ArticleLine> articleLineUpdate);
        [OperationContract]
        List<ArticleLine> ArticleLineInsert(List<ArticleLine> articleLineInsert);
        [OperationContract]
        List<Court> GetCourtsTypeByDescription(string smallDescription);
        [OperationContract]
        List<Court> CourtTypeDelete(List<Court> courtTypeDelete);
        [OperationContract]
        List<Court> CourtTypeUpdate(List<Court> courtTypeUpdate);
        [OperationContract]
        List<Court> CourtTypeInsert(List<Court> courtTypeInsert);
    }
}