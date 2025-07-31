using AutoMapper;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.UnderwritingServices.EEProvider;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COENUMS = Sistran.Core.Application.CommonService.Enums;
using ModelCore = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class ClauseDAO
    {
        UnderwritingServiceEEProviderCore underwritingServiceEEProviderCore = new UnderwritingServiceEEProviderCore();

        /// <summary>
        /// Obtiene la lista de clausas por ramo ramo tecnico
        /// </summary>
        /// <param name="coveredRiskType"></param>
        /// <returns></returns>
        public List<CompanyClause> GetClausesByCoveredRiskType(COENUMS.CoveredRiskType coveredRiskType)
        {
            var stopWach = Stopwatch.StartNew();
            ClauseView view = new ClauseView();
            ViewBuilder builder = new ViewBuilder("ClauseView");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(Clause.Properties.ConditionLevelCode, "Clause", ConditionLevelType.Risk);
            filter.And().PropertyEquals(ClauseLevel.Properties.ConditionLevelId, "ClauseLevel", coveredRiskType);
            builder.Filter = filter.GetPredicate();

            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            List<ModelCore.Clause> clauses = ModelAssembler.CreateClauses(view.Clauses);
            foreach (ModelCore.Clause clause in clauses)
            {
                BusinessObject clauseLevel = view.ClauseLevels.First(x => ((ClauseLevel)x).ClauseId == clause.Id);
                clause.IsMandatory = ((ClauseLevel)clauseLevel).IsMandatory;
            }
            stopWach.Stop();
            Debug.WriteLine(stopWach.Elapsed.ToString(), GetType().FullName);
            IMapper config = ModelAssembler.CreateMapCompanyClause();
            return config.Map<List<ModelCore.Clause>, List<CompanyClause>>(clauses);
        }

        /// <summary>
        /// Obtener lista de clausulas por Covertura
        /// </summary>
        /// <param name="CoverageId">nivel</param>
        /// <returns></returns>
        public List<CompanyClause> GetClausesByCoverageId(int coverageId)
        {
            var stopWatch = Stopwatch.StartNew();

            ClauseView view = new ClauseView();
            ViewBuilder builder = new ViewBuilder("ClauseView");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(Clause.Properties.ConditionLevelCode, "Clause", ConditionLevelType.Coverage);
            filter.And().PropertyEquals(ClauseLevel.Properties.ConditionLevelId, "ClauseLevel", coverageId);
            builder.Filter = filter.GetPredicate();
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            List<ModelCore.Clause> clauses = ModelAssembler.CreateClauses(view.Clauses);
            foreach (ModelCore.Clause clause in clauses)
            {
                BusinessObject clauseLevel = view.ClauseLevels.First(x => ((ClauseLevel)x).ClauseId == clause.Id);
                if (clauseLevel != null)
                {
                    clause.IsMandatory = ((ClauseLevel)clauseLevel).IsMandatory;
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), GetType().FullName);
            IMapper config = ModelAssembler.CreateMapCompanyClause();
            return config.Map<List<ModelCore.Clause>, List<CompanyClause>>(clauses);
        }

        /// <summary>
        /// Obtiene la información de las Clausulas.
        /// </summary>
        /// <param name="ClauseId"></param>
        /// <returns></returns>
        public CompanyClause GetAllClauseByClauseId(int ClauseId)
        {
            ModelCore.Clause response = null;
            Clause clause = null;
            if (ClauseId > 0)
            {
                PrimaryKey key = Clause.CreatePrimaryKey(ClauseId);
                clause = (Clause)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                if (clause != null)
                {
                    response = HelperAssembler.CreateObjectMappingEqualProperties<Clause, ModelCore.Clause>(clause);
                    response.Id = ClauseId;
                }
            }
            IMapper config = ModelAssembler.CreateMapCompanyClause();
            return config.Map<ModelCore.Clause, CompanyClause>(response);
        }

        public List<CompanyClause> GetClauseAll()
        {
            BusinessCollection<Clause> clauses = DataFacadeManager.Instance.GetDataFacade().List<Clause>(null);
            List<ModelCore.Clause> clausesModels = ModelAssembler.CreateClauses(clauses);
            IMapper config = ModelAssembler.CreateMapCompanyClause();
            return config.Map<List<ModelCore.Clause>, List<CompanyClause>>(clausesModels);
        }

        public List<CompanyClause> RemoveClauses(List<CompanyClause> companyClauses, List<int> clauseIds)
        {
            if (companyClauses != null && clauseIds != null)
            {
                companyClauses = companyClauses.Where(x => clauseIds.Any(y => y == x.Id)).ToList();
            }
            return companyClauses;
        }

        public List<CompanyClause> AddClauses(List<CompanyClause> companyClauses, List<int> clauseIds)
        {
            if (clauseIds != null)
            {
                if (companyClauses == null)
                {
                    companyClauses = new List<CompanyClause>();
                }
                companyClauses = companyClauses.Where(x => clauseIds.Any(y => y == x.Id)).ToList();
                foreach (CompanyClause clause in GetCompanyClausesByClauseIds(clauseIds))
                {
                    if (!companyClauses.Any(x => x.Id == clause.Id))
                    {
                        companyClauses.Add(clause);
                    }
                }
            }
            return companyClauses;
        }

        public List<CompanyClause> GetCompanyClausesByClauseIds(List<int> clauseIds)
        {
            List<ModelCore.Clause> clauses = underwritingServiceEEProviderCore.GetClausesByClauseIds(clauseIds);
            IMapper config = ModelAssembler.CreateMapCompanyClause();
            return config.Map<List<ModelCore.Clause>, List<CompanyClause>>(clauses);
        }

        public List<ModelCore.ConditionLevel> GetConditionLevels()
        {
            List<ModelCore.ConditionLevel> conditionLevels = new List<ModelCore.ConditionLevel>();
            BusinessCollection cLevels = new BusinessCollection();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                cLevels = new BusinessCollection(daf.SelectObjects(typeof(ConditionLevel)));
            }

            foreach (var conditionLevel in cLevels.Cast<ConditionLevel>())            
            {
                conditionLevels.Add(new ModelCore.ConditionLevel
                {
                    Id = conditionLevel.ConditionLevelCode,
                    Description = conditionLevel.SmallDescription,
                    EmissionLevel = (EmissionLevel)conditionLevel.LevelId
                });
            }

            return conditionLevels;
        }
    }
}
