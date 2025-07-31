using System;
using System.Linq;
using System.Collections.Generic;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.RulesScriptsServices.Models;
using System.Data;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class ClauseDAO
    {
        /// <summary>
        /// Obtener Clausula
        /// </summary>
        /// <param name="clauseId">Id Clausula</param>
        /// <returns>Clausula</returns>
        public Clause GetClauseByClauseId(int clauseId)
        {
            PrimaryKey key = QUOEN.Clause.CreatePrimaryKey(clauseId);
            QUOEN.Clause entityClause = (QUOEN.Clause)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (entityClause != null)
            {
                return ModelAssembler.CreateClause(entityClause);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Clausulas
        /// </summary>
        /// <param name="clauseIds">Id's Clausulas</param>
        /// <returns>Clausulas</returns>
        public List<Clause> GetClausesByClauseIds(List<int> clauseIds)
        {
            List<Clause> clauses = new List<Clause>();

            if (clauseIds.Count > 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Property(QUOEN.Clause.Properties.ClauseId, typeof(QUOEN.Clause).Name).In().ListValue();

                foreach (int clauseId in clauseIds)
                {
                    filter.Constant(clauseId);
                }

                filter.EndList();

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.Clause), filter.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    clauses = ModelAssembler.CreateClauses(businessCollection);
                }
            }

            return clauses;
        }

        /// <summary>
        /// Obtener Clausulas
        /// </summary>
        /// <param name="emissionLevel">Nivel De Emisión</param>
        /// <param name="conditionLevelId">Id Condición De Nivel</param>
        /// <returns>Clausulas</returns>
        public List<Clause> GetClausesByEmissionLevelConditionLevelId(EmissionLevel emissionLevel, int conditionLevelId)
        {
            
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            
            filter.OpenParenthesis();
            filter.Property(QUOEN.Clause.Properties.CurrentTo, "Tclause").Greater().Constant(DateTime.Now);
            filter.Or();
            filter.Property(QUOEN.Clause.Properties.CurrentTo, "Tclause").IsNull();
            filter.CloseParenthesis();
            filter.And();
            filter.OpenParenthesis();
            filter.Property(QUOEN.Clause.Properties.CurrentTo, "Tclause").GreaterEqual().Constant(DateTime.Now);
            filter.Or();
            filter.Property(QUOEN.Clause.Properties.CurrentTo, "Tclause").IsNull();
            filter.CloseParenthesis();
            filter.And();
            filter.OpenParenthesis();
            filter.Property(QUOEN.ClauseLevel.Properties.ConditionLevelId, "TclauseLevel").Equal().Constant(conditionLevelId);
            filter.Or();
            filter.Property(QUOEN.ClauseLevel.Properties.ConditionLevelId, "TclauseLevel").IsNull();
            filter.CloseParenthesis();
            filter.And();
            //filter.OpenParenthesis();
            //filter.Property(QUOEN.ClauseLevel.Properties.IsMandatory, "TclauseLevel").Distinct().Constant(0);//Constant(0);
            //filter.Or();
            //filter.Property(QUOEN.ClauseLevel.Properties.ConditionLevelId, "TclauseLevel").IsNull();
            //filter.CloseParenthesis();
            //filter.And();
            //filter.Property(QUOEN.ClauseLevel.Properties.IsMandatory, "TclauseLevel").Distinct().Constant(0);
            //filter.And();
            filter.Property(PARAMEN.Levels.Properties.LevelId, "Tleves").Equal().Constant(emissionLevel);    

            SelectQuery SelectQuery = new SelectQuery();
            #region Select
            SelectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.Clause.Properties.ClauseId, "Tclause")));
            SelectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.Clause.Properties.ClauseName, "Tclause")));
            SelectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.Clause.Properties.ClauseTitle, "Tclause")));
            SelectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.Clause.Properties.ClauseText, "Tclause")));
            SelectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.ClauseLevel.Properties.IsMandatory, "TclauseLevel")));
            #endregion Select

            Join join = new Join(new ClassNameTable(typeof(PARAMEN.Levels), "Tleves"), new ClassNameTable(typeof(PARAMEN.ConditionLevel), "TconditionLevel"), JoinType.Left)
            {
                Criteria = new ObjectCriteriaBuilder()
               .Property(PARAMEN.Levels.Properties.LevelId, "Tleves")
               .Equal()
               .Property(PARAMEN.ConditionLevel.Properties.LevelId, "TconditionLevel")
               .And()
               .Property(PARAMEN.Levels.Properties.PackageId, "Tleves")
               .Equal()
               .Property(PARAMEN.ConditionLevel.Properties.PackageId, "TconditionLevel")
               .GetPredicate()
            };

            join = new Join(join, new ClassNameTable(typeof(QUOEN.Clause), "Tclause"), JoinType.Left)
            {
                Criteria = (new ObjectCriteriaBuilder()
                .Property(PARAMEN.ConditionLevel.Properties.ConditionLevelCode, "TconditionLevel")
                .Equal()
                .Property(QUOEN.Clause.Properties.ConditionLevelCode, "Tclause")
                .GetPredicate())
            };


            join = new Join(join, new ClassNameTable(typeof(QUOEN.ClauseLevel), "TclauseLevel"), JoinType.Inner)
            {
                Criteria = (new ObjectCriteriaBuilder()
               .Property(QUOEN.Clause.Properties.ClauseId, "Tclause")
               .Equal()
               .Property(QUOEN.ClauseLevel.Properties.ClauseId, "TclauseLevel")
               .GetPredicate())
            };
            SelectQuery.Table = join;
            SelectQuery.Where = filter.GetPredicate();
            List<Clause> clauses = new List<Clause>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(SelectQuery))
            {
                while (reader.Read())
                {
                    clauses.Add(new Clause
                    {
                        Id = (int)reader["ClauseId"],
                        Name = (string)reader["ClauseName"],
                        Title = (string)reader["ClauseTitle"],
                        Text = (string)reader["ClauseText"],
                        IsMandatory = (bool)reader["IsMandatory"]
                    });
                }
            }

            //filter.Property(PARAMEN.ConditionLevel.Properties.LevelId, typeof(PARAMEN.ConditionLevel).Name).Equal().Constant(emissionLevel);
            //filter.And();
            //filter.OpenParenthesis();
            //filter.Property(QUOEN.Clause.Properties.CurrentTo, typeof(QUOEN.Clause).Name).GreaterEqual().Constant(DateTime.Now);
            //filter.Or();
            //filter.Property(QUOEN.Clause.Properties.CurrentTo, typeof(QUOEN.Clause).Name).IsNull();
            //filter.CloseParenthesis();
            //filter.And();
            //filter.OpenParenthesis();
            //filter.Property(QUOEN.ClauseLevel.Properties.ConditionLevelId, typeof(QUOEN.ClauseLevel).Name).Equal().Constant(conditionLevelId);
            //filter.Or();
            //filter.Property(QUOEN.ClauseLevel.Properties.ConditionLevelId, typeof(QUOEN.ClauseLevel).Name).IsNull();
            //filter.CloseParenthesis();

            //ClauseView view = new ClauseView();
            //ViewBuilder builder = new ViewBuilder("ClauseView");
            //builder.Filter = filter.GetPredicate();

            //using (var daf = DataFacadeManager.Instance.GetDataFacade())
            //{
            //    daf.FillView(builder, view);
            //}

            //if (view.Clauses != null && view.Clauses.Count > 0)
            //{
            //    clauses = ModelAssembler.CreateClauses(view.Clauses);
            //    List<QUOEN.ClauseLevel> entityClauseLevels = view.ClauseLevels.Cast<QUOEN.ClauseLevel>().ToList();
            //    clauses.Where(x => entityClauseLevels.Select(a => a.ClauseId).Contains(x.Id)).AsParallel().ForAll(
            //        z =>
            //        {
            //            z.IsMandatory = entityClauseLevels?.FirstOrDefault(x => x.ClauseId == z.Id)?.IsMandatory ?? false;
            //            z.ConditionLevel = new ConditionLevel
            //            {
            //                ConditionValue = entityClauseLevels?.FirstOrDefault(x => x.ClauseId == z.Id)?.ConditionLevelId ?? 0,
            //                Package = new Package()
            //            };
            //        });
            //}

            return clauses;
        }
        /// <summary>
        /// Obtener Clausulas
        /// </summary>
        /// <param name="emissionLevel">Nivel De Emisión</param>
        /// <returns>Clausulas</returns>
        public List<Clause> GetClausesByEmissionLevel(EmissionLevel emissionLevel)
        {
            List<Clause> clauses = new List<Clause>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(PARAMEN.ConditionLevel.Properties.LevelId, typeof(PARAMEN.ConditionLevel).Name).Equal().Constant(emissionLevel);
            filter.And();
            filter.OpenParenthesis();
            filter.Property(QUOEN.Clause.Properties.CurrentTo, typeof(QUOEN.Clause).Name).GreaterEqual().Constant(DateTime.Now);
            filter.Or();
            filter.Property(QUOEN.Clause.Properties.CurrentTo, typeof(QUOEN.Clause).Name).IsNull();
            filter.CloseParenthesis();


            ClauseView view = new ClauseView();
            ViewBuilder builder = new ViewBuilder("ClauseView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.Clauses.Count > 0)
            {
                clauses = ModelAssembler.CreateClauses(view.Clauses);
                List<QUOEN.ClauseLevel> entityClauseLevels = view.ClauseLevels.Cast<QUOEN.ClauseLevel>().ToList();

                foreach (Clause clause in clauses)
                {
                    if (entityClauseLevels.Exists(x => x.ClauseId == clause.Id))
                    {
                        clause.IsMandatory = entityClauseLevels.First(x => x.ClauseId == clause.Id).IsMandatory;
                        clause.ConditionLevel = new ConditionLevel
                        {
                            ConditionValue = entityClauseLevels.First(x => x.ClauseId == clause.Id).ConditionLevelId.Value
                        };
                    }
                }
            }

            return clauses;
        }

        /// <summary>
        /// Obtuene todas las cláusulas
        /// </summary>
        /// <returns></returns>
        public List<Clause> GetClauseAll()
        {
            BusinessCollection<QUOEN.Clause> clauses = DataFacadeManager.Instance.GetDataFacade().List<QUOEN.Clause>(null);
            List<Clause> clausesModels = ModelAssembler.CreateClauses(clauses);
            return clausesModels;
        }
        
        /// Obtener Clausulas
        /// </summary>
        /// <param name="emissionLevel">Nivel De Emisión</param>
        /// <param name="conditionLevelId">Id Condición De Nivel</param>
        /// <returns>Clausulas</returns>
        public List<Clause> GetClausesByEmissionLevelConditionLevelId(int PolicyIg)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //Obtener Clausulas
             filter.Property(Sistran.Core.Application.Issuance.Entities.PolicyClause.Properties.PolicyId, typeof(Sistran.Core.Application.Issuance.Entities.PolicyClause).Name);
            filter.Equal();
            filter.Constant(PolicyIg);
            filter.And();
            filter.Property(Sistran.Core.Application.Issuance.Entities.PolicyClause.Properties.IsCurrent, typeof(Sistran.Core.Application.Issuance.Entities.PolicyClause).Name);
            filter.Equal();
            filter.Constant(1);

            //IList iListClause = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(ISSEN.PolicyClause), filter.GetPredicate());
            //List<ISSEN.PolicyClause> policyClause = iListClause.Cast<ISSEN.PolicyClause>().ToList();
            
            SelectQuery SelectQuery = new SelectQuery();
            #region Select
            SelectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.Clause.Properties.ClauseId, "Tclause")));
            SelectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.Clause.Properties.ClauseName, "Tclause")));
            SelectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.Clause.Properties.ClauseTitle, "Tclause")));
            SelectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.Clause.Properties.ClauseText, "Tclause")));
            #endregion Select

            Join join = new Join(new ClassNameTable(typeof(Sistran.Core.Application.Issuance.Entities.PolicyClause), typeof(Sistran.Core.Application.Issuance.Entities.PolicyClause).Name), new ClassNameTable(typeof(QUOEN.Clause), "Tclause"), JoinType.Left)
            {
                Criteria = (new ObjectCriteriaBuilder()
                .Property(QUOEN.Clause.Properties.ClauseId, "Tclause")
                .Equal()
                .Property(Sistran.Core.Application.Issuance.Entities.PolicyClause.Properties.ClauseId, typeof(Sistran.Core.Application.Issuance.Entities.PolicyClause).Name)
                .GetPredicate())
            };

           


          
            SelectQuery.Table = join;
            SelectQuery.Where = filter.GetPredicate();
            List<Clause> clauses = new List<Clause>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(SelectQuery))
            {
                while (reader.Read())
                {
                    clauses.Add(new Clause
                    {
                        Id = (int)reader["ClauseId"],
                        Name = (string)reader["ClauseName"],
                        Title = (string)reader["ClauseTitle"],
                        Text = (string)reader["ClauseText"],
                        
                    });
                }
            }

            

            return clauses;
        }

    }
}