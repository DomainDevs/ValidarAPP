using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Retentions;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class RetentionConceptDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        /// <summary>
        /// SaveRetentionConcept
        /// </summary>
        /// <param name="retentionConcept"></param>
        /// <returns>bool</returns>
        public bool SaveRetentionConcept(RetentionConcept retentionConcept)
        {
            bool isSaved = false;
            try
            {
                //DTO->MODEL
                var retentionConceptEntity = EntityAssembler.CreateRetentionConcept(retentionConcept);
                if (retentionConcept.Id == 0)
                {
                    _dataFacadeManager.GetDataFacade().InsertObject(retentionConceptEntity);
                }

                isSaved = true;
            }
            catch (BusinessException)
            {
                isSaved = false;
            }

            return isSaved;
        }

        /// <summary>
        /// GetRetentionConcepts
        /// </summary>
        /// <returns>List<RetentionConcept/></returns>
        public List<RetentionConcept> GetRetentionConcepts()
        {
            List<RetentionConcept> retentionConcepts = new List<RetentionConcept>();
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.PerceivedRetention.Properties.PerceivedRetentionId);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);

                BusinessCollection retentionConceptsCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.PerceivedRetention), criteriaBuilder.GetPredicate()));

                if (retentionConceptsCollection.Count > 0)
                {
                    // Return del model
                    retentionConcepts = ModelAssembler.CreateRetentionConcepts(retentionConceptsCollection);
                }

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return retentionConcepts;
        }

        /// <summary>
        /// GetRetentionConcept
        /// </summary>
        /// <param name="retentionConcept"></param>
        /// <returns>RetentionConcept</returns>
        public RetentionConcept GetRetentionConcept(RetentionConcept retentionConcept)
        {
            RetentionConcept newRetentionConcept = new RetentionConcept();
            List<RetentionConceptPercentage> retentionConceptPercentages = new List<RetentionConceptPercentage>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.PerceivedRetentionValidity.Properties.PerceivedRetentionCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(retentionConcept.Id);

                newRetentionConcept.Id = retentionConcept.Id;

                BusinessCollection collections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.PerceivedRetentionValidity), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.PerceivedRetentionValidity retentionConceptEntity in collections.OfType<ACCOUNTINGEN.PerceivedRetentionValidity>())
                {
                    retentionConceptPercentages.Add(new RetentionConceptPercentage
                    {
                        Id = retentionConceptEntity.PerceivedRetentionValidityId,
                        RetentionConcept = new RetentionConcept() { Id = retentionConceptEntity.PerceivedRetentionCode },
                        Percentage = retentionConceptEntity.RetentionPercentage,
                        DateFrom = retentionConceptEntity.ValidityFrom,
                        DateTo = retentionConceptEntity.ValidityTo

                    });
                }

                return newRetentionConcept;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateRetentionConcept
        /// </summary>
        /// <param name="retentionConcept"></param>
        /// <returns>Range</returns>
        public bool UpdateRetentionConcept(RetentionConcept retentionConcept)
        {
            bool isUpdated = false;
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PerceivedRetention.CreatePrimaryKey(retentionConcept.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.PerceivedRetention retentionConceptEntity = (ACCOUNTINGEN.PerceivedRetention)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                retentionConceptEntity.Description = retentionConcept.Description;
                retentionConceptEntity.RetentionBaseCode = retentionConcept.RetentionBase.Id;
                retentionConceptEntity.MaximumDifferenceTax = retentionConcept.DifferenceAmount;
                retentionConceptEntity.IsActive = Convert.ToBoolean(retentionConcept.Status == 0 ? 0 : 1);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(retentionConceptEntity);

                isUpdated = true;
            }
            catch (BusinessException)
            {
                isUpdated = false;
            }

            return isUpdated;
        }

        /// <summary>
        /// DeleteRetentionConcept
        /// </summary>
        /// <param name="retentionConceptId"></param>
        /// <returns>bool</returns>
        public bool DeleteRetentionConcept(RetentionConcept retentionConcept)
        {
            bool isDeleted = false;

            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.PerceivedRetention.CreatePrimaryKey(retentionConcept.Id);
                ACCOUNTINGEN.PerceivedRetention retentionConceptDeleteEntity = (ACCOUNTINGEN.PerceivedRetention)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                if (retentionConceptDeleteEntity != null)
                {
                    _dataFacadeManager.GetDataFacade().DeleteObject(retentionConceptDeleteEntity);
                }
                isDeleted = true;
            }
            catch (BusinessException)
            {
                isDeleted = false;
            }

            return isDeleted;
        }

    }


}
