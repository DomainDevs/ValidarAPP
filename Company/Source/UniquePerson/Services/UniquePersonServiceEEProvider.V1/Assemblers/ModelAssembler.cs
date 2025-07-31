using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.DAOs;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using comm = Sistran.Company.Application.CommonServices.Models;
using CPCOMO = Sistran.Company.Application.CommonServices.Models;
using entities = Sistran.Core.Application.UniquePersonV1.Entities;
using enums = Sistran.Core.Services.UtilitiesServices.Enums;
using models = Sistran.Core.Application.UniquePersonService.V1.Models;
using UPENT = Sistran.Company.Application.UniquePerson.Entities;
using UPModel = Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using INTEN = Sistran.Core.Application.Integration.Entities;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Company.Application.UniquePersonServices.V1.DTOs;
using Sistran.Company.Application.ModelServices.Models.Param;
using Sistran.Company.Application.ModelServices.Enums;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs;
using System.Linq;
using Sistran.Company.Application.ExternalProxyServices.Models;
using DTO = Sistran.Company.Application.UniquePersonAplicationServices.DTOs;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers
{
    /// <summary>
    /// Convertir de Entities a Modelos
    /// </summary>
    public class ModelAssembler
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelAssembler"/> class.
        /// </summary>
        protected ModelAssembler()
        { }
        #region MapperPerson
        public static IMapper CreateMapperPerson()
        {
            var config = MapperCache.GetMapper<CompanyPerson, models.Person>(cfg =>
            {
                //De model COMPANY a model CORE
                cfg.CreateMap<CompanyPerson, models.Person>();
                cfg.CreateMap<CompanyRole, models.Role>();
                cfg.CreateMap<CompanyEconomicActivity, models.EconomicActivity>();
                cfg.CreateMap<CompanyIdentificationDocument, models.IdentificationDocument>();
                cfg.CreateMap<CompanyMaritalStatus, models.MaritalStatus>();
                cfg.CreateMap<CompanyEducativeLevel, models.EducativeLevel>();
                cfg.CreateMap<CompanySocialLayer, models.SocialLayer>();
                cfg.CreateMap<CompanyHouseType, models.HouseType>();
                cfg.CreateMap<CompanyPersonType, models.PersonType>();
                cfg.CreateMap<CompanyDocumentType, models.DocumentType>();


                cfg.CreateMap<CompanyAddress, models.Address>();
                cfg.CreateMap<Models.CompanyAddressType, models.AddressType>();

                cfg.CreateMap<CompanyPhone, models.Phone>();
                cfg.CreateMap<Models.CompanyPhoneType, models.PhoneType>();

                cfg.CreateMap<CompanyEmail, models.Email>();
                cfg.CreateMap<Models.CompanyEmailType, models.EmailType>();

                cfg.CreateMap<CompanyLabourPerson, models.LabourPerson>();
                cfg.CreateMap<CompanyOccupation, models.Occupation>();
                cfg.CreateMap<CompanyIncomeLevel, models.IncomeLevel>();
                cfg.CreateMap<CompanyPhone, models.Phone>();
                cfg.CreateMap<CompanySpeciality, models.Speciality>();
                cfg.CreateMap<CompanyPersonInterestGroup, models.PersonInterestGroup>();
                cfg.CreateMap<CompanyPersonType, models.PersonType>();
                cfg.CreateMap<CompanyInsured, models.Insured>();
                //De model CORE a model COMPANY 
                cfg.CreateMap<models.Person, CompanyPerson>()
                    .ForMember(x => x.Name, opt => opt.NullSubstitute(string.Empty))
                    .ForMember(x => x.SurName, opt => opt.NullSubstitute(string.Empty))
                    .ForMember(x => x.SecondSurName, opt => opt.NullSubstitute(string.Empty))
                    .ForMember(x => x.FullName, opt => opt.NullSubstitute(string.Empty));
                cfg.CreateMap<models.Role, CompanyRole>();
                cfg.CreateMap<models.EconomicActivity, CompanyEconomicActivity>();
                cfg.CreateMap<models.IdentificationDocument, CompanyIdentificationDocument>();
                cfg.CreateMap<models.MaritalStatus, CompanyMaritalStatus>();
                cfg.CreateMap<models.EducativeLevel, CompanyEducativeLevel>();
                cfg.CreateMap<models.SocialLayer, CompanySocialLayer>();
                cfg.CreateMap<models.HouseType, CompanyHouseType>();
                cfg.CreateMap<models.PersonType, CompanyPersonType>();
                cfg.CreateMap<models.DocumentType, CompanyDocumentType>();
                cfg.CreateMap<models.Insured, CompanyInsured>();

                cfg.CreateMap<models.Address, CompanyAddress>();
                cfg.CreateMap<models.AddressType, Models.CompanyAddressType>();

                cfg.CreateMap<models.Phone, CompanyPhone>();
                cfg.CreateMap<models.PhoneType, Models.CompanyPhoneType>();

                cfg.CreateMap<models.Email, CompanyEmail>();
                cfg.CreateMap<models.EmailType, Models.CompanyEmailType>();

                cfg.CreateMap<models.LabourPerson, CompanyLabourPerson>();
                cfg.CreateMap<models.Occupation, CompanyOccupation>();
                cfg.CreateMap<models.IncomeLevel, CompanyIncomeLevel>();
                cfg.CreateMap<models.Phone, CompanyPhone>();
                cfg.CreateMap<models.Speciality, CompanySpeciality>();
                cfg.CreateMap<models.PersonInterestGroup, CompanyPersonInterestGroup>();
                cfg.CreateMap<models.PersonType, CompanyPersonType>();
            });
            return config;
        }

        #endregion MapperPerson

        #region MapperReinsurer V1
        public static IMapper CreateMapperReInsurer()
        {
            var config = MapperCache.GetMapper<models.ReInsurer, CompanyReInsurer>(cfg =>
            {
                cfg.CreateMap<CompanyReInsurer, models.ReInsurer>();
                cfg.CreateMap<models.ReInsurer, CompanyReInsurer>();
            });
            return config;
        }
        #endregion MapperReinsurer V1

        #region MapperPartner V1
        public static IMapper CreateMapperPartner()
        {
            var config = MapperCache.GetMapper<models.Partner, CompanyPartner>(cfg =>
            {
                cfg.CreateMap<CompanyPartner, models.Partner>();

                cfg.CreateMap<models.Partner, CompanyPartner>();
            });
            return config;
        }
        #endregion MapperPartner V1

        #region MapperAgetn V1
        public static IMapper CreateMapperAgent()
        {
            var config = MapperCache.GetMapper<CompanyAgent, models.Agent>(cfg =>
            {
                cfg.CreateMap<CompanyAgent, models.Agent>();
                cfg.CreateMap<CompanyAgentDeclinedType, models.AgentDeclinedType>();
                cfg.CreateMap<CompanyAgentType, models.AgentType>();
                cfg.CreateMap<CompanyGroupAgent, models.GroupAgent>();
                cfg.CreateMap<CompanySalesChannel, models.SalesChannel>();
                cfg.CreateMap<CompanyEmployeePerson, models.EmployeePerson>();

                cfg.CreateMap<models.Agent, CompanyAgent>();
                cfg.CreateMap<models.AgentDeclinedType, CompanyAgentDeclinedType>();
                cfg.CreateMap<models.AgentType, CompanyAgentType>();
                cfg.CreateMap<models.GroupAgent, CompanyGroupAgent>();
                cfg.CreateMap<models.SalesChannel, CompanySalesChannel>();
                cfg.CreateMap<models.EmployeePerson, CompanyEmployeePerson>();
            });
            return config;
        }

        public static IMapper CreateMapperAgencyByIndividual()
        {
            var config = MapperCache.GetMapper<models.Agency, CompanyAgency>(cfg =>
            {
                cfg.CreateMap<CompanyAgency, models.Agency>();
                cfg.CreateMap<comm.CompanyBranch, Branch>();
                cfg.CreateMap<CompanyAgentDeclinedType, models.AgentDeclinedType>();
                cfg.CreateMap<CompanyAgent, models.Agent>();
                cfg.CreateMap<CompanyAgentType, models.AgentType>();

                cfg.CreateMap<models.Agency, CompanyAgency>();
                cfg.CreateMap<Branch, comm.CompanyBranch>();
                cfg.CreateMap<models.AgentDeclinedType, CompanyAgentDeclinedType>();
                cfg.CreateMap<models.Agent, CompanyAgent>();
                cfg.CreateMap<models.AgentType, CompanyAgentType>();
            });
            return config;


        }
        public static IMapper CreatePrefixAgeuntIndivialId()
        {
            var config = MapperCache.GetMapper<CompanyPrefixs, BasePrefix>(cfg =>
            {
                cfg.CreateMap<CompanyPrefixs, BasePrefix>();

                cfg.CreateMap<BasePrefix, CompanyPrefixs>();
            });
            return config;
        }
        public static IMapper CreateMapperAgency()
        {
            var config = MapperCache.GetMapper<models.Agency, CompanyAgency>(cfg =>
           {
               cfg.CreateMap<CompanyAgency, models.Agency>();
               cfg.CreateMap<comm.CompanyBranch, Branch>();
               cfg.CreateMap<CompanyAgentDeclinedType, models.AgentDeclinedType>();
               cfg.CreateMap<CompanyAgent, models.Agent>();
               cfg.CreateMap<CompanyAgentType, models.AgentType>();

               cfg.CreateMap<models.Agency, CompanyAgency>();
               cfg.CreateMap<Branch, comm.CompanyBranch>();
               cfg.CreateMap<models.AgentDeclinedType, CompanyAgentDeclinedType>();
               cfg.CreateMap<models.Agent, CompanyAgent>();
               cfg.CreateMap<models.AgentType, CompanyAgentType>();

           });
            return config;
        }
        #endregion MapperAgetn V1

        #region MapperInsured V1
        public static IMapper CreateMapperInsured()
        {
            var config = MapperCache.GetMapper<CompanyInsured, models.Insured>(cfg =>
            {
                cfg.CreateMap<CompanyAgent, models.Agent>();
                cfg.CreateMap<CompanyAgentDeclinedType, models.AgentDeclinedType>();
                cfg.CreateMap<CompanyAgentType, models.AgentType>();
                cfg.CreateMap<CompanyGroupAgent, models.GroupAgent>();
                cfg.CreateMap<CompanySalesChannel, models.SalesChannel>();
                cfg.CreateMap<CompanyEmployeePerson, models.EmployeePerson>();

                cfg.CreateMap<models.Agent, CompanyAgent>();
                cfg.CreateMap<models.AgentDeclinedType, CompanyAgentDeclinedType>();
                cfg.CreateMap<models.AgentType, CompanyAgentType>();
                cfg.CreateMap<models.GroupAgent, CompanyGroupAgent>();
                cfg.CreateMap<models.SalesChannel, CompanySalesChannel>();
                cfg.CreateMap<models.EmployeePerson, CompanyEmployeePerson>();
                //De model COMPANY a model CORE       
                cfg.CreateMap<CompanyAgency, models.Agency>();
                cfg.CreateMap<comm.CompanyBranch, Branch>();
                cfg.CreateMap<CompanyAgentDeclinedType, models.AgentDeclinedType>();
                cfg.CreateMap<CompanyAgent, models.Agent>();
                cfg.CreateMap<CompanyAgentType, models.AgentType>();

                cfg.CreateMap<models.Agency, CompanyAgency>();
                cfg.CreateMap<Branch, comm.CompanyBranch>();
                cfg.CreateMap<models.AgentDeclinedType, CompanyAgentDeclinedType>();
                cfg.CreateMap<models.Agent, CompanyAgent>();
                cfg.CreateMap<models.AgentType, CompanyAgentType>();

                cfg.CreateMap<CompanyInsured, models.Insured>();

                cfg.CreateMap<CompanyInsuredDeclinedType, models.InsuredDeclinedType>();
                cfg.CreateMap<CompanyInsuredConcept, models.InsuredConcept>();
                cfg.CreateMap<CPCOMO.CompanyBranch, Sistran.Core.Application.CommonService.Models.Branch>();
                cfg.CreateMap<CPCOMO.CompanySalesPoint, Sistran.Core.Application.CommonService.Models.SalePoint>();
                cfg.CreateMap<CompanyInsuredSegment, models.InsuredSegment>();
                cfg.CreateMap<CompanyInsuredProfile, models.InsuredProfile>();

                //De model CORE a model COMPANY
                cfg.CreateMap<models.Insured, CompanyInsured>();
                cfg.CreateMap<models.InsuredDeclinedType, CompanyInsuredDeclinedType>();
                cfg.CreateMap<models.InsuredConcept, CompanyInsuredConcept>();
                cfg.CreateMap<models.Agency, CompanyAgency>();
                cfg.CreateMap<Sistran.Core.Application.CommonService.Models.Branch, CPCOMO.CompanyBranch>();
                cfg.CreateMap<Sistran.Core.Application.CommonService.Models.SalePoint, CPCOMO.CompanySalesPoint>();
                cfg.CreateMap<models.InsuredSegment, CompanyInsuredSegment>();
                cfg.CreateMap<models.InsuredProfile, CompanyInsuredProfile>();
            });
            return config;
        }

        public static IMapper CreateMapperInsuredConcept()
        {
            var config = MapperCache.GetMapper<CompanyInsuredConcept, models.InsuredConcept>(cfg =>
            {
                //De model COMPANY a model CORE
                cfg.CreateMap<CompanyInsuredConcept, models.InsuredConcept>();

                //De model CORE a model COMPANY
                cfg.CreateMap<models.InsuredConcept, CompanyInsuredConcept>();
            });
            return config;
        }

        public static IMapper CreateMapperInsuredAgent()
        {
            var config = MapperCache.GetMapper<CompanyInsuredAgent, models.InsuredAgent>(cfg =>
            {
                //De model COMPANY a model CORE
                cfg.CreateMap<CompanyInsuredAgent, models.InsuredAgent>();

                //De model CORE a model COMPANY
                cfg.CreateMap<models.InsuredAgent, CompanyInsuredAgent>();
            });
            return config;
        }

        #endregion
        public static IMapper CreateMapperComissionIndividualId()
        {

            var config = MapperCache.GetMapper<CompanyComissionAgent, models.Commission>(cfg =>
            {
                cfg.CreateMap<CompanyComissionAgent, models.Commission>();
                cfg.CreateMap<CommonServices.Models.CompanyPrefix, Prefix>();
                cfg.CreateMap<comm.CompanyLineBusiness, LineBusiness>();
                cfg.CreateMap<comm.CompanySubLineBusiness, SubLineBusiness>();

                cfg.CreateMap<models.Commission, CompanyComissionAgent>();
                cfg.CreateMap<Prefix, CommonServices.Models.CompanyPrefix>();
                cfg.CreateMap<LineBusiness, comm.CompanyLineBusiness>();
                cfg.CreateMap<SubLineBusiness, comm.CompanySubLineBusiness>();
            });
            return config;
        }

        #region IndividualTax
        public static IMapper CreateMapperIndividualTax()
        {
            var config = MapperCache.GetMapper<models.IndividualTaxExeption, CompanyIndividualTaxExeption>(cfg =>
            {
                //De model COMPANY a model CORE
                cfg.CreateMap<CompanyIndividualTaxExeption, models.IndividualTaxExeption>();
                cfg.CreateMap<CompanyTaxCategory, models.TaxCategory>();
                cfg.CreateMap<CompanyIndividualTax, models.IndividualTax>();
                cfg.CreateMap<CompanyTax, models.Tax>();
                cfg.CreateMap<CompanyTaxCondition, models.TaxCondition>();
                //De CORE COMPANY 
                cfg.CreateMap<models.IndividualTaxExeption, CompanyIndividualTaxExeption>();
                cfg.CreateMap<models.TaxCategory, CompanyTaxCategory>();
                cfg.CreateMap<models.IndividualTax, CompanyIndividualTax>();
                cfg.CreateMap<models.Tax, CompanyTax>();
                cfg.CreateMap<models.TaxCondition, CompanyTaxCondition>();
            });
            return config;
        }
        #endregion individualTax

        #region MapperOperatingQuota V1
        public static IMapper CreateMappertOperatingQuota()
        {
            var config = MapperCache.GetMapper<CompanyOperatingQuota, models.OperatingQuota>(cfg =>
            {
                cfg.CreateMap<CompanyOperatingQuota, models.OperatingQuota>();
                cfg.CreateMap<models.OperatingQuota, CompanyOperatingQuota>();
            });
            return config;
        }

        #endregion MapperOperatingQuota V1


        #region Supplier V1

        public static IMapper CreateMapperSupplier()
        {
            var config = MapperCache.GetMapper<CompanySupplier, models.Supplier>(cfg =>
            {
                //De model COMPANY a model CORE
                cfg.CreateMap<CompanySupplier, models.Supplier>();
                cfg.CreateMap<CompanySupplierProfile, models.SupplierProfile>();
                cfg.CreateMap<CompanySupplierType, models.SupplierType>();
                cfg.CreateMap<CompanyPaymentAccountType, models.PaymentAccountType>();
                cfg.CreateMap<CompanySupplierDeclinedType, models.SupplierDeclinedType>();
                cfg.CreateMap<CompanyAccountingConcept, models.SupplierAccountingConcept>();

                //De model CORE a model COMPANY 
                cfg.CreateMap<models.Supplier, CompanySupplier>();
                cfg.CreateMap<models.SupplierProfile, CompanySupplierProfile>();
                cfg.CreateMap<models.SupplierType, CompanySupplierType>();
                cfg.CreateMap<models.PaymentAccountType, CompanyPaymentAccountType>();
                cfg.CreateMap<models.SupplierDeclinedType, CompanySupplierDeclinedType>();
                cfg.CreateMap<models.SupplierAccountingConcept, CompanyAccountingConcept>();
            });
            return config;
        }

        public static IMapper CreateMapperSupplierAccountingConcept()
        {
            var config = MapperCache.GetMapper<CompanySupplierAccountingConcept, models.SupplierAccountingConcept>(cfg =>
            {
                //De model COMPANY a model CORE
                cfg.CreateMap<CompanySupplierAccountingConcept, models.SupplierAccountingConcept>();

                //De model CORE a model COMPANY 
                cfg.CreateMap<models.SupplierAccountingConcept, CompanySupplierAccountingConcept>();
            });
            return config;
        }

        public static IMapper CreateMapperAccountingConcept()
        {
            var config = MapperCache.GetMapper<CompanyAccountingConcept, models.AccountingConcept>(cfg =>
            {
                //De model COMPANY a model CORE
                cfg.CreateMap<CompanyAccountingConcept, models.AccountingConcept>();

                //De model CORE a model COMPANY 
                cfg.CreateMap<models.AccountingConcept, CompanyAccountingConcept>();
            });
            return config;
        }

        public static IMapper CreateMapperSupplierProfile()
        {
            var config = MapperCache.GetMapper<CompanySupplierProfile, models.SupplierProfile>(cfg =>
            {
                //De model COMPANY a model CORE
                cfg.CreateMap<CompanySupplierProfile, models.SupplierProfile>();

                //De model CORE a model COMPANY 
                cfg.CreateMap<models.SupplierProfile, CompanySupplierProfile>();
            });
            return config;
        }

        public static IMapper GetCompanyGroupSupplier()
        {
            var config = MapperCache.GetMapper<CompanyGroupSupplier, models.GroupSupplier>(cfg =>
            {
                //De model COMPANY a model CORE
                cfg.CreateMap<CompanyGroupSupplier, models.GroupSupplier>();

                //De model CORE a model COMPANY 
                cfg.CreateMap<models.GroupSupplier, CompanyGroupSupplier>();
            });
            return config;
        }


        #endregion Supplier V1


        #region CoCompanyName V1

        public static IMapper CreateMapperCoCompanyName()
        {
            var config = MapperCache.GetMapper<CompanyCoCompanyName, models.CompanyName>(cfg =>
            {
                //De model COMPANY a model CORE
                cfg.CreateMap<CompanyCoCompanyName, models.CompanyName>();
                cfg.CreateMap<CompanyAddress, models.Address>();
                cfg.CreateMap<CompanyPhone, models.Phone>();
                cfg.CreateMap<CompanyEmail, models.Email>();

                //De model CORE a model COMPANY 
                cfg.CreateMap<models.CompanyName, CompanyCoCompanyName>();
                cfg.CreateMap<models.Address, CompanyAddress>();
                cfg.CreateMap<models.Phone, CompanyPhone>();
                cfg.CreateMap<models.Email, CompanyEmail>();
            });
            return config;
        }

        #endregion CoCompanyName V1


        #region MapperIndividualRole V1
        public static IMapper CreateMapperIndividualRole()
        {
            var config = MapperCache.GetMapper<CompanyIndividualRole, models.IndividualRole>(cfg =>
            {
                cfg.CreateMap<CompanyIndividualRole, models.IndividualRole>();
                cfg.CreateMap<models.IndividualRole, CompanyIndividualRole>();
            });
            return config;
        }

        #endregion MapperIndividualRole V1

        #region MapperCoInsured
        public static IMapper CretaeCoInsured()
        {
            var config = MapperCache.GetMapper<CompanyCoInsured, models.CompanyCoInsured>(cfg =>
            {
                cfg.CreateMap<CompanyCoInsured, models.CompanyCoInsured>();
                cfg.CreateMap<CompanyIdentificationDocument, models.IdentificationDocument>();
                cfg.CreateMap<CompanyEconomicActivity, models.EconomicActivity>();
                cfg.CreateMap<CompanyIndividualPaymentMethod, models.IndividualPaymentMethod>();

                cfg.CreateMap<models.CompanyCoInsured, CompanyCoInsured>();
                cfg.CreateMap<models.IdentificationDocument, CompanyIdentificationDocument>();
                cfg.CreateMap<models.EconomicActivity, CompanyEconomicActivity>();
                cfg.CreateMap<models.IndividualPaymentMethod, CompanyIndividualPaymentMethod>();
            });
            return config;
        }
        #endregion

        #region MapperConsortuim
        public static IMapper CreateMapperConsortium()
        {
            CreateMapperPerson();
            CreateMapperCompany();
            var config = MapperCache.GetMapper<CompanyConsortium, models.Consortium>(cfg =>
            {
                cfg.CreateMap<CompanyConsortium, models.Consortium>();
                cfg.CreateMap<CoConsurtuimCompany, models.Company>();
                cfg.CreateMap<CompanyRole, models.Role>();
                cfg.CreateMap<CompanyEconomicActivity, models.EconomicActivity>();
                cfg.CreateMap<CompanyIdentificationDocument, models.IdentificationDocument>();
                cfg.CreateMap<CompanyCompanyType, models.CompanyType>();
                cfg.CreateMap<CompanyAssociationType, models.AssociationType>();
                cfg.CreateMap<CompanyMaritalStatus, models.MaritalStatus>();
                cfg.CreateMap<CompanyEducativeLevel, models.EducativeLevel>();
                cfg.CreateMap<CompanySocialLayer, models.SocialLayer>();
                cfg.CreateMap<CompanyHouseType, models.HouseType>();
                cfg.CreateMap<CompanyPersonType, models.PersonType>();

                cfg.CreateMap<models.Consortium, CompanyConsortium>();
                cfg.CreateMap<models.Company, CoConsurtuimCompany>();
                cfg.CreateMap<models.Role, CompanyRole>();
                cfg.CreateMap<models.EconomicActivity, CompanyEconomicActivity>();
                cfg.CreateMap<models.IdentificationDocument, CompanyIdentificationDocument>();
                cfg.CreateMap<models.CompanyType, CompanyCompanyType>();
                cfg.CreateMap<models.AssociationType, CompanyAssociationType>();
                cfg.CreateMap<models.MaritalStatus, CompanyMaritalStatus>();
                cfg.CreateMap<models.EducativeLevel, CompanyEducativeLevel>();
                cfg.CreateMap<models.SocialLayer, CompanySocialLayer>();
                cfg.CreateMap<models.HouseType, CompanyHouseType>();
                cfg.CreateMap<models.PersonType, CompanyPersonType>();
            });
            return config;
        }
        #endregion

        #region Company


        public static IMapper CreateMapperCompany()
        {
            var config = MapperCache.GetMapper<CompanyCompany, models.Company>(cfg =>
            {
                //De model COMPANY a model CORE
                cfg.CreateMap<CompanyCompany, models.Company>();
                cfg.CreateMap<CompanyRole, models.Role>();
                cfg.CreateMap<CompanyEconomicActivity, models.EconomicActivity>();
                cfg.CreateMap<CompanyIdentificationDocument, models.IdentificationDocument>();
                cfg.CreateMap<CompanyMaritalStatus, models.MaritalStatus>();
                cfg.CreateMap<CompanyEducativeLevel, models.EducativeLevel>();
                cfg.CreateMap<CompanySocialLayer, models.SocialLayer>();
                cfg.CreateMap<CompanyHouseType, models.HouseType>();
                cfg.CreateMap<CompanyCompanyType, models.CompanyType>();
                cfg.CreateMap<CompanyDocumentType, models.DocumentType>();
                cfg.CreateMap<CompanyAssociationType, models.AssociationType>();
                cfg.CreateMap<CompanyExonerationType, models.ExonerationType>();
                cfg.CreateMap<CompanyInsured, models.Insured>();
                cfg.CreateMap<CompanyConsortium, models.Consortium>();

                cfg.CreateMap<CompanyAddress, models.Address>();
                cfg.CreateMap<Models.CompanyAddressType, models.AddressType>();

                cfg.CreateMap<CompanyPhone, models.Phone>();
                cfg.CreateMap<Models.CompanyPhoneType, models.PhoneType>();

                cfg.CreateMap<CompanyEmail, models.Email>();
                cfg.CreateMap<Models.CompanyEmailType, models.EmailType>();

                //De model CORE a model COMPANY 


                cfg.CreateMap<models.Company, CompanyCompany>();
                cfg.CreateMap<models.Role, CompanyRole>();
                cfg.CreateMap<models.EconomicActivity, CompanyEconomicActivity>();
                cfg.CreateMap<models.IdentificationDocument, CompanyIdentificationDocument>();
                cfg.CreateMap<models.MaritalStatus, CompanyMaritalStatus>();
                cfg.CreateMap<models.EducativeLevel, CompanyEducativeLevel>();
                cfg.CreateMap<models.SocialLayer, CompanySocialLayer>();
                cfg.CreateMap<models.HouseType, CompanyHouseType>();
                cfg.CreateMap<models.CompanyType, CompanyCompanyType>();
                cfg.CreateMap<models.DocumentType, CompanyDocumentType>();
                cfg.CreateMap<models.AssociationType, CompanyAssociationType>();

                cfg.CreateMap<models.ExonerationType, CompanyExonerationType>();

                cfg.CreateMap<models.Address, CompanyAddress>();
                cfg.CreateMap<models.AddressType, Models.CompanyAddressType>();

                cfg.CreateMap<models.Phone, CompanyPhone>();
                cfg.CreateMap<models.PhoneType, Models.CompanyPhoneType>();

                cfg.CreateMap<models.Email, CompanyEmail>();
                cfg.CreateMap<models.EmailType, Models.CompanyEmailType>();
                cfg.CreateMap<models.Insured, CompanyInsured>();
                cfg.CreateMap<models.Consortium, CompanyConsortium>();

            });
            return config;
        }


        /// <summary>
        /// Crear una Compañia
        /// </summary>
        /// <param name="company">Compañia</param>
        /// <returns></returns>
        public static Models.CompanyCompany CreateCompany(entities.Company company)
        {
            CompanyIdentificationDocument identificationDocument = new CompanyIdentificationDocument
            {
                DocumentType = new CompanyDocumentType { Id = company.TributaryIdTypeCode },
                Number = company.TributaryIdNo
            };

            Country country = new Country
            {
                Id = company.CountryCode
            };

            CompanyCompanyType companyType = new CompanyCompanyType
            {
                Id = company.CompanyTypeCode
            };

            return new Models.CompanyCompany
            {
                IndividualId = company.IndividualId,
                FullName = company.TradeName,
                IdentificationDocument = identificationDocument,
                CountryId = country.Id,
                CompanyType = companyType,
                IndividualType = enums.IndividualType.Company
            };
        }

        /// <summary>
        /// Crear Lista Compañias
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.CompanyCompany> CreateCompanies(BusinessCollection businessCollection)
        {
            List<Models.CompanyCompany> companies = new List<Models.CompanyCompany>();

            foreach (entities.Company field in businessCollection)
            {
                companies.Add(ModelAssembler.CreateCompany(field));
            }

            return companies;
        }

        #endregion

        #region Person
        /// <summary>
        /// Crear una Persona
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public static Models.CompanyPerson CreatePerson(entities.Person person)
        {

            int EducativeLevelCode = 0;
            int HouseTypeCode = 0;
            int SocialLayerCode = 0;

            if (person.EducativeLevelCode != null)
            {
                EducativeLevelCode = (int)person.EducativeLevelCode;
            }
            if (person.HouseTypeCode != null)
            {
                HouseTypeCode = (int)person.HouseTypeCode;
            }
            if (person.SocialLayerCode != null)
            {
                SocialLayerCode = (int)person.SocialLayerCode;
            }

            return new Models.CompanyPerson
            {
                IndividualId = person.IndividualId,
                Gender = person.Gender,
                BirthDate = person.BirthDate,
                BirthPlace = person.BirthPlace,
                Children = person.Children,
                Names = person.Name,
                IdentificationDocument = new CompanyIdentificationDocument { DocumentType = new CompanyDocumentType { Id = person.IdCardTypeCode }, Number = person.IdCardNo },
                SurName = person.Surname,
                SecondSurName = person.MotherLastName,
                Name = person.Surname + " " + person.MotherLastName + " " + person.Name,
                EducativeLevel = new CompanyEducativeLevel { Id = EducativeLevelCode },
                SpouseName = person.SpouseName,
                HouseType = new CompanyHouseType { Id = HouseTypeCode },
                SocialLayer = new CompanySocialLayer { Id = SocialLayerCode },
                MaritalStatus = new CompanyMaritalStatus { Id = person.MaritalStatusCode },
                IndividualType = enums.IndividualType.Person,
                PersonType = new CompanyPersonType { Id = Convert.ToInt32(person.PersonTypeCode) },
                DataProtection = person.DataProtection,
                CustomerType = enums.CustomerType.Individual
            };
        }



        /// <summary>
        /// Crear Lista Personas
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.CompanyPerson> CreatePersons(BusinessCollection businessCollection)
        {
            List<Models.CompanyPerson> persons = new List<Models.CompanyPerson>();

            foreach (entities.Person field in businessCollection)
            {
                persons.Add(ModelAssembler.CreatePerson(field));
            }

            return persons;
        }

        #endregion

        #region prospectos
        /// <summary>
        /// Crear Prospecto
        /// </summary>
        /// <param name="prospectNatural">The prospect natural.</param>
        /// <returns></returns>
        public static Models.CompanyPerson CreatePersonProspect(entities.Prospect prospectNatural)
        {

            List<models.Email> emails = new List<models.Email>();
            emails.Add(new models.Email { Description = prospectNatural.EmailAddress });
            List<models.Phone> phones = new List<models.Phone>();
            phones.Add(new models.Phone { Description = prospectNatural.PhoneNumber.ToString() });
            List<models.Address> address = new List<models.Address>();
            address.Add(new models.Address { Id = Convert.ToInt32(prospectNatural.AddressTypeCode), Description = prospectNatural.Street });

            City city = new City();
            if (prospectNatural.CityCode != null)
            {
                Country country = new Country();
                country.Id = prospectNatural.CountryCode.Value;

                State state = new State();
                state.Id = prospectNatural.StateCode.Value;
                state.Country = country;

                city.Id = prospectNatural.CityCode.Value;
                city.State = state;
            }

            return new Models.CompanyPerson
            {
                //PersonCode = Convert.ToInt32(prospectNatural.ProspectId),
                //Gender = prospectNatural.Gender,
                //BirthDate = Convert.ToDateTime(prospectNatural.BirthDate),
                //Names = prospectNatural.Name,
                //IdentificationDocument = new models.IdentificationDocument { DocumentType = new models.DocumentType { Id = Convert.ToInt32(prospectNatural.IdCardTypeCode) }, Number = prospectNatural.IdCardNo },
                //Surname = prospectNatural.Surname,
                //MotherLastName = prospectNatural.MotherLastName,
                //Name = prospectNatural.Surname + " " + prospectNatural.MotherLastName + " " + prospectNatural.Name,
                //MaritalStatus = new models.MaritalStatus { Id = Convert.ToInt32(prospectNatural.MaritalStatusCode) },
                //City = city,
                //Phones = phones,
                //Emails = emails,
                //Addresses = address
            };

        }
        /// <summary>
        /// Crear Lista prospectos
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.CompanyPerson> CreatePersonProspects(BusinessCollection businessCollection)
        {
            List<Models.CompanyPerson> prospects = new List<Models.CompanyPerson>();
            foreach (entities.Prospect item in businessCollection)
            {
                prospects.Add(ModelAssembler.CreatePersonProspect(item));
            }
            return prospects;
        }
        /// <summary>
        /// Crear Prospecto legal
        /// </summary>
        /// <param name="prospectLegal">The prospect legal.</param>
        /// <returns></returns>
        public static Models.CompanyCompany CreatePersonProspectlegal(entities.Prospect prospectLegal)
        {
            CompanyIdentificationDocument identificationDocument = new CompanyIdentificationDocument
            {
                DocumentType = new CompanyDocumentType { Id = Convert.ToInt32(prospectLegal.TributaryIdTypeCode) },
                Number = prospectLegal.TributaryIdNo,
            };

            City city = new City();
            if (prospectLegal.CityCode != null)
            {
                Country country = new Country();
                country.Id = prospectLegal.CountryCode.Value;

                State state = new State();
                state.Id = prospectLegal.StateCode.Value;
                state.Country = country;

                city.Id = prospectLegal.CityCode.Value;
                city.State = state;
            }


            CompanyCompanyType companyType = new CompanyCompanyType
            {
                Id = Convert.ToInt32(prospectLegal.CompanyTypeCode)
            };

            List<models.Address> address = new List<models.Address>();
            address.Add(new models.Address { Id = Convert.ToInt32(prospectLegal.AddressTypeCode), Description = prospectLegal.Street });
            address[0].City = city;

            List<models.Email> emails = new List<models.Email>();
            emails.Add(new models.Email { Description = prospectLegal.EmailAddress });

            List<models.Phone> phones = new List<models.Phone>();
            phones.Add(new models.Phone { Description = prospectLegal.PhoneNumber.ToString() });

            return new Models.CompanyCompany
            {
                IndividualId = prospectLegal.ProspectId,
                FullName = prospectLegal.TradeName,
                IdentificationDocument = identificationDocument,
                CompanyType = companyType,
            };
        }

        /// <summary>
        /// Crear Lista prospectos legal
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.CompanyCompany> CreatePersonProspectsLegals(BusinessCollection businessCollection)
        {
            List<Models.CompanyCompany> prospects = new List<Models.CompanyCompany>();
            foreach (entities.Prospect item in businessCollection)
            {
                prospects.Add(ModelAssembler.CreatePersonProspectlegal(item));
            }
            return prospects;
        }
        #endregion

        #region Sarlaft
        /// <summary>
        /// Crear sarlaft.
        /// </summary>
        /// <param name="sarlaft">The sarlaft.</param>
        /// <returns></returns>
        public static Models.FinancialSarlaf CreateFinancialSarlaft(UPENT.FinancialSarlaft sarlaft)
        {
            Models.IndividualSarlaft sarlaftId = new Models.IndividualSarlaft();
            sarlaftId.Id = sarlaft.SarlaftId;
            return new Models.FinancialSarlaf
            {
                SarlaftId = sarlaft.SarlaftId,
                IncomeAmount = sarlaft.IncomeAmount,
                ExpenseAmount = sarlaft.ExpenseAmount,
                ExtraIncomeAmount = sarlaft.ExtraIncomeAmount,
                AssetsAmount = sarlaft.AssetsAmount,
                LiabilitiesAmount = sarlaft.LiabilitiesAmount,
                Description = sarlaft.Description,
                IsForeignTransaction = sarlaft.IsForeingTransaction != null ? sarlaft.IsForeingTransaction.Value : false,
                ForeignTransactionAmount = sarlaft.ForeingTransactionAmount

            };
        }
        #region Cupo Operativo


        internal static List<CompanyOrPerson> CreateCompanies1(BusinessCollection businessObjectsCompany)
        {
            List<CompanyOrPerson> companyOrPersons = new List<CompanyOrPerson>();
            foreach (entities.Company company in businessObjectsCompany)
            {
                companyOrPersons.Add(ModelAssembler.CreateCompanyOrPerson1(company));
            }

            return companyOrPersons;
        }

        private static CompanyOrPerson CreateCompanyOrPerson1(entities.Company company)
        {
            return new CompanyOrPerson
            {
                Document = Convert.ToInt32(company.TributaryIdNo),
                Name = company.TradeName,
                IndividualID = company.IndividualId
            };
        }

        internal static List<CompanyOrPerson> CreatePersons1(BusinessCollection businessObjectsCompany)
        {
            List<CompanyOrPerson> companyOrPersons = new List<CompanyOrPerson>();
            foreach (entities.Person person in businessObjectsCompany)
            {
                companyOrPersons.Add(ModelAssembler.CreateCompanyOrPerson(person));
            }

            return companyOrPersons;
        }

        private static CompanyOrPerson CreateCompanyOrPerson(entities.Person person)
        {
            return new CompanyOrPerson
            {
                Document = Convert.ToInt32(person.IdCardNo),
                Name = person.Name,
                IndividualID = person.IndividualId
            };
        }
        #endregion
        /// <summary>
        /// Crear lista sarlafts.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.FinancialSarlaf> CreateFinancialSarlaft(BusinessCollection businessCollection)
        {
            List<Models.FinancialSarlaf> sarlafts = new List<Models.FinancialSarlaf>();
            foreach (UPENT.FinancialSarlaft field in businessCollection)
            {
                sarlafts.Add(ModelAssembler.CreateFinancialSarlaft(field));
            }

            return sarlafts;
        }
        #endregion

        #region IndividualSarlaft
        /// <summary>
        /// Crear Individual sarlaft.
        /// </summary>
        /// <param name="individualSarlaft">The individual sarlaft.</param>
        /// <returns></returns>
        public static Models.IndividualSarlaft CreateIndividualSarlaft(UPENT.IndividualSarlaft individualSarlaft)
        {
            Models.FinancialSarlaf financialSarlaftModel = new Models.FinancialSarlaf();
            SarlaftDAO sarlaftDAO = new SarlaftDAO();
            financialSarlaftModel = sarlaftDAO.GetFinancialSarlaftBySarlaftId(individualSarlaft.SarlaftId);

            return new Models.IndividualSarlaft
            {
                Id = individualSarlaft.SarlaftId,
                IndividualId = individualSarlaft.IndividualId,
                FormNum = individualSarlaft.FormNum,
                Year = individualSarlaft.Year,
                AuthorizedBy = individualSarlaft.AuthorizedBy,
                VerifyingEmployee = individualSarlaft.VerifyingEmployee,
                InterviewerName = individualSarlaft.InterviewerName,
                InternationalOperations = Convert.ToBoolean(individualSarlaft.InternationalOperations),
                InterviewerPlace = individualSarlaft.InterviewPlace,
                InterviewResultCode = individualSarlaft.InterviewResultCode,
                PendingEvents = individualSarlaft.PendingEvent,
                RegistrationDate = individualSarlaft.RegistrationDate,
                BranchCode = Convert.ToInt32(individualSarlaft.BranchCode),
                FinancialSarlaft = financialSarlaftModel,

            };
        }
        #endregion

        #region IndividualSarlaftV2
        /// <summary>
        /// Crear Individual sarlaft.
        /// </summary>
        /// <param name="individualSarlaft">The individual sarlaft.</param>
        /// <returns></returns>
        public static Models.IndividualSarlaft CreateIndividualSarlaft(entities.IndividualSarlaft individualSarlaft)
        {
            Models.FinancialSarlaf financialSarlaftModel = new Models.FinancialSarlaf();
            SarlaftDAO sarlaftDAO = new SarlaftDAO();
            financialSarlaftModel = sarlaftDAO.GetFinancialSarlaftBySarlaftId(individualSarlaft.SarlaftId);

            return new Models.IndividualSarlaft
            {
                Id = individualSarlaft.SarlaftId,
                IndividualId = individualSarlaft.IndividualId,
                FormNum = individualSarlaft.FormNum,
                Year = individualSarlaft.Year,
                AuthorizedBy = individualSarlaft.AuthorizedBy,
                VerifyingEmployee = individualSarlaft.VerifyingEmployee,
                InterviewerName = individualSarlaft.InterviewerName,
                InternationalOperations = Convert.ToBoolean(individualSarlaft.InternationalOperations),
                InterviewerPlace = individualSarlaft.InterviewPlace,
                InterviewResultCode = individualSarlaft.InterviewResultCode,
                PendingEvents = individualSarlaft.PendingEvent,
                RegistrationDate = individualSarlaft.RegistrationDate,
                BranchCode = Convert.ToInt32(individualSarlaft.BranchCode),
                FinancialSarlaft = financialSarlaftModel,

            };
        }

        /// <summary>
        /// Creates the individual sarlafts.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.IndividualSarlaft> CreateIndividualSarlafts(BusinessCollection businessCollection)
        {
            List<Models.IndividualSarlaft> individualsarlafts = new List<Models.IndividualSarlaft>();
            foreach (entities.IndividualSarlaft field in businessCollection)
            {
                individualsarlafts.Add(ModelAssembler.CreateIndividualSarlaft(field));
            }
            return individualsarlafts;
        }
        #endregion

        #region SarlaftExoneration

        /// <summary>
        /// Creates the sarlaft exoneration.
        /// </summary>
        /// <param name="sarlaftExoneration">The sarlaft exoneration.</param>
        /// <returns></returns>
        public static Models.CompanySarlaftExoneration CreateSarlaftExoneration(UPENT.IndividualSarlaftExoneration sarlaftExoneration)
        {
            return new Models.CompanySarlaftExoneration
            {
                Id = sarlaftExoneration.IndividualId,
                EnteredDate = sarlaftExoneration.RegistrationDate,
                UserId = sarlaftExoneration.UserId,
                ExonerationType = new CompanyExonerationType { Id = sarlaftExoneration.ExonerationTypeCode == null ? 0 : (int)sarlaftExoneration.ExonerationTypeCode }
            };
        }

        /// <summary>
        /// Creates the sarlaft exonerations.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.CompanySarlaftExoneration> CreateSarlaftExonerations(BusinessCollection businessCollection)
        {
            List<Models.CompanySarlaftExoneration> sarlaftExonerations = new List<Models.CompanySarlaftExoneration>();

            foreach (UPENT.IndividualSarlaftExoneration field in businessCollection)
            {
                sarlaftExonerations.Add(ModelAssembler.CreateSarlaftExoneration(field));
            }

            return sarlaftExonerations;
        }

        #endregion

        #region CoConsortium
        /// <summary>
        /// Creates the consortium.
        /// </summary>
        /// <param name="coConsortium">The co consortium.</param>
        /// <returns></returns>
        public static Models.CompanyConsortium CreateConsortium(CoConsortium coConsortium)
        {
            return new Models.CompanyConsortium
            {
                ConsortiumId = coConsortium.ConsortiumId,
                IsMain = coConsortium.IsMain,
                ParticipationRate = coConsortium.ParticipationRate,
                StartDate = coConsortium.StartDate,
                Enabled = coConsortium.Enabled
            };
        }
        /// <summary>
        /// Creates the consortium.
        /// </summary>
        /// <param name="coConsortium">The co consortium.</param>
        /// <param name="coConsortiumView">The co consortium view.</param>
        /// <returns></returns>
        public static Models.CompanyConsortium CreateConsortium(CoConsortium coConsortium, Entities.views.CoConsorcioViewV1 consortiumView)
        {
            Models.CompanyCompany mc = null;
            Models.CompanyPerson pc = null;
            foreach (entities.Company ec in consortiumView.CompanyList)
            {
                if (ec.IndividualId == coConsortium.IndividualId)
                {
                    mc = new Models.CompanyCompany();
                    mc = CreateCompany(ec);
                    break;
                }
            }
            foreach (entities.Person ec in consortiumView.CoConsortiumPersonList)
            {
                if (ec.IndividualId == coConsortium.IndividualId)
                {
                    pc = new Models.CompanyPerson();
                    pc = CreatePerson(ec);
                    break;
                }
            }

            return new Models.CompanyConsortium
            {
                ConsortiumId = coConsortium.ConsortiumId,
                IsMain = coConsortium.IsMain,
                ParticipationRate = coConsortium.ParticipationRate,
                StartDate = coConsortium.StartDate,
                Enabled = coConsortium.Enabled,
                IndividualId = coConsortium.IndividualId,
                InsuredCode = coConsortium.InsuredCode,
                Company = mc,
                Person = pc

            };
        }

        /// <summary>
        /// Creates the co consortiums.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.CompanyConsortium> CreateCoConsortiums(Entities.views.CoConsorcioViewV1 businessCollection)
        {
            List<Models.CompanyConsortium> coConsortiums = new List<Models.CompanyConsortium>();
            foreach (CoConsortium field in businessCollection.CoConsortiumList)
            {
                coConsortiums.Add(ModelAssembler.CreateConsortium(field, businessCollection));
            }
            return coConsortiums;
        }
        #endregion

        #region CoInsurer
        /// <summary>
        /// Creates the coinsurer.
        /// </summary>
        /// <param name="coInsurance">The co coInsurer.</param>
        /// <returns></returns>
        public static List<CoInsurerCompany> CreateCoInsurer(BusinessCollection<Sistran.Core.Application.Common.Entities.CoInsuranceCompany> coInsurer)
        {
            List<Models.CoInsurerCompany> coInsurerModel = null;
            foreach (var item in coInsurer)
            {
                coInsurerModel = new List<Models.CoInsurerCompany>(){new Models.CoInsurerCompany(){
                    InsuranceCompanyId = item.InsuranceCompanyId,
                    Description = item.Description,
                AddressTypeCode = item.AddressTypeCode,
                Street = item.Street,
                PostalCode = item.PostalCode,
                AreaCode = item.AreaCode,
                ColonyCode = item.ColonyCode,
                CityCode = item.CityCode,
                StateCode = item.StateCode,
                CountryCode = item.CountryCode,
                PhoneTypeCode = item.PhoneTypeCode,
                PhoneNumber = item.PhoneNumber,
                IvaTypeCode = item.IvaTypeCode,
                TributaryIdNo = item.TributaryIdNo,
                PilotingSpendAmount = item.PilotingSpendAmount,
                YearMinSignInQuantity = item.YearMinSignInQuantity,
                YearMaxSignInQuantity = item.YearMaxSignInQuantity,
                YearMaxLongQuantity = item.YearMaxLongQuantity,
                SmallDescription = item.SmallDescription,
                EnsureInd = item.EnsureInd,
                EnteredDate = item.EnteredDate,
                ModifyDate = item.ModifyDate,
                DeclinedDate = item.DeclinedDate,
                ComDeclinedTypeCode = item.ComDeclinedTypeCode,
                Annotations = item.Annotations
            } };
            }
            return coInsurerModel;
        }
        public static CoInsurerCompany CreateCoInsurer(Core.Application.Common.Entities.CoInsuranceCompany coInsurer)
        {
            CoInsurerCompany coInsurerModel = new Models.CoInsurerCompany()
            {
                InsuranceCompanyId = coInsurer.InsuranceCompanyId,
                Description = coInsurer.Description,
                AddressTypeCode = coInsurer.AddressTypeCode,
                Street = coInsurer.Street,
                PostalCode = coInsurer.PostalCode,
                AreaCode = coInsurer.AreaCode,
                ColonyCode = coInsurer.ColonyCode,
                CityCode = coInsurer.CityCode,
                StateCode = coInsurer.StateCode,
                CountryCode = coInsurer.CountryCode,
                PhoneTypeCode = coInsurer.PhoneTypeCode,
                PhoneNumber = coInsurer.PhoneNumber,
                IvaTypeCode = coInsurer.IvaTypeCode,
                TributaryIdNo = coInsurer.TributaryIdNo,
                PilotingSpendAmount = coInsurer.PilotingSpendAmount,
                YearMinSignInQuantity = coInsurer.YearMinSignInQuantity,
                YearMaxSignInQuantity = coInsurer.YearMaxSignInQuantity,
                YearMaxLongQuantity = coInsurer.YearMaxLongQuantity,
                SmallDescription = coInsurer.SmallDescription,
                EnsureInd = coInsurer.EnsureInd,
                EnteredDate = coInsurer.EnteredDate,
                ModifyDate = coInsurer.ModifyDate,
                DeclinedDate = coInsurer.DeclinedDate,
                ComDeclinedTypeCode = coInsurer.ComDeclinedTypeCode,
                Annotations = coInsurer.Annotations
            };
            return coInsurerModel;
        }

        public static void CopyCompanyToCoInsurer(entities.Company company, Models.CoInsurerCompany coinsurer, models.Address addresses, models.Phone phones)
        {
            coinsurer.TributaryIdNo = company.TributaryIdNo;
            coinsurer.Description = company.TradeName;
            coinsurer.CountryCode = company.CountryCode;
            coinsurer.PhoneNumber = phones.Description;
            coinsurer.PhoneTypeCode = phones.PhoneType.Id;
            coinsurer.AddressTypeCode = addresses.AddressType.Id;
            coinsurer.CountryCode = addresses.City.State.Country.Id;
            coinsurer.StateCode = addresses.City.State.Id;
            coinsurer.CityCode = addresses.City.Id;
            coinsurer.Street = addresses.Description;
        }

        public static List<OthersDeclinedTypes> GetAllOthersDeclinedTypes(BusinessCollection declinedTypes)
        {
            List<OthersDeclinedTypes> declinedTypesModel = new List<OthersDeclinedTypes>();
            foreach (OthersDeclinedType item in declinedTypes)
            {
                declinedTypesModel.Add(new OthersDeclinedTypes()
                {
                    Description = item.Description,
                    SmalDescription = item.SmallDescription,
                    Id = item.OtherDeclinedTypeCode,
                    RoleCd = item.RoleCode == null ? 0 : (decimal)item.RoleCode
                });
            }
            return declinedTypesModel;
        }
        #endregion

        #region CompanyInsured
        public static Sistran.Core.Application.UniquePersonService.V1.Models.Insured CreateCoreInsured(Models.CompanyInsured companyInsured)
        {
            Sistran.Core.Application.UniquePersonService.V1.Models.Insured coreInsurer = new Sistran.Core.Application.UniquePersonService.V1.Models.Insured();
            var imapper = CreateMapInsured();
            return imapper.Map<CompanyInsured, Sistran.Core.Application.UniquePersonService.V1.Models.Insured>(companyInsured);


        }

        public static models.Agency CreateAgency(Sistran.Core.Application.UniquePersonService.V1.Models.Insured coreInsured)
        {
            if (coreInsured?.Agency != null)
            {
                return new models.Agency()
                {
                    Annotations = coreInsured.Agency.Annotations,
                    AgentDeclinedType = coreInsured.Agency.AgentDeclinedType,
                    ExtendedProperties = coreInsured.Agency.ExtendedProperties,
                    DateDeclined = coreInsured.Agency.DateDeclined,
                    AgentType = coreInsured.Agency.AgentType,
                    Branch = coreInsured.Agency.Branch,
                    Code = coreInsured.Agency.Code,
                    FullName = coreInsured.Agency.FullName,
                    Id = coreInsured.Agency.Id,
                    IsPrincipal = coreInsured.Agency.IsPrincipal,
                    Participation = coreInsured.Agency.Participation,
                    Agent = new models.Agent()
                    {
                        FullName = coreInsured.Agency.Agent.FullName,
                        IndividualId = coreInsured.Agency.Agent.IndividualId,
                        Annotations = coreInsured.Agency.Agent.Annotations,
                        AgentType = coreInsured.Agency.Agent.AgentType,
                        AgentDeclinedType = coreInsured.Agency.Agent.AgentDeclinedType,
                        //Agencies = coreInsured.Agency.Agent.Agencies,
                        //AgentAgencies = coreInsured.Agency.Agent.AgentAgencies,
                        //CheckPayableTo = coreInsured.Agency.Agent.CheckPayableTo,
                        //ComissionAgent = coreInsured.Agency.Agent.ComissionAgent,
                        DateCurrent = coreInsured.Agency.Agent.DateCurrent,
                        DateDeclined = coreInsured.Agency.Agent.DateDeclined,
                        DateModification = coreInsured.Agency.Agent.DateModification,
                        EmployeePerson = coreInsured.Agency.Agent.EmployeePerson,
                        ExtendedProperties = coreInsured.Agency.Agent.ExtendedProperties
                    },
                };
            }
            return new models.Agency();
        }

        public static Models.CompanyInsured CreateCompanyInsured(Sistran.Core.Application.UniquePersonService.V1.Models.Insured coreInsured)
        {
            CompanyInsured companyInsurer = new CompanyInsured();
            var imapper = CreateMapInsured();
            return imapper.Map<Sistran.Core.Application.UniquePersonService.V1.Models.Insured, CompanyInsured>(coreInsured);

        }

        #endregion

        #region AutoMapper
        #region Insured
        public static IMapper CreateMapInsured()
        {
            var config = MapperCache.GetMapper<CompanyInsured, models.Insured>(cfg =>
            {
                cfg.CreateMap<CompanyInsured, models.Insured>();
            });

            return config;
        }

        #endregion Insured
        #endregion AutoMapper

        #region Company.ScoreTypeDoc
        /// <summary>
        /// Convierte entidad a modelo del servicio
        /// </summary>
        /// <param name="cptScoreTypeDoc">Entidad del tipo de documento datacrédito</param>
        /// <returns>Modelo del tipo documento datacrédito</returns>
        public static Models.ScoreTypeDoc CreateScoreTypeDoc(UPENT.CompanyScoreTypeDoc cptScoreTypeDoc)
        {
            Models.ScoreTypeDoc cptScoreTypeDocId = new Models.ScoreTypeDoc();
            cptScoreTypeDocId.IdCardTypeScore = cptScoreTypeDoc.IdCardTypeScore;
            return new Models.ScoreTypeDoc
            {
                IdCardTypeScore = cptScoreTypeDoc.IdCardTypeScore,
                Description = cptScoreTypeDoc.Description,
                SmallDescription = cptScoreTypeDoc.SmallDescription
            };
        }

        /// <summary>
        /// Convierte lista de entidades a lista de modelos del servicio
        /// </summary>
        /// <param name="businessCollection">Lista de entidades del tipo de documento datacrédito</param>
        /// <returns>Lista de modelos del tipo documento datacrédito</returns>
        public static List<Models.ScoreTypeDoc> CreateScoreTypeDoc(BusinessCollection businessCollection)
        {
            List<Models.ScoreTypeDoc> scoreTypeDoc = new List<Models.ScoreTypeDoc>();
            foreach (UPENT.CompanyScoreTypeDoc field in businessCollection)
            {
                scoreTypeDoc.Add(ModelAssembler.CreateScoreTypeDoc(field));
            }

            return scoreTypeDoc;
        }

        /// <summary>
        /// Convierte entidad a modelo del servicio
        /// </summary>
        /// <param name="cptScoreTypeDoc">Entidad de la asociación entre el tipo de documento datacréditon y tipo de documento SISE</param>
        /// <returns>Modelo de la asociación entre el tipo documento datacréditoy tipo de documento SISE</returns>
        public static Models.ScoreTypeDoc CreateScore3gTypeDoc(UPENT.CompanyScore3gTypeDoc cptScoreTypeDoc)
        {
            Models.ScoreTypeDoc cptScoreTypeDocId = new Models.ScoreTypeDoc();
            cptScoreTypeDocId.IdCardTypeScore = cptScoreTypeDoc.IdCardTypeScore;
            return new Models.ScoreTypeDoc
            {
                IdCardTypeScore = cptScoreTypeDoc.IdCardTypeScore,
                IdScore3g = cptScoreTypeDoc.IdScore3g,
                IdCardTypeCode = cptScoreTypeDoc.IdCardTypeCode
            };
        }

        /// <summary>
        /// Convierte lista de entidades a lista de modelos del servicio
        /// </summary>
        /// <param name="businessCollection">Lista de entidades de la asociación entre el tipo de documento datacrédito y el tipo de documento SISE</param>
        /// <returns>Lista de modelos de la asociación entre el tipo documento datacréditoy el tipo documento SISE</returns>	
        public static List<Models.ScoreTypeDoc> CreateScore3gTypeDoc(BusinessCollection businessCollection)
        {
            List<Models.ScoreTypeDoc> scoreTypeDoc = new List<Models.ScoreTypeDoc>();
            foreach (UPENT.CompanyScore3gTypeDoc field in businessCollection)
            {
                scoreTypeDoc.Add(ModelAssembler.CreateScore3gTypeDoc(field));
            }

            return scoreTypeDoc;
        }
        #endregion

        #region LealRepresentative
        public static IMapper CreateMapperLegalRepresentative()
        {
            var config = MapperCache.GetMapper<CompanyLegalRepresentative, models.LegalRepresentative>(cfg =>
            {
                cfg.CreateMap<CompanyLegalRepresentative, models.LegalRepresentative>();
                cfg.CreateMap<models.LegalRepresentative, CompanyLegalRepresentative>();

                cfg.CreateMap<CompanyCity, City>();
                cfg.CreateMap<City, CompanyCity>();

                cfg.CreateMap<CompanyIdentificationDocument, models.IdentificationDocument>();
                cfg.CreateMap<models.IdentificationDocument, CompanyIdentificationDocument>();

                cfg.CreateMap<CompanyDocumentType, models.DocumentType>();
                cfg.CreateMap<models.DocumentType, CompanyDocumentType>();


                cfg.CreateMap<CompanyAmount, Amount>();
                cfg.CreateMap<Amount, CompanyAmount>();

                cfg.CreateMap<CompanyCurrency, Currency>();
                cfg.CreateMap<Currency, CompanyCurrency>();

                cfg.CreateMap<CompanyState, State>();
                cfg.CreateMap<State, CompanyState>();

                cfg.CreateMap<CompanyCountry, Country>();
                cfg.CreateMap<Country, CompanyCountry>();
            });
            return config;
        }

        #endregion

        #region PaymentMethod      
        public static IMapper CreateMapperIndividualPaymentMethod()
        {
            var config = MapperCache.GetMapper<CompanyIndividualPaymentMethod, models.IndividualPaymentMethod>(cfg =>
            {
                cfg.CreateMap<CompanyIndividualPaymentMethod, models.IndividualPaymentMethod>();
                cfg.CreateMap<models.IndividualPaymentMethod, CompanyIndividualPaymentMethod>();

                cfg.CreateMap<CompanyPaymentMethod, models.PaymentMethod>();
                cfg.CreateMap<models.PaymentMethod, CompanyPaymentMethod>();

                cfg.CreateMap<CompanyRole, models.Role>();
                cfg.CreateMap<models.Role, CompanyRole>();

                cfg.CreateMap<CompanyPaymentAccount, models.PaymentAccount>();
                cfg.CreateMap<models.PaymentAccount, CompanyPaymentAccount>();

                cfg.CreateMap<CompanyPaymentAccountType, models.PaymentAccountType>();
                cfg.CreateMap<models.PaymentAccountType, CompanyPaymentAccountType>();

                cfg.CreateMap<CompanyBankBranch, BankBranch>();
                cfg.CreateMap<BankBranch, CompanyBankBranch>();

                cfg.CreateMap<CompanyBank, Bank>();
                cfg.CreateMap<Bank, CompanyBank>();

                cfg.CreateMap<CompanyCurrency, Currency>();
                cfg.CreateMap<Currency, CompanyCurrency>();
                cfg.CreateMap<CompanyPaymentAccount, models.PaymentAccount>();
                cfg.CreateMap<models.PaymentAccount, CompanyPaymentAccount>();
            });
            return config;
        }

        internal static IMapper CreateMapperPersonNatural()
        {
            var config = MapperCache.GetMapper<CompanyProspectNatural, models.ProspectNatural>(cfg =>
            {
                cfg.CreateMap<CompanyProspectNatural, models.ProspectNatural>();
                cfg.CreateMap<models.ProspectNatural, CompanyProspectNatural>();

                cfg.CreateMap<CompanyCity, City>();
                cfg.CreateMap<City, CompanyCity>();


                cfg.CreateMap<CompanyState, State>();
                cfg.CreateMap<State, CompanyState>();

                cfg.CreateMap<CompanyCountry, Country>();
                cfg.CreateMap<Country, CompanyCountry>();
            });
            return config;
        }

        internal static IMapper CreateMapperGuaranteeInsuredGuarantee()
        {
            var config = MapperCache.GetMapper<CompanyGuaranteeInsuredGuarantee, models.GuaranteeInsuredGuarantee>(cfg =>
            {
                cfg.CreateMap<CompanyGuaranteeInsuredGuarantee, models.GuaranteeInsuredGuarantee>();
                cfg.CreateMap<models.GuaranteeInsuredGuarantee, CompanyGuaranteeInsuredGuarantee>();

                cfg.CreateMap<CompanyInsuredGuaranteeMortgage, models.InsuredGuaranteeMortgage>();
                cfg.CreateMap<models.InsuredGuaranteeMortgage, CompanyInsuredGuaranteeMortgage>();

                cfg.CreateMap<CompanyInsuredGuaranteeFixedTermDeposit, models.InsuredGuaranteeFixedTermDeposit>();
                cfg.CreateMap<models.InsuredGuaranteeFixedTermDeposit, CompanyInsuredGuaranteeFixedTermDeposit>();

                cfg.CreateMap<CompanyInsuredGuaranteePledge, models.InsuredGuaranteePledge>();
                cfg.CreateMap<models.InsuredGuaranteePledge, CompanyInsuredGuaranteePledge>();

                cfg.CreateMap<CompanyInsuredGuaranteePromissoryNote, models.InsuredGuaranteePromissoryNote>();
                cfg.CreateMap<models.InsuredGuaranteePromissoryNote, CompanyInsuredGuaranteePromissoryNote>();

                cfg.CreateMap<CompanyInsuredGuaranteeOthers, models.InsuredGuaranteeOthers>();
                cfg.CreateMap<models.InsuredGuaranteeOthers, CompanyInsuredGuaranteeOthers>();

                cfg.CreateMap<CompanyAssetType, models.AssetType>();
                cfg.CreateMap<models.AssetType, CompanyAssetType>();

                cfg.CreateMap<CompanyMeasurementType, models.MeasurementType>();
                cfg.CreateMap<models.MeasurementType, CompanyMeasurementType>();

                cfg.CreateMap<CompanyPromissoryNoteType, models.PromissoryNoteType>();
                cfg.CreateMap<models.PromissoryNoteType, CompanyPromissoryNoteType>();

                cfg.CreateMap<CompanyGuarantor, models.Guarantor>();
                cfg.CreateMap<models.Guarantor, CompanyGuarantor>();

                cfg.CreateMap<CompanyInsuredGuaranteePrefix, InsuredGuaranteePrefix>();
                cfg.CreateMap<models.InsuredGuaranteePrefix, CompanyInsuredGuaranteePrefix>();
            });
            return config;

        }
        #endregion

        #region InsuredGuaranteelog
        internal static IMapper CreateMapperInsuredGuaranteelog()
        {
            var config = MapperCache.GetMapper<CompanyInsuredGuaranteeLog, models.InsuredGuaranteeLog>(cfg =>
            {
                cfg.CreateMap<CompanyInsuredGuaranteeLog, models.InsuredGuaranteeLog>();
                cfg.CreateMap<models.InsuredGuaranteeLog, CompanyInsuredGuaranteeLog>();
            });
            return config;

        }

        #endregion

        #region MaritalStatus 
        public static IMapper CreateMapperMaritalStatus()
        {
            var config = MapperCache.GetMapper<MaritalStatus, CompanyMaritalStatus>(cfg =>
           {
               cfg.CreateMap<CompanyMaritalStatus, models.MaritalStatus>();
               cfg.CreateMap<models.MaritalStatus, CompanyMaritalStatus>();
           });
            return config;
        }
        #endregion

        #region DocumentType
        public static IMapper CreateMapperDocumentType()
        {
            var config = MapperCache.GetMapper<CompanyDocumentType, models.DocumentType>(cfg =>
            {
                cfg.CreateMap<CompanyDocumentType, models.DocumentType>();
                cfg.CreateMap<models.DocumentType, CompanyDocumentType>();
            });
            return config;
        }
        #endregion

        #region AddressType 
        public static IMapper CreateMapperAddressType()
        {
            var config = MapperCache.GetMapper<Models.CompanyAddressType, models.AddressType>(cfg =>
            {
                cfg.CreateMap<Models.CompanyAddressType, models.AddressType>();
                cfg.CreateMap<models.AddressType, Models.CompanyAddressType>();
            });
            return config;
        }
        /// <summary>
        /// Convierte lista de entidades a lista de modelos del servicio
        /// </summary>
        /// <param name="businessCollection">Lista de entidades del tipo dirección</param>
        /// <returns>Lista de modelos del tipo dirección</returns>	
        public static List<Models.CompanyAddressType> CreateCompanyAddressType(BusinessCollection businessCollection)
        {
            List<Models.CompanyAddressType> companyAddressType = new List<Models.CompanyAddressType>();
            foreach (entities.CompanyAddressType field in businessCollection)
            {
                companyAddressType.Add(ModelAssembler.CreateCompanyAddressType(field));
            }

            return companyAddressType;
        }

        /// <summary>
        /// Convierte entidad a modelo del servicio
        /// </summary>
        /// <param name="companyAddressType">Entidad del tipo dirección</param>
        /// <returns>Modelo del tipo dirección</returns>
        public static Models.CompanyAddressType CreateCompanyAddressType(entities.CompanyAddressType companyAddressType)
        {
            Models.CompanyAddressType companyAddressTypeId = new Models.CompanyAddressType();
            companyAddressTypeId.AddressTypeCode = companyAddressType.AddressTypeCode;
            return new Models.CompanyAddressType
            {
                AddressTypeCode = companyAddressType.AddressTypeCode,
                SmallDescription = companyAddressType.SmallDescription,
                TinyDescription = companyAddressType.TinyDescription,
                IsElectronicMail = Convert.ToBoolean(companyAddressType.IsElectronicMail)
            };
        }
        #endregion


        #region PhoneType
        public static IMapper CreateMapperPhoneType()
        {
            var config = MapperCache.GetMapper<Models.CompanyPhoneType, models.PhoneType>(cfg =>
            {
                cfg.CreateMap<Models.CompanyPhoneType, models.PhoneType>();
                cfg.CreateMap<models.PhoneType, Models.CompanyPhoneType>();
            });
            return config;
        }

        /// <summary>
        /// Convierte entidad a modelo del servicio
        /// </summary>
        /// <param name="companyPhoneType">Entidad del tipo teléfono</param>
        /// <returns>Modelo del tipo teléfono</returns>
        public static Models.CompanyPhoneType CreateCompanyPhoneType(entities.CompanyPhoneType companyPhoneType)
        {
            Models.CompanyPhoneType companyPhoneTypeId = new Models.CompanyPhoneType();
            companyPhoneTypeId.PhoneTypeCode = companyPhoneType.PhoneTypeCode;
            return new Models.CompanyPhoneType
            {
                PhoneTypeCode = companyPhoneType.PhoneTypeCode,
                Description = companyPhoneType.Description,
                SmallDescription = companyPhoneType.SmallDescription
            };
        }

        /// <summary>
        /// Convierte lista de entidades a lista de modelos del servicio
        /// </summary>
        /// <param name="businessCollection">Lista de entidades del tipo teléfono</param>
        /// <returns>Lista de modelos del tipo teléfono</returns>	
        public static List<Models.CompanyPhoneType> CreateCompanyPhoneType(BusinessCollection businessCollection)
        {
            List<Models.CompanyPhoneType> companyPhoneType = new List<Models.CompanyPhoneType>();
            foreach (entities.CompanyPhoneType field in businessCollection)
            {
                companyPhoneType.Add(ModelAssembler.CreateCompanyPhoneType(field));
            }

            return companyPhoneType;
        }

        #endregion

        #region EmailType
        public static IMapper CreateMapperEmailType()
        {
            var config = MapperCache.GetMapper<CompanyEmailType, models.EmailType>(cfg =>
            {
                cfg.CreateMap<CompanyEmailType, models.EmailType>();
                cfg.CreateMap<models.EmailType, CompanyEmailType>();
            });
            return config;
        }
        #endregion

        #region EconomicActivity
        public static IMapper CreateMapperEconomicActivity()
        {
            var config = MapperCache.GetMapper<CompanyEconomicActivity, models.EconomicActivity>(cfg =>
            {
                cfg.CreateMap<CompanyEconomicActivity, models.EconomicActivity>();
                cfg.CreateMap<models.EconomicActivity, CompanyEconomicActivity>();
            });
            return config;
        }
        #endregion

        #region SarlaftExoneration
        public static IMapper CreateMapperSarlaftExopneration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompanySarlaftExoneration, models.SarlaftExoneration>();
                cfg.CreateMap<models.SarlaftExoneration, CompanySarlaftExoneration>();
                cfg.CreateMap<CompanyExonerationType, models.ExonerationType>();
                cfg.CreateMap<models.ExonerationType, CompanyExonerationType>();
            });
            return config.CreateMapper();
        }
        #endregion

        #region AssociationType
        public static IMapper CreateMapperAssociationType()
        {
            var config = MapperCache.GetMapper<CompanyAssociationType, models.AssociationType>(cfg =>
            {
                cfg.CreateMap<CompanyAssociationType, models.AssociationType>();
                cfg.CreateMap<models.AssociationType, CompanyAssociationType>();
            });
            return config;
        }
        #endregion

        #region CompanyType
        public static IMapper CreateMapperCompanyType()
        {
            var config = MapperCache.GetMapper<CompanyCompanyType, models.CompanyType>(cfg =>
            {
                cfg.CreateMap<CompanyCompanyType, models.CompanyType>();
                cfg.CreateMap<models.CompanyType, CompanyCompanyType>();
            });
            return config;
        }
        #endregion


        #region CompanyFiscalResponsibility
        public static IMapper CreateMapperCompanyFiscalResponsibility()
        {
            var config = MapperCache.GetMapper<CompanyFiscalResponsibility, models.FiscalResponsibility>(cfg =>
            {
                cfg.CreateMap<CompanyFiscalResponsibility, models.FiscalResponsibility>();
                cfg.CreateMap<models.FiscalResponsibility, CompanyFiscalResponsibility>();
            });
            return config;
        }

        public static IMapper CreateMapperCompanyInsuredFiscalResponsibility()
        {
            var config = MapperCache.GetMapper<CompanyInsuredFiscalResponsibility, models.InsuredFiscalResponsibility>(cfg =>
            {
                cfg.CreateMap<CompanyInsuredFiscalResponsibility, models.InsuredFiscalResponsibility>();
                cfg.CreateMap<models.InsuredFiscalResponsibility, CompanyInsuredFiscalResponsibility>();
            });
            return config;
        }
        #endregion CompanyFiscalResponsibility

        #region Insured
        public static IMapper CreateMapperInsuredDeclinedType()
        {
            var config = MapperCache.GetMapper<CompanyInsuredDeclinedType, models.InsuredDeclinedType>(cfg =>
            {
                cfg.CreateMap<CompanyInsuredDeclinedType, models.InsuredDeclinedType>();
                cfg.CreateMap<models.InsuredDeclinedType, CompanyInsuredDeclinedType>();
            });
            return config;
        }

        public static IMapper CreateMapperInsuredSegment()
        {
            var config = MapperCache.GetMapper<CompanyInsuredSegment, models.InsuredSegment>(cfg =>
            {
                cfg.CreateMap<CompanyInsuredSegment, models.InsuredSegment>();
                cfg.CreateMap<models.InsuredSegment, CompanyInsuredSegment>();
            });
            return config;
        }

        public static IMapper CreateMapperInsuredProfile()
        {
            var config = MapperCache.GetMapper<CompanyInsuredProfile, models.InsuredProfile>(cfg =>
            {
                cfg.CreateMap<CompanyInsuredProfile, models.InsuredProfile>();
                cfg.CreateMap<models.InsuredProfile, CompanyInsuredProfile>();
            });
            return config;
        }

        #endregion

        #region AgentType 
        public static IMapper CreateMapperAgentType()
        {
            var config = MapperCache.GetMapper<CompanyAgentType, models.AgentType>(cfg =>
            {
                cfg.CreateMap<CompanyAgentType, models.AgentType>();
                cfg.CreateMap<models.AgentType, CompanyAgentType>();
            });
            return config;
        }
        #endregion

        #region AgentDeclinedType 
        public static IMapper CreateMapperAgentDeclinedType()
        {
            var config = MapperCache.GetMapper<CompanyAgentDeclinedType, models.AgentDeclinedType>(cfg =>
            {
                cfg.CreateMap<CompanyAgentDeclinedType, models.AgentDeclinedType>();
                cfg.CreateMap<models.AgentDeclinedType, CompanyAgentDeclinedType>();
            });
            return config;
        }
        #endregion

        #region GroupAgent 
        public static IMapper CreateMapperGroupAgent()
        {
            var config = MapperCache.GetMapper<CompanyGroupAgent, models.GroupAgent>(cfg =>
            {
                cfg.CreateMap<CompanyGroupAgent, models.GroupAgent>();
                cfg.CreateMap<models.GroupAgent, CompanyGroupAgent>();
            });
            return config;
        }
        #endregion

        #region SalesChannel 
        public static IMapper CreateMapperSalesChannel()
        {
            var config = MapperCache.GetMapper<CompanySalesChannel, models.SalesChannel>(cfg =>
            {
                cfg.CreateMap<CompanySalesChannel, models.SalesChannel>();
                cfg.CreateMap<models.SalesChannel, CompanySalesChannel>();
            });
            return config;
        }
        #endregion

        #region EmployeePerson 
        public static IMapper CreateMapperEmployeePerson()
        {
            var config = MapperCache.GetMapper<CompanyEmployeePerson, models.EmployeePerson>(cfg =>
            {

                cfg.CreateMap<CompanyEmployeePerson, models.EmployeePerson>();
                cfg.CreateMap<models.EmployeePerson, CompanyEmployeePerson>();
            });
            return config;
        }
        #endregion

        #region AllOthersDeclinedType 
        public static IMapper CreateMapperAllOthersDeclinedType()
        {
            var config = MapperCache.GetMapper<CompanyAllOthersDeclinedType, models.AllOthersDeclinedType>(cfg =>
            {
                cfg.CreateMap<CompanyAllOthersDeclinedType, models.AllOthersDeclinedType>();
                cfg.CreateMap<models.AllOthersDeclinedType, CompanyAllOthersDeclinedType>();
            });
            return config;
        }
        #endregion

        #region CompanyInsuredGuarantee
        public static IMapper CreateCompanyInsuredGuarantee()
        {
            var config = MapperCache.GetMapper<CompanyInsuredGuaranteeDocumentation, models.InsuredGuaranteeDocumentation>(cfg =>
            {
                cfg.CreateMap<CompanyInsuredGuaranteeDocumentation, models.InsuredGuaranteeDocumentation>();
                cfg.CreateMap<models.InsuredGuaranteeDocumentation, CompanyInsuredGuaranteeDocumentation>();
            });
            return config;
        }
        #endregion

        #region GuaranteeRequiredDocument
        public static IMapper CreateGuaranteeRequiredDocument()
        {
            var config = MapperCache.GetMapper<CompanyGuaranteeRequiredDocument, models.GuaranteeRequiredDocument>(cfg =>
            {
                cfg.CreateMap<CompanyGuaranteeRequiredDocument, models.GuaranteeRequiredDocument>();
                cfg.CreateMap<models.GuaranteeRequiredDocument, CompanyGuaranteeRequiredDocument>();
            });
            return config;
        }
        #endregion

        public static Models.CompanyInsuredMain CreateCompanyInsuredMain(Sistran.Core.Application.UniquePersonService.V1.Models.Base.BaseInsuredMain coreInsuredMain)
        {
            CompanyInsuredMain companyInsurerMain = new CompanyInsuredMain();
            var imapper = CreateMapInsuredMain();
            return imapper.Map<Sistran.Core.Application.UniquePersonService.V1.Models.Base.BaseInsuredMain, CompanyInsuredMain>(coreInsuredMain);
        }

        #region InsuredMain
        public static IMapper CreateMapInsuredMain()
        {
            var config = MapperCache.GetMapper<UPModel.BaseInsuredMain, CompanyInsuredMain>(cfg =>
            {
                cfg.CreateMap<UPModel.BaseInsuredMain, CompanyInsuredMain>();
            });
            return config;
        }
        #endregion InsuredMain
        #region CiaDocumentsTypeRange
        /// <summary>
        /// Convierte entidad a modelo del servicio
        /// </summary>
        /// <param name="DocumentsTypeRange"></param>
        /// <returns></returns>
        public static Models.CiaDocumentTypeRange CreateCiaDocumentTypeRange(CiaDocumentsTypeRange ciaDocumentTypeRange)
        {
            return new Models.CiaDocumentTypeRange
            {
                IndividualTypeId = ciaDocumentTypeRange.IndividualTypeCode,
                DocumentTypeRange = ciaDocumentTypeRange.DocumentsTypeRangeCode
            };
        }


        #endregion

        #region ThirdPerson
        public static List<CompanyThirdDeclinedType> GetAllThirdDeclinedTypes(BusinessCollection declinedTypes)
        {
            List<CompanyThirdDeclinedType> declinedTypesModel = new List<CompanyThirdDeclinedType>();
            foreach (DeclinedType item in declinedTypes)
            {
                declinedTypesModel.Add(new CompanyThirdDeclinedType()
                {
                    Description = item.Description,
                    SmalDescription = item.SmallDescription,
                    Id = item.DeclinedTypeCode
                });
            }
            return declinedTypesModel;
        }

        /// <summary>
        /// Creates the third.
        /// </summary>       
        public static Models.CompanyThird CreateThird(ThirdParty third)
        {
            var ModelThird = new Models.CompanyThird
            {

                Id = third.ThirdPartyCode,
                EnteredDate = third.EnteredDate,
                DeclinedDate = third.DeclinedDate,
                IndividualId = (int)third.IndividualId,
                ModificationDate = third.ModificationDate,
                Annotation = third.Annotation,

            };

            if (third.DeclinedTypeCode != null)
            {
                ModelThird.DeclinedTypeId = (int)third.DeclinedTypeCode;
            }

            return ModelThird;

        }

        ///Recibe businessCollection
        /// <summary>
        /// Creates the provider.
        /// </summary>       
        public static CompanyThird CreateEntityThird(ThirdParty third)
        {
            var ModelThird = new Models.CompanyThird
            {

                Id = third.ThirdPartyCode,
                EnteredDate = third.EnteredDate,
                DeclinedDate = third.DeclinedDate,
                IndividualId = (int)third.IndividualId,
                ModificationDate = third.ModificationDate,
                Annotation = third.Annotation,

            };

            if (third.DeclinedTypeCode != null)
            {
                ModelThird.DeclinedTypeId = third.DeclinedTypeCode;
            }
            return ModelThird;

        }

        /// <summary>
        /// Creates the providers.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<CompanyThird> CreateThirsParty(BusinessCollection businessCollection)
        {
            List<CompanyThird> provider = new List<CompanyThird>();

            foreach (ThirdParty field in businessCollection)
            {
                provider.Add(ModelAssembler.CreateEntityThird(field));
            }

            return provider;
        }
        #endregion
        #region individualSarlaft v1
        public static Models.IndividualSarlaft CreateIndividualSarlaftDTOs(IndividualSarlaftDTO individualSarlaftDto)
        {
            Models.IndividualSarlaft individualSarlaf = new Models.IndividualSarlaft();
            individualSarlaf.BranchCode = individualSarlaftDto.BranchCode;
            individualSarlaf.FormNum = individualSarlaftDto.FormNum;
            individualSarlaf.Id = individualSarlaftDto.Id;
            individualSarlaf.IndividualId = individualSarlaftDto.IndividualId;
            individualSarlaf.InternationalOperations = individualSarlaftDto.InternationalOperations;
            individualSarlaf.InterviewResultCode = individualSarlaftDto.InterviewResultCode;
            individualSarlaf.InterviewerName = individualSarlaftDto.InterviewerName;
            individualSarlaf.InterviewerPlace = individualSarlaftDto.InterviewerPlace;
            individualSarlaf.PendingEvents = individualSarlaftDto.PendingEvents;
            individualSarlaf.RegistrationDate = individualSarlaftDto.RegistrationDate;
            individualSarlaf.VerifyingEmployee = individualSarlaftDto.VerifyingEmployee;
            individualSarlaf.Year = individualSarlaftDto.Year;
            individualSarlaf.EconomicActivity = new models.EconomicActivity { Id = individualSarlaftDto.ActivityEconomic };
            individualSarlaf.AuthorizedBy = individualSarlaftDto.AuthorizedBy;
            individualSarlaf.UserId = individualSarlaftDto.UserId;
            individualSarlaf.FinancialSarlaft = new FinancialSarlaf
            {
                AssetsAmount = individualSarlaftDto.finacialSarlaft.AssetsAmount,
                ExpenseAmount = individualSarlaftDto.finacialSarlaft.ExpenseAmount,
                ExtraIncomeAmount = individualSarlaftDto.finacialSarlaft.ExtraIncomeAmount,
                IncomeAmount = individualSarlaftDto.finacialSarlaft.IncomeAmount,
                IsForeignTransaction = individualSarlaftDto.finacialSarlaft.IsForeignTransaction,
                ForeignTransactionAmount = individualSarlaftDto.finacialSarlaft.ForeignTransactionAmount,
                LiabilitiesAmount = individualSarlaftDto.finacialSarlaft.LiabilitiesAmount,
                SarlaftId = individualSarlaftDto.finacialSarlaft.SarlaftId,
                Description = individualSarlaftDto.finacialSarlaft.Description,
            };

            return individualSarlaf;
        }
        public static List<Models.IndividualSarlaft> CreateIndividualSarlaftsDTO(List<IndividualSarlaftDTO> individualSarlaftDto)
        {
            List<Models.IndividualSarlaft> individualSarlaft = new List<Models.IndividualSarlaft>();
            foreach (var item in individualSarlaftDto)
            {
                individualSarlaft.Add(CreateIndividualSarlaftDTOs(item));
            }

            return individualSarlaft;
        }
        #endregion

        #region individualSarlaft v1
        public static IndividualSarlaftDTO CreateIndividualSarlaft(Models.IndividualSarlaft individualSarlaft)
        {
            IndividualSarlaftDTO individualSarlaftDTO = new IndividualSarlaftDTO();
            individualSarlaftDTO.BranchCode = individualSarlaft.BranchCode;
            individualSarlaftDTO.FormNum = individualSarlaft.FormNum;
            individualSarlaftDTO.Id = individualSarlaft.Id;
            individualSarlaftDTO.IndividualId = individualSarlaft.IndividualId;
            individualSarlaftDTO.InternationalOperations = individualSarlaft.InternationalOperations;
            individualSarlaftDTO.InterviewResultCode = individualSarlaft.InterviewResultCode;
            individualSarlaftDTO.InterviewerName = individualSarlaft.InterviewerName;
            individualSarlaftDTO.InterviewerPlace = individualSarlaft.InterviewerPlace;
            individualSarlaftDTO.PendingEvents = individualSarlaft.PendingEvents;
            individualSarlaftDTO.RegistrationDate = individualSarlaft.RegistrationDate;
            individualSarlaftDTO.VerifyingEmployee = individualSarlaft.VerifyingEmployee;
            individualSarlaftDTO.Year = individualSarlaft.Year;
            individualSarlaftDTO.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = 0, ErrorDescription = null };
            individualSarlaftDTO.StatusTypeService = StatusTypeService.Original;
            individualSarlaftDTO.finacialSarlaft = new SarlaftDTO
            {
                AssetsAmount = individualSarlaft.FinancialSarlaft.AssetsAmount,
                ExpenseAmount = individualSarlaft.FinancialSarlaft.ExpenseAmount,
                ExtraIncomeAmount = individualSarlaft.FinancialSarlaft.ExtraIncomeAmount,
                IncomeAmount = individualSarlaft.FinancialSarlaft.IncomeAmount,
                IsForeignTransaction = individualSarlaft.FinancialSarlaft.IsForeignTransaction,
                ForeignTransactionAmount = individualSarlaft.FinancialSarlaft.ForeignTransactionAmount,
                LiabilitiesAmount = individualSarlaft.FinancialSarlaft.LiabilitiesAmount,
                SarlaftId = individualSarlaft.FinancialSarlaft.SarlaftId,
                Description = individualSarlaft.FinancialSarlaft.Description,
            };

            return individualSarlaftDTO;
        }

        public static List<IndividualSarlaftDTO> CreateIndividualSarlafts(List<Models.IndividualSarlaft> individualSarlaft)
        {
            List<IndividualSarlaftDTO> individualSarlaftDto = new List<IndividualSarlaftDTO>();
            foreach (var item in individualSarlaft)
            {
                individualSarlaftDto.Add(CreateIndividualSarlaft(item));
            }

            return individualSarlaftDto;
        }
        #endregion

        #region DocumentsTypeRange
        /// <summary>
        /// Convierte entidad a modelo del servicio
        /// </summary>
        /// <param name="DocumentsTypeRange"></param>
        /// <returns></returns>
        public static Models.DocumentTypeRange CreateDocumentTypeRange(DocumentsTypeRange documentTypeRange)
        {
            CompanyDocumentTypeDAO DocumentTypeDAO = new CompanyDocumentTypeDAO();

            return new Models.DocumentTypeRange
            {
                Id = documentTypeRange.DocumentsTypeRangeCode,
                CardTypeCode = DocumentTypeDAO.GetDocumentTypeById(documentTypeRange.IdCardTypeCode),
                Gender = documentTypeRange.Gender,
                CardNumberFrom = documentTypeRange.IdCardNoFrom,
                CardNumberTo = documentTypeRange.IdCardNoTo
            };
        }

        public static List<Models.DocumentTypeRange> CreateDocumentsTypeRange(BusinessCollection businessCollection)
        {
            List<Models.DocumentTypeRange> DocumentTypeRange = new List<Models.DocumentTypeRange>();

            foreach (DocumentsTypeRange DocumentTypeRangeEntity in businessCollection)
            {
                DocumentTypeRange.Add(CreateDocumentTypeRange(DocumentTypeRangeEntity));
            }

            return DocumentTypeRange;
        }

        #endregion

        /// <summary>
        /// Creates the type of the document.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <returns></returns>
        public static Models.CompanyDocumentType CreateDocumentType(DocumentType documentType)
        {
            return new Models.CompanyDocumentType
            {
                Id = documentType.IdDocumentType,
                Description = documentType.Description,
                SmallDescription = documentType.SmallDescription
            };
        }


        public static Models.CompanyEmployee CreateEmployeeAssembler(Employee employee)
        {
            return new Models.CompanyEmployee
            {
                IndividualId = employee.IndividualId,
                BranchId = employee.BranchCode,
                EgressDate = employee.EgressDate,
                EntryDate = employee.EntryDate,
                FileNumber = employee.FileNumber
            };
        }

        public static List<CompanyEmployee> CreateEmployeeBusiness(BusinessCollection businessCollection)
        {
            List<CompanyEmployee> provider = new List<CompanyEmployee>();

            foreach (Employee field in businessCollection)
            {
                provider.Add(ModelAssembler.CreateEntityEmploye(field));
            }

            return provider;
        }

        public static CompanyEmployee CreateEntityEmploye(Employee employee)
        {

            CompanyEmployee companyEmployee = new CompanyEmployee();
            companyEmployee.BranchId = employee.BranchCode;
            companyEmployee.FileNumber = employee.FileNumber;
            companyEmployee.Annotation = employee.Annotation;
            companyEmployee.ModificationDate = employee.ModificationDate;
            companyEmployee.EgressDate = employee.EgressDate;
            companyEmployee.EntryDate = employee.EntryDate;
            companyEmployee.IndividualId = employee.IndividualId;
            companyEmployee.DeclinedTypeId = employee.DeclinedTypeCode == null ? 0 : (int)employee.DeclinedTypeCode;
            return companyEmployee;
        }

        public static IEnumerable<Models.CompanyCoClintonList> CreateCoClintonList(IEnumerable<UPENT.CoClintonList> entityCoClintonList)
        {
            return entityCoClintonList.Select(CreateCoClintonList);
        }

        public static Models.CompanyCoClintonList CreateCoClintonList(UPENT.CoClintonList entityCoClintonList)
        {
            return new CompanyCoClintonList
            {
                IdentificationTypeCode = entityCoClintonList.IdentificationTypeCode,
                IdentificationNro = entityCoClintonList.IdentificationNro,
                Descripction = entityCoClintonList.Descripction,
                CausalCode = entityCoClintonList.CausalCode,
                IncomeDate = entityCoClintonList.IncomeDate,
                ModifierUserCode = entityCoClintonList.ModifierUserCode,
                LastUpdateDate = entityCoClintonList.LastUpdateDate,
                HourLastUpdate = entityCoClintonList.HourLastUpdate
            };
        }

        public static IEnumerable<Models.CompanyCoOnuList> CreateCoOnuList(IEnumerable<UPENT.CoOnuList> entityCoOnuLists)
        {
            return entityCoOnuLists.Select(CreateCoOnuList);
        }

        public static Models.CompanyCoOnuList CreateCoOnuList(UPENT.CoOnuList entityCoOnuList)
        {
            return new CompanyCoOnuList
            {
                IdentificationNro = entityCoOnuList.IdentificationNro,
                Description = entityCoOnuList.Descripction
            };
        }

        public static IEnumerable<Models.CompanyCoOwnList> CreateCoOwnList(IEnumerable<UPENT.CoOwnList> entityCoOwnLists)
        {
            return entityCoOwnLists.Select(CreateCoOwnList);
        }

        public static Models.CompanyCoOwnList CreateCoOwnList(UPENT.CoOwnList entityCoOwnList)
        {
            return new CompanyCoOwnList
            {
                IdentificationNro = entityCoOwnList.IdentificationNro,
                Description = entityCoOwnList.Descripction
            };
        }

        #region BusinessName
        public static IMapper CreateMapperBusinessName()
        {
            var config = MapperCache.GetMapper<models.CompanyName, CompanyCoCompanyName>(cfg =>
            {
                //De model COMPANY a model CORE
                cfg.CreateMap<CompanyAddress, models.Address>();
                cfg.CreateMap<CompanyPhone, models.Phone>();
                cfg.CreateMap<CompanyEmail, models.Email>();
                //De CORE COMPANY 
                cfg.CreateMap<models.Address, CompanyAddress>();
                cfg.CreateMap<models.Phone, CompanyPhone>();
                cfg.CreateMap<models.Email, CompanyEmail>();
            });
            return config;
        }
        #endregion

        #region Politicas
        public static IMapper CreateMapperPersonOperation()
        {
            var config = MapperCache.GetMapper<CompanyPersonOperation, models.PersonOperation>(cfg =>
            {
                //De model COMPANY a model CORE
                cfg.CreateMap<CompanyPersonOperation, models.PersonOperation>();


                //De model CORE a model COMPANY 
                cfg.CreateMap<models.PersonOperation, CompanyPersonOperation>();

            });
            return config;
        }
        #endregion Politicas


        public static List<CompanyPerson> CreatePeople(List<ResponsePerson> persons)
        {
            List<CompanyPerson> peopleDTO = persons.Select(x => new CompanyPerson()
            {
                IndividualId = x.Id,
                FullName = x.FullName,
                IdentificationDocument = new CompanyIdentificationDocument() { Number = x.DocumentNumber },
                IndividualType = enums.IndividualType.Person,
                CustomerType = enums.CustomerType.Individual,
                Role = new CompanyRole() { Description = x.Role }
            }).ToList();

            return peopleDTO;
        }


        public static DTO.PersonDTO CreatePerson2G(ResponsePerson person)
        {
            DTO.PersonDTO companyPerson = new DTO.PersonDTO
            {
                Id = -1,
                FullName = person.FullName,
                Document = person.DocumentNumber,
                DocumentTypeId = person.IdentificationDocument.DocumentType.Id,
                Surname = person.Surname,
                SecondSurname = person.SecondSurname,
                CheckPayable = person.FullName,
                Names = person.Names,
                Gender = person.Gender,
                MaritalStatusId = person.MaritalStatusId,
                BirthDate = person.BirthDate,
                BirthPlace = person.BirthPlace,
                EconomicActivityId = person.EconomicActivityId != 0 ? person.EconomicActivityId : (int)Enums.PersonNaturalBasic.EconomicActivityId,
                IdentificationDocument = person.IdentificationDocument != null ? new DTO.IdentificationDocumentDTO
                {
                    DocumentType = new DTO.DocumentTypeDTO
                    {
                        Id = person.IdentificationDocument.DocumentType.Id,
                        SmallDescription = person.IdentificationDocument.DocumentType.SmallDescription,
                        Description = person.IdentificationDocument.DocumentType.Description,
                    }
                } : null,
                Addresses = person.Addresses != null && person.Addresses.Count > 0 ? person.Addresses.Select(x => new DTO.AddressDTO()
                {
                    Id = 0,
                    AddressTypeId = x.Id,
                    Description = x.Description,
                    CityId = x.CityId,
                    StateId = x.StateId,
                    CountryId = x.CountryId,
                    CountryDescription = x.CountryDescription,
                }).ToList() : null,
                Phones = person.Phones != null && person.Phones.Count > 0 ? person.Phones
                .Where(p => p.Id != (int)Enums.PhoneType.EMAIL).ToList().Select(x => new DTO.PhoneDTO()
                {
                    PhoneTypeId = x.Id,
                    SmallDescription = x.SmallDescription,
                    Description = x.Description != string.Empty ? x.Description : person.DocumentNumber
                }).ToList() : null,
                Emails = person.Phones != null && person.Phones.Count > 0 ? person.Phones
                .Where(p => p.Id == (int)Enums.PhoneType.EMAIL).ToList().Select(x => new DTO.EmailDTO()
                {
                    Description = x.Description,
                    EmailTypeId = x.Id
                }).ToList() : null,
            };

            if (companyPerson.Addresses == null || companyPerson.Addresses.Count == 0)
            {
                companyPerson.Addresses = new List<DTO.AddressDTO>();
                companyPerson.Addresses.Add(new DTO.AddressDTO
                {
                    AddressTypeId = (int)Enums.AddressBasicPerson.AddressTypeId,
                    Description = person.DocumentNumber.ToString(),
                    CityId = (int)Enums.AddressBasicPerson.AddressCityId,
                    StateId = (int)Enums.AddressBasicPerson.AddressStateId,
                    CountryId = (int)Enums.AddressBasicPerson.AddressCountryId,
                    CountryDescription = "COLOMBIA",
                });
            }
            companyPerson.Addresses.FirstOrDefault(x => x.IsPrincipal = true);
            if (companyPerson.Phones == null || companyPerson.Phones.Count == 0)
            {
                companyPerson.Phones = new List<DTO.PhoneDTO>();
                companyPerson.Phones.Add(new DTO.PhoneDTO
                {
                    Id = (int)Enums.PhoneBasicPerson.PhoneId,
                    Description = "0000000",
                    PhoneTypeId = (int)Enums.PhoneBasicPerson.PhoneTypeId
                });
            }
            companyPerson.Phones.FirstOrDefault(x => x.IsPrincipal = true);
            if (companyPerson.Emails == null || companyPerson.Emails.Count == 0)
            {
                companyPerson.Emails = new List<DTO.EmailDTO>();
                companyPerson.Emails.Add(new DTO.EmailDTO
                {
                    Id = (int)Enums.EmailBasicPerson.EmailId,
                    Description = person.DocumentNumber.ToString(),
                    EmailTypeId = (int)Enums.EmailBasicPerson.EmailTypeId
                });
            }
            companyPerson.Emails.FirstOrDefault(x => x.IsPrincipal = true);

            return companyPerson;
        }

        public static DTO.CompanyDTO CreateCompany2G(ResponseCompany company)
        {
            DTO.CompanyDTO companyCompany = new DTO.CompanyDTO
            {
                Id = (int)Enums.PersonLegalBasic.Id,
                BusinessName = company.FullName,
                CheckPayable = company.FullName,
                Document = company.DocumentNumber,
                DocumentTypeId = company.IdentificationDocument.DocumentType.Id,
                EconomicActivityId = company.EconomicActivityId != 0 ? company.EconomicActivityId : (int)Enums.PersonLegalBasic.EconomicActivityId,
                CompanyTypeId = company.CompanyType != 0 ? company.CompanyType : (int)Enums.PersonLegalBasic.CompanyTypeId,
                AssociationTypeId = company.AssociationType != 0 ? company.AssociationType : (int)Enums.PersonLegalBasic.AssociationTypeId,
                VerifyDigit = company.VerifyDigit,
                CountryOriginId = company.CountryId != 0 ? company.CountryId : (int)Enums.AddressBasicPerson.AddressCountryId,
                Addresses = company.Addresses != null && company.Addresses.Count > 0 ? company.Addresses.Select(x => new DTO.AddressDTO()
                {
                    Id = 0,
                    AddressTypeId = x.Id,
                    Description = x.Description,
                    CityId = x.CityId,
                    StateId = x.StateId,
                    CountryId = x.CountryId,
                    CountryDescription = x.CountryDescription,
                }).ToList() : null,
                Phones = company.Phones != null && company.Phones.Count > 0 ? company.Phones
                .Where(p => p.Id != (int)Enums.PhoneType.EMAIL).ToList().Select(x => new DTO.PhoneDTO()
                {
                    PhoneTypeId = x.Id,
                    SmallDescription = x.SmallDescription,
                    Description = x.Description != string.Empty ? x.Description : company.DocumentNumber
                }).ToList() : null,
                Emails = company.Phones != null && company.Phones.Count > 0 ? company.Phones
                .Where(p => p.Id == (int)Enums.PhoneType.EMAIL).ToList().Select(x => new DTO.EmailDTO()
                {
                    Description = x.Description,
                    EmailTypeId = x.Id
                }).ToList() : null,
            };

            if (companyCompany.Addresses == null || companyCompany.Addresses.Count == 0)
            {
                companyCompany.Addresses = new List<DTO.AddressDTO>();
                companyCompany.Addresses.Add(new DTO.AddressDTO
                {
                    AddressTypeId = (int)Enums.AddressBasicPerson.AddressTypeId,
                    Description = company.DocumentNumber.ToString(),
                    CityId = (int)Enums.AddressBasicPerson.AddressCityId,
                    StateId = (int)Enums.AddressBasicPerson.AddressStateId,
                    CountryId = (int)Enums.AddressBasicPerson.AddressCountryId,
                    CountryDescription = "COLOMBIA",
                });
            }
            companyCompany.Addresses.FirstOrDefault(x => x.IsPrincipal = true);

            if (companyCompany.Phones == null || companyCompany.Phones.Count == 0)
            {
                companyCompany.Phones = new List<DTO.PhoneDTO>();
                companyCompany.Phones.Add(new DTO.PhoneDTO
                {
                    Id = (int)Enums.PhoneBasicPerson.PhoneId,
                    Description = "0000000",
                    PhoneTypeId = (int)Enums.PhoneBasicPerson.PhoneTypeId
                });
            }
            companyCompany.Phones.FirstOrDefault(x => x.IsPrincipal = true);

            if (companyCompany.Emails == null || companyCompany.Emails.Count == 0)
            {
                companyCompany.Emails = new List<DTO.EmailDTO>();
                companyCompany.Emails.Add(new DTO.EmailDTO
                {
                    Id = (int)Enums.EmailBasicPerson.EmailId,
                    Description = company.DocumentNumber.ToString(),
                    EmailTypeId = (int)Enums.EmailBasicPerson.EmailTypeId
                });
            }
            companyCompany.Emails.FirstOrDefault(x => x.IsPrincipal = true);

            return companyCompany;
        }

        public static DTO.ProviderDTO CreateProvider2G(ResponseProvider provider, int individualId)
        {
            return new DTO.ProviderDTO
            {
                Id = provider.Id,
                IndividualId = individualId,
                ProviderTypeId = provider.ProviderTypeId,
                OriginTypeId = provider.OriginTypeId,
                CreationDate = provider.CreationDate,
                ModificationDate = provider.ModificationDate,
                Observation = provider.Observation,
                ProviderPaymentConcepts = provider.providerPaymentConcepts != null && provider.providerPaymentConcepts.Count > 0 ?
                provider.providerPaymentConcepts.Select(x => new DTO.ProviderPaymentConceptDTO()
                { Id = x.Id, Description = x.Description, PaymentConceptId = x.PaymentConceptId }).ToList() : null,
                SupplierProfileId = provider.SupplierProfileId
            };
        }

        public static List<DTO.BankTransfersDTO> CreateBankTransfersDTO(List<ResponseBankTransfer> responseBankTransfer, int individualId)
        {
            return responseBankTransfer.Select(x => new DTO.BankTransfersDTO()
            {
                Id = 0,
                IndividualId = individualId,
                BankId = x.BankId,
                BankBranch = x.BankBranch,
                BankSquare = x.BankSquare,
                AccountTypeId = x.AccountTypeId,
                CurrencyId = x.CurrencyId,
                PaymentBeneficiary = x.PaymentBeneficiary,
                AccountNumber = x.AccountNumber,
                ActiveAccount = x.ActiveAccount,
                DefaultAccount = x.DefaultAccount,
                BankDescription = x.BankDescription,
                CurrencyDescription = x.CurrencyDescription,
                InscriptionDate = x.InscriptionDate,
                IntermediaryBank = x.IntermediaryBank,
            }).ToList();
        }

        public static List<DTO.IndividualTaxExeptionDTO> CreateIndividualTaxExeptionDTO(List<ResponseTax> responseTaxes, int individualId)
        {
            return responseTaxes.Select(x => new DTO.IndividualTaxExeptionDTO()
            {
                CountryId = x.CountryId != 0 ? x.CountryId : (int)Enums.AddressBasicPerson.AddressCountryId,
                Datefrom = DateTime.Now,
                DateUntil = DateTime.Now.AddMonths(1),
                IndividualId = individualId,
                OfficialBulletinDate = DateTime.Now,
                StateCode = x.StateCode != 0 ? x.StateCode : (int)Enums.AddressBasicPerson.AddressStateId,
                TaxId = x.TaxId,
                TaxCategoryId = x.TaxCategoryId != 0 ? x.TaxCategoryId : (int)Enums.TaxBasic.CategoryId,
                TaxCondition = x.TaxCondition,
                RoleId = x.RoleId
            }).ToList();
        }
        public static IndividualControl CreateIndividualControl(INTEN.UpIndividualControl entityIndividualControl)
        {
            return new IndividualControl()
            {
                IndividualId = entityIndividualControl.IndividualId,
                Action = entityIndividualControl.Action
            };
        }

        public static CoIndividualControl CreateCoIndividualControl(INTEN.UpCoIndividualControl entityCoIndividualControl)
        {
            return new CoIndividualControl()
            {
                IndividualId = entityCoIndividualControl.IndividualId,
                PerifericoId = entityCoIndividualControl.PerifericoId


            };
        }

        public static EmployeeControl CreateEmployeeControl(INTEN.UpEmployeeControl entityEmployeeControl)
        {
            return new EmployeeControl()
            {
                IndividualId = entityEmployeeControl.IndividualId,
                Action = entityEmployeeControl.Action
            };
        }

        public static InsuranceCompanyControl CreateCoInsuranceCompanyControl(INTEN.UpInsuranceCompanyControl entityCoInsuranceCompanyControl)
        {
            return new InsuranceCompanyControl()
            {
                InsuranceCompanyId = entityCoInsuranceCompanyControl.InsuranceCompanyId,
                Action = entityCoInsuranceCompanyControl.Action
            };
        }


        public static InsuredControl CreateInsuredControl(INTEN.UpInsuredControl entityInsuredControl)
        {
            return new InsuredControl()
            {
                IndividualId = entityInsuredControl.IndividualId,
                InsuredCode = entityInsuredControl.InsuredCode,
                Action = entityInsuredControl.Action
            };
        }

        public static InsuredControl CreateInsuredControl(CompanyInsured modelCompanyInsured)
        {
            return new InsuredControl()
            {
                IndividualId = modelCompanyInsured.IndividualId,
                InsuredCode = modelCompanyInsured.InsuredCode
            };
        }

        public static AgentControl CreateAgentControl(INTEN.UpAgentControl entityCoTmpAgentControl)
        {
            return new AgentControl()
            {
                IndividualId = entityCoTmpAgentControl.IndividualId,
                Action = entityCoTmpAgentControl.Action
            };
        }

        public static CompanyReinsuranceControl CreateCompanyReinsuranceControl(INTEN.UpReinsuranceCompanyControl entityUpReinsuranceCompanyControl)
        {
            return new CompanyReinsuranceControl()
            {
                IndividualId = entityUpReinsuranceCompanyControl.IndividualId,
                Action = entityUpReinsuranceCompanyControl.Action
            };
        }

        public static SupplierControl CreateSupplierControl(INTEN.UpSupplierControl entityUpSupplierControl)
        {
            return new SupplierControl()
            {
                SupplierCode = entityUpSupplierControl.SupplierCode,
                Action = entityUpSupplierControl.Action
            };
        }
    }
}
