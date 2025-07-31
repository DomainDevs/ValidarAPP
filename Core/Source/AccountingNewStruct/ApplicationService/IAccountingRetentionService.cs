using Sistran.Core.Application.AccountingServices.DTOs.Retentions;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.AccountingServices
{
    [ServiceContract]
    public interface IAccountingRetentionService
    {
        #region RetentionBase

        /// <summary>
        /// SaveRetentionBase
        /// <param name="retentionBase"></param>
        /// </summary>            
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveRetentionBase(RetentionBaseDTO retentionBase);

        /// <summary>
        /// UpdateRetentionBase
        /// <param name="retentionBase"></param>
        /// </summary>            
        /// <returns>bool</returns>
        [OperationContract]
        bool UpdateRetentionBase(RetentionBaseDTO retentionBase);

        /// <summary>
        /// DeleteRetentionBase        
        /// <param name="retentionBase"></param>
        /// </summary>            
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteRetentionBase(RetentionBaseDTO retentionBase);

        /// <summary>
        /// GetRetentionBases
        /// </summary>            
        /// <returns>List<RetentionBase></returns>
        [OperationContract]
        List<RetentionBaseDTO> GetRetentionBases();


        #endregion

        #region RetentionConcept

        /// <summary>
        /// SaveRetentionConcept
        /// <param name="retentionConcept"></param>
        /// </summary>            
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveRetentionConcept(RetentionConceptDTO retentionConcept);

        /// <summary>
        /// UpdateRetentionConcept
        /// <param name="retentionConcept"></param>
        /// </summary>            
        /// <returns>bool</returns>
        [OperationContract]
        bool UpdateRetentionConcept(RetentionConceptDTO retentionConcept);

        /// <summary>
        /// DeleteRetentionConcept        
        /// <param name="retentionConcept"></param>
        /// </summary>            
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteRetentionConcept(RetentionConceptDTO retentionConcept);

        /// <summary>
        /// GetRetentionConcepts
        /// </summary>            
        /// <returns>List<RetentionConcept></returns>
        [OperationContract]
        List<RetentionConceptDTO> GetRetentionConcepts();


        #endregion

        #region RetentionConceptPercentage

        /// <summary>
        /// SaveRetentionConceptPercentage
        /// <param name="retentionConceptPercentage"></param>
        /// </summary>            
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveRetentionConceptPercentage(RetentionConceptPercentageDTO retentionConceptPercentage);

        /// <summary>
        /// UpdateRetentionConceptPercentage
        /// <param name="retentionConceptPercentage"></param>
        /// </summary>            
        /// <returns>bool</returns>
        [OperationContract]
        bool UpdateRetentionConceptPercentage(RetentionConceptPercentageDTO retentionConceptPercentage);

        /// <summary>
        /// GetRetentionConceptPercentages
        /// <param name="retentionConcept"></param>
        /// </summary>            
        /// <returns>List<RetentionConceptPercentage></returns>
        [OperationContract]
        List<RetentionConceptPercentageDTO> GetRetentionConceptPercentages(RetentionConceptDTO retentionConcept);


        #endregion
    }
}
