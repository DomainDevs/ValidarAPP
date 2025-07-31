
//Sistran FWK
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Retentions;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class RetentionConceptPercentageDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        /// <summary>
        /// SaveRetentionConceptPercentage
        /// </summary>
        /// <param name="retentionConceptPercentage"></param>
        /// <returns>bool</returns>
        public bool SaveRetentionConceptPercentage(RetentionConceptPercentage retentionConceptPercentage)
        {
            bool isSaved = false;
            try
            {
                ACCOUNTINGEN.PerceivedRetentionValidity retentionConceptPercentageEntity = EntityAssembler.CreatePercentageRetentionConcept(retentionConceptPercentage);
                
                if (retentionConceptPercentage.Id == 0)
                {
                    _dataFacadeManager.GetDataFacade().InsertObject(retentionConceptPercentageEntity);
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
        public List<RetentionConceptPercentage> GetRetentionConceptPercentages()
        {
            List<RetentionConceptPercentage> retentionConceptPercentages = new List<RetentionConceptPercentage>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.PerceivedRetentionValidity.Properties.PerceivedRetentionValidityId);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);

                BusinessCollection retentionConceptPercentagesCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.PerceivedRetentionValidity), criteriaBuilder.GetPredicate()));

                if (retentionConceptPercentagesCollection.Count > 0)
                {
                    // Return del model
                    retentionConceptPercentages = ModelAssembler.CreateRetentionConceptPercentages(retentionConceptPercentagesCollection);
                }                
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return retentionConceptPercentages;
        }
       
        /// <summary>
        /// UpdateRetentionConcept
        /// </summary>
        /// <param name="retentionConcept"></param>
        /// <returns>Range</returns>
        public bool UpdateRetentionConceptPercentage(RetentionConceptPercentage retentionConceptPercentage)
        {
            bool isUpdated = false;
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PerceivedRetentionValidity.CreatePrimaryKey(retentionConceptPercentage.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.PerceivedRetentionValidity retentionConceptPercentageEntity = (ACCOUNTINGEN.PerceivedRetentionValidity)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                retentionConceptPercentageEntity.PerceivedRetentionCode = retentionConceptPercentage.RetentionConcept.Id;
                retentionConceptPercentageEntity.RetentionPercentage = retentionConceptPercentage.Percentage;
                retentionConceptPercentageEntity.ValidityFrom = retentionConceptPercentage.DateFrom;
                retentionConceptPercentageEntity.ValidityTo = retentionConceptPercentage.DateTo;
                retentionConceptPercentageEntity.PerceivedRetentionCode2 = retentionConceptPercentage.ExternalCode;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(retentionConceptPercentageEntity);

                isUpdated = true;
            }
            catch (BusinessException)
            {
                isUpdated = false;
            }

            return isUpdated;
        }
    }
}
