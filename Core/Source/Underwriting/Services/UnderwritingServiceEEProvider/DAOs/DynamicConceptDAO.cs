using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.Parameters.Entities;
using USS = Sistran.Core.Application.Utilities.Entities.Entity;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class DynamicConceptDAO
    {
        public List<DynamicConcept> GetDynamicConceptsByEndorsementIdRiskNumPolicyIdRiskId(int endorsementId, int riskNum, int policyId, int riskId)
        {
            List<DynamicConcept> dynamicConcepts = new List<DynamicConcept>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(DynamicConceptRelation.Properties.Key1, typeof(DynamicConceptRelation).Name).Equal().Constant(endorsementId);
            filter.And();
            filter.Property(DynamicConceptRelation.Properties.Key2, typeof(DynamicConceptRelation).Name).Equal().Constant(riskNum);
            filter.And();
            filter.Property(DynamicConceptRelation.Properties.Key3, typeof(DynamicConceptRelation).Name).Equal().Constant(policyId);
            filter.And();
            filter.Property(DynamicConceptRelation.Properties.Key4, typeof(DynamicConceptRelation).Name).Equal().Constant(riskId);
            filter.And();
            filter.Property(DynamicConceptRelation.Properties.EntityId, typeof(DynamicConceptRelation).Name).Equal().Constant(Convert.ToInt16(USS.EntityTypes.FacadeRiskEndorsement));

            DynamicConceptView view = new DynamicConceptView();
            ViewBuilder builder = new ViewBuilder("DynamicConceptView")
            {
                Filter = filter.GetPredicate()
            };

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            if (view.DynamicConceptRelationList != null && view.DynamicConceptRelationList.Count > 0 && view.DynamicConceptValueList != null && view.DynamicConceptValueList.Count > 0)
            {
                List<DynamicConceptRelation> dynamicConceptRelations = view.DynamicConceptRelationList.Cast<DynamicConceptRelation>().ToList();
                List<DynamicConceptValue> dynamicConceptValue = view.DynamicConceptValueList.Cast<DynamicConceptValue>().ToList();
                dynamicConcepts = ModelAssembler.CreateDynamicConcepts(dynamicConceptValue, dynamicConceptRelations);
            }

            return dynamicConcepts;
        }
    }
}
