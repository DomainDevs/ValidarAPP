using System;
using System.Collections.Generic;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Linq;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    class ApplicationPremiumItemDAO
    {

        ///<summary>
        /// SavePremiumRecievableTransactionItem
        /// </summary>
        /// <param name="premiumRecievableTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="registerDate"></param>
        /// <returns>PremiumReceivableTransactionItem</returns>
        /// 
        public PremiumReceivableTransactionItem SavePremiumRecievableTransactionItem(PremiumReceivableTransactionItem premiumRecievableTransactionItem, int imputationId, decimal exchangeRate, DateTime registerDate, DateTime accountingDate)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.ApplicationPremium entityApplicationPremium = EntityAssembler.CreatePremiumReceivableApplication(premiumRecievableTransactionItem, imputationId, exchangeRate, registerDate, accountingDate);

                // Realizar las operaciones con los entities utilizando DAF
                DataFacadeManager.Insert(entityApplicationPremium);

                return ModelAssembler.CreatePremiumReceivableTransactionItem(entityApplicationPremium);
                //CreatePremiumReceivableTransactionItem
                //return default(PremiumReceivableTransactionItem);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// SavePremiumRecievableTransactionItem
        /// </summary>
        /// <param name="premiumRecievableTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="registerDate"></param>
        /// <returns>PremiumReceivableTransactionItem</returns>
        /// 
        public PremiumReceivableTransactionItem SavePremiumRecievableTransactionItem(ACCOUNTINGEN.TempApplicationPremium tempApplicationPremium, int status)
        {
            try
            {                
                // Convertir de model a entity
                ACCOUNTINGEN.ApplicationPremium entityApplicationPremium = EntityAssembler.CreateApplicationPremium(tempApplicationPremium, status);

                // Realizar las operaciones con los entities utilizando DAF
                DataFacadeManager.Insert(entityApplicationPremium);

                return ModelAssembler.CreatePremiumReceivableTransactionItem(entityApplicationPremium);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeletePremiumRecievableTransactionItem
        /// </summary>
        /// <param name="applicationPremiumId"></param>
        public void DeleteApplicationPremium(int applicationPremiumId)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.ApplicationPremium.CreatePrimaryKey(applicationPremiumId);
                DataFacadeManager.Delete(primaryKey);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPremiumRecievableTransactionItem
        /// </summary>
        /// <param name="premiumRecievableTransactionItem"> </param>
        /// <returns>PremiumReceivableTransactionItem</returns>
        public PremiumReceivableTransactionItem GetPremiumRecievableTransactionItem(PremiumReceivableTransactionItem premiumRecievableTransactionItem)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.ApplicationPremium.CreatePrimaryKey(premiumRecievableTransactionItem.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.ApplicationPremium entityApplicationPremium = (ACCOUNTINGEN.ApplicationPremium)
                    (DataFacadeManager.GetObject(primaryKey));

                // Return del model
                //return ModelAssembler.CreatePremiumReceivableTransactionItem(entityApplicationPremium);
               return default(PremiumReceivableTransactionItem);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<Models.Imputations.ApplicationPremium> GetApplicationsByFilter(List<Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.PremiumSearchPolicyDTO> premiums)
        {

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.IsRev, 0);
            criteriaBuilder.And();
            criteriaBuilder.Property(ACCOUNTINGEN.ApplicationPremium.Properties.EndorsementCode);
            criteriaBuilder.In();
            criteriaBuilder.ListValue();
            premiums.ForEach(x =>
            {
                criteriaBuilder.Constant(Convert.ToInt32(x.EndorsementId));
            });
            criteriaBuilder.EndList();

            List<ACCOUNTINGEN.ApplicationPremium> entityApplicationPremiums = DataFacadeManager.GetObjects(
                typeof(ACCOUNTINGEN.ApplicationPremium), criteriaBuilder.GetPredicate()).
                Cast<ACCOUNTINGEN.ApplicationPremium>().ToList();

            return ModelAssembler.CreateApplicationPremiums(entityApplicationPremiums);
        }

        public ApplicationPremium SaveApplicationPremium(ApplicationPremium applicationPremium)
        {

            // Convertir de model a entity
            ACCOUNTINGEN.ApplicationPremium entityApplicationPremium = EntityAssembler.CreateApplicationPremium(applicationPremium);

            // Realizar las operaciones con los entities utilizando DAF
            DataFacadeManager.Insert(entityApplicationPremium);

            return ModelAssembler.CreateApplicationPremium(entityApplicationPremium);
        }

        public List<Models.Imputations.ApplicationPremium> GetApplicationPremiumsByEndorsementId(int endorsementId)
        {

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.IsRev, 0);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.EndorsementCode, endorsementId);

            List<ACCOUNTINGEN.ApplicationPremium> entityApplicationPremiums = DataFacadeManager.GetObjects(
                typeof(ACCOUNTINGEN.ApplicationPremium), criteriaBuilder.GetPredicate()).
                Cast<ACCOUNTINGEN.ApplicationPremium>().ToList();

            return ModelAssembler.CreateApplicationPremiums(entityApplicationPremiums);
        }
    }
}
