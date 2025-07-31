using System;
using System.Collections.Generic;
using System.Data;
using MCommon = Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using EUtilities = Sistran.Core.Application.Script.Entities;
using APEntity = Sistran.Core.Application.AuthorizationPolicies.Entities;
using MRules = Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Application.Utilities.DataFacade;

//using modelsUtilities = Sistran.Core.Application.UtilitiesServices.Models;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.DAOs
{
    public class ConceptDescriptionDao
    {
        /// <summary>
        /// Obtiene los conceptos descripcion por Id de la Politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <returns></returns>
        public List<Models.ConceptDescription> GetConceptDescriptionsByIdPolicies(int idPolicies)
        {            
            SelectQuery select = new SelectQuery();

            select.AddSelectValue(new SelectValue(new Column(APEntity.ConceptDescription.Properties.PoliciesId, "cd")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.ConceptDescription.Properties.ConceptId, "cd")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.ConceptDescription.Properties.EntityId, "cd")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.ConceptDescription.Properties.Order, "cd")));

            select.AddSelectValue(new SelectValue(new Column(APEntity.ConceptDescriptionValue.Properties.TableName, "cdv")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.ConceptDescriptionValue.Properties.Fields, "cdv")));
            select.AddSelectValue(new SelectValue(new Column(APEntity.ConceptDescriptionValue.Properties.Filter, "cdv")));

            select.AddSelectValue(new SelectValue(new Column(EUtilities.Concept.Properties.Description, "c")));
            select.AddSelectValue(new SelectValue(new Column(EUtilities.Concept.Properties.ConceptName, "c")));
            select.AddSelectValue(new SelectValue(new Column(EUtilities.Concept.Properties.IsStatic, "c")));


            Join join = new Join(new ClassNameTable(typeof(APEntity.ConceptDescription), "cd"), new ClassNameTable(typeof(APEntity.ConceptDescriptionValue), "cdv"), JoinType.Left)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.ConceptDescription.Properties.EntityId, "cd")
                        .Equal().Property(APEntity.ConceptDescriptionValue.Properties.EntityId, "cdv").And()
                        .Property(APEntity.ConceptDescription.Properties.ConceptId, "cd")
                        .Equal().Property(APEntity.ConceptDescriptionValue.Properties.ConceptId, "cdv").GetPredicate()
            };
            join = new Join(join, new ClassNameTable(typeof(EUtilities.Concept), "c"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(APEntity.ConceptDescription.Properties.EntityId, "cd")
                    .Equal().Property(EUtilities.Concept.Properties.EntityId, "c").And()

                    .Property(APEntity.ConceptDescription.Properties.ConceptId, "cd")
                    .Equal().Property(EUtilities.Concept.Properties.ConceptId, "c").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(APEntity.ConceptDescription.Properties.PoliciesId, "cd").Equal().Constant(idPolicies);

            select.Table = join;
            select.Where = where.GetPredicate();

            select.AddSortValue(new SortValue(new Column(APEntity.ConceptDescription.Properties.Order, "cd"), SortOrderType.Ascending));

            List<Models.ConceptDescription> result = new List<Models.ConceptDescription>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    var conceptDescription = new Models.ConceptDescription
                    {
                        Policies = new Models.PoliciesAut { IdPolicies = (int)reader["PoliciesId"], Type = Enums.TypePolicies.Authorization },
                        Concept = new MRules._Concept
                        {
                            Description = (string)reader["Description"],
                            ConceptId = (int)reader["ConceptId"],
                            Entity = new MRules.Entity { EntityId = (int)reader["EntityId"] },
                            ConceptName = (string)reader["ConceptName"],
                            IsStatic = (bool)reader["IsStatic"],
                            ConceptType = ConceptType.Basic,
                            ConceptControlType = ConceptControlType.TextBox
                        },
                        Order = (int)reader["Order"],
                        Name = (string)reader["Description"],
                        Description = (string)reader["Description"]
                        
                    };

                    var tableName = reader["TableName"];
                    if (tableName != null)
                    {
                        conceptDescription.ConceptDescriptionValue = new Models.ConceptDescriptionValue
                        {
                            Concept = conceptDescription.Concept,
                            Fields = (string)reader["Fields"],
                            TableName = (string)tableName,
                            Filter = (string)reader["Filter"]
                        };
                    }
                    result.Add(conceptDescription);
                }
            }

            foreach (Models.ConceptDescription item in result)
            {
                MRules._Concept ConceptoDependencies = DelegateService.ConceptService.GetConceptByIdConceptIdEntity(item.Concept.ConceptId, item.Concept.Entity.EntityId);
                item.Concept.ConceptDependences = ConceptoDependencies.ConceptDependences;
            }

            return result;
        }

        /// <summary>
        /// Guarda los conceptos asociados a la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="conceptDescriptions">lista de conceptos</param>
        /// <returns></returns>
        public void SaveConceptDescriptions(int idPolicies, List<Models.ConceptDescription> conceptDescriptions)
        {

            bool createdRaised = false;
            bool disposedRaised = false;
            bool completedRaised = false;

            IDataFacadeManager dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
            IDataFacade df = dataFacadeManager.GetDataFacade();

            Transaction.Created += delegate (object sender, TransactionEventArgs e)
            {
                createdRaised = true;
            };

            using (Context.Current)
            using (Transaction transaction = new Transaction())
            {
                transaction.Completed += delegate (object sender, TransactionEventArgs e)
                {
                    completedRaised = true;
                };
                transaction.Disposed += delegate (object sender, TransactionEventArgs e)
                {
                    disposedRaised = true;
                };

                try
                {
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(APEntity.ConceptDescription.Properties.PoliciesId).Equal().Constant(idPolicies);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(APEntity.ConceptDescription), filter.GetPredicate());

                    if (conceptDescriptions != null)
                    {
                        var businessCollection = new BusinessCollection();
                        for (var index = 0; index < conceptDescriptions.Count; index++)
                        {
                            var conceptDescription = conceptDescriptions[index];
                            conceptDescription.Policies = new Models.PoliciesAut { IdPolicies = idPolicies };
                            conceptDescription.Order = index + 1;

                            APEntity.ConceptDescription entityConcepts = Assemblers.EntityAssembler.CreateConceptDescription(conceptDescription);
                            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityConcepts);
                        }
                    }

                    transaction.Complete();
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    throw new Exception(e.Message, e);
                }
            }
        }
    }
}
