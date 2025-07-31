using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Integration.AccountingServices.DTOs.Accounting;
using System;
using System.Collections.Generic;
using System.Data;
using ACCEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Integration.AccountingServices.EEProvider.DAOs
{
    public class ApplicationPremiumDAO
    {
        public static PaymentPlanDTO GetApplicationPremiumByEndorsementFilter(int endorsementId)
        {
            PaymentPlanDTO payer = new PaymentPlanDTO();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ACCEN.ApplicationPremium.Properties.EndorsementCode);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(endorsementId);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.PayerCode, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.PayerCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.PaymentNum, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.PaymentNum));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.AccountingDate, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.AccountingDate));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.Amount, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.Amount));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.PremiumQuotaStatusCode, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.PremiumQuotaStatusCode));

            selectQuery.Table = new ClassNameTable(typeof(ACCEN.ApplicationPremium), typeof(ACCEN.ApplicationPremium).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            QuotaDTO financialPlanDTO = null;
            payer.Quotas = new List<QuotaDTO>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    financialPlanDTO = new QuotaDTO();
                    financialPlanDTO.Id = Convert.ToInt32(reader[ACCEN.ApplicationPremium.Properties.PayerCode]);
                    financialPlanDTO.Number = Convert.ToInt32(reader[ACCEN.ApplicationPremium.Properties.PaymentNum]);
                    financialPlanDTO.ExpirationDate = Convert.ToDateTime(reader[ACCEN.ApplicationPremium.Properties.AccountingDate]);
                    financialPlanDTO.Amount = Convert.ToInt32(reader[ACCEN.ApplicationPremium.Properties.Amount]);
                    payer.Quotas.Add(financialPlanDTO);
                }
            }
            return payer;
        }
    }
}
