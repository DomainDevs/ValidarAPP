using Sistran.Company.Application.ModelServices.Models.UniquePerson;
using Sistran.Company.Application.UniquePersonParamService.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonParamService.EEProvider.DAOs;
using Sistran.Company.Application.UniquePersonParamService.Models;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.Utilities.Error;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.UniquePersonParamService.EEProvider.Resources;
using Sistran.Company.Application.ModelServices.Models;
using modelsServiceCompany = Sistran.Company.Application.ModelServices.Models.Param;
using daosCompany = Sistran.Company.Application.UniquePersonParamService.EEProvider.DAOs;
using enumsCompany = Sistran.Company.Application.ModelServices.Enums;

namespace Sistran.Company.Application.UniquePersonParamService.EEProvider
{
    /// <summary>
    /// Clase que implementa la interfaz IUnderwritingParamServiceWeb.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UniquePersonParamServiceEEProviderWeb : Sistran.Core.Application.UniquePersonParamServices.EEProvider.UniquePersonParamServiceEEProviderWebCore, IUniquePersonParamServiceWeb
    {
        /// <summary>
        /// Obtiene la lista de Firma Representante Legal.
        /// </summary>
        /// <returns>Lista de Firma Representante Legal consultadas</returns>
        public LegalRepresentativesSingServiceModel GetLstCptLegalReprSign()
        {
            LegalRepresentativeSingDAO legalRepresentativeSingDAO = new LegalRepresentativeSingDAO();
            BranchTypeDAO branchTypeDAO = new BranchTypeDAO();
            DAOs.CompanyTypeDAO companyTypeDAO = new DAOs.CompanyTypeDAO();
            LegalRepresentativesSingServiceModel legalRepresentativesSingServiceModel = new LegalRepresentativesSingServiceModel();
            Result<List<ParamLegalRepresentativeSing>, ErrorModel> resultGetLstCptLegalReprSign = legalRepresentativeSingDAO.GetLstCptLegalReprSign();
            Result<List<ParamBranchType>, ErrorModel> resultGetBranchTypes = branchTypeDAO.GetBranchTypes();
            Result<List<ParamCompanyType>, ErrorModel> resultGetCompanyTypes = companyTypeDAO.GetCompanyTypes();
            if (resultGetLstCptLegalReprSign is ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetLstCptLegalReprSign as ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>).Message;
                legalRepresentativesSingServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                legalRepresentativesSingServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamLegalRepresentativeSing> resultValue = (resultGetLstCptLegalReprSign as ResultValue<List<ParamLegalRepresentativeSing>, ErrorModel>).Value;
                List<ParamBranchType> resultValueBranchTypes = (resultGetBranchTypes as ResultValue<List<ParamBranchType>, ErrorModel>).Value;
                List<ParamCompanyType> resultValueCompanyTypes = (resultGetCompanyTypes as ResultValue<List<ParamCompanyType>, ErrorModel>).Value;
                legalRepresentativesSingServiceModel = ModelServiceAssembler.MappLegalRepresentativesSing(resultValue, resultValueBranchTypes, resultValueCompanyTypes);
            }

            return legalRepresentativesSingServiceModel;
        }

