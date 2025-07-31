using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Base;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;

namespace Sistran.Core.Integration.UnderwritingServices.EEProvider.DAOs
{
    /// <summary>
    /// Planes de pago
    /// </summary>
    public class PayerPaymentDAO
    {
        /// <summary>
        /// Gets the payments schedule by product identifier.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        public static List<ComboBaseDTO> GetPaymentsScheduleByProductId(FilterBaseDTO filterBase)
        {
            List<ComboBaseDTO> comboDTOs = new List<ComboBaseDTO>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(PRODEN.ProductFinancialPlan.Properties.ProductId, typeof(PRODEN.ProductFinancialPlan).Name);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Id);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.PaymentSchedule.Properties.PaymentScheduleId, typeof(PRODEN.PaymentSchedule).Name), PRODEN.PaymentSchedule.Properties.PaymentScheduleId));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.PaymentSchedule.Properties.Description, typeof(PRODEN.PaymentSchedule).Name), PRODEN.PaymentSchedule.Properties.Description));

            Join join = new Join(new ClassNameTable(typeof(PRODEN.PaymentSchedule), typeof(PRODEN.PaymentSchedule).Name), new ClassNameTable(typeof(PRODEN.PaymentSchedule), typeof(PRODEN.PaymentSchedule).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
              .Property(PRODEN.PaymentSchedule.Properties.PaymentScheduleId, typeof(PRODEN.PaymentSchedule).Name)
              .Equal()
              .Property(PRODEN.PaymentSchedule.Properties.PaymentScheduleId, typeof(PRODEN.PaymentSchedule).Name)
              .GetPredicate());

            selectQuery.Table = new ClassNameTable(typeof(PRODEN.PaymentSchedule), typeof(PRODEN.PaymentSchedule).Name);
            //selectQuery.Where = criteriaBuilder.GetPredicate();
            ComboBaseDTO comboDTO = null;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    comboDTO = new ComboBaseDTO();
                    comboDTO.Id = Convert.ToInt32(reader[PRODEN.PaymentSchedule.Properties.PaymentScheduleId]);
                    comboDTO.Description = Convert.ToString(reader[PRODEN.PaymentSchedule.Properties.Description]);
                    comboDTOs.Add(comboDTO);
                }
            }
            return comboDTOs;
        }

        public static PaymentsScheduleDTO GetPaymentsScheduleBySheduleId(FilterBaseDTO filterBase)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(PRODEN.PaymentSchedule.Properties.PaymentScheduleId, typeof(PRODEN.PaymentSchedule).Name);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Id);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.PaymentSchedule.Properties.PaymentQuantity, typeof(PRODEN.PaymentSchedule).Name), PRODEN.PaymentSchedule.Properties.PaymentQuantity));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.PaymentSchedule.Properties.GapUnitCode, typeof(PRODEN.PaymentSchedule).Name), PRODEN.PaymentSchedule.Properties.GapUnitCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.PaymentSchedule.Properties.GapQuantity, typeof(PRODEN.PaymentSchedule).Name), PRODEN.PaymentSchedule.Properties.GapQuantity));

            selectQuery.Table = new ClassNameTable(typeof(PRODEN.PaymentSchedule), typeof(PRODEN.PaymentSchedule).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            PaymentsScheduleDTO paymentsScheduleDTO = new PaymentsScheduleDTO();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    paymentsScheduleDTO.Id = filterBase.Id;
                    paymentsScheduleDTO.QuotasNumber = Convert.ToInt32(reader[PRODEN.PaymentSchedule.Properties.PaymentQuantity]);
                    paymentsScheduleDTO.CalculationUnit = Convert.ToInt16(reader[PRODEN.PaymentSchedule.Properties.GapUnitCode]);
                    paymentsScheduleDTO.CalculationQuantity = Convert.ToInt16(reader[PRODEN.PaymentSchedule.Properties.GapQuantity]);
                    break;
                }
            }
            return paymentsScheduleDTO;
        }

        /// <summary>
        /// Gets the payments distribution.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        public static List<PaymentDistributionDTO> GetPaymentsDistribution(FilterBaseDTO filterBase)
        {
            List<PaymentDistributionDTO> paymentDistributionDTOs = new List<PaymentDistributionDTO>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(PRODEN.PaymentDistribution.Properties.PaymentScheduleId, typeof(PRODEN.PaymentDistribution).Name);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Id);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.PaymentDistribution.Properties.PaymentNumber, typeof(PRODEN.PaymentDistribution).Name), PRODEN.PaymentDistribution.Properties.PaymentNumber));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.PaymentDistribution.Properties.GapUnitCode, typeof(PRODEN.PaymentDistribution).Name), PRODEN.PaymentDistribution.Properties.GapUnitCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.PaymentDistribution.Properties.GapQuantity, typeof(PRODEN.PaymentDistribution).Name), PRODEN.PaymentDistribution.Properties.GapQuantity));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.PaymentDistribution.Properties.PaymentPercentage, typeof(PRODEN.PaymentDistribution).Name), PRODEN.PaymentDistribution.Properties.PaymentPercentage));

            selectQuery.Table = new ClassNameTable(typeof(PRODEN.PaymentDistribution), typeof(PRODEN.PaymentDistribution).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            PaymentDistributionDTO paymentDistributionDTO = null;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    paymentDistributionDTO = new PaymentDistributionDTO();
                    paymentDistributionDTO.Id = filterBase.Id;
                    paymentDistributionDTO.Number = Convert.ToInt32(reader[PRODEN.PaymentDistribution.Properties.PaymentNumber]);
                    paymentDistributionDTO.CalculationUnit = Convert.ToInt16(reader[PRODEN.PaymentDistribution.Properties.GapUnitCode]);
                    paymentDistributionDTO.CalculationQuantity = Convert.ToInt16(reader[PRODEN.PaymentDistribution.Properties.GapQuantity]);
                    paymentDistributionDTO.Percentage = Convert.ToDecimal(reader[PRODEN.PaymentDistribution.Properties.PaymentPercentage]);
                    paymentDistributionDTOs.Add(paymentDistributionDTO);
                }
            }
            return paymentDistributionDTOs;
        }

        public static PaymenQuotaDTO GetPayment(FilterBaseDTO filterBase)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ISSEN.PayerPayment.Properties.PayerPaymentId);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Id);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.EndorsementId, typeof(ISSEN.PayerPayment).Name), ISSEN.PayerPayment.Properties.EndorsementId));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.PayerId, typeof(ISSEN.PayerPayment).Name), ISSEN.PayerPayment.Properties.PayerId));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.PaymentNum, typeof(ISSEN.PayerPayment).Name), ISSEN.PayerPayment.Properties.PaymentNum));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.Amount, typeof(ISSEN.PayerPayment).Name), ISSEN.PayerPayment.Properties.Amount));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.PaymentState, typeof(ISSEN.PayerPayment).Name), ISSEN.PayerPayment.Properties.PaymentState));
            selectQuery.Table = new ClassNameTable(typeof(ISSEN.PayerPayment), typeof(ISSEN.PayerPayment).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            PaymenQuotaDTO paymentDTO = null;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                paymentDTO = reader.SelectReader(read => new PaymenQuotaDTO
                {
                    EndorsementId = Convert.ToInt32(reader[ISSEN.PayerPayment.Properties.EndorsementId]),
                    Id = Convert.ToInt32(reader[ISSEN.PayerPayment.Properties.PayerId]),
                    Number = Convert.ToInt32(reader[ISSEN.PayerPayment.Properties.PaymentNum]),
                    Amount = Convert.ToDecimal(reader[ISSEN.PayerPayment.Properties.Amount]),
                    StateQuota = Convert.ToInt16(reader[ISSEN.PayerPayment.Properties.PaymentState])
                }).FirstOrDefault();
            }
            return paymentDTO;
        }

        public static int GetPaymentQuota(FilterBaseDTO filterBase)
        {
            int quota = 0;
            Function function = new Function(FunctionType.Min);
            function.AddParameter(new Column(ISSEN.PayerPayment.Properties.PaymentNum, typeof(ISSEN.PayerPayment).Name));
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ISSEN.PayerPayment.Properties.EndorsementId);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Id);
            criteriaBuilder.And();
            criteriaBuilder.Property(ISSEN.PayerPayment.Properties.PaymentState);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant((int)PaymentStatus.PENDING);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(function, ISSEN.PayerPayment.Properties.PaymentNum));
            selectQuery.Table = new ClassNameTable(typeof(ISSEN.PayerPayment), typeof(ISSEN.PayerPayment).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                quota = reader.SelectReader(read =>
                 Convert.ToInt32(reader[ISSEN.PayerPayment.Properties.PaymentNum])
                ).FirstOrDefault();
            }
            return quota;
        }

        public static PolicyDTO GetPaymentQuotaData(FilterBaseDTO filterBase)
        {
            PolicyDTO policy = new PolicyDTO();
            Function function = new Function(FunctionType.Min);
            function.AddParameter(new Column(ISSEN.PayerPayment.Properties.PaymentNum, typeof(ISSEN.PayerPayment).Name));
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ISSEN.PayerPayment.Properties.EndorsementId, typeof(ISSEN.PayerPayment).Name);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Id);
            criteriaBuilder.And();
            criteriaBuilder.Property(ISSEN.PayerPayment.Properties.PaymentState, typeof(ISSEN.PayerPayment).Name);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant((int)PaymentStatus.PENDING);
            
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(function, ISSEN.PayerPayment.Properties.PaymentNum));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name), ISSEN.Policy.Properties.DocumentNumber));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.DocumentNum, typeof(ISSEN.Endorsement).Name), ISSEN.Endorsement.Properties.DocumentNum));
            selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.Prefix.Properties.TinyDescription, typeof(COMMEN.Prefix).Name), COMMEN.Prefix.Properties.TinyDescription));

            Join join = new Join(new ClassNameTable(typeof(ISSEN.PayerPayment), typeof(ISSEN.PayerPayment).Name), new ClassNameTable(typeof(ISSEN.Policy), typeof(ISSEN.Policy).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.PayerPayment.Properties.PolicyId, typeof(ISSEN.PayerPayment).Name)
                .Equal()
                .Property(ISSEN.Policy.Properties.PolicyId, typeof(ISSEN.Policy).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(ISSEN.Endorsement), typeof(ISSEN.Endorsement).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.PayerPayment.Properties.EndorsementId, typeof(ISSEN.PayerPayment).Name)
                .Equal()
                .Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(COMMEN.Prefix), typeof(COMMEN.Prefix).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.Policy.Properties.PrefixCode, typeof(ISSEN.Policy).Name))
                .Equal()
                .Property(COMMEN.Prefix.Properties.PrefixCode, typeof(COMMEN.Prefix).Name)
                .GetPredicate();

            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();
            selectQuery.AddGroupValue(new Column(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name));
            selectQuery.AddGroupValue(new Column(ISSEN.Endorsement.Properties.DocumentNum, typeof(ISSEN.Endorsement).Name));
            selectQuery.AddGroupValue(new Column(COMMEN.Prefix.Properties.PrefixTypeCode, typeof(COMMEN.Prefix).Name));

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    if (reader[ISSEN.PayerPayment.Properties.PaymentNum] != null)
                        policy.PaymentNum = Convert.ToInt32(reader[ISSEN.PayerPayment.Properties.PaymentNum]);
                    if (reader[ISSEN.Endorsement.Properties.DocumentNum] != null)
                        policy.EndorsementDocumentNum = Convert.ToInt32(reader[ISSEN.Endorsement.Properties.DocumentNum]);
                    if (reader[COMMEN.Prefix.Properties.TinyDescription] != null)
                        policy.PrefixDescription = Convert.ToString(reader[COMMEN.Prefix.Properties.TinyDescription]);
                    if (reader[ISSEN.Policy.Properties.DocumentNumber] != null)
                        policy.DocumentNumber = Convert.ToInt32(reader[ISSEN.Policy.Properties.DocumentNumber]);
                }
            }
            return policy;
        }

        public static PolicyDTO GetPaymentQuotaDescription(FilterBaseDTO filterBase)
        {
            PolicyDTO policy = new PolicyDTO();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ISSEN.PayerPayment.Properties.EndorsementId, typeof(ISSEN.PayerPayment).Name);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Id);
            criteriaBuilder.And();
            criteriaBuilder.Property(ISSEN.PayerPayment.Properties.PaymentNum, typeof(ISSEN.PayerPayment).Name);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Quota);
            

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.DocumentNum, typeof(ISSEN.Endorsement).Name), ISSEN.Endorsement.Properties.DocumentNum));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name), ISSEN.Policy.Properties.DocumentNumber));
            selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.Prefix.Properties.TinyDescription, typeof(COMMEN.Prefix).Name), COMMEN.Prefix.Properties.TinyDescription));

            Join join = new Join(new ClassNameTable(typeof(ISSEN.Endorsement), typeof(ISSEN.Endorsement).Name), new ClassNameTable(typeof(ISSEN.Policy), typeof(ISSEN.Policy).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.Endorsement.Properties.PolicyId, typeof(ISSEN.Endorsement).Name)
                .Equal()
                .Property(ISSEN.Policy.Properties.PolicyId, typeof(ISSEN.Policy).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(ISSEN.PayerPayment), typeof(ISSEN.PayerPayment).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name)
                .Equal()
                .Property(ISSEN.PayerPayment.Properties.EndorsementId, typeof(ISSEN.PayerPayment).Name))
                .GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(COMMEN.Prefix), typeof(COMMEN.Prefix).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.Policy.Properties.PrefixCode, typeof(ISSEN.Policy).Name))
                .Equal()
                .Property(COMMEN.Prefix.Properties.PrefixCode, typeof(COMMEN.Prefix).Name)
                .GetPredicate();

            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    if (reader[ISSEN.Endorsement.Properties.DocumentNum] != null)
                        policy.EndorsementDocumentNum = Convert.ToInt32(reader[ISSEN.Endorsement.Properties.DocumentNum]);
                    if (reader[COMMEN.Prefix.Properties.TinyDescription] != null)
                        policy.PrefixDescription = Convert.ToString(reader[COMMEN.Prefix.Properties.TinyDescription]);
                    if (reader[ISSEN.Policy.Properties.DocumentNumber] != null)
                        policy.DocumentNumber = Convert.ToInt32(reader[ISSEN.Policy.Properties.DocumentNumber]);
                }
            }
            return policy;
        }
    }
}
