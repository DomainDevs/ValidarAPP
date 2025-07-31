using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    public class TempApplicationDAO
    {
        #region Instance Variables
        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance Variables

        #region TempImputationDAO

        /// <summary>
        /// SaveTempImputation
        /// </summary>
        /// <param name="application"></param>
        /// <param name="sourceCode"></param>
        /// <returns>Imputation</returns>
        public Models.Imputations.Application SaveTempApplication(Models.Imputations.Application application, int sourceCode)
        {
            try
            {
                ACCOUNTINGEN.TempApplication entityApplication = EntityAssembler.CreateTempApplication(application, sourceCode);
                DataFacadeManager.Insert(entityApplication);
                return ModelAssembler.CreateApplicationByTempApplication(entityApplication);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateTempImputation
        /// </summary>
        /// <param name="application"></param>
        /// <param name="sourceCode"></param>
        /// <returns>Imputation</returns>
        public Models.Imputations.Application UpdateTempApplication(Models.Imputations.Application application, int sourceCode)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplication.CreatePrimaryKey(application.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempApplication entityApplication = (ACCOUNTINGEN.TempApplication)
                    DataFacadeManager.GetObject(primaryKey);

                entityApplication.ModuleCode = application.ModuleId;
                entityApplication.RegisterDate = application.RegisterDate;
                entityApplication.UserCode = application.UserId;
                entityApplication.SourceCode = sourceCode;

                // Realizar las operaciones con los entities utilizando DAF
                DataFacadeManager.Update(entityApplication);

                // Return del model
                return ModelAssembler.CreateApplicationByTempApplication(entityApplication);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateTempImputation
        /// </summary>
        /// <param name="application"></param>
        /// <param name="sourceCode"></param>
        /// <returns>Imputation</returns>
        public Models.Imputations.Application UpdateSourceCodeTempApplication(int applicationId, int sourceCode)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplication.CreatePrimaryKey(applicationId);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempApplication entityApplication = (ACCOUNTINGEN.TempApplication)
                    DataFacadeManager.GetObject(primaryKey);

                entityApplication.SourceCode = sourceCode;

                // Realizar las operaciones con los entities utilizando DAF
                DataFacadeManager.Update(entityApplication);

                // Return del model
                return ModelAssembler.CreateApplicationByTempApplication(entityApplication);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempImputation
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempApplication(int applicationId)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplication.CreatePrimaryKey(applicationId);
                DataFacadeManager.Delete(primaryKey);
                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempImputation by Id
        /// </summary>
        /// <param name="tempApplicaton"></param>
        /// <returns>Imputation</returns>
        public Models.Imputations.Application GetTempApplication(Models.Imputations.Application tempApplicaton)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplication.CreatePrimaryKey(tempApplicaton.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempApplication entityTempApplication = (ACCOUNTINGEN.TempApplication)
                    DataFacadeManager.GetObject(primaryKey);

                if (entityTempApplication != null)
                {
                    // Return del model
                    return ModelAssembler.CreateApplicationByTempApplication(entityTempApplication);
                }

                tempApplicaton.Id = 0;
                return tempApplicaton;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempImputation by Id
        /// </summary>
        /// <param name="tempApplicaton"></param>
        /// <returns>Imputation</returns>
        public Models.Imputations.Application GetTempApplicationByTempApplicationId(int tempApplicatonId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplication.CreatePrimaryKey(tempApplicatonId);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempApplication entityTempApplication = (ACCOUNTINGEN.TempApplication)
                    DataFacadeManager.GetObject(primaryKey);

                if (entityTempApplication != null)
                {
                    // Return del model
                    return ModelAssembler.CreateApplicationByTempApplication(entityTempApplication);
                }
                return null;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempImputation
        /// </summary>
        /// <param name="tempApplicaton"></param>
        /// <returns>Imputation</returns>
        public List<Models.Imputations.TempApplication> GetTempApplicationByUserId(int userId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.TempApplication.Properties.UserCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(userId);
                
                string sortExp = null;
                sortExp = "-" + ACCOUNTINGEN.TempApplication.Properties.TempAppCode;

                string[] sortExps = null;
                if ((sortExp != null))
                {
                    sortExps = sortExp.Split(' ');
                }

                BusinessCollection businessObjects = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.TempApplication), criteriaBuilder.GetPredicate(), sortExps));

                //Se retorna como una lista.
                return ModelAssembler.CreateTempApplications(businessObjects);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetTempImputation by Id
        /// </summary>
        /// <param name="tempApplicaton"></param>
        /// <returns>Imputation</returns>
        public bool UpdateTempApplicationIndividualId(int tempApplicationId, int individualId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplication.CreatePrimaryKey(tempApplicationId);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempApplication entityTempApplication = (ACCOUNTINGEN.TempApplication)
                    DataFacadeManager.GetObject(primaryKey);

                if (entityTempApplication != null)
                {
                    entityTempApplication.IndividualCode = individualId;
                    DataFacadeManager.Update(entityTempApplication);
                    return true;
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            return false;
        }
        #endregion
    }
}
