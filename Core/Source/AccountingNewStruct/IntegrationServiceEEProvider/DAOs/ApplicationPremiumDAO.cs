using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Integration.AccountingServices.DTOs.Accounting;
using Sistran.Core.Integration.AccountingServices.DTOs.Reversion;
using Sistran.Core.Integration.AccountingServices.EEProvider.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ACCEN = Sistran.Core.Application.Accounting.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;
namespace Sistran.Core.Integration.AccountingServices.EEProvider.DAOs
{
    /// <summary>
    /// Primas Aplicadas
    /// </summary>
    public class ApplicationPremiumDAO
    {
        /// <summary>
        /// Gets the application premium by endorsement filter.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        public static PaymentAppliedDTO GetApplicationPremiumByEndorsementFilter(int endorsementId)
        {
            PaymentAppliedDTO payer = new PaymentAppliedDTO();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ACCEN.ApplicationPremium.Properties.EndorsementCode);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(endorsementId);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.PayerCode, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.PayerCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.PaymentNum, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.PaymentNum));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.Amount, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.Amount));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.PremiumQuotaStatusCode, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.PremiumQuotaStatusCode));

            selectQuery.Table = new ClassNameTable(typeof(ACCEN.ApplicationPremium), typeof(ACCEN.ApplicationPremium).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            PaymentQuotaDTO financialPlanDTO = null;
            payer.Quotas = new List<PaymentQuotaDTO>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                payer.Quotas = reader.SelectReader(read => new PaymentQuotaDTO
                {
                    Id = Convert.ToInt32(read[ACCEN.ApplicationPremium.Properties.PayerCode]),
                    Number = Convert.ToInt32(read[ACCEN.ApplicationPremium.Properties.PaymentNum]),
                    Amount = Convert.ToDecimal(read[ACCEN.ApplicationPremium.Properties.Amount])
                }).ToList();
            }
            return payer;
        }

        /// <summary>
        /// Gets the application premium by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        public static List<QuotaPremiumDTO> GetApplicationPremiumByEndorsementId(int endorsementId)
        {
            //fgomez nase-1500
            List<QuotaPremiumDTO> quotaPremiumDTOs = new List<QuotaPremiumDTO>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ACCEN.ApplicationPremium.Properties.EndorsementCode);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(endorsementId);
            criteriaBuilder.And();
            criteriaBuilder.Property(ACCEN.ApplicationPremium.Properties.IsRev);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(false);
            criteriaBuilder.And();
            criteriaBuilder.Property(ACCEN.ApplicationPremium.Properties.Amount);
            criteriaBuilder.Distinct();//Greater
            criteriaBuilder.Constant(0);
          

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.AppPremiumCode, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.AppPremiumCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.Application.Properties.TechnicalTransaction, typeof(ACCEN.Application).Name), ACCEN.Application.Properties.TechnicalTransaction));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.PayerCode, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.PayerCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.PaymentNum, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.PaymentNum));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.Amount, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.Amount));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.LocalAmount, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.LocalAmount));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.AccountingDate, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.AccountingDate));
            Join join = new Join(new ClassNameTable(typeof(ACCEN.Application), typeof(ACCEN.Application).Name), new ClassNameTable(typeof(ACCEN.ApplicationPremium), typeof(ACCEN.ApplicationPremium).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(ACCEN.Application.Properties.ApplicationCode, typeof(ACCEN.Application).Name)
                    .Equal()
                    .Property(ACCEN.ApplicationPremium.Properties.AppCode, typeof(ACCEN.ApplicationPremium).Name)
                    .GetPredicate());
            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();
            QuotaPremiumDTO quotaPremiumDTO = null;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                quotaPremiumDTO = reader.SelectReader(read => new QuotaPremiumDTO
                {
                    Id = Convert.ToInt32(read[ACCEN.ApplicationPremium.Properties.AppPremiumCode]),
                    PayerId = Convert.ToInt32(read[ACCEN.ApplicationPremium.Properties.PayerCode]),
                    TechnicalTransaction = Convert.ToInt32(read[ACCEN.Application.Properties.TechnicalTransaction]),
                    Number = Convert.ToInt32(read[ACCEN.ApplicationPremium.Properties.PaymentNum]),
                    Amount = Convert.ToDecimal(read[ACCEN.ApplicationPremium.Properties.Amount]),
                    LocalAmount = Convert.ToDecimal(read[ACCEN.ApplicationPremium.Properties.LocalAmount]),
                    AccountingDate = Convert.ToDateTime(read[ACCEN.ApplicationPremium.Properties.AccountingDate])
                }).ToList().OrderByDescending(a => a.Id).FirstOrDefault();
            }
            if (quotaPremiumDTO != null)
            {
                quotaPremiumDTO.LocalAmountCommis = GetCommisionDiscount(quotaPremiumDTO.Id);
                quotaPremiumDTO.TotalAmount = Math.Abs((quotaPremiumDTO.LocalAmount * -1) + quotaPremiumDTO.LocalAmountCommis);
                quotaPremiumDTOs.Add(quotaPremiumDTO);
            }
            return quotaPremiumDTOs;
        }
        public static decimal GetCommisionDiscount(int premiumId)
        {
            decimal localAmountCommis = 0;
            Function function = new Function(FunctionType.Sum);
            function.AddParameter(new Column(ACCEN.ApplicationPremiumCommiss.Properties.LocalAmount, typeof(ACCEN.ApplicationPremiumCommiss).Name));
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ACCEN.ApplicationPremiumComponent.Properties.AppPremiumCode);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(premiumId);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(function));

            Join join = new Join(new ClassNameTable(typeof(ACCEN.ApplicationPremiumComponent), typeof(ACCEN.ApplicationPremiumComponent).Name), new ClassNameTable(typeof(ACCEN.ApplicationPremiumCommiss), typeof(ACCEN.ApplicationPremiumCommiss).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(ACCEN.ApplicationPremiumComponent.Properties.AppComponentCode, typeof(ACCEN.ApplicationPremiumComponent).Name)
                    .Equal()
                    .Property(ACCEN.ApplicationPremiumCommiss.Properties.AppComponentId, typeof(ACCEN.ApplicationPremiumCommiss).Name)
                    .GetPredicate());
            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                localAmountCommis = reader.SelectReader(read =>
                {
                    return Convert.ToDecimal(read[0]);
                }).FirstOrDefault();
            }
            return localAmountCommis;
        }

        /// <summary>
        /// Gets the application component by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        public static List<QuotaComponentsDTO> GetApplicationComponentByEndorsementId(int endorsementId)
        {
            List<QuotaComponentsDTO> quotaComponentsDTOs = new List<QuotaComponentsDTO>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ACCEN.ApplicationPremium.Properties.EndorsementCode);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(endorsementId);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremiumComponent.Properties.Amount, typeof(ACCEN.ApplicationPremiumComponent).Name), ACCEN.ApplicationPremiumComponent.Properties.Amount));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremiumComponent.Properties.ComponentCode, typeof(ACCEN.ApplicationPremiumComponent).Name), ACCEN.ApplicationPremiumComponent.Properties.ComponentCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.PayerCode, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.PayerCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.PaymentNum, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.PaymentNum));

            Join join = new Join(new ClassNameTable(typeof(ACCEN.ApplicationPremium), typeof(ACCEN.ApplicationPremium).Name), new ClassNameTable(typeof(ACCEN.ApplicationPremiumComponent), typeof(ACCEN.ApplicationPremiumComponent).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCEN.ApplicationPremium.Properties.AppPremiumCode, typeof(ACCEN.ApplicationPremium).Name)
                .Equal()
                .Property(ACCEN.ApplicationPremiumComponent.Properties.AppPremiumCode, typeof(ACCEN.ApplicationPremiumComponent).Name)
                .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();
            QuotaComponentsDTO quotaComponentsDTO = null;

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    quotaComponentsDTO = new QuotaComponentsDTO();
                    quotaComponentsDTO.Amount = Convert.ToDecimal(reader[ACCEN.ApplicationPremiumComponent.Properties.Amount]);
                    quotaComponentsDTO.ComponentId = Convert.ToInt32(reader[ACCEN.ApplicationPremiumComponent.Properties.ComponentCode]);
                    quotaComponentsDTO.PayerId = Convert.ToInt32(reader[ACCEN.ApplicationPremium.Properties.PayerCode]);
                    quotaComponentsDTO.QuotaNumber = Convert.ToInt32(reader[ACCEN.ApplicationPremium.Properties.PaymentNum]);
                    quotaComponentsDTOs.Add(quotaComponentsDTO);
                }
            }
            return quotaComponentsDTOs;
        }
        /// <summary>
        /// Gets the application component by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        public static List<PremiumLbSbModel> GetApplicationComponentLbSbByEndorsementId(int endorsementId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ACCEN.ApplicationPremium.Properties.EndorsementCode);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(endorsementId);
            List<PremiumLbSbModel> premiumLbSbModels = new List<PremiumLbSbModel>();
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.PayerCode, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.PayerCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremium.Properties.PaymentNum, typeof(ACCEN.ApplicationPremium).Name), ACCEN.ApplicationPremium.Properties.PaymentNum));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremiumComponent.Properties.ComponentCode, typeof(ACCEN.ApplicationPremiumComponent).Name), ACCEN.ApplicationPremiumComponent.Properties.ComponentCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremiumComponentLbsb.Properties.Amount, typeof(ACCEN.ApplicationPremiumComponentLbsb).Name), ACCEN.ApplicationPremiumComponentLbsb.Properties.Amount));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremiumComponentLbsb.Properties.LineBusinessCode, typeof(ACCEN.ApplicationPremiumComponentLbsb).Name), ACCEN.ApplicationPremiumComponentLbsb.Properties.LineBusinessCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.ApplicationPremiumComponentLbsb.Properties.SubLineBusinessCode, typeof(ACCEN.ApplicationPremiumComponentLbsb).Name), ACCEN.ApplicationPremiumComponentLbsb.Properties.SubLineBusinessCode));


            Join join = new Join(new ClassNameTable(typeof(ACCEN.ApplicationPremium), typeof(ACCEN.ApplicationPremium).Name), new ClassNameTable(typeof(ACCEN.ApplicationPremiumComponent), typeof(ACCEN.ApplicationPremiumComponent).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCEN.ApplicationPremium.Properties.AppPremiumCode, typeof(ACCEN.ApplicationPremium).Name)
                .Equal()
                .Property(ACCEN.ApplicationPremiumComponent.Properties.AppPremiumCode, typeof(ACCEN.ApplicationPremiumComponent).Name)
                .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(ACCEN.ApplicationPremiumComponentLbsb), typeof(ACCEN.ApplicationPremiumComponentLbsb).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCEN.ApplicationPremiumComponent.Properties.AppComponentCode, typeof(ACCEN.ApplicationPremiumComponent).Name)
                .Equal()
                .Property(ACCEN.ApplicationPremiumComponentLbsb.Properties.AppComponentCode, typeof(ACCEN.ApplicationPremiumComponentLbsb).Name)
                .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();
            PremiumLbSbModel premiumLbSbModel = new PremiumLbSbModel();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                premiumLbSbModels = reader.SelectReader(m => new PremiumLbSbModel
                {
                    PayerId = Convert.ToInt32(m[ACCEN.ApplicationPremium.Properties.PayerCode]),
                    QuotaNumber = Convert.ToInt32(m[ACCEN.ApplicationPremium.Properties.PaymentNum]),
                    ComponentId = Convert.ToInt32(m[ACCEN.ApplicationPremiumComponent.Properties.ComponentCode]),
                    Amount = Convert.ToDecimal(m[ACCEN.ApplicationPremiumComponentLbsb.Properties.Amount]),
                    Lb = Convert.ToInt32(m[ACCEN.ApplicationPremiumComponentLbsb.Properties.LineBusinessCode]),
                    Sb = Convert.ToInt32(m[ACCEN.ApplicationPremiumComponentLbsb.Properties.SubLineBusinessCode])
                }).ToList();
            }
            return premiumLbSbModels;
        }

        /// <summary>
        /// Gets the application premium base by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        public static List<PremiumBaseDTO> GetApplicationPremiumBaseByEndorsementId(int endorsementId)
        {
            List<PremiumBaseDTO> premiumBaseDTOs = new List<PremiumBaseDTO>();
            var paymentAppliedDTO = TP.Task.Run(() => GetApplicationPremiumByEndorsementFilter(endorsementId));
            var paymentAppliedCompDTO = TP.Task.Run(() => GetApplicationComponentByEndorsementId(endorsementId));
            var paymentAppliedComLbDTO = TP.Task.Run(() => GetApplicationComponentLbSbByEndorsementId(endorsementId));
            Task.WaitAll(paymentAppliedDTO, paymentAppliedCompDTO, paymentAppliedComLbDTO);
            foreach (IGrouping<int, PaymentQuotaDTO> paymentQuota in paymentAppliedDTO.Result.Quotas.GroupBy(g => g.Id))
            {
                PremiumBaseDTO premiumBaseDTO = new PremiumBaseDTO
                {
                    Id = paymentQuota.Key,
                    PayerId = paymentQuota.Key,
                    PremiumDTOs = paymentQuota.Select(a => new PremiumDTO { Number = a.Number, Amount = a.Amount, PremiumComponentDTOs = new List<PremiumComponentDTO>() }).ToList()
                };

                premiumBaseDTOs.Add(premiumBaseDTO);
                List<QuotaComponentsDTO> quotaComponentsDTOs = paymentAppliedCompDTO.Result.GroupBy(g => g.PayerId).Where(u => u.Key == premiumBaseDTO.PayerId).SelectMany(z => z.Select(a => a)).ToList();
                List<PremiumLbSbModel> premiumLbSbModels = paymentAppliedComLbDTO.Result.GroupBy(g => g.PayerId).Where(u => u.Key == premiumBaseDTO.PayerId).SelectMany(a => a.Select(b => b)).ToList();
                foreach (PremiumDTO q in premiumBaseDTO.PremiumDTOs)
                {
                    q.PremiumComponentDTOs = quotaComponentsDTOs.Where(a => a.QuotaNumber == q.Number).Select(r => new PremiumComponentDTO { Id = r.ComponentId, Amount = r.Amount }).ToList();
                    foreach (PremiumComponentDTO z in q.PremiumComponentDTOs)
                    {
                        z.PremiumComponentLbSbDTOs = premiumLbSbModels.Where(c => c.QuotaNumber == q.Number && c.ComponentId == z.Id).Select(r => new PremiumComponentLbSbDTO { Id = r.ComponentId, Amount = r.Amount, LbId = r.Lb, SbId = r.Sb }).ToList();
                    }
                }
                premiumBaseDTOs.Add(premiumBaseDTO);
            }

            return premiumBaseDTOs;
        }
    }
}
