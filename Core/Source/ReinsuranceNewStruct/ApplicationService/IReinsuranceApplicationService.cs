using Sistran.Core.Application.ReinsuranceServices.DTOs;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.ReinsuranceServices
{
    [ServiceContract]
    public interface IReinsuranceApplicationService
    {

        #region Param

        #region Contract

        #region Contract

        /// <summary>
        /// SaveContract
        /// Graba un registro del Contrato
        /// </summary>
        /// <param name="contract"></param>
        [OperationContract]
        int SaveContract(ContractDTO contract);

        /// <summary>
        /// UpdateContract
        /// Actualiza un registro del Contrato
        /// </summary>
        /// <param name="contract"></param>
        [OperationContract]
        void UpdateContract(ContractDTO contract);

        /// <summary>
        /// DeleteContract
        /// Elimina un registro del Contrato dado como parámetro el id 
        /// </summary>
        /// <param name="contractId"></param>
        [OperationContract]
        void DeleteContract(int contractId);

        /// <summary>
        /// GetContractById
        /// Obtiene un registro dado como parámetro el id de contrato
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns>ContractDTO</returns>
        [OperationContract]
        ContractDTO GetContractById(int contractId);


        /// <summary>
        /// GetContractsByYearAndContractTypeId
        /// Obtiene los rContratos en base al año y tipo 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="contractTypeId"></param>
        /// <returns>List<Contract/></returns>
        [OperationContract]
        List<ContractDTO> GetContractsByYearAndContractTypeId(int year, int contractTypeId);

        /// <summary>
        /// GetEnabledContracts
        /// </summary>
        /// <returns> List<ContractDTO/></returns>
        [OperationContract]
        List<ContractDTO> GetEnabledContracts();

        /// <summary>
        /// ValidateContractIssueAllocation
        /// Valida si un contrato ya tiene una repartición de valores 
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [OperationContract]
        bool ValidateContractIssueAllocation(int contractId);

        /// <summary>
        ///  Obtiene el contrato por tipo de contrato
        /// </summary>
        /// <param name="contractTypeId"></param>
        /// <returns></returns>
        [OperationContract]
        ContractTypeDTO GetContractTypeByContractTypeId(int contractTypeId);

        /// <summary>
        /// Obtiene los contratos activos por grupo de contrato
        /// </summary>
        /// <param name="groupContract"></param>
        /// <returns></returns>

        [OperationContract]
        List<ContractDTO> GetEnabledContractsByGroupContract(string groupContract);

        /// <summary>
        /// Valida si existe el contrato duplicado
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool ValidateDuplicateContract(ContractDTO contractDTO);

        /// <summary>
        /// Copia un contrato 
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="smallDescription"></param>
        /// <param name="year"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        bool CopyContract(int contractId, string smallDescription, int year, string description);

        /// <summary>
        /// Valida si el contracto esta parametrizado correctamente
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [OperationContract]
        bool ValidateCompleteContract(int contractId);

        #region ContractType

        /// <summary>
        /// GetContractTypes
        /// Obtiene todos los tipos de contrato de la tabla REINS.CONTRACT_TYPE
        /// </summary>
        /// <returns>List<ContractTypeDTO></returns>
        [OperationContract]
        List<ContractTypeDTO> GetContractTypes();
        #endregion ContractType

        #endregion Contract

        #region Level

        /// <summary>
        /// SaveLayer
        /// Graba un registro en Nivel/Capa de Contrato
        /// </summary>
        /// <param name="layer"></param>
        [OperationContract]
        int SaveLevel(LevelDTO levelDTO);

        /// <summary>
        /// UpdateLayer
        /// Actualiza una capa del Contrato
        /// </summary>
        /// <param name="layer"></param>
        [OperationContract]
        void UpdateLevel(LevelDTO levelDTO);

        /// <summary>
        /// DeleteLayer
        /// Elimina un registro de capa dado como parámetro el id de nivel de contrato
        /// </summary>
        /// <param name="layerId"></param>
        [OperationContract]
        void DeleteLevel(int levelId);

        /// <summary>
        /// GetLevelByLevelId
        /// Obtiene un registro de capa dado como parámetro el id de nivel de contrato
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns>LevelDTO</returns>
        [OperationContract]
        LevelDTO GetLevelByLevelId(int levelId);

        /// <summary>
        /// GetLevelsByContractId
        /// Obtiene las capas dado el id de contrato
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns>List<LevelDTO><Level/></returns>
        [OperationContract]
        List<LevelDTO> GetLevelsByContractId(int contractId);

        /// <summary>
        /// GetLevelNumberByContractId
        /// Obtiene el número de nivel secuencial dado el id. del contrato
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int GetLevelNumberByContractId(int contractId);


        #region Payments

        /// <summary>
        ///SaveLevelPayment
        /// </summary>
        /// <param name="levelPayment"></param>   
        [OperationContract]
        void SaveLevelPayment(LevelPaymentDTO levelPayment);

        /// <summary>
        ///UpdateLevelPayment
        /// </summary>
        /// <param name="levelPayment"></param>
        [OperationContract]
        void UpdateLevelPayment(LevelPaymentDTO levelPayment);

        /// <summary>
        ///DeleteLevelPayment
        /// </summary>
        /// <param name="levelPaymentId"></param>
        [OperationContract]
        void DeleteLevelPayment(int levelPaymentId);

        /// <summary>
        ///GetLevelPayment
        /// </summary>
        /// <param name="levelPaymentId"></param>
        /// <returns>LevelPayment</returns>
        [OperationContract]
        LevelPaymentDTO GetLevelPayment(int levelPaymentId);

        /// <summary>
        ///GetLevelPaymentsByLevelId
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns>List<LevelPayment/></returns>
        [OperationContract]
        List<LevelPaymentDTO> GetLevelPaymentsByLevelId(int levelId);
        #endregion

        #region Restore

        /// <summary>
        ///SaveLevelRestore
        /// </summary>
        /// <param name="levelRestore"></param>  
        [OperationContract]
        void SaveLevelRestore(LevelRestoreDTO levelRestore);

        /// <summary>
        ///UpdateLevelRestore
        /// </summary>
        /// <param name="levelRestore"></param> 
        [OperationContract]
        void UpdateLevelRestore(LevelRestoreDTO levelRestore);

        /// <summary>
        ///DeleteLevelRestore
        /// </summary>
        /// <param name="levelRestoreId"></param>
        [OperationContract]
        void DeleteLevelRestore(int levelRestoreId);

        /// <summary>
        ///GetLevelRestore
        /// </summary>
        /// <param name="levelRestoreId"></param>
        /// <returns>LevelRestoreDTO</returns>
        [OperationContract]
        LevelRestoreDTO GetLevelRestore(int levelRestoreId);

        /// <summary>
        ///GetLevelRestoresByLevelId
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns>List<LevelRestore/></returns>
        [OperationContract]
        List<LevelRestoreDTO> GetLevelRestoresByLevelId(int levelId);
        #endregion

        #endregion Level

        #region LevelCompany

        /// <summary>
        /// SaveLevelCompany
        /// Graba un registro de nivel de Contrato
        /// </summary>
        /// <param name="levelCompany"></param>
        [OperationContract]
        int SaveLevelCompany(LevelCompanyDTO levelCompany);

        /// <summary>
        /// UpdateLevelCompany
        /// Actualiza un registro de nivel de Compañia
        /// </summary>
        /// <param name="levelCompany"></param>
        [OperationContract]
        void UpdateLevelCompany(LevelCompanyDTO levelCompany);

        /// <summary>
        /// DeleteLevelCompany
        /// Elimina un registro dado como parámetro el id de nivel de contrato de compañia
        /// </summary>
        /// <param name="levelCompanyId"></param>
        [OperationContract]
        void DeleteLevelCompany(int levelCompanyId);

        /// <summary>
        /// GetLevelCompanyByCompanyId
        /// Obtiene un registro de capa de Compañia dado como parámetro el id de nivel de contrato de compañía
        /// </summary>
        /// <param name="levelCompanyId"></param>
        /// <returns>LevelCompanyDTO</returns>
        [OperationContract]
        LevelCompanyDTO GetLevelCompanyByCompanyId(int levelCompanyId);

        /// <summary>
        /// GetLevelCompaniesByLevelId
        /// Obtiene los registros de capas de compañia dado el id de nivel de contrato
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns>List<LevelCompanyDTO/></returns>
        [OperationContract]
        List<LevelCompanyDTO> GetLevelCompaniesByLevelId(int levelId);


        /// <summary>
        /// GetParticipationPercentageByLevelId
        /// Obtiene el porcentaje de participación de un nivel de contrato, esto para validar que no supere el 100%
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns>decimal</returns>
        [OperationContract]
        decimal GetParticipationPercentageByLevelId(int levelId);

        /// <summary>
        /// GetReinsuranceCompanyIdByLevelIdAndIndividualId
        /// Obtiene el identificativo de la compañía reaseguradora, esto para no permitir el ingreso de 
        /// la misma compañía a un nivel de contrato 
        /// </summary>
        /// <param name="levelId"></param>
        /// <param name="individualId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int GetReinsuranceCompanyIdByLevelIdAndIndividualId(int levelId, int individualId);

        /// <summary>
        /// Valida si una reaseguradora esta activa
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [OperationContract]
        bool IsReinsurerActive(int individualId);
        #endregion LevelCompany

        #region Installment

        /// <summary>
        /// SaveInstallment
        /// </summary>
        /// <param name="installment"></param>
        [OperationContract]
        void SaveInstallment(InstallmentDTO installment);

        /// <summary>
        /// UpdateInstallment
        /// </summary>
        /// <param name="installment"></param>
        [OperationContract]
        void UpdateInstallment(InstallmentDTO installment);

        /// <summary>
        /// DeleteInstallment
        /// </summary>
        /// <param name="installment"></param>
        [OperationContract]
        void DeleteInstallment(InstallmentDTO installment);

        /// <summary>
        /// GetInstallmentsByLevelCompanyId
        /// </summary>
        /// <param name="levelCompanyId"></param>
        /// <returns>List<InstallmentDTO></returns>
        [OperationContract]
        List<InstallmentDTO> GetInstallmentsByLevelCompanyId(int levelCompanyId);

        #endregion


        #endregion

        #region Line

        #region Line


        /// <summary>
        /// SaveLine
        /// Graba una Línea
        /// </summary>
        /// <param name="line"></param>
        [OperationContract]
        void SaveLine(LineDTO line);

        /// <summary>
        /// UpdateLine
        /// Actualiza una Línea
        /// </summary>
        /// <param name="line"></param>
        [OperationContract]
        void UpdateLine(LineDTO line);

        /// <summary>
        /// DeleteLine
        /// Borra una Línea por el código
        /// </summary>
        /// <param name="lineId"></param>
        [OperationContract]
        void DeleteLine(int lineId);

        /// <summary>
        /// GetLineByLineId
        /// Obtiene una Línea por su código
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns>LineDTO</returns>
        [OperationContract]
        LineDTO GetLineByLineId(int lineId);

        /// <summary>
        /// GetLines
        /// Obtiene todas las Lineas
        /// </summary>
        /// <returns>List<LineDTO><Line/></returns>
        [OperationContract]
        List<LineDTO> GetLines();

        /// <summary>
        /// GetLineCumulusType
        /// Obtiene la relación entre Line y CumulusType
        /// </summary>
        /// <returns>List<LineCumulusTypeDTO/></returns>
        [OperationContract]
        List<LineCumulusTypeDTO> GetLineCumulusType();

        /// <summary>
        /// GetCumulusTypes
        /// Obtiene todos los tipos de Cumulo
        /// </summary>
        /// <returns>List<CumulusTypeDTO></returns>
        [OperationContract]
        List<CumulusTypeDTO> GetCumulusTypes();

        #endregion Line

        #region ContractLine

        /// <summary>
        /// SaveContractLine
        /// Graba un nuevo registro de línea de contrato
        /// </summary>
        /// <param name="LineDTO"></param>
        [OperationContract]
        void SaveContractLine(LineDTO line);

        /// <summary>
        /// UpdateContractLine
        /// Actualiza un  registro de línea de contrato
        /// </summary>
        /// <param name="contractLine"></param>
        [OperationContract]
        void UpdateContractLine(LineDTO contractLine);

        /// <summary>
        /// DeleteContractLine
        /// Elimina un registro línea de contrato
        /// </summary>
        /// <param name="contractLineId"></param>
        [OperationContract]
        void DeleteContractLine(int contractLineId);

        /// <summary>
        /// GetContractLineById
        /// Obtiene un registro de línea de contrato por Id
        /// </summary>
        /// <param name="contractLineId"></param>
        /// <returns>ContractLineDTO</returns>
        [OperationContract]
        ContractLineDTO GetContractLineByContractLineId(int contractLineId);

        /// <summary>
        /// GetContractLines
        /// Obtiene Lineas de Contratos
        /// </summary>
        /// <returns>List<ContractLineDTO/></returns>
        [OperationContract]
        List<ContractLineDTO> GetContractLines();

        /// <summary>
        /// GetContractLineByLineId
        /// </summary>
        /// <returns>LineDTO</returns>
        [OperationContract]
        LineDTO GetContractLineByLineId(int lineId);
        //Line GetContractLineByLineId(int lineId);
        #endregion ContractLine


        #endregion

        #region AssociationLine

        /// <summary>
        /// GetAssociationLine
        /// </summary>
        /// <returns>List<AssociationLineDTO/></returns>
        [OperationContract]
        List<AssociationLineDTO> GetAssociationLine(int year, int associationTypeId, int associationLineId);

        /// <summary>
        /// DeleteAssociationColumnValueByAssociationLineId
        /// Elimina un registro de AssociationColumnValue dado el associationLineId
        /// </summary>
        [OperationContract]
        void DeleteAssociationColumnValueByAssociationLineId(int associationLineId);

        /// <summary>
        /// GetAssociationTypes
        /// Obtiene todos los registros de la tabla REINS.ASSOCIATION_TYPE
        /// Devuelve los Tipos de Asociación de Líneas
        /// </summary>
        [OperationContract]
        List<LineAssociationTypeDTO> GetAssociationTypes();

        /// <summary>
        /// GetAssociationColumnByAssociationTypeId
        /// </summary>
        /// <returns>List<LineAssociationType/></returns>
        [OperationContract]
        List<LineAssociationTypeDTO> GetAssociationColumnByAssociationTypeId(int associationTypeId);

        /// <summary>
        /// SaveLineAssociation
        /// Graba AssociationColumnValue y AssociationLine
        /// </summary>
        /// <param name="associationLine"></param>
        [OperationContract]
        LineAssociationDTO SaveLineAssociation(LineAssociationDTO associationLine);

        /// <summary>
        /// UpdateLineAssociation
        /// </summary>
        /// <param name="associationLine"></param>
        /// <returns>LineAssociation</returns>
        [OperationContract]
        LineAssociationDTO UpdateLineAssociation(LineAssociationDTO associationLine);

        /// <summary>
        /// DeleteAssociationColumnValue
        /// </summary>
        [OperationContract]
        void DeleteAssociationColumnValue(int associationColumnValueId);

        /// <summary>
        /// DeleteAssociationLine
        /// Borra un registro de Línea de Asociacion 
        /// </summary>
        /// <param name="associationLineId"></param>
        [OperationContract]
        void DeleteAssociationLine(int associationLineId);

        /// <summary>
        /// GetCountAssociationColumn
        /// </summary>
        /// <param name="associationTypeId"></param>
        [OperationContract]
        int GetCountAssociationColumn(int associationTypeId);

        /// <summary>
        /// Valida si una sociación de line se duplica
        /// </summary>
        /// <param name="lineAssociationDTO"></param>
        /// <returns></returns>
        [OperationContract]
        bool ValidateDuplicateLineAssociation(AssociationLineDTO associationLineDTO);

        #endregion AssociationLine

        #region ReinsurancePrefix


        /// <summary>
        /// SaveReinsurancePrefix
        /// </summary>
        /// <param name="reinsurancePrefix"></param>
        /// <returns>ReinsurancePrefix</returns>
        [OperationContract]
        ReinsurancePrefixDTO SaveReinsurancePrefix(ReinsurancePrefixDTO reinsurancePrefix);

        /// <summary>
        /// UpdateReinsurancePrefix
        /// </summary>
        /// <param name="reinsurancePrefix"></param>
        /// <returns>ReinsurancePrefix</returns>
        [OperationContract]
        ReinsurancePrefixDTO UpdateReinsurancePrefix(ReinsurancePrefixDTO reinsurancePrefix);

        /// <summary>
        /// DeleteReinsurancePrefix
        /// </summary>
        /// <param name="reinsurancePrefix"></param>
        /// <returns>bool</returns>
        [OperationContract]
        bool DeleteReinsurancePrefix(ReinsurancePrefixDTO reinsurancePrefix);


        /// <summary>
        /// GetReinsurancePrefixes
        /// </summary>
        /// <returns>List<ReinsurancePrefix> </returns>
        [OperationContract]
        List<ReinsurancePrefixDTO> GetReinsurancePrefixes();


        #endregion

        #region EPIType
        /// <summary>
        /// GetEPITypes
        /// </summary>
        /// <returns>List<EPITypeDTO></returns>
        [OperationContract]
        List<EPITypeDTO> GetEPITypes();

        #endregion

        #region AffectationType

        /// <summary>
        /// GetAffectationTypes
        /// </summary>
        /// <returns>List<AffectationTypeDTO></returns>
        [OperationContract]
        List<AffectationTypeDTO> GetAffectationTypes();

        #endregion

        #region ResettlementType
        /// <summary>
        /// GetResettlementTypes
        /// </summary>
        /// <returns>List<ResettlementTypeDTO></returns>
        [OperationContract]
        List<ResettlementTypeDTO> GetResettlementTypes();

        [OperationContract]
        List<CoverageDTO> GetCoveragesByLineBusinessIdSubLineBusinessId(int lineBusinessId, int subLineBusinessId);

        [OperationContract]
        List<InsuredObjectDTO> GetInsuredObjectByPrefixIdList(int prefixId);

        [OperationContract]
        List<ModuleDateDTO> GetModuleDates();

        [OperationContract]
        List<CurrencyDTO> GetCurrencies();

        [OperationContract]
        InsuredDTO GetInsuredByIndividualId(int individualId);
        //[OperationContract]
        //List<PolicyDTO> GetPoliciesByBranchAndPrefix(int policyId, int prefixId, int branchId);

        [OperationContract]
        List<IndividualDTO> GetReinsurerByName(string name, int reinsurance, int foreignReinsurance);

        [OperationContract]
        List<BranchDTO> GetBranches();

        [OperationContract]
        List<PrefixDTO> GetPrefixes();
        #endregion

        #endregion Param

        #region IssuanceReinsurance


        /// <summary>
        /// GetReinsuranceDistributionHeaders
        /// Método que obtiene datos del Reaseguros de Emisión realizados por el proceso automático.
        /// </summary>
        /// <param name="endorsementId">Clave primaria del endoso</param>
        /// <returns>List<ReinsuranceDistributionHeaderDTO/></returns>
        [OperationContract]
        List<ReinsuranceDistributionHeaderDTO> GetReinsuranceDistributionHeaders(int? endorsementId);

        /// <summary>
        /// ReinsureEndorsement
        /// Reasegurar Poliza de Emisión por el proceso automático 
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="userId"></param>
        /// <param name="onLine"></param>
        /// <returns>ReinsuranceDTO</returns>
        [OperationContract]
        ReinsuranceDTO ReinsureEndorsement(PolicyDTO policy, int userId, bool onLine);


        /// <summary>
        /// ReinsuranceMasive
        /// Reasegurar Polizas masivo ya sea de emisión/siniestros/pagos
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="moduleId"></param>
        [OperationContract]
        void ReinsuranceMassiveByProccesIdModuleId(int processId, int moduleId);

        /// <summary>
        /// GetTempReinsuranceProcess
        /// Trae información de registros procesados/fallidos del proceso masivo
        /// </summary>
        /// <param name="tempReinsuranceProcessId"></param>
        /// <param name="moduleId"></param>
        /// <returns>TempReinsuranceProcessDTO</returns>
        [OperationContract]
        List<TempReinsuranceProcessDTO> GetTempReinsuranceProcess(int? tempReinsuranceProcessId, int moduleId);

        /// <summary>
        /// GetTempReinsuranceProcessDetails
        /// Trae información de registros procesados/fallidos del proceso masivo
        /// </summary>
        /// <param name="tempReinsuranceProcessId"></param>
        /// <param name="page">Número de Página</param>
        /// <param name="pageSize">Tamaño de Página</param>
        /// <returns>List<TempReinsuranceProcessDTO/></returns>
        [OperationContract]
        List<TempReinsuranceProcessDTO> GetTempReinsuranceProcessDetails(int tempReinsuranceProcessId);

        /// <summary>
        /// UpdateTempReinsuranceProcess
        /// </summary>
        /// <param name="tempReinsuranceProcessId"></param>
        /// <param name="recordsProcessed"></param>
        /// <param name="recordsFailed"></param>
        /// <param name="endDate"></param>
        /// <param name="status"></param>
        [OperationContract]
        void UpdateTempReinsuranceProcess(int tempReinsuranceProcessId, int? recordsProcessed,
                                int? recordsFailed, DateTime endDate, int status);

        /// <summary>
        /// GetModules
        /// </summary>
        [OperationContract]
        List<ModuleDTO> GetModules();

        /// <summary>
        /// GetDistributionErrors
        /// Trae información de errores
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns>List<IssGetDistributionErrorsDTO/></returns>
        [OperationContract]
        List<IssGetDistributionErrorsDTO> GetDistributionErrors(int endorsementId);

        /// <summary>
        /// GetTempLayerDistribution
        /// Trae información de distribución de Reaseguros
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns>List<ReinsuranceLayerIssuance/></returns>
        [OperationContract]
        List<ReinsuranceLayerIssuanceDTO> GetTempLayerDistribution(int endorsementId);

        /// <summary>
        /// LoadReinsuranceLayer
        /// Ejecuta el procedimiento de carga de reaseguros
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="username">Usuario que ejecuta el proceso</param>
        /// <param name="processType"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="List<PrefixDTO>"></param>
        /// <returns>int</returns>
        [OperationContract]
        int LoadReinsuranceLayer(int endorsementId, int userId, int processType, DateTime dateFrom, DateTime dateTo, List<PrefixDTO> prefixes);

        /// <summary>
        /// GetTmpIssueLayerById
        /// Trae la información de la tabla REINS.TMP_ISSUE_LAYER capas de reaseguros
        /// </summary>
        /// <param name="tmpIssueLayerId"></param>
        [OperationContract]
        ReinsuranceLayerIssuanceDTO GetTempIssueLayerById(int tmpIssueLayerId);

        /// <summary>
        /// SaveTempIssueLayer
        /// Graba una nueva capa de reaseguro
        /// </summary>
        /// <param name="reinsuranceLayerIssuance"></param>
        [OperationContract]
        void SaveTempIssueLayer(ReinsuranceLayerIssuanceDTO reinsuranceLayerIssuance);

        /// <summary>
        /// UpdateTempIssueLayer
        /// Actualiza una  capa de reaseguro
        /// </summary>
        /// <param name="reinsuranceLayerIssuance"></param>
        [OperationContract]
        void UpdateTempIssueLayer(ReinsuranceLayerIssuanceDTO reinsuranceLayerIssuance);

        /// <summary>
        /// DeleteTempIssueLayer
        /// Elimina una capa de reaseguro dado su Id
        /// </summary>
        /// <param name="tempIssueLayerId"></param>
        [OperationContract]
        void DeleteTempIssueLayer(int tempIssueLayerId);

        /// <summary>
        /// GetTempLineCumulus
        /// Trae la línea proveniente de la vista: REINS.GET_ISS_TMP_LINE_CUMULUS
        /// </summary>
        /// <param name="tempIssueLayerId"></param>
        /// <returns>ReinsuranceLayerIssuanceDTO</returns>
        [OperationContract]
        ReinsuranceLayerIssuanceDTO GetTempLineCumulus(int tempIssueLayerId);

        /// <summary>
        /// UpdateTempLayerLine
        /// Actualiza una línea de Reaseguros de Pólizas 
        /// </summary>
        /// <param name="reinsuranceLine"></param>
        [OperationContract]
        void UpdateTempLayerLine(ReinsuranceLineDTO reinsuranceLine);

        /// <summary>
        /// GetTempLayerLineById
        /// Trae una línea para	Modificación de Reaseguros de Pólizas 
        /// </summary>
        /// <param name="tmpIssueLayerId"></param>
        /// <returns>ReinsuranceLine</returns>
        [OperationContract]
        ReinsuranceLineDTO GetTempLayerLineById(int tmpIssueLayerId);

        /// <summary>
        /// GetLineCumulusType
        /// Trae una línea para	Modificación de Reaseguros de Pólizas 
        /// </summary>
        /// <returns>List<LineDTO/></returns> 
        [OperationContract]
        List<LineDTO> GetReinsuranceLines();

        /// <summary>
        /// GetTempAllocation
        /// Trae la distribución temporal por línea
        /// </summary>
        /// <returns>ReinsuranceLineDTO</returns> 
        [OperationContract]
        ReinsuranceLineDTO GetTempAllocation(int tempLayerLineId);

        /// <summary>
        /// UpdateTempAllocation
        /// Actualiza valores en la distribución de contratos
        /// </summary>
        [OperationContract]
        void UpdateTempAllocation(ReinsuranceAllocationDTO tempAllocation);

        /// <summary>
        /// GetTempAllocationById
        /// Trae los valores de la distribución de contratos dada la línea
        /// </summary>
        /// <param name="tempIssueAllocationId"></param>
        /// <returns>ReinsuranceAllocation</returns>
        [OperationContract]
        ReinsuranceAllocationDTO GetTempAllocationById(int tempIssueAllocationId);

        /// <summary>
        /// LoadFacultative
        /// Carga facultativo
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="layerNumber"></param>
        /// <param name="lineId"></param>
        /// <param name="cumulusKey"></param>
        /// <param name="description"></param>
        /// <param name="sumPercentage"></param>
        /// <param name="premiumPercentage"></param>   
        /// <param name="username"></param>
        /// <returns>int</returns> 
        [OperationContract]
        int LoadFacultative(int processId, int? layerNumber, int? lineId, string cumulusKey,
                            string description, decimal sumPercentage, decimal premiumPercentage, int userId);

        /// <summary>
        /// CalculationValue
        /// Recupera valores de suma y prima para el facultativo
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="layerNumber"></param>
        /// <param name="lineId"></param>
        /// <param name="cumulusKey"></param>
        /// <returns>decimal[]</returns> 
        [OperationContract]
        decimal[] CalculationValue(int processId, int? layerNumber, int? lineId, string cumulusKey);

        /// <summary>
        /// GetTempFacultativeCompanies
        /// Recupera los temporales del facultativo
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="layerNumber"></param>
        /// <param name="lineId"></param>
        /// <param name="cumulusKey"></param>
        /// <returns>List<ReinsuranceLayerIssuance/></returns>
        [OperationContract]
        List<ReinsuranceLayerIssuanceDTO> GetTempFacultativeCompanies(int endorsementId, int layerNumber, int? lineId, string cumulusKey);

        /// <summary>
        /// SaveTempFacultativeCompany
        /// Graba los temporales del facultativo
        /// </summary>
        /// <param name="reinsuranceFacultative"></param>
        [OperationContract]
        void SaveTempFacultativeCompany(ReinsuranceAllocationDTO reinsuranceFacultative);

        /// <summary>
        /// UpdateTempFacultativeCompany
        /// Actualiza los temporales del facultativo
        /// </summary>
        /// <param name="reinsuranceFacultative"></param>
        [OperationContract]
        void UpdateTempFacultativeCompany(ReinsuranceAllocationDTO reinsuranceFacultative);

        /// <summary>
        /// Obtiene la compañia reaseguradora por facultativo
        /// </summary>
        /// <param name="facultativeId"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        int GetReinsuranceCompanyIdByFacultativeIdAndIndividualId(int facultativeId, int individualId);
        /// <summary>
        /// GetSlips
        /// Obtiene los seriales de facultativo
        /// </summary>
        [OperationContract]
        List<SlipDTO> GetSlips(int processId, int endorsementId);

        [OperationContract]
        int ExpandFacultative(int processId, int endorsementId, int layerNumber, int facultativeId);

        #endregion

        #region ReinsuranceClaim

        /// <summary>
        /// GetClaims
        /// </summary>
        /// <param name="branchCode"></param>
        /// <param name="prefixCode"></param>
        /// <param name="policyNumber"></param>
        /// <param name="claimNumber"></param>
        /// <returns>List<Claim/></returns>
        [OperationContract]
        List<ClaimDTO> GetClaims(int branchCode, int prefixCode, decimal policyNumber, int? claimNumber);

        /// <summary>
        /// ReinsuranceClaim
        /// </summary>
        /// <param name="claimCode"></param>
        /// <param name="userId"></param>
        /// <returns>Models.Reinsurance</returns>
        [OperationContract]
        ReinsuranceDTO ReinsuranceClaim(int claimId, int claimModify, int userId);

        /// <summary>
        /// LoadReinsuranceClaim
        /// </summary>
        /// <param name="claimCode"></param>
        /// <param name="userId"></param>
        /// <param name="processType"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="prefixes"></param>
        /// <returns>Reinsurance</returns>
        [OperationContract]
        ReinsuranceDTO LoadReinsuranceClaim(int? claimCode, int userId, int processType, DateTime dateFrom, DateTime dateTo, List<PrefixDTO> prefixes);

        /// <summary>
        /// ModificationReinsuranceClaimLine
        /// </summary>
        /// <param name="tempClaimReinsSourceId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [OperationContract]
        void ModificationReinsuranceClaim(int tempClaimReinsSourceId, decimal newAmount);

        /// <summary>
        /// GetReinsuranceClaim
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns>ReinsurancePaymentClaim</returns>
        [OperationContract]
        ReinsurancePaymentClaimDTO GetReinsuranceClaim(int claimId);

        #endregion ReinsuranceClaim

        #region ReinsurancePayment

        [OperationContract]
        List<PaymentDistributionDTO> GetReinsurancePaymentDistributionsByPaymentRequestId(int paymentRequestId, int movementSourceId, int voucherConceptCd, int claimCoverageCd);

        [OperationContract]
        ReinsuranceDTO ReinsurancePayment(int requestPaymentCode, int userId);

        [OperationContract]
        ReinsuranceDTO LoadReinsurancePayment(int userId, DateTime dateFrom, DateTime dateTo, List<PrefixDTO> prefixes);
        
        [OperationContract]
        void ModificationReinsurancePayment(int tmpPaymentReinsSourceId, decimal amount);

        [OperationContract]
        ReinsurancePaymentClaimDTO GetReinsurancePayment(int paymentRequestId, int userId);

        #endregion ReinsurancePayment

        #region FindReinsurance

        /// <summary>
        /// DeleteReinsurance
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <param name="endorsementNumber"></param>
        /// <returns> bool </returns>
        [OperationContract]
        bool DeleteReinsurance(decimal documentNumber, int endorsementNumber);

        /// <summary>
        /// GetReinsuranceByEndorsement
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns>List<EndorsementReinsuranceDTO/></returns>
        [OperationContract]
        List<EndorsementReinsuranceDTO> GetReinsuranceByEndorsement(int endorsementId);

        /// <summary>
        /// GetDistributionByReinsurance
        /// </summary>
        /// <param name="layerId"></param>
        /// <returns>List<ReinsuranceDistributionDTO/></returns>
        [OperationContract]
        List<ReinsuranceDistributionDTO> GetDistributionByReinsurance(int layerId);

        #endregion FindReinsurance

        #region AccountingParameters

        /// <summary>
        /// GetReinsuranceAccountingParameters
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [OperationContract]
        List<ReinsuranceAccountingParameterDTO> GetReinsuranceAccountingParameters(int processId);

        #endregion AccountingParameters
        
        #region ManualReinsurance

        [OperationContract]
        ReinsuranceDTO ManualIssueReinsurance(PolicyDTO policy, int userId, int? processId);

        [OperationContract]
        int ManualPaymentReinsurance(int paymentRequestId, int userId);

        [OperationContract]
        int ManualClaimReinsurance(int claimId, int claimModifyId, int userId);

        [OperationContract]
        List<ReinsuranceDTO> SaveIssueReinsurance(PolicyDTO policy, ReinsuranceDTO reinsurance);

        [OperationContract]
        int SavePaymentReinsurance(int processId, int paymentRequestId, int userId);

        [OperationContract]
        int SaveClaimReinsurance(int processId, int claimId, int claimModifyId, int userId);

        [OperationContract]
        List<EndorsementDTO> GetEndorsementByPolicyId(int branchCode, int prefixCode, decimal documentNumber, int endorsementNumber);

        [OperationContract]
        List<ReinsuranceDistributionHeaderDTO> GetReinsuranceDistributionByEndorsementId(int endorsementId);

        [OperationContract]
        List<ReinsuranceLayerIssuanceDTO> ModificationReinsuranceLayer(int endorsementId, int tempIssueLayerId);
        [OperationContract]
        bool DeleteTempIssueLayerByEndorsementId(int endorsementId, int tempIssueLayerId);
        [OperationContract]
        List<TempAllocationDTO> GetReinsuranceAllocationBytempLayerLineId(int tempLayerLineId);
        [OperationContract]
        List<InsuredDTO> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);
        [OperationContract]
        ContractDTO AddContract(int contractId, int contractYear, int contractTypeId);
        [OperationContract]
        List<ContractTypeDTO> GetContractTypeEnabled();
        [OperationContract]
        List<ContractTypeDTO> GetContractFuncionalityId(int contractTypeId);
        [OperationContract]
        List<ContractDTO> GetCurrentPeriodContracts(int year);
        [OperationContract]
        int ValidateBeforeDeleteContract(int contractId);
        [OperationContract]
        int GetNextLevelNumberByLevelId(int levelId);
        [OperationContract]
        int GetNextNumberRestoreByLevelId(int levelId);
        [OperationContract]
        bool DeleteLineByLineId(int lineId);
        [OperationContract]
        LineDTO AddLine(int lineId);
        [OperationContract]
        LineDTO AddContractLine(int contractLineId, int lineId);
        [OperationContract]
        bool SaveContractLineByLine(LineDTO lineDTO);
        [OperationContract]
        bool DeleteContractLineByLine(int contractLineId, int lineId);
        [OperationContract]
        bool LineIsUsed(int lineId);
        [OperationContract]
        List<LineAssociationTypeDTO> GetLineAssociationTypes();
        [OperationContract]
        bool DeleteAssociationLines(int associationLineId);
        [OperationContract]
        List<LineBusinessDTO> GetLineBusiness();
        [OperationContract]
        List<SubLineBusinessDTO> GetSubLineBusiness(int lineBusiness);
        [OperationContract]
        int SaveLineAssociationByAssociationLine(AssociationLineDTO associationLineDto);
        [OperationContract]
        AssociationLineDTO AddAssociationLine(int lineAssociationTypeId, int associationLineId, int year);
        [OperationContract]
        List<TempLayerDistributionsDTO> GetTempLayerDistributionByEndorsementId(int endorsementId);
        [OperationContract]
        List<TempLineCumulusIssuanceDTO> GetTempLineeCumulusByIssuance(int tempIssueLayerId);
        [OperationContract]
        List<LevelDTO> GetContractLevelByContractId(string contractId);
        [OperationContract]
        List<SelectDTO> GetContractYear();
        [OperationContract]
        LevelDTO AddContractLevel(int contractId, int contractLevelId, int contractTypeId);
        [OperationContract]
        int SaveContractLevel(LevelDTO levelDTO);
        [OperationContract]
        List<int> ValidateBeforeDeleteContractLevel(int contractLevelId);
        [OperationContract]
        bool DeleteContractLevel(int contractId, int contractLevelId, int level);
        [OperationContract]
        bool ResultSaveContract(ContractDTO contractDTO);
        [OperationContract]
        List<LevelPaymentDTO> GetLevelPaymentsByLevelIdByLevelId(int levelId);
        [OperationContract]
        List<CumulusTypeDTO> GetCumulusTypesOrderByDesc();
        [OperationContract]
        List<LineCumulusTypeDTO> GetLineCumulusTypeOrderByLineId();
        [OperationContract]
        List<ContractLineDTO> GetContractLineByLine(int lineId);
        [OperationContract]
        List<AssociationLineDTO> GetAssociationLineByTypeLineYear(int year, int associationTypeId, int associationLineId);

        #endregion

        #region Process Controller
        [OperationContract]
        List<ReinsuranceAllocationDTO> GetTempAllocationByLayerLineId(int tempLayerLineId);

        [OperationContract]
        List<ReinsuranceAllocationDTO> GetTotSumPrimeAllocation(int tempLayerLineId);

        [OperationContract]
        ReinsuranceAllocationDTO ModificationReinsuranceContractDialog(int tempIssueAllocationId);

        [OperationContract]
        ReinsuranceDTO ReinsuranceIssue(int policyId, int endorsementId, int userId);

        [OperationContract]
        List<TempReinsuranceProcessDTO> GetTempReinsuranceProcessByProcessId(int tempReinsuranceProcessId);

        [OperationContract]
        ReinsuranceFacultativeDTO GetReinsuranceFacultative(int? tempFacultativeCompanyId, int endorsementId, int layerNumber, int lineId, string cumulusKey);
        
        [OperationContract]
        List<TempFacultativeCompaniesDTO> GetTempFacultativeCompaniesByEndorsementId(int endorsementId, int layerNumber, int lineId, string cumulusKey);
        
        [OperationContract]
        List<PlanFacultativeDTO> GetTempFacultativePayment(int levelCompanyId);
        
        [OperationContract]
        bool SavePaymentPlanFacultative(int tmpFacultativeCompanyCode, int feeNumber, string paymentDate, decimal paymentAmount);
        
        [OperationContract]
        bool DeletePlanFacultative(int facultativePaymentsId, int facultativeCompanyId);
        
        [OperationContract]
        List<ClaimDistributionDTO> GetReinsuranceClaimDistributionByClaimCodeClaimModifyCode(int claimCode, int claimModifyCode, int movementSourceId, int claimCoverageCd);
        
        [OperationContract]
        List<ClaimAllocationDTO> GetClaimAllocationByMovementSource(int processId, int movementSourceId, int claimCoverageCd);
        
        [OperationContract]
        List<PaymentRequestDTO> GetPaymentsRequest(int branchCode, int prefixCode, int policyNumber, int claimNumber, int? paymentRequest);

        [OperationContract]
        List<PaymentAllocationDTO> GetPaymentAllocationByMovementSource(int processId, int movementSourceId, int voucherConceptCd, int claimCoverageCd);

        [OperationContract]
        List<FacultativeCompaniesDTO> SaveTempFacultativeCompanyByLineId(ReinsuranceFacultativeDTO reinsuranceFacultative,
                                int endorsementId, int layerNumber, int lineId, string cumulusKey);
        [OperationContract]
        List<ReinsuranceLayerIssuanceDTO> SaveTempIssueLayerByEndorsementId(ReinsuranceLayerDTO reinsuranceLayerIssuanceDTO, int endorsementId);

        [OperationContract]
        bool UpdateTempAllocationByReinsuranceAllocation(ReinsuranceAllocationDTO reinsuranceAllocationDTO);

        [OperationContract]
        List<ReinsurancePaymentLayerDTO> GetPaymentLayerByPaymentRequestId(int paymentRequestId, int voucherConceptCd, int claimCoverageCd);


        [OperationContract]
        List<ReinsurancePaymentDistributionDTO> GetDistributionPaymentByPaymentLayerId(int paymentLayerId);
       
        [OperationContract]
        List<EndorsementReinsuranceDTO> GetReinsuranceByEndorsementId(int endorsementId);
        
        [OperationContract]
        List<ReinsuranceDistributionDTO> GetDistributionByReinsuranceByLayerId(int layerId);

        [OperationContract]
        List<ReinsuranceClaimLayerDTO> GetClaimLayerByClaimIdClaimModifyId(int claimId, int claimModifyId, int movementSorceId, int claimCoverageCd);
        
        [OperationContract]
        List<ReinsuranceClaimDistributionDTO> GetDistributionClaimByClaimLayerId(int claimLayerId, int movementSorceId);
        
        [OperationContract]
        ReinsuranceMassiveHeaderDTO SaveReinsuranceMassiveHeaderByTypeProcess(string dateFrom, string dateTo, int typeProcess, List<PrefixDTO> prefixDTOs, int userId);
       
        [OperationContract]
        List<TempReinsuranceProcessDTO> LoadProcessMassiveDetailsReport(int tempReinsuranceProcessId);
        
        [OperationContract]
        List<ReinsuranceDTO> SaveReinsuranceByEndorsementId(int endorsementId, int userId, ReinsuranceDTO reinsuranceDTO);
        
        [OperationContract]
        ModuleDateDTO GetModuleDate(ModuleDateDTO moduleDateDTO);
        
        [OperationContract]
        List<AgentDTO> GetAgentByName(string name);
        
        [OperationContract]
        List<ProductDTO> GetProductsByPrefixId(int prefixId);
        
        [OperationContract]
        List<BranchDTO> GetBranchesByUserId(int userId);

        [OperationContract]
        UserDTO GetUserByLogin(string login);

        [OperationContract]
        PolicyDTO GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber);
        #endregion

        #region Cúmulos

        /// <summary>
        /// GetCumulusByIndividual
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="lineBusiness"></param>
        /// <param name="dateCumulus"></param>
        /// <param name="IsFuture"></param>
        /// <returns></returns>
        [OperationContract]
        ReinsuranceCumulusDTO GetCumulusByIndividual(int individualId, int lineBusiness, DateTime dateCumulus, bool IsFuture, int subLineBusiness, int PrefixCd);

        /// <summary>
        /// GetCumulusIndividualDetail
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="lineBusiness"></param>
        /// <param name="dateCumulus"></param>
        /// <param name="IsFuture"></param>
        /// <returns></returns>
        [OperationContract]
        ReinsuranceCumulusDTO GetCumulusDetailByIndividual(int individualId, int lineBusiness, DateTime dateCumulus, bool IsFuture, int subLineBusiness, int PrefixCd);

        /// <summary>
        ///  GenerateFileCumulusByIndividual
        /// </summary>
        /// <param name="coverageReinsuranceCumulusDTOs"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateFileCumulusByIndividual(string fileName, List<CoverageReinsuranceCumulusDTO> coverageReinsuranceCumulusDTOs);

        /// <summary>
        /// GetDetailCumulusParticipantsEconomicGroup
        /// </summary>
        /// <param name="economicGroupId"></param>
        /// <param name="lineBusiness"></param>
        /// <param name="dateCumulus"></param>
        /// <param name="IsFuture"></param>
        /// <returns></returns>
        [OperationContract]
        List<DetailCumulusParticipantsEconomicGroupDTO> GetDetailCumulusParticipantsEconomicGroup(int economicGroupId, int lineBusiness, DateTime dateCumulus, bool IsFuture, int sublineBusiness, int prefixCd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [OperationContract]
        bool CreateOperatingQuotaEventsByProcessId(int processId);

        #endregion

        #region PriorityRetention
        [OperationContract]
        List<PriorityRetentionDTO> GetPriorityRetentions();

        [OperationContract]
        List<PriorityRetentionDTO> SavePriorityRetentions(List<PriorityRetentionDTO> lstPriorityRetentionAdded);

        [OperationContract]
        List<PriorityRetentionDTO> UpdatePriorityRetentions(List<PriorityRetentionDTO> lstPriorityRetentionModified);

        [OperationContract]
        bool DeletePriorityRetentions(List<PriorityRetentionDTO> lstPriorityRetentionDelete);

        [OperationContract]
        bool CanPriorityRetentionUpdated(int priorityRetentionId);
        #endregion
    }
}
