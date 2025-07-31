using System;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Framework.Queries;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class ApplicationDAO
    {
        /// <summary>
        /// SaveImputation
        /// </summary>
        /// <param name="imputation"></param>
        /// <param name="sourceCode"></param>
        /// <param name="TechnicalTransaction"></param>
        /// <returns>Imputation</returns>
        public Models.Imputations.Application SaveImputation(Models.Imputations.Application imputation, int sourceCode, int TechnicalTransaction)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.Application entityApplication = EntityAssembler.CreateApplication(imputation, sourceCode, TechnicalTransaction);

                // Realizar las operaciones con los entities utilizando DAF
                DataFacadeManager.Insert(entityApplication);

                // Return del model
                return ModelAssembler.CreateApplication(entityApplication);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetImputation
        /// </summary>
        /// <param name="application"></param>
        /// <returns>CollectImputation</returns>
        public CollectApplication GetApplication(Models.Imputations.Application application)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Application.CreatePrimaryKey(application.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.Application entityApplication = (ACCOUNTINGEN.Application)DataFacadeManager.GetObject(primaryKey);

                // Return del model
                return ModelAssembler.CreateCollectApplication(entityApplication);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetImputation
        /// </summary>
        /// <param name="application"></param>
        /// <returns>CollectImputation</returns>
        public CollectApplication GetApplicationByTechnicalTransaction(int technicalTransaction)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.Application.Properties.TechnicalTransaction);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(technicalTransaction);
                // Realizar las operaciones con los entities utilizando DAF
                List<ACCOUNTINGEN.Application> entityApplications = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.Application), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.Application>().ToList();

                if (entityApplications != null && entityApplications.Count > 0)
                {
                    // Return del model
                    return ModelAssembler.CreateCollectApplication(entityApplications.FirstOrDefault());
                }

                // No se encontró la aplicación
                return new CollectApplication()
                {
                    Application = new Models.Imputations.Application()
                };
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// GetImputation
        /// </summary>
        /// <param name="application"></param>
        /// <returns>CollectImputation</returns>
        public CollectApplication GetApplicationBySourceId(int sourceId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.Application.Properties.SourceCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(sourceId);
                // Realizar las operaciones con los entities utilizando DAF
                List<ACCOUNTINGEN.Application> entityApplications = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.Application), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.Application>().ToList();

                if (entityApplications != null && entityApplications.Count > 0)
                {
                    // Return del model
                    return ModelAssembler.CreateCollectApplication(entityApplications.FirstOrDefault());
                }

                // No se encontró la aplicación
                return new CollectApplication()
                {
                    Application = new Models.Imputations.Application()
                };
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetApplicationIdByTechnicalTransaction
        /// </summary>
        /// <param name="technicalTransaction"></param>
        /// <returns>id</returns>
        public int GetApplicationIdByTechnicalTransaction(int technicalTransaction)
        {
            int applicationId = 0;
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.Application.Properties.TechnicalTransaction);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(technicalTransaction);

                List<ACCOUNTINGEN.Application> entityApplications = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.Application), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.Application>().ToList();

                if (entityApplications != null && entityApplications.Count > 0)
                {
                    applicationId = Convert.ToInt32(entityApplications.First().ApplicationCode);
                }
                return applicationId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateImputation
        /// </summary>
        /// <param name="collectApplication"></param>
        /// <returns>CollectImputation</returns>
        public CollectApplication UpdateImputation(CollectApplication collectApplication)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Application.CreatePrimaryKey(collectApplication.Application.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.Application entityApplication = (ACCOUNTINGEN.Application)
                    (DataFacadeManager.GetObject(primaryKey));

                entityApplication.ModuleCode = collectApplication.Application.ModuleId;
                entityApplication.SourceCode = collectApplication.Collect.Id;
                entityApplication.RegisterDate = collectApplication.Application.RegisterDate;
                entityApplication.UserCode = collectApplication.Application.UserId;
                entityApplication.TechnicalTransaction = collectApplication.Transaction.TechnicalTransaction;

                // Realizar las operaciones con los entities utilizando DAF
                DataFacadeManager.Update(entityApplication);

                // Return del model
                return ModelAssembler.CreateCollectApplication(entityApplication);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetImputationBySourceType
        /// Obtiene el Id de imputaciòn dado el source y el tipo
        /// </summary>
        /// <param name="source"></param>
        /// <param name="type"></param>
        /// <returns>int</returns>
        public int GetImputationBySourceType(int source, int type)
        {
            try
            {
                int applicationId = 0;

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.SourceCode, source);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.ModuleCode, type);

                List<ACCOUNTINGEN.Application> entityApplications = DataFacadeManager.GetObjects(
                               typeof(ACCOUNTINGEN.Application), criteriaBuilder.GetPredicate()).
                               Cast<ACCOUNTINGEN.Application>().ToList();

                if (entityApplications != null && entityApplications.Count > 0)
                {
                    applicationId = Convert.ToInt32(entityApplications.First().ApplicationCode);
                }

                return applicationId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetImputationBySourceType
        /// Obtiene el Id de imputaciòn dado el source y el tipo
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="sourceId"></param>
        /// <returns>int</returns>
        public Models.Imputations.Application GetApplicationByModuleIdSourceId(int moduleId, int sourceId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.ModuleCode, moduleId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.SourceCode, sourceId);

                List<ACCOUNTINGEN.Application> entityApplications = DataFacadeManager.GetObjects(
                               typeof(ACCOUNTINGEN.Application), criteriaBuilder.GetPredicate()).
                               Cast<ACCOUNTINGEN.Application>().ToList();

                if (entityApplications != null && entityApplications.Count > 0)
                {
                    return ModelAssembler.CreateApplication(entityApplications.First());
                }
                return null;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        
        public Models.Imputations.Application SaveApplication(Models.Imputations.Application application)
        {
            try
            {
                ACCOUNTINGEN.Application entityApplication = EntityAssembler.CreateApplication(application);
                // Realizar las operaciones con los entities utilizando DAF
                DataFacadeManager.Insert(entityApplication);

                // Return del model
                return ModelAssembler.CreateApplication(entityApplication);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public bool SaveLogMassiveDataPolicy(Models.Imputations.LogMassiveDataPolicy logMassiveDataPolicyDTO)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.LogMassiveDataPolicy entityApplication = EntityAssembler.LogMassiveDataPolicy(logMassiveDataPolicyDTO);

                // Realizar las operaciones con los entities utilizando DAF
                DataFacadeManager.Insert(entityApplication);

                // Return del model
                return true;
            }
            catch (BusinessException ex)
            {
                return false;
            }
        }
    }
}
