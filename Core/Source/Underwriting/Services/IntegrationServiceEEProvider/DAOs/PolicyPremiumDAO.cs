using System;
using System.Collections.Generic;
using System.Linq;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Framework.Queries;
using System.Threading;
using System.Globalization;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Data;
using Sistran.Core.Integration.UndewritingIntegrationServices.Models;

namespace Sistran.Core.Integration.UnderwritingServices.EEProvider.DAOs
{
    public class PolicyPremiumDAO
    {
        /// <summary>
        /// FormatDateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>DateTime</returns>
        private DateTime FormatDateTime(string dateTime)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                IFormatProvider culture = new CultureInfo("es-EC", true);

                return Convert.ToDateTime(dateTime, culture);

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        public List<PremiumSearchPolicy> GetPremiumSearchPolicies(SearchPolicyPayment searchPolicyPayment)
        {
            int pageIndex = 0;

            #region Filtro


            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            if (searchPolicyPayment.InsuredId != "" || searchPolicyPayment.PayerId != "" || searchPolicyPayment.AgentId != "" || searchPolicyPayment.GroupId != "" || searchPolicyPayment.PolicyId != "" ||
                searchPolicyPayment.SalesTicket != "" || searchPolicyPayment.BranchId != "" || searchPolicyPayment.PrefixId != "" || searchPolicyPayment.EndorsementId != "" ||
                searchPolicyPayment.DateFrom != "" || searchPolicyPayment.DateTo != "")
            {
                filter.Property(ISSEN.Policy.Properties.PolicyId, "p");
                filter.GreaterEqual();
                filter.Constant(0);
                filter.And();
                //Filtra solo las cuotas que tengan saldo
                filter.Property(ISSEN.PayerPayment.Properties.Amount, "pp");
                filter.Distinct();
                filter.Constant(0);
            }
            else
            {
                filter.Property(ISSEN.Policy.Properties.PolicyId, "p");
                filter.Greater();
                filter.Constant(0);
            }

            if (searchPolicyPayment.InsuredId != "")
            {
                filter.And();
                filter.PropertyEquals(ISSEN.Policy.Properties.PolicyholderId, "p", Convert.ToInt64(searchPolicyPayment.InsuredId));
            }

            if (searchPolicyPayment.PayerId != "")
            {
                filter.And();
                filter.PropertyEquals(ISSEN.PayerPayment.Properties.PayerId, "pp", Convert.ToInt64(searchPolicyPayment.PayerId));
                //criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PolicyPremiumReceivableTrans.Properties.PayerIndividualId, Convert.ToInt64(payerId));
            }

            if (searchPolicyPayment.AgentId != "")
            {
                // Implementar en la vista el campo para Agente.
                filter.And();
                filter.PropertyEquals(UPEN.AgentAgency.Properties.IndividualId, "upag", Convert.ToInt64(searchPolicyPayment.AgentId));
            }

            if (searchPolicyPayment.GroupId != "")
            {
                filter.And();
                filter.PropertyEquals(ISSEN.Policy.Properties.BillingGroupCode, "p", Convert.ToInt64(searchPolicyPayment.GroupId));
            }

            if (searchPolicyPayment.PolicyId != "")
            {
                filter.And();
                filter.PropertyEquals(ISSEN.Policy.Properties.PolicyId, "p", Convert.ToDecimal(searchPolicyPayment.PolicyId));
            }
            if (searchPolicyPayment.PolicyDocumentNumber != "")
            {
                filter.And();
                filter.PropertyEquals(ISSEN.Policy.Properties.DocumentNumber, "p", Convert.ToDecimal(searchPolicyPayment.PolicyDocumentNumber));
            }

            if (searchPolicyPayment.SalesTicket != "")
            {
                //Pendiente implementar en la vista el campo para Factura.
            }

            if (searchPolicyPayment.BranchId != "")
            {
                filter.And();
                filter.PropertyEquals(ISSEN.Policy.Properties.BranchCode, "p", Convert.ToInt64(searchPolicyPayment.BranchId));
            }

            if (searchPolicyPayment.PrefixId != "")
            {
                filter.And();
                filter.PropertyEquals(ISSEN.Policy.Properties.PrefixCode, "p", Convert.ToInt64(searchPolicyPayment.PrefixId));
            }

            if (searchPolicyPayment.EndorsementId != "")
            {
                filter.And();
                filter.PropertyEquals(ISSEN.Endorsement.Properties.EndorsementId, "e", Convert.ToInt64(searchPolicyPayment.EndorsementId));
            }

            if(searchPolicyPayment.EndorsementDocumentNumber != "")
            {
                filter.And();
                filter.PropertyEquals(ISSEN.Endorsement.Properties.DocumentNum, "e", Convert.ToInt64(searchPolicyPayment.EndorsementDocumentNumber));
            }

            if (searchPolicyPayment.DateFrom != "" && searchPolicyPayment.DateTo != "")
            {
                filter.And();
                filter.OpenParenthesis();
                filter.Property(ISSEN.PayerPayment.Properties.PayExpDate, "pp");
                filter.GreaterEqual();
                filter.Constant(FormatDateTime(searchPolicyPayment.DateFrom));
                filter.CloseParenthesis();
                filter.And();
                filter.OpenParenthesis();
                filter.Property(ISSEN.PayerPayment.Properties.PayExpDate, "pp");
                filter.LessEqual();
                filter.Constant(FormatDateTime(searchPolicyPayment.DateTo));
                filter.CloseParenthesis();
            }
            #endregion end Filtro
            #region selectQuery
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.DocumentNumber, "p"), "POLICY_DOCUMENT_NUMBER"));

            select.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.PolicyId, "p"), "POLICY_ID"));
            select.AddSelectValue(new SelectValue(new Column(PARAMEN.BusinessType.Properties.SmallDescription, "bt"), "BUSSINESS_TYPE_DESCRIPTION"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.BusinessTypeCode, "p"), "BUSINESS_TYPE_CD"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.BranchCode, "p"), "BRANCH_CD"));
            select.AddSelectValue(new SelectValue(new Column(COMMEN.Branch.Properties.Description, "br"), "BRANCH_DESCRIPTION"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.PrefixCode, "p"), "PREFIX_CD"));
            select.AddSelectValue(new SelectValue(new Column(COMMEN.Prefix.Properties.Description, "pr"), "PREFIX_DESCRIPTION"));
            select.AddSelectValue(new SelectValue(new Column(COMMEN.Prefix.Properties.TinyDescription, "pr"), "PREFIX_TINY_DESCRIPTION"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.PolicyholderId, "p"), "INSURED_INDIVIDUAL_ID"));

            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.IdCardNo ?? "", "per"), "INSURED_DOCUMENT_NUMBER_PER"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.TributaryIdNo ?? "", "co"), "INSURED_DOCUMENT_NUMBER_COMP"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Surname ?? "", "per"), "INSURED_SURNAME"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.MotherLastName ?? "", "per"), "INSURED_MOTHER_LAST_NAME"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Name, "per"), "INSURED_NAME"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.TradeName, "co"), "INSURED_TRADE_NAME"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.EndorsementId, "e"), "ENDORSEMENT_ID"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.DocumentNum, "e"), "ENDORSEMENT_DOCUMENT_NUMBER"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.EndoTypeCode, "e"), "ENDO_TYPE_CD"));
            select.AddSelectValue(new SelectValue(new Column(PARAMEN.EndorsementType.Properties.Description, "et"), "ENDORSEMENT_TYPE_DESCRIPTION"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.BillingGroupCode, "p"), "BILLING_GROUP_CD"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.BillingGroup.Properties.Description, "bg"), "BILLING_GROUP_DESCRIPTION"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.PaymentNum, "pp"), "PAYMENT_NUM"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.PayerId, "pp"), "PAYER_INDIVIDUAL_ID"));

            select.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.Description, "upag"), "AGENT_NAME"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.AgentAgencyId, "upag"), "AGENT_AGENCY_ID"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.IdCardNo, "perag"), "AGENT_DOCUMENT_NUMBER_PER"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.TributaryIdNo, "coag"), "AGENT_DOCUMENT_NUMBER_COMP"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Surname, "perag"), "AGENT_SURNAME"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.MotherLastName, "perag"), "AGENT_MOTHER_LAST_NAME"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Name, "perag"), "AGENT_PERSON_NAME"));


            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.IdCardNo, "perpay"), "PAYER_DOCUMENT_NUMBER_PER"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.TributaryIdNo, "copay"), "PAYER_DOCUMENT_NUMBER_COMP"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Surname, "perpay"), "PAYER_SURNAME"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.MotherLastName, "perpay"), "PAYER_MOTHER_LAST_NAME"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Name, "perpay"), "PAYER_NAME"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.TradeName, "copay"), "PAYER_TRADE_NAME"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.PayExpDate, "pp"), "PAYMENT_EXPIRATION_DATE"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.Amount, "pp"), "PAYMENT_AMOUNT"));
            select.AddSelectValue(new SelectValue(new Column(COMMEN.Currency.Properties.Description, "cc"), "CURRENCY_DESCRIPTION"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.CurrencyCode, "p"), "CURRENCY_ID"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.ExchangeRate, "e"), "EXCHANGE_RATE"));
            #endregion finish selectQuery
            #region join

            Join join = new Join(new ClassNameTable(typeof(ISSEN.Policy), "p"), new ClassNameTable(typeof(ISSEN.Endorsement), "e"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.Policy.Properties.PolicyId, "p")
                .Equal()
                .Property(ISSEN.Endorsement.Properties.PolicyId, "e")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(PARAMEN.BusinessType), "bt"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(PARAMEN.BusinessType.Properties.BusinessTypeCode, "bt")
                .Equal()
                .Property(ISSEN.Policy.Properties.BusinessTypeCode, "p")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(COMMEN.Branch), "br"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(COMMEN.Branch.Properties.BranchCode, "br")
                .Equal()
                .Property(ISSEN.Policy.Properties.BranchCode, "p")
                .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(COMMEN.Prefix), "pr"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(COMMEN.Prefix.Properties.PrefixCode, "pr")
                .Equal()
                .Property(ISSEN.Policy.Properties.PrefixCode, "p")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UPEN.Individual), "ind"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Individual.Properties.IndividualId, "ind")
                .Equal()
                .Property(ISSEN.Policy.Properties.PolicyholderId, "p")
                .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(UPEN.Person), "per"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Person.Properties.IndividualId, "per")
                .Equal()
                .Property(UPEN.Individual.Properties.IndividualId, "ind")
                .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(UPEN.Company), "co"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Company.Properties.IndividualId, "co")
                .Equal()
                .Property(UPEN.Individual.Properties.IndividualId, "ind")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ISSEN.PayerPayment), "pp"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.PayerPayment.Properties.PolicyId, "pp")
                .Equal()
                .Property(ISSEN.Policy.Properties.PolicyId, "p")
                .And()
                .Property(ISSEN.PayerPayment.Properties.EndorsementId, "pp")
                .Equal()
                .Property(ISSEN.Endorsement.Properties.EndorsementId, "e")
                .And()
                .Property(ISSEN.PayerPayment.Properties.PaymentState, "pp")
                .Equal()
                .Constant(1)// TODO: Se deja "1" ya que no existe tabla parametrica. el 1 filtra
                .GetPredicate());


            join = new Join(join, new ClassNameTable(typeof(UPEN.Individual), "indpay"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Individual.Properties.IndividualId, "indpay")
                .Equal()
                .Property(ISSEN.PayerPayment.Properties.PayerId, "pp")
                .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(UPEN.Person), "perpay"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Person.Properties.IndividualId, "perpay")
                .Equal()
                .Property(UPEN.Individual.Properties.IndividualId, "indpay")
                .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(UPEN.Company), "copay"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Company.Properties.IndividualId, "copay")
                .Equal()
                .Property(UPEN.Individual.Properties.IndividualId, "indpay")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ISSEN.PolicyAgent), "pa"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.PolicyAgent.Properties.PolicyId, "pa")
                .Equal()
                .Property(ISSEN.Policy.Properties.PolicyId, "p")
                .And()
                .Property(ISSEN.PolicyAgent.Properties.EndorsementId, "pa")
                .Equal()
                .Property(ISSEN.Endorsement.Properties.EndorsementId, "e")
                .And()
                .Property(ISSEN.PolicyAgent.Properties.IsPrimary, "pa")
                .Equal()
                .Constant(1)
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UPEN.Individual), "indag"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Individual.Properties.IndividualId, "indag")
                .Equal()
                .Property(ISSEN.PolicyAgent.Properties.IndividualId, "pa")
                .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(UPEN.Person), "perag"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Person.Properties.IndividualId, "perag")
                .Equal()
                .Property(UPEN.Individual.Properties.IndividualId, "indag")
                .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(UPEN.Company), "coag"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Company.Properties.IndividualId, "coag")
                .Equal()
                .Property(UPEN.Individual.Properties.IndividualId, "indag")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UPEN.AgentAgency), "upag"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.AgentAgency.Properties.IndividualId, "upag")
                .Equal()
                .Property(ISSEN.PolicyAgent.Properties.IndividualId, "pa")
                .And()
                .Property(UPEN.AgentAgency.Properties.AgentAgencyId, "upag")
                .Equal()
                .Property(ISSEN.PolicyAgent.Properties.AgentAgencyId, "pa")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ISSEN.BillingGroup), "bg"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.BillingGroup.Properties.BillingGroupCode, "bg")
                .Equal()
                .Property(ISSEN.Policy.Properties.BillingGroupCode, "p")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(PARAMEN.EndorsementType), "et"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(PARAMEN.EndorsementType.Properties.EndoTypeCode, "et")
                .Equal()
                .Property(ISSEN.Endorsement.Properties.EndoTypeCode, "e")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(COMMEN.Currency), "cc"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(COMMEN.Currency.Properties.CurrencyCode, "cc")
                .Equal()
                .Property(ISSEN.Policy.Properties.CurrencyCode, "p")
                .GetPredicate());
            #endregion finish join

            select.Table = join;
            select.Where = filter.GetPredicate();
            if (searchPolicyPayment.BranchId != "" || searchPolicyPayment.PrefixId != "")
                select.MaxRows = 50;
                
            PremiumSearchPolicy premiumSearchPolicyData = null;

            List<PremiumSearchPolicy> premiumSearchPolicies = new List<PremiumSearchPolicy>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    if (reader["POLICY_ID"] != null)
                    {
                        premiumSearchPolicyData = new PremiumSearchPolicy
                        {
                            BranchPrefixPolicyEndorsement = Convert.ToString(reader["BRANCH_DESCRIPTION"]).Substring(0, 3) + '-' + Convert.ToString(reader["PREFIX_DESCRIPTION"]).Substring(0, 3) + '-' + Convert.ToString(reader["POLICY_DOCUMENT_NUMBER"]) + '-' + Convert.ToString(reader["ENDORSEMENT_DOCUMENT_NUMBER"]),
                            PolicyId = Convert.ToInt32(reader["POLICY_ID"]),
                            PolicyDocumentNumber = reader["POLICY_DOCUMENT_NUMBER"].ToString(),
                            BussinessTypeDescription = reader["BUSSINESS_TYPE_DESCRIPTION"].ToString(),
                            BussinessTypeId = Convert.ToInt32(reader["BUSINESS_TYPE_CD"]),
                            BranchId = Convert.ToInt32(reader["BRANCH_CD"]),
                            BranchDescription = reader["BRANCH_DESCRIPTION"].ToString(),
                            PrefixId = Convert.ToInt32(reader["PREFIX_CD"]),
                            PrefixDescription = reader["PREFIX_DESCRIPTION"].ToString(),
                            PrefixTinyDescription = reader["PREFIX_TINY_DESCRIPTION"].ToString(),
                            InsuredIndividualId = Convert.ToInt32(reader["INSURED_INDIVIDUAL_ID"]),
                            InsuredDocumentNumber = reader["INSURED_DOCUMENT_NUMBER_PER"] == null ? reader["INSURED_DOCUMENT_NUMBER_COMP"].ToString() : reader["INSURED_DOCUMENT_NUMBER_PER"].ToString(),
                            InsuredName = reader["INSURED_NAME"] == null ? Convert.ToString(reader["INSURED_TRADE_NAME"]) : Convert.ToString(reader["INSURED_SURNAME"] + " " + Convert.ToString(reader["INSURED_MOTHER_LAST_NAME"]) + " " + Convert.ToString(reader["INSURED_NAME"])),
                            EndorsementId = Convert.ToInt32(reader["ENDORSEMENT_ID"]),
                            EndorsementDocumentNumber = reader["ENDORSEMENT_DOCUMENT_NUMBER"].ToString(),
                            EndorsementTypeId = Convert.ToInt32(reader["ENDO_TYPE_CD"]),
                            EndorsementTypeDescription = reader["ENDORSEMENT_TYPE_DESCRIPTION"].ToString(),
                            CollectGroupId = reader["BILLING_GROUP_CD"] == null ? 0 : Convert.ToInt32(reader["BILLING_GROUP_CD"]),
                            CollectGroupDescription = reader["BILLING_GROUP_DESCRIPTION"] == null ? "" : Convert.ToString(reader["BILLING_GROUP_DESCRIPTION"]),
                            PayerId = Convert.ToInt32(reader["PAYER_INDIVIDUAL_ID"]),
                            PayerIndividualId = Convert.ToInt32(reader["PAYER_INDIVIDUAL_ID"]),
                            PayerDocumentNumber = reader["PAYER_DOCUMENT_NUMBER_PER"] == null ? reader["PAYER_DOCUMENT_NUMBER_COMP"].ToString() : reader["PAYER_DOCUMENT_NUMBER_PER"].ToString(),
                            PayerName = reader["PAYER_NAME"] == null ? Convert.ToString(reader["PAYER_TRADE_NAME"]) : Convert.ToString(reader["PAYER_SURNAME"] + " " + Convert.ToString(reader["PAYER_MOTHER_LAST_NAME"]) + " " + Convert.ToString(reader["PAYER_NAME"])),
                            PaymentExpirationDate = Convert.ToDateTime(reader["PAYMENT_EXPIRATION_DATE"]),
                            Amount = Convert.ToDecimal(reader["PAYMENT_AMOUNT"]),
                            CurrencyDescription = reader["CURRENCY_DESCRIPTION"].ToString(),
                            ExchangeRate = Convert.ToDecimal(reader["EXCHANGE_RATE"]),
                            PaymentNumber = Convert.ToInt32(reader["PAYMENT_NUM"]),
                            CurrencyId = Convert.ToInt32(reader["CURRENCY_ID"]),

                            PolicyAgentId = Convert.ToInt32(reader["AGENT_AGENCY_ID"]),
                            PolicyAgentDocumentNumber = reader["AGENT_DOCUMENT_NUMBER_PER"] == null ? reader["AGENT_DOCUMENT_NUMBER_COMP"].ToString() : reader["AGENT_DOCUMENT_NUMBER_PER"].ToString(),
                            PolicyAgentName = reader["AGENT_NAME"] == null ? Convert.ToString(reader["AGENT_PERSON_NAME"]) : Convert.ToString(reader["AGENT_NAME"] + " " + Convert.ToString(reader["AGENT_MOTHER_LAST_NAME"]) + " " + Convert.ToString(reader["AGENT_PERSON_NAME"]))
                        };
                        premiumSearchPolicies.Add(premiumSearchPolicyData);
                    }
                }
            }

            return premiumSearchPolicies;
        }
    }
}
