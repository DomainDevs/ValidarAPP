using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Integration.UnderwritingServices.EEProvider.Assemblers;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Collections.Generic;
using Sistran.Core.Framework.Queries;
using System.Linq;
using System.Data;
using System;
using Sistran.Core.Integration.UndewritingIntegrationServices.Models;

namespace Sistran.Core.Integration.UnderwritingServices.EEProvider.DAOs
{
    public class PolicyPaymentComponentDistributionDAO
    {
        public PayerPayment GetPayerPayment(int endorsementId, int paymentNum)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();


            filter.Property(ISSEN.PayerPayment.Properties.EndorsementId, typeof(ISSEN.PayerPayment).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            filter.And();
            filter.Property(ISSEN.PayerPayment.Properties.PaymentNum, typeof(ISSEN.PayerPayment).Name);
            filter.Equal();
            filter.Constant(paymentNum);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ISSEN.PayerPayment), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                return ModelAssembler.CreatePayerPaymentModel(businessCollection.Cast<ISSEN.PayerPayment>().FirstOrDefault());
            }
            else
            {
                return null;
            }
        }

        public void UpdateStatusPayerPayment(PayerPayment payerPayment)
        {
            var primaryKey = ISSEN.PayerPayment.CreatePrimaryKey(payerPayment.EndorsementId, payerPayment.PolicyId, payerPayment.PaymentNum, payerPayment.PayerId);
            var entityPayerPayment = (ISSEN.PayerPayment)DataFacadeManager.GetObject(primaryKey);
            entityPayerPayment.PaymentState = payerPayment.PaymentState;
            DataFacadeManager.Update(entityPayerPayment);
        }

        public List<PayerPaymentComp> GetPayerPaymentComp(int payerPaymentId)
        {
            List<PayerPaymentComp> payerPaymentComps = new List<PayerPaymentComp>();
            PayerPaymentComp payerPaymentComp = new PayerPaymentComp();
            #region selectQuery
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPaymentComp.Properties.PayerPaymentCompId, "c"), "PAYER_PAYMENT_COMP_ID"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPaymentComp.Properties.PayerPaymentId, "c"), "PAYER_PAYMENT_ID"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPaymentComp.Properties.ComponentCode, "c"), "COMPONENT_CD"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPaymentComp.Properties.PaymentPercentage, "c"), "PAYMENT_PCT"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPaymentComp.Properties.Amount, "c"), "AMOUNT"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPaymentComp.Properties.LocalAmount, "c"), "LOCAL_AMOUNT"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPaymentComp.Properties.MainAmount, "c"), "MAIN_AMOUNT"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPaymentComp.Properties.MainLocalAmount, "c"), "MAIN_LOCAL_AMOUNT"));
            select.AddSelectValue(new SelectValue(new Column(PARAMEN.ComponentType.Properties.TinyDescription, "ct"), "TINY_DESCRIPTION"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerPayment.Properties.PayerPaymentId, "pp"), "PAYER_PAYMENT_ID"));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.ExchangeRate, "e"), "EXCHANGE_RATE"));
            #endregion  finishSelectQuery
            #region join
            Join join = new Join(new ClassNameTable(typeof(ISSEN.PayerPaymentComp), "c"), new ClassNameTable(typeof(QUOEN.Component), "co"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.PayerPaymentComp.Properties.ComponentCode, "c")
                .Equal()
                .Property(QUOEN.Component.Properties.ComponentCode, "co")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(PARAMEN.ComponentType), "ct"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(PARAMEN.ComponentType.Properties.ComponentTypeCode, "ct")
                .Equal()
                .Property(QUOEN.Component.Properties.ComponentTypeCode, "co")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ISSEN.PayerPayment), "pp"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.PayerPayment.Properties.PayerPaymentId, "pp")
                .Equal()
                .Property(ISSEN.PayerPaymentComp.Properties.PayerPaymentId, "c")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ISSEN.Endorsement), "e"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.PayerPayment.Properties.EndorsementId, "pp")
                .Equal()
                .Property(ISSEN.Endorsement.Properties.EndorsementId, "e")
                .GetPredicate());
            #endregion finish join
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.PayerPaymentComp.Properties.PayerPaymentId, "c");
            filter.Equal();
            filter.Constant(payerPaymentId);
            select.Table = join;
            select.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    if (reader["PAYER_PAYMENT_COMP_ID"] != null)
                    {
                        payerPaymentComp = new PayerPaymentComp
                        {
                            Amount = Convert.ToDecimal(reader["AMOUNT"]),
                            ComponentCode = Convert.ToInt32(reader["COMPONENT_CD"]),
                            LocalAmount = Convert.ToDecimal(reader["LOCAL_AMOUNT"]),
                            MainAmount = Convert.ToDecimal(reader["MAIN_AMOUNT"]),
                            MainLocalAmount = Convert.ToDecimal(reader["MAIN_LOCAL_AMOUNT"]),
                            PayerPaymentCompId = Convert.ToInt32(reader["PAYER_PAYMENT_COMP_ID"]),
                            PayerPaymentId = Convert.ToInt32(reader["PAYER_PAYMENT_ID"]),
                            PaymentPercentage = Convert.ToDecimal(reader["PAYMENT_PCT"]),
                            TinyDescription = reader["TINY_DESCRIPTION"].ToString(),
                            ExchangeRate = Convert.ToDecimal(reader["EXCHANGE_RATE"])
                        };
                        payerPaymentComps.Add(payerPaymentComp);
                    }
                }
            }
            return payerPaymentComps;
        }

        public List<PayerPaymentCompLbsb> GetPayerPaymentCompLbsb(int payerPaymentId)
        {
            List<PayerPaymentCompLbsb> payerPaymentCompLbsb = new List<PayerPaymentCompLbsb>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.PayerPaymentCompLbsb.Properties.PayerPaymentId, typeof(ISSEN.PayerPaymentCompLbsb).Name);
            filter.Equal();
            filter.Constant(payerPaymentId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ISSEN.PayerPaymentCompLbsb), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                payerPaymentCompLbsb = ModelAssembler.CreatePayerPaymentCompLbsb(businessCollection);
            }

            return payerPaymentCompLbsb;
        }
    }
}
