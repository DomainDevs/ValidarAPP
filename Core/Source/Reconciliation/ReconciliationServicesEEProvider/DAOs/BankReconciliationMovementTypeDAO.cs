//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.ReconciliationServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReconciliationServices.Models;
using CommonModels = Sistran.Core.Application.CommonService.Models;


//Sitran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;

using System;
using System.Collections.Generic;
using System.Data;

namespace Sistran.Core.Application.ReconciliationServices.EEProvider.DAOs
{
    public class BankReconciliationMovementTypeDAO
    {
        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods

        #region Save

        /// <summary>
        /// SaveBankReconciliationMovementType
        /// </summary>
        /// <param name="bankReconciliationMovementType"></param>
        public void SaveBankReconciliationMovementType(BankReconciliationMovementType bankReconciliationMovementType)
        {
            try
            {
                // Convertir de model a entity
                Entities.BankReconciliationBank bankReconciliationBankEntity = EntityAssembler.CreateBankReconciliationBank(bankReconciliationMovementType);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(bankReconciliationBankEntity);

                // Return del model
                ModelAssembler.CreateBankReconciliationMovementType(bankReconciliationBankEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateBankReconciliationMovementType
        /// </summary>
        /// <param name="bankReconciliationMovementType"></param>
        public void UpdateBankReconciliationMovementType(BankReconciliationMovementType bankReconciliationMovementType)
        {
            try
            {
                // Crea el Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.BankReconciliationBank.CreatePrimaryKey(bankReconciliationMovementType.Bank.Id,
                                                                        bankReconciliationMovementType.ReconciliationMovementType.Id,
                                                                        bankReconciliationMovementType.SmallDescription);

                // Encuentra el objeto en referencia a la llave primaria
                Entities.BankReconciliationBank bankReconciliationBankEntity = (Entities.BankReconciliationBank)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                
                bankReconciliationBankEntity.IsVoucher = bankReconciliationMovementType.VoucherNumber;

                // Realiza las operaciones con las entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(bankReconciliationBankEntity);

                // Return del model
                ModelAssembler.CreateBankReconciliationMovementType(bankReconciliationBankEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteBankReconciliationMovementType
        /// </summary>
        /// <param name="bankReconciliationMovementType"></param>
        public void DeleteBankReconciliationMovementType(BankReconciliationMovementType bankReconciliationMovementType)
        {
            try
            {
                // Crea el Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.BankReconciliationBank.CreatePrimaryKey(bankReconciliationMovementType.Bank.Id,
                                                                        bankReconciliationMovementType.ReconciliationMovementType.Id,
                                                                        bankReconciliationMovementType.SmallDescription);

                // Realiza las operaciones con las entities utilizando DAF
                Entities.BankReconciliationBank bankReconciliationBankEntity = (Entities.BankReconciliationBank)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                _dataFacadeManager.GetDataFacade().DeleteObject(bankReconciliationBankEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Get

        /// <summary>
        /// BankReconciliationMovementType
        /// </summary>        
        /// <returns>List<ReconciliationMovementType></returns>
        public List<BankReconciliationMovementType> GetBankReconciliationMovementTypes(CommonModels.Bank bank)
        {
            int rowGrid;
            List<BankReconciliationMovementType> bankReconciliationMovementTypes = new List<BankReconciliationMovementType>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(Entities.BankReconciliationBank.Properties.BankCode, bank.Id);
                UIView data = _dataFacadeManager.GetDataFacade().GetView("BankReconciliationBankView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rowGrid);

                foreach (DataRow dataRow in data)
                {
                    bankReconciliationMovementTypes.Add(new BankReconciliationMovementType()
                    {
                        Bank = new CommonModels.Bank()
                        {
                            Description = dataRow["BankDescription"] == DBNull.Value ? "" : dataRow["BankDescription"].ToString(),
                            Id = dataRow["BankCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BankCode"]),
                        },
                        Id = 0,
                        ReconciliationMovementType = new ReconciliationMovementTypeDTO()
                        {
                            Description = dataRow["BankReconciliationDescription"] == DBNull.Value ? "" : dataRow["BankReconciliationDescription"].ToString(),
                            Id = dataRow["BankReconciliationId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BankReconciliationId"]),
                        },
                        VoucherNumber = Convert.ToBoolean(dataRow["IsVoucher"]),
                        SmallDescription = dataRow["BankMovementCode"] == DBNull.Value ? "" : dataRow["BankMovementCode"].ToString()
                    });
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return bankReconciliationMovementTypes;
        }

        #endregion


        #endregion
    }
}
