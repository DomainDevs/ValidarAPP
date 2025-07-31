using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.QuotationServices.Models;

namespace Sistran.Core.Application.QuotationServices.EEProvider.Assemblers
{
    public class EntityAssembler
    {
        public static ConditionText CreateConditionText(ConditionTextModel model)
        {
            CondTextLevelModel levelModel = new CondTextLevelModel();
            return new ConditionText
            {
                ConditionLevelCode = model.ConditionLevelCode,
                ConditionTextId = model.ConditionTextId,
                TextBody = model.TextBody,
                TextTitle = model.TextTitle,
            };
        }

        public static CondTextLevel CreateConditionTextLevel(CondTextLevelModel model)
        {
            return new CondTextLevel
            {
                ConditionLevelId = model.ConditionLevelId,
                ConditionTextId = model.ConditionTextIdCod,
                CondTextLevelId = model.CondTextLevelId,
                IsAutomatic = model.IsAutomatic,

            };
        }
    }
}
