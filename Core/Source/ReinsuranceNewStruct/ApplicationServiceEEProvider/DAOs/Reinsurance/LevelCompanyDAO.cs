using System;
//Sistran FWK
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Linq;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{

    internal class LevelCompanyDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveLevelCompany
        /// </summary>
        /// <param name="levelCompany"></param>
        public int SaveLevelCompany(LevelCompany levelCompany)
        {
            // Convertir de model a entity
            REINSEN.LevelCompany entityLevelCompany = EntityAssembler.CreateLevelCompany(levelCompany);

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(entityLevelCompany);
            return entityLevelCompany.LevelCompanyId;
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateLevelCompany
        /// </summary>
        /// <param name="levelCompany"></param>
        public void UpdateLevelCompany(LevelCompany levelCompany)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.LevelCompany.CreatePrimaryKey(levelCompany.LevelCompanyId);

            // Encuentra el objeto en referencia a la llave primaria
            REINSEN.LevelCompany entityLevelCompany = (REINSEN.LevelCompany)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            entityLevelCompany.BrokerReinsuranceCompanyId = levelCompany.Agent.IndividualId;
            entityLevelCompany.CommissionPercentage = levelCompany.ComissionPercentage;
            entityLevelCompany.LevelCode = levelCompany.ContractLevel.ContractLevelId;
            entityLevelCompany.ParticipationPercentage = levelCompany.GivenPercentage;
            entityLevelCompany.ReinsuranceCompanyId = levelCompany.Company.IndividualId;
            entityLevelCompany.InterestReserveRelease = levelCompany.InterestReserveRelease;
            entityLevelCompany.ReservePremiumPercentage = levelCompany.ReservePremiumPercentage;
            entityLevelCompany.AdditionalCommission = levelCompany.AdditionalCommissionPercentage;
            entityLevelCompany.DragLoss = Convert.ToInt16(levelCompany.DragLossPercentage);
            entityLevelCompany.ReinsurerExpenditur = levelCompany.ReinsuranceExpensePercentage;
            entityLevelCompany.ProfitSharingPercentage = levelCompany.UtilitySharePercentage;
            entityLevelCompany.Presentation = Convert.ToInt16(levelCompany.PresentationInformationType);
            entityLevelCompany.BrokerCommission = levelCompany.IntermediaryCommission;
            entityLevelCompany.LossCommissionPercentage = levelCompany.ClaimCommissionPercentage;
            entityLevelCompany.DifferentialCommissionPercentage = levelCompany.DifferentialCommissionPercentage;

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityLevelCompany);
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteLevelCompany
        /// </summary>
        /// <param name="levelCompanyId"></param>
        public void DeleteLevelCompany(int levelCompanyId)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.LevelCompany.CreatePrimaryKey(levelCompanyId);
            // Realizar las operaciones con los entities utilizando DAF
            REINSEN.LevelCompany entityLevelCompany = (REINSEN.LevelCompany)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().DeleteObject(entityLevelCompany);
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetLevelCompanyByCompanyId
        /// </summary>
        /// <param name="levelCompanyId"></param>
        /// <returns>LevelCompany</returns>
        public LevelCompany GetLevelCompanyByCompanyId(int levelCompanyId)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.LevelCompany.CreatePrimaryKey(levelCompanyId);
            // Realizar las operaciones con los entities utilizando DAF
            REINSEN.LevelCompany entityLevelCompany = (REINSEN.LevelCompany)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
            // Retornar el model
            return ModelAssembler.CreateLevelCompany(entityLevelCompany);
        }
        #endregion Get

        /// <summary>
        /// ValidateContractIssueAllocation
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns>bool</returns>
        public bool ValidateContractIssueAllocation(int contractId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.Level.Properties.ContractCode, contractId);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
               typeof(REINSEN.Level), criteriaBuilder.GetPredicate()));

            foreach (REINSEN.Level level in businessCollection.OfType<REINSEN.Level>())
            {
                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(REINSEN.LevelCompany.Properties.LevelCode, level.LevelId);

                BusinessCollection companyBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                typeof(REINSEN.LevelCompany), criteriaBuilder.GetPredicate()));

                foreach (REINSEN.LevelCompany levelcompany in companyBusinessCollection.OfType<REINSEN.LevelCompany>())
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(REINSEN.IssueAllocation.Properties.ContractCompanyId, levelcompany.LevelCompanyId);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(REINSEN.IssueAllocation.Properties.IsFacultative, 0);

                    BusinessCollection issueBusinessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                              typeof(REINSEN.IssueAllocation), criteriaBuilder.GetPredicate()));

                    if (issueBusinessCollection.Count > 0)
                    {
                        return true;
                    }
                }

            }
            return false;

        }

        /// <summary>
        /// GetReinsuranceCompanyIdByLevelIdAndIndividualId
        /// </summary>
        /// <param name="levelId"></param>
        /// <param name="individualId"></param>
        /// <returns>int</returns>
        public int GetReinsuranceCompanyIdByLevelIdAndIndividualId(int levelId, int individualId)
        {
            int reinsuranceCompanyId = 0;

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.LevelCompany.Properties.LevelCode, levelId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(REINSEN.LevelCompany.Properties.ReinsuranceCompanyId, individualId);
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                typeof(REINSEN.LevelCompany), criteriaBuilder.GetPredicate()));

            foreach (REINSEN.LevelCompany levelCompanyEntity in businessCollection.OfType<REINSEN.LevelCompany>())
            {
                reinsuranceCompanyId = levelCompanyEntity.ReinsuranceCompanyId;
            }

            return reinsuranceCompanyId;
        }
    }
}