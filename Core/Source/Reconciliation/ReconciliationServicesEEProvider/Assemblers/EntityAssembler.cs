//Sistran Core
using Sistran.Core.Application.ReconciliationServices.Models;

//Sistran FWK


namespace Sistran.Core.Application.ReconciliationServices.EEProvider.Assemblers
{
    internal static class EntityAssembler
    {
        #region Statement

        /// <summary>
        /// CreateBankStatement
        /// </summary>
        /// <param name="bankStatement"></param>
        /// <returns></returns>
        public static Entities.BankStatement CreateBankStatement(Statement bankStatement)
        {
            int? reconciliationMovementTypeId;
            int? sourceBranchId;
            if (bankStatement.ReconciliationMovementType.Id == 0)
            {
                reconciliationMovementTypeId = null;
            }
            else
            {
                reconciliationMovementTypeId = bankStatement.ReconciliationMovementType.Id;
            }
            if (bankStatement.Branch.Id == 0)
            {
                sourceBranchId = null;
            }
            else
            {
                sourceBranchId = bankStatement.Branch.Id;
            }

            return new Entities.BankStatement(bankStatement.Id)
            {
                Amount = bankStatement.LocalAmount.Value,
                BankAccountCompanyCode = bankStatement.BankAccountCompany.Id,
                BankReconciliationCode = reconciliationMovementTypeId, // Identificador de conciliación bancaria
                BankStatementId = bankStatement.Id,
                CityCode = null,
                CutDate = null,
                Description = bankStatement.Description,
                MovementDate = bankStatement.Date,
                ProcessDate = bankStatement.ProcessDate,
                ProcessNumber = bankStatement.ProcessNumber,
                ReconciliationCode = null,
                ReconciliationDate = null,
                SourceBranchCode = sourceBranchId,
                ThirdDescription = bankStatement.ThirdPerson.FullName,
                UserCode = bankStatement.UserId,
                VoucherNumber = bankStatement.DocumentNumber
            };
        }

        /// <summary>
        /// CreateTempBankStatement
        /// </summary>
        /// <param name="bankStatement"></param>
        /// <returns></returns>
        public static Entities.TempBankStatement CreateTempBankStatement(Statement bankStatement)
        {
            int? reconciliationMovementTypeId;
            int? sourceBranchId;
            if(bankStatement.ReconciliationMovementType.Id == 0)
            {
                reconciliationMovementTypeId = null;
            }
            else
            {
                reconciliationMovementTypeId = bankStatement.ReconciliationMovementType.Id;
            }
            if (bankStatement.Branch.Id == 0)
            {
                sourceBranchId = null;
            }
            else
            {
                sourceBranchId = bankStatement.Branch.Id;
            }

            return new Entities.TempBankStatement(bankStatement.Id)
            {
                Amount = bankStatement.LocalAmount.Value,
                BankAccountCompanyCode = bankStatement.BankAccountCompany.Id,
                BankReconciliationCode = reconciliationMovementTypeId, // Identificador de conciliación bancaria
                BankStatementId = bankStatement.Id,
                CityCode = null,
                CutDate = null,
                Description = bankStatement.Description,
                ErrorDescription = bankStatement.ThirdPerson.CustomerTypeDescription,
                MovementDate = bankStatement.Date,
                ProcessDate = bankStatement.ProcessDate,
                ProcessNumber = bankStatement.ProcessNumber,
                ReconciliationCode = null,
                ReconciliationDate = null,
                SourceBranchCode = sourceBranchId,
                ThirdDescription = bankStatement.ThirdPerson.FullName,
                UserCode = bankStatement.UserId,
                VoucherNumber = bankStatement.DocumentNumber
            };
        }

        #endregion

        #region Reconciliation

        /// <summary>
        /// CreateReconciliation
        /// </summary>
        /// <param name="reconciliation"></param>
        /// <returns></returns>
        public static Entities.Reconciliation CreateReconciliation(Reconciliation reconciliation)
        {
            return new Entities.Reconciliation(reconciliation.Id, 0, 0, "", 0)
            {
                /*
                BankNetworkId = bankNetwork.Id,
                Description = bankNetwork.Description,
                Commission = bankNetwork.Commission.LocalAmount > 0 ? true : false,
                Tax = bankNetwork.HasTax == true ? true : false,
                MaximumCoupon = bankNetwork.RetriesNumber,
                TypePercentageCommission = bankNetwork.TaxCategory.Id,
                CommissionRate = 0,
                CommissionAmount = bankNetwork.Commission.LocalAmount,
                Header = false,
                Summary = false,
                Prenotification = bankNetwork.RequiresNotification
                */
            };
        }

        #endregion

        #region BankReconciliationMovementType

        /// <summary>
        /// CreateBankReconciliationBank
        /// </summary>
        /// <param name="bankReconciliationMovementType"></param>
        /// <returns></returns>
        public static Entities.BankReconciliationBank CreateBankReconciliationBank(BankReconciliationMovementType bankReconciliationMovementType)
        {
            return new Entities.BankReconciliationBank(bankReconciliationMovementType.Bank.Id, bankReconciliationMovementType.ReconciliationMovementType.Id, bankReconciliationMovementType.SmallDescription)
            {
                BankCode = bankReconciliationMovementType.Bank.Id,
                BankReconciliationCode = bankReconciliationMovementType.ReconciliationMovementType.Id,
                Description = bankReconciliationMovementType.SmallDescription,
                IsVoucher = bankReconciliationMovementType.VoucherNumber
            };
        }

        #endregion

        #region ReconciliationFormat

        /// <summary>
        /// CreateReconciliationFormat
        /// </summary>
        /// <param name="reconciliationFormat"></param>
        /// <returns>BankNetwork</returns>
        public static Entities.BankAccountCompanyFormat CreateReconciliationFormat(ReconciliationFormat reconciliationFormat)
        {
            return new Entities.BankAccountCompanyFormat(reconciliationFormat.Id)
            {
                BankAccountCompanyCode = reconciliationFormat.Bank.Id,
                BankAccountCompanyFormatId = reconciliationFormat.Id,
                FormatCode = reconciliationFormat.Format.Id,
            };
        }

        #endregion
    }
}
