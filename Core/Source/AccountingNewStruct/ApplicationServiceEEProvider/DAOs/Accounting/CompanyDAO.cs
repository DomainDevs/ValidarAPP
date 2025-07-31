using System.Collections.Generic;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using UniquePersonModels = Sistran.Core.Application.UniquePersonService.V1.Models;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
   public class CompanyDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveCompany
        /// </summary>
        /// <param name="company"></param>
        /// <returns>Company</returns>
        public UniquePersonModels.Company SaveCompany(UniquePersonModels.Company company)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.Company accountingCompanyEntity = EntityAssembler.CreateAccountingCompany(company);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(accountingCompanyEntity);

                // Return del model
                return ModelAssembler.CreateAccountingCompany(accountingCompanyEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateCompany
        /// </summary>
        /// <param name="company"></param>
        /// <returns>Company</returns>
        public UniquePersonModels.Company UpdateCompany(UniquePersonModels.Company company)
        {
            try
            {
                // Crea el Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Company.CreatePrimaryKey(company.IndividualId);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.Company companyEntity = (ACCOUNTINGEN.Company)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                companyEntity.Description = company.FullName;

                // Realiza las operaciones con las entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(companyEntity);

                // Return del model
                return ModelAssembler.CreateAccountingCompany(companyEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteCompany
        /// </summary>
        /// <param name="accountingCompanyId"></param>
        /// <returns>bool</returns>
        public bool DeleteCompany(int accountingCompanyId)
        {
            bool isDeleted = false;

            try
            {
                // Crea el Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Company.CreatePrimaryKey(accountingCompanyId);

                // Realiza las operaciones con las entities utilizando DAF
                ACCOUNTINGEN.Company companyEntity = (ACCOUNTINGEN.Company)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                _dataFacadeManager.GetDataFacade().DeleteObject(companyEntity);

                isDeleted = true;
            }
            catch (BusinessException)
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        /// <summary>
        /// GetCompany
        /// </summary>
        /// <param name="company"></param>
        /// <returns>Company</returns>
        public UniquePersonModels.Company GetCompany(UniquePersonModels.Company company)
        {
            try
            {
                // Crea el Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Company.CreatePrimaryKey(company.IndividualId);

                // Realiza las operaciones con las entities utilizando DAF
                ACCOUNTINGEN.Company companyEntity = (ACCOUNTINGEN.Company)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return model
                return ModelAssembler.CreateAccountingCompany(companyEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCompanies
        /// </summary>
        /// <returns>List<Company/></returns>
        public List<UniquePersonModels.Company> GetCompanies()
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(ACCOUNTINGEN.Company.Properties.CompanyId);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);

                // Se asigna BusinessCollection a una lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Company), criteriaBuilder.GetPredicate()));

                // Return Lista
                return ModelAssembler.CreateAccountingCompany(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
