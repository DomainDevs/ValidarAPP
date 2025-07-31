using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.QuotationServices.Models;
using System.Collections.Generic;

namespace Sistran.Core.Application.QuotationServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        public static ConditionTextModel CreateConditionText(ConditionText entidad)
        {
            return new ConditionTextModel
            {
                ConditionTextId = entidad.ConditionTextId,
                TextTitle = entidad.TextTitle,
                TextBody = entidad.TextBody,
                ConditionLevelCode = entidad.ConditionLevelCode,
            };
        }
        public static List<ConditionTextModel> CreateConditionTexts(List<ConditionText> entidad)
        {
            var ConditionText = new List<ConditionTextModel>();
            foreach (var item in entidad)
            {
                ConditionText.Add(CreateConditionText(item));
            }
            return ConditionText;
        }

        public static CondTextLevelModel CreateConditionTextLevel(CondTextLevel entidad)
        {
            return new CondTextLevelModel
            {
                ConditionLevelId = entidad.ConditionLevelId,
                CondTextLevelId = entidad.CondTextLevelId,
                IsAutomatic = entidad.IsAutomatic,
                ConditionTextIdCod = entidad.ConditionTextId

            };
        }
        public static List<CondTextLevelModel> CreateConditionTextsLevels(List<CondTextLevel> entidad)
        {
            var conditionTxt = new List<CondTextLevelModel>();
            foreach (var item in entidad)
            {
                conditionTxt.Add(CreateConditionTextLevel(item));
            }

            return conditionTxt;
        }
    }
}
