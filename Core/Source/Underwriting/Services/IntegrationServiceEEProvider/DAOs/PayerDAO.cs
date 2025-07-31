using Sistran.Co.Application.Data;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;
namespace Sistran.Core.Integration.UnderwritingServices.EEProvider.DAOs
{
    /// <summary>
    /// Pagadores
    /// </summary>
    public class PayerDAO
    {
        /// <summary>
        /// Gets the payer by endorsement filter.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        public static List<PayerBaseDTO> GetPayersByEndorsementFilter(FilterBaseDTO filterBase)
        {
            List<PayerBaseDTO> payers = new List<PayerBaseDTO>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ISSEN.EndorsementPayer.Properties.EndorsementId);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Id);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.EndorsementPayer.Properties.PayerId, typeof(ISSEN.EndorsementPayer).Name), ISSEN.EndorsementPayer.Properties.PayerId));
            selectQuery.Table = new ClassNameTable(typeof(ISSEN.EndorsementPayer), typeof(ISSEN.EndorsementPayer).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    payers.Add(new PayerBaseDTO { Id = Convert.ToInt32(reader[ISSEN.EndorsementPayer.Properties.PayerId]) });
                }
            }
            payers.ForEach((m) =>
            {
                m.Description = PersonDAO.GetNameByIndividualId(Convert.ToInt32(m.Id));
            });
            return payers;
        }

        /// <summary>
        /// Gets the payer payment by endorsement filter.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        public static List<FinancialPlanDTO> GetPayerPaymentByEndorsementFilter(FilterBaseDTO filterBase, bool includeComp = true)
        {
            List<FinancialPlanDTO> payers = new List<FinancialPlanDTO>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ISSEN.PayerPayment.Properties.EndorsementId);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Id);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.PayerId, typeof(ISSEN.PayerPayment).Name), ISSEN.PayerPayment.Properties.PayerId));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.PaymentNum, typeof(ISSEN.PayerPayment).Name), ISSEN.PayerPayment.Properties.PaymentNum));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.PayExpDate, typeof(ISSEN.PayerPayment).Name), ISSEN.PayerPayment.Properties.PayExpDate));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.Amount, typeof(ISSEN.PayerPayment).Name), ISSEN.PayerPayment.Properties.Amount));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.PaymentState, typeof(ISSEN.PayerPayment).Name), ISSEN.PayerPayment.Properties.PaymentState));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.PayerPaymentId, typeof(ISSEN.PayerPayment).Name), ISSEN.PayerPayment.Properties.PayerPaymentId));

            selectQuery.Table = new ClassNameTable(typeof(ISSEN.PayerPayment), typeof(ISSEN.PayerPayment).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            FinancialPlanDTO financialPlanDTO = null;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    financialPlanDTO = new FinancialPlanDTO();
                    financialPlanDTO.Id = Convert.ToInt32(reader[ISSEN.PayerPayment.Properties.PayerId]);
                    financialPlanDTO.Number = Convert.ToInt32(reader[ISSEN.PayerPayment.Properties.PaymentNum]);
                    financialPlanDTO.ExpirationDate = Convert.ToDateTime(reader[ISSEN.PayerPayment.Properties.PayExpDate]);
                    financialPlanDTO.Amount = Convert.ToDecimal(reader[ISSEN.PayerPayment.Properties.Amount]);
                    financialPlanDTO.StateQuota = Convert.ToInt16(reader[ISSEN.PayerPayment.Properties.PaymentState]);
                    financialPlanDTO.PayerPaymentId = Convert.ToInt32(reader[ISSEN.PayerPayment.Properties.PayerPaymentId]);
                    payers.Add(financialPlanDTO);
                }
            }
            if (includeComp)
            {
                List<QuotaComponentDTO> components = GetPayerPaymentComponentByEndorsementFilter(filterBase);
                List<ComponentTypeDTO> componentTypes = PayerComponentDAO.GetComponentTypes();
                payers.ForEach(a =>
                            {

                                a.Premium = decimal.Round(components.Where(m => m.Id == a.PayerPaymentId && componentTypes.Where(u => u.Id == (int)ComponentType.Premium).Select(p => p.ComponentId).Contains(m.ComponentId)).Sum(z => z.Amount), QuoteManager.DecimalRound);
                                a.Expenses = decimal.Round(components.Where(m => m.Id == a.PayerPaymentId && componentTypes.Where(u => u.Id == (int)ComponentType.Expenses).Select(p => p.ComponentId).Contains(m.ComponentId)).Sum(z => z.Amount), QuoteManager.DecimalRound);
                                a.Tax = decimal.Round(components.Where(m => m.Id == a.PayerPaymentId && componentTypes.Where(u => u.Id == (int)ComponentType.Taxes).Select(p => p.ComponentId).Contains(m.ComponentId)).Sum(z => z.Amount), QuoteManager.DecimalRound);
                            });
            }
            return payers;
        }

        /// <summary>
        /// Gets the payer payment component by endorsement filter.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        public static List<QuotaComponentDTO> GetPayerPaymentComponentByEndorsementFilter(FilterBaseDTO filterBase)
        {
            List<QuotaComponentDTO> payers = new List<QuotaComponentDTO>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ISSEN.PayerPayment.Properties.EndorsementId);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Id);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPaymentComp.Properties.PayerPaymentId, typeof(ISSEN.PayerPaymentComp).Name), ISSEN.PayerPaymentComp.Properties.PayerPaymentId));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPaymentComp.Properties.ComponentCode, typeof(ISSEN.PayerPaymentComp).Name), ISSEN.PayerPaymentComp.Properties.ComponentCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPaymentComp.Properties.Amount, typeof(ISSEN.PayerPaymentComp).Name), ISSEN.PayerPaymentComp.Properties.Amount));

            Join join = new Join(new ClassNameTable(typeof(ISSEN.PayerPayment), typeof(ISSEN.PayerPayment).Name), new ClassNameTable(typeof(ISSEN.PayerPaymentComp), typeof(ISSEN.PayerPaymentComp).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.PayerPayment.Properties.PayerPaymentId, typeof(ISSEN.PayerPayment).Name)
                .Equal()
                .Property(ISSEN.PayerPaymentComp.Properties.PayerPaymentId, typeof(ISSEN.PayerPaymentComp).Name)
                .GetPredicate());
            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();
            QuotaComponentDTO financialPlanDTO = null;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    financialPlanDTO = new QuotaComponentDTO();
                    financialPlanDTO.Id = Convert.ToInt32(reader[ISSEN.PayerPaymentComp.Properties.PayerPaymentId]);
                    financialPlanDTO.ComponentId = Convert.ToInt32(reader[ISSEN.PayerPaymentComp.Properties.ComponentCode]);
                    financialPlanDTO.Amount = Convert.ToDecimal(reader[ISSEN.PayerPaymentComp.Properties.Amount]);
                    payers.Add(financialPlanDTO);
                }
            }
            return payers;
        }
        /// <summary>
        /// Gets the payer payment component lb sb by endorsement filter.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        public static List<QuotaComponentLbSbDTO> GetPayerPaymentComponentLbSbByEndorsementFilter(FilterBaseDTO filterBase)
        {
            List<QuotaComponentLbSbDTO> payers = new List<QuotaComponentLbSbDTO>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ISSEN.PayerPayment.Properties.EndorsementId);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Id);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPaymentComp.Properties.PayerPaymentId, typeof(ISSEN.PayerPaymentComp).Name), ISSEN.PayerPaymentComp.Properties.PayerPaymentId));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPaymentComp.Properties.ComponentCode, typeof(ISSEN.PayerPaymentComp).Name), ISSEN.PayerPaymentComp.Properties.ComponentCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPaymentComp.Properties.Amount, typeof(ISSEN.PayerPaymentComp).Name), ISSEN.PayerPaymentComp.Properties.Amount));

            Join join = new Join(new ClassNameTable(typeof(ISSEN.PayerPayment), typeof(ISSEN.PayerPayment).Name), new ClassNameTable(typeof(ISSEN.PayerPaymentComp), typeof(ISSEN.PayerPaymentComp).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.PayerPayment.Properties.PayerPaymentId, typeof(ISSEN.PayerPayment).Name)
                .Equal()
                .Property(ISSEN.PayerPaymentComp.Properties.PayerPaymentId, typeof(ISSEN.PayerPaymentComp).Name)
                .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(ISSEN.PayerPaymentCompLbsb), typeof(ISSEN.PayerPaymentCompLbsb).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.PayerPayment.Properties.PayerPaymentId, typeof(ISSEN.PayerPayment).Name)
                .Equal()
                .Property(ISSEN.PayerPaymentCompLbsb.Properties.PayerPaymentId, typeof(ISSEN.PayerPaymentComp).Name)
                .GetPredicate());
            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();
            QuotaComponentLbSbDTO financialPlanDTO = null;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    financialPlanDTO = new QuotaComponentLbSbDTO();
                    financialPlanDTO.Id = Convert.ToInt32(reader[ISSEN.PayerPaymentComp.Properties.PayerPaymentId]);
                    financialPlanDTO.ComponentId = Convert.ToInt32(reader[ISSEN.PayerPaymentComp.Properties.ComponentCode]);
                    financialPlanDTO.LbId = Convert.ToInt32(reader[ISSEN.PayerPaymentCompLbsb.Properties.LineBusinessCode]);
                    financialPlanDTO.SbId = Convert.ToInt32(reader[ISSEN.PayerPaymentCompLbsb.Properties.LineBusinessCode]);
                    financialPlanDTO.Amount = Convert.ToDecimal(reader[ISSEN.PayerPaymentCompLbsb.Properties.Amount]);
                    payers.Add(financialPlanDTO);
                }
            }
            return payers;
        }
        /// <summary>
        /// Gets the quotas by enddorsement identifier.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        public static QuotaAll GetQuotasByEndorsementId(FilterBaseDTO filterBase)
        {
            var paymentDTOs = TP.Task.Run(() => GetPayerPaymentByEndorsementFilter(filterBase));
            var paymentCompDTOs = TP.Task.Run(() => GetPayerPaymentComponentByEndorsementFilter(filterBase));
            var paymentComLbDTOs = TP.Task.Run(() => GetPayerPaymentComponentLbSbByEndorsementFilter(filterBase));
            Task.WaitAll(paymentDTOs, paymentCompDTOs, paymentComLbDTOs);
            QuotaAll quotaAll = new QuotaAll();
            quotaAll.FinancialPlanDTOs = paymentDTOs.Result;
            quotaAll.QuotaComponentDTOs = paymentCompDTOs.Result;
            quotaAll.QuotaComponentLbSbDTOs = paymentComLbDTOs.Result;
            return quotaAll;
        }

        public static List<ComponentLbSb> GetLbSbByEndorsementId(FilterBaseDTO filterBase)
        {
            Function function = new Function(FunctionType.Sum);
            function.AddParameter(new Column(ISSEN.PayerComp.Properties.ComponentAmount, typeof(ISSEN.PayerComp).Name));
            List<ComponentLbSb> componentLbSbs = new List<ComponentLbSb>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ISSEN.PayerComp.Properties.EndorsementId);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Id);
            criteriaBuilder.And();
            criteriaBuilder.Property(ISSEN.PayerComp.Properties.ComponentAmount);
            criteriaBuilder.Distinct();
            criteriaBuilder.Constant(0);
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.Coverage.Properties.LineBusinessCode, typeof(QUOEN.Coverage).Name), QUOEN.Coverage.Properties.LineBusinessCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.Coverage.Properties.SubLineBusinessCode, typeof(QUOEN.Coverage).Name), QUOEN.Coverage.Properties.SubLineBusinessCode));
            selectQuery.AddSelectValue(new SelectValue(function));
            Join join = new Join(new ClassNameTable(typeof(ISSEN.PayerComp), typeof(ISSEN.PayerComp).Name), new ClassNameTable(typeof(QUOEN.Coverage), typeof(QUOEN.Coverage).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.PayerComp.Properties.CoverageId, typeof(ISSEN.PayerComp).Name)
                .Equal()
                .Property(QUOEN.Coverage.Properties.CoverageId, typeof(QUOEN.Coverage).Name)
                .GetPredicate());
            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();
            selectQuery.AddGroupValue(new Column(QUOEN.Coverage.Properties.LineBusinessCode, typeof(QUOEN.Coverage).Name));
            selectQuery.AddGroupValue(new Column(QUOEN.Coverage.Properties.SubLineBusinessCode, typeof(QUOEN.Coverage).Name));
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                componentLbSbs = reader.SelectReader(read => new ComponentLbSb { LineBusiness = Convert.ToInt32(read[QUOEN.Coverage.Properties.LineBusinessCode]), SubLineBusiness = Convert.ToInt32(read[QUOEN.Coverage.Properties.SubLineBusinessCode]), Amount = Convert.ToDecimal(read[2]) }).ToList();
            }
            return componentLbSbs;
        }

        /// <summary>
        /// Creates the endorsement quotas.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <param name="sequentialId">The sequential identifier.</param>
        /// <returns></returns>
        public static bool CreateEndorsementQuotas(int endorsementId, int sequentialId)
        {
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("ENDORSEMENT_ID", endorsementId);
            parameters[1] = new NameValue("SEQUENTIAL_ID", sequentialId);
            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("ISS.CREATE_PAYERS_PAYMENT", parameters);
            }
            bool stateResult = false; ;
            if (result != null && result.Rows.Count > 0)
            {
                Parallel.ForEach(result.Rows.Cast<DataRow>(), (drow, state) =>
                 {
                     stateResult = Convert.ToBoolean(drow[0]);
                     state.Stop();
                 });
            }
            return stateResult;
        }
    }
}
