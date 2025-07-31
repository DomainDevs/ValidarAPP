using System;
using System.Collections.Generic;

//Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.ReconciliationServices.EEProvider.DAOs;
using Sistran.Core.Application.ReconciliationServices.Models;


namespace Sistran.Core.Application.ReconciliationServices.EEProvider
{
    public class ReconciliationServicesEEProvider : IReconciliationService
    {
        #region Instance Variables

        #region Interfaz

        #endregion

        #region DAOs

        private readonly BankReconciliationMovementTypeDAO _bankReconciliationMovementTypeDAO = new BankReconciliationMovementTypeDAO();
        private readonly ReconciliationDAO _reconciliationDAO = new ReconciliationDAO();
        private readonly ReconciliationFormatDAO _reconciliationFormatDAO = new ReconciliationFormatDAO();
        private readonly StatementDAO _statementDAO = new StatementDAO();

        #endregion

        #endregion

        #region Statements

        /// <summary>
        /// SaveBankStatements : Graba Extractos Bancarios
        /// </summary>
        /// <param name="bankStatements"></param>
        /// <returns></returns>
        public void SaveBankStatements(List<Statement> bankStatements)
        {
            _statementDAO.SaveBankStatement(bankStatements);
        }

        /// <summary>
        /// UpdateBankStatement : Actualiza un Extracto Bancario
        /// </summary>
        /// <param name="bankStatement"></param>
        /// <returns></returns>
        public void UpdateBankStatement(Statement bankStatement)
        {
            _statementDAO.UpdateBankStatement(bankStatement);
        }

        /// <summary>
        /// DeleteBankStatement : Elimina un Extracto Bancario
        /// </summary>
        /// <param name="bankStatement"></param>
        /// <returns></returns>
        public void DeleteBankStatement(Statement bankStatement)
        {
            _statementDAO.DeleteBankStatement(bankStatement);
        }

        /// <summary>
        /// GetBankStatementsByAccountBank : Obtiene Extractos Bancarios por Cuenta Bancaria y Fecha
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <param name="dateTo"></param>
        /// <returns>List<Statement></returns>
        public List<Statement> GetBankStatementsByAccountBank(BankAccountCompanyDTO bankAccountCompany, DateTime dateTo)
        {
            return _statementDAO.GetBankStatementsByAccountBank(bankAccountCompany, dateTo);
        }

        /// <summary>
        /// GetFailedBankStatementsByAccountBank : Obtiene Extractos Bancarios Fallidos  por Cuenta Bancaria y Fecha
        /// </summary>
        /// <param name="BankAccountCompany"></param>
        /// <param name="dateTo"></param>
        /// <returns>List<Statement></returns>
        public List<Statement> GetFailedBankStatementsByAccountBank(BankAccountCompanyDTO bankAccountCompany, DateTime dateTo)
        {
            return _statementDAO.GetFailedBankStatementsByAccountBank(bankAccountCompany, dateTo);
        }


        #endregion

        #region Reconciliation

        /// <summary>
        /// SaveReconciliation: Graba la Conciliacion Bancaria
        /// </summary>            
        /// <param name="reconciliations"></param>
        /// <returns>Reconciliation</returns>
        public Reconciliation SaveReconciliations(List<Reconciliation> reconciliations)
        {
            return _reconciliationDAO.SaveReconciliation(reconciliations);
        }

        /// <summary>
        /// Reconcile: Conciliacion Automatica
        /// </summary>
        /// <param name="BankAccountCompany"></param>
        /// <param name="dateTo"></param>
        /// <param name="dateConciliation"></param>
        /// <param name="getBanks"></param>
        /// <param name="getCentralAccountings"></param>
        /// <param name="getDailyAccountings"></param>
        /// <param name="byType"></param>
        /// <param name="byMonth"></param>
        /// <param name="byDocumentNumber"></param>
        /// <param name="byDate"></param>
        /// <param name="byBranch"></param>
        /// <param name="userId"></param>
        /// <returns>List<Reconciliation></returns>
        public List<Reconciliation> Reconcile(BankAccountCompanyDTO bankAccountCompany, DateTime dateTo, DateTime dateConciliation, bool getBanks, bool getCentralAccountings, bool getDailyAccountings, bool byType, bool byMonth, bool byDocumentNumber, bool byDate, bool byBranch, int userId)
        {
            return _reconciliationDAO.Reconcile(bankAccountCompany, dateTo, dateConciliation, getBanks, getCentralAccountings, getDailyAccountings, byType, byMonth, byDocumentNumber, byDate, byBranch, userId);
        }


