// -----------------------------------------------------------------------
// <copyright file="ModelServiceAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.Assemblers
{
    using Sistran.Company.Application.ModelServices.Models;
    using Sistran.Company.Application.ModelServices.Models.UniquePerson;
    using Sistran.Company.Application.ModelServices.Models.Param;
    using Sistran.Company.Application.UniquePersonParamService.Models;
    using Sistran.Core.Application.ModelServices.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Clase ensambladora para mapear modelos de negocio a modelos de servicios.
    /// </summary>
    public class ModelServiceAssembler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ModelServiceAssembler"/> class.
        /// </summary>
        protected ModelServiceAssembler()
        {
        }

        /// <summary>
        /// Mapeo de la modelo ParamLegalRepresentativeSing al modelo LegalRepresentativesSingServiceModel
        /// </summary>
        /// <param name="paramLegalRepresentativesSing">Lista de ParamLegalRepresentativeSing</param>
        /// <param name="paramBranchTypes">Lista de paramBranchTypes</param>
        /// <param name="paramCompanyTypes">Lista de paramCompanyTypes</param>
        /// <returns>Modelo LegalRepresentativesSingServiceModel</returns>
        public static LegalRepresentativesSingServiceModel MappLegalRepresentativesSing(List<ParamLegalRepresentativeSing> paramLegalRepresentativesSing, List<ParamBranchType> paramBranchTypes, List<ParamCompanyType> paramCompanyTypes)
        {
            LegalRepresentativesSingServiceModel paramLegalRepresentativesSingServiceModel = new LegalRepresentativesSingServiceModel();
            List<LegalRepresentativeSingServiceModel> listLegalRepresentativeSingServiceModel = new List<LegalRepresentativeSingServiceModel>();
            foreach (ParamLegalRepresentativeSing paramLegalRepresentativeSing in paramLegalRepresentativesSing)
            {
                LegalRepresentativeSingServiceModel itemLegalRepresentativeSingServiceModel = new LegalRepresentativeSingServiceModel();
                itemLegalRepresentativeSingServiceModel.CurrentFrom = paramLegalRepresentativeSing.CurrentFrom;
                itemLegalRepresentativeSingServiceModel.LegalRepresentative = paramLegalRepresentativeSing.LegalRepresentative;
                itemLegalRepresentativeSingServiceModel.PathSignatureImg = paramLegalRepresentativeSing.PathSignatureImg;
                itemLegalRepresentativeSingServiceModel.SignatureImg = paramLegalRepresentativeSing.SignatureImg;
                itemLegalRepresentativeSingServiceModel.UserId = paramLegalRepresentativeSing.UserId;

                ////Se busca la compañia para agregar en LegalRepresentativeSing
                CompanyTypeServiceModel companyTypeServiceModel = new CompanyTypeServiceModel();
                companyTypeServiceModel.Id = paramLegalRepresentativeSing.ParamCompanyType.Id;
                companyTypeServiceModel.Description = paramCompanyTypes.Where(t => t.Id == paramLegalRepresentativeSing.ParamCompanyType.Id).First().Description;
                itemLegalRepresentativeSingServiceModel.CompanyTypeServiceModel = companyTypeServiceModel;

                ////Se busca la sucursal para agregar en LegalRepresentativeSing
                BranchTypeServiceModel branchTypeServiceModel = new BranchTypeServiceModel();
                branchTypeServiceModel.Id = paramLegalRepresentativeSing.ParamBranchType.Id;
                branchTypeServiceModel.Description = paramBranchTypes.Where(t => t.Id == paramLegalRepresentativeSing.ParamBranchType.Id).First().Description;
                itemLegalRepresentativeSingServiceModel.BranchTypeServiceModel = branchTypeServiceModel;

                listLegalRepresentativeSingServiceModel.Add(itemLegalRepresentativeSingServiceModel);
            }

            paramLegalRepresentativesSingServiceModel.ErrorDescription = new List<string>();
            paramLegalRepresentativesSingServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            paramLegalRepresentativesSingServiceModel.LegalRepresentativeSingServiceModel = listLegalRepresentativeSingServiceModel;

            return paramLegalRepresentativesSingServiceModel;
        }

        /// <summary>
        /// Mapeo de la modelo ParamLegalRepresentativeSing al modelo LegalRepresentativeSingServiceModel
        /// </summary>
        /// <param name="paramLegalRepresentativeSing">Lista de ParamLegalRepresentativeSing</param>
        /// <returns>Modelo LegalRepresentativeSingServiceModel</returns>
        public static LegalRepresentativeSingServiceModel MappLegalRepresentativeSing(ParamLegalRepresentativeSing paramLegalRepresentativeSing)
        {
            LegalRepresentativeSingServiceModel legalRepresentativeSingServiceModel = new LegalRepresentativeSingServiceModel();
            legalRepresentativeSingServiceModel.CompanyTypeServiceModel.Id = paramLegalRepresentativeSing.ParamCompanyType.Id;
            legalRepresentativeSingServiceModel.BranchTypeServiceModel.Id = paramLegalRepresentativeSing.ParamBranchType.Id;
            legalRepresentativeSingServiceModel.CurrentFrom = paramLegalRepresentativeSing.CurrentFrom;
            legalRepresentativeSingServiceModel.LegalRepresentative = paramLegalRepresentativeSing.LegalRepresentative;
            legalRepresentativeSingServiceModel.PathSignatureImg = paramLegalRepresentativeSing.PathSignatureImg;
            legalRepresentativeSingServiceModel.SignatureImg = paramLegalRepresentativeSing.SignatureImg;
            legalRepresentativeSingServiceModel.UserId = paramLegalRepresentativeSing.UserId;

            return legalRepresentativeSingServiceModel;
        }

        /// <summary>
        /// Método para mapear una lista de objetos de tipo AlliancePrintFormatViewModel a InsuredProfile.
        /// </summary>
        /// <param name="legalRepresentativeSingServiceModels">lista de objetos de tipo legalRepresentativeSingServiceModel.</param>
        /// <param name="statusTypeService">Estado de la operación</param>
        /// <returns>lista de objetos de tipo InsuredProfile.</returns>
        public static List<LegalRepresentativeSingServiceModel> MappLegalRepresentativeSingByStatusType(List<LegalRepresentativeSingServiceModel> legalRepresentativeSingServiceModels, StatusTypeService statusTypeService)
        {
            List<LegalRepresentativeSingServiceModel> listLegalRepresentativeSingServiceModel = new List<LegalRepresentativeSingServiceModel>();
            if (legalRepresentativeSingServiceModels != null)
            {
                foreach (LegalRepresentativeSingServiceModel itemModel in legalRepresentativeSingServiceModels)
                {
                    if (itemModel.ParametricServiceModel.StatusTypeService == statusTypeService)
                    {
                        listLegalRepresentativeSingServiceModel.Add(itemModel);
                    }
                }
            }

            return listLegalRepresentativeSingServiceModel;
        }

        /// <summary>
        /// Mapeo de la modelo ParamCompanyType al modelo CompanyTypeServiceModel
        /// </summary>
        /// <param name="paramCompanyTypes">Lista de ParamCompanyType</param>
        /// <returns>Modelo CompanyTypeServiceModel</returns>
        public static CompanyTypesServiceModel MappCompanyTypes(List<ParamCompanyType> paramCompanyTypes)
        {
            CompanyTypesServiceModel paramCompanyTypesServiceModel = new CompanyTypesServiceModel();
            List<CompanyTypeServiceModel> listCompanyTypeServiceModel = new List<CompanyTypeServiceModel>();
            foreach (ParamCompanyType paramCompanyTypeBusinessModel in paramCompanyTypes)
            {
                CompanyTypeServiceModel itemCompanyTypeServiceModel = new CompanyTypeServiceModel();
                itemCompanyTypeServiceModel.Id = paramCompanyTypeBusinessModel.Id;
                itemCompanyTypeServiceModel.Description = paramCompanyTypeBusinessModel.Description;
                listCompanyTypeServiceModel.Add(itemCompanyTypeServiceModel);
            }

            paramCompanyTypesServiceModel.ErrorDescription = new List<string>();
            paramCompanyTypesServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            paramCompanyTypesServiceModel.CompanyTypeServiceModel = listCompanyTypeServiceModel;

            return paramCompanyTypesServiceModel;
        }

        /// <summary>
        /// Mapeo de la modelo ParamBranchType al modelo BranchTypesServiceModel
        /// </summary>
        /// <param name="paramBranchTypes">Lista de ParamBranchType</param>
        /// <returns>Modelo BranchTypesServiceModel</returns>
        public static BranchTypesServiceModel MappBranchTypes(List<ParamBranchType> paramBranchTypes)
        {
            BranchTypesServiceModel paramBranchTypesServiceModel = new BranchTypesServiceModel();
            List<BranchTypeServiceModel> listBranchTypeServiceModel = new List<BranchTypeServiceModel>();
            foreach (ParamBranchType paramBranchTypeBusinessModel in paramBranchTypes)
            {
                BranchTypeServiceModel itemBranchTypeServiceModel = new BranchTypeServiceModel();
                itemBranchTypeServiceModel.Id = paramBranchTypeBusinessModel.Id;
                itemBranchTypeServiceModel.Description = paramBranchTypeBusinessModel.Description;
                listBranchTypeServiceModel.Add(itemBranchTypeServiceModel);
            }

            paramBranchTypesServiceModel.ErrorDescription = new List<string>();
            paramBranchTypesServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            paramBranchTypesServiceModel.BranchTypeServiceModel = listBranchTypeServiceModel;

            return paramBranchTypesServiceModel;
        }

        public static DocumentTypesServiceModel MappDocumentTypes(List<ParamDocumentType> paramDocumentTypes)
        {
            DocumentTypesServiceModel paramDocumentTypesServiceModel = new DocumentTypesServiceModel();
            List<DocumentTypeServiceModel> listDocumentTypeServiceModel = new List<DocumentTypeServiceModel>();
            foreach (ParamDocumentType paramDocumentTypeBusinessModel in paramDocumentTypes)
            {
                DocumentTypeServiceModel itemDocumentTypeServiceModel = new DocumentTypeServiceModel();
                itemDocumentTypeServiceModel.Id = paramDocumentTypeBusinessModel.Id;
                itemDocumentTypeServiceModel.Description = paramDocumentTypeBusinessModel.Description;
                itemDocumentTypeServiceModel.SmallDescription = paramDocumentTypeBusinessModel.SmallDescription;
                listDocumentTypeServiceModel.Add(itemDocumentTypeServiceModel);
            }

            paramDocumentTypesServiceModel.ErrorDescription = new List<string>();
            paramDocumentTypesServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            paramDocumentTypesServiceModel.DocumentTypeServiceModel = listDocumentTypeServiceModel;

            return paramDocumentTypesServiceModel;
        }

        public static CountriesStatesCitiesServiceModel MappCountryStateCity(List<ParamCountryStateCity> resultValue)
        {
            CountriesStatesCitiesServiceModel countriesStatesCitiesServiceModel = new CountriesStatesCitiesServiceModel();
            List<CountryStateCityServiceModel> ListCountryStateCity = new List<CountryStateCityServiceModel>();
            foreach (ParamCountryStateCity paramCountryStateCity in resultValue)
            {
                CountryStateCityServiceModel itemCountryStateCity = new CountryStateCityServiceModel();
                itemCountryStateCity.CityCd = paramCountryStateCity.CityCd;
                itemCountryStateCity.CityDescription = paramCountryStateCity.CityDescription;
                itemCountryStateCity.CountryCd = paramCountryStateCity.CountryCd;
                itemCountryStateCity.CountryDescription = paramCountryStateCity.CountryDescription;
                itemCountryStateCity.StateCd = paramCountryStateCity.StateCd;
                itemCountryStateCity.StateDescription = paramCountryStateCity.StateDescription;

                ListCountryStateCity.Add(itemCountryStateCity);
            }

            countriesStatesCitiesServiceModel.ErrorDescription = new List<string>();
            countriesStatesCitiesServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            countriesStatesCitiesServiceModel.CountryStateCityServiceModel = ListCountryStateCity;

            return countriesStatesCitiesServiceModel;
        }



        /// <summary>
        /// Mapeo de modelo ParamBasicPerson al modelo BasicPersonServiceModel
        /// </summary>
        /// <param name="paramBasicPersons">Lista de ParamBasicPerson</param>
        /// <returns>Modelo BasicPersonsServiceModel</returns>
        public static BasicPersonsServiceModel MappBasicPersons(List<ParamBasicPerson> paramBasicPersons)
        {
            BasicPersonsServiceModel basicPersonsServiceModel = new BasicPersonsServiceModel();
            List<BasicPersonServiceModel> listBasicPersonServiceModel = new List<BasicPersonServiceModel>();
            foreach (ParamBasicPerson paramBasicPersonBusinessModel in paramBasicPersons)
            {
                BasicPersonServiceModel itemBasicPersonServiceModel = new BasicPersonServiceModel();
                itemBasicPersonServiceModel.Birthdate = paramBasicPersonBusinessModel.Birthdate;
                itemBasicPersonServiceModel.BirthPlace = paramBasicPersonBusinessModel.BirthPlace;
                itemBasicPersonServiceModel.DocumentNumber = paramBasicPersonBusinessModel.DocumentNumber;
                itemBasicPersonServiceModel.DocumentType = paramBasicPersonBusinessModel.DocumentType;
                itemBasicPersonServiceModel.FirstName = paramBasicPersonBusinessModel.FirstName;
                itemBasicPersonServiceModel.Gender = paramBasicPersonBusinessModel.Gender;
                itemBasicPersonServiceModel.IndividualId = paramBasicPersonBusinessModel.IndividualId;
                itemBasicPersonServiceModel.LastName = paramBasicPersonBusinessModel.LastName;
                itemBasicPersonServiceModel.LastUpdate = paramBasicPersonBusinessModel.LastUpdate;
                itemBasicPersonServiceModel.MaritalStatus = paramBasicPersonBusinessModel.MaritalStatus;
                itemBasicPersonServiceModel.Name = paramBasicPersonBusinessModel.Name;
                itemBasicPersonServiceModel.PersonCode = paramBasicPersonBusinessModel.PersonCode;
                itemBasicPersonServiceModel.UpdateBy = paramBasicPersonBusinessModel.UpdateBy;
                itemBasicPersonServiceModel.Insured = paramBasicPersonBusinessModel.Insured;
                itemBasicPersonServiceModel.Beneficiary = paramBasicPersonBusinessModel.Beneficiary;
                itemBasicPersonServiceModel.Policy = paramBasicPersonBusinessModel.Policy;
                listBasicPersonServiceModel.Add(itemBasicPersonServiceModel);
            }
            basicPersonsServiceModel.ErrorDescription = new List<string>();
            basicPersonsServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            basicPersonsServiceModel.BasicPersonServiceModel = listBasicPersonServiceModel;

            return basicPersonsServiceModel;
        }

        /// <summary>
        /// Mapeo de modelo ParamBasicPerson al modelo BasicPersonServiceModel
        /// </summary>
        /// <param name="paramBasicPerson">Objeto de ParamBasicPerson</param>
        /// <returns>Modelo BasicPersonsServiceModel</returns>
        public static BasicPersonsServiceModel MappBasicPerson(ParamBasicPerson paramBasicPerson)
        {
            BasicPersonsServiceModel basicPersonsServiceModel = new BasicPersonsServiceModel();
            BasicPersonServiceModel itemBasicPersonServiceModel = new BasicPersonServiceModel();

            itemBasicPersonServiceModel.Birthdate = paramBasicPerson.Birthdate;
            itemBasicPersonServiceModel.BirthPlace = paramBasicPerson.BirthPlace;
            itemBasicPersonServiceModel.DocumentNumber = paramBasicPerson.DocumentNumber;
            itemBasicPersonServiceModel.DocumentType = paramBasicPerson.DocumentType;
            itemBasicPersonServiceModel.FirstName = paramBasicPerson.FirstName;
            itemBasicPersonServiceModel.Gender = paramBasicPerson.Gender;
            itemBasicPersonServiceModel.IndividualId = paramBasicPerson.IndividualId;
            itemBasicPersonServiceModel.LastName = paramBasicPerson.LastName;
            itemBasicPersonServiceModel.LastUpdate = paramBasicPerson.LastUpdate;
            itemBasicPersonServiceModel.MaritalStatus = paramBasicPerson.MaritalStatus;
            itemBasicPersonServiceModel.Name = paramBasicPerson.Name;
            itemBasicPersonServiceModel.PersonCode = paramBasicPerson.PersonCode;
            itemBasicPersonServiceModel.UpdateBy = paramBasicPerson.UpdateBy;
            itemBasicPersonServiceModel.Insured = paramBasicPerson.Insured;
            itemBasicPersonServiceModel.Beneficiary = paramBasicPerson.Beneficiary;
            itemBasicPersonServiceModel.Policy = paramBasicPerson.Policy;
            basicPersonsServiceModel.BasicPersonServiceModel.Add(itemBasicPersonServiceModel);

            basicPersonsServiceModel.ErrorDescription = new List<string>();
            basicPersonsServiceModel.ErrorTypeService = ErrorTypeService.Ok;

            return basicPersonsServiceModel;
        }

        /// <summary>
        /// Mapeo de modelo ParamBasicCompany al modelo BasicCompanysServiceModel
        /// </summary>
        /// <param name="paramBasicCompanys">Lista de ParamBasicCompany</param>
        /// <returns>Modelo BasicCompanysServiceModel</returns>
        public static BasicCompanysServiceModel MappBasicCompanys(List<ParamBasicCompany> paramBasicCompanys)
        {
            BasicCompanysServiceModel basicCompanysServiceModel = new BasicCompanysServiceModel();
            List<BasicCompanyServiceModel> listBasicCompanyServiceModel = new List<BasicCompanyServiceModel>();
            foreach (ParamBasicCompany paramBasicCompanyBusinessModel in paramBasicCompanys)
            {
                BasicCompanyServiceModel itemBasicCompanyServiceModel = new BasicCompanyServiceModel();
                itemBasicCompanyServiceModel.DocumentNumber = paramBasicCompanyBusinessModel.DocumentNumber;
                itemBasicCompanyServiceModel.DocumentType = paramBasicCompanyBusinessModel.DocumentType;
                itemBasicCompanyServiceModel.IndividualId = paramBasicCompanyBusinessModel.IndividualId;
                itemBasicCompanyServiceModel.LastUpdate = paramBasicCompanyBusinessModel.LastUpdate;
                itemBasicCompanyServiceModel.UpdateBy = paramBasicCompanyBusinessModel.UpdateBy;
                itemBasicCompanyServiceModel.CompanyCode = paramBasicCompanyBusinessModel.CompanyCode;
                itemBasicCompanyServiceModel.CompanyDigit = paramBasicCompanyBusinessModel.CompanyDigit;
                itemBasicCompanyServiceModel.CompanyTypePartnership = paramBasicCompanyBusinessModel.CompanyTypePartnership;
                itemBasicCompanyServiceModel.Country = paramBasicCompanyBusinessModel.Country;
                itemBasicCompanyServiceModel.TradeName = paramBasicCompanyBusinessModel.TradeName;
                itemBasicCompanyServiceModel.TypePartnership = paramBasicCompanyBusinessModel.TypePartnership;
                itemBasicCompanyServiceModel.Insured = paramBasicCompanyBusinessModel.Insured;
                itemBasicCompanyServiceModel.Beneficiary = paramBasicCompanyBusinessModel.Beneficiary;
                itemBasicCompanyServiceModel.Policy = paramBasicCompanyBusinessModel.Policy;
                listBasicCompanyServiceModel.Add(itemBasicCompanyServiceModel);
            }
            basicCompanysServiceModel.ErrorDescription = new List<string>();
            basicCompanysServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            basicCompanysServiceModel.BasicCompanyServiceModel = listBasicCompanyServiceModel;

            return basicCompanysServiceModel;
        }

        /// <summary>
        /// Mapeo de modelo ParamBasicCompany al modelo BasicCompanysServiceModel
        /// </summary>
        /// <param name="paramBasicCompany">ParamBasicCompany</param>
        /// <returns>Modelo BasicCompanysServiceModel</returns>
        public static BasicCompanysServiceModel MappBasicCompanys(ParamBasicCompany paramBasicCompany)
        {
            BasicCompanysServiceModel basicCompanysServiceModel = new BasicCompanysServiceModel();
            BasicCompanyServiceModel itemBasicCompanyServiceModel = new BasicCompanyServiceModel();

            itemBasicCompanyServiceModel.DocumentNumber = paramBasicCompany.DocumentNumber;
            itemBasicCompanyServiceModel.DocumentType = paramBasicCompany.DocumentType;
            itemBasicCompanyServiceModel.IndividualId = paramBasicCompany.IndividualId;
            itemBasicCompanyServiceModel.LastUpdate = paramBasicCompany.LastUpdate;
            itemBasicCompanyServiceModel.UpdateBy = paramBasicCompany.UpdateBy;
            itemBasicCompanyServiceModel.CompanyCode = paramBasicCompany.CompanyCode;
            itemBasicCompanyServiceModel.CompanyDigit = paramBasicCompany.CompanyDigit;
            itemBasicCompanyServiceModel.CompanyTypePartnership = paramBasicCompany.CompanyTypePartnership;
            itemBasicCompanyServiceModel.Country = paramBasicCompany.Country;
            itemBasicCompanyServiceModel.TradeName = paramBasicCompany.TradeName;
            itemBasicCompanyServiceModel.TypePartnership = paramBasicCompany.TypePartnership;
            itemBasicCompanyServiceModel.Insured = paramBasicCompany.Insured;
            itemBasicCompanyServiceModel.Beneficiary = paramBasicCompany.Beneficiary;
            itemBasicCompanyServiceModel.Policy = paramBasicCompany.Policy;
            basicCompanysServiceModel.BasicCompanyServiceModel.Add(itemBasicCompanyServiceModel);

            basicCompanysServiceModel.ErrorDescription = new List<string>();
            basicCompanysServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            return basicCompanysServiceModel;
        }

        /// <summary>
        /// Metodo lista de modulo
        /// </summary>
        /// <param name="modules">Recibe modulos</param>
        /// <returns>Retorna lista de modulos</returns>
        public static List<GenericModelServicesQueryModel> CreateAddress(List<CompanyAddress> address)
        {
            List<GenericModelServicesQueryModel> moduleServiceModel = new List<GenericModelServicesQueryModel>();
            foreach (var item in address)
            {
                moduleServiceModel.Add(CreateAddresses(item));
            }
            return moduleServiceModel;
        }

        /// <summary>
        /// Metodo Modulo
        /// </summary>
        /// <param name="module">Recibe modulo</param>
        /// <returns>Retorna modulo</returns>
        public static GenericModelServicesQueryModel CreateAddresses(CompanyAddress address)
        {
            GenericModelServicesQueryModel moduleBaseGroupPolicies = new GenericModelServicesQueryModel();
            moduleBaseGroupPolicies.id = address.AddressTypeCd;
            moduleBaseGroupPolicies.description = address.SmallDescription;
            moduleBaseGroupPolicies.smallDescription = address.TinyDescription;

            return moduleBaseGroupPolicies;
        }
    }
}
