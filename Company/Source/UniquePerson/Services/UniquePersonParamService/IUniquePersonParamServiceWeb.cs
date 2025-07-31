using Sistran.Company.Application.ModelServices.Models;
using Sistran.Company.Application.ModelServices.Models.UniquePerson;
using Sistran.Company.Application.UniquePersonParamService.Models;
using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.UniquePersonParamService;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using modelsServiceCompany = Sistran.Company.Application.ModelServices.Models.Param;

namespace Sistran.Company.Application.UniquePersonParamService
{
    /// <summary>
    /// Interfaz de parametrización.
    /// </summary>
    [ServiceContract]
    public interface IUniquePersonParamServiceWeb : IUniquePersonParamServiceWebCore
    {

        /// <summary>
        /// /// Obtiene la lista de tipos de riesgo cubierto.
        /// </summary>
        /// <returns>Modelo de sevicio del tipo de riesgo cubierto.</returns>
        [OperationContract]
        LegalRepresentativesSingServiceModel GetLstCptLegalReprSign();

        /// <summary>
        /// Método que obtine una Firma Representante Legal por Id
        /// </summary>
        /// <param name="ciaCode">Id Compañia</param>
        /// <param name="branchTypeCode">Id Sucursal</param>
        /// <param name="currentFrom">Fecha actual</param>
        /// <returns>una Firma Representante Legal consultada</returns>
        [OperationContract]
        LegalRepresentativeSingServiceModel GetCptLegalReprSignByCiaCodeBranchTypeCodeCurrentFrom(decimal ciaCode, decimal branchTypeCode, DateTime currentFrom);

        /// <summary>
        /// Guarda los registros nuevos y editados
        /// </summary>
        /// <param name="legalRepresentativesSingServiceModel">Objeto de modelo de servicio legalRepresentativesSingServiceModel</param>
        /// <returns>Objecto ParametrizationResponse</returns>
        [OperationContract]
        ParametrizationResponse<LegalRepresentativesSingServiceModel> CreateLegalRepresentativeSing(LegalRepresentativesSingServiceModel legalRepresentativesSingServiceModel);

        /// <summary>
        /// Genera el archivo de Excel de firma de representante legal
        /// </summary>
        /// <param name="legalRepresentativesSingServiceModel">Lista de firma de representante legal</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>String de respuesta</returns>
        [OperationContract]
        ExcelFileServiceModel GenerateFileToLegalRepresentativeSing(LegalRepresentativesSingServiceModel legalRepresentativesSingServiceModel, string fileName);


        /// <summary>
        /// Obtiene la lista de tipo de compañia.
        /// </summary>
        /// <returns>Lista de tipo de compañia consultadas</returns>
        [OperationContract]
        CompanyTypesServiceModel GetLstCompanyTypes();

        /// <summary>
        /// Obtiene la lista de tipo de sucursal.
        /// </summary>
        /// <returns>Lista de tipo de sucursal consultadas</returns>
        [OperationContract]
        BranchTypesServiceModel GetLstBranchTypes();

        #region ALLIANCE
        /// <summary>
        /// Obtiene la lista de aliados
        /// </summary>
        /// <returns>Listado de aliados</returns>
        [OperationContract]
        List<Alliance> GetAllAlliances();

        /// <summary>
        /// Obtiene la lista de sucursales de un aliado
        /// </summary>
        /// <param name="allianceId">Identificdor del aliado</param>
        /// <returns>Listado de sucursales</returns>
        [OperationContract]
        List<BranchAlliance> GetAllBranchAlliancesByAlliancedId(int allianceId);

        /// <summary>
        /// Obtiene la lista de sucursales (aliados)
        /// </summary>
        /// <returns>Listado de sucursales</returns>
        [OperationContract]
        List<BranchAlliance> GetAllBranchAlliances();

