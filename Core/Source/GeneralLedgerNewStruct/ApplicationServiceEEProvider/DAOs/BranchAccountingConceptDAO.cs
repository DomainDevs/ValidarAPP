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

#endregion Using

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    public class BranchAccountingConceptDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        
        #endregion

        #region Save

        /// <summary>
        /// SaveBranchAccountingConcept
        /// </summary>
        /// <param name="branchAccountingConcept">Model</param>
        /// <returns>ModelAssembler</returns>
        public BranchAccountingConcept SaveBranchAccountingConcept(BranchAccountingConcept branchAccountingConcept)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.BranchAccountingConcept branchAccountingConceptTypeEntity = EntityAssembler.CreateBranchAccountingConcept(branchAccountingConcept);

                BusinessCollection branchAccountingConcepts = GetBranchAccountingConceptByCriteria(branchAccountingConcept.Branch.Id,
                                                                branchAccountingConcept.AccountingConcept.Id,
                                                                branchAccountingConcept.MovementType.Id,
                                                                branchAccountingConcept.MovementType.ConceptSource.Id);

                var branchAccountingConceptId = 0;

                if (branchAccountingConcepts.Count == 0)
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().InsertObject(branchAccountingConceptTypeEntity);
                }
                else
                {
                    foreach (GENERALLEDGEREN.BranchAccountingConcept EntitiesFind in branchAccountingConcepts.OfType<GENERALLEDGEREN.BranchAccountingConcept>())
                    {
                        branchAccountingConceptId = Convert.ToInt32(EntitiesFind.BranchAccountingConceptId);
                    }
                    branchAccountingConceptTypeEntity.BranchAccountingConceptId = branchAccountingConceptId;
                }
                // Return del model
                return ModelAssembler.CreateBranchAccountingConcept(branchAccountingConceptTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        #endregion

        #region Update

        /// <summary>
        /// UpdateBranchAccountingConcept
        /// </summary>
        /// <param name="branchAccountingConcept">Model</param>
        /// <returns>ModelAssembler</returns>
        public BranchAccountingConcept UpdateBranchAccountingConcept(BranchAccountingConcept branchAccountingConcept)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.BranchAccountingConcept.CreatePrimaryKey(branchAccountingConcept.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.BranchAccountingConcept branchAccountingConceptEntity = (GENERALLEDGEREN.BranchAccountingConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                branchAccountingConceptEntity.BranchCode = branchAccountingConcept.Branch.Id;
                branchAccountingConceptEntity.AccountingConceptCode = branchAccountingConcept.AccountingConcept.Id;
                branchAccountingConceptEntity.MovementTypeCode = branchAccountingConcept.MovementType.Id;
                branchAccountingConceptEntity.ConceptSourceCode = branchAccountingConcept.MovementType.ConceptSource.Id;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(branchAccountingConceptEntity);

                // Return del model
                return ModelAssembler.CreateBranchAccountingConcept(branchAccountingConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteBranchAccountingConcept
        /// </summary>
        /// <param name="branchAccountingConcept">Model</param>
        /// <returns>bool</returns>
        public bool DeleteBranchAccountingConcept(BranchAccountingConcept branchAccountingConcept)
        {
            bool isDeleted = false;

            try
            {   // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.BranchAccountingConcept.CreatePrimaryKey(branchAccountingConcept.Id);

                BusinessCollection userBranchAccountingConcepts = GetCollectionUserBranchAccountingConcept(branchAccountingConcept.Id);

                if (userBranchAccountingConcepts.Count == 0)
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    GENERALLEDGEREN.BranchAccountingConcept branchAccountingConceptEntity = (GENERALLEDGEREN.BranchAccountingConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(branchAccountingConceptEntity);
                    isDeleted = true;
                }
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
        /// GetBranchAccountingConcept
        /// </summary>
        /// <param name="branchAccountingConcept">Model</param>
        /// <returns>ModelAssembler</returns>
        public BranchAccountingConcept GetBranchAccountingConcept(BranchAccountingConcept branchAccountingConcept)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.BranchAccountingConcept.CreatePrimaryKey(branchAccountingConcept.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.BranchAccountingConcept branchAccountingConceptEntity = (GENERALLEDGEREN.BranchAccountingConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateBranchAccountingConcept(branchAccountingConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBranchAccountingConcepts
        /// </summary>
        /// <returns>List BranchAccountingConcept</returns>
        public List<BranchAccountingConcept> GetBranchAccountingConcepts()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.BranchAccountingConcept)));

                // Return como Lista
                return ModelAssembler.CreateBranchAccountingConcepts(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBranchAccountingConceptByBranch
        /// </summary>
        /// <param name="branch">Model</param>
        /// <returns>List BranchAccountingConcept</returns>
        public List<BranchAccountingConcept> GetBranchAccountingConceptByBranch(Branch branch)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.BranchAccountingConcept.Properties.BranchCode, branch.Id);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                   typeof(GENERALLEDGEREN.BranchAccountingConcept), criteriaBuilder.GetPredicate()));

                // Return como Lista
                return ModelAssembler.CreateBranchAccountingConcepts(businessCollection);

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            
        }

        /// <summary>
        /// GetBranchAccountingConceptByCriteria
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="accountingConceptId"></param>
        /// <param name="movementTypeId"></param>
        /// <param name="conceptSourceId"></param>
        /// <returns></returns>
        private BusinessCollection GetBranchAccountingConceptByCriteria(int branchId, int accountingConceptId, 
                                                                                  int movementTypeId, int conceptSourceId)
        {
            try
            {
                // criteria
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.BranchAccountingConcept.Properties.BranchCode, branchId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.BranchAccountingConcept.Properties.AccountingConceptCode, accountingConceptId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.BranchAccountingConcept.Properties.MovementTypeCode, movementTypeId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.BranchAccountingConcept.Properties.ConceptSourceCode, conceptSourceId);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                   typeof(GENERALLEDGEREN.BranchAccountingConcept), criteriaBuilder.GetPredicate()));
               
                return businessCollection;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCollectionUserBranchAccountingConcept
        /// </summary>
        /// <param name="branchAccountingConceptId"></param>
        /// <returns></returns>
        private BusinessCollection GetCollectionUserBranchAccountingConcept(int branchAccountingConceptId)
        {
            try
            {
                // criteria
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.UserBranchAccountingConcept.Properties.BranchAccountingConceptId, branchAccountingConceptId);

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
