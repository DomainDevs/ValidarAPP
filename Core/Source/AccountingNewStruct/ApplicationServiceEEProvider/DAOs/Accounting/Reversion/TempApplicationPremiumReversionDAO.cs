using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Reversion;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ACCEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting.Reversion
{
    public class TempApplicationPremiumReversionDAO
    {
        /// <summary>
        /// Gets the temporary application premium by temporary application identifier.
        /// </summary>
        /// <param name="tempAppId">The temporary application identifier.</param>
        /// <returns></returns>
        public static ApplicationPremiumTransaction GetTempApplicationPremiumByTempApplicationId(int tempAppId)
        {

            ApplicationPremiumTransaction tempPremiumReceivableTransaction = new ApplicationPremiumTransaction();
            tempPremiumReceivableTransaction.Id = tempAppId;
            tempPremiumReceivableTransaction.PremiumReceivableItems = new List<ApplicationPremiumTransactionItem>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCEN.TempApplicationPremiumRev.Properties.TempAppId, tempAppId);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.Table = new ClassNameTable(typeof(ACCEN.TempApplicationPremiumRev), typeof(ACCEN.TempApplicationPremiumRev).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.TempApplicationPremiumRev.Properties.TempAppId, typeof(ACCEN.TempApplicationPremiumRev).Name), ACCEN.TempApplicationPremiumRev.Properties.TempAppId));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.TempApplicationPremiumRev.Properties.AppPremiumId, typeof(ACCEN.TempApplicationPremiumRev).Name), ACCEN.TempApplicationPremiumRev.Properties.AppPremiumId));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.TempApplicationPremiumRev.Properties.LocalAmount, typeof(ACCEN.TempApplicationPremiumRev).Name), ACCEN.TempApplicationPremiumRev.Properties.LocalAmount));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.TempApplicationPremiumRev.Properties.LocalAmountCommis, typeof(ACCEN.TempApplicationPremiumRev).Name), ACCEN.TempApplicationPremiumRev.Properties.LocalAmountCommis));
            List<ApplicationPremiumTransactionItem> applicationPremiumRevs = new List<ApplicationPremiumTransactionItem>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                applicationPremiumRevs = reader.SelectReader(read => new ApplicationPremiumTransactionItem { Id = Convert.ToInt32(read[ACCEN.TempApplicationPremiumRev.Properties.AppPremiumId]), Amount = new Amount { Value = Convert.ToDecimal(read[ACCEN.TempApplicationPremiumRev.Properties.LocalAmount]) }, DeductCommission = new Amount { Value = Convert.ToDecimal(read[ACCEN.TempApplicationPremiumRev.Properties.LocalAmountCommis]) }, IsReversion = true }).ToList();
            }
            tempPremiumReceivableTransaction.PremiumReceivableItems = applicationPremiumRevs;
            return tempPremiumReceivableTransaction;
        }

        public static List<int> GetTempAppPremiumRevByTempAppId(int tempAppId)
        {
            List<int> temRevIds = new List<int>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCEN.TempApplicationPremiumRev.Properties.TempAppId, tempAppId);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.Table = new ClassNameTable(typeof(ACCEN.TempApplicationPremiumRev), typeof(ACCEN.TempApplicationPremiumRev).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.TempApplicationPremiumRev.Properties.TempAppPremiumRevId, typeof(ACCEN.TempApplicationPremiumRev).Name), ACCEN.TempApplicationPremiumRev.Properties.TempAppPremiumRevId));
            List<ApplicationPremiumTransactionItem> applicationPremiumRevs = new List<ApplicationPremiumTransactionItem>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                temRevIds = reader.SelectReader(read => Convert.ToInt32(read[ACCEN.TempApplicationPremiumRev.Properties.TempAppPremiumRevId])).ToList();
            }
            return temRevIds;
        }

        public static List<ReversionPremium> GetTempAppRevPremiumBytempAppId(int tempAppId)
        {
            List<ReversionPremium> temPremiums = new List<ReversionPremium>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCEN.TempApplicationPremiumRev.Properties.TempAppId, tempAppId);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.Table = new ClassNameTable(typeof(ACCEN.TempApplicationPremiumRev), typeof(ACCEN.TempApplicationPremiumRev).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.TempApplicationPremiumRev.Properties.LocalAmount, typeof(ACCEN.TempApplicationPremiumRev).Name), ACCEN.TempApplicationPremiumRev.Properties.LocalAmount));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.TempApplicationPremiumRev.Properties.LocalAmountCommis, typeof(ACCEN.TempApplicationPremiumRev).Name), ACCEN.TempApplicationPremiumRev.Properties.LocalAmountCommis));
            List<ApplicationPremiumTransactionItem> applicationPremiumRevs = new List<ApplicationPremiumTransactionItem>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                temPremiums = reader.SelectReader(read => new ReversionPremium { Premium = Convert.ToDecimal(read[ACCEN.TempApplicationPremiumRev.Properties.LocalAmount]), PremiumCommision = Convert.ToDecimal(read[ACCEN.TempApplicationPremiumRev.Properties.LocalAmountCommis]) }).ToList();
            }
            return temPremiums;
        }
    }
}