using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.EEProvider;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using System.Collections.Generic;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Linq;
using QUO = Sistran.Core.Application.Quotation.Entities;
using SCRIPTEN = Sistran.Core.Application.Script.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using System.Data;
using Model = Sistran.Core.Application.UnderwritingServices.Models;
using System.Threading.Tasks;
using Sistran.Core.Application.Utilities.Helper;
using System;
using Sistran.Core.Application.Utilities.Managers;

namespace Sistran.Core.Application.UnderwritingServices.DAOs
{
    public class ExpenseDAO
    {

        /// <summary>
        /// Obtener la lista de Gastos
        /// </summary>
        /// <returns>Lista de Gastos consultados en DB</returns>

        public List<Expense> GetExpenses()
        {
            // Instanciar Objeto para consultar
            ExpensesView ExpenseView = new ExpensesView();

            // Constructor para consultar la vista ExpensesView
            ViewBuilder builder = new ViewBuilder("ExpensesView");

            // Instanciar objetos para el filtro
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            // Agregar filtro para Component_Class
            filter.Property(QUO.Component.Properties.ComponentClassCode, typeof(QUO.Component).Name);
            filter.Equal();
            filter.Constant(ComponentClassType.Expenses);

            //
            filter.And();

            // Agregar filtro para Component_Type
            filter.Property(QUO.Component.Properties.ComponentTypeCode, typeof(QUO.Component).Name);
            filter.Equal();
            filter.Constant(Enums.ComponentType.Expenses);

            // Agregar los Filtros al builder
            builder.Filter = filter.GetPredicate();

            // Ejecutar consulta en base al builder            
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, ExpenseView);
            }

