using System.Collections.Generic;
using System.Linq;

using Sistran.Company.Application.QuotationServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;

using QuotationEntitiesCore = Sistran.Core.Application.Quotation.Entities;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using COMM = Sistran.Core.Application.Common.Entities;
using PROD = Sistran.Core.Application.Product.Entities;


namespace Sistran.Company.Application.QuotationServices.EEProvider.Assemblers
{
    public class EntityAssembler
    {
        public static QuotationEntitiesCore.Peril CreatePeril(Peril peril)
        {
            return new QuotationEntitiesCore.Peril(peril.Id)
            {
                Description = peril.Description,
                SmallDescription = peril.SmallDescription,
                PerilCode = peril.Id
            };
        }

        public static Sistran.Core.Application.QuotationServices.Models.ConditionTextModel CreateText(TextPretacalogued model)
        {
            return new Sistran.Core.Application.QuotationServices.Models.ConditionTextModel()
            {
                ConditionLevelCode = model.ConditionLevelCode,
                ConditionLevelId = model.ConditionLevelId,
                ConditionTextId = model.ConditionTextId,
                ConditionTextIdCod = model.ConditionTextIdCod,
                CondTextLevelId = model.CondTextLevelId,
                IsAutomatic = model.IsAutomatic,
                TextBody = model.TextBody,
                TextTitle = model.TextTitle,
                
            };
        }

        public static List<Sistran.Core.Application.QuotationServices.Models.ConditionTextModel> CreateTexts(List<TextPretacalogued> model)
        {
            var ConditionTextModel = new List<Sistran.Core.Application.QuotationServices.Models.ConditionTextModel>();
            foreach (var item in model)
            {
                ConditionTextModel.Add(CreateText(item));
            }
            return ConditionTextModel;
        }

        public static Sistran.Core.Application.QuotationServices.Models.CondTextLevelModel CreateTextLevel(TextPretacalogued model)
        {
            return new Sistran.Core.Application.QuotationServices.Models.CondTextLevelModel()
            {
                ConditionTextIdCod = model.ConditionTextId,
                CondTextLevelId = model.CondTextLevelId,
                IsAutomatic = model.IsAutomatic,
            };
        }

        public static List<Sistran.Core.Application.QuotationServices.Models.CondTextLevelModel> CreateTextsLevels(List<TextPretacalogued> model)
        {
            var ConditionTextModel = new List<Sistran.Core.Application.QuotationServices.Models.CondTextLevelModel>();
            foreach (var item in model)
            {
                ConditionTextModel.Add(CreateTextLevel(item));
            }
            return ConditionTextModel;
        }

        public static QuotationEntitiesCore.ConditionText CreateConditionText(TextPretacalogued modelTextPretacalogued)
        {
            QuotationEntitiesCore.ConditionText conditionText = new QuotationEntitiesCore.ConditionText();
            conditionText.ConditionLevelCode = modelTextPretacalogued.ConditionLevelCode;
            
            conditionText.TextBody = modelTextPretacalogued.TextBody;
            conditionText.TextTitle = modelTextPretacalogued.TextTitle;
            if (modelTextPretacalogued.ConditionTextId > 0)
            {
                conditionText.ConditionTextId = modelTextPretacalogued.ConditionTextId;
            }
            return conditionText; 
        }
        public static List<QuotationEntitiesCore.ConditionText> CreateConditionTexts(List<TextPretacalogued> modelTextPretacalogueds)
        {
            List<QuotationEntitiesCore.ConditionText> entityTexTPrecatalogued = new List<QuotationEntitiesCore.ConditionText>();
            foreach (var item in modelTextPretacalogueds)
            {
                entityTexTPrecatalogued.Add(CreateConditionText(item));
            }
           return entityTexTPrecatalogued;
        }
        public static QuotationEntitiesCore.CondTextLevel CreateConditionTextLevel(TextPretacalogued modelTextPretacalogued)
        {
            QuotationEntitiesCore.CondTextLevel condTextLevel = new QuotationEntitiesCore.CondTextLevel();
            condTextLevel.ConditionTextId = modelTextPretacalogued.ConditionTextId;
            condTextLevel.ConditionLevelId = modelTextPretacalogued?.ConditionLevelId ?? 0;
            condTextLevel.IsAutomatic = true;
            
            if (modelTextPretacalogued.CondTextLevelId > 0)
            {
                condTextLevel.CondTextLevelId = modelTextPretacalogued.CondTextLevelId;
            }
            return condTextLevel;
        }
        public static List<QuotationEntitiesCore.CondTextLevel> CreateConditionTextsLevels(List<TextPretacalogued> modelTextPretacalogueds)
        {
            List<QuotationEntitiesCore.CondTextLevel> entityTexTPrecatalogued = new List<QuotationEntitiesCore.CondTextLevel>();
            foreach (var item in modelTextPretacalogueds)
            {
                entityTexTPrecatalogued.Add(CreateConditionTextLevel(item));
            }
            return entityTexTPrecatalogued;
        }

        internal static List<CompanyQuotationVehicleSearch> CreateQuotationVehicleSearch
           (List<TMPEN.TempSubscription> entityTempSuscriptions,
           List<TMPEN.TempRiskVehicle> entityTempRiskVehicles,
           List<COMM.Branch> entityBranches,
           List<COMM.Prefix> entityPrefix,
           List<TMPEN.TempPayerComponent> entityPayerComponents,
           List<PROD.Product> entityProducts,
            List<TMPEN.TempRiskCoverage> entityTempRiskCoverages)
        {
            List<CompanyQuotationVehicleSearch> companyQuotationVehicleSearches = new List<CompanyQuotationVehicleSearch>();
            foreach (TMPEN.TempSubscription TempSuscription in entityTempSuscriptions)
            {
                companyQuotationVehicleSearches.Add(new CompanyQuotationVehicleSearch
                {
                    BranchId = entityTempSuscriptions.First(x => x.BranchCode == TempSuscription.BranchCode).BranchCode,
                    Branch = entityBranches.First(x => x.BranchCode == TempSuscription.BranchCode).Description,
                    PrefixId = entityPrefix.First(x => x.PrefixCode == TempSuscription.PrefixCode).PrefixCode,
                    Prefix = entityPrefix.First(x => x.PrefixCode == TempSuscription.PrefixCode).Description,
                    QuotationId = entityTempSuscriptions.First(x => x.QuotationId == TempSuscription.QuotationId).QuotationId,
                    PremiumAmount = entityPayerComponents.Last(x => x.TempId == TempSuscription.TempId).CalcBaseAmount,
                    ProductId = entityTempSuscriptions.First(x => x.ProductId == TempSuscription.ProductId).ProductId,
                    DeclaredAmount = entityTempRiskVehicles.Last(x => x.TempId == TempSuscription.TempId).VehiclePrice,
                    Product = entityProducts.First(x => x.ProductId == TempSuscription.ProductId).Description,
                    DateQuotation = entityTempSuscriptions.First(x => x.BeginDate == TempSuscription.BeginDate).BeginDate,
                    SentEmail = TempSuscription.PrintedDate.HasValue,
                });


            }
            return companyQuotationVehicleSearches;
        }

    }
}
