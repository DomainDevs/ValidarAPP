using System;
//Sistran FWK
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using REINSURANCEEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using Sistran.Core.Framework.Queries;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;
using System.Linq;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{

    internal class LevelDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region SaveB

        /// <summary>
        /// SaveLevel
        /// </summary>
        /// <param name="level"></param>
        public int SaveLevel(Level level)
        {
            // Convertir de model a entity
            REINSURANCEEN.Level entityLevel = EntityAssembler.CreateLevel(level);
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(entityLevel);
            return entityLevel.LevelId;
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateLevel
        /// </summary>
        /// <param name="level"></param>
        public void UpdateLevel(Level level)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.Level.CreatePrimaryKey(level.ContractLevelId);
            // Encuentra el objeto en referencia a la llave primaria
            REINSURANCEEN.Level entityLevel = (REINSURANCEEN.Level)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            entityLevel.AssignmentPercentage = level.AssignmentPercentage;
            entityLevel.ContractCode = level.Contract.ContractId;
            entityLevel.LevelLimit = Convert.ToDecimal(level.ContractLimit);
            entityLevel.EventLimit = level.EventLimit;
            entityLevel.LevelNumber = level.Number;
            entityLevel.LinesNumber = level.LinesNumber;
            entityLevel.RetentionLimit = level.RetentionLimit;

            entityLevel.AdjustmentPercentage = level.AdjustmentPercentage;
            entityLevel.FixedRatePercentage = level.FixedRatePercentage;
            entityLevel.MinimumRatePercentage = level.MinimumRatePercentage;
            entityLevel.MaximumRatePercentage = level.MaximumRatePercentage;
            entityLevel.LifeRate = level.LifeRate;
            entityLevel.CalculationType = Convert.ToInt32(level.CalculationType);
            entityLevel.ApplyOn = Convert.ToInt32(level.ApplyOnType);
            entityLevel.AnnualAddedLimit = level.AnnualAddedLimit;
            entityLevel.PremiumType = Convert.ToInt32(level.PremiumType);


            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityLevel);
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteLevel
        /// </summary>
        /// <param name="levelId"></param>
        public void DeleteLevel(int levelId)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.Level.CreatePrimaryKey(levelId);

            // Realizar las operaciones con los entities utilizando DAF
            REINSURANCEEN.Level entityLevel = (REINSURANCEEN.Level)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().DeleteObject(entityLevel);

        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetLevelsByLevelId
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns>Level</returns>
        public Level GetLevelsByLevelId(int levelId)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.Level.CreatePrimaryKey(levelId);

            // Realizar las operaciones con los entities utilizando DAF
            REINSURANCEEN.Level entityLevel = (REINSURANCEEN.Level)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            // Retornar el model
            return ModelAssembler.CreateLevel(entityLevel);
        }

        /// <summary>
        /// GetLayersByContractId
        /// </summary>
        /// <param name = "contractId" ></ param >
        /// < returns > List < Level /></ returns >
        public List<Level> GetLevelsByContractId(int contractId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.Level.Properties.ContractCode, contractId);
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(REINSEN.Level),
                criteriaBuilder.GetPredicate()));
            return (from REINSEN.Level contractLevelEntity in businessCollection
                    select new Level
                    {
                        AssignmentPercentage = Convert.ToDecimal(contractLevelEntity.AssignmentPercentage),
                        Contract = new Contract { ContractId = contractLevelEntity.ContractCode },
                        ContractLevelId = contractLevelEntity.LevelId,
                        ContractLimit = contractLevelEntity.LevelLimit,
                        EventLimit = contractLevelEntity.EventLimit,
                        Number = contractLevelEntity.LevelNumber,
                        LinesNumber = contractLevelEntity.LinesNumber,
                        RetentionLimit = contractLevelEntity.RetentionLimit,

                        AdjustmentPercentage = contractLevelEntity.AdjustmentPercentage == null ? 0 : Convert.ToDecimal(contractLevelEntity.AdjustmentPercentage),
                        FixedRatePercentage = contractLevelEntity.FixedRatePercentage == null ? 0 : Convert.ToDecimal(contractLevelEntity.FixedRatePercentage),
                        MinimumRatePercentage = contractLevelEntity.MinimumRatePercentage == null ? 0 : Convert.ToDecimal(contractLevelEntity.MinimumRatePercentage),
                        MaximumRatePercentage = contractLevelEntity.MaximumRatePercentage == null ? 0 : Convert.ToDecimal(contractLevelEntity.MaximumRatePercentage),
                        LifeRate = contractLevelEntity.LifeRate == null ? 0 : Convert.ToDecimal(contractLevelEntity.LifeRate),
                        CalculationType = contractLevelEntity.CalculationType == null ? 0 : (CalculationTypes)contractLevelEntity.CalculationType,
                        ApplyOnType = contractLevelEntity.ApplyOn == null ? 0 : (ApplyOnTypes)contractLevelEntity.ApplyOn,
                        AnnualAddedLimit = contractLevelEntity.AnnualAddedLimit == null ? 0 : Convert.ToDecimal(contractLevelEntity.AnnualAddedLimit),
                        PremiumType = contractLevelEntity.PremiumType == null ? 0 : (PremiumTypes)contractLevelEntity.PremiumType

                    }).ToList();
        }

        /// <summary>
        /// GetLevelNumberByContractId
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns>int</returns>
        public int GetLevelNumberByContractId(int contractId)
        {
            int levelNumber = 0;
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.Level.Properties.ContractCode, contractId);
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(REINSEN.Level),
                                         criteriaBuilder.GetPredicate()));

            foreach (REINSEN.Level contractLevelEntity in businessCollection.OfType<REINSEN.Level>())
            {
                levelNumber = contractLevelEntity.LevelNumber;
            }
            levelNumber++;

            return levelNumber;
        }

        /// <summary>
        /// GetLevelCompaniesByLevelId
        /// </summary>
        /// <param name = "levelId" ></ param >
        /// < returns > List < LevelCompany /></ returns >
        public List<LevelCompany> GetLevelCompaniesByLevelId(int levelId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(REINSEN.GetContractLevelCompany.Properties.LevelCode, levelId);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                typeof(REINSEN.GetContractLevelCompany), criteriaBuilder.GetPredicate()));

            return (
                from REINSEN.GetContractLevelCompany
                        contractLevelCompanyEntity in businessCollection
                let agent = new Agent
                {
                    IndividualId = contractLevelCompanyEntity.BrokerReinsuranceCompanyId,
                    FullName = contractLevelCompanyEntity.BrokerReinsuranceCompanyName
                }
                let company = new Company
                {
                    IndividualId = contractLevelCompanyEntity.ReinsuranceCompanyId,
                    FullName = contractLevelCompanyEntity.ReinsuranceCompanyName
                }
                select new LevelCompany()
                {
                    Agent = agent,
                    ComissionPercentage = contractLevelCompanyEntity.CommissionPercentage,
                    LevelCompanyId = contractLevelCompanyEntity.LevelCompanyId,
                    ContractLevel = new Level { ContractLevelId = contractLevelCompanyEntity.LevelCode },
                    GivenPercentage = Convert.ToDecimal(contractLevelCompanyEntity.ParticipationPercentage),
                    Company = company

                }).ToList();
        }


        /// <summary>
        /// GetParticipationPercentageByLevelId
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns>decimal</returns>
        public decimal GetParticipationPercentageByLevelId(int levelId)
        {
            decimal participationPercentage = 0;

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(REINSEN.LevelCompany.Properties.LevelCode, levelId);
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                typeof(REINSEN.LevelCompany), filter.GetPredicate()));

            foreach (REINSEN.LevelCompany contractLevelCompanyEntity in businessCollection.OfType<REINSEN.LevelCompany>())
            {
                participationPercentage += contractLevelCompanyEntity.ParticipationPercentage;
            }

            return participationPercentage;
        }
        #endregion Get
    }
}
