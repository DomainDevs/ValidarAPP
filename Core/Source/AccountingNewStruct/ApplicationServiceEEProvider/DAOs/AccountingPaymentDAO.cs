using Sistran.Core.Application.AccountingServices.EEProvider.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System;
using System.Data;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class AccountingPaymentDAO
    {
        public BillReport GetBillReport(int technicalTransaction)
        {
            BillReport billReport = new BillReport();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.TechnicalTransaction, technicalTransaction);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.Branch.Properties.Description, typeof(COMMEN.Branch).Name), "BranchName"));
            selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.Currency.Properties.Description, typeof(COMMEN.Currency).Name), "CurrencyDescription"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.CollectConcept.Properties.Description, typeof(ACCOUNTINGEN.CollectConcept).Name), "ConceptDescription"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.PaymentType.Properties.Description, typeof(ACCOUNTINGEN.PaymentType).Name), "PaymentTypeDescription"));

            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Collect.Properties.Description, typeof(ACCOUNTINGEN.Collect).Name), ACCOUNTINGEN.Collect.Properties.Description));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Collect.Properties.IndividualId, typeof(ACCOUNTINGEN.Collect).Name), ACCOUNTINGEN.Collect.Properties.IndividualId));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Collect.Properties.RegisterDate, typeof(ACCOUNTINGEN.Collect).Name), ACCOUNTINGEN.Collect.Properties.RegisterDate));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Collect.Properties.PaymentsTotal, typeof(ACCOUNTINGEN.Collect).Name), ACCOUNTINGEN.Collect.Properties.PaymentsTotal));
            
            Join join = new Join(new ClassNameTable(typeof(ACCOUNTINGEN.Collect), typeof(ACCOUNTINGEN.Collect).Name), 
                new ClassNameTable(typeof(ACCOUNTINGEN.CollectControl), typeof(ACCOUNTINGEN.CollectControl).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.Collect.Properties.CollectControlCode, typeof(ACCOUNTINGEN.Collect).Name)
                .Equal()
                .Property(ACCOUNTINGEN.CollectControl.Properties.CollectControlId, typeof(ACCOUNTINGEN.CollectControl).Name)
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ACCOUNTINGEN.CollectConcept), typeof(ACCOUNTINGEN.CollectConcept).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.Collect.Properties.CollectConceptCode, typeof(ACCOUNTINGEN.Collect).Name)
                .Equal()
                .Property(ACCOUNTINGEN.CollectConcept.Properties.CollectConceptId, typeof(ACCOUNTINGEN.CollectConcept).Name)
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(COMMEN.Branch), typeof(COMMEN.Branch).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.CollectControl.Properties.BranchCode, typeof(ACCOUNTINGEN.CollectControl).Name)
                .Equal()
                .Property(COMMEN.Branch.Properties.BranchCode, typeof(COMMEN.Branch).Name)
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ACCOUNTINGEN.CollectPayment), typeof(ACCOUNTINGEN.CollectPayment).Name), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.Collect.Properties.CollectId, typeof(ACCOUNTINGEN.Collect).Name)
                .Equal()
                .Property(ACCOUNTINGEN.CollectPayment.Properties.CollectCode, typeof(ACCOUNTINGEN.CollectPayment).Name)
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(COMMEN.Currency), typeof(COMMEN.Currency).Name), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.CollectPayment.Properties.CurrencyCode, typeof(ACCOUNTINGEN.CollectPayment).Name)
                .Equal()
                .Property(COMMEN.Currency.Properties.CurrencyCode, typeof(COMMEN.Currency).Name)
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ACCOUNTINGEN.PaymentType), typeof(ACCOUNTINGEN.PaymentType).Name), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.CollectPayment.Properties.PaymentMethodTypeCode, typeof(ACCOUNTINGEN.CollectPayment).Name)
                .Equal()
                .Property(ACCOUNTINGEN.PaymentType.Properties.PaymentTypeCode, typeof(ACCOUNTINGEN.PaymentType).Name)
                .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();
            int payerId = 0;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    billReport = new BillReport
                    {
                        TechnicalTransaction = technicalTransaction,
                        BranchName = (reader["BranchName"] != null) ? Convert.ToString(reader["BranchName"]) : "",
                        ConceptDescription = (reader["ConceptDescription"] != null) ? Convert.ToString(reader["ConceptDescription"]) : "",
                        CurrencyDescription = (reader["CurrencyDescription"] != null) ? Convert.ToString(reader["CurrencyDescription"]) : "",
                        PaymentMethod = (reader["PaymentTypeDescription"] != null) ? Convert.ToString(reader["PaymentTypeDescription"]) : "",

                        Description = (reader[ACCOUNTINGEN.Collect.Properties.Description] != null) ? Convert.ToString(reader[ACCOUNTINGEN.Collect.Properties.Description]) : "",
                        PaymentDate = (reader[ACCOUNTINGEN.Collect.Properties.RegisterDate] != null) ? Convert.ToDateTime(reader[ACCOUNTINGEN.Collect.Properties.RegisterDate]) : DateTime.Now,
                        Total = (reader[ACCOUNTINGEN.Collect.Properties.PaymentsTotal] != null) ? Convert.ToDecimal(reader[ACCOUNTINGEN.Collect.Properties.PaymentsTotal]) : 0,
                        TotalInLetters = ""
                    };
                    payerId = (reader[ACCOUNTINGEN.Collect.Properties.IndividualId] != null) ? Convert.ToInt32(reader[ACCOUNTINGEN.Collect.Properties.IndividualId]) : 0;
                }
            }

            if (payerId > 0)
            {
                DTOs.IndividualDTO person = DelegateService.accountingAccountsPayableService.GetIndividualByIndividualId(payerId);
                if (person != null)
                {
                    billReport.PayerName = person.Name;
                    billReport.PayerDocumentNumber = person.IdentificationDocument == null ? "0" : person.IdentificationDocument.Number;
                }
            }
            return billReport;
        }
    }
}
