using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.Queries;
using System;
using System.Data;
using System.Linq;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
namespace Sistran.Core.Integration.UnderwritingServices.EEProvider.DAOs
{
    public class IntCouinsuranceDAO
    {

        public static int GetPercentageByEndorsementId(int endorsementId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ISSEN.Endorsement.Properties.EndorsementId);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(endorsementId);
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.CoissuePercentage, typeof(ISSEN.Policy).Name), ISSEN.Policy.Properties.CoissuePercentage));
            Join join = new Join(new ClassNameTable(typeof(ISSEN.Policy), typeof(ISSEN.Policy).Name), new ClassNameTable(typeof(ISSEN.Endorsement), typeof(ISSEN.Endorsement).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder().Property(ISSEN.Policy.Properties.PolicyId, typeof(ISSEN.Policy).Name).Equal().Property(ISSEN.Endorsement.Properties.PolicyId, typeof(ISSEN.Endorsement).Name)
           .GetPredicate());
            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();            
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                return reader.SelectReader(read => Convert.ToInt32(read[ISSEN.Policy.Properties.CoissuePercentage])).FirstOrDefault();
            }
        }
    }
}
