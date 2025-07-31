#region Using

//Sistran Core
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
    public class MovementTypeDAO
    {

        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveMovementType
        /// </summary>
        /// <param name="movementType"></param>
        /// <returns></returns>
        public MovementType SaveMovementType(MovementType movementType)
        {
            try
            {
                // recuperar todos los registros
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.MovementType)));
                
                var movementTypeId = 0;
                if (businessCollection.Count > 0)
                {
                    var maxNumber = (from GENERALLEDGEREN.MovementType movementTypeEntityFind in businessCollection select movementTypeEntityFind.MovementTypeCode).Max();
                    movementTypeId = Convert.ToInt32(maxNumber);
                    movementTypeId = movementTypeId + 1;
                }
                else
                {
                    movementTypeId = 1;
                }
                movementType.Id = movementTypeId;

                // Convertir de model a entity
                GENERALLEDGEREN.MovementType movementTypeEntity = EntityAssembler.CreateMovementType(movementType);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(movementTypeEntity);

                // Return del model
                return ModelAssembler.CreateMovementType(movementTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        #endregion

        #region Update

        /// <summary>
        /// UpdateMovementType
        /// </summary>
        /// <param name="movementType"></param>
        /// <returns></returns>
        public MovementType UpdateMovementType(MovementType movementType)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.MovementType.CreatePrimaryKey(movementType.Id, movementType.ConceptSource.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.MovementType movementTypeEntity = (GENERALLEDGEREN.MovementType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                movementTypeEntity.Description = movementType.Description;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(movementTypeEntity);

                // Return del model
                return ModelAssembler.CreateMovementType(movementTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        #endregion

        #region Delete

        /// <summary>
        /// DeleteMovementType
        /// </summary>
        /// <param name="movementType"></param>
        /// <returns></returns>
        public bool DeleteMovementType(MovementType movementType)
        {
            bool isDeleted = false;

            try
            {   // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.MovementType.CreatePrimaryKey(movementType.Id, movementType.ConceptSource.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.MovementType movementTypeEntity = (GENERALLEDGEREN.MovementType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(movementTypeEntity);

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
        /// GetMovementType
        /// </summary>
        /// <param name="movementType"></param>
        /// <returns></returns>
        public MovementType GetMovementType(MovementType movementType)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.MovementType.CreatePrimaryKey(movementType.Id, movementType.ConceptSource.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.MovementType movementTypeEntity = (GENERALLEDGEREN.MovementType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateMovementType(movementTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetMovementTypes
        /// </summary>
        /// <returns></returns>
        public List<MovementType> GetMovementTypes()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.MovementType)));

                // Return como Lista
                return ModelAssembler.CreateMovementTypes(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// GetMovementTypesByConceptSource
        /// </summary>
        /// <param name="conceptSource">Modelo conceptSource</param>
        /// <returns>List MovementType</returns>
        public List<MovementType> GetMovementTypesByConceptSource(ConceptSource conceptSource)
        {
            try
            {

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.MovementType.Properties.ConceptSourceCode, conceptSource.Id);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                   typeof(GENERALLEDGEREN.MovementType), criteriaBuilder.GetPredicate()));

                // Return como Lista
                return ModelAssembler.CreateMovementTypes(businessCollection);

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// GetMovementTypesByConceptSource
        /// </summary>
        /// <param name="conceptSource">Modelo conceptSource</param>
        /// <returns>List MovementType</returns>
        public List<MovementType> GetMovementTypesByConceptSourceId(int conceptSourceId)
        {
            try
            {

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.MovementType.Properties.ConceptSourceCode, conceptSourceId);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                   typeof(GENERALLEDGEREN.MovementType), criteriaBuilder.GetPredicate()));

                // Return como Lista
                return ModelAssembler.CreateMovementTypes(businessCollection);

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// GetMovementTypesByConceptSourceFilter
        /// </summary>
        /// <param name="conceptSource">Modelo conceptSource</param>
        /// <returns>List MovementType</returns>
        public List<MovementType> GetMovementTypesByConceptSourceFilter(ConceptSource conceptSource)
        {
            try
            {

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.MovementType.Properties.ConceptSourceCode, conceptSource.Id);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                   typeof(GENERALLEDGEREN.MovementType), criteriaBuilder.GetPredicate()));

                // Return como Lista
                return ModelAssembler.CreateMovementTypesFilter(businessCollection);

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }


        #endregion
    }
}
