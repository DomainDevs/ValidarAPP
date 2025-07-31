using Sistran.Core.Application.UniquePersonService.V1.Enums;
using MOUP = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.Common.Entities;
using System.Linq;
using System;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class CompanyBusiness
    {
        public List<MOUP.Company> GetCompanyByDocument(string documentNumber)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.Property(Company.Properties.TributaryIdNo, typeof(Company).Name);
            filter.Equal();
            filter.Constant(documentNumber);
            var CompanyCollection = DataFacadeManager.GetObjects(typeof(Company), filter.GetPredicate());

            var companies = ModelAssembler.CreateCompanies(CompanyCollection);

            foreach (var company in companies)
            {
                var primaryKeyCoCompany = CoCompany.CreatePrimaryKey(company.IndividualId);
                var entityCoCompany = (CoCompany)DataFacadeManager.GetObject(primaryKeyCoCompany);
                company.AssociationType = new MOUP.AssociationType { Id = entityCoCompany.AssociationTypeCode };
                company.VerifyDigit = entityCoCompany.VerifyDigit;


                //var primaryKeyIndividualSarlaftExoneration = IndividualSarlaftExoneration.CreatePrimaryKey(company.IndividualId);
                //var entityExoneration = (IndividualSarlaftExoneration)DataFacadeManager.GetObject(primaryKeyIndividualSarlaftExoneration);

                //company.ExonerationType = new MOUP.ExonerationType
                //{
                //    Id = entityExoneration.ExonerationTypeCode == null ? 0 : (int)entityExoneration.ExonerationTypeCode,
                //    IndividualTypeCode = (int)company.IndividualType,
                //};

            }
            return companies;
        }

        public List<MOUP.Company> GetCompanyAdv(MOUP.Company Company)
        {
            bool useAdd = false;
            var filter = new ObjectCriteriaBuilder();
            if (!string.IsNullOrEmpty(Company.IdentificationDocument.Number))
            {
                filter.Property(UniquePersonV1.Entities.Company.Properties.TributaryIdNo, typeof(Company).Name);
                filter.Equal();
                filter.Constant(Company.IdentificationDocument.Number);
                useAdd = true;
            }
            if (!string.IsNullOrEmpty(Company.FullName))
            {
                if (useAdd)
                {
                    filter.And();
                }
                filter.Property(UniquePersonV1.Entities.Company.Properties.TradeName, typeof(UniquePersonV1.Entities.Company).Name);
                filter.Like();
                filter.Constant(Company.FullName + "%");
                useAdd = true;
            }
            if (!string.IsNullOrEmpty(Company.IndividualId.ToString()) && Company.IndividualId > 0)
            {
                if (useAdd)
                {
                    filter.And();
                }
                filter.Property(UniquePersonV1.Entities.Company.Properties.IndividualId, typeof(UniquePersonV1.Entities.Company).Name);
                filter.Equal();
                filter.Constant(Company.IndividualId);
                useAdd = true;
            }

            var CompanyCollection = DataFacadeManager.GetObjects(typeof(Company), filter.GetPredicate());
            var companies = ModelAssembler.CreateCompanies(CompanyCollection);

            foreach (var company in companies)
            {
                var primaryKeyCoCompany = CoCompany.CreatePrimaryKey(company.IndividualId);
                var entityCoCompany = (CoCompany)DataFacadeManager.GetObject(primaryKeyCoCompany);
                company.AssociationType = new MOUP.AssociationType { Id = entityCoCompany.AssociationTypeCode };
                company.VerifyDigit = entityCoCompany.VerifyDigit;
            }
            return companies;
        }

        public MOUP.Company CreateCompany(MOUP.Company company)
        {
            company.IndividualType = IndividualType.Company;
            var entityCompany = EntityAssembler.CreateCompany(company);
            var entityCoCompany = EntityAssembler.CreateCoCompany(company);

            entityCompany = (Company)DataFacadeManager.Insert(entityCompany);
            entityCoCompany.IndividualId = entityCompany.IndividualId;

            DataFacadeManager.Insert(entityCoCompany);

            var result = ModelAssembler.CreateCompany(entityCompany);
            result.AssociationType = new MOUP.AssociationType { Id = entityCoCompany.AssociationTypeCode };
            result.VerifyDigit = entityCoCompany.VerifyDigit;
            
            if (company.Insured != null)
            {
                company.Insured.IndividualId = entityCompany.IndividualId;
                result.Insured = new MOUP.Insured
                {
                    IndividualId = entityCompany.IndividualId
                };
                InsuredBusiness insuredBusiness = new InsuredBusiness();
                company.Insured = insuredBusiness.CreateInsured(company.Insured);
                result.Insured = company.Insured;
                if (company.Consortiums != null)
                {
                    company.Consortiums.ForEach(c => c.InsuredCode = company.Insured.InsuredCode);
                    ConsortiumBusiness consortiumBusiness = new ConsortiumBusiness();
                    company.Consortiums = consortiumBusiness.CreateConsortiums(company.Consortiums);
                    result.Consortiums = company.Consortiums;

                }
            }

            return result;
        }

        public MOUP.Company UpdateCompany(MOUP.Company company)
        {
            var primaryKeyCompany = Company.CreatePrimaryKey(company.IndividualId);
            var entityCompany = (Company)DataFacadeManager.GetObject(primaryKeyCompany);

            entityCompany.IndividualTypeCode = (int)IndividualType.Company;
            entityCompany.EconomicActivityCode = company.EconomicActivity.Id;
            entityCompany.TradeName = company.FullName;
            entityCompany.TributaryIdTypeCode = company.IdentificationDocument.DocumentType.Id;
            entityCompany.TributaryIdNo = company.IdentificationDocument.Number;
            entityCompany.CountryCode = company.CountryId;
            entityCompany.CompanyTypeCode = company.CompanyType.Id;
            entityCompany.CheckPayable = Convert.ToString(company.CheckPayable);

            var primaryKeyCoCompany = CoCompany.CreatePrimaryKey(company.IndividualId);
            var entityCoCompany = (CoCompany)DataFacadeManager.GetObject(primaryKeyCoCompany);

            entityCoCompany.VerifyDigit = company.VerifyDigit;
            entityCoCompany.AssociationTypeCode = company.AssociationType.Id;

            DataFacadeManager.Update(entityCompany);
            DataFacadeManager.Update(entityCoCompany);            

            return company;
        }

        public MOUP.Company UpdateCompanyBasicInfo(MOUP.Company company)
        {
            var primaryKeyCompany = Company.CreatePrimaryKey(company.IndividualId);
            var primaryKeyCoCompany = CoCompany.CreatePrimaryKey(company.IndividualId);
            var entityCompany = (Company)DataFacadeManager.GetObject(primaryKeyCompany);
            var entityCoCompany = (CoCompany)DataFacadeManager.GetObject(primaryKeyCoCompany);

            entityCompany.IndividualTypeCode = (int)IndividualType.Company;
            entityCompany.TradeName = company.FullName;
            entityCompany.TributaryIdNo = company.IdentificationDocument.Number;
            entityCompany.CheckPayable = company.FullName;

            DataFacadeManager.Update(entityCompany);

            entityCoCompany.VerifyDigit = company.VerifyDigit;
            entityCoCompany.NitAssociationType = (company.IdentificationDocument.NitAssociationType == "0" || company.IdentificationDocument.NitAssociationType == null) ?
                "0" : company.IdentificationDocument.NitAssociationType;

            DataFacadeManager.Update(entityCoCompany);

            PrimaryKey primaryKeyAgent = Agent.CreatePrimaryKey(company.IndividualId);
            Agent entityAgent = (Agent)DataFacadeManager.GetObject(primaryKeyAgent);

            if (entityAgent != null)
            {
                entityAgent.CheckPayableTo = company.FullName;
                DataFacadeManager.Update(entityAgent);
            }

            return company;
        }

    }
}
