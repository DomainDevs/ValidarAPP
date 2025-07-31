using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    public class ApplicationPremiumCommisionDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        public List<ApplicationPremiumCommision> GetApplicationPremiumCommisions()
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                                                   typeof(ACCOUNTINGEN.TempApplicationPremiumCommiss)));
                return ModelAssembler.CreateApplicacionPremiumCommission(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        public List<ApplicationPremiumCommision> GetApplicationPremiumCommissByAppPremiumId(int appPremiumId)
        {
            try
            {
                
                ObjectCriteriaBuilder criteriaBuilderComponent = new ObjectCriteriaBuilder();
                criteriaBuilderComponent.Property(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.AppPremiumCode);
                criteriaBuilderComponent.Equal();
                criteriaBuilderComponent.Constant(appPremiumId);
                int appComponentId = 0;
                var businessCollection = DataFacadeManager.GetObjects(typeof(ACCOUNTINGEN.ApplicationPremiumComponent), criteriaBuilderComponent.GetPredicate());
                if (businessCollection != null )
                {
                    appComponentId = ModelAssembler.CreateApplicationPremiumComponent(businessCollection.OfType<ACCOUNTINGEN.ApplicationPremiumComponent>().Where(x => x.ComponentCode == 1).FirstOrDefault()).AppComponentId;
                }
                ObjectCriteriaBuilder criteriaBuilderCommiss = new ObjectCriteriaBuilder();
                criteriaBuilderCommiss.Property(ACCOUNTINGEN.ApplicationPremiumCommiss.Properties.AppComponentId);
                criteriaBuilderCommiss.Equal();
                criteriaBuilderCommiss.Constant(appComponentId);

                var businessObjects = DataFacadeManager.GetObjects(typeof(ACCOUNTINGEN.ApplicationPremiumCommiss), criteriaBuilderCommiss.GetPredicate());

                return ModelAssembler.CretaApplicationPremiumCommision(businessObjects);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<ApplicationPremiumCommision> CreateApplicationPremiumCommisions(List<ApplicationPremiumCommision> applicationPremiumCommision)
        {
            try
            {
                List<ApplicationPremiumCommision> applicationPremiumCommisions = new List<ApplicationPremiumCommision>();
                applicationPremiumCommision.ForEach( x => {
                    applicationPremiumCommisions.Add(CreateApplicationPremiumCommision(x));
                    });
                return applicationPremiumCommisions;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        public ApplicationPremiumCommision CreateApplicationPremiumCommision(ApplicationPremiumCommision applicationPremiumCommision)
        {
            try
            {
                ACCOUNTINGEN.ApplicationPremiumCommiss entityApplicationPremiumCommiss = EntityAssembler.CreateApplicationPremiumCommission(applicationPremiumCommision);
                _dataFacadeManager.GetDataFacade().InsertObject(entityApplicationPremiumCommiss);
                return ModelAssembler.CreateApplicacionPremiumCommission(entityApplicationPremiumCommiss);

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        public ApplicationPremiumCommision UpdateApplicationPremiumCommision(ApplicationPremiumCommision applicationPremiumCommision)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.ApplicationPremiumCommiss.CreatePrimaryKey(applicationPremiumCommision.Id);

                ACCOUNTINGEN.ApplicationPremiumCommiss entityApplicationPremiumCommiss = (ACCOUNTINGEN.ApplicationPremiumCommiss)
                                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                _dataFacadeManager.GetDataFacade().InsertObject(entityApplicationPremiumCommiss);
                return ModelAssembler.CreateApplicacionPremiumCommission(entityApplicationPremiumCommiss);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public bool DeleteApplicationPremiumCommision(int id)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.ApplicationPremiumCommiss.CreatePrimaryKey(id);

                ACCOUNTINGEN.ApplicationPremiumCommiss entityApplicationPremiumCommiss = (ACCOUNTINGEN.ApplicationPremiumCommiss)
                                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                _dataFacadeManager.GetDataFacade().DeleteObject(entityApplicationPremiumCommiss);
                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<ApplicationPremiumCommision> GetTempApplicationPremiumCommisses()
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                                                   typeof(ACCOUNTINGEN.TempApplicationPremiumCommiss)));

                return ModelAssembler.CreateApplicacionPremiumCommission(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<ApplicationPremiumCommision> GetTempApplicationPremiumCommissByTempAppId(int tempAppPremiumId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.TempApplicationPremiumCommiss.Properties.TempAppPremiumId);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(tempAppPremiumId);

                var businessCollection = DataFacadeManager.GetObjects(typeof(ACCOUNTINGEN.TempApplicationPremiumCommiss), criteriaBuilder.GetPredicate());

                return ModelAssembler.CretaTempApplicationPremiumCommision(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        public ApplicationPremiumCommision CreateTempApplicationPremiumCommisses(ApplicationPremiumCommision tempApplicationPremiumCommiss)
        {
            try
            {
                ACCOUNTINGEN.TempApplicationPremiumCommiss entityTempApplicationPremiumCommis = EntityAssembler.CreateTempApplicationPremiumCommission(tempApplicationPremiumCommiss);
                _dataFacadeManager.GetDataFacade().InsertObject(entityTempApplicationPremiumCommis);
                return ModelAssembler.CretaTempApplicationPremiumCommision(entityTempApplicationPremiumCommis);

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        public ApplicationPremiumCommision CreateTempApplicationPremiumCommisses(TempApplicationPremiumCommiss tempApplicationPremiumCommiss)
        {
            try
            {
                ACCOUNTINGEN.TempApplicationPremiumCommiss entityTempApplicationPremiumCommis = EntityAssembler.CreateTempApplicationPremiumCommission(tempApplicationPremiumCommiss);
                _dataFacadeManager.GetDataFacade().InsertObject(entityTempApplicationPremiumCommis);
                return ModelAssembler.CretaTempApplicationPremiumCommision(entityTempApplicationPremiumCommis);

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }        
        public ApplicationPremiumCommision UpdateTempApplicationPremiumCommisses(TempApplicationPremiumCommiss tempApplicationPremiumCommiss)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplicationPremiumCommiss.CreatePrimaryKey(tempApplicationPremiumCommiss.Id);

                ACCOUNTINGEN.TempApplicationPremiumCommiss entityTempApplicationPremiumCommiss = (ACCOUNTINGEN.TempApplicationPremiumCommiss)
                                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                entityTempApplicationPremiumCommiss.CurrencyCode = tempApplicationPremiumCommiss.Currency;
                entityTempApplicationPremiumCommiss.ExchangeRate = tempApplicationPremiumCommiss.ExchangeRate;
                entityTempApplicationPremiumCommiss.Amount = tempApplicationPremiumCommiss.Amount;
                entityTempApplicationPremiumCommiss.LocalAmount = tempApplicationPremiumCommiss.LocalAmount;

                _dataFacadeManager.GetDataFacade().UpdateObject(entityTempApplicationPremiumCommiss);

                return ModelAssembler.CretaTempApplicationPremiumCommision(entityTempApplicationPremiumCommiss);

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        public bool DeleteTempApplicationPremiumCommisses(int id)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.TempApplicationPremiumCommiss.Properties.TempAppPremiumId);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(id);

                BusinessCollection collectCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.TempApplicationPremiumCommiss), criteriaBuilder.GetPredicate()));
                
                collectCollection.Select(m => DataFacadeManager.Delete(m.PrimaryKey));                

                return true;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
