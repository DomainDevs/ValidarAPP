using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;


namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class BankStatementDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// DeleteBankStatementTransactionItem
        /// </summary>
        /// <param name="bankStatementId"></param>
        /// <returns>bool</returns>
        public bool DeleteBankStatementTransactionItem(int bankStatementId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.BankStatement.CreatePrimaryKey(bankStatementId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.BankStatement bankStatementEntity = (ACCOUNTINGEN.BankStatement)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(bankStatementEntity);

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateBankStatementTransactionItem
        /// Actualiza la entidad de Extracto Bancario
        /// </summary>
        /// <param name="bankStatement"></param>
        /// <returns>bool</returns>
        public bool UpdateBankStatementTransactionItem(ACCOUNTINGEN.BankStatement bankStatement)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.BankStatement.CreatePrimaryKey(bankStatement.BankStatementId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.BankStatement bankStatementEntity = (ACCOUNTINGEN.BankStatement)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                bankStatementEntity.VoucherNumber = bankStatement.VoucherNumber;
                bankStatementEntity.MovementDate = bankStatement.MovementDate;
                bankStatementEntity.Amount = bankStatement.Amount;
                bankStatementEntity.Description = bankStatement.Description;
                bankStatementEntity.SourceBranchCode = bankStatement.SourceBranchCode;
                bankStatementEntity.ThirdDescription = bankStatement.ThirdDescription;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(bankStatementEntity);

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}
