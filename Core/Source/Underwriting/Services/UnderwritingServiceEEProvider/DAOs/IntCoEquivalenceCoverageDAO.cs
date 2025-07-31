using Sistran.Core.Application.Integration.Entities;
using ENQU = Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class IntCoEquivalenceCoverageDAO
    {
        public List<IntCoEquivalenceCoverage> GetCoEquivalenceCoverage(int coverageId)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(CoEquivalenceCoverage.Properties.Coverage3gId, "ce")));
            select.AddSelectValue(new SelectValue(new Column(CoEquivalenceCoverage.Properties.Coverage2gId, "ce")));
            select.AddSelectValue(new SelectValue(new Column(ENQU.Coverage.Properties.CoverageId, "co")));
            select.Distinct = true;

            Join join = new Join(new ClassNameTable(typeof(CoEquivalenceCoverage), "ce"), new ClassNameTable(typeof(ENQU.Coverage), "co"), JoinType.Right)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(CoEquivalenceCoverage.Properties.Coverage3gId, "ce").Equal()
                    .Property(ENQU.Coverage.Properties.CoverageId, "co").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();

            where.Property(ENQU.Coverage.Properties.CoverageId, "co").Equal().Constant(coverageId);
            where.Or().Property(CoEquivalenceCoverage.Properties.Coverage3gId, "ce").Equal().Constant(coverageId);

            select.Table = join;
            select.Where = where.GetPredicate();

            List<IntCoEquivalenceCoverage> listCoEquivalenceCoverage = new List<IntCoEquivalenceCoverage>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    listCoEquivalenceCoverage.Add(new IntCoEquivalenceCoverage
                    {
                        Coverage3GId = reader["Coverage3gId"] == null ? 0 : (int)reader["Coverage3gId"],
                        Coverage2GId = reader["Coverage2gId"] == null ? 0 : (int)reader["Coverage2gId"],
                        CoverageId = reader["CoverageId"] == null ? 0 : (int)reader["CoverageId"]
                    });
                }
            }

            return listCoEquivalenceCoverage;

        }
    }
}
