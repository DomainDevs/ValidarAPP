using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountReclassification;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.GeneralLedgerServices
{
    [ServiceContract]
    public interface IAccountingApplicationService
    {
        #region GL

        #region AccountingAccountDTO

        /// <summary>
        /// Guarda una cuenta contable
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns>AccountingAccountDTO</returns>
        [OperationContract]
        AccountingAccountDTO SaveAccountingAccount(AccountingAccountDTO accountingAccount);

        /// <summary>
        /// UpdateAccountingAccount
        /// Actualiza una cuenta contable
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <returns>AccountingAccountDTO</returns>
        [OperationContract]
        AccountingAccountDTO UpdateAccountingAccount(AccountingAccountDTO accountingAccount);

        /// <summary>
        /// DeleteAccountingAccount
        /// Borra una cuenta contable
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteAccountingAccount(int accountingAccountId);

        /// <summary>
        /// GetAccountingAccount
        /// Obtiene datos de una cuenta contable
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>AccountingAccountDTO</returns>
        [OperationContract]
        AccountingAccountDTO GetAccountingAccount(int accountingAccountId);

        /// <summary>
        /// GetAccountingAccounts
        /// Obtiene todas una cuenta contables
        /// </summary>
        /// <returns>List<AccountingAccountDTO/></returns>
        [OperationContract]
        List<AccountingAccountDTO> GetAccountingAccounts();

        /// <summary>
        /// GetAccountingAccountsByNumberDescription
        /// Obtiene una cuenta contable haciendo uso de su numero de cuenta o descripcion
        /// </summary>
        /// <returns>List<AccountingAccountDTO/></returns>
        [OperationContract]
        List<AccountingAccountDTO> GetAccountingAccountsByNumberDescription(AccountingAccountDTO accountingAccount);

        /// <summary>
        /// HasChildren
        /// Verifica si la cuenta contable tiene cuentas hijas
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool HasChildren(int accountingAccountId);

        /// <summary>
        /// OnEntry
        /// Función que comprueba que la cuenta no está siendo usada en asientos de diario o de mayor
        /// </summary>
        /// <param name="accountingAccountId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool OnEntry(int accountingAccountId);

        /// <summary>
        /// ValidateAccountingAccount
        /// Método para validar número de Cuenta
        /// </summary>
        /// <param name="accountingAccount"></param>
        /// <param name="edit"></param>
        /// <returns>AccountingAccountValidationDTO</returns>
        [OperationContract]
        AccountingAccountValidationDTO ValidateAccountingAccount(AccountingAccountDTO accountingAccount, int edit);

        /// <summary>
        /// GetAccountingAccountsByParentId
        /// </summary>
        /// <param name="accountParentId"></param>
        /// <returns></returns>
        [OperationContract]
        List<AccountingAccountDTO> GetAccountingAccountsByParentId(int accountParentId);

        #endregion AccountingAccountDTO      

        #region AccountingAccountParent

        /// <summary>
        /// GetAccountingAccountParents
        /// Obtiene Listado de cuentas contables principales
        /// </summary>
        /// <returns>List<AccountingAccountDTO/></returns>
        [OperationContract]
        List<AccountingAccountDTO> GetAccountingAccountParents();

        #endregion AccountingAccountParent

        #region EntryConsultation

        /// <summary>
        /// SearchEntryMovements
        /// Busca asientos de mayor
        /// </summary>
        /// <param name="entryNumber"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="branchId"></param>
        /// <param name="destinationId"></param>
        /// <param name="accountingMovementTypeId"></param>
        /// <returns>List<EntryConsultationDTO/></returns>
        [OperationContract]
        List<EntryConsultationDTO> SearchEntryMovements(int entryNumber, DateTime startDate, DateTime endDate, int branchId, int destinationId, int accountingMovementTypeId);

        /// <summary>
        /// Busca asientos de diario
        /// SearchDailyEntryMovements
        /// </summary>
        /// <param name="entryNumber"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="branchId"></param>
        /// <param name="destinationId"></param>
        /// <param name="accountingMovementTypeId"></param>
        /// <returns>List<EntryConsultationDTO/></returns>
        [OperationContract]
        List<EntryConsultationDTO> SearchDailyEntryMovements(int entryNumber, DateTime startDate, DateTime endDate, int branchId, int destinationId, int accountingMovementTypeId);

        /// <summary>
        /// GetCostCentersByEntryId
        /// Obtiene centros de costo por Id de asiento
        /// </summary>
        /// <param name="entryItemId"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns>List<CostCenterDTO/></returns>
        [OperationContract]
        List<CostCenterDTO> GetCostCentersByEntryId(int entryItemId, bool isJournalEntry);

        /// <summary>
        /// GetEntryAnalysesByEntryId
        /// Obtiene análisis por Id de asiento
        /// </summary>
        /// <param name="entryItemId"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns>List<EntryAnalysisDTO/></returns>
        [OperationContract]
        List<EntryAnalysisDTO> GetEntryAnalysesByEntryId(int entryItemId, bool isJournalEntry);

        /// <summary>
        /// GetPostdatedByEntryId
        /// Obtiene postfechados por Id de asiento
        /// </summary>
        /// <param name="entryItemId"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns>List<PostDated/></returns>
        [OperationContract]
        List<PostDatedDTO> GetPostdatedByEntryId(int entryItemId, bool isJournalEntry);

        #endregion EntryConsultation

        #endregion GL

        #region PARAM

        #region AccountingCompanyDTO

        /// <summary>
        /// SaveAccountingCompany
        /// Graba una compania
        /// </summary>
        /// <param name="accountingCompany"></param>
        /// <returns>AccountingCompanyDTO</returns>
        [OperationContract]
        AccountingCompanyDTO SaveAccountingCompany(AccountingCompanyDTO accountingCompany);

        /// <summary>
        /// UpdateAccountingCompany
        /// Actualiza compañía
        /// </summary>
        /// <param name="accountingCompany"></param>
        /// <returns>AccountingCompanyDTO</returns>
        [OperationContract]
        AccountingCompanyDTO UpdateAccountingCompany(AccountingCompanyDTO accountingCompany);

        /// <summary>
        /// DeleteAccountingCompany
        /// Borra compañía
        /// </summary>
        /// <param name="accountingCompanyId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteAccountingCompany(int accountingCompanyId);

        /// <summary>
        /// GetAccountingCompany
        /// Obtiene compañía
        /// </summary>
        /// <param name="accountingCompany"></param>
        /// <returns>AccountingCompanyDTO</returns>
        [OperationContract]
        AccountingCompanyDTO GetAccountingCompany(AccountingCompanyDTO accountingCompany);

        /// <summary>
        /// GetAccountingCompanies
        /// Obtiene el listado de compañías
        /// </summary>
        /// <returns>List<AccountingCompanyDTO/></returns>
        [OperationContract]
        List<AccountingCompanyDTO> GetAccountingCompanies();

        /// <summary>
        /// VerifyCompanyUsed
        /// Verifica si una compania de la tabla esta siendo usada por un movimineto Contable
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool VerifyCompanyUsed(int companyId);

        #endregion AccountingCompanyDTO

        #region AccountingMovementTypeDTO


        /// <summary>
        /// GetAccountingMovementType
        /// Obtiene un Tipo de movimiento contable
        /// </summary>
        /// <param name="accountingMovementType"></param>
        /// <returns>AccountingMovementTypeDTO</returns>
        [OperationContract]
        AccountingMovementTypeDTO GetAccountingMovementType(AccountingMovementTypeDTO accountingMovementType);

        /// <summary>
        /// GetAccountingMovementTypes
        /// Obtiene listado de Tipo de movimiento contable
        /// </summary>
        /// <returns>List<AccountingMovementTypeDTO/></returns>
        [OperationContract]
        List<AccountingMovementTypeDTO> GetAccountingMovementTypes();

        /// <summary>
        /// GetManualAccountingMovementTypes
        /// Obtiene Tipo de movimiento contable manuales
        /// </summary>
        /// <returns>List<AccountingMovementTypeDTO/></returns>
        [OperationContract]
        List<AccountingMovementTypeDTO> GetManualAccountingMovementTypes();

        #endregion AccountingMovementTypeDTO

        #region AnalysisDTO


        /// <summary>
        /// GetAnalysis
        /// Obtiene Analisis
        /// </summary>
        /// <param name="analysisId"></param>
        /// <returns>AnalysisDTO</returns>
        [OperationContract]
        AnalysisDTO GetAnalysis(int analysisId);

        /// <summary>
        /// GetAnalyses
        /// Obtiene listado de Analisis
        /// </summary>
        /// <returns>List<AnalysisDTO/></returns>
        [OperationContract]
        List<AnalysisDTO> GetAnalyses();

        #endregion AnalysisDTO

        #region AnalysisCodeDTO

        /// <summary>
        /// SaveAnalysisCode
        /// </summary>
        /// <param name="analysisCode"></param>
        /// <returns>AnalysisCodeDTO</returns>
        [OperationContract]
        AnalysisCodeDTO SaveAnalysisCode(AnalysisCodeDTO analysisCode);

        /// <summary>
        /// UpdateAnalysisCode
        /// Actualiza Código de Analisis
        /// </summary>
        /// <param name="analysisCode"></param>
        /// <returns>AnalysisCodeDTO</returns>
        [OperationContract]
        AnalysisCodeDTO UpdateAnalysisCode(AnalysisCodeDTO analysisCode);

        /// <summary>
        /// DeleteAnalysisCode
        /// Borra Código de Analisis
        /// </summary>
        /// <param name="analysisId"></param>
        [OperationContract]
        void DeleteAnalysisCode(int analysisId);

        /// <summary>
        /// GetAnalysisCode
        /// Obtiene Código de Analisis
        /// </summary>
        /// <param name="analysisCodeId"></param>
        /// <returns>AnalysisCodeDTO</returns>
        [OperationContract]
        AnalysisCodeDTO GetAnalysisCode(int analysisCodeId);

        /// <summary>
        /// GetAnalysisCodes
        /// Obtiene listado de Códigos de Analisis
        /// </summary>
        /// <returns>List<AnalysisCodeDTO/></returns>
        [OperationContract]
        List<AnalysisCodeDTO> GetAnalysisCodes();

        #endregion AnalysisCodeDTO

        #region AnalysisTreatmentDTO

        /// <summary>
        /// SaveAnalysisTreatment
        /// Guarda Tratamiento de Analisis
        /// </summary>
        /// <param name="analysisTreatment"></param>
        /// <returns>AnalysisTreatmentDTO</returns>
        [OperationContract]
        AnalysisTreatmentDTO SaveAnalysisTreatment(AnalysisTreatmentDTO analysisTreatment);

        /// <summary>
        /// UpdateAnalysisTreatment
        /// Actualiza Tratamiento de Analisis
        /// </summary>
        /// <param name="analysisTreatment"></param>
        /// <returns>AnalysisTreatmentDTO</returns>
        [OperationContract]
        AnalysisTreatmentDTO UpdateAnalysisTreatment(AnalysisTreatmentDTO analysisTreatment);

        /// <summary>
        /// DeleteAnalysisTreatment
        /// Borra Tratamiento de Analisis
        /// </summary>
        /// <param name="analysisTreatmentId"></param>
        [OperationContract]
        void DeleteAnalysisTreatment(int analysisTreatmentId);

        /// <summary>
        /// GetAnalysisTreatment
        /// Obtiene Tratamiento de análisis
        /// </summary>
        /// <param name="analysisTreatmentId"></param>
        /// <returns>AnalysisTreatmentDTO</returns>
        [OperationContract]
        AnalysisTreatmentDTO GetAnalysisTreatment(int analysisTreatmentId);

        /// <summary>
        /// GetAnalysisTreatments
        /// Obtiene listado de Tratamientos de análisis
        /// </summary>
        /// <returns>List<AnalysisTreatmentDTO/></returns>
        [OperationContract]
        List<AnalysisTreatmentDTO> GetAnalysisTreatments();

        #endregion AnalysisTreatmentDTO

        #region AnalysisConceptDTO

        /// <summary>
        /// SaveAnalysisConcept
        /// Guarda Concepto de Análisis
        /// </summary>
        /// <param name="analysisConcept"></param>
        /// <returns>AnalysisConceptDTO</returns>
        [OperationContract]
        AnalysisConceptDTO SaveAnalysisConcept(AnalysisConceptDTO analysisConcept);

        /// <summary>
        /// UpdateAnalysisConcept
        /// Actualiza Concepto de Análisis
        /// </summary>
        /// <param name="analysisConcept"></param>
        /// <returns>AnalysisConceptDTO</returns>
        [OperationContract]
        AnalysisConceptDTO UpdateAnalysisConcept(AnalysisConceptDTO analysisConcept);

        /// <summary>
        /// DeleteAnalysisConcept
        /// Borra Concepto de Análisis
        /// </summary>
        /// <param name="analysisConceptId"></param>
        [OperationContract]
        void DeleteAnalysisConcept(int analysisConceptId);

        /// <summary>
        /// GetAnalysisConcept
        /// Obtiene Concepto de Análisis
        /// </summary>
        /// <param name="analysisConceptId"></param>
        /// <returns>AnalysisConceptDTO</returns>
        [OperationContract]
        AnalysisConceptDTO GetAnalysisConcept(int analysisConceptId);

        /// <summary>
        /// GetAnalysisConcepts
        /// Obtiene listado de Conceptos de Análisis
        /// </summary>
        /// <returns>List<AnalysisConceptDTO/></returns>
        [OperationContract]
        List<AnalysisConceptDTO> GetAnalysisConcepts();

        #endregion AnalysisConceptDTO

        #region AnalysisConceptKeyDTO

        /// <summary>
        /// SaveAnalysisConceptKey        
        /// </summary>
        /// <param name="analysisConceptKey"></param>
        /// <returns>AnalysisConceptKeyDTO</returns>
        [OperationContract]
        AnalysisConceptKeyDTO SaveAnalysisConceptKey(AnalysisConceptKeyDTO analysisConceptKey);

        /// <summary>
        /// UpdateAnalysisConceptKey        
        /// </summary>
        /// <param name="analysisConceptKey"></param>
        /// <returns>AnalysisConceptKeyDTO</returns>
        [OperationContract]
        AnalysisConceptKeyDTO UpdateAnalysisConceptKey(AnalysisConceptKeyDTO analysisConceptKey);

        /// <summary>
        /// DeleteAnalysisConceptKey
        /// </summary>
        /// <param name="analysisConceptKey"></param>
        [OperationContract]
        void DeleteAnalysisConceptKey(AnalysisConceptKeyDTO analysisConceptKey);

        /// <summary>
        /// GetAnalysisConceptKey
        /// </summary>
        /// <param name="analysisConceptKey"></param>
        /// <returns>AnalysisConceptKeyDTO</returns>
        [OperationContract]
        AnalysisConceptKeyDTO GetAnalysisConceptKey(AnalysisConceptKeyDTO analysisConceptKey);

        /// <summary>
        /// GetAnalysisConceptKeys
        /// </summary>
        /// <returns>List<AnalysisConceptKeyDTO/></returns>
        [OperationContract]
        List<AnalysisConceptKeyDTO> GetAnalysisConceptKeys();

        /// <summary>
        /// GetAnalysisConceptKeysByAnalysisConcept
        /// </summary>
        /// <param name="analysisConcept"></param>
        /// <returns>List<AnalysisConceptKeyDTO/></returns>
        [OperationContract]
        List<AnalysisConceptKeyDTO> GetAnalysisConceptKeysByAnalysisConcept(AnalysisConceptDTO analysisConcept);

        #endregion

        #region AnalysisConceptAnalysis

        /// <summary>
        /// SaveAnalysisConceptAnalysis
        /// Graba relación Analisis y Concepto de Analisis
        /// </summary>
        /// <param name="analysisId"></param>
        /// <param name="analysisConceptId"></param>
        [OperationContract]
        void SaveAnalysisConceptAnalysis(int analysisId, int analysisConceptId);

        /// <summary>
        /// DeleteAnalysisConceptAnalysis
        /// Borra relación Analisis y Concepto de Analisis
        /// </summary>
        /// <param name="analysisConceptAnalysisId"></param>
        [OperationContract]
        void DeleteAnalysisConceptAnalysis(int analysisConceptAnalysisId);

        /// <summary>
        /// GetPaymentConceptsByAnalysisCode
        /// Obtiene los conceptos de pago a partir del Analisis
        /// </summary>
        /// <param name="analysisId"></param>
        /// <returns>List<AnalysisConceptAnalysisDTO/></returns>
        [OperationContract]
        List<AnalysisConceptAnalysisDTO> GetPaymentConceptsByAnalysisCode(int analysisId);

        /// <summary>
        /// GetRemainingAnalysisConcepts
        /// Obtiene los conceptos de pago que aún no han sido relacionados con el Analisis
        /// </summary>
        /// <param name="analysisCodeId"></param>
        /// <returns>List<AnalysisConceptDTO/></returns>
        [OperationContract]
        List<AnalysisConceptDTO> GetRemainingAnalysisConcepts(int analysisCodeId);

        #endregion AnalysisConceptAnalysis

        #region MovementTypeDTO


        /// <summary>
        /// SaveReconciliationMovementType
        /// </summary>
        /// <param name="reconciliationMovementType"></param>
        [OperationContract]
        void SaveReconciliationMovementType(ReconciliationMovementTypeDTO reconciliationMovementType);

        /// <summary>
        /// UpdateReconciliationMovementType
        /// </summary>
        /// <param name="reconciliationMovementType"></param>
        [OperationContract]
        void UpdateReconciliationMovementType(ReconciliationMovementTypeDTO reconciliationMovementType);

        /// <summary>
        /// DeleteReconciliationMovementType
        /// </summary>
        /// <param name="reconciliationMovementType"></param>
		/// <returns>bool</returns>
        [OperationContract]
        bool DeleteReconciliationMovementType(ReconciliationMovementTypeDTO reconciliationMovementType);

        /// <summary>
        /// GetReconciliationMovementTypes
        /// </summary>        
        /// <returns>List<ReconciliationMovementTypeDTO/></returns>
        [OperationContract]
        List<ReconciliationMovementTypeDTO> GetReconciliationMovementTypes();

        #endregion BankReconciliation

        #region CostCenterDTO

        /// <summary>
        /// SaveCostCenter
        /// Guarda Centro de costos
        /// </summary>
        /// <param name="costCenter"></param>
        /// <returns>CostCenterDTO</returns>
        [OperationContract]
        CostCenterDTO SaveCostCenter(CostCenterDTO costCenter);

        /// <summary>
        /// UpdateCostCenter
        /// Actualiza Centro de costos
        /// </summary>
        /// <param name="costCenter"></param>
        /// <returns>CostCenterDTO</returns>
        [OperationContract]
        CostCenterDTO UpdateCostCenter(CostCenterDTO costCenter);

        /// <summary>
        /// DeleteCostCenter
        /// Borra Centro de costos
        /// </summary>
        /// <param name="costCenterId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteCostCenter(int costCenterId);

        /// <summary>
        /// GetCostCenter
        /// Obtiene Centro de costos
        /// </summary>
        /// <param name="costCenter"></param>
        /// <returns>CostCenterDTO</returns>
        [OperationContract]
        CostCenterDTO GetCostCenter(CostCenterDTO costCenter);

        /// <summary>
        /// GetCostCenters
        /// Obtiene listado de Centros de costos
        /// </summary>
        /// <returns>List<CostCenterDTO/></returns>
        [OperationContract]
        List<CostCenterDTO> GetCostCenters();

        #endregion CostCenterDTO

        #region CostCenterTypeDTO

        /// <summary>
        /// SaveCostCenterType
        /// Guarda Tipo de centro de costos
        /// </summary>
        /// <param name="costCenterType"></param>
        [OperationContract]
        void SaveCostCenterType(CostCenterTypeDTO costCenterType);

        /// <summary>
        /// UpdateCostCenterType
        /// Actualiza Tipo de centro de costos
        /// </summary>
        /// <param name="costCenterType"></param>
        [OperationContract]
        void UpdateCostCenterType(CostCenterTypeDTO costCenterType);

        /// <summary>
        /// DeleteCostCenterType
        /// Borra Tipo de centro de costos
        /// </summary>
        /// <param name="costCenterTypeId"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteCostCenterType(int costCenterTypeId);

        /// <summary>
        /// GetCostCenterTypeById
        /// Obtiene Tipo de centro de costos
        /// </summary>
        /// <param name="costCenterTypeId"></param>
        /// <returns>CostCenterTypeDTO</returns>
        [OperationContract]
        CostCenterTypeDTO GetCostCenterTypeById(int costCenterTypeId);

        /// <summary>
        /// GetCostCenterTypes
        /// Obtiene listado de Tipos de centro de costos
        /// </summary>
        /// <returns>List<CostCenterTypeDTO/></returns>
        [OperationContract]
        List<CostCenterTypeDTO> GetCostCenterTypes();

        #endregion CostCenterTypeDTO

        #region Destination

        /// <summary>
        /// SaveEntryDestination
        /// Guarda Destino
        /// </summary>
        /// <param name="entryDestination"></param>
        /// <returns>EntryDestinationDTO</returns>
        [OperationContract]
        EntryDestinationDTO SaveEntryDestination(EntryDestinationDTO entryDestination);

        /// <summary>
        /// UpdateEntryDestination
        /// Actualiza Destino
        /// </summary>
        /// <param name="entryDestination"></param>
        /// <returns>EntryDestinationDTO</returns>
        [OperationContract]
        EntryDestinationDTO UpdateEntryDestination(EntryDestinationDTO entryDestination);

        /// <summary>
        /// DeleteEntryDestination
        /// Borra Destino
        /// </summary>
        /// <param name="entryDestinationId"></param>
        [OperationContract]
        void DeleteEntryDestination(int entryDestinationId);

        /// <summary>
        /// GetDestination
        /// Obtiene un Destino
        /// </summary>
        /// <param name="entryDestination"></param>
        /// <returns>EntryDestinationDTO</returns>
        [OperationContract]
        EntryDestinationDTO GetDestination(EntryDestinationDTO entryDestination);

        /// <summary>
        /// GetEntryDestinations
        /// Obtiene listado de Destinos
        /// </summary>
        /// <returns>List<EntryDestinationDTO/></returns>
        [OperationContract]
        List<EntryDestinationDTO> GetEntryDestinations();

        #endregion Destination

        #region EntryTypeDTO

        /// <summary>
        /// SaveEntryType
        /// Guarda Asiento Tipo
        /// </summary>
        /// <param name="entryType"></param>
        /// <returns>EntryTypeDTO</returns>
        [OperationContract]
        EntryTypeDTO SaveEntryType(EntryTypeDTO entryType);

        /// <summary>
        /// UpdateEntryType
        /// Actualiza Asiento Tipo
        /// </summary>
        /// <param name="entryType"></param>
        [OperationContract]
        void UpdateEntryType(EntryTypeDTO entryType);

        /// <summary>
        /// DeleteEntryTypeRequest
        /// Borra Asiento Tipo
        /// </summary>
        /// <param name="entryTypeId"></param>
        [OperationContract]
        void DeleteEntryTypeRequest(int entryTypeId);

        /// <summary>
        /// GetEntryType
        /// Obtiene Asiento Tipo
        /// </summary>
        /// <param name="entryType"></param>
        /// <returns>EntryTypeDTO</returns>
        [OperationContract]
        EntryTypeDTO GetEntryType(EntryTypeDTO entryType);

        /// <summary>
        /// GetEntryTypes
        /// Obtiene listado de Asientos Tipo
        /// </summary>
        /// <returns>List<EntryTypeDTO/></returns>
        [OperationContract]
        List<EntryTypeDTO> GetEntryTypes();

        #endregion EntryTypeDTO

        #region EntryTypeItem

        /// <summary>
        /// DeleteEntryTypeAccounting
        /// Borra Cuenta contable relacionada al Asiento tipo
        /// </summary>
        /// <param name="entryTypeAccountingId"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteEntryTypeAccounting(int entryTypeAccountingId);

        #endregion EntryTypeItem

        #endregion PARAM

        #region EntryPosting

        /// <summary>
        /// SaveProcessEntries
        /// Guarda agrupando por cuenta contable, naturaleza del movimiento, etc
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <param name="isClosure"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveProcessEntries(int year, int month, int userId, DateTime date, int isClosure);

        #endregion EntryPosting

        #region Reports

        /// <summary>
        /// GetDailyEntriesByDate
        /// Obtiene Asientos de diario por fecha 
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <returns>List<EntryDTO/></returns>
        [OperationContract]
        List<EntryDTO> GetDailyEntriesByRangeDateBranchId(DateTime dateFrom, DateTime dateTo, int branchId);

        #region EntryReport

        /// <summary>
        /// GetEntriesByDate
        /// Obtiene Asientos por fecha
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <returns>List<EntryDTO/></returns>
        [OperationContract]
        List<EntryDTO> GetEntriesByDate(DateTime dateFrom, DateTime dateTo, int branchId);

        #endregion EntryReport

        #endregion

        #region BalanceCheking

        /// <summary>
        /// GetBalanceCheckingDTO
        /// Obtiene el balance de cuentas
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>List<BalanceCheckingDTO/></returns>
        [OperationContract]
        List<BalanceCheckingDTO> GetBalanceCheckingDTO(DateTime dateFrom, DateTime dateTo);

        #endregion BalanceCheking

        #region Closing

        /// <summary>
        /// IncomeOutcomeClosing
        /// Cierre de Ingresos y Egresos
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int IncomeOutcomeClosing(int year, int userId);

        /// <summary>
        /// MonthlyIncomeClosing
        /// Cierre de utilidad mensual
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <param name="month"></param>
        /// <returns>int</returns>
        [OperationContract]
        int MonthlyIncomeClosing(int year, int userId, int month);

        /// <summary>
        /// AssetAndLiabilityOpening
        /// Asiento de Apertura de Activos y Pasivos
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int AssetAndLiabilityOpening(int year, int userId);

        /// <summary>
        /// RevertAnualEntryOpening
        /// Revesión de Asiento Anual de Apertura
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int RevertAnualEntryOpening(int year, int userId);

        /// <summary>
        /// RevertIncomeOutcomeClosing
        /// Reversar Cierre Anual de Ingresos y Egresos
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int RevertIncomeOutcomeClosing(int year, int userId);

        #endregion Closing

        #region EntryMassiveLoad

        /// <summary>
        /// SaveEntryMassiveLoadRequest
        /// </summary>
        /// <param name="massiveEntryDTO"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool SaveEntryMassiveLoadRequest(MassiveEntryDTO massiveEntryDTO);

        /// <summary>
        /// ClearEntryMassiveLoad
        /// </summary>
        [OperationContract]
        void ClearEntryMassiveLoad();

        /// <summary>
        /// DisableEntryMassiveLoadLog
        /// Deshabilita todos los registros de la tabla de log de asientos masivos.
        /// </summary>
        [OperationContract]
        void DisableEntryMassiveLoadLog();

        /// <summary>
        /// SaveEntryMassiveLoadLogRequest
        /// graba el regsitro en la tabla de log
        /// </summary>
        /// <param name="massiveEntryLogDTO"></param>
        /// <returns>MassiveEntryLogDTO</returns>
        [OperationContract]
        MassiveEntryLogDTO SaveEntryMassiveLoadLogRequest(MassiveEntryLogDTO massiveEntryLogDTO);

        /// <summary>
        /// GetEntryMassiveLoadRecords
        /// Obtienes los totales del proceso masivo de asiento
        /// </summary>
        /// <returns>EntryMassiveLoadResultDTO</returns>
        [OperationContract]
        EntryMassiveLoadResultDTO GetEntryMassiveLoadRecords();

        /// <summary>
        /// GetMassiveEntryFailedRecords
        /// Obtiene el listado de registros fallidos para mostrarlo en pantalla
        /// </summary>
        /// <returns>List<MassiveEntryLogDTO/></returns>
        [OperationContract]
        List<MassiveEntryLogDTO> GetMassiveEntryFailedRecords();

        /// <summary>
        /// GenerateEntry
        /// Genera y graba el asiento desde carga masiva
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int GenerateEntry(int userId);

        #endregion EntryMassiveLoad

        #region GetDinamicAccountingAccount

        /// <summary>
        /// Obtiene el listado de operadores
        /// </summary>
        /// <returns>List<OperatorDTO/></returns>
        [OperationContract]
        List<OperatorDTO> GetOperators();

        #endregion GetDinamicAccountingAccount

        #region TempEntry

        /// <summary>
        /// Borra la tabla temporal de generación de asientos.
        /// </summary>
        [OperationContract]
        void ClearTempAccountEntry();

        /// <summary>
        /// Graba los movimientos del asiento en una tabla temporal
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <param name="transactionNumber"></param>
        /// <param name="isJournalEntry"></param>
        /// <param name="userId"></param>
        [OperationContract]
        void SaveTempEntryItem(LedgerEntryDTO ledgerEntry, int transactionNumber, bool isJournalEntry, int userId);

        /// <summary>
        /// Método para generar el asiento en el cierre de reaseguros.
        /// </summary>
        /// <param name="transactionNumber"></param>
        /// <param name="sourceId"></param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveTempEntry(int transactionNumber, int sourceId, string description, int userId);

        #endregion TempEntry

        #region AccountReclassificationDTO

        /// <summary>
        /// SaveAccountReclassification
        /// </summary>
        /// <param name="accountReclassification"></param>
        /// <returns>AccountReclassificationDTO</returns>
        [OperationContract]
        AccountReclassificationDTO SaveAccountReclassification(AccountReclassificationDTO accountReclassification);

        /// <summary>
        /// UpdateAccountReclassification
        /// </summary>
        /// <param name="accountReclassification"></param>
        /// <returns>AccountReclassificationDTO</returns>
        [OperationContract]
        AccountReclassificationDTO UpdateAccountReclassification(AccountReclassificationDTO accountReclassification);

        /// <summary>
        /// DeleteAccountReclassification
        /// </summary>
        /// <param name="accountReclassification"></param>
        [OperationContract]
        void DeleteAccountReclassification(AccountReclassificationDTO accountReclassification);

        /// <summary>
        /// GetAccountReclassification
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns> List<AccountReclassificationDTO/></returns>
        [OperationContract]
        List<AccountReclassificationDTO> GetAccountReclassification(int month, int year);

        /// <summary>
        /// GenerateEntryReclassification
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>int</returns>
        [OperationContract]
        int GenerateEntryReclassification(int month, int year);

        /// <summary>
        /// SaveAccountReclassificationResult
        /// </summary>
        /// <param name="accountReclassificationResult"></param>
        /// <returns>AccountReclassificationResultDTO</returns>
        [OperationContract]
        AccountReclassificationResultDTO SaveAccountReclassificationResult(AccountReclassificationResultDTO accountReclassificationResult);

        /// <summary>
        /// GetAccountReclassificationResult
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>List<AccountReclassificationResultDTO/></returns>
        [OperationContract]
        List<AccountReclassificationResultDTO> GetAccountReclassificationResults(int month, int year);

        #endregion

        #region ProcessLogDTO

        /// <summary>
        /// SaveProcessLog
        /// </summary>
        /// <param name="processLog"></param>
        /// <returns>ProcessLogDTO</returns>
        [OperationContract]
        ProcessLogDTO SaveProcessLog(ProcessLogDTO processLog);

        /// <summary>
        /// UpdateProcessLog
        /// </summary>
        /// <param name="processLog"></param>
        /// <returns>ProcessLogDTO</returns>
        [OperationContract]
        ProcessLogDTO UpdateProcessLog(ProcessLogDTO processLog);

        /// <summary>
        /// GetProcessLog
        /// </summary>
        /// <param name="processLog"></param>
        /// <returns>ProcessLogDTO</returns>
        [OperationContract]
        ProcessLogDTO GetProcessLog(ProcessLogDTO processLog);

        #endregion

        #region LedgerEntryDTO

        /// <summary>
        /// SaveLedgerEntry
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveLedgerEntry(LedgerEntryDTO ledgerEntry);

        /// <summary>
        /// ReverseLedgerEntry
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ReverseLedgerEntry(LedgerEntryDTO ledgerEntry);

        /// <summary>
        /// SaveEntryTypeAccounting
        /// </summary>
        /// <param name="ledgerEntryItem"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveLedgerEntryItem(LedgerEntryItemDTO ledgerEntryItem);

        /// <summary>
        /// UpdateEntryTypeAccounting
        /// </summary>
        /// <param name="ledgerEntryItem"></param>
        /// <returns>int</returns>
        [OperationContract]
        int UpdateLedgerEntryItem(LedgerEntryItemDTO ledgerEntryItem);

        /// <summary>
        /// GetLedgerEntries
        /// </summary>
        /// <param name="ledgerEntryId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="destinationId"></param>
        /// <param name="accountingMovementTypeId"></param>
        /// <returns>List<LedgerEntryDTO/></returns>
        [OperationContract]
        List<LedgerEntryDTO> GetLedgerEntries(int ledgerEntryId, DateTime dateFrom, DateTime dateTo, int branchId, int destinationId, int accountingMovementTypeId);

        #endregion

        #region JournalEntryDTO

        /// <summary>
        /// SaveJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>int</returns>
        [OperationContract]
        int SaveJournalEntry(JournalEntryDTO journalEntry);

        /// <summary>
        /// ReverseLedgerEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ReverseJournalEntry(JournalEntryDTO journalEntry, int NewTechnicalTransaction = 0);

        /// <summary>
        /// GetJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>JournalEntryDTO</returns>
        [OperationContract]
        JournalEntryDTO GetJournalEntry(JournalEntryDTO journalEntry);

        /// <summary>
        /// GetJournalEntries
        /// </summary>
        /// <param name="journalEntryId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>        
        /// <returns>List<LedgerEntryDTO/></returns>
        [OperationContract]
        List<JournalEntryDTO> GetJournalEntries(int journalEntryId, DateTime dateFrom, DateTime dateTo, int branchId);

        /// <summary>
        /// GetJournalEntryReversion
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns></returns>
        [OperationContract]
        JournalEntryDTO GetJournalEntryReversion(JournalEntryDTO journalEntry);

        /// <summary>
        /// GetJournalEntryItemsByTechnicalTransaction
        /// </summary>
        /// <param name="technicalTransaction"></param>
        /// <returns></returns>
        [OperationContract]
        JournalEntryDTO GetJournalEntryItemsByTechnicalTransaction(int technicalTransaction);

        [OperationContract]
        List<JournalEntryItemDTO> GetJournalEntryItemsBySourceCode(int sourceCode);
        #endregion

        #region AccountingConcepts

        #region ConceptSourceDTO

        /// <summary>
        /// SaveConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns>ConceptSourceDTO</returns>
        [OperationContract]
        ConceptSourceDTO SaveConceptSource(ConceptSourceDTO conceptSource);

        /// <summary>
        /// UpdateConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns>ConceptSourceDTO</returns>
        [OperationContract]
        ConceptSourceDTO UpdateConceptSource(ConceptSourceDTO conceptSource);

        /// <summary>
        /// DeleteConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
		/// <returns>bool</returns>
        [OperationContract]
        bool DeleteConceptSource(ConceptSourceDTO conceptSource);

        /// <summary>
        /// GetConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns>ConceptSourceDTO</returns>
        [OperationContract]
        ConceptSourceDTO GetConceptSource(ConceptSourceDTO conceptSource);

        /// <summary>
        /// GetConceptSources
        /// </summary>
        /// <returns>List<ConceptSourceDTO/></returns>
        [OperationContract]
        List<ConceptSourceDTO> GetConceptSources();

        #endregion

        #region MovementTypeDTO

        /// <summary>
        /// SaveMovementType
        /// </summary>
        /// <param name="movementType"></param>
        /// <returns>MovementTypeDTO</returns>
        [OperationContract]
        MovementTypeDTO SaveMovementType(MovementTypeDTO movementType);

        /// <summary>
        /// UpdateMovementType
        /// </summary>
        /// <param name="movementType"></param>
        /// <returns>MovementTypeDTO</returns>
        [OperationContract]
        MovementTypeDTO UpdateMovementType(MovementTypeDTO movementType);

        /// <summary>
        /// DeleteMovementType
        /// </summary>
        /// <param name="movementType"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteMovementType(MovementTypeDTO movementType);

        /// <summary>
        /// GetMovementType
        /// </summary>
        /// <param name="movementType"></param>
        /// <returns>MovementTypeDTO</returns>      
        [OperationContract]
        MovementTypeDTO GetMovementType(MovementTypeDTO movementType);

        /// <summary>
        /// GetMovementTypes
        /// </summary>
        /// <returns>List<MovementTypeDTO/></returns>
        [OperationContract]
        List<MovementTypeDTO> GetMovementTypes();

        /// <summary>
        /// GetMovementTypesByConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns>List<MovementTypeDTO/></returns>
        [OperationContract]
        List<MovementTypeDTO> GetMovementTypesByConceptSource(ConceptSourceDTO conceptSource);

        /// <summary>
        /// GetMovementTypesByConceptSourceFilter
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns>List<MovementTypeDTO/></returns>
        [OperationContract]
        List<MovementTypeDTO> GetMovementTypesByConceptSourceFilter(ConceptSourceDTO conceptSource);

        #endregion

        #region AccountingConceptDTO

        /// <summary>
        /// SaveAccountingConcept
        /// </summary>
        /// <param name="accountingConcept"></param>
        /// <returns>AccountingConceptDTO</returns>
        [OperationContract]
        AccountingConceptDTO SaveAccountingConcept(AccountingConceptDTO accountingConcept);


        /// <summary>
        /// UpdateAccountingConcept
        /// </summary>
        /// <param name="accountingConcept"></param>
        /// <returns>AccountingConceptDTO</returns>
        [OperationContract]
        AccountingConceptDTO UpdateAccountingConcept(AccountingConceptDTO accountingConcept);

        /// <summary>
        /// DeleteAccountingConcept
        /// </summary>
        /// <param name="accountingConcept"></param>
        /// <returns>bool</returns>   
        [OperationContract]
        bool DeleteAccountingConcept(AccountingConceptDTO accountingConcept);

        /// <summary>
        /// GetAccountingConcept
        /// </summary>
        /// <param name="accountingConcept"></param>
        /// <returns>AccountingConceptDTO</returns>     
        [OperationContract]
        AccountingConceptDTO GetAccountingConcept(AccountingConceptDTO accountingConcept);


        /// <summary>
        /// GetAccountingConcepts
        /// </summary>        
        /// <returns>List<AccountingConceptDTO/></returns>        
        [OperationContract]
        List<AccountingConceptDTO> GetAccountingConcepts();

        /// <summary>
        /// GetAccountingConceptsByCriteria
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="branchId"></param>
        /// <param name="individualId"></param>
        /// <returns>List<AccountingConceptDTO/></returns>
        [OperationContract]
        List<AccountingConceptDTO> GetAccountingConceptsByCriteria(int userId, int branchId, int individualId);

        /// <summary>
        /// Obtiene la lista de conceptos de pago de una sucursal
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        [OperationContract]
        List<AccountingConceptDTO> GetAccountingConceptsByBranchId(int branchId);

        #endregion

        #region BranchAccountingConceptDTO

        /// <summary>
        /// SaveBranchAccountingConcept
        /// </summary>
        /// <param name="branchAccountingConcept"></param>
        /// <returns>BranchAccountingConceptDTO</returns>
        [OperationContract]
        BranchAccountingConceptDTO SaveBranchAccountingConcept(BranchAccountingConceptDTO branchAccountingConcept);

        /// <summary>
        /// UpdateBranchAccountingConcept
        /// </summary>
        /// <param name="branchAccountingConcept"></param>
        /// <returns>BranchAccountingConceptDTO</returns>
        [OperationContract]
        BranchAccountingConceptDTO UpdateBranchAccountingConcept(BranchAccountingConceptDTO branchAccountingConcept);

        /// <summary>
        /// DeleteBranchAccountingConcept
        /// </summary>
        /// <param name="branchAccountingConcept"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteBranchAccountingConcept(BranchAccountingConceptDTO branchAccountingConcept);

        /// <summary>
        /// GetBranchAccountingConcept
        /// </summary>
        /// <param name="branchAccountingConcept"></param>
        /// <returns>BranchAccountingConceptDTO</returns>
        [OperationContract]
        BranchAccountingConceptDTO GetBranchAccountingConcept(BranchAccountingConceptDTO branchAccountingConcept);

        /// <summary>
        /// GetBranchAccountingConcepts
        /// </summary>
        /// <returns> List<BranchAccountingConceptDTO/></returns>
        [OperationContract]
        List<BranchAccountingConceptDTO> GetBranchAccountingConcepts();

        /// <summary>
        /// GetBranchAccountingConceptByBranch
        /// </summary>
        /// <param name="branch"></param>
        /// <returns>List<BranchAccountingConceptDTO/></returns>
        [OperationContract]
        List<BranchAccountingConceptDTO> GetBranchAccountingConceptByBranch(BranchDTO branch);

        #endregion

        #region UserBranchAccountingConceptDTO

        /// <summary>
        /// SaveUserBranchAccountingConcept
        /// </summary>
        /// <param name="userBranchAccountingConcept"></param>
        /// <returns>UserBranchAccountingConceptDTO</returns>
        [OperationContract]
        UserBranchAccountingConceptDTO SaveUserBranchAccountingConcept(UserBranchAccountingConceptDTO userBranchAccountingConcept);

        /// <summary>
        /// UpdateUserBranchAccountingConcept
        /// </summary>
        /// <param name="userBranchAccountingConcept"></param>
        /// <returns>UserBranchAccountingConceptDTO</returns>
        [OperationContract]
        UserBranchAccountingConceptDTO UpdateUserBranchAccountingConcept(UserBranchAccountingConceptDTO userBranchAccountingConcept);

        /// <summary>
        /// DeleteUserBranchAccountingConcept
        /// </summary>
        /// <param name="userBranchAccountingConcept"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteUserBranchAccountingConcept(UserBranchAccountingConceptDTO userBranchAccountingConcept);

        /// <summary>
        /// GetUserBranchAccountingConcept
        /// </summary>
        /// <param name="userBranchAccountingConcept"></param>
        /// <returns>UserBranchAccountingConceptDTO</returns>
        [OperationContract]
        UserBranchAccountingConceptDTO GetUserBranchAccountingConcept(UserBranchAccountingConceptDTO userBranchAccountingConcept);

        /// <summary>
        /// GetUserBranchAccountingConcepts
        /// </summary>
        /// <returns>List<UserBranchAccountingConceptDTO/></returns>
        [OperationContract]
        List<UserBranchAccountingConceptDTO> GetUserBranchAccountingConcepts();

        /// <summary>
        /// GetUserBranchAccountingConceptByUserByBranch
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="branch"></param>
        /// <returns>List<UserBranchAccountingConceptDTO/></returns>
        [OperationContract]
        List<UserBranchAccountingConceptDTO> GetUserBranchAccountingConceptByUserByBranch(int userId, BranchDTO branch);

        #endregion

        #endregion

        #region AutomaticLedgerEntry

        /// <summary>
        /// SaveAutomaticLedgerEntry
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <param name="date"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        
        [OperationContract]
        int SaveAutomaticLedgerEntry(int moduleDateId, DateTime date, int userId);

        #endregion

        #region Accounting

        /// <summary>
        /// Accounting Metodo Temporal para unificar desarrollo CONTABILIZAR
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <param name="parameters"></param>
        /// <param name="journalEntry"></param>
        /// <returns>int</returns>
        [OperationContract]
        int Accounting(int moduleDateId, List<List<DTOs.AccountingRules.ParameterDTO>> parameters, JournalEntryDTO journalEntry);

        /// <summary>
        /// Accounting Metodo Temporal para unificar desarrollo CONTABILIZAR
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <param name="parameters"></param>
        /// <param name="journalEntry"></param>
        /// <returns>int</returns>
        [OperationContract]
        int AccountingWithoutTransactional(string accountingJournalEntryParametersCollection);

        /// <summary>
        /// contabilidad para el rechazo de cheuques
        /// </summary>
        /// <param name="accountingJournalEntryParametersCollection"></param>
        /// <param name="codeParameter"></param>
        /// <returns></returns>
        [OperationContract]
        int AccountingChecking(string accountingJournalEntryParametersCollection);

        /// <summary>
        /// AccountingPaymentBallot
        /// </summary>
        /// <param name="accountingJournalEntryParametersCollection"></param>
        /// <returns></returns>
        [OperationContract]
        int AccountingPaymentBallot(string accountingJournalEntryParametersCollection);

        /// <summary>
        /// realiza la reversion de contabilidad
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <param name="parameters"></param>
        /// <param name="journalEntry"></param>
        /// <returns>int</returns>
        [OperationContract]
        int GetJournalEntryTechnicalTransaction(int technicalTransaction, int technicalTransactionRevertion);

        /// <summary>
        /// CreateAccountingTransactionalItems
        /// </summary>
        /// <param name="accountingJournalEntryParametersCollection"></param>
        /// <param name="codeRulePackage"></param>
        /// <returns></returns>
        [OperationContract]
        int CreateAccountingTransactionalItems(string accountingJournalEntryParametersCollection, string codeRulePackage);

        #endregion

        #region New Accounting Concepts
        /// <summary>
        /// Devuelve el número de cuenta a partir del código del concepto
        /// </summary>
        /// <param name="parameters">Parámetros necesarios para la consulta</param>
        /// <returns>Número de cuenta</returns>
        [OperationContract]
        AccountingAccountDTO GetAccountingNumberByAccountingConcept(AccountingParameterDTO parameters);
        #endregion

        /// <summary>
        /// GetAccountingConceptsByCriteria
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>List of accounting concepts</returns>
        [OperationContract]
        List<AccountingConceptDTO> GetAccountingConceptsByFilter(AccountingAccountFilterDTO filter);

        /// <summary>
        /// Saves journal entry
        /// </summary>
        /// <param name="accountingJournalEntryParametersCollection">Parameters</param>
        /// <returns></returns>
        [OperationContract]
        int SaveGenericJournalEntry(string accountingJournalEntryParametersCollection);

        /// <summary>
        /// Get accounting concepts by filter
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="branchId">Branch identifier</param>
        /// <param name="individualId">Individual Identifier</param>
        /// <returns>Accounting concepts</returns>
        [OperationContract]
        List<AccountingConceptDTO> GetAccountingConceptsByUserIdBranchIdIndividualId(int userId, int branchId, int individualId);

        /// <summary>
        /// Get accounting concepts
        /// </summary>
        /// <returns>Accounting concepts</returns>
        [OperationContract]
        List<AccountingConceptDTO> GetLiteAccountingConcepts();

        /// <summary>
        /// Save Journal Entry
        /// </summary>
        /// <param name="accountingJournalEntryParametersCollection"></param>
        /// <returns>New journal entry id</returns>
        [OperationContract]
        int SaveBasicJournalEntry(string accountingJournalEntryParametersCollection);

        /// <summary>
        /// Reverse journal entry
        /// </summary>
        /// <param name="reverseParameters">Parameters</param>
        /// <returns>The new journal entry id</returns>
        [OperationContract]
        int ReverseBasicJournalEntry(string reverseParameters);
    }
}
