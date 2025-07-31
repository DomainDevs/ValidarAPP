using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;

//Sistran Core
using Sistran.Core.Application.TempCommonServices.DTOs;
using Sistran.Core.Application.TempCommonServices.Models;
using Sistran.Core.Application.TempCommonServices.Provider.DAOs;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using UniquePersonEntities = Sistran.Core.Application.UniquePerson.Entities;
using TaxEntities = Sistran.Core.Application.Tax.Entities;
using TaxModels = Sistran.Core.Application.TaxServices.Models;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.TempCommonServices.Provider
{
    public class TempCommonServicesProvider : ITempCommonService
    {
        #region Instance Variables

        #region Interfaz

        /// <summary>
        /// Declaración del contexto y del dataFacadeManager
        /// </summary>
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Interfaz

        #region DAOs

        readonly ModuleDateDao _moduleDateDao = new ModuleDateDao();

        #endregion DAOs

        #endregion Instance Viarables

        #region Public Methods 

        #region BankBranch

        /// <summary>
        /// GetBankBranchsByBranchId
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>List<BankBranch></returns>
        public List<BankBranch> GetBankBranchsByBranchId(int branchId)
        {
            List<BankBranch> bankBranches = new List<BankBranch>();

            try
            {
                //Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(Entities.BankBranch.Properties.BankBranchCode, branchId);

                //Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.BankBranch), criteriaBuilder.GetPredicate()));

                foreach (Entities.BankBranch bankBranchEntity in businessCollection.OfType<Entities.BankBranch>())
                {
                    bankBranches.Add(new BankBranch()
                    {
                        Id = bankBranchEntity.BankCode,
                        Description = bankBranchEntity.Description,
                        IsEnabled = Convert.ToBoolean(bankBranchEntity.Enabled)
                    });
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return bankBranches;
        }

        #endregion

        #region Currency

        /// <summary>
        /// GetCurrencyLocal
        /// </summary>
        /// <returns>int</returns>
        public int GetCurrencyLocal()
        {
            int currencyId = 0;

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(Entities.CompanyParameter.Properties.CompanyCode, Entities.CompanyParameter.SISTRAN);

            BusinessCollection currencyCollections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.CompanyParameter), criteriaBuilder.GetPredicate()));

            if (currencyCollections.Count > 0)
            {
                currencyId = Convert.ToInt32(currencyCollections.OfType<Entities.CompanyParameter>().First().CurrencyCode);
            }

            return currencyId;
        }

        #endregion

        #region ModuleDate

        /// <summary>
        /// GetModuleDate
        /// </summary>
        /// <param name="moduleDate"></param>
        /// <returns>ModuleDate</returns>
        public ModuleDate GetModuleDate(ModuleDate moduleDate)
        {
            return _moduleDateDao.GetModuleDate(moduleDate);
        }

        /// <summary>
        /// UpdateModuleDate
        /// </summary>
        /// <param name="moduleDate"></param>
        /// <returns>ModuleDate</returns>
        public ModuleDate UpdateModuleDate(ModuleDate moduleDate)
        {
            return _moduleDateDao.UpdateModuleDate(moduleDate);
        }

        /// <summary>
        /// GetModuleDates
        /// </summary>
        /// <returns>List<ModuleDate></returns>
        public List<ModuleDate> GetModuleDates()
        {
            return _moduleDateDao.GetModuleDates();
        }

        #endregion

        #region Product

        /// <summary>
        /// GetProducts
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns>List<Product></returns>
        public List<Product> GetProductsByPrefixId(int prefixId)
        {
            List<Product> products = new List<Product>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(Entities.Product.Properties.PrefixCode, prefixId);
            BusinessCollection productCollections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.Product), criteriaBuilder.GetPredicate()));

            if (productCollections.Count > 0)
            {
                foreach (Entities.Product productEntity in productCollections.OfType<Entities.Product>())
                {
                    products.Add(new Product()
                    {
                        Id = productEntity.ProductId,
                        Description = productEntity.SmallDescription
                    });
                }
            }

            return products;
        }

        #endregion

        #region Person

        /// <summary>
        /// GetReinsurerByName        
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyTypeCode"></param>
        /// <returns>List<IndividualDTO></returns>
        public List<IndividualDTO> GetReinsurerByName(string name, int reinsurance, int foreignReinsurance)
        {

            CoReinsuranceSearch coReinsuranceSearch = new CoReinsuranceSearch()
            {
                Name = name,
                CompanyTypeCode = reinsurance,
                ForeignReinsurance = foreignReinsurance
            };

            try
            {
                CoReinsuranceBusiness coReinsuranceBusiness = new CoReinsuranceBusiness();
                return coReinsuranceBusiness.GetReinsurerByDocumentNumber(coReinsuranceSearch);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetReinsurerByDocumentNumber        
        /// </summary>
        /// <param name="number"></param>
        /// <param name="companyTypeCode"></param>
        /// <returns>List<IndividualDTO></returns>
        public List<IndividualDTO> GetReinsurerByDocumentNumber(string number, int companyTypeCode)
        {
            CoReinsuranceSearch coReinsuranceSearch = new CoReinsuranceSearch()
            {
                DocumentNumber = number,
                CompanyTypeCode = companyTypeCode
            };
            
            try
            {
                CoReinsuranceBusiness coReinsuranceBusiness = new CoReinsuranceBusiness();
                return coReinsuranceBusiness.GetReinsurerByDocumentNumber(coReinsuranceSearch);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// GetSuppliersByDocumentNumber
        /// </summary>
        /// <param name="number"></param>
        /// <returns>List<IndividualDTO></returns>
        public List<IndividualDTO> GetSuppliersByDocumentNumber(string number)
        {
            List<IndividualDTO> suppliers = new List<IndividualDTO>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(UniquePersonEntities.Company.Properties.TributaryIdNo);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(number + "%");

                UIView supplierCompanies = _dataFacadeManager.GetDataFacade().GetView("CompanySupplierView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out int rows);

                #region DTO

                foreach (DataRow dataRow in supplierCompanies)
                {
                    suppliers.Add(new IndividualDTO()
                    {
                        DocumentNumber = dataRow["TributaryIdNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["TributaryIdNo"]),
                        DocumentTypeId = dataRow["DocumentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DocumentTypeCode"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        IndividualTypeId = dataRow["IndividualTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualTypeCode"]),
                        Name = dataRow["TradeName"] == DBNull.Value ? "" : Convert.ToString(dataRow["TradeName"])
                    });
                }

                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.IdCardNo);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(number + "%");

                UIView supplierPersons = _dataFacadeManager.GetDataFacade().GetView("PersonSupplierView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out rows);

                foreach (DataRow dataRow in supplierPersons)
                {
                    string surname = dataRow["Surname"] == DBNull.Value ? "" : Convert.ToString(dataRow["Surname"]);
                    string motherLastName = dataRow["MotherLastName"] == DBNull.Value ? "" : Convert.ToString(dataRow["MotherLastName"]);
                    string name = dataRow["Name"] == DBNull.Value ? "" : Convert.ToString(dataRow["Name"]);

                    suppliers.Add(new IndividualDTO()
                    {
                        DocumentNumber = dataRow["IdCardNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["IdCardNo"]),
                        DocumentTypeId = dataRow["DocumentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DocumentTypeCode"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        IndividualTypeId = dataRow["IndividualTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualTypeCode"]),
                        Name = surname.Trim() + " " + motherLastName.Trim() + " " + name.Trim()
                    });
                }

                #endregion
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return suppliers;
        }

        /// <summary>
        /// GetSuppliersByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List<IndividualDTO></returns>
        public List<IndividualDTO> GetSuppliersByName(string name)
        {
            List<IndividualDTO> suppliers = new List<IndividualDTO>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(UniquePersonEntities.Company.Properties.TradeName);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");

                UIView supplierCompanies = _dataFacadeManager.GetDataFacade().GetView("CompanySupplierView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out int rows);

                #region DTO

                foreach (DataRow dataRow in supplierCompanies)
                {
                    suppliers.Add(new IndividualDTO()
                    {
                        DocumentNumber = dataRow["TributaryIdNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["TributaryIdNo"]),
                        DocumentTypeId = dataRow["DocumentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DocumentTypeCode"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        IndividualTypeId = dataRow["IndividualTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualTypeCode"]),
                        Name = dataRow["TradeName"] == DBNull.Value ? "" : Convert.ToString(dataRow["TradeName"])
                    });
                }

                criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.OpenParenthesis();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.Surname);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");
                criteriaBuilder.Or();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.MotherLastName);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");
                criteriaBuilder.Or();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.Name);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");
                criteriaBuilder.CloseParenthesis();

                UIView supplierPersons = _dataFacadeManager.GetDataFacade().GetView("PersonSupplierView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out rows);

                foreach (DataRow dataRow in supplierPersons)
                {
                    string surname = dataRow["Surname"] == DBNull.Value ? "" : Convert.ToString(dataRow["Surname"]);
                    string motherLastName = dataRow["MotherLastName"] == DBNull.Value ? "" : Convert.ToString(dataRow["MotherLastName"]);
                    string supplierName = dataRow["Name"] == DBNull.Value ? "" : Convert.ToString(dataRow["Name"]);

                    suppliers.Add(new IndividualDTO()
                    {
                        DocumentNumber = dataRow["IdCardNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["IdCardNo"]),
                        DocumentTypeId = dataRow["DocumentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DocumentTypeCode"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        IndividualTypeId = dataRow["IndividualTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualTypeCode"]),
                        Name = surname.Trim() + " " + motherLastName.Trim() + " " + supplierName.Trim()
                    });
                }

                #endregion
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return suppliers;
        }

        /// <summary>
        /// GetInsuredByDocumentNumber
        /// </summary>
        /// <param name="number"></param>
        /// <returns>List<IndividualDTO></returns>
        public List<IndividualDTO> GetInsuredByDocumentNumber(string number)
        {
            List<IndividualDTO> insurers = new List<IndividualDTO>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(UniquePersonEntities.Company.Properties.TributaryIdNo);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(number + "%");

                UIView insurerCompanies = _dataFacadeManager.GetDataFacade().GetView("CompanyInsurerView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out int rows);

                #region DTO

                foreach (DataRow dataRow in insurerCompanies)
                {
                    insurers.Add(new IndividualDTO()
                    {
                        DocumentNumber = dataRow["TributaryIdNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["TributaryIdNo"]),
                        DocumentTypeId = dataRow["DocumentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DocumentTypeCode"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        IndividualTypeId = dataRow["IndividualTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualTypeCode"]),
                        Name = dataRow["TradeName"] == DBNull.Value ? "" : Convert.ToString(dataRow["TradeName"])
                    });
                }

                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.IdCardNo);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(number + "%");

                UIView insurerPersons = _dataFacadeManager.GetDataFacade().GetView("PersonInsurerView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out rows);

                foreach (DataRow dataRow in insurerPersons)
                {
                    string surname = dataRow["Surname"] == DBNull.Value ? "" : Convert.ToString(dataRow["Surname"]);
                    string motherLastName = dataRow["MotherLastName"] == DBNull.Value ? "" : Convert.ToString(dataRow["MotherLastName"]);
                    string name = dataRow["Name"] == DBNull.Value ? "" : Convert.ToString(dataRow["Name"]);

                    insurers.Add(new IndividualDTO()
                    {
                        DocumentNumber = dataRow["IdCardNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["IdCardNo"]),
                        DocumentTypeId = dataRow["DocumentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DocumentTypeCode"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        IndividualTypeId = dataRow["IndividualTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualTypeCode"]),
                        Name = surname.Trim() + " " + motherLastName.Trim() + " " + name.Trim()
                    });
                }

                #endregion
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return insurers;
        }

        /// <summary>
        /// GetInsuredByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List<IndividualDTO></returns>
        public List<IndividualDTO> GetInsuredByName(string name)
        {
            List<IndividualDTO> insurers = new List<IndividualDTO>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(UniquePersonEntities.Company.Properties.TradeName);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");

                UIView supplierCompanies = _dataFacadeManager.GetDataFacade().GetView("CompanyInsurerView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out int rows);

                #region DTO

                foreach (DataRow dataRow in supplierCompanies)
                {
                    insurers.Add(new IndividualDTO()
                    {
                        DocumentNumber = dataRow["TributaryIdNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["TributaryIdNo"]),
                        DocumentTypeId = dataRow["DocumentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DocumentTypeCode"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        IndividualTypeId = dataRow["IndividualTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualTypeCode"]),
                        Name = dataRow["TradeName"] == DBNull.Value ? "" : Convert.ToString(dataRow["TradeName"])
                    });
                }

                criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.OpenParenthesis();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.Surname);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");
                criteriaBuilder.Or();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.MotherLastName);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");
                criteriaBuilder.Or();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.Name);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");
                criteriaBuilder.CloseParenthesis();

                UIView supplierPersons = _dataFacadeManager.GetDataFacade().GetView("PersonInsurerView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out rows);

                foreach (DataRow dataRow in supplierPersons)
                {
                    string surname = dataRow["Surname"] == DBNull.Value ? "" : Convert.ToString(dataRow["Surname"]);
                    string motherLastName = dataRow["MotherLastName"] == DBNull.Value ? "" : Convert.ToString(dataRow["MotherLastName"]);
                    string supplierName = dataRow["Name"] == DBNull.Value ? "" : Convert.ToString(dataRow["Name"]);

                    insurers.Add(new IndividualDTO()
                    {
                        DocumentNumber = dataRow["IdCardNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["IdCardNo"]),
                        DocumentTypeId = dataRow["DocumentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DocumentTypeCode"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        IndividualTypeId = dataRow["IndividualTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualTypeCode"]),
                        Name = surname.Trim() + " " + motherLastName.Trim() + " " + supplierName.Trim()
                    });
                }

                #endregion
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return insurers;
        }


        /// <summary>
        /// GetAgentByDocumentNumber         
        /// </summary>
        /// <param name="number"></param>
        /// <returns>List<AgentDTO></returns>
        public List<AgentDTO> GetAgentByDocumentNumber(string number)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(UniquePersonEntities.AgentAgency.Properties.AgentAgencyId, 1);
                criteriaBuilder.And();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.IdCardNo);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(number + "%");

                UIView uiviewAgents = _dataFacadeManager.GetDataFacade().GetView("AgentsView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out int rows);

                #region DTO

                List<AgentDTO> agents = new List<AgentDTO>();

                int individualTypes = -1;

                foreach (DataRow dataRow in uiviewAgents)
                {
                    if (Convert.ToInt32(dataRow["IndividualTypeCode"]) == 1)
                    {
                        individualTypes = 0;
                    }
                    else
                    {
                        individualTypes = 1;
                    }

                    string surname = dataRow["Surname"] == DBNull.Value ? "" : Convert.ToString(dataRow["Surname"]);
                    string motherLastName = dataRow["MotherLastName"] == DBNull.Value ? "" : Convert.ToString(dataRow["MotherLastName"]);
                    string supplierName = dataRow["Name"] == DBNull.Value ? "" : Convert.ToString(dataRow["Name"]);

                    agents.Add(new AgentDTO()
                    {
                        AgentAgencyId = dataRow["AgentAgencyId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AgentAgencyId"]),
                        AgentId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        AgentType = dataRow["AgentTypeDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["AgentTypeDescription"]),
                        AgentTypeId = dataRow["AgentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AgentTypeCode"]),
                        BranchId = dataRow["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BranchCode"]),
                        DocumentNumber = dataRow["IdCardNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["IdCardNo"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        IndividualType = individualTypes,
                        Name = surname.Trim() + " " + motherLastName.Trim() + " " + supplierName.Trim()
                    });
                }

                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(UniquePersonEntities.AgentAgency.Properties.AgentAgencyId, 1);
                criteriaBuilder.And();
                criteriaBuilder.Property(UniquePersonEntities.Company.Properties.TributaryIdNo);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(number + "%");

                uiviewAgents = _dataFacadeManager.GetDataFacade().GetView("CompanyAgentView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out rows);

                individualTypes = -1;

                foreach (DataRow dataRow in uiviewAgents)
                {
                    if (Convert.ToInt32(dataRow["IndividualTypeCode"]) == 1)
                    {
                        individualTypes = 0;
                    }
                    else
                    {
                        individualTypes = 1;
                    }

                    agents.Add(new AgentDTO()
                    {
                        AgentAgencyId = dataRow["AgentAgencyId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AgentAgencyId"]),
                        AgentId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        AgentType = dataRow["AgentTypeDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["AgentTypeDescription"]),
                        AgentTypeId = dataRow["AgentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AgentTypeCode"]),
                        BranchId = dataRow["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BranchCode"]),
                        DocumentNumber = dataRow["TributaryIdNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["TributaryIdNo"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        IndividualType = individualTypes,
                        Name = dataRow["TradeName"] == DBNull.Value ? "" : Convert.ToString(dataRow["TradeName"])
                    });
                }

                #endregion 

                return agents;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAgentByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List<AgentDTO></returns>
        public List<AgentDTO> GetAgentByName(string name)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(UniquePersonEntities.AgentAgency.Properties.AgentAgencyId, 1).And();
                criteriaBuilder.OpenParenthesis();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.Surname);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");
                criteriaBuilder.Or();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.MotherLastName);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");
                criteriaBuilder.Or();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.Name);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");
                criteriaBuilder.CloseParenthesis();

                UIView uiviewAgents = _dataFacadeManager.GetDataFacade().GetView("AgentsView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out int rows);

                #region DTO

                List<AgentDTO> agents = new List<AgentDTO>();

                int individualTypes = -1;

                foreach (DataRow dataRow in uiviewAgents)
                {
                    if (Convert.ToInt32(dataRow["IndividualTypeCode"]) == 1)
                    {
                        individualTypes = 0;
                    }
                    else
                    {
                        individualTypes = 1;
                    }

                    string surname = dataRow["Surname"] == DBNull.Value ? "" : Convert.ToString(dataRow["Surname"]);
                    string motherLastName = dataRow["MotherLastName"] == DBNull.Value ? "" : Convert.ToString(dataRow["MotherLastName"]);
                    string supplierName = dataRow["Name"] == DBNull.Value ? "" : Convert.ToString(dataRow["Name"]);

                    agents.Add(new AgentDTO()
                    {
                        AgentAgencyId = dataRow["AgentAgencyId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AgentAgencyId"]),
                        AgentId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        AgentType = dataRow["AgentTypeDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["AgentTypeDescription"]),
                        AgentTypeId = dataRow["AgentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AgentTypeCode"]),
                        BranchId = dataRow["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BranchCode"]),
                        DocumentNumber = dataRow["IdCardNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["IdCardNo"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        IndividualType = individualTypes,
                        Name = surname.Trim() + " " + motherLastName.Trim() + " " + supplierName.Trim()
                    });
                }

                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(UniquePersonEntities.AgentAgency.Properties.AgentAgencyId, 1).And();
                criteriaBuilder.Property(UniquePersonEntities.Company.Properties.TradeName);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");

                uiviewAgents = _dataFacadeManager.GetDataFacade().GetView("CompanyAgentView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out rows);

                individualTypes = -1;

                foreach (DataRow dataRow in uiviewAgents)
                {
                    if (Convert.ToInt32(dataRow["IndividualTypeCode"]) == 1)
                    {
                        individualTypes = 0;
                    }
                    else
                    {
                        individualTypes = 1;
                    }

                    agents.Add(new AgentDTO()
                    {
                        AgentAgencyId = dataRow["AgentAgencyId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AgentAgencyId"]),
                        AgentId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        AgentType = dataRow["AgentTypeDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["AgentTypeDescription"]),
                        AgentTypeId = dataRow["AgentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AgentTypeCode"]),
                        BranchId = dataRow["BranchCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BranchCode"]),
                        DocumentNumber = dataRow["TributaryIdNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["TributaryIdNo"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        IndividualType = individualTypes,
                        Name = dataRow["TradeName"] == DBNull.Value ? "" : Convert.ToString(dataRow["TradeName"])
                    });
                }

                #endregion 

                return agents;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCommissionDiscountAgreementByAgentId
        /// Verifica si el agente tiene convenio de descuento de comisiones        
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns>bool</returns>
        public bool GetCommissionDiscountAgreementByAgentId(int agentId)
        {
            bool result = false;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(Entities.Agent.Properties.IndividualId, agentId);

                BusinessCollection businessCollection =
                   new BusinessCollection(
                       _dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.Agent), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (Entities.Agent agent in businessCollection.OfType<Entities.Agent>())
                    {
                        result = Convert.ToBoolean(agent.CommissionDiscountAgreement);
                    }
                }
                else
                {
                    result = false;
                }

                return result;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPersonTypeByPaymentOrderEnable
        /// </summary>
        /// <returns>List<PersonType></returns>
        public List<PersonType> GetPersonTypeByPaymentOrderEnable()
        {
            List<PersonType> personTypes = new List<PersonType>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(Entities.PersonType.Properties.PaymentOrderEnable, 1);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                          (typeof(Entities.PersonType), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (Entities.PersonType personTypeItem in businessCollection.OfType<Entities.PersonType>())
                    {
                        PersonType personType = new PersonType()
                        {
                            Description = personTypeItem.Description,
                            Id = personTypeItem.PersonTypeCode
                        };

                        personTypes.Add(personType);
                    }
                }

                return personTypes;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPersonTypeByPaymentOrderEnable
        /// </summary>
        /// <returns>List<PersonType></returns>
        public List<PersonType> GetPersonTypeByPaymentOrderEnableFilter()
        {
            List<PersonType> personTypes = new List<PersonType>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(Entities.PersonType.Properties.PaymentOrderEnable, 1);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                          (typeof(Entities.PersonType), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (Entities.PersonType personTypeItem in businessCollection.OfType<Entities.PersonType>())
                    {
                        PersonType personType = new PersonType()
                        {
                            Description = personTypeItem.Description,
                            Id = personTypeItem.PersonTypeCode
                        };

                        personTypes.Add(personType);
                    }
                }

                return personTypes.Where(x => x.Description != "COMPANIA" && x.Description != "ASEGURADO").ToList(); ;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPersonTypesByBillEnabled
        /// </summary>
        /// <returns>List<PersonType></returns>
        public List<PersonType> GetPersonTypesByBillEnabled()
        {
            List<PersonType> personTypes = new List<PersonType>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(Entities.PersonType.Properties.BillEnabled, 1);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                          (typeof(Entities.PersonType), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (Entities.PersonType personTypeItem in businessCollection.OfType<Entities.PersonType>())
                    {
                        PersonType personType = new PersonType()
                        {
                            Description = personTypeItem.Description,
                            Id = personTypeItem.PersonTypeCode
                        };

                        personTypes.Add(personType);
                    }
                }

                return personTypes;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPersonTypeByPreaplicationEnable
        /// </summary>
        /// <returns>List<PersonType></returns>
        public List<PersonType> GetPersonTypeByPreaplicationEnable()
        {
            List<PersonType> personTypes = new List<PersonType>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(Entities.PersonType.Properties.PreaplicationEnabled, 1);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                          (typeof(Entities.PersonType), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (Entities.PersonType personTypeItem in businessCollection.OfType<Entities.PersonType>())
                    {
                        PersonType personType = new PersonType()
                        {
                            Description = personTypeItem.Description,
                            Id = personTypeItem.PersonTypeCode
                        };

                        personTypes.Add(personType);
                    }
                }

                return personTypes;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPersonsByDocumentNumber
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <returns>List<IndividualDTO></returns>
        public List<IndividualDTO> GetPersonsByDocumentNumber(string documentNumber, bool typePerson)
        {
            List<IndividualDTO> persons = new List<IndividualDTO>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.IdCardNo);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(documentNumber + "%");

                UIView uiviewPersons = _dataFacadeManager.GetDataFacade().GetView("PersonView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out int rows);

                foreach (DataRow dataRow in uiviewPersons.Rows)
                {
                    string surname = dataRow["Surname"] == DBNull.Value ? "" : Convert.ToString(dataRow["Surname"]);
                    string motherLastName = dataRow["MotherLastName"] == DBNull.Value ? "" : Convert.ToString(dataRow["MotherLastName"]);
                    string supplierName = dataRow["Name"] == DBNull.Value ? "" : Convert.ToString(dataRow["Name"]);

                    persons.Add(new IndividualDTO()
                    {
                        DocumentNumber = dataRow["IdCardNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["IdCardNo"]),
                        DocumentTypeId = dataRow["DocumentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DocumentTypeCode"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        IndividualTypeId = dataRow["IndividualTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualTypeCode"]),
                        Name = surname.Trim() + " " + motherLastName.Trim() + " " + supplierName.Trim()
                    });
                }

                if (!typePerson) //Ajuste para tipo Empleado, consulta solo persona natural.
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.Property(UniquePersonEntities.Company.Properties.TributaryIdNo);
                    criteriaBuilder.Like();
                    criteriaBuilder.Constant(documentNumber + "%");

                    UIView uiviewCompanies = _dataFacadeManager.GetDataFacade().GetView("CompanyView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out rows);

                    foreach (DataRow dataRow in uiviewCompanies.Rows)
                    {
                        persons.Add(new IndividualDTO()
                        {
                            DocumentNumber = dataRow["TributaryIdNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["TributaryIdNo"]),
                            DocumentTypeId = dataRow["DocumentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DocumentTypeCode"]),
                            IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                            IndividualTypeId = dataRow["IndividualTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualTypeCode"]),
                            Name = dataRow["TradeName"] == DBNull.Value ? "" : Convert.ToString(dataRow["TradeName"])
                        });
                    }
                }
            }
            catch (BusinessException)
            {
                persons = new List<IndividualDTO>();
            }

            return persons;
        }

        /// <summary>
        /// GetPersonsByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List<IndividualDTO></returns>
        public List<IndividualDTO> GetPersonsByName(string name)
        {
            List<IndividualDTO> persons = new List<IndividualDTO>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.OpenParenthesis();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.Surname);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");
                criteriaBuilder.Or();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.MotherLastName);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");
                criteriaBuilder.Or();
                criteriaBuilder.Property(UniquePersonEntities.Person.Properties.Name);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");
                criteriaBuilder.CloseParenthesis();

                UIView uiviewPersons = _dataFacadeManager.GetDataFacade().GetView("PersonView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out int rows);

                foreach (DataRow dataRow in uiviewPersons.Rows)
                {
                    string surname = dataRow["Surname"] == DBNull.Value ? "" : Convert.ToString(dataRow["Surname"]);
                    string motherLastName = dataRow["MotherLastName"] == DBNull.Value ? "" : Convert.ToString(dataRow["MotherLastName"]);
                    string supplierName = dataRow["Name"] == DBNull.Value ? "" : Convert.ToString(dataRow["Name"]);

                    persons.Add(new IndividualDTO()
                    {
                        DocumentNumber = dataRow["IdCardNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["IdCardNo"]),
                        DocumentTypeId = dataRow["DocumentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DocumentTypeCode"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        IndividualTypeId = dataRow["IndividualTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualTypeCode"]),
                        Name = surname.Trim() + " " + motherLastName.Trim() + " " + supplierName.Trim()
                    });
                }

                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(UniquePersonEntities.Company.Properties.TradeName);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(name + "%");

                UIView uiviewCompanies = _dataFacadeManager.GetDataFacade().GetView("CompanyView", criteriaBuilder.GetPredicate(), null, 0, 10, null, false, out rows);

                foreach (DataRow dataRow in uiviewCompanies.Rows)
                {
                    persons.Add(new IndividualDTO()
                    {
                        DocumentNumber = dataRow["TributaryIdNo"] == DBNull.Value ? "" : Convert.ToString(dataRow["TributaryIdNo"]),
                        DocumentTypeId = dataRow["DocumentTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DocumentTypeCode"]),
                        IndividualId = dataRow["IndividualId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualId"]),
                        IndividualTypeId = dataRow["IndividualTypeCode"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["IndividualTypeCode"]),
                        Name = dataRow["TradeName"] == DBNull.Value ? "" : Convert.ToString(dataRow["TradeName"])
                    });
                }
            }
            catch (BusinessException)
            {
                persons = new List<IndividualDTO>();
            }

            return persons;
        }

        #endregion

        #region Reinsurance

        /// <summary>
        /// GetEndorsementByPolicyId
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns>List<EndorsementDTO></returns>
        public EndorsementDTO GetEndorsementByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                EndorsementDTO endorsement = new EndorsementDTO();

                //Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(Entities.EndorsementByPolicyIdView.Properties.PolicyId, Convert.ToInt32(policyId));
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(Entities.EndorsementByPolicyIdView.Properties.EndorsementId, Convert.ToInt32(endorsementId));

                UIView uiviewEndorsements = _dataFacadeManager.GetDataFacade().GetView("EndorsementByPolicyIdView", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out int rows);

                foreach (DataRow dataRow in uiviewEndorsements.Rows)
                {
                    endorsement = new EndorsementDTO()
                    {
                        Currency = dataRow["CurrencyDescription"].ToString(),
                        CurrentFrom = Convert.ToDateTime(dataRow["CurrentFrom"]),
                        CurrentTo = Convert.ToDateTime(dataRow["CurrentTo"]),
                        Description = dataRow["EndorsementType"].ToString(),
                        EndorsementId = Convert.ToInt32(dataRow["EndorsementId"]),
                        EndorsementNumber = Convert.ToInt32(dataRow["DocumentNum"]),
                        InsuredAmount = Convert.ToDecimal(dataRow["InsuredAmount"]),
                        InsuredCd = Convert.ToInt32(dataRow["InsuredCode"]),
                        InsuredName = dataRow["InsuredName"].ToString(),
                        IssueDate = Convert.ToDateTime(dataRow["IssueDate"]),
                        OperationType = dataRow["OperationType"].ToString(),
                        PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                        Prime = Convert.ToDecimal(dataRow["Prime"]),
                        ResponsibilityMaximumAmount = Convert.ToDecimal(dataRow["ResponsibilityMaximumAmount"])
                    };
                }

                return endorsement;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<EndorsementDTO> GetEndorsementByPolicyId(int policyId)
        {
            try
            {
                List<EndorsementDTO> endorsementDTOs = new List<EndorsementDTO>();

                //Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(Entities.EndorsementByPolicyIdView.Properties.PolicyId, Convert.ToInt32(policyId));

                string sortExp = null;
                string[] sortExps = null;
                sortExp = "+" + "PolicyId";
                sortExps = new string[] { sortExp };


                UIView uiviewEndorsements = _dataFacadeManager.GetDataFacade().GetView("EndorsementByPolicyIdView", criteriaBuilder.GetPredicate(), null, 0, 50, sortExps, false, out int rows);

                foreach (DataRow dataRow in uiviewEndorsements.Rows)
                {
                    EndorsementDTO endorsementDTO = new EndorsementDTO()
                    {
                        Currency = dataRow["CurrencyDescription"].ToString(),
                        CurrentFrom = Convert.ToDateTime(dataRow["CurrentFrom"]),
                        CurrentTo = Convert.ToDateTime(dataRow["CurrentTo"]),
                        Description = dataRow["EndorsementType"].ToString(),
                        EndorsementId = Convert.ToInt32(dataRow["EndorsementId"]),
                        EndorsementNumber = Convert.ToInt32(dataRow["DocumentNum"]),
                        InsuredAmount = Convert.ToDecimal(dataRow["InsuredAmount"]),
                        InsuredCd = Convert.ToInt32(dataRow["InsuredCode"]),
                        InsuredName = dataRow["InsuredName"].ToString(),
                        IssueDate = Convert.ToDateTime(dataRow["IssueDate"]),
                        OperationType = dataRow["OperationType"].ToString(),
                        PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                        Prime = Convert.ToDecimal(dataRow["Prime"]),
                        ResponsibilityMaximumAmount = Convert.ToDecimal(dataRow["ResponsibilityMaximumAmount"])
                    };

                    endorsementDTOs.Add(endorsementDTO);
                }

                return endorsementDTOs;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
		/// Devuelve una poliza Reaseguro
		/// </summary>
		/// <param name="policyId">id de poliza </param>
		/// <param name="endorsementId">id de endoso</param>
		/// <returns>Policy</returns>
        public Policy GetPolicyReinsuranceByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                Policy policy = new Policy() { Id = policyId };
                EndorsementDTO endorsement = GetEndorsementByPolicyIdEndorsementId(policyId, endorsementId);

                if (endorsementId == endorsement.EndorsementId)
                {
                    policy.Endorsement = new Endorsement()
                    {
                        CurrentFrom = Convert.ToDateTime(endorsement.CurrentFrom),
                        CurrentTo = Convert.ToDateTime(endorsement.CurrentTo),
                        Id = endorsement.EndorsementId,
                        IssueDate = Convert.ToDateTime(endorsement.IssueDate),
                        Number = endorsement.EndorsementNumber
                    };
                }

                policy.Endorsement.Risks = new List<Risk>();

                int processType = 4;
                int service = 1;
                var parameters = new NameValue[7];

                parameters[0] = new NameValue("@ENDORSEMENT_ID", policy.Endorsement.Id);
                parameters[1] = new NameValue("@DATE_FROM", "");
                parameters[2] = new NameValue("@DATE_TO", "");
                parameters[3] = new NameValue("@PROCESS_TYPE", processType);
                parameters[4] = new NameValue("@USER_ID", 0);
                parameters[5] = new NameValue("@PREFIX_XML", "");
                parameters[6] = new NameValue("@SERVICE", service);

                DataTable result;

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("REINS.ISS_AUTOPROC1_WORKTABLES_LOAD", parameters);
                }

                List<Risk> risks = new List<Risk>();
                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        Risk risk = new Risk()
                        {
                            Id = Convert.ToInt32(row[1]),
                            IndividualId = Convert.ToInt32(row[6]) 
                        };
                        risks.Add(risk);
                    }
                }

                List<Risk> finalRisks = risks.GroupBy(x => new { x.Id }).Select(y => y.First()).ToList();
                List<Coverage> coverages = new List<Coverage>();

                foreach (Risk risk in finalRisks)
                {
                    coverages = new List<Coverage>();

                    if (result != null && result.Rows.Count > 0)
                    {
                        foreach (DataRow row in result.Rows)
                        {
                            if (risk.Id == Convert.ToInt32(row[1]))
                            {
                                Coverage coverage = new Coverage()
                                {
                                    Id = Convert.ToInt32(row[2]),
                                    LineId = Convert.ToInt32(row[3]),
                                    CumulusKey = row[4].ToString(),
                                    ErrorId = Convert.ToInt32(row[5]),
                                    LineBusinessId = Convert.ToInt32(row[7]),
                                };
                                coverages.Add(coverage);
                            }
                        }
                    }

                    risk.Coverages = coverages;
                    policy.Endorsement.Risks.Add(risk);
                }

                return policy;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Tax

        /// <summary>   
        /// GetTaxes
        /// Llena el combo de impuestos, desarrollado para General Ledger
        /// </summary>
        /// <returns>List<TaxModels.Tax></returns>
        public List<TaxModels.Tax> GetTaxes()
        {
            List<TaxModels.Tax> taxes = new List<TaxModels.Tax>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(TaxEntities.Tax.Properties.TaxCode);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);

                BusinessCollection taxCollections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(TaxEntities.Tax), criteriaBuilder.GetPredicate()));

                if (taxCollections.Count > 0)
                {
                    foreach (TaxEntities.Tax taxEntity in taxCollections.OfType<TaxEntities.Tax>())
                    {
                        taxes.Add(
                            new TaxModels.Tax()
                            {
                                Description = taxEntity.Description,
                                Id = taxEntity.TaxCode
                            }
                        );
                    }
                }

            }
            catch (BusinessException)
            {
                taxes = new List<TaxModels.Tax>();
            }

            return taxes;
        }

        /// <summary>
        /// GetTotalTax
        /// </summary>
        /// <param name="individualId">individualId</param>
        /// <param name="conditionCode">conditionCode</param>
        /// <param name="categoryCode">categoryCode</param>
        /// <param name="branchCode">branchCode</param>
        /// <param name="lineBusinessCode">lineBusinessCode</param>
        /// <param name="stateCode">stateCode</param>
        /// <param name="countryCode">countryCode</param>
        /// <param name="economicActivity">economicActivity</param>
        /// <param name="exchangeRate">exchangeRate</param>
        /// <param name="amount">amount</param>
        /// <returns>Decimal</returns>
        public Decimal GetTotalTax(int individualId, int conditionCode, Dictionary<int, int> categories, int branchCode, int lineBusinessCode,
                             double exchangeRate, double amount)
        {
            try
            {
                decimal totalTax = 0;

                if (lineBusinessCode == -1)
                {
                    totalTax = CalculateRetention(individualId, conditionCode, categories, branchCode, lineBusinessCode, exchangeRate, amount);
                }
                else
                {
                    totalTax = CalculateTax(individualId, conditionCode, categories, branchCode, lineBusinessCode, exchangeRate, amount);
                }

                return totalTax;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// CalculateRetention
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="conditionCode"></param>
        /// <param name="taxCategories"></param>
        /// <param name="branchCode"></param>
        /// <param name="lineBusinessCode"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="amount"></param>
        /// <returns>Decimal</returns>
        private Decimal CalculateRetention(int individualId, int conditionCode, Dictionary<int, int> taxCategories, int branchCode, int lineBusinessCode, double exchangeRate, double amount)
        {
            try
            {
                int categoryCode = 0;
                DataTable dataTableTax = new DataTable();
                dataTableTax.Columns.Add("TaxConditionCode", typeof(int));
                dataTableTax.Columns.Add("TaxCategoryCode", typeof(int));
                dataTableTax.Columns.Add("TaxCode", typeof(int));
                dataTableTax.Columns.Add("Tax", typeof(string));
                dataTableTax.Columns.Add("Rate", typeof(decimal));
                dataTableTax.Columns.Add("TaxAmountBase", typeof(decimal));
                dataTableTax.Columns.Add("TaxValue", typeof(decimal));

                int stateCode = 0;
                int countryCode = 0;
                int economicActivity = 0;

                #region DataRequest

                List<int> address = GetAddressByIndividualId(individualId);
                if (address.Count > 1)
                {
                    stateCode = address[0];
                    countryCode = address[1];
                }

                economicActivity = GetEconomicActivityByIndividualId(individualId);

                #endregion


                #region SearchTax

                //Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(TaxEntities.IndividualTax.Properties.IndividualId, individualId).And();
                criteriaBuilder.PropertyEquals(TaxEntities.Tax.Properties.Enabled, 1).And();
                //criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.BranchCode, branchCode).And();
                //filter.PropertyEquals(TaxEntities.TaxRate.Properties.LineBusinessCode, 1);
                criteriaBuilder.Property(TaxEntities.TaxRate.Properties.BranchCode);
                criteriaBuilder.IsNull();
                criteriaBuilder.And();
                criteriaBuilder.Property(TaxEntities.TaxRate.Properties.LineBusinessCode);
                criteriaBuilder.IsNull();

                DataTable dataTableAttribute = GetAttributeTaxByIndividualId(individualId);
                int auxTaxCode = 0;
                int auxTaxCodeOne = 0;
                int numberTax = 0;

                if (dataTableAttribute.Rows.Count > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.OpenParenthesis();
                }
                foreach (DataRow dataRow in dataTableAttribute.Rows)
                {
                    auxTaxCode = Convert.ToInt32(dataRow["TaxCode"]);

                    categoryCode = taxCategories[auxTaxCode];

                    if (auxTaxCode != auxTaxCodeOne)
                    {
                        if (numberTax > 0)
                        {
                            criteriaBuilder.CloseParenthesis();
                            criteriaBuilder.Or();
                            numberTax = 0;
                        }
                        criteriaBuilder.OpenParenthesis();
                        criteriaBuilder.PropertyEquals(TaxEntities.TaxCondition.Properties.TaxCode, auxTaxCode);
                        criteriaBuilder.And();
                        numberTax = numberTax + 1;
                    }
                    else
                    {
                        criteriaBuilder.And();
                    }

                    auxTaxCodeOne = auxTaxCode;

                    switch (Convert.ToString(dataRow["Description"]))
                    {
                        case "TAX_CONDITION_CODE":
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxCondition.Properties.TaxConditionCode, conditionCode);
                            break;
                        case "TAX_CATEGORY_CODE":
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxCategory.Properties.TaxCategoryCode, categoryCode);
                            break;
                        case "STATE_CODE":
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.StateCode, stateCode);
                            break;
                        case "COUNTRY_CODE":
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.CountryCode, countryCode);
                            break;
                        case "ECONOMIC_ACTIVITY_CODE":
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.EconomicActivityTaxCode, economicActivity);
                            break;
                        case "BRANCH_CODE":
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.BranchCode, branchCode);
                            break;
                        case "LINE_BUSINESS_CODE":
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.LineBusinessCode, lineBusinessCode);
                            break;
                    }
                }
                if (numberTax > 0)
                {
                    criteriaBuilder.CloseParenthesis();
                    numberTax = 0;
                }

                if (dataTableAttribute.Rows.Count > 0)
                {
                    criteriaBuilder.CloseParenthesis();
                }

                //Lista de impuestos
                UIView individualTaxes = _dataFacadeManager.GetDataFacade().GetView("IndividualTaxView", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out int rows);

                #endregion 

                #region CalculateTax

                foreach (DataRow dataRow in individualTaxes.Rows)
                {
                    int baseConditionTaxCode = 0;

                    //Con retenciones
                    if (Convert.ToBoolean(dataRow["IsRetention"]))
                    {
                        if (dataRow["BaseConditionTaxCode"] != DBNull.Value)
                        {
                            baseConditionTaxCode = Convert.ToInt32(dataRow["BaseConditionTaxCode"]);
                        }
                        // tax con base imponible
                        if (baseConditionTaxCode == 0)
                        {
                            int rateTypeCode = Convert.ToInt32(dataRow["RateTypeCode"]);
                            int taxCode = Convert.ToInt32(dataRow["TaxCode"]);
                            double rate = Convert.ToDouble(dataRow["Rate"]);
                            int taxConditionCode = Convert.ToInt32(dataRow["TaxConditionCode"]);
                            int enabled = Convert.ToInt32(dataRow["Enabled"]);
                            DateTime currentFrom = Convert.ToDateTime(dataRow["CurrentFrom"]);
                            double minBaseAmount = minBaseAmount = Convert.ToDouble(dataRow["MinBaseAmount"]);

                            if (enabled == 1 && currentFrom < DateTime.Now)
                            {
                                if (rateTypeCode == Entities.RateType.FIXED_VALUE)
                                {
                                    rate = rate / exchangeRate;
                                }

                                decimal excemptionPct = GetExemptionPercentage(individualId, taxCode, DateTime.Now, countryCode, stateCode);

                                // calcula si supera la base minima
                                double itemAmount = 0;
                                if (minBaseAmount <= amount && taxConditionCode == 1)
                                {
                                    itemAmount = Calculate(rateTypeCode, rate, amount, excemptionPct);
                                }

                                DataRow dataRowTax = dataTableTax.NewRow();
                                dataRowTax["TaxConditionCode"] = dataRow["TaxConditionCode"];
                                dataRowTax["TaxCategoryCode"] = dataRow["TaxCategoryCode"];
                                dataRowTax["TaxCode"] = taxCode;
                                dataRowTax["Tax"] = dataRow["Description"];
                                dataRowTax["Rate"] = rate;
                                dataRowTax["TaxAmountBase"] = amount;
                                dataRowTax["TaxValue"] = itemAmount;
                                dataTableTax.Rows.Add(dataRowTax);
                            }
                        }
                    }
                }

                #endregion

                #region CalculateTaxDep

                foreach (DataRow dataRow in individualTaxes.Rows)
                {
                    //Con retenciones
                    if (Convert.ToBoolean(dataRow["IsRetention"]) == false)
                    {
                        int baseConditionTaxCode = 0;
                        if (dataRow["BaseConditionTaxCode"] != DBNull.Value)
                        {
                            baseConditionTaxCode = Convert.ToInt32(dataRow["BaseConditionTaxCode"]);
                        }
                        // tax con base imponible
                        if (baseConditionTaxCode > 0)
                        {
                            int rateTypeCode = Convert.ToInt32(dataRow["RateTypeCode"]);
                            int taxCode = Convert.ToInt32(dataRow["TaxCode"]);
                            double rate = Convert.ToDouble(dataRow["Rate"]);
                            int taxConditionCode = Convert.ToInt32(dataRow["TaxConditionCode"]);
                            int enabled = Convert.ToInt32(dataRow["Enabled"]);
                            DateTime currentFrom = Convert.ToDateTime(dataRow["CurrentFrom"]);
                            double minBaseAmount = minBaseAmount = Convert.ToDouble(dataRow["MinBaseAmount"]);

                            if (enabled == 1 && currentFrom < DateTime.Now)
                            {
                                if (rateTypeCode == Entities.RateType.FIXED_VALUE)
                                {
                                    rate = rate / exchangeRate;
                                }

                                double itemAmount = 0;
                                bool taxBaseCondition = false;
                                foreach (DataRow row in dataTableTax.Rows)
                                {
                                    if (baseConditionTaxCode == Convert.ToInt32(row["TaxCode"]))
                                    {
                                        amount = Convert.ToDouble(row["TaxValue"]);

                                        decimal excemptionPct = GetExemptionPercentage(individualId, taxCode, DateTime.Now, countryCode, stateCode);

                                        // calcula si supera la base minima
                                        itemAmount = 0;
                                        if (minBaseAmount <= amount && taxConditionCode == 1)
                                        {
                                            itemAmount = Calculate(rateTypeCode, rate, amount, excemptionPct);
                                        }
                                        taxBaseCondition = true;
                                    }
                                }

                                if (!taxBaseCondition)
                                {
                                    amount = 0;
                                    itemAmount = 0;
                                }

                                DataRow dataRowTax = dataTableTax.NewRow();
                                dataRowTax["TaxConditionCode"] = dataRow["TaxConditionCode"];
                                dataRowTax["TaxCategoryCode"] = dataRow["TaxCategoryCode"];
                                dataRowTax["TaxCode"] = taxCode;
                                dataRowTax["Tax"] = dataRow["Description"];
                                dataRowTax["Rate"] = rate;
                                dataRowTax["TaxAmountBase"] = amount;
                                dataRowTax["TaxValue"] = itemAmount;
                                dataTableTax.Rows.Add(dataRowTax);
                            }
                        }
                    }
                }

                #endregion

                decimal totalTax = 0;

                foreach (DataRow dataRow in dataTableTax.Rows)
                {
                    totalTax = totalTax + Convert.ToDecimal(dataRow["TaxValue"]);
                }

                return totalTax;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAttributeTaxByIndividualId
        /// </summary>
        /// <param name="individualId">individualId</param>
        /// <returns>DataTable</returns>
        private DataTable GetAttributeTaxByIndividualId(int individualId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(TaxEntities.IndividualTax.Properties.IndividualId, individualId).And();
                filter.PropertyEquals(TaxEntities.Tax.Properties.Enabled, 1);

                return _dataFacadeManager.GetDataFacade().GetView("TaxAttributeIndividualView", filter.GetPredicate(), null, 0, 50, null, false, out int rows);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAddressByIndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        private List<int> GetAddressByIndividualId(int individualId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(UniquePersonEntities.Address.Properties.IndividualId, individualId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(UniquePersonEntities.Address.Properties.IsMailingAddress, true);
                int rows;
                UIView individualAddress = _dataFacadeManager.GetDataFacade().GetView("AddressView", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out rows);
                List<int> address = new List<int>();
                foreach (DataRow dataRow in individualAddress.Rows)
                {
                    address.Add(Convert.ToInt32(dataRow["StateCode"]));
                    address.Add(Convert.ToInt32(dataRow["CountryCode"]));
                }

                return address;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetEconomicActivityByIndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>int</returns>
        private int GetEconomicActivityByIndividualId(int individualId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(UniquePersonEntities.Address.Properties.IndividualId, individualId);

            int rows;
            UIView economicActivities = _dataFacadeManager.GetDataFacade().GetView("IndividualView", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out rows);
            if (economicActivities.Rows.Count > 0)
            {
                return Convert.ToInt32(economicActivities.Rows[0]["EconomicActivityCode"]);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// GetExemptionPercentage
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="taxCode"></param>
        /// <param name="date"></param>
        /// <param name="countryCode"></param>
        /// <param name="stateCode"></param>
        /// <returns>decimal</returns>
        private decimal GetExemptionPercentage(int individualId, int taxCode, DateTime date, int countryCode, int stateCode)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            #region Construcción_Filtro

            criteriaBuilder.PropertyEquals(TaxEntities.IndividualTaxExemption.Properties.IndividualId, individualId).And();
            criteriaBuilder.PropertyEquals(TaxEntities.IndividualTaxExemption.Properties.TaxCode, taxCode).And();

            criteriaBuilder.Property(TaxEntities.IndividualTaxExemption.Properties.CurrentFrom);
            criteriaBuilder.LessEqual();
            criteriaBuilder.Constant(date);

            criteriaBuilder.And();
            criteriaBuilder.Property(TaxEntities.IndividualTaxExemption.Properties.CountryCode);

            if (countryCode == 0)
            {
                criteriaBuilder.IsNull();
            }
            else
            {
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(countryCode);
            }


            //Provincia
            criteriaBuilder.And();

            criteriaBuilder.OpenParenthesis();
            criteriaBuilder.Property(TaxEntities.IndividualTaxExemption.Properties.StateCode);
            criteriaBuilder.IsNull();

            if (stateCode != 0)
            {
                criteriaBuilder.Or();
                criteriaBuilder.Property(TaxEntities.IndividualTaxExemption.Properties.StateCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(stateCode);
            }

            criteriaBuilder.CloseParenthesis();

            #endregion


            decimal exemptionPercentage = 0;


            int rows;
            UIView taxExemptions = _dataFacadeManager.GetDataFacade().GetView("IndividualTaxExemptionView", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out rows);

            if (taxExemptions.Rows.Count > 0)
            {
                exemptionPercentage = Convert.ToDecimal(taxExemptions.Rows[0]["ExemptionPercentage"]);
            }

            return exemptionPercentage;
        }

        /// <summary>
        /// Calculate
        /// Calcula el impuesto. Multiplica la tasa por la base imponible.
        /// </summary>
        /// <param name="rateTypeCode">Tipo de tasa</param>
        /// <param name="rate">Tasa que se desea aplicar.</param>
        /// <param name="amount">Base imponible</param>
        /// <param name="exemptionPercentage"></param>
        /// <returns>double</returns>
        private double Calculate(int rateTypeCode, double rate, double amount, decimal exemptionPercentage)
        {
            double factor = Convert.ToDouble(GetFactor(rateTypeCode));
            double tax = 0;

            if (rateTypeCode == Entities.RateType.FIXED_VALUE)
            {
                tax = rate;
            }
            else
            {
                tax = amount * rate * factor;
            }

            return tax - (tax * (double)exemptionPercentage / 100);
        }

        /// <summary>
        /// CalculateTax
        /// </summary>
        /// <param name="individualId">individualId</param>
        /// <param name="branchCode">branchCode</param>
        /// <param name="lineBusinessCode">lineBusinessCode</param>
        /// <param name="exchangeRate">exchangeRate</param>
        /// <param name="amount">amount</param>
        /// <param name="paymentRequestCode">paymentRequestCode</param>
        /// <param name="voucherConceptCode">voucherConceptCode</param>
        /// <returns>Decimal</returns>
        private Decimal CalculateTax(int individualId, int conditionCode, Dictionary<int, int> taxCategories, int branchCode, int lineBusinessCode, double exchangeRate, double amount)
        {
            try
            {
                int categoryCode = 0;
                DataTable dataTableTax = new DataTable();
                dataTableTax.Columns.Add("TaxConditionCode", typeof(int));
                dataTableTax.Columns.Add("TaxCategoryCode", typeof(int));
                dataTableTax.Columns.Add("TaxCode", typeof(int));
                dataTableTax.Columns.Add("Tax", typeof(string));
                dataTableTax.Columns.Add("Rate", typeof(decimal));
                dataTableTax.Columns.Add("TaxAmountBase", typeof(decimal));
                dataTableTax.Columns.Add("TaxValue", typeof(decimal));

                int stateCode = 0;
                int countryCode = 0;
                int economicActivity = 0;
                #region DataRequest

                List<int> address = GetAddressByIndividualId(individualId);
                if (address.Count > 1)
                {
                    stateCode = address[0];
                    countryCode = address[1];
                }

                economicActivity = GetEconomicActivityByIndividualId(individualId);

                #endregion

                #region SearchTax

                //Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(TaxEntities.IndividualTax.Properties.IndividualId, individualId).And();
                criteriaBuilder.PropertyEquals(TaxEntities.Tax.Properties.Enabled, 1).And();
                criteriaBuilder.Property(TaxEntities.TaxRate.Properties.BranchCode);
                criteriaBuilder.IsNull();
                criteriaBuilder.And();
                criteriaBuilder.Property(TaxEntities.TaxRate.Properties.LineBusinessCode);
                criteriaBuilder.IsNull();

                DataTable dataTableAttribute = GetAttributeTaxByIndividualId(individualId);
                int auxTaxCode = 0;
                int auxTaxCodeOne = 0;
                int numberTax = 0;

                if (dataTableAttribute.Rows.Count > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.OpenParenthesis();
                }
                foreach (DataRow dr in dataTableAttribute.Rows)
                {
                    auxTaxCode = Convert.ToInt32(dr["TaxCode"]);

                    categoryCode = taxCategories[auxTaxCode];

                    if (auxTaxCode != auxTaxCodeOne)
                    {
                        if (numberTax > 0)
                        {
                            criteriaBuilder.CloseParenthesis();
                            criteriaBuilder.Or();
                            numberTax = 0;
                        }
                        criteriaBuilder.OpenParenthesis();
                        criteriaBuilder.PropertyEquals(TaxEntities.TaxCondition.Properties.TaxCode, auxTaxCode);
                        criteriaBuilder.And();
                        numberTax = numberTax + 1;
                    }
                    else
                    {
                        criteriaBuilder.And();
                    }

                    auxTaxCodeOne = auxTaxCode;

                    switch (Convert.ToString(dr["Description"]))
                    {
                        case "TAX_CONDITION_CODE":
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxCondition.Properties.TaxConditionCode, conditionCode);
                            break;
                        case "TAX_CATEGORY_CODE":
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxCategory.Properties.TaxCategoryCode, categoryCode);
                            break;
                        case "STATE_CODE":
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.StateCode, stateCode);
                            break;
                        case "COUNTRY_CODE":
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.CountryCode, countryCode);
                            break;
                        case "ECONOMIC_ACTIVITY_CODE":
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.EconomicActivityTaxCode, economicActivity);
                            break;
                        case "BRANCH_CODE":
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.BranchCode, branchCode);
                            break;
                        case "LINE_BUSINESS_CODE":
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.LineBusinessCode, lineBusinessCode);
                            break;
                    }
                }
                if (numberTax > 0)
                {
                    criteriaBuilder.CloseParenthesis();
                    numberTax = 0;
                }

                if (dataTableAttribute.Rows.Count > 0)
                {
                    criteriaBuilder.CloseParenthesis();
                }


                int rows;

                //Lista de impuestos
                UIView individualTaxes = _dataFacadeManager.GetDataFacade().GetView("IndividualTaxView", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out rows);

                #endregion 


                #region CalculateTax

                foreach (DataRow tax in individualTaxes.Rows)
                {
                    int baseConditionTaxCode = 0;

                    //Excepto retenciones
                    if (Convert.ToBoolean(tax["IsRetention"]) == false)
                    {
                        if (tax["BaseConditionTaxCode"] != DBNull.Value)
                        {
                            baseConditionTaxCode = Convert.ToInt32(tax["BaseConditionTaxCode"]);
                        }
                        // tax con base imponible
                        if (baseConditionTaxCode == 0)
                        {
                            int rateTypeCode = Convert.ToInt32(tax["RateTypeCode"]);
                            int taxCode = Convert.ToInt32(tax["TaxCode"]);
                            double rate = Convert.ToDouble(tax["Rate"]);
                            int taxConditionCode = Convert.ToInt32(tax["TaxConditionCode"]);
                            int enabled = Convert.ToInt32(tax["Enabled"]);
                            DateTime currentFrom = Convert.ToDateTime(tax["CurrentFrom"]);
                            double minBaseAmount = Convert.ToDouble(tax["MinBaseAmount"]);

                            if (enabled == 1 && currentFrom < DateTime.Now)
                            {
                                if (rateTypeCode == Entities.RateType.FIXED_VALUE)
                                {
                                    rate = rate / exchangeRate;
                                }

                                decimal excemptionPct = GetExemptionPercentage(individualId, taxCode, DateTime.Now, countryCode, stateCode);

                                // calcula si supera la base minima
                                double itemAmount = 0;
                                if (minBaseAmount <= amount && taxConditionCode == 1)
                                {
                                    itemAmount = Calculate(rateTypeCode, rate, amount, excemptionPct);
                                }

                                DataRow dataRowTax = dataTableTax.NewRow();
                                dataRowTax["TaxConditionCode"] = tax["TaxConditionCode"];
                                dataRowTax["TaxCategoryCode"] = tax["TaxCategoryCode"];
                                dataRowTax["TaxCode"] = taxCode;
                                dataRowTax["Tax"] = tax["Description"];
                                dataRowTax["Rate"] = rate;
                                dataRowTax["TaxAmountBase"] = amount;
                                dataRowTax["TaxValue"] = itemAmount;
                                dataTableTax.Rows.Add(dataRowTax);
                            }
                        }
                    }
                }

                #endregion

                #region CalculateTaxDep

                foreach (DataRow tax in individualTaxes.Rows)
                {
                    //Excepto retenciones
                    if (Convert.ToBoolean(tax["IsRetention"]) == false)
                    {
                        int baseConditionTaxCode = 0;
                        if (tax["BaseConditionTaxCode"] != DBNull.Value)
                        {
                            baseConditionTaxCode = Convert.ToInt32(tax["BaseConditionTaxCode"]);
                        }
                        // tax con base imponible
                        if (baseConditionTaxCode > 0)
                        {
                            int rateTypeCode = Convert.ToInt32(tax["RateTypeCode"]);
                            int taxCode = Convert.ToInt32(tax["TaxCode"]);
                            double rate = Convert.ToDouble(tax["Rate"]);
                            int taxConditionCode = Convert.ToInt32(tax["TaxConditionCode"]);
                            int enabled = Convert.ToInt32(tax["Enabled"]);
                            DateTime currentFrom = Convert.ToDateTime(tax["CurrentFrom"]);
                            double minBaseAmount = Convert.ToDouble(tax["MinBaseAmount"]);

                            if (enabled == 1 && currentFrom < DateTime.Now)
                            {
                                if (rateTypeCode == Entities.RateType.FIXED_VALUE)
                                {
                                    rate = rate / exchangeRate;
                                }

                                double itemAmount = 0;
                                bool taxBaseCondition = false;
                                foreach (DataRow row in dataTableTax.Rows)
                                {
                                    if (baseConditionTaxCode == Convert.ToInt32(row["TaxCode"]))
                                    {
                                        amount = Convert.ToDouble(row["TaxValue"]);

                                        decimal excemptionPct = GetExemptionPercentage(individualId, taxCode, DateTime.Now, countryCode, stateCode);

                                        // calcula si supera la base minima
                                        itemAmount = 0;
                                        if (minBaseAmount <= amount && taxConditionCode == 1)
                                        {
                                            itemAmount = Calculate(rateTypeCode, rate, amount, excemptionPct);
                                        }
                                        taxBaseCondition = true;
                                    }
                                }

                                if (!taxBaseCondition)
                                {
                                    amount = 0;
                                    itemAmount = 0;
                                }

                                DataRow dataRowTax = dataTableTax.NewRow();
                                dataRowTax["TaxConditionCode"] = tax["TaxConditionCode"];
                                dataRowTax["TaxCategoryCode"] = tax["TaxCategoryCode"];
                                dataRowTax["TaxCode"] = taxCode;
                                dataRowTax["Tax"] = tax["Description"];
                                dataRowTax["Rate"] = rate;
                                dataRowTax["TaxAmountBase"] = amount;
                                dataRowTax["TaxValue"] = itemAmount;
                                dataTableTax.Rows.Add(dataRowTax);
                            }
                        }
                    }
                }

                #endregion

                decimal totalTax = 0;

                foreach (DataRow dataRow in dataTableTax.Rows)
                {
                    totalTax = totalTax + Convert.ToDecimal(dataRow["TaxValue"]);
                }

                return totalTax;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetFactor
        /// </summary>
        /// <param name="rateTypeCode"></param>
        /// <returns>Decimal</returns>
        public static Decimal GetFactor(int rateTypeCode)
        {
            Decimal factor = new Decimal(1);
            Decimal divider = new Decimal(1);
            switch (rateTypeCode)
            {
                case 1:
                    divider = new Decimal(100);
                    break;
                case 2:
                    divider = new Decimal(1000);
                    break;
                case 3:
                    divider = new Decimal(1);
                    break;
            }

            return factor / divider;
        }

        #endregion
    }
}
