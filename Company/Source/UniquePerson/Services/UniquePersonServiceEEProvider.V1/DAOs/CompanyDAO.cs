using Sistran.Company.Application.UniquePersonServices.V1.DAOs;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.DAOs;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using entities = Sistran.Core.Application.UniquePersonV1.Entities;
using entityAssembler = Sistran.Core.Application.UniquePersonService.V1.Assemblers.EntityAssembler;
using EnumPerson = Sistran.Core.Application.UniquePersonService.V1.Enums;
using modelAssembler = Sistran.Core.Application.UniquePersonService.V1.Assemblers.ModelAssembler;
using models = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Company.Application.UniquePersonServices.V1.Entities.DAOs
{
    public class CompanyDAO : Sistran.Core.Application.UniquePersonService.V1.DAOs.CompanyDAO
    {
        /// <summary>
        /// Crear una nueva compañia
        /// </summary>
        /// <param name="company">Datos compañia</param>
        /// <returns></returns>
        public Models.CompanyCompany CreateCompany(Models.CompanyCompany company)
        {
            return null;
        }

        /// <summary>
        /// Actualizar Compañia
        /// </summary>
        /// <param name="company">Compañia</param>
        /// <returns></returns>
        public Models.CompanyCompany UpdateCompany(Models.CompanyCompany company)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PhoneDAO phoneDAO = new PhoneDAO();
            EmailDAO emailDAO = new EmailDAO();
            PartnerDAO partnerDAO = new PartnerDAO();
            AddressDAO addressDAO = new AddressDAO();
            ConsortiumDAO consortiumDAO = new ConsortiumDAO();
            IndividualSarlaftDAO individualSarlaftDAO = new IndividualSarlaftDAO();
            IndividualSarlaftExonerationDAO individualSarlaftExonerationDAO = new IndividualSarlaftExonerationDAO();
            LegalRepresentativeDAO legalRepresentativeDAO = new LegalRepresentativeDAO();
            PaymentMethodAccountDAO paymentMethodAccountDAO = new PaymentMethodAccountDAO();
            //PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
            CompanyNameDAO coCompanyNameDAO = new CompanyNameDAO();

            PrimaryKey key = entities.Company.CreatePrimaryKey(company.IndividualId);
            entities.Company companyEntity = (entities.Company)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (companyEntity != null)
            {
                companyEntity.EconomicActivityCode = company.EconomicActivity.Id;
                companyEntity.TradeName = company.FullName;
                companyEntity.TributaryIdTypeCode = company.IdentificationDocument.DocumentType.Id;
                companyEntity.TributaryIdNo = company.IdentificationDocument.Number;
                companyEntity.CountryCode = company.CountryId;
                companyEntity.CompanyTypeCode = company.CompanyType.Id;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(companyEntity);

                
                if (company.Consortiums != null)
                {
                    ObjectCriteriaBuilder filterInsured = new ObjectCriteriaBuilder();
                    filterInsured.Property(entities.Insured.Properties.IndividualId, typeof(entities.Insured).Name);
                    filterInsured.Equal();
                    filterInsured.Constant(companyEntity.IndividualId);
                    entities.Insured insuredEntity = (entities.Insured)DataFacadeManager.Instance.GetDataFacade().List(typeof(entities.Insured), filterInsured.GetPredicate()).FirstOrDefault();
                    models.Insured insuredModel = null;
                    if (insuredEntity == null)
                    {
                        insuredEntity = InsuredModelByIndividualId(companyEntity.IndividualId, company.UserBranch.Id);
                        insuredModel = modelAssembler.CreateInsured(insuredEntity);
                        //insuredModel.InsuredConcept = new models.InsuredConcept() { InsuredCode = insuredModel.InsuredId, IsInsured = true, IsBeneficiary = false, IsHolder = false, IsPayer = false };
                        //InsuredDAO insuredDAO = new InsuredDAO();
                        //insuredModel = insuredDAO.CreateInsured(insuredModel);
                    }
                    DataFacadeManager.Instance.GetDataFacade().ClearObjectCache();
                    key = entities.Insured.CreatePrimaryKey(insuredEntity.IndividualId);
                    insuredEntity = (entities.Insured)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                    consortiumDAO.DeleteConsortiumByInsuredCode(company.Consortiums[0].InsuredCode);
                    foreach (Models.CompanyConsortium consortium in company.Consortiums)
                    {
                        consortium.InsuredCode = insuredEntity.InsuredCode;
                        consortiumDAO.CreateConsortium(consortium);
                    }
                }

                

                if (company.Sarlafts != null)
                {
                    foreach (Models.IndividualSarlaft sf in company.Sarlafts)
                    {
                        if (sf.FinancialSarlaft.SarlaftId != 0)
                        {
                            individualSarlaftDAO.UpdateIndividualSarlaft(sf);
                        }
                        else
                        {
                            individualSarlaftDAO.CreateIndividualSarlaft(sf, company.IndividualId, company.EconomicActivity.Id);
                        }
                    }
                }

                

                if (company.Exoneration != null) 
                {
                    individualSarlaftExonerationDAO.UpdateSarlaftExoneration(company.Exoneration, company.IndividualId);
                }

            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UniquePersonServices.V1.Entities.DAOs.UpdateCompany");

            return GetCompanyByIndividualId(companyEntity.IndividualId);

        }
        /// <summary>
        /// Buscar compañias y prospectos por número de documento o por razón social
        /// </summary>
        /// <param name="filter">Filtro de la busqueda</param>
        /// <param name="searchType">Tipo de busqueda
        /// 2. Compañia
        /// 3. Prospecto
        /// 4. Todos</param>
        /// <returns></returns>
        public new List<Models.CompanyCompany> GetCompaniesByDocumentNumberNameSearchType(string documentNumber, string name, int searchType)
        {
            return null;
        }

        /// <summary>
        /// Obtener compañia por identificador
        /// </summary>
        /// <param name="individualId">identificador</param>
        /// <returns></returns>
        public new Models.CompanyCompany GetCompanyByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PrimaryKey key = entities.Company.CreatePrimaryKey(individualId);
            entities.Company companyEntity = (entities.Company)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            Models.CompanyCompany company = new Models.CompanyCompany();
            Models.CompanyAssociationType associationType = new Models.CompanyAssociationType();

            if (companyEntity != null)
            {
                company = ModelAssembler.CreateCompany(companyEntity);

                IndividualDAO individualDAO = new IndividualDAO();
                models.Individual individualModel = new models.Individual();           
                
                individualModel = individualDAO.GetIndividualByIndividualId(company.IndividualId);

                var immaper = ModelAssembler.CreateMapperEconomicActivity();

                EconomicActivityDAO economicActivityDAO = new EconomicActivityDAO();
                EconomicActivity result = economicActivityDAO.GetEconomicActivitiesByEconomicActiviti(individualModel.EconomicActivity.Id);
                company.EconomicActivity = immaper.Map<EconomicActivity,CompanyEconomicActivity>(result);

                company.IndividualType = individualModel.IndividualType;
                company.CustomerType = individualModel.CustomerType;

                var imap = ModelAssembler.CreateMapperSarlaftExopneration();
                IndividualSarlaftExonerationDAO individualSarlaftExonerationDAO = new IndividualSarlaftExonerationDAO();
                var exoneration = individualSarlaftExonerationDAO.GetSarlaftExonerationByIndividualId(company.IndividualId);
                company.Exoneration = exoneration;
                //company.Exoneration = imap.Map<SarlaftExoneration, CompanySarlaftExoneration>(exoneration);
                //company.Exoneration.ExonerationType = imap.Map<ExonerationType, CompanyExonerationType>(exoneration.ExonerationType); 

                //company.Exoneration.ExonerationType = new CompanyExonerationType();
                //company.Exoneration.ExonerationType.Id = (int)exoneration.ExonerationType.Id;
                //company.Exoneration.ExonerationType.IndividualTypeCode = (int)company.IndividualType;
                //company.Exoneration = /*exoneration == null ? null : */new CompanySarlaftExoneration
                //{
                //    Id = exoneration.Id,
                //    ExonerationType = new CompanyExonerationType
                //    {
                //        Id = (int)exoneration.ExonerationType.Id,
                //        IndividualTypeCode = (int)company.IndividualType
                //    },
                //};



                var imapper = ModelAssembler.CreateMapperAssociationType();
                CoAssociationTypeDAO coAssociationTypeDAO = new CoAssociationTypeDAO();
                CompanyExtended coCompany = coAssociationTypeDAO.GetAssociationTypeByIndividualId(company.IndividualId);
                company.AssociationType = new CompanyAssociationType();
                company.AssociationType.Id = coCompany.AssociationType.Id;
                company.AssociationType.Description = coCompany.AssociationType.Description;


                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(entities.Insured.Properties.IndividualId, typeof(entities.Insured).Name);
                filter.Equal();
                filter.Constant(individualId);

                entities.Insured insured = (entities.Insured)DataFacadeManager.Instance.GetDataFacade().List(typeof(entities.Insured), filter.GetPredicate()).FirstOrDefault();
                if (insured != null)
                {
                    ConsortiumDAO consortiumDAO = new ConsortiumDAO();
                    company.Consortiums = consortiumDAO.GetCoConsortiumsByInsuredCode(insured.InsuredCode);
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UniquePersonServices.V1.Entities.DAOs.GetCompanyByIndividualId");

            return company;
        }
        /// <summary>
        /// Obtener Asegurado por Id
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        private entities.Insured InsuredModelByIndividualId(int individualId, int userBranchId)
        {
            return new entities.Insured()
            {
                IndividualId = individualId,
                InsuredCode = 0,
                CheckPayableTo = "",
                BranchCode = userBranchId,
                InsDeclinedTypeCode = null,
                Annotations = "",
                InsSegmentCode = (int)RolesType.Insured,
                EnteredDate = DelegateService.commonService.GetDate(),
                DeclinedDate = null,
                InsProfileCode = (int)RolesType.Insured,
                ModifyDate = null
            };
        }

        /// <summary>
        /// Buscar empresa por identificación
        /// </summary>
        /// <param name="company">datos empresa</param>
        /// <returns></returns>
        public Models.CompanyCompany GetCompanyByIdentification(int identificationTypeCode, string identificationNumber)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Sistran.Core.Application.UniquePersonV1.Entities.Company.Properties.TributaryIdTypeCode);
            filter.Equal();
            filter.Constant(identificationTypeCode);
            filter.And();
            filter.Property(Sistran.Core.Application.UniquePersonV1.Entities.Company.Properties.TributaryIdNo);
            filter.Equal();
            filter.Constant(identificationNumber);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                                                                                     .SelectObjects(typeof(Sistran.Core.Application.UniquePersonV1.Entities.Company), filter.GetPredicate()));

            if (businessCollection != null && businessCollection.Count > 0)
            {
                Sistran.Core.Application.UniquePersonV1.Entities.Company companyEntity = (Sistran.Core.Application.UniquePersonV1.Entities.Company)(businessCollection[0]);
                return ModelAssembler.CreateCompany(companyEntity);
            }

            return null;
        }

        /// <summary>
        /// Obtener compañia por numero de documento
        /// </summary>
        /// <param name="individualCode">numero de documento</param>
        /// <returns></returns>
        public Models.CompanyCompany GetCompanyByDocumentNumber(string documentNumber)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Models.CompanyCompany company = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(entities.Company.Properties.TributaryIdNo, typeof(entities.Company).Name);
            filter.Equal();
            filter.Constant(documentNumber);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(entities.Company), filter.GetPredicate()));
            if (businessCollection.Count > 0)
            {
                company = ModelAssembler.CreateCompanies(businessCollection).FirstOrDefault();

                PaymentMethodAccountDAO paymentMethodAccountDAO = new PaymentMethodAccountDAO();


                IndividualDAO individualDAO = new IndividualDAO();
                models.Individual individualModel = new models.Individual();
                individualModel = individualDAO.GetIndividualByIndividualId(company.IndividualId);

                EconomicActivityDAO economicActivityDAO = new EconomicActivityDAO();
                //company.EconomicActivity = economicActivityDAO.GetEconomicActivitiesByEconomicActiviti(individualModel.EconomicActivity.Id);
                company.IndividualType = individualModel.IndividualType;
                company.CustomerType = individualModel.CustomerType;

                //PaymentMethodDAO paymentMethodDao = new PaymentMethodDAO();

            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetCompanyByDocumentNumber");
            return company;
        }
    }
}