        /// <summary>
        /// GetReconciliationsByStatementTypes: Obtiene Conciliaciones por Tipo de Extracto
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <param name="dateTo"></param>
        /// <param name="getBanks"></param>
        /// <param name="getCentralAccountings"></param>
        /// <param name="getDailyAccountings"></param>
        /// <param name="userId"></param>
        /// <returns> List<Reconciliation></returns>
        public List<Reconciliation> GetReconciliationsByStatementTypes(BankAccountCompanyDTO bankAccountCompany, DateTime dateTo, bool getBanks, bool getCentralAccountings, bool getDailyAccountings, int userId)
        {
            return _reconciliationDAO.GetReconciliationsByStatementTypes(bankAccountCompany, dateTo, getBanks, getCentralAccountings, getDailyAccountings, userId);
        }

        /// <summary>
        /// GetReconciliationsByReconciliationId: Obtiene Conciliacion por el Id de Conciliacion
        /// </summary>
        /// <param name="reconciliationId"></param>
        /// <returns> List<Conciliation></returns>
        public List<Reconciliation> GetReconciliationsByReconciliationId(int reconciliationId)
        {
            return _reconciliationDAO.GetReconciliationsByReconciliationId(reconciliationId);
        }

        /// <summary>
        /// ReverseReconciliation: Reversa Conciliacion 
        /// </summary>
        /// <param name="reconciliation"></param>
        /// <returns>Status</returns>
        public int ReverseReconciliation(Reconciliation reconciliation)
        {
            return _reconciliationDAO.ReverseReconciliation(reconciliation);
        }

        #endregion

        #region BankReconciliationMovementType

        /// <summary>
        /// SaveBankReconciliationMovementType
        /// </summary>
        /// <param name="bankReconciliationMovementType"></param>
        public void SaveBankReconciliationMovementType(BankReconciliationMovementType bankReconciliationMovementType)
        {
            _bankReconciliationMovementTypeDAO.SaveBankReconciliationMovementType(bankReconciliationMovementType);
        }

        /// <summary>
        /// UpdateBankReconciliationMovementType
        /// </summary>
        /// <param name="bankReconciliationMovementType"></param>
        public void UpdateBankReconciliationMovementType(BankReconciliationMovementType bankReconciliationMovementType)
        {
            _bankReconciliationMovementTypeDAO.UpdateBankReconciliationMovementType(bankReconciliationMovementType);
        }

        /// <summary>
        /// DeleteBankReconciliationMovementType
        /// </summary>
        /// <param name="bankReconciliationMovementType"></param>
        public void DeleteBankReconciliationMovementType(BankReconciliationMovementType bankReconciliationMovementType)
        {
            _bankReconciliationMovementTypeDAO.DeleteBankReconciliationMovementType(bankReconciliationMovementType);
        }

        /// <summary>
        /// BankReconciliationMovementType
        /// </summary>        
        /// <returns>List<ReconciliationMovementType></returns>
        public List<BankReconciliationMovementType> GetBankReconciliationMovementTypes(Bank bank)
        {
            return _bankReconciliationMovementTypeDAO.GetBankReconciliationMovementTypes(bank);
        }


        #endregion

        #region ReconciliationFormat

        /// <summary>
        /// SaveReconciliationFormat
        /// </summary>
        /// <param name="reconciliationFormat"></param>
        public void SaveReconciliationFormat(ReconciliationFormat reconciliationFormat)
        {
            _reconciliationFormatDAO.SaveReconciliationFormat(reconciliationFormat);
        }

        /// <summary>
        /// UpdateReconciliationFormat
        /// </summary>
        /// <param name="reconciliationFormat"></param>
        public void UpdateReconciliationFormat(ReconciliationFormat reconciliationFormat)
        {
            _reconciliationFormatDAO.UpdateReconciliationFormat(reconciliationFormat);
        }

        /// <summary>
        /// DeleteReconciliationFormat
        /// </summary>
        /// <param name="reconciliationFormatId"></param>
        public void DeleteReconciliationFormat(int reconciliationFormatId)
        {
            _reconciliationFormatDAO.DeleteReconciliationFormat(reconciliationFormatId);
        }

        /// <summary>
        /// GetReconciliationFormats
        /// </summary>
        /// <returns>List<ReconciliationFormat></returns>
        public List<ReconciliationFormat> GetReconciliationFormats()
        {
            return _reconciliationFormatDAO.GetReconciliationFormats();
        }

        /// <summary>
        /// GetReconciliationFormat
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns>int</returns>
        public int GetReconciliationFormat(int bankId)
        {
            return _reconciliationFormatDAO.GetReconciliationFormat(bankId);
        }


        #endregion

    }
}
