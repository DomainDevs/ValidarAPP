//Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.ReconciliationServices.Models;
using Sistran.Core.Application.ReportingServices.Models.Formats;
using Sistran.Core.Application.UniquePersonService.V1.Models;

//Sistran FWK
using Sistran.Core.Framework.DAF;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.ReconciliationServices.EEProvider.Assemblers
{
    static internal class ModelAssembler
    {
        #region Statement


        /// <summary>
        /// CreateBankStatement
        /// </summary>
        /// <param name="bankStatement"></param>
        /// <returns></returns>
        public static Statement CreateBankStatement(Entities.BankStatement bankStatement)
        {
            Amount amount = new Amount() { Value = Convert.ToDecimal(bankStatement.Amount) };
            Amount localAmount = new Amount() { Value = Convert.ToDecimal(bankStatement.Amount) };

            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO() { Id = Convert.ToInt32(bankStatement.BankAccountCompanyCode) };
            Branch branch = new Branch() { Id = Convert.ToInt32(bankStatement.SourceBranchCode) };
            Individual thirdPerson = new Individual() { FullName = bankStatement.ThirdDescription };
            ReconciliationMovementTypeDTO reconciliationMovementType = new ReconciliationMovementTypeDTO() { Id = Convert.ToInt32(bankStatement.BankReconciliationCode) };

            return new Statement()
            {
                Amount = amount,
                BankAccountCompany = bankAccountCompany,
                Branch = branch,
                Date = Convert.ToDateTime(bankStatement.MovementDate),
                Description = bankStatement.Description,
                DocumentNumber = bankStatement.VoucherNumber,
                Id = bankStatement.BankStatementId,
                LocalAmount = localAmount,
                ProcessDate = Convert.ToDateTime(bankStatement.ProcessDate),
                ProcessNumber = Convert.ToInt32(bankStatement.ProcessNumber),
                ReconciliationMovementType = reconciliationMovementType,
                StatementType = StatementTypes.Bank,
                Status = 1,
                ThirdPerson = thirdPerson
            };
        }

        /// <summary>
        /// CreateTempBankStatement
        /// </summary>
        /// <param name="tempBankStatement"></param>
        /// <returns></returns>
        public static Statement CreateTempBankStatement(Entities.TempBankStatement tempBankStatement)
        {
            Amount amount = new Amount() { Value = Convert.ToDecimal(tempBankStatement.Amount) };
            Amount localAmount = new Amount() { Value = Convert.ToDecimal(tempBankStatement.Amount) };

            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO() { Id = Convert.ToInt32(tempBankStatement.BankAccountCompanyCode) };
            Branch branch = new Branch() { Id = Convert.ToInt32(tempBankStatement.SourceBranchCode) };
            Individual thirdPerson = new Individual() { FullName = tempBankStatement.ThirdDescription };
            ReconciliationMovementTypeDTO reconciliationMovementType = new ReconciliationMovementTypeDTO() { Id = Convert.ToInt32(tempBankStatement.BankReconciliationCode) };

            return new Statement()
            {
                Amount = amount,
                BankAccountCompany = bankAccountCompany,
                Branch = branch,
                Date = Convert.ToDateTime(tempBankStatement.MovementDate),
                Description = tempBankStatement.Description,
                DocumentNumber = tempBankStatement.VoucherNumber,
                Id = tempBankStatement.BankStatementId,
                LocalAmount = localAmount,
                ProcessDate = Convert.ToDateTime(tempBankStatement.ProcessDate),
                ProcessNumber = Convert.ToInt32(tempBankStatement.ProcessNumber),
                ReconciliationMovementType = reconciliationMovementType,
                StatementType = StatementTypes.Bank,
                Status = 1,
                ThirdPerson = thirdPerson
            };
        }

        #endregion

        #region Reconciliation

        /// <summary>
        /// CreateReconciliation
        /// </summary>
        /// <param name="reconciliation"></param>
        /// <returns></returns>
        public static Reconciliation CreateReconciliation(Entities.Reconciliation reconciliation)
        {
            return new Reconciliation()
            {
                BankStatements = null,
                CompanyStatements = null,
                Date = reconciliation.ReconciliationDate,
                Id = reconciliation.ReconciliationCode,
                ReconciliationType = ReconciliationTypes.Manual,
                UserId = reconciliation.UserCode 
            };
        }

        /// <summary>
        /// CreateReconciliations
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<Reconciliation> CreateReconciliations(BusinessCollection businessCollection)
        {
            List<Reconciliation> reconciliations = new List<Reconciliation>();
            foreach (Entities.Reconciliation reconciliationEntity in businessCollection.OfType<Entities.Reconciliation>())
            {
                reconciliations.Add(CreateReconciliation(reconciliationEntity));
            }

            return reconciliations;
        }

        #endregion

        #region BankReconciliationMovementType

        /// <summary>
        /// CreateBankReconciliationMovementType
        /// </summary>
        /// <param name="bankReconciliationBank"></param>
        /// <returns></returns>
        public static BankReconciliationMovementType CreateBankReconciliationMovementType(Entities.BankReconciliationBank bankReconciliationBank)
        {
            Bank bank = new Bank() { Id = bankReconciliationBank.BankCode };
            ReconciliationMovementTypeDTO reconciliationMovementType = new ReconciliationMovementTypeDTO() { Id = bankReconciliationBank.BankReconciliationCode };

            return new BankReconciliationMovementType()
            {
                Bank = bank,
                Id = bankReconciliationBank.BankReconciliationCode,
                ReconciliationMovementType = reconciliationMovementType,
                SmallDescription = bankReconciliationBank.Description,
                VoucherNumber = Convert.ToBoolean(bankReconciliationBank.IsVoucher)
            };
        }

        /// <summary>
        /// CreateBankReconciliationMovementTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<BankReconciliationMovementType> CreateBankReconciliationMovementTypes(BusinessCollection businessCollection)
        {
            List<BankReconciliationMovementType> bankReconciliationMovementTypes = new List<BankReconciliationMovementType>();
            foreach (Entities.BankReconciliationBank bankReconciliationBankEntity in businessCollection.OfType<Entities.BankReconciliationBank>())
            {
                bankReconciliationMovementTypes.Add(CreateBankReconciliationMovementType(bankReconciliationBankEntity));
            }

            return bankReconciliationMovementTypes;
        }

        #endregion

        #region ReconciliationFormats

        /// <summary>
        /// CreateReconciliationFormat
        /// </summary>
        /// <param name="bankAccountCompanyFormat"></param>
        /// <returns></returns>
        public static ReconciliationFormat CreateReconciliationFormat(Entities.BankAccountCompanyFormat bankAccountCompanyFormat)
        {
            return new ReconciliationFormat()
            {
                Bank = new Bank() { Id = bankAccountCompanyFormat.BankAccountCompanyCode },
                Format = new Format() { Id = bankAccountCompanyFormat.FormatCode },
                Id = bankAccountCompanyFormat.BankAccountCompanyFormatId
            };
        }

        /// <summary>
        /// CreateReconciliationFormats
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<ReconciliationFormat> CreateReconciliationFormats(BusinessCollection businessCollection)
        {
            List<ReconciliationFormat> reconciliationFormats = new List<ReconciliationFormat>();
            foreach (Entities.BankAccountCompanyFormat bankAccountCompanyFormatEntity in businessCollection.OfType<Entities.BankAccountCompanyFormat>())
            {
                reconciliationFormats.Add(CreateReconciliationFormat(bankAccountCompanyFormatEntity));
            }

            return reconciliationFormats;
        }

        #endregion
    }
}
