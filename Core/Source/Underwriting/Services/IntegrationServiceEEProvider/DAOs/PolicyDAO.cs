using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Data;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
namespace Sistran.Core.Integration.UnderwritingServices.EEProvider.DAOs
{
    /// <summary>
    /// Polizas
    /// </summary>
    public class PolicyDAO
    {
        /// <summary>
        /// Gets the policies by policy filter.
        /// </summary>
        /// <param name="policyFilterDTO">The policy filter dto.</param>
        /// <returns></returns>
        public static List<decimal> GetPoliciesByPolicyFilter(PolicyFilterDTO policyFilterDTO)
        {
            List<decimal> policies = new List<decimal>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ISSEN.Policy.Properties.DocumentNumber);
            criteriaBuilder.Like();
            criteriaBuilder.Constant(policyFilterDTO.DocumentNumber + "%");
            if (policyFilterDTO.BranchId != 0)
            {
                criteriaBuilder.And();
                criteriaBuilder.Property(ISSEN.Policy.Properties.BranchCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(policyFilterDTO.BranchId);
            }
            if (policyFilterDTO.PrefixId != 0)
            {
                criteriaBuilder.And();
                criteriaBuilder.Property(ISSEN.Policy.Properties.PrefixCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(policyFilterDTO.PrefixId);
            }          
             SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name), ISSEN.Policy.Properties.DocumentNumber));
            selectQuery.Table = new ClassNameTable(typeof(ISSEN.Policy), typeof(ISSEN.Policy).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            selectQuery.Distinct = true;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    policies.Add(Convert.ToDecimal(reader[ISSEN.Policy.Properties.DocumentNumber]));
                }
            }
            return policies;
        }

        /// <summary>
        /// Gets the endorsements by policy filter.
        /// </summary>
        /// <param name="policyFilterDTO">The policy filter dto.</param>
        /// <returns></returns>
        public static List<EndorsementBaseDTO> GetEndorsementsByPolicyFilter(PolicyFilterDTO policyFilterDTO)
        {
            List<EndorsementBaseDTO> endorsementBaseDTOs = new List<EndorsementBaseDTO>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ISSEN.Policy.Properties.DocumentNumber, typeof(ISSEN.Policy).Name);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(policyFilterDTO.DocumentNumber);
            if (policyFilterDTO.BranchId != 0)
            {
                criteriaBuilder.And();
                criteriaBuilder.Property(ISSEN.Policy.Properties.BranchCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(policyFilterDTO.BranchId);
            }
            if (policyFilterDTO.PrefixId != 0)
            {
                criteriaBuilder.And();
                criteriaBuilder.Property(ISSEN.Policy.Properties.PrefixCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(policyFilterDTO.PrefixId);
            }
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name), ISSEN.Endorsement.Properties.EndorsementId));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.DocumentNum, typeof(ISSEN.Endorsement).Name), ISSEN.Endorsement.Properties.DocumentNum));

            Join join = new Join(new ClassNameTable(typeof(ISSEN.Policy), typeof(ISSEN.Policy).Name), new ClassNameTable(typeof(ISSEN.Endorsement), typeof(ISSEN.Endorsement).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
              .Property(ISSEN.Policy.Properties.PolicyId, typeof(ISSEN.Policy).Name)
              .Equal()
              .Property(ISSEN.Endorsement.Properties.PolicyId, typeof(ISSEN.Endorsement).Name)
              .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();
            EndorsementBaseDTO endorsementBaseDTO = null;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    endorsementBaseDTO = new EndorsementBaseDTO();
                    endorsementBaseDTO.Id = Convert.ToInt32(reader[ISSEN.Endorsement.Properties.EndorsementId]);
                    endorsementBaseDTO.DocumentNum = Convert.ToInt32(reader[ISSEN.Endorsement.Properties.DocumentNum]);
                    endorsementBaseDTOs.Add(endorsementBaseDTO);
                }
            }
            return endorsementBaseDTOs;
        }

        /// <summary>
        /// Gets the summary by policy filter.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        public static SummaryDTO GetSummaryByPolicyFilter(FilterBaseDTO filterBase)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Id);
            criteriaBuilder.And();
            criteriaBuilder.Property(ISSEN.PolicyAgent.Properties.IsPrimary, typeof(ISSEN.PolicyAgent).Name);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(1);
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.PolicyholderId, typeof(ISSEN.Policy).Name), ISSEN.Policy.Properties.PolicyholderId));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.CurrencyCode, typeof(ISSEN.Policy).Name), ISSEN.Policy.Properties.CurrencyCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.Currency.Properties.Description, typeof(COMMEN.Currency).Name), COMMEN.Currency.Properties.Description));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.IssueDate, typeof(ISSEN.Endorsement).Name), ISSEN.Endorsement.Properties.IssueDate));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.CurrentFrom, typeof(ISSEN.Endorsement).Name), ISSEN.Endorsement.Properties.CurrentFrom));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.CurrentTo, typeof(ISSEN.Endorsement).Name), ISSEN.Endorsement.Properties.CurrentTo));
            selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.PaymentMethod.Properties.Description, typeof(COMMEN.PaymentMethod).Name), COMMEN.PaymentMethod.Properties.Description));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PolicyAgent.Properties.IndividualId, typeof(ISSEN.PolicyAgent).Name), ISSEN.PolicyAgent.Properties.IndividualId));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.PolicyAgent.Properties.AgentAgencyId, typeof(ISSEN.PolicyAgent).Name), ISSEN.PolicyAgent.Properties.AgentAgencyId));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.PaymentSchedule.Properties.Description, typeof(PRODEN.PaymentSchedule).Name), PRODEN.PaymentSchedule.Properties.Description));

            Join join = new Join(new ClassNameTable(typeof(ISSEN.Policy), typeof(ISSEN.Policy).Name), new ClassNameTable(typeof(ISSEN.Endorsement), typeof(ISSEN.Endorsement).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder().Property(ISSEN.Policy.Properties.PolicyId, typeof(ISSEN.Policy).Name).Equal().Property(ISSEN.Endorsement.Properties.PolicyId, typeof(ISSEN.Endorsement).Name)
           .GetPredicate());
            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();

            join = new Join(join, new ClassNameTable(typeof(ISSEN.EndorsementPayer), typeof(ISSEN.EndorsementPayer).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder().Property(ISSEN.Endorsement.Properties.PolicyId, typeof(ISSEN.Endorsement).Name).Equal().Property(ISSEN.EndorsementPayer.Properties.PolicyId, typeof(ISSEN.EndorsementPayer).Name)
           .And()
           .Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name).Equal().Property(ISSEN.EndorsementPayer.Properties.EndorsementId, typeof(ISSEN.EndorsementPayer).Name)
           .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ISSEN.PolicyAgent), typeof(ISSEN.PolicyAgent).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder().Property(ISSEN.Endorsement.Properties.PolicyId, typeof(ISSEN.Endorsement).Name).Equal().Property(ISSEN.PolicyAgent.Properties.PolicyId, typeof(ISSEN.PolicyAgent).Name)
           .And()
           .Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name).Equal().Property(ISSEN.PolicyAgent.Properties.EndorsementId, typeof(ISSEN.PolicyAgent).Name)
           .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(COMMEN.Currency), typeof(COMMEN.Currency).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder().Property(COMMEN.Currency.Properties.CurrencyCode, typeof(COMMEN.Currency).Name).Equal().Property(ISSEN.Policy.Properties.CurrencyCode, typeof(ISSEN.Policy).Name)
           .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(COMMEN.PaymentMethod), typeof(COMMEN.PaymentMethod).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder().Property(COMMEN.PaymentMethod.Properties.PaymentMethodCode, typeof(COMMEN.PaymentMethod).Name).Equal().Property(ISSEN.EndorsementPayer.Properties.PaymentMethodCode, typeof(ISSEN.EndorsementPayer).Name)
           .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(PRODEN.PaymentSchedule), typeof(PRODEN.PaymentSchedule).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder().Property(PRODEN.PaymentSchedule.Properties.PaymentScheduleId, typeof(PRODEN.PaymentSchedule).Name).Equal().Property(ISSEN.EndorsementPayer.Properties.PaymentScheduleId, typeof(ISSEN.EndorsementPayer).Name)
           .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();

            SummaryDTO summaryDTO = new SummaryDTO();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    summaryDTO.Holder = new PersonBaseDTO { Id = Convert.ToInt32(reader[ISSEN.Policy.Properties.PolicyholderId]) };
                    summaryDTO.BaseCurrencyDTO = new BaseCurrencyDTO { Id = Convert.ToInt32(reader[COMMEN.Currency.Properties.CurrencyCode]), Description = Convert.ToString(reader[COMMEN.Currency.Properties.Description]) };
                    summaryDTO.IssuanceDate = Convert.ToDateTime(reader[ISSEN.Endorsement.Properties.IssueDate]);
                    summaryDTO.FromDate = Convert.ToDateTime(reader[ISSEN.Endorsement.Properties.CurrentFrom]);
                    summaryDTO.ToDate = Convert.ToDateTime(reader[ISSEN.Endorsement.Properties.CurrentTo]);
                    summaryDTO.ConduitName = Convert.ToString(reader[6]);
                    summaryDTO.MainAgent = new PersonBaseDTO { Id = Convert.ToInt32(reader[ISSEN.PolicyAgent.Properties.IndividualId]), IdSequential = Convert.ToInt32(reader[ISSEN.PolicyAgent.Properties.AgentAgencyId]) };
                    summaryDTO.CurrentPlan = Convert.ToString(reader[9]);
                    break;
                }
            }
            if (summaryDTO.Holder != null)
                summaryDTO.Holder.Description = PersonDAO.GetNameByIndividualId(Convert.ToInt32(summaryDTO.Holder.Id));
            if (summaryDTO.MainAgent != null)
                summaryDTO.MainAgent.Description = PersonDAO.GetNameByIndividualId(Convert.ToInt32(summaryDTO.MainAgent.Id));
            return summaryDTO;
        }

        /// <summary>
        /// Gets the dates by endorsement identifier.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        public static PolicyBaseDTO GetEndorsementBaseByEndorsementId(FilterBaseDTO filterBase)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ISSEN.Endorsement.Properties.EndorsementId, typeof(ISSEN.Endorsement).Name);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Id);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.CurrentFrom, typeof(ISSEN.Endorsement).Name), ISSEN.Endorsement.Properties.CurrentFrom));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.CurrentTo, typeof(ISSEN.Endorsement).Name), ISSEN.Endorsement.Properties.CurrentTo));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.ExchangeRate, typeof(ISSEN.Endorsement).Name), ISSEN.Endorsement.Properties.ExchangeRate));
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.CoissuePercentage, typeof(ISSEN.Policy).Name), ISSEN.Policy.Properties.CoissuePercentage));
            Join join = new Join(new ClassNameTable(typeof(ISSEN.Policy), typeof(ISSEN.Policy).Name), new ClassNameTable(typeof(ISSEN.Endorsement), typeof(ISSEN.Endorsement).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder().Property(ISSEN.Policy.Properties.PolicyId, typeof(ISSEN.Policy).Name).Equal().Property(ISSEN.Endorsement.Properties.PolicyId, typeof(ISSEN.Endorsement).Name)
           .GetPredicate());
            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();
            //selectQuery.Table = new ClassNameTable(typeof(ISSEN.Endorsement), typeof(ISSEN.Endorsement).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();

            PolicyBaseDTO summaryDTO = new PolicyBaseDTO();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    summaryDTO.FromDate = Convert.ToDateTime(reader[ISSEN.Endorsement.Properties.CurrentFrom]);
                    summaryDTO.ToDate = Convert.ToDateTime(reader[ISSEN.Endorsement.Properties.CurrentTo]);
                    summaryDTO.ExchangeRate = Convert.ToDecimal(reader[ISSEN.Endorsement.Properties.ExchangeRate]);
                    summaryDTO.CoinsurancePct = Convert.ToDecimal(reader[ISSEN.Policy.Properties.CoissuePercentage]);                  
                    break;
                }
            }
            return summaryDTO;
        }
    }
}