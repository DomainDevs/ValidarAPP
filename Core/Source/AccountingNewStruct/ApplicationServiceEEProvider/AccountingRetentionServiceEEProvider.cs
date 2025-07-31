using System.Collections.Generic;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.AccountingServices.DTOs.Retentions;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.Assemblers;
using System.Linq;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public class AccountingRetentionServiceEEProvider : IAccountingRetentionService
    {

        #region Constants


        #endregion

        #region Instance Variables

        #region Interfaz

        /// <summary>
        /// Declaración del contexto y del dataFacadeManager
        /// </summary>
        internal static readonly IDataFacadeManager dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Interfaz

        #region DAOs

        readonly RetentionBaseDAO _retentionBaseDAO = new RetentionBaseDAO();
        readonly RetentionConceptDAO _retentionConceptDAO = new RetentionConceptDAO();
        readonly RetentionConceptPercentageDAO _retentionConceptPercentageDAO = new RetentionConceptPercentageDAO();

        #endregion DAOs

        #endregion Instance Variables

        #region Public Methods

        #region Retention

        #region BaseRetention

        /// <summary>
        /// SaveRetentionBase
        /// </summary>
        /// <param name="retentionBase"></param>
        /// <returns>bool</returns>
        public bool SaveRetentionBase(RetentionBaseDTO retentionBase)
        {
            try
            {
                return _retentionBaseDAO.SaveRetentionBase(retentionBase.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateRetentionBase
        /// </summary>
        /// <param name="retentionBase"></param>
        /// <returns>bool</returns>
        public bool UpdateRetentionBase(RetentionBaseDTO retentionBase)
        {
            try
            {
                return _retentionBaseDAO.UpdateRetentionBase(retentionBase.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteRetentionBase
        /// </summary>
        /// <param name="retentionBase"></param>
        /// <returns>bool</returns>
        public bool DeleteRetentionBase(RetentionBaseDTO retentionBase)
        {
            try
            {
                return _retentionBaseDAO.DeleteRetentionBase(retentionBase.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetRetentionBases
        /// Obtiene los conceptos bases de retenciones
        /// </summary>
        /// <returns>List<RetentionBase/></returns>
        public List<RetentionBaseDTO> GetRetentionBases()
        {
            try
            {
                return _retentionBaseDAO.GetRetentionBases().ToDTOs().ToList();

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        #endregion BaseRetention

        #region RetentionConcept

        /// <summary>
        /// SaveRetentionConcept
        /// </summary>
        /// <param name="retentionConcept"></param>
        /// <returns>bool</returns>
        public bool SaveRetentionConcept(RetentionConceptDTO retentionConcept)
        {
            try
            {
                return _retentionConceptDAO.SaveRetentionConcept(retentionConcept.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateRetentionConcept
        /// <param name="retentionConcept"></param>
        /// </summary>
        /// <returns>bool</returns>
        public bool UpdateRetentionConcept(RetentionConceptDTO retentionConcept)
        {
            try
            {
                return _retentionConceptDAO.UpdateRetentionConcept(retentionConcept.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteRetentionConcept
        /// <param name="retentionConcept"></param>
        /// </summary>
        /// <returns>bool</returns>
        public bool DeleteRetentionConcept(RetentionConceptDTO retentionConcept)
        {
            try
            {

                return _retentionConceptDAO.DeleteRetentionConcept(retentionConcept.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetRetentionConcepts
        /// </summary>
        /// <returns>List<RetentionConcept/></returns>
        public List<RetentionConceptDTO> GetRetentionConcepts()
        {
            try
            {
                return _retentionConceptDAO.GetRetentionConcepts().ToDTOs().ToList();

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion RetentionConcept

        #region RetentionConceptPercentage

        /// <summary>
        /// SaveRetentionConceptPercentage
        /// <param name="retentionConceptPercentage"></param>
        /// </summary>
        /// <returns>bool</returns>
        public bool SaveRetentionConceptPercentage(RetentionConceptPercentageDTO retentionConceptPercentage)
        {
            try
            {
                return _retentionConceptPercentageDAO.SaveRetentionConceptPercentage(retentionConceptPercentage.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateRetentionConceptPercentage
        /// <param name="retentionConceptPercentage"></param>
        /// </summary>
        /// <returns>bool</returns>
        public bool UpdateRetentionConceptPercentage(RetentionConceptPercentageDTO retentionConceptPercentage)
        {
            try
            {
                return _retentionConceptPercentageDAO.UpdateRetentionConceptPercentage(retentionConceptPercentage.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetRetentionConceptPercentages
        /// </summary>
        /// <param name="retentionConcept"></param>
        /// <returns>List<RetentionConceptPercentage/></returns>
        public List<RetentionConceptPercentageDTO> GetRetentionConceptPercentages(RetentionConceptDTO retentionConcept)
        {
            try
            {
                return _retentionConceptPercentageDAO.GetRetentionConceptPercentages().ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion RetentionConceptPercentage

        #endregion Retention

        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods

    }
}
