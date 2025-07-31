// -----------------------------------------------------------------------
// <copyright file="ModelAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
using Sistran.Company.Application.UniquePerson.Entities;
using entitiesUPersonCore = Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Company.Application.UniquePersonParamService.EEProvider.Resources;
using Sistran.Company.Application.UniquePersonParamService.Enums;
using Sistran.Company.Application.UniquePersonParamService.Models;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Error;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Data;
using UNENT = Sistran.Core.Application.UniquePerson.Entities;

namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.Assemblers
{
    /// <summary>
    /// Clase enmbladora para mapear entidades a modelos de negocio.
    /// </summary>
    public class ModelAssembler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ModelAssembler"/> class.
        /// </summary>
        protected ModelAssembler()
        {

        }

        /// <summary>
        /// Mapeo de la entidad CptLegalReprSign al modelo CptLegalReprSign
        /// </summary>
        /// <param name="cptLegalReprSign">Entidad CptLegalReprSign</param>
        /// <returns> Modelo CptLegalReprSign</returns>
        public static Result<ParamLegalRepresentativeSing, ErrorModel> CreateCptLegalReprSign(CptLegalReprSign cptLegalReprSign)
        {
            List<string> errorListDescription = new List<string>();
            ParamCompanyType paramCompanyType;
            Result<ParamCompanyType, ErrorModel> resultParamCompanyType = ParamCompanyType.GetParamCompanyType(cptLegalReprSign.CiaCode, string.Empty);
            if (resultParamCompanyType is ResultError<ParamCompanyType, ErrorModel>)
            {
                errorListDescription.Add(Errors.ErrorMappingServiceModelAndBusinessModelCompany);
                return new ResultError<ParamLegalRepresentativeSing, ErrorModel>(ErrorModel.CreateErrorModel(errorListDescription, ErrorType.BusinessFault, null));
            }

            paramCompanyType = (resultParamCompanyType as ResultValue<ParamCompanyType, ErrorModel>).Value;

            ParamBranchType paramBranchType;
            Result<ParamBranchType, ErrorModel> resultParamBranchType = ParamBranchType.CreateParamBranchType(cptLegalReprSign.BranchTypeCode, string.Empty);
            if (resultParamBranchType is ResultError<ParamBranchType, ErrorModel>)
            {
                errorListDescription.Add(Errors.ErrorMappingServiceModelAndBusinessModelBranch);
                return new ResultError<ParamLegalRepresentativeSing, ErrorModel>(ErrorModel.CreateErrorModel(errorListDescription, ErrorType.BusinessFault, null));
            }

            paramBranchType = (resultParamBranchType as ResultValue<ParamBranchType, ErrorModel>).Value;
            Result<ParamLegalRepresentativeSing, ErrorModel> result = ParamLegalRepresentativeSing.GetParamLegalRepresentativeSing(paramCompanyType, paramBranchType, cptLegalReprSign.CurrentFrom, cptLegalReprSign.LegalRepresentative, cptLegalReprSign.PathSignatureImg, cptLegalReprSign.SignatureImg, cptLegalReprSign.UserId);
            return result;
        }


        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo CptLegalReprSign
        /// </summary>
        /// <param name="businessCollection">Objeto businessCollection</param>
        /// <returns>Lista de Modelos CptLegalReprSign</returns>
        public static Result<List<ParamLegalRepresentativeSing>, ErrorModel> CreateLstCptLegalReprSign(BusinessCollection businessCollection)
        {
            List<ParamLegalRepresentativeSing> cptLegalReprSign = new List<ParamLegalRepresentativeSing>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamLegalRepresentativeSing, ErrorModel> result;
            foreach (CptLegalReprSign entityCptLegalReprSign in businessCollection)
            {
                result = CreateCptLegalReprSign(entityCptLegalReprSign);
                if (result is ResultError<ParamLegalRepresentativeSing, ErrorModel>)
                {
                    errorModelListDescription.Add(Errors.ErrorMappingServiceModelAndBusinessModelParamLegalRepresentativeSing);
                    return new ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamLegalRepresentativeSing resultValue = (result as ResultValue<ParamLegalRepresentativeSing, ErrorModel>).Value;
                    cptLegalReprSign.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamLegalRepresentativeSing>, ErrorModel>(cptLegalReprSign);
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo ParamBranchType
        /// </summary>
        /// <param name="businessCollection">Objeto BusinessCollection</param>
        /// <returns>Lista de Modelos ParamBranchType</returns>
        public static Result<List<ParamBranchType>, ErrorModel> CreateBranchTypes(BusinessCollection businessCollection)
        {
            List<ParamBranchType> branchType = new List<ParamBranchType>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamBranchType, ErrorModel> result;
            foreach (CoBranchType entityBranchType in businessCollection)
            {
                result = CreateBranchType(entityBranchType);
                if (result is ResultError<ParamBranchType, ErrorModel>)
                {
                    errorModelListDescription.Add(Errors.ErrorMappingServiceModelAndBusinessModelParamLegalRepresentativeSing);
                    return new ResultError<List<ParamBranchType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamBranchType resultValue = (result as ResultValue<ParamBranchType, ErrorModel>).Value;
                    branchType.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamBranchType>, ErrorModel>(branchType);
        }

        /// <summary>
        /// Mapeo de la entidad BranchType al modelo ParamBranchType
        /// </summary>
        /// <param name="branchType">Entidad BranchType</param>
        /// <returns>Modelo ParamBranchType</returns>
        public static Result<ParamBranchType, ErrorModel> CreateBranchType(CoBranchType branchType)
        {
            Result<ParamBranchType, ErrorModel> result = ParamBranchType.GetParamBranchType(branchType.BranchTypeCode, branchType.Description);
            return result;
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo ParamCompanyType
        /// </summary>
        /// <returns>Lista de Modelos ParamCompanyType</returns>
        public static Result<List<ParamCompanyType>, ErrorModel> CreateCompanyTypes()
        {
            List<ParamCompanyType> branchType = new List<ParamCompanyType>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamCompanyType, ErrorModel> result;
            foreach (var value in Enum.GetValues(typeof(CompanyType)))
            {
                result = ParamCompanyType.GetParamCompanyType((int)value, ((CompanyType)value).ToString());
                if (result is ResultError<ParamCompanyType, ErrorModel>)
                {
                    errorModelListDescription.Add(Errors.ErrorMappingServiceModelAndBusinessModelParamLegalRepresentativeSing);
                    return new ResultError<List<ParamCompanyType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamCompanyType resultValue = (result as ResultValue<ParamCompanyType, ErrorModel>).Value;
                    branchType.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamCompanyType>, ErrorModel>(branchType);
        }

        #region ALLIANCE
        /// <summary>
        /// Transforma los aliados de la entidad de base de datos a modelo de negocio
        /// </summary>
        /// <param name="businessCollection">Objetode base de datos con la lista de aliados</param>
        /// <returns>Listado de aliados</returns>
        public static List<Alliance> CreateAlliances(BusinessCollection businessCollection)
        {
            List<Alliance> alliances = new List<Alliance>();
            foreach (CptAlliance item in businessCollection)
            {
                alliances.Add(CreateAlliance(item));
            }
            return alliances;
        }

        /// <summary>
        /// Transforma un aliados de la entidad de base de datos a modelo de negocio
        /// </summary>
        /// <param name="alliance"></param>
        /// <returns>Modelo de negoco Aliado</returns>
        public static Alliance CreateAlliance(CptAlliance alliance)
        {
            return new Alliance()
            {
                AllianceId = alliance.AllianceId,
                Description = alliance.Description,
                IsFine = alliance.IsFine.Value,
                IsScore = alliance.IsScore.Value
            };
        }

        /// <summary>
        /// Transforma las sucursales de la entidad de base de datos a modelo de negocio
        /// </summary>
        /// <param name="businessCollection">Entidades de base de datos con sucursales</param>
        /// <returns>Modelo de negocio Sucursales</returns>
        public static BranchAlliance CreateBrachAlliances(IDataReader businessCollection)
        {
            return new BranchAlliance()
            {
                AllianceId = (int)businessCollection["AllianceId"],
                BranchDescription = businessCollection["BranchDescription"].ToString(),
                BranchId = (int)businessCollection["BranchId"],
                CityCD = (int)businessCollection["DivipolaID"],
                CityName = businessCollection["CityDescription"].ToString(),
                CountryCD = (int)businessCollection["CountryCD"],
                CountryName = businessCollection["CountryDescription"].ToString(),
                StateCD = (int)businessCollection["StateCD"],
                StateName = businessCollection["StateDescription"].ToString()
            };
        }

        /// <summary>
        /// Transformar puntos de venta
        /// </summary>
        /// <param name="businessCollection">Entidades de bases de datos</param>
        /// <returns>Modelos de negocio, puntos de venta</returns>
        public static List<AllianceBranchSalePonit> CreateSalesPointsAlliances(BusinessCollection businessCollection)
        {
            List<AllianceBranchSalePonit> salesPoints = new List<AllianceBranchSalePonit>();
            foreach (CptAllianceBranchSalePoint item in businessCollection)
            {
                salesPoints.Add(CreateSalePointAlliance(item));
            }
            return salesPoints;
        }

        /// <summary>
        /// Tranforma puntos de venta
        /// </summary>
        /// <param name="alliance">Entidad de base de datos</param>
        /// <returns>Modelo de negocio</returns>
        public static AllianceBranchSalePonit CreateSalePointAlliance(CptAllianceBranchSalePoint alliance)
        {
            return new AllianceBranchSalePonit()
            {
                AllianceId = alliance.AllianceId,
                BranchId = alliance.BranchId,
                SalePointId = alliance.SalePointId,
                SalePointDescription = alliance.SalePointDescription
            };
        }


        /// <summary>
        /// Transformar puntos de venta
        /// </summary>
        /// <param name="businessCollection">Entidades de bases de datos</param>
        /// <returns>Modelos de negocio, puntos de venta</returns>
        public static AllianceBranchSalePonit CreateSalesPointsAlliances(IDataReader reader)
        {
            return new AllianceBranchSalePonit()
            {
                AllianceId = (int)reader["AllianceId"],
                AllianceDescription = reader["Description"].ToString(),
                BranchId = (int)reader["BranchId"],
                BranchDescription = reader["BranchDescription"].ToString(),
                SalePointId = (int)reader["SalePointId"],
                SalePointDescription = reader["SalePointDescription"].ToString()
            };
        }

        /// <summary>
        /// Tranforma Agencias por agentes
        /// </summary>
        /// <param name="businessCollection">Colección de entidades</param>
        /// <returns>Listaod de modelos de negocio, agencias por agente</returns>
        //public static List<modelsUPersonCore.AgentAgency> CreateAgentAgencies(BusinessCollection businessCollection)
        //{
        //    List<modelsUPersonCore.AgentAgency> salesPoints = new List<modelsUPersonCore.AgentAgency>();
        //    foreach (CptAgentAlliance item in businessCollection)
        //    {
        //        salesPoints.Add(CreateAgentAgency(item));
        //    }
        //    return salesPoints;
        //}

        /// <summary>
        /// Tranforma agencia por agente
        /// </summary>
        /// <param name="agentAlliance">Entidad de base de datos</param>
        /// <returns>Modelo de negocio, agencia por agente</returns>
        //public static modelsUPersonCore.AgentAgency CreateAgentAgency(CptAgentAlliance agentAlliance)
        //{
        //    return new modelsUPersonCore.AgentAgency()
        //    {
        //        AllianceId = agentAlliance.AllianceId,
        //        AgencyAgencyId = agentAlliance.AgentAgencyId,
        //        IndividualId = agentAlliance.IndividualId,
        //        IsSpecialImpression = agentAlliance.SpecialPrint
        //    };
        //}

        public static List<SmAgentAgency> MappSMAgentAgencyList(BusinessCollection businessCollection)
        {
            List<SmAgentAgency> agentAgenyList = new List<SmAgentAgency>();
            foreach (entitiesUPersonCore.AgentAgency field in businessCollection)
            {
                agentAgenyList.Add(ModelAssembler.MappSMAgentAgency(field));

            }
            return agentAgenyList;
        }

        /// <summary>
        /// Mapeo de entidad AgentAgency a Modelo de servicio SMAgency
        /// </summary>
        /// <param name="agentAgencyEntity">Entidad AgentAgency</param>
        /// <returns>Modelo de servicio SMAgentAgency.</returns>
        public static SmAgentAgency MappSMAgentAgency(entitiesUPersonCore.AgentAgency agentAgencyEntity)
        {
            return new SmAgentAgency
            {
                IndividualId = agentAgencyEntity.IndividualId,
                AgencyAgencyId = agentAgencyEntity.AgentAgencyId,
                Description = agentAgencyEntity.Description,
                BranchCode = agentAgencyEntity.BranchCode,
                AgentCode = agentAgencyEntity.AgentCode,
                DeclinedDate = agentAgencyEntity.DeclinedDate,
                AgentDeclinedTypeCode = agentAgencyEntity.AgentDeclinedTypeCode
            };
        }

        /// <summary>
        /// Mapear el listado de Aliados a modelo de servicio.
        /// </summary>
        /// <param name="cptAlliance">Colección de entidades de tipo CptAlliance.</param>
        /// <returns>Listado de aliados.</returns>
        public static List<SmAlly> MappAllyList(BusinessCollection businessCollection)
        {
            List<SmAlly> SmAllyList = new List<SmAlly>();
            foreach (CptAlliance field in businessCollection)
            {
                SmAllyList.Add(ModelAssembler.MappSMAlly(field));

            }
            return SmAllyList;
        }

        /// <summary>
        /// Mapeo de entidad CptAlliance a Modelo de servicio SMAlly.
        /// </summary>
        /// <param name="agentAgencyEntity">Entidad CptAlliance</param>
        /// <returns>Modelo de servicio SMAlly.</returns>
        public static SmAlly MappSMAlly(CptAlliance cptAllianceEntity)
        {
            return new SmAlly
            {
                AllianceId = cptAllianceEntity.AllianceId,
                Description = cptAllianceEntity.Description
            };
        }

        #endregion

        /// <summary>
        /// Convierte de entidad a modelo de negocio del país estado ciudad  
        /// </summary>
        /// <param name="city">objeto ciudad /param>
        /// <param name="state">objeto estado </param>
        /// <param name="country">objeto país</param>
        /// <returns>modelo de negocio </returns>
        public static ParamCountryStateCity GenerateCountryStateCity(City city, State state, Country country) => new ParamCountryStateCity
        {
            CityCd = city.CityCode,
            CityDescription = city.Description,
            CountryCd = country.CountryCode,
            CountryDescription = country.Description,
            StateCd = state.StateCode,
            StateDescription = state.Description
        };

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo ParamDocumentType
        /// </summary>
        /// <param name="businessCollection">Objeto BusinessCollection</param>
        /// <returns>Lista de Modelos ParamDocumentType</returns>
        public static List<ParamDocumentType> GetDocumentTypes(BusinessCollection businessCollection)
        {
            List<ParamDocumentType> companyDocuments = new List<ParamDocumentType>();

            foreach (UNENT.DocumentType entityDocuemnt in businessCollection)
            {
                companyDocuments.Add(GenerateIdentityCardType(entityDocuemnt));
            }

            return companyDocuments;
        }

        /// <summary>
        /// convierte de entidad a modelo 
        /// </summary>
        /// <param name="entityDocument">entidad de tipo de docuemento</param>
        /// <returns>objeto ParamDocumentType</returns>
        private static ParamDocumentType GenerateIdentityCardType(UNENT.DocumentType entityDocument) => new ParamDocumentType
        {
            Id = entityDocument.IdDocumentType,
            Description = entityDocument.Description,
            SmallDescription = entityDocument.SmallDescription
        };


        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo CompanyAddress
        /// </summary>
        /// <param name="businessCollection">Objeto BusinessCollection</param>
        /// <returns>Lista de Modelos CompanyAddress</returns>
        public static List<CompanyAddress> GetAddress(BusinessCollection businessCollection)
        {
            List<CompanyAddress> companyAddress = new List<CompanyAddress>();

            foreach (CptAddressType entityAddressType in businessCollection)
            {
                companyAddress.Add(GenerateIdentityAddressType(entityAddressType));
            }

            return companyAddress;
        }

        /// <summary>
        /// convierte de entidad a modelo 
        /// </summary>
        /// <param name="entityAddressType">entidad de tipo de docuemento</param>
        /// <returns>objeto ParamDocumentType</returns>
        private static CompanyAddress GenerateIdentityAddressType(CptAddressType entityAddressType) => new CompanyAddress
        {
            AddressTypeCd = entityAddressType.AddressTypeCode,
            SmallDescription = entityAddressType.SmallDescription,
            TinyDescription = entityAddressType.TinyDescription,
            IsElectronicMail = (bool)entityAddressType.IsElectronicMail,

        };
        #region Person-CompanyBasic
        /// <summary>
        /// Crear Lista Información Basica de Personas
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns>Lista de Modelos ParamBasicPerson</returns>
        public static Result<List<ParamBasicPerson>, ErrorModel> CreatePersonsBasic(BusinessCollection businessCollection)
        {
            List<ParamBasicPerson> basicPersons = new List<ParamBasicPerson>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamBasicPerson, ErrorModel> result;
            foreach (UNENT.Person person in businessCollection)
            {
                result = CreatePersonBasic(person);
                if (result is ResultError<ParamBasicPerson, ErrorModel>)
                {
                    errorModelListDescription.Add(Errors.ErrorMappingServiceModelAndBusinessModelParamBasicPerson);
                    return new ResultError<List<ParamBasicPerson>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamBasicPerson resultValue = (result as ResultValue<ParamBasicPerson, ErrorModel>).Value;
                    basicPersons.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamBasicPerson>, ErrorModel>(basicPersons);
        }

        /// <summary>
        /// Crear Lista Información Basica de Compañia
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static Result<List<ParamBasicCompany>, ErrorModel> CreateCompanyBasic(BusinessCollection businessCollection)
        {
            List<ParamBasicCompany> basicCompanys = new List<ParamBasicCompany>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamBasicCompany, ErrorModel> result;
            foreach (UNENT.Company company in businessCollection)
            {
                result = CreateCompanyBasic(company);
                if (result is ResultError<ParamBasicCompany, ErrorModel>)
                {
                    errorModelListDescription.Add(Errors.ErrorMappingServiceModelAndBusinessModelCompany);
                    return new ResultError<List<ParamBasicCompany>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamBasicCompany resultValue = (result as ResultValue<ParamBasicCompany, ErrorModel>).Value;
                    basicCompanys.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamBasicCompany>, ErrorModel>(basicCompanys);
        }

        /// <summary>
        /// Crear Información basica de una Persona
        /// </summary>
        /// <param name="person">Person Basic</param>
        /// <returns></returns>
        public static Result<ParamBasicPerson, ErrorModel> CreatePersonBasic(UNENT.Person person)
        {
            return new ResultValue<ParamBasicPerson, ErrorModel>(new ParamBasicPerson
            {
                IndividualId = person.IndividualId,
                DocumentType = person.IdCardTypeCode,
                DocumentNumber = person.IdCardNo,
                PersonCode = person.IndividualId,
                FirstName = person.Surname,
                LastName = person.MotherLastName,
                Name = person.Name,
                Gender = person.Gender,
                MaritalStatus = person.MaritalStatusCode,
                Birthdate = person.BirthDate,
                BirthPlace = person.BirthPlace
            });

        }

        /// <summary>
        /// Crear Información basica de una Compañia
        /// </summary>
        /// <param name="person">Company Basic</param>
        /// <returns></returns>
        public static ResultValue<ParamBasicCompany, ErrorModel> CreateCompanyBasic(UNENT.Company company)
        {
            return new ResultValue<ParamBasicCompany, ErrorModel>(new ParamBasicCompany
            {
                CompanyCode = company.IndividualId,
                Country = company.CountryCode,
                DocumentNumber = company.TributaryIdNo,
                DocumentType = company.TributaryIdTypeCode,
                IndividualId = company.IndividualId,
                TradeName = company.TradeName,
                CompanyTypePartnership = company.CompanyTypeCode,
                CompanyDigit = company.TributaryIdTypeCode
            });
        }


        #endregion
    }
}
