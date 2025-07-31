using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.ClaimServices
{
    [ServiceContract]
    public interface IClaimApplicationService
    {
        /// <summary>
        /// Obtener ramos comerciales
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<PrefixDTO> GetPrefixes();

        /// <summary>
        /// Obtener ramos técnicos por identificador del ramo comercial
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        [OperationContract]
        List<LineBusinessDTO> GetLinesBusinessByPrefixId(int prefixId);

        /// <summary>
        /// Obtener sub ramos técnicos por identificador del ramo técnico
        /// </summary>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SubLineBusinessDTO> GetSubLinesBusinessByLineBusinessId(int lineBusinessId);

        /// <summary>
        /// Obtener causas del siniestro por identificador del ramo comercial
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CauseDTO> GetCausesByPrefixId(int prefixId);

        /// <summary>
        /// Obtener coberturas por identificadores de ramo técnico, sub ramo técnico y causa del siniestro
        /// </summary>
        /// <param name="lineBussinessId"></param>
        /// <param name="subLineBussinessId"></param>
        /// <param name="causeId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CoverageDTO> GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(int lineBussinessId, int subLineBussinessId, int causeId);

        /// <summary>
        /// Obtener coberturas por identificadores de ramo técnico y sub ramo técnico
        /// </summary>
        /// <param name="lineBussinessId"></param>
        /// <param name="subLineBussinessId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CoverageDTO> GetCoveragesByLineBusinessIdSubLineBusinessId(int lineBussinessId, int subLineBussinessId);

        /// <summary>
        /// Obtener coberturas por identificadores de ramo técnico
        /// </summary>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CoverageDTO> GetCoveragesByLineBusinessId(int lineBusinessId);

        /// <summary>
        /// Obtener coberturas por identificador de la causa del siniestro
        /// </summary>
        /// <param name="causeId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CoverageDTO> GetCoveragesByCauseId(int causeId);

        /// <summary>
        /// Crear coberturas por idetificador causa del siniestro
        /// </summary>
        /// <param name="causeId"></param>
        /// <param name="coverageDTO"></param>
        /// <returns></returns>
        [OperationContract]
        CoverageDTO CreateCoverageByCause(int causeId, CoverageDTO coverageDTO);

        /// <summary>
        /// Eliminar coberturas por idetificador causa del siniestro
        /// </summary>
        /// <param name="causeId"></param>
        /// <param name="coverageId"></param>
        [OperationContract]
        void DeleteCoverageByCause(int causeId, int coverageId);

        /// <summary>
        /// Elimina cobertura del aviso
        /// </summary>
        /// <param name="noticeId"></param>
        /// <param name="coverageId"></param>
        /// <param name="individualId"></param>
        /// <param name="estimateTypeId"></param>
        [OperationContract]
        void DeleteNoticeCoverageByCoverage(int noticeId, int coverageId, int individualId, int estimateTypeId);

        /// <summary>
        /// Obtener Estados por id concepto de estimacion
        /// </summary>
        /// <param name="estimationTypeId"></param>
        /// <returns>List<StatusDTO></returns>
        [OperationContract]
        List<StatusDTO> GetStatusesByEstimationTypeId(int estimationTypeId);

        /// <summary>
        /// Obtener estados
        /// </summary>
        /// <returns>List<StatusDTO></returns>
        [OperationContract]
        List<StatusDTO> GetStatuses();

        /// <summary>
        /// Obtener tipos de estimacion
        /// </summary>
        /// <returns>List<EstimationTypeDTO></returns>
        [OperationContract]
        List<EstimationTypeDTO> GetEstimationTypes();

        /// <summary>
        /// Crea relacion de estado con concepto de estimacion
        /// </summary>
        /// <param name="statusDTO"></param>
        /// <returns>StatusDTO</returns>
        [OperationContract]
        StatusDTO CreateStatusByEstimationType(StatusDTO statusDTO);

        /// <summary>
        /// Elimina relacion de estado con concepto de estimacion
        /// </summary>
        /// <param name="statusDTO"></param>
        [OperationContract]
        void DeleteStatusByEstimationType(StatusDTO statusDTO);

        /// <summary>
        /// Obtener razones por id estado
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>List<ReasonDTO></returns>
        [OperationContract]
        List<ReasonDTO> GetReasonsByStatusIdPrefixId(int statusId, int prefixId);

        /// <summary>
        /// Crear razon
        /// </summary>
        /// <param name="reasonDTO"></param>
        /// <returns>ReasonDTO</returns>
        [OperationContract]
        ReasonDTO CreateReason(ReasonDTO reasonDTO);

        /// <summary>
        /// Modificar razon
        /// </summary>
        /// <param name="reasonDTO"></param>
        /// <returns>ReasonDTO</returns>
        [OperationContract]
        ReasonDTO UpdateReason(ReasonDTO reasonDTO);

        /// <summary>
        /// Eliminar razon
        /// </summary>
        /// <param name="reasonId"></param>
        /// <param name="statusId"></param>
        [OperationContract]
        void DeleteReason(int reasonId, int statusId, int prefixId);

        /// <summary>
        /// Obtener ramos comerciales por idetificador del tipo de estimación
        /// </summary>
        /// <param name="estimationTypeId"></param>
        /// <returns></returns>
        [OperationContract]
        List<PrefixDTO> GetPrefixesByEstimationTypeId(int estimationTypeId);

        /// <summary>
        /// Crear ramos comerciales por idetificador del tipo de estimación
        /// </summary>
        /// <param name="estimationTypeId"></param>
        /// <param name="PrefixesDTO"></param>
        /// <returns></returns>
        [OperationContract]
        List<PrefixDTO> CreatePrefixesByEstimationType(int estimationTypeId, List<PrefixDTO> PrefixesDTO);

        /// <summary>
        /// Traer Endosos por sucursal, ramo, numero poliza y fecha
        /// </summary>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <param name="documentNumber"></param>
        /// <param name="claimDate"></param>
        /// <returns>List<EndorsementDTO></returns>
        [OperationContract]
        List<EndorsementDTO> GetEndorsementByPrefixIdBranchIdCoveredRiskTypeIdDocumentNumber(int? prefixId, int? branchId, CoveredRiskType coveredRiskTypeId, decimal documentNumber, DateTime claimDate);


        /// <summary>
        /// Obtener la denuncia por su idetificador
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimDTO GetClaimByClaimId(int claimId);

        /// <summary>
        /// Crear Denuncia
        /// </summary>
        /// <param name="claimDTO"></param>
        /// <returns>ClaimDTO</returns>
        [OperationContract]
        ClaimVehicleDTO CreateClaimVehicle(ClaimVehicleDTO claimVehicle);

        /// <summary>
        /// Crear Denuncia
        /// </summary>
        /// <param name="claimSuretyDTO"></param>
        /// <returns>ClaimSuretyDTO</returns>
        [OperationContract]
        ClaimSuretyDTO CreateClaimSurety(ClaimSuretyDTO claimSuretyDTO);

        /// <summary>
        /// Crear Denuncia
        /// </summary>
        /// <param name="claimSuretyDTO"></param>
        /// <returns>ClaimSuretyDTO</returns>
        [OperationContract]
        ClaimLocationDTO CreateClaimLocation(ClaimLocationDTO claimLocationDTO);

        /// <summary>
        /// Actualiza la denuncia
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimVehicleDTO UpdateClaimVehicle(ClaimVehicleDTO claimVehicle);

        /// <summary>
        /// Actualiza la denuncia
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimSuretyDTO UpdateClaimSurety(ClaimSuretyDTO claimSuretyDTO);

        /// <summary>
        /// Actualiza la denuncia
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimLocationDTO UpdateClaimLocation(ClaimLocationDTO claimLocationDTO);

        /// <summary>
        /// Obtener Sucursales
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List<BranchDTO></returns>
        [OperationContract]
        List<BranchDTO> GetBranches();

        /// <summary>
        /// Obtener informacion de la poliza por identificador del endoso
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns>PolicyDTO</returns>
        [OperationContract]
        PolicyDTO GetPolicyByEndorsementIdModuleType(int endorsementId);

        [OperationContract]
        List<SubClaimDTO> GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber);

        /// <summary>
        /// Consulta las estimaciones por sub reclamo
        /// </summary>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <param name="claimNumber"></param>
        /// <returns></returns>
        [OperationContract]
        List<SubClaimDTO> GetSubClaimsEstimationByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber);

        /// <summary>
        /// Obtener lista de tipos de notificacion
        /// </summary>
        /// <returns>List<NoticeTypeDTO></returns>
        [OperationContract]
        List<NoticeTypeDTO> GetNoticeTypes();

        /// <summary>
        /// Obtener Fabricantes por descripcion
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetVehicleMakesByDescription(string description);

        /// <summary>
        /// Obtener Modelos por descripcion
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetVehicleModelsByDescription(string description);

        /// <summary>
        /// obtener Lista de Colores de Vehiculos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetVehicleColors();

        /// <summary>
        /// Obtener analizadores
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetAnalizers();

        /// <summary>
        /// Obtener ajustadores
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetAdjusters();

        /// <summary>
        /// Obtener rebuscadores
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetResearchers();

        /// <summary>
        /// Obtener Lista de tipos de daño
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetDamageTypes();

        /// <summary>
        /// Obtener Lista de DamageResponsibilities
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetDamageResponsibilities();

        /// <summary>
        /// Consultar sucursales por usuario
        /// </summary>
        /// <param name="userId">Usuario Id</param>
        /// <returns>Listado de Sucursal</returns>
        [OperationContract]
        List<SelectDTO> GetBranchesByUserId(int userId);

        /// <summary>
        /// Consulta monedas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetCurrencies();

        /// <summary>
        /// Consultar ramos comerciales por tipo de riesgo cubierto
        /// </summary>
        /// <param name="coveredRiskType"></param>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetPrefixesByCoveredRiskType(CoveredRiskType coveredRiskType);

        /// <summary>
        /// Consulta la información de las coberturas vigentes asociadas al riesgo según fecha de ocurrencia del siniestro y muestra las 
        /// sumas aseguradas según el porcentaje de participación de la compañía en el siniestro
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="occurrenceDate"></param>
        /// <param name="companyParticipationPercentage"></param>
        /// <returns></returns>
        [OperationContract]
        List<CoverageDTO> GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(int riskId, DateTime occurrenceDate, decimal companyParticipationPercentage);


        /// <summary>
        /// Consulta toda la información de las coberturas vigentes asociadas al riesgo
        /// </summary>
        /// <param name="riskId">Identificador del riesgo</param>
        /// <returns></returns>
        [OperationContract]
        List<CoverageDTO> GetCoveragesByRiskIdDescription(int riskId, string Description);


        /// Consultar tipo de documento para personas naturales o juridicas.
        /// </summary>
        /// <param name="typeDocument"></param>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetDocumentTypesByIndividualType(int typeDocument);


        /// <summary>
        /// Consulta los tipos de estimación por ramo comercial
        /// </summary>
        /// <param name="prefixId">ID del ramo</param>
        /// <returns>Listado de tipos de estimacion</returns>
        [OperationContract]
        List<SelectDTO> GetEstimationTypesByPrefixId(int prefixId);

        /// <summary>
        /// Consulta Catástrofes
        /// </summary>
        /// <returns>List<SelectDTO></returns>
        [OperationContract]
        List<SelectDTO> GetCatastrophes();

        /// <summary>
        /// Consulta las catastrofe por Descripcion
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetCatastrophesByDescription(string query);

        /// <summary>
        /// Obtener proveedores por idetificador del tipo de perfil 
        /// </summary>
        /// <param name="supplierProfile"></param>
        /// <returns></returns>
        [OperationContract]
        List<SupplierDTO> GetSuppliersBySupplierProfile(SupplierProfile supplierProfile);

        /// <summary>
        /// Obtener proveedores por nombre, número de documento ó identificador
        /// </summary>
        /// <param name="description"></param>
        /// <param name="insuredSearchType"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        [OperationContract]
        List<SupplierDTO> GetSuppliersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        /// <summary>
        /// Obtener paises
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetCountries();

        /// <summary>
        /// Obtener departamentos por identificador del pais
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetStatesByCountryId(int countryId);

        /// <summary>
        /// Obtener ciudades por identificadores del departamento y el pais
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetCitiesByCountryIdStateId(int countryId, int stateId);

        /// <summary>
        /// Obtener la denuncia por identificadores del ramo comercial, sucursal y número de denuncia
        /// </summary>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <param name="claimNumber"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimDTO GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber);

        /// <summary>
        /// Obtener fecha del módulo por identificador del tipo de módulo y fecha
        /// </summary>
        /// <param name="moduleType"></param>
        /// <param name="movementDate"></param>
        /// <returns></returns>
        [OperationContract]
        DateTime GetModuleDateByModuleTypeMovementDate(ModuleType moduleType, DateTime movementDate);

        /// <summary>
        /// Obtener riesgos de vehículos por identificador del endoso
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        List<VehicleDTO> GetRiskVehiclesByEndorsementId(int endorsementId);

        /// <summary>
        /// Obtener riesgos de vehículos por identificador del endoso y tipo del módulo
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        [OperationContract]
        List<VehicleDTO> GetRiskVehiclesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType);

        /// <summary>
        /// Obtener riesgos de ubicación por identificador del endoso y tipo del módulo
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        [OperationContract]
        List<RiskLocationDTO> GetRiskPropertiesByEndorsementId(int endorsementId);

        /// <summary>
        /// Obtener riesgos de fianza por identificador del endoso, ramo comercial y tipo del módulo
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="prefixId"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        [OperationContract]
        List<SuretyDTO> GetSuretiesByEndorsementIdPrefixId(int endorsementId, int prefixId);

        /// <summary>
        /// Obtener estimaciones por ramo comercial
        /// </summary>
        /// <param name="PrefixId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EstimationDTO> GetEstimationsByPrefixId(int PrefixId);

        /// <summary>
        /// Obtener marcas de lo vehículos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetVehicleMakes();

        /// <summary>
        /// Obtener años de los vehículos por identificadores de marca, modelo y versión
        /// </summary>
        /// <param name="MakeId"></param>
        /// <param name="ModelId"></param>
        /// <param name="VersionId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetVehicleYearsByMakeIdModelIdVersionId(int MakeId, int ModelId, int VersionId);

        /// <summary>
        /// Obtener modelos de vehículos por identificador de la marca
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetVehicleModelsByMakeId(int makeId);

        /// <summary>
        /// Obtener versiones de los vehículos por identificadores de marca y modelo
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetVehicleVersionsByMakeIdModelId(int makeId, int modelId);

        /// <summary>
        /// Obtener los tipos de persona
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetPersonTypes();

        /// <summary>
        /// Obtener estado civil
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetMaritalStatus();

        /// <summary>
        /// Obtener asegurados por nombre, número de documento ó identificador
        /// </summary>
        /// <param name="description"></param>
        /// <param name="insuredSearchType"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        [OperationContract]
        List<InsuredDTO> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        /// <summary>
        /// Obtener el asegurado de un riesgo
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        InsuredDTO GetInsuredByRiskId(int riskId);

        /// <summary>
        /// Obtener el tercero por nombre o número de identificación
        /// </summary>
        /// <param name="description"></param>
        /// <param name="insuredSearchType"></param>
        /// <returns></returns>
        [OperationContract]
        List<ThirdPartyDTO> GetThirdPartyByDescriptionInsuredSearchType(string description, InsuredSearchType insuredSearchType);

        /// <summary>
        /// Obtener generos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetGenders();

        /// <summary>
        /// Obtiene el usuario por nombre.
        /// </summary>
        /// <param name="name">parametro de entrada.</param>
        /// <returns></returns>
        [OperationContract]
        List<UserDTO> GetUserByName(string name);

        /// <summary>
        /// Buscar las denuncias
        /// </summary>
        /// <param name="searchClaimDTO"></param>
        /// <returns></returns>
        [OperationContract]
        List<ClaimDTO> SearchClaims(SearchClaimDTO searchClaimDTO);

        /// <summary>
        /// Buscar las denuncias con estimación por salario
        /// </summary>
        /// <param name="searchClaimDTO"></param>
        /// <returns></returns>
        [OperationContract]
        List<SubClaimDTO> SearchClaimsBySalaryEstimationCurrentYear(SearchClaimDTO searchClaimDTO, int currentYear);

        /// <summary>
        /// Obtiene las coberturas por ramos y subramos. 
        /// </summary>
        /// <param name="lineBusinessid"></param>
        /// <param name="subLineBusinessId"></param>
        /// <returns></returns>
        [OperationContract]
        List<ClaimCoverageActivePanelDTO> GetClaimCoverageActivePanelsByLineBusinessIdSubLineBusinessId(int lineBusinessid, int subLineBusinessId);

        /// <summary>
        /// Crea los active panel por coberturas.
        /// </summary>
        /// <param name="claimCoverageActivePanels"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimCoverageActivePanelDTO CreateCoverageActivePanel(ClaimCoverageActivePanelDTO claimCoverageActivePanels);

        /// <summary>
        /// Actualiza los actives panel por coberturas.
        /// </summary>
        /// <param name="claimCoverageActivePanels"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimCoverageActivePanelDTO UpdateCoverageActivePanel(ClaimCoverageActivePanelDTO claimCoverageActivePanels);

        /// <summary>
        /// Obtener tipo de riesgo cubierdo de la denuncia por identificador del ramo comercial
        /// </summary>
        /// <param name="prefixCode"></param>
        /// <returns></returns>
        [OperationContract]
        int GetClaimPrefixCoveredRiskTypeByPrefixCode(int prefixCode);

        /// <summary>
        /// Obtener lista de deudores por nombre, número de documento ó identificador
        /// </summary>
        /// <param name="description"></param>
        /// <param name="insuredSearchType"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        [OperationContract]
        List<DebtorDTO> GetDebtorsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        /// <summary>
        /// Obtener recuperadores por nombre, número de documento ó identificador
        /// </summary>
        /// <param name="description"></param>
        /// <param name="insuredSearchType"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        [OperationContract]
        List<RecuperatorDTO> GetRecuperatorsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        /// <summary>
        /// Buscar avisos
        /// </summary>
        /// <param name="searchNoticeDTO"></param>
        /// <returns></returns>
        [OperationContract]
        List<NoticeDTO> SearchNotices(SearchNoticeDTO searchNoticeDTO);

        /// <summary>
        /// Obtener motivos de cancelación
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetCancellationReasons();

        /// <summary>
        /// Obtener los compradores por identificador, nombre y numero de documento
        /// </summary>
        /// <param name="description"></param>
        /// <param name="insuredSearchType"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        [OperationContract]
        List<AffectedDTO> GetAffectedByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        /// <summary>
        /// Obtener paneles activos por identificador de la cobertura
        /// </summary>
        /// <param name="coverageId"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimCoverageActivePanelDTO GetActivePanelsByCoverageId(int coverageId);

        /// <summary>
        /// Obtener los conductores por identificador, nombre y numero de documento
        /// </summary>
        /// <param name="description"></param>
        /// <param name="insuredSearchType"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimCoverageDriverInformationDTO GetDriverByDocumentNumberFullName(string description, InsuredSearchType insuredSearchType);

        /// <summary>
        /// Obtener los tomadores por identificador, nombre y numero de documento
        /// </summary>
        /// <param name="description"></param>
        /// <param name="insuredSearchType"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        [OperationContract]
        List<HolderDTO> GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        /// <summary>
        /// Buscar pólizas
        /// </summary>
        /// <param name="policyDTO"></param>
        /// <returns></returns>
        [OperationContract]
        List<PolicyDTO> GetClaimPoliciesByPolicy(PolicyDTO policyDTO);

        /// <summary>
        /// Obtener informacion de conductor por id de la cobertura
        /// </summary>
        /// <param name="claimCoverageId"></param>
        /// <returns>ClaimCoverageDriverInformationDTO</returns>
        [OperationContract]
        ClaimCoverageDriverInformationDTO GetClaimDriverInformationByClaimCoverageId(int claimCoverageId);

        /// <summary>
        /// Consultar vehiculo(tercero) del asegurado
        /// </summary>
        /// <param name="claimCoverageId"></param>
        /// <returns>ClaimCoveragethirdPartiesVehicleDTO</returns>
        [OperationContract]
        ClaimCoverageThirdPartyVehicleDTO GetClaimThirdPartyVehicleByClaimCoverageId(int claimCoverageId);

        /// <summary>
        /// Obtener Analista Ajustador
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns>ClaimSupplierDTO</returns>
        [OperationContract]
        ClaimSupplierDTO GetClaimSupplierByClaimId(int claimId);

        /// <summary>
        /// Consulta las estimaciones codigo de Claim
        /// </summary>
        /// <param name="claimCode"></param>
        /// <returns></returns>
        [OperationContract]
        List<SubClaimDTO> GetEstimationByClaimId(int claimId);

        /// <summary>
        /// Obtener informacion catastrofica por id de la denuncia
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns>CatastrophicInformationDTO</returns>
        [OperationContract]
        CatastrophicInformationDTO GetClaimCatastrophicInformationByClaimId(int claimId);

        /// <summary>
        /// Obtiene los vehiculos segun coincidencia de la placa (Autocomplete)
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetSelectRisksVehicleByLicensePlate(string plate);

        /// <summary>
        /// Obtener deducibles por identificador de la cobertura
        /// </summary>
        /// <param name="coverageId"></param>
        /// <returns></returns>
        [OperationContract]
        CoverageDeductibleDTO GetCoverageDeductibleByCoverageId(int coverageId);

        /// <summary>
        /// Objetar aviso
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        [OperationContract]
        NoticeDTO ObjectNotice(NoticeDTO notice);

        /// <summary>
        /// Obtener tipo de búsqueda
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetSearchTypes();

        /// <summary>
        /// Crea la Reserva
        /// </summary>
        /// <param name="claimReserve"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimReserveDTO SetClaimReserve(ClaimReserveDTO claimReserve);

        [OperationContract]
        void SetClaimReserveByClaimIdSubClaimEstimationTypeIdPaymentUserId(int claimId, int subClaim, int estimationTypeId, int userId);

        /// <summary>
        /// Obtiene las reservas segun el ramo, la sucursal y el numero de denuncia
        /// </summary>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <param name="claimNumber"></param>
        /// <returns></returns>
        [OperationContract]
        List<SubClaimDTO> GetClaimReserveByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(int? prefixId, int? branchId, string PpolicyDocumentNumber, int claimNumber);

        /// <summary>
        /// Obtiene las estimaciones la denuncia, el ramo y la cobertura
        /// </summary>
        /// <param name="claimId"></param>
        /// <param name="prefixId"></param>
        /// <param name="coverageId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EstimationDTO> GetEstimationTypesByClaimModifyIdPrefixIdCoverageIdIndividualId(int claimModifyId, int prefixId, int coverageId, int individualId);

        /// <summary>
        /// Obtiene los riesgos de transporte según el Id del Endoso
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        [OperationContract]
        List<TransportDTO> GetTransportByEndorsementIdModuleType(int endorsementId, ModuleType moduleType);

        /// <summary>
        /// Crea una denuncia de Transporte
        /// </summary>
        /// <param name="claimTransportDTO"></param>
        /// <param name="driversInformationDTO"></param>
        /// <param name="thirdPartiesVehicleDTO"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimTransportDTO CreateClaimTransport(ClaimTransportDTO claimTransportDTO);

        /// <summary>
        /// Actualiza una denuncia de Transporte
        /// </summary>
        /// <param name="claimTransportDTO"></param>
        /// <param name="driversInformationDTO"></param>
        /// <param name="thirdPartiesVehicleDTO"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimTransportDTO UpdateClaimTransport(ClaimTransportDTO claimTransportDTO);

        /// <summary>
        /// Obtiene los riesgos de Vehiculo según el Id del Asegurado
        /// </summary>
        /// <param name="insuredId"></param>
        /// <returns></returns>
        [OperationContract]
        List<VehicleDTO> GetRisksVehicleByInsuredId(int insuredId);

        /// <summary>
        /// Obtiene los riesgos de Vehiculo según el Id del Asegurado
        /// </summary>
        /// <param name="insuredId"></param>
        /// <returns></returns>
        [OperationContract]
        List<VehicleDTO> GetRisksVehicleByLicensePlate(string licensePlate);

        /// <summary>
        /// Crea un Aviso de Vehículos
        /// </summary>
        /// <param name="noticeVehicleDTO"></param>
        /// <param name="contactInformationDTO"></param>
        /// <param name="vehicleDTO"></param>
        /// <param name="coveragesDTO"></param>
        /// <returns></returns>
        [OperationContract]
        NoticeVehicleDTO CreateNoticeVehicle(NoticeVehicleDTO noticeVehicleDTO, ContactInformationDTO contactInformationDTO, VehicleDTO vehicleDTO);

        /// <summary>
        /// Actualizar aviso de vehículos
        /// </summary>
        /// <param name="noticeVehicleDTO"></param>
        /// <param name="contactInformationDTO"></param>
        /// <param name="vehicleDTO"></param>
        /// <param name="noticeCoverageDTO"></param>
        /// <returns></returns>
        [OperationContract]
        NoticeVehicleDTO UpdateNoticeVehicle(NoticeVehicleDTO noticeVehicleDTO, ContactInformationDTO contactInformationDTO, VehicleDTO vehicleDTO);

        /// <summary>
        /// Obtiene los riesgos de Propiedad según el Id del Asegurado
        /// </summary>
        /// <param name="insuredId"></param>
        /// <returns></returns>
        [OperationContract]
        List<RiskLocationDTO> GetRiskPropertiesByInsuredId(int insuredId);

        /// <summary>
        /// Crea un aviso de Propiedad
        /// </summary>
        /// <param name="noticeLocationDTO"></param>
        /// <param name="contactInformationDTO"></param>
        /// <param name="locationDTO"></param>
        /// <param name="coveragesDTO"></param>
        /// <returns></returns>
        [OperationContract]
        NoticeLocationDTO CreateNoticeLocation(NoticeLocationDTO noticeLocationDTO, ContactInformationDTO contactInformationDTO, RiskLocationDTO locationDTO);

        /// <summary>
        /// Actualizar un aviso de propiedad
        /// </summary>
        /// <param name="noticeLocationDTO"></param>
        /// <param name="contactInformationDTO"></param>
        /// <param name="locationDTO"></param>
        /// <param name="noticeCoverageDTO"></param>
        /// <returns></returns>
        [OperationContract]
        NoticeLocationDTO UpdateNoticeLocation(NoticeLocationDTO noticeLocationDTO, ContactInformationDTO contactInformationDTO, RiskLocationDTO locationDTO);

        /// <summary>
        /// Obtiene los riesgos de fianza según el Id del Asegurado
        /// </summary>
        /// <param name="insuredId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SuretyDTO> GetRisksSuretyByInsuredId(int insuredId);

        /// <summary>
        /// Obtener riesgos de fianza por identificador del afianzado
        /// </summary>
        /// <param name="suretyId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SuretyDTO> GetRisksSuretyBySuretyIdPrefixId(int suretyId, int prefixId);

        /// <summary>
        /// Crea un aviso de fianza
        /// </summary>
        /// <param name="noticeSuretyDTO"></param>
        /// <param name="contactInformationDTO"></param>
        /// <param name="suretyDTO"></param>
        /// <param name="coveragesDTO"></param>
        /// <returns></returns>
        [OperationContract]
        NoticeSuretyDTO CreateNoticeSurety(NoticeSuretyDTO noticeSuretyDTO, ContactInformationDTO contactInformationDTO, SuretyDTO suretyDTO);

        /// <summary>
        /// Actualizar aviso de fianza
        /// </summary>
        /// <param name="noticeSuretyDTO"></param>
        /// <param name="contactInformationDTO"></param>
        /// <param name="suretyDTO"></param>
        /// <param name="noticeCoverageDTO"></param>
        /// <returns></returns>
        [OperationContract]
        NoticeSuretyDTO UpdateNoticeSurety(NoticeSuretyDTO noticeSuretyDTO, ContactInformationDTO contactInformationDTO, SuretyDTO suretyDTO);

        /// <summary>
        /// Obtener aviso por su identificador
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        [OperationContract]
        NoticeDTO GetNoticeByNoticeId(int noticeId);

        /// <summary>
        /// Obtener riesgo de vehículo por su identificador
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        VehicleDTO GetRiskVehicleByRiskId(int riskId);

        /// <summary>
        /// Obtener riesgo del vehículo de no seguro por identificador del aviso
        /// </summary>
        /// <param name="claimNoticeId"></param>
        /// <returns></returns>
        [OperationContract]
        VehicleDTO GetRiskVehicleByClaimNoticeId(int claimNoticeId);

        /// <summary>
        /// Obtener riesgo del ubicación de no seguro por identificador del aviso
        /// </summary>
        /// <param name="claimNoticeId"></param>
        /// <returns></returns>
        [OperationContract]
        RiskLocationDTO GetRiskLocationByClaimNoticeId(int claimNoticeId);

        /// <summary>
        /// Obtener riesgo del fianza de no seguro por identificador del aviso
        /// </summary>
        /// <param name="claimNoticeId"></param>
        /// <returns></returns>
        [OperationContract]
        SuretyDTO GetRiskSuretyByClaimNoticeId(int claimNoticeId);

        /// <summary>
        /// Obtener riesgo de fianza por identificador del riesgo y tipo de módulo
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        [OperationContract]
        SuretyDTO GetSuretyByRiskIdPrefixId(int riskId, int prefixId);

        /// <summary>
        /// Obtener riesgo de ubicación por identificador del riesgo y tipo de módulo
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        [OperationContract]
        RiskLocationDTO GetRiskPropertyByRiskId(int riskId);

        /// <summary>
        /// Obtener riesgos de ubicación por dirección
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        [OperationContract]
        List<RiskLocationDTO> GetRiskPropertiesByAddress(string address);

        /// <summary>
        /// Obtener riesgos de fianza por información del afianzado
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        List<SuretyDTO> GetRisksBySurety(string description);

        /// <summary>
        /// Crear aviso de transportes
        /// </summary>
        /// <param name="noticeTransport"></param>
        /// <param name="contactInformationDTO"></param>
        /// <param name="transportDTO"></param>
        /// <param name="noticeCoverageDTO"></param>
        /// <returns></returns>
        [OperationContract]
        NoticeTransportDTO CreateNoticeTransport(NoticeTransportDTO noticeTransport, ContactInformationDTO contactInformationDTO, TransportDTO transportDTO);

        /// <summary>
        /// Actualizar aviso de transportes
        /// </summary>
        /// <param name="noticeTransport"></param>
        /// <param name="contactInformationDTO"></param>
        /// <param name="transportDTO"></param>
        /// <param name="noticeCoverageDTO"></param>
        /// <returns></returns>
        [OperationContract]
        NoticeTransportDTO UpdateNoticeTransport(NoticeTransportDTO noticeTransport, ContactInformationDTO contactInformationDTO, TransportDTO transportDTO);

        /// <summary>
        /// Obtener riesgos de transporte por identificador del asegurado
        /// </summary>
        /// <param name="insuredId"></param>
        /// <returns></returns>
        [OperationContract]
        List<TransportDTO> GetTransportsByInsuredId(int insuredId);

        /// <summary>
        /// Obtener riesgo del transporte de no seguro por identificador del aviso
        /// </summary>
        /// <param name="claimNoticeId"></param>
        /// <returns></returns>
        [OperationContract]
        TransportDTO GetRiskTransportByClaimNoticeId(int claimNoticeId);

        /// <summary>
        /// Obtener riesgo de transporte por su identificador
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        TransportDTO GetRiskTransportByRiskId(int riskId);

        /// <summary>
        /// Obtener riesgos de casco aviación por identificador del asegurado
        /// </summary>
        /// <param name="insuredId"></param>
        /// <returns></returns>
        [OperationContract]
        List<AirCraftDTO> GetRiskAirCraftsByInsuredId(int insuredId);

        /// <summary>
        /// Crear aviso de casco aviación
        /// </summary>
        /// <param name="noticeAirCraftDTO"></param>
        /// <param name="contactInformationDTO"></param>
        /// <param name="airCraftDTO"></param>
        /// <param name="noticeCoverageDTO"></param>
        /// <returns></returns>
        [OperationContract]
        NoticeAirCraftDTO CreateNoticeAirCraft(NoticeAirCraftDTO noticeAirCraftDTO, ContactInformationDTO contactInformationDTO, AirCraftDTO airCraftDTO);

        /// <summary>
        /// Actualizar aviso de casco aviación
        /// </summary>
        /// <param name="noticeAirCraftDTO"></param>
        /// <param name="contactInformationDTO"></param>
        /// <param name="airCraftDTO"></param>
        /// <param name="noticeCoverageDTO"></param>
        /// <returns></returns>
        [OperationContract]
        NoticeAirCraftDTO UpdateNoticeAirCraft(NoticeAirCraftDTO noticeAirCraftDTO, ContactInformationDTO contactInformationDTO, AirCraftDTO airCraftDTO);

        /// <summary>
        /// Obtener riesgos de casco aviación por su identificador
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        AirCraftDTO GetRiskAirCraftByRiskId(int riskId);

        /// <summary>
        /// Obtener riesgo de casco aviación de no seguro por identificador del aviso
        /// </summary>
        /// <param name="claimNoticeId"></param>
        /// <returns></returns>
        [OperationContract]
        AirCraftDTO GetRiskAirCraftByClaimNoticeId(int claimNoticeId);

        /// <summary>
        /// Obtener marcas del avion
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetAirCraftMakes();

        /// <summary>
        /// Obtener modelos del avion por identificador de la marca
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetAirCraftModelsByMakeId(int makeId);

        /// <summary>
        /// Obtener usos del casco por ramo
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetAirCraftUses();

        /// <summary>
        /// Obtener registros del casco avión
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetAircraftRegisters();

        /// <summary>
        /// Obtener operadores del casco avión
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetAircraftOperators();

        /// <summary>
        /// Crear denuncia de casco
        /// </summary>
        /// <param name="claimAirCraftDTO"></param>
        /// <param name="driversInformationDTO"></param>
        /// <param name="thirdPartiesVehicleDTO"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimAirCraftDTO CreateClaimAirCraft(ClaimAirCraftDTO claimAirCraftDTO);

        /// <summary>
        /// Actualizar denuncia de casco
        /// </summary>
        /// <param name="claimAirCraftDTO"></param>
        /// <param name="driversInformationDTO"></param>
        /// <param name="thirdPartiesVehicleDTO"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimAirCraftDTO UpdateClaimAirCraft(ClaimAirCraftDTO claimAirCraftDTO);

        /// <summary>
        /// Obtener riesgo de casco por identificador del endoso, el ramo y tipo de módulo
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="prefixId"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        [OperationContract]
        List<AirCraftDTO> GetRiskAirCraftByEndorsementIdPrefixId(int endorsementId, int prefixId);

        /// <summary>
        /// Obtener riesgos de manejo por identificador del asegurado
        /// </summary>
        /// <param name="insuredId"></param>
        /// <returns></returns>
        [OperationContract]
        List<FidelityDTO> GetRiskFidelitiesByInsuredId(int insuredId);

        /// <summary>
        /// Obtener riesgos de manejo por su identificador
        /// </summary>
        /// <param name="insuredId"></param>
        /// <returns></returns>
        [OperationContract]
        FidelityDTO GetRiskFidelityByRiskId(int riskId);

        /// <summary>
        /// Obtener actividad del riesgo de manejo
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetRiskCommercialClasses();

        /// <summary>
        /// Obtener ocupación del riesgo de manejo
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetOccupations();

        /// <summary>
        /// Crear un aviso de manejo
        /// </summary>
        /// <param name="noticeFidelityDTO"></param>
        /// <returns></returns>
        [OperationContract]
        NoticeFidelityDTO CreateNoticeFidelity(NoticeFidelityDTO noticeFidelityDTO, ContactInformationDTO contactInformationDTO, FidelityDTO fidelityDTO);

        /// <summary>
        /// Actualizar un aviso de manejo
        /// </summary>
        /// <param name="noticeFidelityDTO"></param>
        /// <returns></returns>
        [OperationContract]
        NoticeFidelityDTO UpdateNoticeFidelity(NoticeFidelityDTO noticeFidelityDTO, ContactInformationDTO contactInformationDTO, FidelityDTO fidelityDTO);

        /// <summary>
        /// Obtener riesgo de no seguro de manejo por identificador del aviso
        /// </summary>
        /// <param name="claimNoticeId"></param>
        /// <returns></returns>
        [OperationContract]
        FidelityDTO GetRiskFidelityByClaimNoticeId(int claimNoticeId);

        /// <summary>
        /// Crear denuncias de manejo
        /// </summary>
        /// <param name="claimFidelityDTO"></param>
        /// <param name="driversInformationDTO"></param>
        /// <param name="thirdPartiesVehicleDTO"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimFidelityDTO CreateClaimFidelity(ClaimFidelityDTO claimFidelityDTO);

        /// <summary>
        /// Actualizar denuncias de manejo
        /// </summary>
        /// <param name="claimFidelityDTO"></param>
        /// <param name="driversInformationDTO"></param>
        /// <param name="thirdPartiesVehicleDTO"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimFidelityDTO UpdateClaimFidelity(ClaimFidelityDTO claimFidelityDTO);

        /// <summary>
        /// Obtener riesgos de manejo por identificador del endoso y tipo de módulo
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        [OperationContract]
        List<FidelityDTO> GetRiskFidelitiesByEndorsementId(int endorsementId);

        /// <summary>
        /// Obtener siniestros de una poliza
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        List<ClaimDTO> GetClaimsByPolicyId(int policyId);


        /// <summary>
        /// Obtener siniestros de una póliza por fecha de ocurrencia
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="occurrenceDate"></param>
        /// <returns></returns>
        [OperationContract]
        List<ClaimDTO> GetClaimsByPolicyIdOccurrenceDate(int policyId, DateTime occurrenceDate);

        /// <summary>
        /// Obtener los limites por monto
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimLimitDTO GetInsuredAmount(int policyId, int riskNum, int coverageId, int coverNum, int claimId, int subClaimId);

        /// <summary>
        /// Envia el correo notificando el agendamiento del aviso
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="mailDestination"></param>
        [OperationContract]
        void SendEmailToAgendNotice(string subject, string message, string mailDestination);

        /// <summary>
        /// Crear el archivo ICS para el agendamiento del aviso
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="startEventDate"></param>
        /// <returns></returns>
        [OperationContract]
        string ScheduleNotice(string subject, string message, DateTime startEventDate, DateTime finishEventDate);

        /// <summary>
        /// Obtener los Deducibles 
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="riskNum"></param>
        /// <param name="coverageId"></param>
        /// <param name="coverNum"></param>
        /// <returns></returns>
        [OperationContract]
        List<CoverageDeductibleDTO> GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(int policyId, int riskNum, int coverageId, int coverNum);

        /// <summary>
        /// Obtener subcausas del siniestro por causas
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SubCauseDTO> GetSubCausesByCause(int CauseId);

        /// <summary>
        /// Guarda las subcausas
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        [OperationContract]
        SubCauseDTO CreateSubCause(SubCauseDTO subCause);

        /// <summary>
        /// Actualiza la sub causa
        /// </summary>
        /// <param name="subCause"></param>
        /// <returns></returns>
        [OperationContract]
        SubCauseDTO UpdateSubCause(SubCauseDTO subCause);

        /// <summary>
        /// Elimina la sub causa
        /// </summary>
        /// <param name="subCauseId"></param>
        [OperationContract]
        void DeleteSubCause(int subCauseId);

        /// <summary>
        /// Obtiene los modulos
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [OperationContract]
        List<ModuleDTO> GetModule(string description);

        /// <summary>
        /// Obtiene los submodulos por moduloId
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SubModuleDTO> GetSubModule(int moduleId);

        /// <summary>
        /// Obtiene los documentos por submoduleId
        /// </summary>
        /// <param name="SubmoduleId"></param>
        /// <returns></returns>
        [OperationContract]
        List<ClaimDocumentationDTO> GetDocumentationBySubmoduleId(int SubmoduleId);

        /// <summary>
        /// Elimina la documentación
        /// </summary>
        /// <param name="DocumentationId"></param>
        [OperationContract]
        void DeleteDocumentation(int DocumentationId);

        /// <summary>
        /// Crea la documentación
        /// </summary>
        /// <param name="claimsDocumentationDTO"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimDocumentationDTO CreateDocumentationes(ClaimDocumentationDTO claimsDocumentationDTO);

        /// <summary>
        /// Actualiza la documentación
        /// </summary>
        /// <param name="claimsDocumentationDTO"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimDocumentationDTO UpdateDocumentation(ClaimDocumentationDTO claimsDocumentationDTO);

        /// <summary>
        /// Crear concepto por cobertura
        /// </summary>
        /// <param name="coverageId"></param>
        /// <param name="PaymentConceptDTO"></param>
        /// <returns></returns>
        [OperationContract]
        CoveragePaymentConceptDTO CreatePaymentConcept(CoveragePaymentConceptDTO coveragePaymentConceptDTO);

        /// <summary>
        /// Eliminar concepto asignado por cobertura y concepto Id
        /// </summary>
        /// <param name="conceptId"></param>
        /// <param name="coverageId"></param>
        [OperationContract]
        void DeletePaymentConcept(int conceptId, int coverageId, int estimationTypeId);

        /// <summary>
        /// Obtiene los conceptos de pago por identificador de la cobertura
        /// </summary>
        /// <param name="covergaeId"></param>
        /// <returns></returns>
        [OperationContract]
        List<PaymentConceptDTO> GetPaymentConceptsByCoverageIdEstimationTypeId(int coverageId, int estimationTypeId);

        /// <summary>
        /// Obtiene el listado de impuestos que posee el individuo
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [OperationContract]
        List<IndividualTaxDTO> GetIndividualTaxesByIndividualIdRoleId(int individualId, int roleId);

        /// <summary>
        /// Obtiene los bienes afectados por claimModifyId Y CoverageId
        /// </summary>
        /// <param name="claimCoverageId"></param>
        /// <returns></returns>
        [OperationContract]
        string GetAffectedPropertyByClaimCoverageId(int claimCoverageId);

        /// <summary>
        /// Guarda tercero
        /// </summary>
        /// <param name="claimsPatitionDTO"></param>
        /// <returns></returns>
        [OperationContract]
        ClaimParticipantDTO CreateParticipant(ClaimParticipantDTO claimsParticipantDTO);

        /// <summary>
        /// Consulta los terceros por cobertura
        /// </summary>
        /// <param name="claimCoverageId"></param>
        /// <returns></returns>
        [OperationContract]
        List<ThirdAffectedDTO> GetThirdAffectedByClaimCoverageId(int claimCoverageId);

        /// <summary>
        /// Consulta los terceros creados
        /// </summary>
        /// <param name="description"></param>
        /// <param name="insuredSearchType"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        [OperationContract]
        List<ClaimParticipantDTO> GetParticipantsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        /// <summary>
        /// consulta el tercero por individual Id
        /// </summary>
        /// <param name="IndividualId"></param>
        [OperationContract]
        ClaimParticipantDTO GetParticipantByParticipantId(int participantId);

        /// <summary>
        /// Crea el temporal de siniestros
        /// </summary>
        /// <param name="pendingOperation"></param>
        /// <returns></returns>
        [OperationContract]
        PendingOperationDTO CreatePendingOperation(PendingOperationDTO pendingOperation);

        /// <summary>
        /// Consulta el temporal de siniestros
        /// </summary>
        /// <param name="pendingOperationId"></param>
        /// <returns></returns>
        [OperationContract]
        PendingOperationDTO GetPendingOperationByPendingOperationId(int pendingOperationId);

        /// <summary>
        /// Elimina el temporal de siniestros
        /// </summary>
        /// <param name="pendingOperationId"></param>
        [OperationContract]
        void DeletePendingOperationByPendingOperationId(int pendingOperationId);

        /// <summary>
        /// Crea la denuncia a partir de un temporal
        /// </summary>
        /// <param name="temporalId"></param>
        [OperationContract]
        void CreateClaimByTemporalId(int temporalId);

        /// <summary>
        /// crea el aviso a partir del temporal
        /// </summary>
        /// <param name="noticeTemporalId"></param>
        [OperationContract]
        void CreateClaimNoticeByTemporalId(int noticeTemporalId);

        /// <summary>
        /// Consulta los motivos por StatusReasonId,statusId,prefixId
        /// </summary>
        /// <param name="StatusReasonId"></param>
        /// <param name="statusId"></param>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        [OperationContract]
        List<ReasonDTO> GetReasonsByPrefixId(int prefixId);
        /// <summary>
        /// consulta todas las modificaciones que se han realizado a un siniestros
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SubClaimDTO> GetClaimModifiesByClaimId(int claimId);

        [OperationContract]
        List<SubClaimDTO> UpdateEstimationsSalaries(List<SubClaimDTO> subClaimsDTO, int currentYear);

        [OperationContract]
        List<CoInsuranceAssignedDTO> GetCoInsuranceByPolicyIdByEndorsementId(int endorsementId, int policyId);

        [OperationContract]
        ClaimCoverageDTO GetClaimedAmountByClaimCoverageId(int claimCoverageId);

        [OperationContract]
        List<AmountTypeDTO> GetAmountType();

        [OperationContract]
        MinimumSalaryDTO GetMinimumSalaryByYear(int year);

        /// <summary>
        /// Obtiene si para el producto en cuestion está activa la fecha de acto administrativo
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        bool GetJudicialDecisionDateIsActiveByPrefixId(int prefixId);

        [OperationContract]
        List<StatusDTO> GetEstimationTypeStatusUnassignedByEstimationTypeId(int estimationTypeId);

        /// <summary>
        /// Obtiene los posibles beneficiarios de un pago por nombre o número de documento
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        List<IndividualDTO> GetPaymentBeneficiariesByDescription(string description);

        /// <summary>
        /// Obtiene un posible beneficiario de un pago por el individualId
        /// </summary>
        /// <param name="beneficiaryId"></param>
        /// <returns></returns>
        [OperationContract]
        IndividualDTO GetPaymentBeneficiaryByBeneficiaryId(int beneficiaryId);

        /// <summary>
        /// Obtiene los identificadores de los tipos de estimación a los que aplica la estimación en salarios
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<int> GetEstimationTypesSalariesEstimation();
    }
}