        /// <summary>
        /// Obtiene la lista de puntos de venta de sucursales de un aliado
        /// </summary>
        /// <returns>Listado de puntos de venta</returns>
        [OperationContract]
        List<AllianceBranchSalePonit> GetAllSalesPointsByBranchId(int branchId, int allianceId);

        /// <summary>
        /// Obtiene la lista de todos los puntos de venta aliados
        /// </summary>
        /// <returns>Listado de puntos de venta</returns>
        [OperationContract]
        List<AllianceBranchSalePonit> GetAllSalesPointsAlliance();

        /// <summary>
        /// Ejecuta las operaciones de Crear, modificar y borrar aliados
        /// </summary>
        /// <param name="alliances">Listado de aliados</param>
        /// <returns>Listado de todos los aliados</returns>
        [OperationContract]
        List<string> ExecuteOprationsAlliances(List<Alliance> alliances);

        /// <summary>
        /// Obtiene los aliados por descripción
        /// </summary>
        /// <param name="description">Descripción</param>
        /// <returns>Listado de aliados</returns>
        [OperationContract]
        List<Alliance> GetAllianceByDescription(string description);

        /// <summary>
        /// Ejecuta las operaciones de Crear, modificar y borrar sucursales de aliados
        /// </summary>
        /// <param name="alliances">Listado de sucursales</param>
        /// <returns>Listado de las sucursales los aliados</returns>
        [OperationContract]
        List<BranchAlliance> ExecuteOprationsBranchAlliances(List<BranchAlliance> branchAlliance);

        /// <summary>
        /// Consulta sucursales de aliado por el nombre
        /// </summary>
        /// <param name="description">Nombre de la sucursal</param>
        /// <returns>sucursales de aliado</returns>
        [OperationContract]
        List<BranchAlliance> GetBranchAllianceByDescription(string description);

        /// <summary>
        /// Genera archivo excel para aliados
        /// </summary>
        /// <param name="alliancesList">Lista de todos los aliados</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Url de desacarga del archivo</returns>
        [OperationContract]
        string GenerateFileToAlliance(List<Alliance> alliancesList, string fileName);

        /// <summary>
        /// Genera archivo excel para sucursal aliados
        /// </summary>
        /// <param name="branchAlliancesList">Lista de todas las sucursales de aliados</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Url de desacarga del archivo</returns>
        [OperationContract]
        string GenerateFileToBranchAlliance(List<BranchAlliance> branchAlliancesList, string fileName);

        /// <summary>
        /// Genera archivo excel para puntos de venta de aliados
        /// </summary>
        /// <param name="salePointsAlliancesList">Lista de todos los puntos de venta de aliados</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Url de desacarga del archivo</returns>
        [OperationContract]
        string GenerateFileToSalePointsAlliance(List<AllianceBranchSalePonit> salePointsAlliancesList, string fileName);

        /// <summary>
        /// Obtener Agencias por agente
        /// </summary>        
        /// <param name="description">Código o Nombre</param>
        /// <returns>Agencias</returns>
        [OperationContract]
        List<SmAgentAgency> GetAgenAgencyByAgentIdDescription(string description);

        /// <summary>
        /// Obtiene agente por agencia.
        /// </summary>
        /// <param name="individualId">Identificador de la agencia.</param>
        /// <param name="agentAgencyId">Identificador del agente de la agencia.</param>
        /// <returns></returns>
        [OperationContract]
        SmAgentAgency GetAgentAgencyByPrimaryKey(int individualId, int agentAgencyId);

        /// <summary>
        /// Obtener aliado por Identificadores.
        /// </summary>
        /// <param name="individualId">Id del individuo.</param>
        /// <param name="agentAgencyId">Id agencia</param>
        /// <returns>Listado de Aliados</returns>
        [OperationContract]
        List<SmAlly> GetAllyByIntermediary(int individualId, int agentAgencyId);

