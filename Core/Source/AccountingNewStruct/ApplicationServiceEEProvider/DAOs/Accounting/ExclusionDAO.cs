using System.Collections.Generic;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.CancellationPolicies;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class ExclusionDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveExclusion
        /// </summary>
        /// <param name="exclusion"></param>
        /// <returns>Exclusion</returns>
        public Exclusion SaveExclusion(Exclusion exclusion)
        {
            try
            {
                ACCOUNTINGEN.Exclusion exclusionEntity = EntityAssembler.CreateExclusion(exclusion);

                _dataFacadeManager.GetDataFacade().InsertObject(exclusionEntity);

                return ModelAssembler.GetExclusion(exclusionEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteExclusion
        /// </summary>
        /// <param name="exclusion"></param>
        public void DeleteExclusion(Exclusion exclusion)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.Exclusion.CreatePrimaryKey(exclusion.Id);

                ACCOUNTINGEN.Exclusion actionConcept = (ACCOUNTINGEN.Exclusion)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                _dataFacadeManager.GetDataFacade().DeleteObject(actionConcept);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetExclusions
        /// </summary>
        /// <param name="exclusionType"></param>
        /// <returns>List<Exclusion/></returns>
        public List<Exclusion> GetExclusions(ExclusionTypes exclusionType) //ya no hay enum
        {
            try
            {
                int otherExclusionType = exclusionType == ExclusionTypes.Agent ? 2 : 3;
                int exclusionTypeNumber = exclusionType == ExclusionTypes.Policy ? 1 : otherExclusionType;

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetExclusion.Properties.ExclusionType, exclusionTypeNumber);

                BusinessCollection collections = new BusinessCollection(
                _dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.GetExclusion),
                criteriaBuilder.GetPredicate()));

                return ModelAssembler.CreateExclusions(collections, exclusionTypeNumber);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
