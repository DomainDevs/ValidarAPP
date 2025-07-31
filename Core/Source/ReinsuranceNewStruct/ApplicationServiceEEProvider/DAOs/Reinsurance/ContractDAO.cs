using System;
using System.Collections.Generic;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{

    internal class ContractDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveContract
        /// </summary>
        /// <param name="contract"></param>
        public int SaveContract(Contract contract)
        {
            REINSEN.Contract entityContract = EntityAssembler.CreateContract(contract);
            _dataFacadeManager.GetDataFacade().InsertObject(entityContract);
            return entityContract.ContractId;
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateContract
        /// </summary>
        /// <param name="contract"></param>
        public void UpdateContract(Contract contract)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.Contract.CreatePrimaryKey(contract.ContractId);
            // Encuentra el objeto en referencia a la llave primaria
            REINSEN.Contract entityContract = (REINSEN.Contract)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));


            DateTime? estimated = null;
            if (contract.EstimatedDate == Convert.ToDateTime("01/01/0001"))
            {
                estimated = null;
            }
            else
            {
                estimated = contract.EstimatedDate;
            }

            entityContract.ContractTypeId = contract.ContractType.ContractTypeId;
            entityContract.CurrencyId = contract.Currency.Id;
            entityContract.DateFrom = contract.DateFrom;
            entityContract.DateTo = contract.DateTo;
            entityContract.Description = contract.Description;
            entityContract.SmallDescription = contract.SmallDescription;
            entityContract.Year = contract.Year;
            entityContract.ReleaseTimeReserve = contract.ReleaseTimeReserve;
            entityContract.AffectationTypeCode = contract.AffectationType.Id;
            entityContract.ReestablishmentTypeCode = contract.ResettlementType.Id;
            entityContract.Epi = contract.PremiumAmount;
            entityContract.EpiTypeCode = contract.EPIType.Id;
            entityContract.Status = contract.Enabled;
            entityContract.Grouper = contract.GroupContract;
            entityContract.CoinsurancePercentage = contract.CoInsurancePercentage;
            entityContract.QuantityRisk = contract.RisksNumber;
            entityContract.EstimatedDate = estimated;
            entityContract.DepositPremiumAmount = contract.DepositPremiumAmount;
            entityContract.PercentageDepositPremium = contract.DepositPercentageAmount;
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityContract);
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteContract
        /// </summary>
        /// <param name="contractId"></param>
        public void DeleteContract(int contractId)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.Contract.CreatePrimaryKey(contractId);
            // Realizar las operaciones con los entities utilizando DAF
            REINSEN.Contract entityContract = (REINSEN.Contract)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().DeleteObject(entityContract);
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetContractById
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns>Contract</returns>
        public Contract GetContractById(int contractId)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.Contract.CreatePrimaryKey(contractId);
            // Realizar las operaciones con los entities utilizando DAF
            REINSEN.Contract entityContract = (REINSEN.Contract)DataFacadeManager.GetObject(primaryKey);

            // Retornar el model
            return ModelAssembler.CreateContract(entityContract);
        }

        /// <summary>
        /// GetContracts
        /// Lista de todos los contratos
        /// </summary>
        /// <returns></returns>
        public List<Contract> GetContracts()
        {

            // Asignamos BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                        typeof(REINSEN.Contract)));
            // Return como Lista
            return ModelAssembler.CreateContracts(businessCollection);
        }

        /// <summary>
        /// GetEnabledContracts
        /// </summary>
        /// <returns></returns>
        public List<Contract> GetEnabledContracts()
        {
            List<Contract> contracts = new List<Contract>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.Contract.Properties.Status, true);

            // Asignamos BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                                            typeof(REINSEN.Contract), criteriaBuilder.GetPredicate()));

            foreach (REINSEN.Contract entityContract in businessCollection.OfType<REINSEN.Contract>())
            {
                contracts.Add(new Contract
                {
                    ContractId = entityContract.ContractId,
                    SmallDescription = entityContract.SmallDescription,
                    Year = entityContract.Year
                });
            }
            return contracts;
        }

        /// <summary>
        /// GetContractsByYearAndContractTypeId
        /// </summary>
        /// <param name="year"></param>
        /// <param name="contractTypeId"></param>
        /// <returns>List<Contract/></returns>
        public List<Contract> GetContractsByYearAndContractTypeId(int year, int contractTypeId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            BusinessCollection businessCollection = new BusinessCollection();

            if (year == 0)
            {

                criteriaBuilder.PropertyEquals(REINSEN.Contract.Properties.ContractTypeId, contractTypeId);
                businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(REINSEN.Contract), criteriaBuilder.GetPredicate()));

                return (from REINSEN.Contract contractEntity in businessCollection
                        select new
                        Contract
                        {
                            ContractId = contractEntity.ContractId,
                            ContractType = new ContractType { ContractTypeId = contractEntity.ContractTypeId },
                            Description = contractEntity.Description,
                        }).ToList();
            }
            else
            {
                criteriaBuilder.PropertyEquals(REINSEN.Contract.Properties.Year, year);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(REINSEN.Contract.Properties.ContractTypeId, contractTypeId);
                businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                                typeof(REINSEN.Contract), criteriaBuilder.GetPredicate()));
                return (from REINSEN.Contract contractEntity in businessCollection
                        select new
                        Contract
                        {
                            ContractId = contractEntity.ContractId,
                            Currency = new Currency() { Id = contractEntity.CurrencyId },
                            ContractType = new ContractType { ContractTypeId = contractEntity.ContractTypeId },
                            DateFrom = contractEntity.DateFrom,
                            DateTo = contractEntity.DateTo,
                            Description = contractEntity.Description,
                            SmallDescription = contractEntity.SmallDescription,
                            ReleaseTimeReserve = Convert.ToInt32(contractEntity.ReleaseTimeReserve),
                            Year = contractEntity.Year
                        }).ToList();
            }
        }

        public List<Contract> GetContractsByContractTypeIdDescription(Contract contract)
        {
            List<Contract> contracts = new List<Contract>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.Contract.Properties.SmallDescription, contract.SmallDescription).Or();
            criteriaBuilder.PropertyEquals(REINSEN.Contract.Properties.Description, contract.SmallDescription).And();
            criteriaBuilder.PropertyEquals(REINSEN.Contract.Properties.ContractTypeId, contract.ContractType.ContractTypeId);

            // Asignamos BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                                            typeof(REINSEN.Contract), criteriaBuilder.GetPredicate()));

            foreach (REINSEN.Contract entityContract in businessCollection.OfType<REINSEN.Contract>())
            {
                contracts.Add(new Contract
                {
                    ContractId = entityContract.ContractId,
                    SmallDescription = entityContract.SmallDescription,
                    Year = entityContract.Year,
                    ContractType = new ContractType
                    {
                        ContractTypeId = entityContract.ContractTypeId
                    }
                });
            }

            return contracts;
        }

        #endregion Get
    }
}