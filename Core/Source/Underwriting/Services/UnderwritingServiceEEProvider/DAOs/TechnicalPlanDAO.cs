using Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Model = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class TechnicalPlanDAO
    {
        /// <summary>
        /// Obtener Planes Tecnicos por Tipo de Riesgo Cubierto
        /// </summary>
        /// <param name="coveredRiskTypeId">Tipo de Riesgo Cubierto</param>
        /// <returns>Planes Tecnicos</returns>
        public List<Model.TechnicalPlan> GetTechnicalPlanByCoveredRiskTypeId(int coveredRiskTypeId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection();

            if (coveredRiskTypeId != 0)
            {
                ObjectCriteriaBuilder filterTechnicalPlan = new ObjectCriteriaBuilder();
                filterTechnicalPlan.Property(TechnicalPlan.Properties.CoveredRiskTypeCode, typeof(TechnicalPlan).Name);
                filterTechnicalPlan.Equal();
                filterTechnicalPlan.Constant(coveredRiskTypeId);
                businessCollection =
                    new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(TechnicalPlan),
                        filterTechnicalPlan.GetPredicate()));
            }
            else
            {
                businessCollection =
                new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(TechnicalPlan)));
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetTechnicalPlanByCoveredRiskTypeId");

            return ModelAssembler.CreateTechnicalPlans(businessCollection).OrderBy(x => x.Description).ToList();
        }

        /// <summary>
        /// Obtener Planes Tecnicos por Tipo de Riesgo Cubierto
        /// </summary>
        /// <param name="coveredRiskTypeId">Tipo de Riesgo Cubierto</param>
        /// <param name="insuredObjectId">objeto del seguro</param>
        /// <returns></returns>
        public List<Model.TechnicalPlan> GetTechnicalPlanByCoveredRiskTypeIdInsuredObjectId(int coveredRiskTypeId, int insuredObjectId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection();

            if (coveredRiskTypeId != 0)
            {

                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(TechnicalPlan.Properties.CoveredRiskTypeCode, "t")));
                select.AddSelectValue(new SelectValue(new Column(TechnicalPlan.Properties.TechnicalPlanId, "t")));
                select.AddSelectValue(new SelectValue(new Column(TechnicalPlan.Properties.Description, "t")));
                select.AddSelectValue(new SelectValue(new Column(TechnicalPlan.Properties.SmallDescription, "t")));
                select.AddSelectValue(new SelectValue(new Column(TechnicalPlan.Properties.CurrentFrom, "t")));
                select.AddSelectValue(new SelectValue(new Column(TechnicalPlan.Properties.CurrentTo, "t")));


                Join join = new Join(new ClassNameTable(typeof(TechnicalPlan), "t"), new ClassNameTable(typeof(TechnicalPlanCoverage), "tc"), JoinType.Inner);
                join.Criteria = new ObjectCriteriaBuilder().Property(TechnicalPlan.Properties.TechnicalPlanId, "t").Equal().Property(TechnicalPlanCoverage.Properties.TechnicalPlanId, "tc").GetPredicate();

                join = new Join(join, new ClassNameTable(typeof(Coverage), "c"), JoinType.Inner);
                join.Criteria = new ObjectCriteriaBuilder().Property(TechnicalPlanCoverage.Properties.CoverageId, "tc").Equal().Property(Coverage.Properties.CoverageId, "c").GetPredicate();


                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(TechnicalPlan.Properties.CoveredRiskTypeCode, "t").Equal().Constant(coveredRiskTypeId).And()
                    .Property(Coverage.Properties.InsuredObjectId, "c").Equal().Constant(insuredObjectId);

                select.Table = join;
                select.Where = where.GetPredicate();

                List<Model.TechnicalPlan> list = new List<Model.TechnicalPlan>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        var id = Convert.ToInt32(reader["TechnicalPlanId"]);
                        if (list.Count(x => x.TechnicalPlanId == id) == 0)
                        {
                            list.Add(new Models.TechnicalPlan()
                            {
                                TechnicalPlanId = Convert.ToInt32(reader["TechnicalPlanId"]),
                                Description = Convert.ToString(reader["Description"]),
                                SmallDescription = Convert.ToString(reader["SmallDescription"]),
                                CoveredRiskTypeCode = Convert.ToInt32(reader["CoveredRiskTypeCode"]),
                            });
                        }
                    }
                }

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetTechnicalPlanByCoveredRiskTypeIdInsuredObjectId");

                return list.OrderBy(x => x.Description).ToList();
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetTechnicalPlanByCoveredRiskTypeIdInsuredObjectId");

                return ModelAssembler.CreateTechnicalPlans(businessCollection).OrderBy(x => x.Description).ToList();
            }
        }
    }
}