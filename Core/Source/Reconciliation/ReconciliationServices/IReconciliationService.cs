
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReconciliationServices.Models;

namespace Sistran.Core.Application.ReconciliationServices
{
    [ServiceContract]
    public interface IReconciliationService
    {

        #region Statements

        /// <summary>
        /// SaveBankStatements : Graba Extractos Bancarios
        /// </summary>
        /// <param name="bankStatements"></param>
        /// <returns></returns>
        [OperationContract]
        void SaveBankStatements(List<Statement> bankStatements);


        /// <summary>
        /// UpdateBankStatement : Actualiza un Extracto Bancario
        /// </summary>
        /// <param name="bankStatement"></param>
        /// <returns></returns>
        [OperationContract]
        void UpdateBankStatement(Statement bankStatement);

        /// <summary>
        /// DeleteBankStatement : Elimina un Extracto Bancario
        /// </summary>
        /// <param name="bankStatement"></param>
        /// <returns></returns>
        [OperationContract]
        void DeleteBankStatement(Statement bankStatement);

        /// <summary>
        /// GetBankStatementsByAccountBank : Obtiene Extractos Bancarios por Cuenta Bancaria y Fecha
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <param name="dateTo"></param>
        /// <returns> List<Statement></returns>
        [OperationContract]
        List<Statement> GetBankStatementsByAccountBank(BankAccountCompanyDTO bankAccountCompany, DateTime dateTo);

        /// <summary>
        /// GetFailedBankStatementsByAccountBank : Obtiene Extractos Bancarios Fallidos  por Cuenta Bancaria y Fecha
        /// </summary>
        /// <param name="BankAccountCompany"></param>
        /// <param name="dateTo"></param>
        /// <returns> List<Statement></returns>
        [OperationContract]
        List<Statement> GetFailedBankStatementsByAccountBank(BankAccountCompanyDTO bankAccountCompany, DateTime dateTo);


        #endregion

        #region Reconciliation
        /// <summary>
        /// SaveReconciliation: Graba la Conciliacion Bancaria
        /// </summary>            
        /// <param name="reconciliations"></param>
        /// <returns>Reconciliation</returns>
        [OperationContract]
        Reconciliation SaveReconciliations(List<Reconciliation> reconciliations);

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
        /// <param name="user"></param>
        /// <returns> List<Reconciliation></returns>
        [OperationContract]
        List<Reconciliation> Reconcile(BankAccountCompanyDTO bankAccountCompany, DateTime dateTo, DateTime dateConciliation, bool getBanks, bool getCentralAccountings, bool getDailyAccountings, bool byType, bool byMonth, bool byDocumentNumber, bool byDate, bool byBranch, int userId);


        /// <summary>
        /// GetReconciliationsByStatementTypes: Obtiene Conciliaciones por Tipo de Extracto
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <param name="dateTo"></param>
        /// <param name="getBanks"></param>
        /// <param name="getCentralAccountings"></param>
        /// <param name="getDailyAccountings"></param>
        /// <param name="user"></param>
        /// <returns> List<Reconciliation></returns>
        [OperationContract]
        List<Reconciliation> GetReconciliationsByStatementTypes(BankAccountCompanyDTO bankAccountCompany, DateTime dateTo, bool getBanks, bool getCentralAccountings, bool getDailyAccountings , int userId);

        /// <summary>
        /// GetReconciliationsByReconciliationId: Obtiene Conciliacion por el Id de Conciliacion
        /// </summary>
        /// <param name="reconciliationId"></param>
        /// <returns> List<Conciliation></returns>
        [OperationContract]
        List<Reconciliation> GetReconciliationsByReconciliationId(int reconciliationId);


        /// <summary>
        /// ReverseReconciliation: Reversa Conciliacion 
        /// </summary>
        /// <param name="reconciliation"></param>
        /// <returns>Status</returns>
        [OperationContract]
        int ReverseReconciliation(Reconciliation reconciliation);

        #endregion
        
        #region BankReconciliationMovementType

        /// <summary>
        /// SaveBankReconciliationMovementType
        /// </summary>
        /// <param name="bankReconciliationMovementType"></param>
        [OperationContract]
        void SaveBankReconciliationMovementType(BankReconciliationMovementType bankReconciliationMovementType);

        /// <summary>
        /// UpdateBankReconciliationMovementType
        /// </summary>
        /// <param name="bankReconciliationMovementType"></param>
        [OperationContract]
        void UpdateBankReconciliationMovementType(BankReconciliationMovementType bankReconciliationMovementType);

        /// <summary>
        /// DeleteBankReconciliationMovementType
        /// </summary>
        /// <param name="bankReconciliationMovementType"></param>
        [OperationContract]
        void DeleteBankReconciliationMovementType(BankReconciliationMovementType bankReconciliationMovementType);


        /// <summary>
        /// BankReconciliationMovementType
        /// </summary>        
        /// <returns>List<ReconciliationMovementType></returns>
        [OperationContract]
        List<BankReconciliationMovementType> GetBankReconciliationMovementTypes( Bank bank);


        #endregion

        #region ReconciliationFormat

        /// <summary>
        /// SaveReconciliationFormat
        /// </summary>
        /// <param name="reconciliationFormat"></param>
        [OperationContract]
        void SaveReconciliationFormat(ReconciliationFormat reconciliationFormat);

        /// <summary>
        /// UpdateReconciliationFormat
        /// </summary>
        /// <param name="reconciliationFormat"></param>
        [OperationContract]
        void UpdateReconciliationFormat(ReconciliationFormat reconciliationFormat);

        /// <summary>
        /// DeleteReconciliationFormat
        /// </summary>
        /// <param name="reconciliationFormatId"></param>
        [OperationContract]
        void DeleteReconciliationFormat(int reconciliationFormatId);

        /// <summary>
        /// GetReconciliationFormats
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ReconciliationFormat> GetReconciliationFormats();

        /// <summary>
        /// GetReconciliationFormat
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int GetReconciliationFormat(int bankId);


        #endregion
    }
}