        /// <summary>
        /// Método que obtine una Firma Representante Legal por Id
        /// </summary>
        /// <param name="ciaCode">Id Compañia</param>
        /// <param name="branchTypeCode">Id Sucursal</param>
        /// <param name="currentFrom">Fecha actual</param>
        /// <returns>una Firma Representante Legal consultada</returns>
        public LegalRepresentativeSingServiceModel GetCptLegalReprSignByCiaCodeBranchTypeCodeCurrentFrom(decimal ciaCode, decimal branchTypeCode, DateTime currentFrom)
        {
            LegalRepresentativeSingDAO legalRepresentativeSingDAO = new LegalRepresentativeSingDAO();
            LegalRepresentativeSingServiceModel legalRepresentativeSingServiceModel = new LegalRepresentativeSingServiceModel();
            Result<ParamLegalRepresentativeSing, ErrorModel> result = legalRepresentativeSingDAO.GetCptLegalReprSignByCiaCodeBranchTypeCodeCurrentFrom(ciaCode, branchTypeCode, currentFrom);
            if (result is ResultError<ParamLegalRepresentativeSing, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<ParamLegalRepresentativeSing, ErrorModel>).Message;
                legalRepresentativeSingServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                legalRepresentativeSingServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                ParamLegalRepresentativeSing resultValue = (result as ResultValue<ParamLegalRepresentativeSing, ErrorModel>).Value;
                legalRepresentativeSingServiceModel = ModelServiceAssembler.MappLegalRepresentativeSing(resultValue);
            }

