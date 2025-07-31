using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class TextDAO
    {
        /// <summary>
        /// Obtener textos precatalogados
        /// </summary>
        /// <param name="name">nombre</param>
        /// <param name="levelId">Id nivel</param>
        /// <param name="conditionalLevelId">Id condición</param>
        /// <returns>Lista de textos</returns>
        public List<Text> GetTextsByNameLevelIdConditionLevelId(string name, int levelId, int conditionLevelId)
        {
            int textId = 0;
            int.TryParse(name, out textId);

            ConditionTextView view = new ConditionTextView();
            ViewBuilder builder = new ViewBuilder("ConditionTextView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PARAMEN.ConditionLevel.Properties.LevelId, typeof(PARAMEN.ConditionLevel).Name);
            filter.Equal();
            filter.Constant(levelId);
            filter.And();

            filter.OpenParenthesis();
            filter.Property(QUOEN.CondTextLevel.Properties.ConditionLevelId, typeof(QUOEN.CondTextLevel).Name);
            filter.Equal();
            filter.Constant(conditionLevelId);
            if (levelId == 1)
            {
                filter.Or();
                filter.Property(QUOEN.CondTextLevel.Properties.ConditionLevelId, typeof(QUOEN.CondTextLevel).Name);
                filter.Equal();
                filter.Constant(0);
            }
            filter.CloseParenthesis();
            filter.And();
            if (textId == 0)
            {
                filter.Property(QUOEN.ConditionText.Properties.TextTitle, typeof(QUOEN.ConditionText).Name);
                filter.Like();
                filter.Constant("%" + name + "%");
            }
            else
            {
                filter.Property(QUOEN.ConditionText.Properties.ConditionTextId, typeof(QUOEN.ConditionText).Name);
                filter.Equal();
                filter.Constant(textId);
            }

            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }         

            return ModelAssembler.CreateTexts(view.ConditionText);
        }
    }
}