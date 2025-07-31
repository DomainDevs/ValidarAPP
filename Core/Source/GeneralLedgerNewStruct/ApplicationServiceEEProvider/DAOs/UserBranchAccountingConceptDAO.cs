#region Using

//Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingConcepts;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;

#endregion Using

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    public class UserBranchAccountingConceptDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveUserBranchAccountingConcept
        /// </summary>
        /// <param name="userBranchAccountingConcept">Model</param>
        /// <returns>ModelAssembler</returns>
        public UserBranchAccountingConcept SaveUserBranchAccountingConcept(UserBranchAccountingConcept userBranchAccountingConcept)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.UserBranchAccountingConcept userBranchAccountingConceptEntity =
                    EntityAssembler.CreateUserBranchAccountingConcept(userBranchAccountingConcept);


                BusinessCollection businessCollection = GetUserBranchAccountingConceptByCriteria(userBranchAccountingConcept.BranchAccountingConcept.Id,
                                                                              userBranchAccountingConcept.UserId);

                var userBranchAccountingConceptId = 0;

                if (businessCollection.Count == 0)
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().InsertObject(userBranchAccountingConceptEntity);
                }
                else
                {
                    foreach (GENERALLEDGEREN.UserBranchAccountingConcept ItemEntities in businessCollection.OfType<GENERALLEDGEREN.UserBranchAccountingConcept>())
                    {
                        userBranchAccountingConceptId = Convert.ToInt32(ItemEntities.UserBranchAccountingConceptId);
                    }
                    userBranchAccountingConceptEntity.UserBranchAccountingConceptId = userBranchAccountingConceptId;
                }

                // Return del model
                return ModelAssembler.CreateUserBranchAccountingConcept(userBranchAccountingConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateUserBranchAccountingConcept
        /// </summary>
        /// <param name="userBranchAccountingConcept">Model</param>
        /// <returns>ModelAssembler</returns>
        public UserBranchAccountingConcept UpdateUserBranchAccountingConcept(UserBranchAccountingConcept userBranchAccountingConcept)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.UserBranchAccountingConcept.CreatePrimaryKey(userBranchAccountingConcept.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.UserBranchAccountingConcept userBranchAccountingConceptEntity =
                    (GENERALLEDGEREN.UserBranchAccountingConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                userBranchAccountingConceptEntity.BranchAccountingConceptId = userBranchAccountingConcept.BranchAccountingConcept.Id;
                userBranchAccountingConceptEntity.UserCode = userBranchAccountingConcept.UserId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(userBranchAccountingConceptEntity);

                // Return del model
                return ModelAssembler.CreateUserBranchAccountingConcept(userBranchAccountingConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteUserBranchAccountingConcept
        /// </summary>
        /// <param name="userBranchAccountingConcept">Model</param>
        /// <returns>bool</returns>
        public bool DeleteUserBranchAccountingConcept(UserBranchAccountingConcept userBranchAccountingConcept)
        {
            bool isDeleted = false;

            try
            {   // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.UserBranchAccountingConcept.CreatePrimaryKey(userBranchAccountingConcept.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.UserBranchAccountingConcept userBranchAccountingConceptEntity =
                    (GENERALLEDGEREN.UserBranchAccountingConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(userBranchAccountingConceptEntity);

                isDeleted = true;
            }
            catch
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        #endregion

        #region Get

        /// <summary>
        /// GetUserBranchAccountingConcept
        /// </summary>
        /// <param name="userBranchAccountingConcept">Model</param>
        /// <returns>ModelAssembler</returns>
        public UserBranchAccountingConcept GetUserBranchAccountingConcept(UserBranchAccountingConcept userBranchAccountingConcept)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.UserBranchAccountingConcept.CreatePrimaryKey(userBranchAccountingConcept.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.UserBranchAccountingConcept userBranchAccountingConceptEntity =
                    (GENERALLEDGEREN.UserBranchAccountingConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateUserBranchAccountingConcept(userBranchAccountingConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetUserBranchAccountingConcepts
        /// </summary>
        /// <returns>List UserBranchAccountingConcept</returns>
        public List<UserBranchAccountingConcept> GetUserBranchAccountingConcepts()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection =
                    new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.UserBranchAccountingConcept)));

                // Return como Lista
                return ModelAssembler.CreateUserBranchAccountingConcepts(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetUserBranchAccountingConceptByUserByBranch
        /// </summary>
        /// <param name="userId">Model</param>
        /// <param name="branch">Model</param>
        /// <returns>List UserBranchAccountingConcept</returns>
        public List<UserBranchAccountingConcept> GetUserBranchAccountingConceptByUserByBranch_old(int userId, Branch branch)
        {
            try
            {
                List<UserBranchAccountingConcept> userBranchAccountingConcepts = new List<UserBranchAccountingConcept>();

                // Se obtiene la relacion de [ (conceptos contables-sucursal) - (usuarios)]  por criterio (UserCode)
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.UserBranchAccountingConcept.Properties.UserCode, userId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.UserBranchAccountingConcept), criteriaBuilder.GetPredicate()));

                // Se obtiene la relacion de [ (conceptos contables-sucursal) ]  por criterio (idsucursal_conceptoscontables)
                foreach (GENERALLEDGEREN.UserBranchAccountingConcept userBranchAccountingConcept in businessCollection.OfType<GENERALLEDGEREN.UserBranchAccountingConcept>())
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.BranchAccountingConcept.Properties.BranchAccountingConceptId,
                                                    userBranchAccountingConcept.BranchAccountingConceptId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.BranchAccountingConcept.Properties.BranchCode, branch.Id);

                    BusinessCollection branchBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                    typeof(GENERALLEDGEREN.BranchAccountingConcept), criteriaBuilder.GetPredicate()));

                    foreach (GENERALLEDGEREN.BranchAccountingConcept branchAccountingConcept in branchBusinessCollection.OfType<GENERALLEDGEREN.BranchAccountingConcept>())
                    {
                        userBranchAccountingConcepts.Add(new UserBranchAccountingConcept
                        {
                            Id = userBranchAccountingConcept.UserBranchAccountingConceptId,
                            BranchAccountingConcept = ModelAssembler.CreateBranchAccountingConcept(branchAccountingConcept),
                            UserId = userBranchAccountingConcept.UserCode
                        });
                    }
                }
                return userBranchAccountingConcepts;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<UserBranchAccountingConcept> GetUserBranchAccountingConceptByUserByBranch(int userId, Branch branch)
        {
            try
            {




                List<UserBranchAccountingConcept> userBranchAccountingConcepts = new List<UserBranchAccountingConcept>();

                // Se obtiene la relacion de [ (conceptos contables-sucursal) - (usuarios)]  por criterio (UserCode)
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.UserBranchAccountingConcept.Properties.UserCode, userId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.UserBranchAccountingConcept), criteriaBuilder.GetPredicate()));

                // Se obtiene la relacion de [ (conceptos contables-sucursal) ]  por criterio (idsucursal_conceptoscontables)
                foreach (GENERALLEDGEREN.UserBranchAccountingConcept userBranchAccountingConcept in businessCollection.OfType<GENERALLEDGEREN.UserBranchAccountingConcept>())
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();

                    criteriaBuilder.Property(GENERALLEDGEREN.BranchAccountingConcept.Properties.BranchAccountingConceptId, "bac");
                    criteriaBuilder.Equal();
                    criteriaBuilder.Constant(userBranchAccountingConcept.BranchAccountingConceptId);
                    criteriaBuilder.And();
                    criteriaBuilder.Property(GENERALLEDGEREN.BranchAccountingConcept.Properties.BranchCode, "bac");
                    criteriaBuilder.Equal();
                    criteriaBuilder.Constant(branch.Id);

                    SelectQuery selectQuery = new SelectQuery();
                    selectQuery.AddSelectValue(new SelectValue(new Column(GENERALLEDGEREN.AccountingAccount.Properties.AccountNumber, "a"), "AccountNumber"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GENERALLEDGEREN.AccountingAccount.Properties.AccountingAccountId, "a"), "AccountingAccountId"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GENERALLEDGEREN.BranchAccountingConcept.Properties.BranchAccountingConceptId, "bac"),"BranchAccountingConceptId"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GENERALLEDGEREN.BranchAccountingConcept.Properties.BranchCode, "bac"), "BranchCode"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GENERALLEDGEREN.BranchAccountingConcept.Properties.AccountingConceptCode, "bac"), "AccountingConceptCode"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GENERALLEDGEREN.BranchAccountingConcept.Properties.MovementTypeCode, "bac"), "MovementTypeCode"));
                    selectQuery.AddSelectValue(new SelectValue(new Column(GENERALLEDGEREN.BranchAccountingConcept.Properties.ConceptSourceCode, "bac"), "ConceptSourceCode"));

                    Join join = new Join(new ClassNameTable(typeof(GENERALLEDGEREN.AccountingConcept), "ac"), new ClassNameTable(typeof(GENERALLEDGEREN.AccountingAccount), "a"), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(GENERALLEDGEREN.AccountingConcept.Properties.AccountingAccountId, "ac")
                    .Equal()
                    .Property(GENERALLEDGEREN.AccountingAccount.Properties.AccountingAccountId, "a"))
                    .GetPredicate();

                    join = new Join(join, new ClassNameTable(typeof(GENERALLEDGEREN.BranchAccountingConcept), "bac"), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(GENERALLEDGEREN.BranchAccountingConcept.Properties.AccountingConceptCode, "bac")
                        .Equal()
                        .Property(GENERALLEDGEREN.AccountingConcept.Properties.AccountingConceptCode, "ac")
                        .GetPredicate());


                    selectQuery.Table = join;
                    selectQuery.Where = criteriaBuilder.GetPredicate();
                    using (IDataReader reader = _dataFacadeManager.GetDataFacade().Select(selectQuery))
                    {
                        while (reader.Read())
                        {
                            BranchAccountingConcept
                            branchAccountingConcept = new BranchAccountingConcept
                            {
                                Id = Convert.ToInt32(reader["BranchAccountingConceptId"]),
                                Branch = new Branch
                                {
                                    Id = Convert.ToInt32(reader["BranchCode"])
                                },
                                AccountingConcept = new AccountingConcept
                                {
                                    Id = Convert.ToInt32(reader["AccountingConceptCode"]),
                                    AccountingAccount = new AccountingAccount
                                    {
                                        AccountingAccountId = Convert.ToInt16(reader["AccountingAccountId"]),
                                        Number = Convert.ToString(reader["AccountNumber"])
                                    }
                                },
                                MovementType = new MovementType()
                                {
                                    Id = Convert.ToInt32(reader["MovementTypeCode"]),
                                    ConceptSource = new ConceptSource()
                                    {
                                        Id = Convert.ToInt32(reader["ConceptSourceCode"]),
                                    },
                                }
                            };

                            userBranchAccountingConcepts.Add(new UserBranchAccountingConcept
                            {
                                Id = userBranchAccountingConcept.UserBranchAccountingConceptId,
                                BranchAccountingConcept = branchAccountingConcept,
                                UserId = userBranchAccountingConcept.UserCode
                            });
                            break;
                        }
                    }
                }
                return userBranchAccountingConcepts;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetUserBranchAccountingConceptByCriteria
        /// </summary>
        /// <param name="branchAccountingConceptId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private BusinessCollection GetUserBranchAccountingConceptByCriteria(int branchAccountingConceptId, int userId)
        {
            try
            {
                // criteria
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.UserBranchAccountingConcept.Properties.BranchAccountingConceptId, branchAccountingConceptId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.UserBranchAccountingConcept.Properties.UserCode, userId);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                  typeof(GENERALLEDGEREN.UserBranchAccountingConcept), criteriaBuilder.GetPredicate()));

                return businessCollection;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }



        #endregion

    }
}