        /// <summary>
        /// Listado de agencias de un intermediario
        /// </summary>
        /// <param name="individualId">Identificador del agente</param>
        /// <returns>Listado de agencias del intermediario</returns>
        //[OperationContract]
        //List<modelsUPersonCore.AgentAgency> GetAgenciesAgentByIndividualId(int individualId);
        #endregion

        /// <summary>
        /// Obtiene la lista de tipos de documento de persona jurídica y natural
        /// </summary>
        /// <returns>lista de tipos de documento de persona jurídica y natural</returns>
        [OperationContract]
        DocumentTypesServiceModel GetDocumentTypes();

        /// <summary>
        ///    Obtiene la lista del país ciudad estado
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        CountriesStatesCitiesServiceModel GetCountriesStatesCities();


        /// <summary>
        /// Actualiza la Informacion basica de la persona
        /// </summary>
        /// <param name="personServiceModel">Objeto de modelo de servicio BasicPersonServiceModel</param>
        /// <returns>Modelo BasicPersonsServiceModel</returns>
        [OperationContract]
        BasicPersonsServiceModel SavePersonBasic(BasicPersonServiceModel personServiceModel);

        /// <summary>
        /// Actualiza la Informacion basica de la compañia
        /// </summary>
        /// <param name="companyServiceModel">Objeto de modelo de servicio BasicCompanysServiceModel</param>
        /// <returns>Modelo BasicCompanysServiceModel</returns>
        [OperationContract]
        BasicCompanysServiceModel SaveCompanyBasic(BasicCompanyServiceModel companyServiceModel);

        /// <summary>
        ///  Obtener Información basica de persona por codigo de persona o primer apellido o segundo apellido o nombres  o número de documento
        /// </summary>
        /// <param name="codePerson"> Código de Persona </param>
        /// <param name="firstName"> Primer Apellido </param>
        /// <param name="lastName"> Segundo Apellido </param>
        /// <param name="name"> Nombre(s)</param>
        /// <param name="documentNumber"> Número de Documento</param>
        /// <returns>MOdelo BasicPersonsServiceModel</returns>
        [OperationContract]
        BasicPersonsServiceModel GetPersonBasicByCodePersonByFirtsNameByLastNameByNameByTypeDocumentByDocumentNumber(string codePerson, string firstName, string lastName, string name, string documentNumber,string typeDocument);

        /// <summary>
        ///  Obtener Información basica de la Compañia por codigo de compañia o nombre de razon social o número de documento
        /// </summary>
        /// <param name="codeCompany"> Código de Compañia </param>
        /// <param name="tradeName"> Nombre de Razón social </param>
        /// <param name="tradeName"> Nombre de Razón social </param>
        /// <param name="documentNumber"> Número de Documento</param>
        /// <returns>Modelo BasicCompanysServiceModel</returns>
        [OperationContract]
        BasicCompanysServiceModel GetCompanyBasicByCodeCompanyByTradeNameByTypeDocumentByDocumentNumber(string codeCompany, string tradeName, string documentNumber, string typeDocument);

        /// <summary>
        ///  Obtener Información basica de persona por codigo de persona
        /// </summary>
        /// <param name="documentNumber"> No. Documento de Persona </param>
        /// <returns>Modelo BasicPersonsServiceModel</returns>
        [OperationContract]
        BasicPersonsServiceModel GetPersonBasicByDocumentNumber(string documentNumber);

        /// <summary>
        ///  Obtener Información basica de la Compañia por codigo de compañia
        /// </summary>
        /// <param name="documentNumber"> No. Documento de la Compañia </param>
        /// <returns>Modelo BasicCompanysServiceModel</returns>
        [OperationContract]
        BasicCompanysServiceModel GetCompanyBasicByDocumentNumber(string documentNumber);


        /// <summary>
        /// Obtiene la lista de dirrecciones de una persona 
        /// </summary>
        /// <returns>lista de dirrecciones de una persona</returns>
        [OperationContract]
        modelsServiceCompany.GenericModelsServicesQueryModel GetAddress(Boolean? isEmail);

       
    }
}
