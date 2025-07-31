using AutoMapper;
using Sistran.Company.Application.UniquePerson.Entities;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Business;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UPE = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }


        /// <summary>
        /// Crear una nueva Persona
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public CompanyPerson CreateCompanyPerson(CompanyPerson person)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperPerson();
                var personCore = imapper.Map<CompanyPerson, Person>(person);
                var result = coreProvider.CreatePerson(personCore);
                return imapper.Map<Person, CompanyPerson>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCompany> GetCompanyCompanyByDocument(CustomerType customerType, string documentNumber)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperCompany();
                var coreCompany = coreProvider.GetCompanyByDocument(customerType, documentNumber);

                var result = imapper.Map<List<Core.Application.UniquePersonService.V1.Models.Company>, List<CompanyCompany>>(coreCompany);

                foreach (var company in result)
                {
                    var primaryKeyIndividualSarlaftExoneration = IndividualSarlaftExoneration.CreatePrimaryKey(company.IndividualId);
                    var individualSarlaftExoneration = (IndividualSarlaftExoneration)DataFacadeManager.GetObject(primaryKeyIndividualSarlaftExoneration);

                    company.Exoneration = individualSarlaftExoneration?.ExonerationTypeCode == null ? null : new CompanySarlaftExoneration
                    {
                        Id = individualSarlaftExoneration.IndividualId,
                        ExonerationType = new CompanyExonerationType
                        {
                            Id = (int)individualSarlaftExoneration.ExonerationTypeCode,
                            IndividualTypeCode = (int)company.IndividualType
                        },

                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyCompany CreateCompanyCompany(CompanyCompany company)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperCompany();
                var companyCore = imapper.Map<CompanyCompany, Core.Application.UniquePersonService.V1.Models.Company>(company);
                var coreCompany = coreProvider.CreateCompany(companyCore);
                var result = imapper.Map<Core.Application.UniquePersonService.V1.Models.Company, CompanyCompany>(coreCompany);

                if (company.Exoneration != null)
                {
                    company.Exoneration.Id = result.IndividualId;
                    company.Exoneration.EnteredDate = DateTime.Now;

                    var sarlaftExoneration = EntityAssembler.CreateSarlaftExoneration(company.Exoneration);
                    sarlaftExoneration = (IndividualSarlaftExoneration)DataFacadeManager.Insert(sarlaftExoneration);

                    result.Exoneration = sarlaftExoneration.ExonerationTypeCode == null ? null : new CompanySarlaftExoneration
                    {
                        Id = sarlaftExoneration.IndividualId,
                        ExonerationType = new CompanyExonerationType
                        {
                            Id = (int)sarlaftExoneration.ExonerationTypeCode,
                            IndividualTypeCode = (int)company.IndividualType
                        },

                    };
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyCompany UpdateCompanyCompany(CompanyCompany company)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperCompany();
                var companyCore = imapper.Map<CompanyCompany, Core.Application.UniquePersonService.V1.Models.Company>(company);
                var coreCompany = coreProvider.UpdateCompany(companyCore);
                var result = imapper.Map<Core.Application.UniquePersonService.V1.Models.Company, CompanyCompany>(coreCompany);
                if (company.Exoneration != null)
                {
                    company.Exoneration.Id = coreCompany.IndividualId;
                    company.Exoneration.EnteredDate = DateTime.Now;
                    var primaryKeyExoneration = IndividualSarlaftExoneration.CreatePrimaryKey(coreCompany.IndividualId);
                    var sarlaftExoneration = (IndividualSarlaftExoneration)DataFacadeManager.GetObject(primaryKeyExoneration);
                    if (sarlaftExoneration != null)
                    {
                        sarlaftExoneration.IsExonerated = company.Exoneration.IsExonerated;
                        sarlaftExoneration.RegistrationDate = company.Exoneration.EnteredDate;
                        sarlaftExoneration.RoleCode = company.Exoneration.RolId;
                        sarlaftExoneration.UserId = company.Exoneration.UserId;
                        sarlaftExoneration.ExonerationTypeCode = company.Exoneration.ExonerationType.Id;

                        DataFacadeManager.Update(sarlaftExoneration);
                    }
                    else
                    {
                        sarlaftExoneration = (IndividualSarlaftExoneration)DataFacadeManager.Insert(EntityAssembler.CreateSarlaftExoneration(company.Exoneration));
                    }
                    result.Exoneration = sarlaftExoneration?.ExonerationTypeCode == null ? null : new CompanySarlaftExoneration
                    {
                        Id = sarlaftExoneration.IndividualId,
                        ExonerationType = new CompanyExonerationType
                        {
                            Id = (int)sarlaftExoneration.ExonerationTypeCode,
                            IndividualTypeCode = (int)result.IndividualType
                        },

                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCompany> GetCompanyCompanyAdv(CustomerType customerType, CompanyCompany company)
        {
            try
            {
                var immaper = ModelAssembler.CreateMapperCompany();
                CompanyBusiness personBusiness = new CompanyBusiness();
                var coreCompany = immaper.Map<CompanyCompany, Sistran.Core.Application.UniquePersonService.V1.Models.Company>(company);
                var companyCore = coreProvider.GetCompanyAdv(customerType, coreCompany);
                var result = immaper.Map<List<Sistran.Core.Application.UniquePersonService.V1.Models.Company>, List<CompanyCompany>>(companyCore);

                foreach (var comp in result)
                {
                    var primaryKeyIndividualSarlaftExoneration = IndividualSarlaftExoneration.CreatePrimaryKey(comp.IndividualId);
                    var individualSarlaftExoneration = (IndividualSarlaftExoneration)DataFacadeManager.GetObject(primaryKeyIndividualSarlaftExoneration);

                    comp.Exoneration = individualSarlaftExoneration?.ExonerationTypeCode == null ? null : new CompanySarlaftExoneration
                    {
                        Id = individualSarlaftExoneration.IndividualId,
                        ExonerationType = new CompanyExonerationType
                        {
                            Id = (int)individualSarlaftExoneration.ExonerationTypeCode,
                            IndividualTypeCode = (int)comp.IndividualType
                        },

                    };
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener compañia por identificador
        /// </summary>
        /// <param name="individualId">identificador</param>
        /// <returns></returns>
        public new CompanyCompany GetCompanyByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PrimaryKey key = UPE.Company.CreatePrimaryKey(individualId);
            UPE.Company companyEntity = (UPE.Company)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            Models.CompanyCompany company = new Models.CompanyCompany();
            Models.CompanyAssociationType associationType = new Models.CompanyAssociationType();

            if (companyEntity != null)
            {
                company = ModelAssembler.CreateCompany(companyEntity);

                IndividualBusiness individualBss = new IndividualBusiness();
                Individual individualModel = new Individual();

                individualModel = individualBss.GetIndividualByIndividualId(company.IndividualId);

                var immaper = ModelAssembler.CreateMapperEconomicActivity();

                EconomyActivityBusiness economicActivityBss = new EconomyActivityBusiness();
                EconomicActivity result = economicActivityBss.GetEconomicActivitiesById(individualModel.EconomicActivity.Id);
                company.EconomicActivity = immaper.Map<EconomicActivity, CompanyEconomicActivity>(result);

                company.IndividualType = individualModel.IndividualType;
                company.CustomerType = individualModel.CustomerType;

                var imap = ModelAssembler.CreateMapperSarlaftExopneration();
                IndividualSarlaftExonerationBusiness individualSarlaftExonerationBss = new IndividualSarlaftExonerationBusiness();
                CompanySarlaftExoneration exoneration = individualSarlaftExonerationBss.GetSarlaftExonerationByIndividualId(company.IndividualId);
                company.Exoneration = exoneration;

                company.Exoneration = exoneration == null ? null : new CompanySarlaftExoneration
                {
                    Id = exoneration.Id,
                    ExonerationType = new CompanyExonerationType
                    {
                        Id = (int)exoneration.ExonerationType.Id,
                        IndividualTypeCode = (int)company.IndividualType
                    },
                };



                var imapper = ModelAssembler.CreateMapperAssociationType();
                AssociationTypeBusiness coAssociationTypeBss = new AssociationTypeBusiness();
                CompanyExtended coCompany = coAssociationTypeBss.GetAssociationTypeByIndividualId(company.IndividualId);
                company.AssociationType = new CompanyAssociationType();
                company.AssociationType.Id = coCompany.AssociationType.Id;
                company.AssociationType.Description = coCompany.AssociationType.Description;
                company.IdentificationDocument.NitAssociationType = coCompany.AssociationType.NitAssociationType;
                company.VerifyDigit = coCompany.VerifyDigit;


                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UPE.Insured.Properties.IndividualId, typeof(Insured).Name);
                filter.Equal();
                filter.Constant(individualId);

                UPE.Insured insured = (UPE.Insured)DataFacadeManager.Instance.GetDataFacade().List(typeof(UPE.Insured), filter.GetPredicate()).FirstOrDefault();
                if (insured != null)
                {
                    CompanyConsotuimBusiness consortiumBss = new CompanyConsotuimBusiness();
                    company.Consortiums = consortiumBss.GetCoConsortiumsByInsuredCode(insured.InsuredCode);
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UniquePersonServices.V1.Entities.DAOs.GetCompanyByIndividualId");

            return company;
        }

        public CompanyCompany UpdateCompanyCompanyBasicInfo(CompanyCompany company)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperCompany();
                var companyCore = imapper.Map<CompanyCompany, Core.Application.UniquePersonService.V1.Models.Company>(company);
                var coreCompany = coreProvider.UpdateCompanyBasicInfo(companyCore);
                var result = imapper.Map<Core.Application.UniquePersonService.V1.Models.Company, CompanyCompany>(coreCompany);

                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
