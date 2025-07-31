using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Compañia
    /// </summary>
    public class CompanyDAO
    {
        /// <summary>
        /// Buscar compañias y prospectos por número de documento o por razón social
        /// </summary>
        /// <param name="filter">Filtro de la busqueda</param>
        /// <param name="searchType">Tipo de busqueda
        /// 2. Compañia
        /// 3. Prospecto
        /// 4. Todos</param>
        /// <returns></returns>
        public virtual List<Models.Company> GetCompaniesByDocumentNumberNameSearchType(string documentNumber, string name, int searchType)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Models.Company> companies = new List<Models.Company>();
            List<Models.Company> companiesAll = new List<Models.Company>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            List<Models.DocumentType> documentTypes = new List<Models.DocumentType>();
            BusinessCollection businessCollectionDoc = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.DocumentType)));
            documentTypes = ModelAssembler.CreateDocumentTypes(businessCollectionDoc);

            if (searchType == (int)IndividualSearchType.Company || searchType == (int)IndividualSearchType.All)
            {
                BusinessCollection<UniquePersonV1.Entities.Company> businessCollection;
                filter = new ObjectCriteriaBuilder();
                if (documentNumber != "")
                {
                    filter.Property(UniquePersonV1.Entities.Company.Properties.TributaryIdNo, typeof(UniquePersonV1.Entities.Company).Name);
                    filter.Equal();
                    filter.Constant(documentNumber);
                    businessCollection = DataFacadeManager.Instance.GetDataFacade().List<UniquePersonV1.Entities.Company>(filter.GetPredicate());
                }
                else
                {
                    filter.Property(UniquePersonV1.Entities.Company.Properties.TradeName, typeof(UniquePersonV1.Entities.Company).Name);
                    filter.Like();
                    filter.Constant(name + "%");

                    using (IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade())
                    {
                        string[] sort = { };
                        int totalCount;
                        businessCollection = dataFacade.List<UniquePersonV1.Entities.Company>(filter.GetPredicate(), sort, 200, 0, out totalCount);

                        if (totalCount == 1)
                        {
                            businessCollection = DataFacadeManager.Instance.GetDataFacade().List<UniquePersonV1.Entities.Company>(filter.GetPredicate());
                        }


                    }
                }

                companies = ModelAssembler.CreateCompanies(businessCollection);
                businessCollection = null;
                if (companies.Count == 1)
                {
                    //todo ricardo
                    //AddressDAO addressDAO = new AddressDAO();
                    //companies[0].Addresses = addressDAO.GetAddresses(companies[0].IndividualId);

                    //PhoneDAO phoneDAO = new PhoneDAO();
                    //companies[0].Phones = phoneDAO.GetPhonesByIndividualId(companies[0].IndividualId);

                    //EmailDAO emailDAO = new EmailDAO();
                    //companies[0].Emails = emailDAO.GetEmailsByIndividualId(companies[0].IndividualId);

                    //PaymentMethodAccountDAO paymentMethodAccountDAO = new PaymentMethodAccountDAO();
                    //companies[0].PaymentMethodAccount = paymentMethodAccountDAO.GetPaymentMethodAccountByIndividualId(companies[0].IndividualId);

                    //LegalRepresentativeDAO legalRepresentativeProvider = new LegalRepresentativeDAO();
                    //companies[0].LegalRepresentative = legalRepresentativeProvider.GetLegalRepresentByIndividualId(companies[0].IndividualId);

                    //PartnerDAO parnertDAO = new PartnerDAO();
                    //companies[0].Partners = parnertDAO.GetPartnerByIndividualId(companies[0].IndividualId);

                    //CompanyExtendedDAO coCompanyDAO = new CompanyExtendedDAO();
                    //companies[0].CompanyExtended = coCompanyDAO.GetCoCompanyByIndividualId(companies[0].IndividualId);

                    //CompanyNameDAO companyNameDAO = new CompanyNameDAO();
                    //companies[0].CompanyNames = companyNameDAO.GetCompanyNamesByIndividualId(companies[0].IndividualId);

                    IndividualDAO individualDAO = new IndividualDAO();
                    Models.Individual individualModel = new Models.Individual();
                    individualModel = individualDAO.GetIndividualByIndividualId(companies[0].IndividualId);

                    EconomicActivityDAO economicActivityDAO = new EconomicActivityDAO();
                    companies[0].EconomicActivity = economicActivityDAO.GetEconomicActivitiesByEconomicActiviti(individualModel.EconomicActivity.Id);
                    companies[0].IndividualType = individualModel.IndividualType;
                    companies[0].CustomerType = individualModel.CustomerType;


                }
                companiesAll.AddRange(companies);
            }
            else if (searchType == (int)IndividualSearchType.ProspectusCompany || searchType == (int)IndividualSearchType.All)
            {
                filter = new ObjectCriteriaBuilder();
                BusinessCollection businessCollection1;
                if (documentNumber != "")
                {
                    filter.Property(UniquePersonV1.Entities.Prospect.Properties.TributaryIdNo, typeof(UniquePersonV1.Entities.Prospect).Name);
                    filter.Equal();
                    filter.Constant(documentNumber);
                    businessCollection1 = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.Prospect), filter.GetPredicate()));
                }
                else
                {
                    filter.Property(UniquePersonV1.Entities.Prospect.Properties.TradeName, typeof(UniquePersonV1.Entities.Prospect).Name);
                    filter.Like();
                    filter.Constant(name + "%");

                    using (IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade())
                    {
                        string[] sort = { };
                        int totalCount;

                        businessCollection1 = dataFacade.List<UniquePersonV1.Entities.Prospect>(filter.GetPredicate(), sort, 200, 0, out totalCount);

                        if (totalCount == 1)
                        {
                            businessCollection1 = DataFacadeManager.Instance.GetDataFacade().List<UniquePersonV1.Entities.Prospect>(filter.GetPredicate());
                        }

                    }
                }

                companies = ModelAssembler.CreatePersonProspectsLegals(businessCollection1);
                businessCollection1 = null;
                //todo ricardo
                //if (companies.Count == 1)
                //{
                //    foreach (Models.Address item in companies[0].Addresses)
                //    {
                //        if (item.City != null && item.City.Id != 0)
                //        {
                //            item.City = DelegateService.commonServiceCore.GetCityByCity(item.City);
                //        }
                //    }
                //}
                companiesAll.AddRange(companies);
            }


            for (int i = 0; i < companiesAll.Count(); i++)
            {
                companiesAll[i].IdentificationDocument.DocumentType.SmallDescription = documentTypes.Where(b => b.Id == companiesAll[i].IdentificationDocument.DocumentType.Id).FirstOrDefault().SmallDescription;
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetCompaniesByDocumentNumberNameSearchType");
            return companiesAll;
        }

        /// <summary>
        /// Obtener compañia por identificador
        /// </summary>
        /// <param name="individualId">identificador</param>
        /// <returns></returns>
        public virtual Models.Company GetCompanyByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = UniquePersonV1.Entities.Company.CreatePrimaryKey(individualId);
            UniquePersonV1.Entities.Company companyEntity = (UniquePersonV1.Entities.Company)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            Models.Company company = new Models.Company();

            if (companyEntity != null)
            {
                company = ModelAssembler.CreateCompany(companyEntity);
                //todo ricardo
                //AddressDAO addressDAO = new AddressDAO();
                //company.Addresses = addressDAO.GetAddresses(company.IndividualId);

                //PhoneDAO phoneDAO = new PhoneDAO();
                //company.Phones = phoneDAO.GetPhonesByIndividualId(company.IndividualId);

                //EmailDAO emailDAO = new EmailDAO();
                //company.Emails = emailDAO.GetEmailsByIndividualId(company.IndividualId);
                //PaymentMethodAccountDAO paymentMethodAccountDAO = new PaymentMethodAccountDAO();
                //company.PaymentMethodAccount = paymentMethodAccountDAO.GetPaymentMethodAccountByIndividualId(company.IndividualId);

                //CompanyExtendedDAO coCompanyDAO = new CompanyExtendedDAO();
                //company.CompanyExtended = coCompanyDAO.GetCoCompanyByIndividualId(company.IndividualId);

                IndividualDAO individualDAO = new IndividualDAO();
                Models.Individual individualModel = new Models.Individual();
                individualModel = individualDAO.GetIndividualByIndividualId(company.IndividualId);

                EconomicActivityDAO economicActivityDAO = new EconomicActivityDAO();
                company.EconomicActivity = economicActivityDAO.GetEconomicActivitiesByEconomicActiviti(individualModel.EconomicActivity.Id);
                company.IndividualType = individualModel.IndividualType;
                company.CustomerType = individualModel.CustomerType;

                //todo ricardo
                //PartnerDAO parnertDAO = new PartnerDAO();
                //company.Partners = parnertDAO.GetPartnerByIndividualId(company.IndividualId);

                //LegalRepresentativeDAO legalRepresentativeProvider = new LegalRepresentativeDAO();
                //company.LegalRepresentative = legalRepresentativeProvider.GetLegalRepresentByIndividualId(company.IndividualId);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetCompanyByIndividualId");
            return company;
        }

        ///// <summary>
        ///// Obtener compañia por numero de documento
        ///// </summary>
        ///// <param name="individualCode">numero de documento</param>
        ///// <returns></returns>
        //public virtual Models.Company GetCompanyByDocumentNumber(string documentNumber)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();

        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(UniquePersonV1.Entities.Company.Properties.TributaryIdNo, typeof(UniquePersonV1.Entities.Company).Name);
        //    filter.Equal();
        //    filter.Constant(documentNumber);
        //    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.Company), filter.GetPredicate()));
        //    Models.Company company = ModelAssembler.CreateCompanies(businessCollection).FirstOrDefault();

        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetCompanyByDocumentNumber");
        //    return company;
        //}

        /// <summary>
        /// Crear una nueva compañia
        /// </summary>
        /// <param name="company">Datos compañia</param>
        /// <returns></returns>
        public virtual Models.Company CreateCompany(Models.Company company)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PhoneDAO phoneDAO = new PhoneDAO();
            EmailDAO emailDAO = new EmailDAO();
            PartnerDAO partnerDAO = new PartnerDAO();
            AddressDAO addressDAO = new AddressDAO();
            LegalRepresentativeDAO legalRepresentativeDAO = new LegalRepresentativeDAO();
            //   PaymentMethodAccountDAO paymentMethodAccountDAO = new PaymentMethodAccountDAO();
            CompanyNameDAO coCompanyNameDAO = new CompanyNameDAO();
            // PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
            IDataFacadeManager dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
            IDataFacade df = dataFacadeManager.GetDataFacade();
            Transaction.Created += delegate (object sender, TransactionEventArgs e) { };

            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    transaction.Completed += delegate (object sender, TransactionEventArgs e)
                    {

                    };
                    transaction.Disposed += delegate (object sender, TransactionEventArgs e)
                    {

                    };
                    try
                    {
                        company.IndividualType = IndividualType.Company;
                        UniquePersonV1.Entities.Company entityCompany = EntityAssembler.CreateCompany(company);

                        DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCompany);
                        transaction.Complete();

                        stopWatch.Stop();
                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateCompany");
                        return GetCompanyByIndividualId(entityCompany.IndividualId);

                    }
                    catch (Exception ex)
                    {
                        stopWatch.Stop();
                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateCompany");
                        transaction.Dispose();
                        throw new BusinessException("Error in CreateCompany", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        public virtual Models.Company UpdateCompany(Models.Company company)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PhoneDAO phoneDAO = new PhoneDAO();
            EmailDAO emailDAO = new EmailDAO();
            PartnerDAO partnerDAO = new PartnerDAO();
            AddressDAO addressDAO = new AddressDAO();
            LegalRepresentativeDAO legalRepresentativeDAO = new LegalRepresentativeDAO();
            CompanyNameDAO coCompanyNameDAO = new CompanyNameDAO();

            PrimaryKey key = UniquePersonV1.Entities.Company.CreatePrimaryKey(company.IndividualId);
            UniquePersonV1.Entities.Company entityCompany = (UniquePersonV1.Entities.Company)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (entityCompany != null)
            {
                entityCompany.EconomicActivityCode = company.EconomicActivity.Id;
                entityCompany.TradeName = company.FullName;
                entityCompany.TributaryIdTypeCode = company.IdentificationDocument.DocumentType.Id;
                entityCompany.TributaryIdNo = company.IdentificationDocument.Number;
                entityCompany.CompanyTypeCode = company.CompanyType.Id;
                entityCompany.CheckPayable = company.CheckPayable;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityCompany);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdateCompany");
            return GetCompanyByIndividualId(entityCompany.IndividualId);
        }
    }
}