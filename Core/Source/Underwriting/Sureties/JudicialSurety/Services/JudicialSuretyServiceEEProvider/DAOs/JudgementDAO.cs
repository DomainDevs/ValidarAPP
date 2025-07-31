using Sistran.Core.Application.Sureties.JudicialSuretyServices.EEProvider.Assemblers;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.Diagnostics;
using Sistran.Core.Framework.Views;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using UPEN = Sistran.Core.Application.UniquePerson.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.EEProvider.ModelsAutoMapper;
using System;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Models;
using System.Data;

namespace Sistran.Core.Application.Sureties.JudicialSuretyServices.EEProvider.DAOs
{
    public class JudgementDAO
    {
        /// <summary>
        /// Funcion para obtener los tipos de Juzgados
        /// </summary>
        /// <returns></returns>
        public List<Models.Court> GetCourt()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection courtList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.Court)));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.JudicialSuretyService.Providers.GetCourt");
            return ModelAssembler.CreateCourt(courtList);
        }

        /// <summary>
        /// Funciona para obtener los articulos
        /// </summary>
        /// <returns></returns>
        public List<Models.Article> GetArticle()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection articletList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.Article)));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.JudicialSuretyService.Providers.GetCourt");
            return ModelAssembler.CreateArticle(articletList);
        }

        /// <summary>
        /// obtener los articulos del producto
        /// </summary>
        /// <returns></returns>
        public List<Models.Article> GetArticlesByProductId(int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Models.Article> articles = new List<Models.Article>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.ProductArticle.Properties.ProductId, typeof(COMMEN.ProductArticle).Name);
            filter.Equal();
            filter.Constant(productId);

            BusinessCollection entityCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(COMMEN.ProductArticle), filter.GetPredicate());
            if (entityCollection.Count > 0)
            {
                filter = new ObjectCriteriaBuilder();
                filter.Property(COMMEN.Article.Properties.ArticleCode, typeof(COMMEN.Article).Name);
                filter.In();
                filter.ListValue();
                foreach (COMMEN.ProductArticle entity in entityCollection)
                {
                    filter.Constant(entity.ArticleCode);
                }
                filter.EndList();


                BusinessCollection articleCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(COMMEN.Article), filter.GetPredicate());
                articles = ModelAssembler.CreateArticle(articleCollection);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.JudicialSuretyService.Providers.GetCourt");
            return articles;
        }

        public Models.Judgement CreateJudgement(Models.Judgement judgementModel)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ISSEN.CoRiskSurety judicialSuretytEntity = new ISSEN.CoRiskSurety(judgementModel.Risk.Id);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.JudicialSuretyService.Providers.GetCourt");
            return ModelAssembler.CreateJudgement(judgementModel);
        }

        public List<Models.Judgement> GetJudicialSuretiesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            List<Models.Judgement> judgements = new List<Models.Judgement>();
            switch (moduleType)
            {
                case ModuleType.Claim:

                    List<ISSEN.Risk> entityRisks = new List<ISSEN.Risk>();
                    List<ISSEN.Policy> entityPolicies = new List<ISSEN.Policy>();
                    List<ISSEN.EndorsementRisk> entityEndorsementRisks = new List<ISSEN.EndorsementRisk>();
                    List<ISSEN.RiskJudicialSurety> enitytRiskJudicialSureties = new List<ISSEN.RiskJudicialSurety>();
                    List<ISSEN.Endorsement> entityEndorsements = new List<ISSEN.Endorsement>();
                    COMMEN.Article entityArticle = new COMMEN.Article();

                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                    filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
                    filter.Equal();
                    filter.Constant(endorsementId);

                    ClaimJudicialSuretyView view = new ClaimJudicialSuretyView();
                    ViewBuilder builder = new ViewBuilder("ClaimJudicialSuretyView");
                    builder.Filter = filter.GetPredicate();

                    DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                    if (view.Risks.Count > 0)
                    {
                        entityRisks = view.Risks.Cast<ISSEN.Risk>().ToList();
                        entityPolicies = view.Policies.Cast<ISSEN.Policy>().ToList();
                        entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                        enitytRiskJudicialSureties = view.RiskJudicialSurety.Cast<ISSEN.RiskJudicialSurety>().ToList();
                        entityEndorsements = view.Endorsement.Cast<ISSEN.Endorsement>().ToList();
                        entityArticle = view.Articles.Cast<COMMEN.Article>().First();
                        foreach (ISSEN.Risk entityRisk in entityRisks)
                        {
                            ISSEN.EndorsementRisk entityEndorsementRisk = entityEndorsementRisks.Where(x => x.RiskId == entityRisk.RiskId).FirstOrDefault();
                            ISSEN.RiskJudicialSurety entityRiskJudicialSurety = enitytRiskJudicialSureties.First(x => x.RiskId == entityRisk.RiskId);
                            ISSEN.Endorsement entityEndorsement = entityEndorsements.First(x => x.EndorsementId == entityEndorsementRisk.EndorsementId);
                            ISSEN.Policy entityPolicy = entityPolicies.First(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                            JudicialSuretyMapper judicialSuretyMapper = new JudicialSuretyMapper();
                            judicialSuretyMapper.risk = entityRisk;
                            judicialSuretyMapper.RiskJudicialSurety = entityRiskJudicialSurety;
                            judicialSuretyMapper.endorsementRisk = entityEndorsementRisk;
                            judicialSuretyMapper.endorsement = entityEndorsement;
                            judicialSuretyMapper.article = entityArticle;
                            judicialSuretyMapper.policy = entityPolicy;
                            Models.Judgement judgement = ModelAssembler.CreateJudicialSurety(judicialSuretyMapper);

                            if (judgement.Risk.MainInsured.IndividualId != 0)
                            {
                                IssuanceInsured insured = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(judgement.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                                if (insured != null)
                                {
                                    judgement.Risk.MainInsured.Name = insured.IndividualType == IndividualType.Person ? (
                                            insured.Surname + " " + (string.IsNullOrEmpty(insured.SecondSurname) ? "" : insured.SecondSurname + " ") + insured.Name
                                            ) : insured.Name;

                                    judgement.Risk.MainInsured.IdentificationDocument = insured.IdentificationDocument;

                                    judgements.Add(judgement);
                                }
                            }
                        }
                    }
                    break;
            }

            return judgements;
        }

        public Models.Judgement GetJudicialSuretyByRiskIdModuleType(int riskId, ModuleType moduleType)
        {
            Models.Judgement judgement = new Models.Judgement();
            switch (moduleType)
            {
                case ModuleType.Claim:
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                    filter.Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name);
                    filter.Equal();
                    filter.Constant(riskId);

                    ClaimJudicialSuretyView view = new ClaimJudicialSuretyView();
                    ViewBuilder builder = new ViewBuilder("ClaimJudicialSuretyView");
                    builder.Filter = filter.GetPredicate();

                    DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                    if (view.Risks.Count > 0)
                    {
                        ISSEN.Risk entityRisk = view.Risks.Cast<ISSEN.Risk>().FirstOrDefault();
                        ISSEN.EndorsementRisk entityEndorsementRisk = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().Where(x => x.RiskId == entityRisk.RiskId).FirstOrDefault();
                        ISSEN.RiskJudicialSurety entityRiskJudicialSurety = view.RiskJudicialSurety.Cast<ISSEN.RiskJudicialSurety>().First(x => x.RiskId == entityRisk.RiskId);
                        ISSEN.Endorsement entityEndorsement = view.Endorsement.Cast<ISSEN.Endorsement>().First(x => x.EndorsementId == entityEndorsementRisk.EndorsementId);
                        ISSEN.Policy entityPolicy = view.Policies.Cast<ISSEN.Policy>().First(x => x.PolicyId == entityEndorsementRisk.PolicyId);
                        COMMEN.Article entityArticle = view.Articles.Cast<COMMEN.Article>().First(x => x.ArticleCode == entityRiskJudicialSurety.ArticleCode);

                        JudicialSuretyMapper judicialSuretyMapper = new JudicialSuretyMapper();
                        judicialSuretyMapper.risk = entityRisk;
                        judicialSuretyMapper.RiskJudicialSurety = entityRiskJudicialSurety;
                        judicialSuretyMapper.endorsementRisk = entityEndorsementRisk;
                        judicialSuretyMapper.endorsement = entityEndorsement;
                        judicialSuretyMapper.article = entityArticle;
                        judicialSuretyMapper.policy = entityPolicy;

                        judgement = ModelAssembler.CreateJudicialSurety(judicialSuretyMapper);

                        if (judgement.Risk.MainInsured.IndividualId != 0)
                        {
                            IssuanceInsured insured = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(judgement.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                            if (insured != null)
                            {
                                judgement.Risk.MainInsured.Name = insured.IndividualType == IndividualType.Person ? (
                                        insured.Surname + " " + (string.IsNullOrEmpty(insured.SecondSurname) ? "" : insured.SecondSurname + " ") + insured.Name
                                        ) : insured.Name;

                                judgement.Risk.MainInsured.IdentificationDocument = insured.IdentificationDocument;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }

                    return judgement;
            }

            return null;
        }

        public List<Models.Judgement> GetJudicialSuretiesBySureryIdModuleType(int suretyId)
        {
            List<Models.Judgement> judgements = new List<Models.Judgement>();

            List<ISSEN.Risk> entityRisks = new List<ISSEN.Risk>();
            List<ISSEN.Policy> entityPolicies = new List<ISSEN.Policy>();
            List<ISSEN.EndorsementRisk> entityEndorsementRisks = new List<ISSEN.EndorsementRisk>();
            List<ISSEN.RiskJudicialSurety> enitytRiskJudicialSureties = new List<ISSEN.RiskJudicialSurety>();
            List<ISSEN.Endorsement> entityEndorsements = new List<ISSEN.Endorsement>();
            List<COMMEN.Article> entityArticles = new List<COMMEN.Article>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.RiskJudicialSurety.Properties.InsuredId, typeof(ISSEN.RiskJudicialSurety).Name);
            filter.Equal();
            filter.Constant(suretyId);
            
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);

            ClaimJudicialSuretyView view = new ClaimJudicialSuretyView();
            ViewBuilder builder = new ViewBuilder("ClaimJudicialSuretyView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                entityRisks = view.Risks.Cast<ISSEN.Risk>().ToList();
                entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                enitytRiskJudicialSureties = view.RiskJudicialSurety.Cast<ISSEN.RiskJudicialSurety>().ToList();
                entityEndorsements = view.Endorsement.Cast<ISSEN.Endorsement>().ToList();
                entityPolicies = view.Policies.Cast<ISSEN.Policy>().ToList();
                entityArticles = view.Articles.Cast<COMMEN.Article>().ToList();

                foreach (ISSEN.Risk entityRisk in entityRisks)
                {
                    ISSEN.EndorsementRisk entityEndorsementRisk = entityEndorsementRisks.Where(x => x.RiskId == entityRisk.RiskId).FirstOrDefault();
                    ISSEN.RiskJudicialSurety entityRiskJudicialSurety = enitytRiskJudicialSureties.First(x => x.RiskId == entityRisk.RiskId);
                    ISSEN.Endorsement entityEndorsement = entityEndorsements.First(x => x.EndorsementId == entityEndorsementRisk.EndorsementId);
                    COMMEN.Article entityArticle = entityArticles.First(x => x.ArticleCode == entityRiskJudicialSurety.ArticleCode);
                    ISSEN.Policy entityPolicy = entityPolicies.First(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                    JudicialSuretyMapper judicialSuretyMapper = new JudicialSuretyMapper();
                    judicialSuretyMapper.risk = entityRisk;
                    judicialSuretyMapper.RiskJudicialSurety = entityRiskJudicialSurety;
                    judicialSuretyMapper.endorsementRisk = entityEndorsementRisk;
                    judicialSuretyMapper.endorsement = entityEndorsement;
                    judicialSuretyMapper.article = entityArticle;
                    judicialSuretyMapper.policy = entityPolicy;

                    Models.Judgement judgement = ModelAssembler.CreateJudicialSurety(judicialSuretyMapper);

                    if (judgement.Risk.MainInsured.IndividualId != 0)
                    {
                        IssuanceInsured insured = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(judgement.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                        if (insured != null)
                        {
                            judgement.Risk.MainInsured.Name = insured.IndividualType == IndividualType.Person ? (
                                    insured.Surname + " " + (string.IsNullOrEmpty(insured.SecondSurname) ? "" : insured.SecondSurname + " ") + insured.Name
                                    ) : insured.Name;

                            judgement.Risk.MainInsured.IdentificationDocument = insured.IdentificationDocument;

                            judgements.Add(judgement);
                        }
                    }
                }
            }

            return judgements;
        }

        public List<Models.Judgement> GetJudicialSuretiesByDescription(string description)
        {
            List<IssuanceInsured> insureds = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, InsuredSearchType.DocumentNumber, CustomerType.Individual);

            List<Models.Judgement> judgements = new List<Models.Judgement>();

            if (insureds.Any())
            {
                List<ISSEN.Risk> entityRisks = new List<ISSEN.Risk>();
                List<ISSEN.Policy> entityPolicies = new List<ISSEN.Policy>();
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = new List<ISSEN.EndorsementRisk>();
                List<ISSEN.RiskJudicialSurety> enitytRiskJudicialSureties = new List<ISSEN.RiskJudicialSurety>();
                List<ISSEN.Endorsement> entityEndorsements = new List<ISSEN.Endorsement>();
                List<COMMEN.Article> entityArticles = new List<COMMEN.Article>();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.RiskJudicialSurety.Properties.InsuredId, typeof(ISSEN.RiskJudicialSurety).Name);
                filter.In();
                filter.ListValue();
                insureds.ForEach(x => filter.Constant(x.IndividualId));
                filter.EndList();

                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
                filter.Equal();
                filter.Constant(true);

                ClaimJudicialSuretyView view = new ClaimJudicialSuretyView();
                ViewBuilder builder = new ViewBuilder("ClaimJudicialSuretyView");
                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                if (view.Risks.Count > 0)
                {
                    entityRisks = view.Risks.Cast<ISSEN.Risk>().ToList();
                    entityPolicies = view.Policies.Cast<ISSEN.Policy>().ToList();
                    entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                    enitytRiskJudicialSureties = view.RiskJudicialSurety.Cast<ISSEN.RiskJudicialSurety>().ToList();
                    entityEndorsements = view.Endorsement.Cast<ISSEN.Endorsement>().ToList();
                    entityArticles = view.Articles.Cast<COMMEN.Article>().ToList();

                    foreach (ISSEN.Risk entityRisk in entityRisks)
                    {
                        ISSEN.EndorsementRisk entityEndorsementRisk = entityEndorsementRisks.Where(x => x.RiskId == entityRisk.RiskId).FirstOrDefault();
                        ISSEN.RiskJudicialSurety entityRiskJudicialSurety = enitytRiskJudicialSureties.First(x => x.RiskId == entityRisk.RiskId);
                        ISSEN.Endorsement entityEndorsement = entityEndorsements.First(x => x.EndorsementId == entityEndorsementRisk.EndorsementId);
                        COMMEN.Article entityArticle = entityArticles.First(x => x.ArticleCode == entityRiskJudicialSurety.ArticleCode);
                        ISSEN.Policy entityPolicy = entityPolicies.First(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                        JudicialSuretyMapper judicialSuretyMapper = new JudicialSuretyMapper();
                        judicialSuretyMapper.risk = entityRisk;
                        judicialSuretyMapper.RiskJudicialSurety = entityRiskJudicialSurety;
                        judicialSuretyMapper.endorsementRisk = entityEndorsementRisk;
                        judicialSuretyMapper.endorsement = entityEndorsement;
                        judicialSuretyMapper.article = entityArticle;
                        judicialSuretyMapper.policy = entityPolicy;

                        Models.Judgement judgement = ModelAssembler.CreateJudicialSurety(judicialSuretyMapper);

                        if (judgement.Risk.MainInsured.IndividualId != 0)
                        {
                            IssuanceInsured insured = insureds.First(x => x.IndividualId == judgement.Risk.MainInsured.IndividualId);
                            if (insured != null)
                            {
                                judgement.Risk.MainInsured.Name = insured.IndividualType == IndividualType.Person ? (
                                        insured.Surname + " " + (string.IsNullOrEmpty(insured.SecondSurname) ? "" : insured.SecondSurname + " ") + insured.Name
                                        ) : insured.Name;

                                judgement.Risk.MainInsured.IdentificationDocument = insured.IdentificationDocument;

                                judgements.Add(judgement);

                            }
                        }
                    }
                }
            }

            return judgements;
        }
        public List<ProductArticle> GetProductArticles()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.Description, "P"), "ProductDescription"));
            select.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.ProductId, "P"), "ProductId"));
            select.AddSelectValue(new SelectValue(new Column(COMMEN.Article.Properties.SmallDescription, "A"), "ArticleDescription"));
            select.AddSelectValue(new SelectValue(new Column(COMMEN.Article.Properties.ArticleCode, "A"), "ArticleCode"));

            Join join = new Join(new ClassNameTable(typeof(COMMEN.Article), "A"), new ClassNameTable(typeof(COMMEN.ProductArticle), "PA"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
               .Property(COMMEN.Article.Properties.ArticleCode, "A")
               .Equal()
               .Property(COMMEN.ProductArticle.Properties.ArticleCode, "PA")
               .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(PRODEN.Product), "P"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
             .Property(PRODEN.Product.Properties.ProductId, "P")
               .Equal()
               .Property(COMMEN.ProductArticle.Properties.ProductId, "PA")
               .GetPredicate());
            select.Table = join;
            List<ProductArticle> listProductArticleModel = new List<ProductArticle>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    ProductArticle productArticle = new ProductArticle
                    {
                        ProductDescription = (string)reader["ProductDescription"],
                        ProductId = (int)reader["ProductId"],
                        ArticleDescription = (string)reader["ArticleDescription"],
                        ArticleId = (int)reader["ArticleCode"]
                    };
                    listProductArticleModel.Add(productArticle);

                }
            }
            return listProductArticleModel;
        }
        public List<ProductArticle> GetProductArticlesByDescription(string smallDescription)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.Description, "P"), "ProductDescription"));
            select.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.ProductId, "P"), "ProductId"));
            select.AddSelectValue(new SelectValue(new Column(COMMEN.Article.Properties.SmallDescription, "A"), "ArticleDescription"));
            select.AddSelectValue(new SelectValue(new Column(COMMEN.Article.Properties.ArticleCode, "A"), "ArticleCode"));

            Join join = new Join(new ClassNameTable(typeof(COMMEN.Article), "A"), new ClassNameTable(typeof(COMMEN.ProductArticle), "PA"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
               .Property(COMMEN.Article.Properties.ArticleCode, "A")
               .Equal()
               .Property(COMMEN.ProductArticle.Properties.ArticleCode, "PA")
               .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(PRODEN.Product), "P"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
             .Property(PRODEN.Product.Properties.ProductId, "P")
               .Equal()
               .Property(COMMEN.ProductArticle.Properties.ProductId, "PA")
               .GetPredicate());
            select.Table = join;

            filter.Property(COMMEN.Article.Properties.SmallDescription, "A");
            filter.Like();
            filter.Constant(smallDescription + "%");
            filter.Or();
            filter.Property(COMMEN.Article.Properties.Description, "A");
            filter.Like();
            filter.Constant(smallDescription + "%");

            select.Where = filter.GetPredicate();

            List<ProductArticle> listProductArticleModel = new List<ProductArticle>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    ProductArticle productArticle = new ProductArticle
                    {
                        ProductDescription = (string)reader["ProductDescription"],
                        ProductId = (int)reader["ProductId"],
                        ArticleDescription = (string)reader["ArticleDescription"],
                        ArticleId = (int)reader["ArticleCode"]
                    };
                    listProductArticleModel.Add(productArticle);

                }
            }
            return listProductArticleModel;
        }

        public ProductArticle DeleteProductArticle(ProductArticle productArticleDelete)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMMEN.ProductArticle.Properties.ArticleCode, productArticleDelete.ArticleId);
            filter.And();
            filter.PropertyEquals(COMMEN.ProductArticle.Properties.ProductId, productArticleDelete.ProductId);
            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(COMMEN.ProductArticle), filter.GetPredicate());
            return productArticleDelete;
        }
        public ProductArticle UpdateProductArticle(ProductArticle productArticleUpdate)
        {

            PrimaryKey primaryKey = COMMEN.ProductArticle.CreatePrimaryKey(productArticleUpdate.ProductId, productArticleUpdate.ArticleId);
            COMMEN.ProductArticle productArticleEntity = (COMMEN.ProductArticle)DataFacadeManager.GetObject(primaryKey);
            DataFacadeManager.Update(productArticleEntity);
            return productArticleUpdate;
        }
        public ProductArticle InsertProductArticle(ProductArticle productArticleInsert)
        {
            COMMEN.ProductArticle productArticle = new COMMEN.ProductArticle() {
                ProductId = productArticleInsert.ProductId,
                ArticleCode = productArticleInsert.ArticleId,
                ByDefault = true
            };
            DataFacadeManager.Instance.GetDataFacade().InsertObject(productArticle);
            return productArticleInsert;
        }

        public List<ArticleLine> getArticleLines()
        {
            BusinessCollection articleLines = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.ArticleLine)));
            return ModelAssembler.CreateArticleLine(articleLines);
        }
        public List<ArticleLine> GetArticleLineByDescription(string smallDescription)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.ArticleLine.Properties.Description);
            filter.Like();
            filter.Constant(smallDescription + "%");
            filter.Or();
            filter.Property(COMMEN.ArticleLine.Properties.SmallDescription);
            filter.Like();
            filter.Constant(smallDescription + "%");
            BusinessCollection articleLines = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.ArticleLine), filter.GetPredicate()));
            return ModelAssembler.CreateArticleLine(articleLines);
        }
        public ArticleLine ArticleLineDelete(ArticleLine articleLine)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMMEN.ArticleLine.Properties.ArticleLineCode, articleLine.ArticleLineCd);
           
            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(COMMEN.ArticleLine), filter.GetPredicate());
            return articleLine;
        }
        public ArticleLine ArticleLineUpdate(ArticleLine articleLine)
        {
            PrimaryKey primaryKey = COMMEN.ArticleLine.CreatePrimaryKey(articleLine.ArticleLineCd);
            COMMEN.ArticleLine articleLineEntity = (COMMEN.ArticleLine)DataFacadeManager.GetObject(primaryKey);
            articleLineEntity.Description = articleLine.Description;
            articleLineEntity.SmallDescription = articleLine.SmallDescription;
            articleLineEntity.Enabled = articleLine.Enabled;
            DataFacadeManager.Update(articleLineEntity);
            return articleLine;
        }
        public ArticleLine ArticleLineInsert(ArticleLine articleLine)
        {
            var articleLineId = getArticleLines().OrderByDescending(x => x.ArticleLineCd).FirstOrDefault().ArticleLineCd + 1;
            COMMEN.ArticleLine productArticle = new COMMEN.ArticleLine()
            {
                Description = articleLine.Description,
                SmallDescription = articleLine.SmallDescription,
                Enabled = articleLine.Enabled,
                ArticleLineCode = articleLineId
            };
            DataFacadeManager.Instance.GetDataFacade().InsertObject(productArticle);
            return articleLine;
        }
        public List<Models.Court> GetCourtsTypeByDescription(string smallDescription)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.Court.Properties.DescriptionCode);
            filter.Like();
            filter.Constant(smallDescription + "%");
            filter.Or();
            filter.Property(COMMEN.Court.Properties.SmallDescription);
            filter.Like();
            filter.Constant(smallDescription + "%");
            BusinessCollection courtList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.Court), filter.GetPredicate()));
            return ModelAssembler.CreateCourt(courtList);

        }
        public Court CourtTypeDelete(Court court)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMMEN.Court.Properties.CourtCode, court.Id);

            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(COMMEN.Court), filter.GetPredicate());
            return court;
        }
        public Court CourtTypeUpdate(Court court)
        {
            PrimaryKey primaryKey = COMMEN.Court.CreatePrimaryKey(court.Id);
            COMMEN.Court CourtEntity = (COMMEN.Court)DataFacadeManager.GetObject(primaryKey);
            CourtEntity.DescriptionCode = court.Description;
            CourtEntity.SmallDescription = court.SmallDescription;
            CourtEntity.Enabled = court.Enabled;
            DataFacadeManager.Update(CourtEntity);
            return court;
        }
        public Court CourtTypeInsert(Court court)
        {
            var courtId = GetCourt().OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            COMMEN.Court courtEntity = new COMMEN.Court()
            {
                DescriptionCode = court.Description,
                SmallDescription = court.SmallDescription,
                Enabled = court.Enabled,
                CourtCode = courtId
            };
            DataFacadeManager.Instance.GetDataFacade().InsertObject(courtEntity);
            return court;
        }
    }


}