            return legalRepresentativeSingServiceModel;
        }

        /// <summary>
        /// Guarda los registros nuevos y editados
        /// </summary>
        /// <param name="legalRepresentativesSingServiceModel">Objeto de modelo de servicio legalRepresentativesSingServiceModel</param>
        /// <returns>Objecto ParametrizationResponse</returns>
        public ParametrizationResponse<LegalRepresentativesSingServiceModel> CreateLegalRepresentativeSing(LegalRepresentativesSingServiceModel legalRepresentativesSingServiceModel)
        {
            ParametrizationResponse<LegalRepresentativesSingServiceModel> legalRepresentativesSingServiceModelReturn = new ParametrizationResponse<LegalRepresentativesSingServiceModel>();
            LegalRepresentativeSingDAO legalRepresentativeSingDAO = new LegalRepresentativeSingDAO();
            List<ParamLegalRepresentativeSing> resultValueListAdd = new List<ParamLegalRepresentativeSing>();
            List<ParamLegalRepresentativeSing> resultValueListModify = new List<ParamLegalRepresentativeSing>();

            List<LegalRepresentativeSingServiceModel> filterListAdd = ModelServiceAssembler.MappLegalRepresentativeSingByStatusType(legalRepresentativesSingServiceModel.LegalRepresentativeSingServiceModel, StatusTypeService.Create);
            List<LegalRepresentativeSingServiceModel> filterListModify = ModelServiceAssembler.MappLegalRepresentativeSingByStatusType(legalRepresentativesSingServiceModel.LegalRepresentativeSingServiceModel, StatusTypeService.Update);

            Result<List<ParamLegalRepresentativeSing>, ErrorModel> resultListAdd = ServicesModelsAssembler.MappListParamLegalRepresentativeSing(filterListAdd);
            if (resultListAdd is ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultListAdd as ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>).Message;
                legalRepresentativesSingServiceModelReturn.ErrorAdded = string.Join(" <br/> ", errorModelResult.ErrorDescription);
            }
            else
            {
                resultValueListAdd = (resultListAdd as ResultValue<List<ParamLegalRepresentativeSing>, ErrorModel>).Value;
            }

            Result<List<ParamLegalRepresentativeSing>, ErrorModel> resultListModify = ServicesModelsAssembler.MappListParamLegalRepresentativeSing(filterListModify);
            if (resultListModify is ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultListModify as ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>).Message;
                legalRepresentativesSingServiceModelReturn.ErrorModify = string.Join(" <br/> ", errorModelResult.ErrorDescription);
            }
            else
            {
                resultValueListModify = (resultListModify as ResultValue<List<ParamLegalRepresentativeSing>, ErrorModel>).Value;
            }

            ParametrizationResponse<ParamLegalRepresentativeSing> parametrizationBusinessReturn = legalRepresentativeSingDAO.SaveLegalRepresentativeSing(resultValueListAdd, resultValueListModify);

            legalRepresentativesSingServiceModelReturn.ErrorAdded = parametrizationBusinessReturn.ErrorAdded;
            legalRepresentativesSingServiceModelReturn.ErrorModify = parametrizationBusinessReturn.ErrorModify;
            legalRepresentativesSingServiceModelReturn.TotalAdded = parametrizationBusinessReturn.TotalAdded;
            legalRepresentativesSingServiceModelReturn.TotalModify = parametrizationBusinessReturn.TotalModify;

            return legalRepresentativesSingServiceModelReturn;
        }

        /// <summary>
        /// Genera el archivo de Excel de firma de representante legal
        /// </summary>
        /// <param name="legalRepresentativesSingServiceModel">Lista de firma de representante legal</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>String de respuesta</returns>
        public ExcelFileServiceModel GenerateFileToLegalRepresentativeSing(LegalRepresentativesSingServiceModel legalRepresentativesSingServiceModel, string fileName)
        {
            LegalRepresentativeSingDAO legalRepresentativeSingDAO = new LegalRepresentativeSingDAO();
            Result<List<ParamLegalRepresentativeSing>, ErrorModel> resultParamLegalRepresentativeSing = ServicesModelsAssembler.MappListParamLegalRepresentativeSing(legalRepresentativesSingServiceModel.LegalRepresentativeSingServiceModel);
            List<ParamLegalRepresentativeSing> resultValue;
            if (resultParamLegalRepresentativeSing is ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultParamLegalRepresentativeSing as ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>).Message;
                return new ExcelFileServiceModel()
                {
                    ErrorDescription = errorModelResult.ErrorDescription,
                    ErrorTypeService = ErrorTypeService.TechnicalFault
                };
            }

            resultValue = (resultParamLegalRepresentativeSing as ResultValue<List<ParamLegalRepresentativeSing>, ErrorModel>).Value;
            ExcelFileServiceModel excelFileServiceModel = legalRepresentativeSingDAO.GenerateFileToLegalRepresentativeSing(resultValue, fileName);
            if (excelFileServiceModel.ErrorTypeService != ErrorTypeService.Ok)
            {
                return excelFileServiceModel;
            }

            return new ExcelFileServiceModel()
            {
                ErrorTypeService = ErrorTypeService.Ok,
                FileData = excelFileServiceModel.FileData
            };
        }

        /// <summary>
        /// Obtiene la lista de tipo de compañia.
        /// </summary>
        /// <returns>Lista de tipo de compañia consultadas</returns>
        public CompanyTypesServiceModel GetLstCompanyTypes()
        {
            DAOs.CompanyTypeDAO companyTypeDAO = new DAOs.CompanyTypeDAO();
            CompanyTypesServiceModel companyTypesServiceServiceModel = new CompanyTypesServiceModel();
            Result<List<ParamCompanyType>, ErrorModel> result = companyTypeDAO.GetCompanyTypes();
            if (result is ResultError<List<ParamCompanyType>, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<List<ParamCompanyType>, ErrorModel>).Message;
                companyTypesServiceServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                companyTypesServiceServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamCompanyType> resultValue = (result as ResultValue<List<ParamCompanyType>, ErrorModel>).Value;
                companyTypesServiceServiceModel = ModelServiceAssembler.MappCompanyTypes(resultValue);
            }

            return companyTypesServiceServiceModel;
        }

        /// <summary>
        /// Obtiene la lista de tipo de sucursal.
        /// </summary>
        /// <returns>Lista de tipo de sucursal consultadas</returns>
        public BranchTypesServiceModel GetLstBranchTypes()
        {
            BranchTypeDAO branchTypeDAO = new BranchTypeDAO();
            BranchTypesServiceModel branchTypesServiceServiceModel = new BranchTypesServiceModel();
            Result<List<ParamBranchType>, ErrorModel> result = branchTypeDAO.GetBranchTypes();
            if (result is ResultError<List<ParamBranchType>, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<List<ParamBranchType>, ErrorModel>).Message;
                branchTypesServiceServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                branchTypesServiceServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamBranchType> resultValue = (result as ResultValue<List<ParamBranchType>, ErrorModel>).Value;
                branchTypesServiceServiceModel = ModelServiceAssembler.MappBranchTypes(resultValue);
            }

            return branchTypesServiceServiceModel;
        }

        #region ALLIANCE

        /// <summary>
        /// Obtines todos los aliados.
        /// </summary>
        /// <returns>Listado de aliados</returns>
        public List<Alliance> GetAllAlliances()
        {
            try
            {
                AllianceDAO alliancesDAO = new AllianceDAO();
                return alliancesDAO.GetAllAlliances();
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorGetAlliances, ex);
            }
        }

        /// <summary>
        /// Ejecuta las operaciones de Crear, modificar y borrar aliados
        /// </summary>
        /// <param name="alliances">Listado de aliados</param>
        /// <returns>Listado de todos los aliados</returns>
        public List<string> ExecuteOprationsAlliances(List<Alliance> alliances)
        {
            try
            {
                AllianceDAO alliancesDAO = new AllianceDAO();
                return alliancesDAO.ExecuteOprationsAlliances(alliances);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorSaveAlliances, ex);
            }
        }

        /// Obtiene los aliados por descripción
        /// </summary>
        /// <param name="description">Descripción</param>
        /// <returns>Listado de aliados</returns>
        public List<Alliance> GetAllianceByDescription(string description)
        {
            try
            {
                AllianceDAO alliancesDAO = new AllianceDAO();
                return alliancesDAO.GetAllianceByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorGetBranchAlliances, ex);
            }
        }

        /// <summary>
        /// Genera archivo excel para aliados
        /// </summary>
        /// <param name="alliancesList">Lista de todos los aliados</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Url de desacarga del archivo</returns>
        public string GenerateFileToAlliance(List<Alliance> alliancesList, string fileName)
        {
            try
            {
                CompanyFileDAO companyFileDAO = new CompanyFileDAO();
                return companyFileDAO.GenerateFileToAlliance(alliancesList, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGenerateFileToAlliance), ex);
            }
        }

        /// <summary>
        /// Obtiene la lista de sucursales de un aliado
        /// </summary>
        /// <param name="allianceId">Identificdor del aliado</param>
        /// <returns>Listado de sucursales</returns>
        public List<BranchAlliance> GetAllBranchAlliancesByAlliancedId(int allianceId)
        {
            try
            {
                AllianceDAO alliancesDAO = new AllianceDAO();
                return alliancesDAO.GetAllBranchAlliancesByAlliancedId(allianceId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorGetBranchAlliances, ex);
            }
        }

        /// <summary>
        /// Obtiene la lista de sucursales (aliados)
        /// </summary>
        /// <returns>Listado de sucursales</returns>
        public List<BranchAlliance> GetAllBranchAlliances()
        {
            try
            {
                AllianceDAO alliancesDAO = new AllianceDAO();
                return alliancesDAO.GetAllBranchAlliances();
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorGetBranchAlliances, ex);
            }
        }

        /// <summary>
        /// Obtiene la lista de puntos de venta de sucursales de un aliado
        /// </summary>
        /// <param name="branchId">Identificador de la sucursal</param>
        /// <returns>Listado de puntos de venta</returns>
        public List<AllianceBranchSalePonit> GetAllSalesPointsByBranchId(int branchId, int allianceId)
        {
            try
            {
                AllianceDAO alliancesDAO = new AllianceDAO();
                return alliancesDAO.GetAllSalesPointsByBranchId(branchId, allianceId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorGetBranchAlliances, ex);
            }
        }

        /// <summary>
        /// Obtiene la lista de todos los puntos de venta aliados
        /// </summary>
        /// <returns>Listado de puntos de venta</returns>
        public List<AllianceBranchSalePonit> GetAllSalesPointsAlliance()
        {
            try
            {
                AllianceDAO alliancesDAO = new AllianceDAO();
                return alliancesDAO.GetAllSalesPointsAlliance();
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorGetBranchAlliances, ex);
            }
        }


        /// Ejecuta las operaciones de Crear, modificar y borrar sucursales de aliados
        /// </summary>
        /// <param name="alliances">Listado de aliados</param>
        /// <returns>Listado de todos los aliados</returns>
        public List<BranchAlliance> ExecuteOprationsBranchAlliances(List<BranchAlliance> branchAlliance)
        {
            try
            {
                AllianceDAO alliancesDAO = new AllianceDAO();
                List<BranchAlliance> branches = alliancesDAO.ExecuteOprationsBranchAlliances(branchAlliance);
                foreach (BranchAlliance item in branches)
                {
                    BranchAlliance branch = branchAlliance.Find(x => x.AllianceId == item.AllianceId && x.BranchDescription == item.BranchDescription);
                    if (branch != null && branch.Status != "delete")
                    {
                        item.SalesPointsAlliance = alliancesDAO.ExecuteOprationsSalesPointsAlliances(branch.SalesPointsAlliance);
                    }
                }
                return branches;
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorSaveAlliances, ex);
            }
        }

        /// <summary>
        /// Consulta sucursales de aliado por el nombre
        /// </summary>
        /// <param name="description">Nombre de la sucursal</param>
        /// <returns>sucursales de aliado</returns>
        public List<BranchAlliance> GetBranchAllianceByDescription(string description)
        {
            try
            {
                AllianceDAO alliancesDAO = new AllianceDAO();
                return alliancesDAO.GetBranchAllianceByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorGetBranchAlliances, ex);
            }
        }

        /// <summary>
        /// Genera archivo excel para sucursal aliados
        /// </summary>
        /// <param name="branchAlliancesList">Lista de todas las sucursales de aliados</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Url de desacarga del archivo</returns>
        public string GenerateFileToBranchAlliance(List<BranchAlliance> branchAlliancesList, string fileName)
        {
            try
            {
                CompanyFileDAO companyFileDAO = new CompanyFileDAO();
                return companyFileDAO.GenerateFileToBranchAlliance(branchAlliancesList, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGenerateFileToAlliance), ex);
            }
        }

        /// <summary>
        /// Genera archivo excel para puntos de venta de aliados
        /// </summary>
        /// <param name="salePointsAlliancesList">Lista de todos los puntos de venta de aliados</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Url de desacarga del archivo</returns>
        public string GenerateFileToSalePointsAlliance(List<AllianceBranchSalePonit> salePointsAlliancesList, string fileName)
        {
            try
            {
                CompanyFileDAO companyFileDAO = new CompanyFileDAO();
                return companyFileDAO.GenerateFileToSalePointsAlliance(salePointsAlliancesList, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGenerateFileToAlliance), ex);
            }
        }

        /// <summary>
        /// Obtener Agencias por agente
        /// </summary>        
        /// <param name="description">Código o Nombre</param>
        /// <returns>Agencias</returns>
        public List<SmAgentAgency> GetAgenAgencyByAgentIdDescription(string description)
        {
            try
            {
                CompanyAgentAgencyDAO agentAgencyDAO = new CompanyAgentAgencyDAO();
                return agentAgencyDAO.GetAgenAgencyByAgentIdDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public SmAgentAgency GetAgentAgencyByPrimaryKey(int individualId, int agentAgencyId)
        {
            try
            {
                return ModelAssembler.MappSMAgentAgency(CompanyAgentAgencyDAO.Find(individualId, agentAgencyId));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener aliado por Identificadores
        /// </summary>
        /// <param name="individualId">Id del individuo.</param>
        /// <param name="agentAgencyId">Id agencia</param>
        /// <returns>Listado de Aliados</returns>
        public List<SmAlly> GetAllyByIntermediary(int individualId, int agentAgencyId)
        {
            try
            {
                DAOs.AgencyDAO agencyDAO = new DAOs.AgencyDAO();
                return agencyDAO.GetAllyByIntermediary(individualId, agentAgencyId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///// <summary>
        ///// Listado de agencias de un intermediario
        ///// </summary>
        ///// <param name="individualId">Identificador del agente</param>
        ///// <returns>Listado de agencias del intermediario</returns>
        //public List<modelsUPersonCore.AgentAgency> GetAgenciesAgentByIndividualId(int individualId)
        //{
        //    try
        //    {
        //        CompanyAgentAgencyDAO AgenciesAgenDAO = new CompanyAgentAgencyDAO();
        //        return AgenciesAgenDAO.GetAgenciesAgentByIndividualId(individualId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(Errors.ErrorGetBranchAlliances, ex);
        //    }
        //}

        ///// <summary>
        ///// Creacion del Agente
        ///// </summary>
        ///// <param name="Agent">Models.Agent</param>
        ///// <returns>
        ///// Models.Agent
        ///// </returns>
        ///// <exception cref="BusinessException"></exception>
        //public modelsUPersonCore.Agent CreateAgent(modelsUPersonCore.Agent agent)
        //{
        //    try
        //    {
        //        AgentDAO agentDAO = new AgentDAO();
        //        modelsUPersonCore.Agent agentCreated = agentDAO.CreateAgent(agent);

        //        if (agent.AgentAgencies != null)
        //        {
        //            CompanyAgentAgencyDAO companyAgentAgencyDAO = new CompanyAgentAgencyDAO();
        //            foreach (modelsUPersonCore.AgentAgency ma in agent.AgentAgencies)
        //            {
        //                companyAgentAgencyDAO.CreateAgentAgency(ma, agentCreated.IndividualId);
        //            }
        //        }

        //        return agentCreated;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}
        #endregion

        /// <summary>
        /// Obtiene la lista de tipos de documento de persona jurídica y natural
        /// </summary>
        /// <returns>lista de tipos de documento de persona jurídica y natural</returns>
        public DocumentTypesServiceModel GetDocumentTypes()
        {
            DocumentTypeParamDAO documentTypeDAO = new DocumentTypeParamDAO();
            DocumentTypesServiceModel documentTypesServiceServiceModel = new DocumentTypesServiceModel();
            Result<List<ParamDocumentType>, ErrorModel> result = documentTypeDAO.GetDocumentTypes();
            if (result is ResultError<List<ParamDocumentType>, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<List<ParamDocumentType>, ErrorModel>).Message;
                documentTypesServiceServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                documentTypesServiceServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamDocumentType> resultValue = (result as ResultValue<List<ParamDocumentType>, ErrorModel>).Value;
                documentTypesServiceServiceModel = ModelServiceAssembler.MappDocumentTypes(resultValue);
            }

            return documentTypesServiceServiceModel;
        }

        /// <summary>
        ///    Obtiene la lista del país ciudad estado
        /// </summary>
        /// <returns></returns>
        public CountriesStatesCitiesServiceModel GetCountriesStatesCities()
        {
            CountryStateCityDAO countryStateCityDAO = new CountryStateCityDAO();
            CountriesStatesCitiesServiceModel countriesStatesCitiesServiceModel = new CountriesStatesCitiesServiceModel();
            Result<List<ParamCountryStateCity>, ErrorModel> result = countryStateCityDAO.GetCountryStateCity();
            if (result is ResultError<List<ParamCountryStateCity>, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<List<ParamCountryStateCity>, ErrorModel>).Message;
                countriesStatesCitiesServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                countriesStatesCitiesServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamCountryStateCity> resultValue = (result as ResultValue<List<ParamCountryStateCity>, ErrorModel>).Value;
                countriesStatesCitiesServiceModel = ModelServiceAssembler.MappCountryStateCity(resultValue);
            }

            return countriesStatesCitiesServiceModel;
        }

        public BasicPersonsServiceModel GetPersonBasicByCodePersonByFirtsNameByLastNameByNameByTypeDocumentByDocumentNumber(string codePerson, string firstName, string lastName, string name, string documentNumber,string typeDocument)
        {
            PersonBasicDAO personBasicDAO = new PersonBasicDAO();
            BasicPersonsServiceModel personBasicServiceModel = new BasicPersonsServiceModel();
            Result<List<ParamBasicPerson>, ErrorModel> result = personBasicDAO.GetPersonBasicByCodePersonByFirtsNameByLastNameByNameByTypeDocumentByDocumentNumber(codePerson, firstName, lastName, name, documentNumber,typeDocument);
            if (result is ResultError<List<ParamBasicPerson>, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<List<ParamBasicPerson>, ErrorModel>).Message;
                personBasicServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                personBasicServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamBasicPerson> resultValue = (result as ResultValue<List<ParamBasicPerson>, ErrorModel>).Value;
                personBasicServiceModel = ModelServiceAssembler.MappBasicPersons(resultValue);
            }

            return personBasicServiceModel;
        }


        public BasicCompanysServiceModel GetCompanyBasicByCodeCompanyByTradeNameByTypeDocumentByDocumentNumber(string codeCompany, string tradeName, string documentNumber, string typeDocument)
        {
            PersonBasicDAO personBasicDAO = new PersonBasicDAO();
            BasicCompanysServiceModel companyBasicServiceModel = new BasicCompanysServiceModel();
            Result<List<ParamBasicCompany>, ErrorModel> result = personBasicDAO.GetCompanyBasicByCodeCompanyByTradeNameByTypeDocumentByTypeDocumentByDocumentNumber(codeCompany, tradeName, documentNumber,typeDocument);
            if (result is ResultError<List<ParamBasicCompany>, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<List<ParamBasicCompany>, ErrorModel>).Message;
                companyBasicServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                companyBasicServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamBasicCompany> resultValue = (result as ResultValue<List<ParamBasicCompany>, ErrorModel>).Value;
                companyBasicServiceModel = ModelServiceAssembler.MappBasicCompanys(resultValue);
            }

            return companyBasicServiceModel;
        }


        public BasicPersonsServiceModel GetPersonBasicByDocumentNumber(string documentNumber)
        {
            PersonBasicDAO personBasicDAO = new PersonBasicDAO();
            BasicPersonsServiceModel personBasicServiceModel = new BasicPersonsServiceModel();
            Result<List<ParamBasicPerson>, ErrorModel> result = personBasicDAO.GetPersonBasicByByDocumentNumber(documentNumber);
            if (result is ResultError<List<ParamBasicPerson>, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<List<ParamBasicPerson>, ErrorModel>).Message;
                personBasicServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                personBasicServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamBasicPerson> resultValue = (result as ResultValue<List<ParamBasicPerson>, ErrorModel>).Value;
                personBasicServiceModel = ModelServiceAssembler.MappBasicPersons(resultValue);
            }
            return personBasicServiceModel;
        }


        public BasicCompanysServiceModel GetCompanyBasicByDocumentNumber(string documentNumber)
        {
            PersonBasicDAO personBasicDAO = new PersonBasicDAO();
            BasicCompanysServiceModel companyBasicServiceModel = new BasicCompanysServiceModel();
            Result<List<ParamBasicCompany>, ErrorModel> result = personBasicDAO.GetCompanyBasicByDocumentNumber(documentNumber);
            if (result is ResultError<List<ParamBasicCompany>, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<List<ParamBasicCompany>, ErrorModel>).Message;
                companyBasicServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                companyBasicServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamBasicCompany> resultValue = (result as ResultValue<List<ParamBasicCompany>, ErrorModel>).Value;
                companyBasicServiceModel = ModelServiceAssembler.MappBasicCompanys(resultValue);
            }

            return companyBasicServiceModel;
        }

        /// <summary>
        /// Actualiza la Informacion basica de la compañia
        /// </summary>
        /// <param name="companyServiceModel">Objeto de modelo de servicio BasicCompanysServiceModel</param>
        /// <returns>Modelo BasicCompanysServiceModel</returns>
        public BasicCompanysServiceModel SaveCompanyBasic(BasicCompanyServiceModel companyServiceModel)
        {
            PersonBasicDAO personBasicDAO = new PersonBasicDAO();
            ParamBasicCompany paramBasicCompany;
            Result<ParamBasicCompany, ErrorModel> resultParamBasicCompany = ServicesModelsAssembler.MappListParamBasicCompany(companyServiceModel);
            paramBasicCompany = (resultParamBasicCompany as ResultValue<ParamBasicCompany, ErrorModel>).Value;
            BasicCompanysServiceModel companyServiceModelResult = personBasicDAO.SaveCompanyBasic(paramBasicCompany);
            if (companyServiceModelResult.ErrorTypeService != ErrorTypeService.Ok)
            {
                return companyServiceModelResult;
            }
            return new BasicCompanysServiceModel()
            {
                ErrorTypeService = ErrorTypeService.Ok,
                BasicCompanyServiceModel = companyServiceModelResult.BasicCompanyServiceModel
            };
        }

        /// <summary>
        /// Actualiza la Informacion basica de la persona
        /// </summary>
        /// <param name="personServiceModel">Objeto de modelo de servicio BasicPersonServiceModel</param>
        /// <returns>Modelo BasicPersonsServiceModel</returns>
        public BasicPersonsServiceModel SavePersonBasic(BasicPersonServiceModel personServiceModel)
        {
            PersonBasicDAO personBasicDAO = new PersonBasicDAO();
            ParamBasicPerson paramBasicPerson;
            Result<ParamBasicPerson, ErrorModel> resultParamBasicPerson = ServicesModelsAssembler.MappListParamBasicPerson(personServiceModel);
            paramBasicPerson = (resultParamBasicPerson as ResultValue<ParamBasicPerson, ErrorModel>).Value;
            BasicPersonsServiceModel personServiceModelResult = personBasicDAO.SavePersonBasic(paramBasicPerson);
            if (personServiceModelResult.ErrorTypeService != ErrorTypeService.Ok)
            {
                return personServiceModelResult;
            }
            return new BasicPersonsServiceModel()
            {
                ErrorTypeService = ErrorTypeService.Ok,
                BasicPersonServiceModel = personServiceModelResult.BasicPersonServiceModel
            };
        }

        /// <summary>
        /// Obtiene la Lista de Direcciones
        /// </summary>
        /// <returns>Lista de Direcciones</returns>
        public modelsServiceCompany.GenericModelsServicesQueryModel GetAddress(Boolean? isEmail)
        {
            daosCompany.CompanyAddressDAO addressDAO = new daosCompany.CompanyAddressDAO();
            CompanyAddress addressServiceServiceModel = new CompanyAddress();
            modelsServiceCompany.GenericModelsServicesQueryModel genericModelsServicesQueryModel = new modelsServiceCompany.GenericModelsServicesQueryModel();
            Result<List<CompanyAddress>, ErrorModel> result = addressDAO.GetAddress(isEmail);

            if (result is ResultError<List<CompanyAddress>, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<List<CompanyAddress>, ErrorModel>).Message;
                genericModelsServicesQueryModel.ErrorTypeService = (enumsCompany.ErrorTypeService)errorModelResult.ErrorType;
                genericModelsServicesQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else
            {
                List<CompanyAddress> resultValue = (result as ResultValue<List<CompanyAddress>, ErrorModel>).Value;
                genericModelsServicesQueryModel.GenericModelServicesQueryModel = ModelServiceAssembler.CreateAddress(resultValue);
                genericModelsServicesQueryModel.ErrorTypeService = enumsCompany.ErrorTypeService.Ok;
            }

            return genericModelsServicesQueryModel;
        }
    }
}