            // Validacion de registros
            if (ExpenseView.ExpenseComponents.Count > 0)
            {
                List<QUO.Component> listComponents = ExpenseView.Components.Cast<QUO.Component>().ToList();

                List<QUO.ExpenseComponent> listExpensesComponents = ExpenseView.ExpenseComponents.Cast<QUO.ExpenseComponent>().ToList();

                List<Parameters.Entities.RateType> listRateType = ExpenseView.RateTypes.Cast<Parameters.Entities.RateType>().ToList();

                List<SCRIPTEN.RuleSet> listRuleSet = ExpenseView.RulesSet.Cast<SCRIPTEN.RuleSet>().ToList();

                List<Expense> listExpenses = new List<Expense>();

                foreach (QUO.Component item in listComponents)
                {
                    Expense Expenses = new Expense();
                    foreach (QUO.ExpenseComponent row in listExpensesComponents)
                    {
                        if (item.ComponentCode == row.ComponentCode)
                        {

                            Expenses.Abbreviation = item.TinyDescription;
                            Expenses.Description = item.SmallDescription;
                            Expenses.ComponentClass = item.ComponentClassCode;
                            Expenses.ComponentType = item.ComponentTypeCode;
                            Expenses.id = item.ComponentCode;

                            Expenses.InitiallyIncluded = row.IsInitially;
                            Expenses.Mandatory = row.IsMandatory;
                            Expenses.Rate = row.Rate;
                            Expenses.RateType = row.RateTypeCode;
                            Expenses.RuleSet = row.RuleSetId;

                            foreach (Parameters.Entities.RateType index in listRateType)
                            {
                                if (index.RateTypeCode == row.RateTypeCode)
                                {
                                    Expenses.RateTypeName = index.Description;
                                }
                            }

                            foreach (SCRIPTEN.RuleSet index in listRuleSet)
                            {
                                if (row.RuleSetId == index.RuleSetId)
                                {
                                    Expenses.RuleSetName = index.Description;
                                }
                            }

                        }
                    }
                    listExpenses.Add(Expenses);
                }

                // Retornar la lista de Expenses
                return listExpenses;
            }
            return null;
        }

        /// <summary>
        /// Consulta las reglas de negocio
        /// </summary>
        /// <returns>Lista de Reglas en DB</returns>
        public List<BusinessRuleSet> GetRulesSet()
        {
            BusinessCollection collectionBusiness = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(SCRIPTEN.RuleSet)));
            List<BusinessRuleSet> listRules = new List<BusinessRuleSet>();
            if (collectionBusiness.Count > 0)
            {
                foreach (SCRIPTEN.RuleSet item in collectionBusiness)
                {
                    BusinessRuleSet objectRuleSet = new BusinessRuleSet();
                    objectRuleSet.Id = item.RuleSetId;
                    objectRuleSet.Description = item.Description;
                    listRules.Add(objectRuleSet);
                }
            }
            return listRules;
        }
        public static decimal GetExpensByPolicyId(int policyId)
        {
            List<int> lstComp = new List<int>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUO.Component.Properties.ComponentTypeCode, typeof(QUO.Component).Name);
            filter.In();
            filter.ListValue();
            filter.Constant(Enums.ComponentType.Expenses);
            filter.EndList();
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(QUO.Component.Properties.ComponentCode, typeof(QUO.Component).Name)));
            select.Table = new ClassNameTable(typeof(QUO.Component), typeof(QUO.Component).Name);
            select.Where = filter.GetPredicate();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {

                while (reader.Read())
                {
                    lstComp.Add((int)reader[QUO.Component.Properties.ComponentCode]);
                }
            }

            decimal expenses = decimal.Zero;
            filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.PayerComp.Properties.PolicyId, typeof(ISSEN.PayerComp).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.PayerComp.Properties.ComponentCode, typeof(ISSEN.PayerComp).Name);
            filter.In();
            filter.ListValue();
            foreach (int component in lstComp)
            {
                filter.Constant(component);
            }
            filter.EndList();
            select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerComp.Properties.ComponentAmount, typeof(ISSEN.PayerComp).Name)));
            select.Table = new ClassNameTable(typeof(ISSEN.PayerComp), typeof(ISSEN.PayerComp).Name);
            select.Where = filter.GetPredicate();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {

                while (reader.Read())
                {
                    expenses += (decimal)reader[ISSEN.PayerComp.Properties.ComponentAmount];
                }
            }
            return expenses;
        }

        public Model.PayerComponent CalculateExpensesAmount(Model.PayerComponent item, Model.Policy policy, List<Model.Coverage> coverages, bool isExpenses)
        {
            decimal amount = 0;
            decimal amountLocal = 0;
            decimal baseExpensesAmount = coverages.Sum(x => x.PremiumAmount);
            object objLock = new object();
            Model.PayerComponent payerComponentExpense = new Model.PayerComponent();
            payerComponentExpense.Component = item.Component;
            payerComponentExpense.CoverageId = coverages[0].Id;
            payerComponentExpense.LineBusinessId = coverages[0].SubLineBusiness.LineBusiness.Id;
            payerComponentExpense.Rate = item.Rate = GetExpensByPolicyId(policy.Endorsement.PolicyId) * -1;
            payerComponentExpense.RateType = item.RateType;
            payerComponentExpense.DynamicProperties = item.DynamicProperties;
            object objLockExpenses = new object();
            Parallel.For(0, coverages.Count, ParallelHelper.DebugParallelFor(), m =>
                {
                    var coverage = coverages[m];
                    var totalInsuredAmount = coverage.EndorsementLimitAmount;

                    if (totalInsuredAmount != 0)
                    {
                        if (coverage.EndorsementLimitAmount != 0)
                        {
                            lock (objLockExpenses)
                            {
                                if (isExpenses)
                                {
                                    amountLocal += decimal.Round((((coverage.EndorsementLimitAmount * policy.ExchangeRate.SellAmount) * payerComponentExpense.Rate) / (totalInsuredAmount * policy.ExchangeRate.SellAmount)), QuoteManager.DecimalRound);
                                    amount += decimal.Round(((coverage.EndorsementLimitAmount * (payerComponentExpense.Rate / policy.ExchangeRate.SellAmount)) / totalInsuredAmount), QuoteManager.DecimalRound);
                                }
                                else
                                {
                                    amount += decimal.Round(((coverage.EndorsementLimitAmount * payerComponentExpense.Rate) / totalInsuredAmount), QuoteManager.DecimalRound);
                                    amountLocal += decimal.Round((((coverage.EndorsementLimitAmount * policy.ExchangeRate.SellAmount) * (payerComponentExpense.Rate * policy.ExchangeRate.SellAmount)) / (totalInsuredAmount * policy.ExchangeRate.SellAmount)), QuoteManager.DecimalRound);
                                }
                            }
                        }
                    }
                });
            lock (objLock)
            {
                payerComponentExpense.Amount += amount;
                payerComponentExpense.AmountLocal += amountLocal;
                payerComponentExpense.BaseAmount = baseExpensesAmount;
            }

            return payerComponentExpense;
        }

        public Model.PayerComponent CalculateExpensesAmountByCoverage(Model.PayerComponent item, Model.Policy policy, List<Model.Coverage> coverages, bool isExpenses, int factor =-1)
        {
            decimal amount = 0;
            decimal amountLocal = 0;
            decimal baseExpensesAmount = coverages.Sum(x => x.PremiumAmount);
            object objLock = new object();
            Model.PayerComponent payerComponentExpense = new Model.PayerComponent();
            payerComponentExpense.Component = item.Component;
            payerComponentExpense.CoverageId = coverages[0].Id;
            payerComponentExpense.LineBusinessId = coverages[0].SubLineBusiness.LineBusiness.Id;
            payerComponentExpense.Rate = item.Rate = GetExpensByCoverage(coverages.Last().Id, policy.Endorsement.PolicyId, policy.ExchangeRate.SellAmount) * factor;
            payerComponentExpense.RateType = item.RateType;
            payerComponentExpense.DynamicProperties = item.DynamicProperties;
            object objLockExpenses = new object();
            Parallel.For(0, coverages.Count, ParallelHelper.DebugParallelFor(), m =>
                {
                    var coverage = coverages[m];
                    var totalInsuredAmount = coverage.EndorsementLimitAmount;

                    if (totalInsuredAmount != 0)
                    {
                        if (coverage.EndorsementLimitAmount != 0)
                        {
                            lock (objLockExpenses)
                            {
                                if (isExpenses)
                                {
                                    amountLocal += decimal.Round((((coverage.EndorsementLimitAmount * policy.ExchangeRate.SellAmount) * payerComponentExpense.Rate) / (totalInsuredAmount * policy.ExchangeRate.SellAmount)), QuoteManager.DecimalRound);
                                    amount += decimal.Round(((coverage.EndorsementLimitAmount * (payerComponentExpense.Rate / policy.ExchangeRate.SellAmount)) / totalInsuredAmount), QuoteManager.DecimalRound);
                                }
                                else
                                {
                                    amount += decimal.Round(((coverage.EndorsementLimitAmount * payerComponentExpense.Rate) / totalInsuredAmount), QuoteManager.DecimalRound);
                                    amountLocal += decimal.Round((((coverage.EndorsementLimitAmount * policy.ExchangeRate.SellAmount) * (payerComponentExpense.Rate * policy.ExchangeRate.SellAmount)) / (totalInsuredAmount * policy.ExchangeRate.SellAmount)), QuoteManager.DecimalRound);
                                }
                            }
                        }
                    }
                });
            lock (objLock)
            {
                payerComponentExpense.Amount += amount;
                payerComponentExpense.AmountLocal += amountLocal;
                payerComponentExpense.BaseAmount = baseExpensesAmount;
            }

            return payerComponentExpense;
        }

        public static decimal GetExpensByCoverage(int coverageId, int policyId, decimal sellAmount)
        {
            List<int> lstComp = new List<int>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUO.Component.Properties.ComponentTypeCode, typeof(QUO.Component).Name);
            filter.In();
            filter.ListValue();
            filter.Constant(Enums.ComponentType.Expenses);
            filter.EndList();
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(QUO.Component.Properties.ComponentCode, typeof(QUO.Component).Name)));
            select.Table = new ClassNameTable(typeof(QUO.Component), typeof(QUO.Component).Name);
            select.Where = filter.GetPredicate();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {

                while (reader.Read())
                {
                    lstComp.Add((int)reader[QUO.Component.Properties.ComponentCode]);
                }
            }

            decimal expenses = decimal.Zero;
            filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.PayerComp.Properties.CoverageId, typeof(ISSEN.PayerComp).Name);
            filter.Equal();
            filter.Constant(coverageId);
            filter.And();
            filter.Property(ISSEN.PayerComp.Properties.PolicyId, typeof(ISSEN.PayerComp).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.PayerComp.Properties.ComponentCode, typeof(ISSEN.PayerComp).Name);
            filter.In();
            filter.ListValue();
            foreach (int component in lstComp)
            {
                filter.Constant(component);
            }
            filter.EndList();
            select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerComp.Properties.ComponentAmount, typeof(ISSEN.PayerComp).Name)));
            select.AddSelectValue(new SelectValue(new Column(ISSEN.PayerComp.Properties.ComponentAmountLocal, typeof(ISSEN.PayerComp).Name)));
            select.Table = new ClassNameTable(typeof(ISSEN.PayerComp), typeof(ISSEN.PayerComp).Name);
            select.Where = filter.GetPredicate();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {

                while (reader.Read())
                {
                    expenses += ((decimal)reader[ISSEN.PayerComp.Properties.ComponentAmountLocal] / sellAmount);
                }
            }
            return expenses;
        }
    }
}