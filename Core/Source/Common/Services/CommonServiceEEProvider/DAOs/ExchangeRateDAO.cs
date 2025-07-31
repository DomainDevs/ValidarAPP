using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class ExchangeRateDAO
    {
        /// <summary>
        /// Obtener el importe de cambio por fecha y moneda
        /// </summary>
        /// <param name="rateDate"></param>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        public COMMML.ExchangeRate GetExchangeRateByRateDateCurrencyId(DateTime rateDate, int currencyId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.ExchangeRate.Properties.RateDate, "er");
            filter.LessEqual();
            filter.Constant(rateDate);
            filter.And();
            filter.Property(COMMEN.ExchangeRate.Properties.CurrencyCode, "er");
            filter.Equal();
            filter.Constant(currencyId);
            COMMML.ExchangeRate exchangeRate = new COMMML.ExchangeRate();

            List<COMMEN.ExchangeRate> exchangeRates = new List<COMMEN.ExchangeRate>();
            List<COMMEN.Currency> currencies = new List<COMMEN.Currency>();

            BusinessCollection collectionExchangeRates = DataFacadeManager.GetObjects(typeof(COMMEN.ExchangeRate), filter.GetPredicate());
            BusinessCollection collectionCurrencies = DataFacadeManager.GetObjects(typeof(COMMEN.Currency));

            collectionExchangeRates.ForEach(x => exchangeRates.Add((COMMEN.ExchangeRate)x));
            collectionCurrencies.ForEach(x => currencies.Add((COMMEN.Currency)x));

            if (collectionExchangeRates != null)
            {
                COMMEN.ExchangeRate entityExchangeRate = exchangeRates.OrderByDescending(x => x.RateDate).First();
                exchangeRate.RateDate = Convert.ToDateTime(entityExchangeRate.RateDate);
                exchangeRate.SellAmount = Convert.ToDecimal(entityExchangeRate.SellAmount);
                exchangeRate.Currency = new COMMML.Currency
                {
                    Id = currencyId,
                    Description = currencies.Where(x => x.CurrencyCode == currencyId).First().Description
                };
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetExchangeRateByRateDateCurrencyId");
            return exchangeRate;
        }

        /// <summary>
        /// Obtener el importe de cambio por fecha y moneda
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        public bool CalculateExchangeRateTolerance(decimal newRate, int currencyId)
        {
            decimal currencyRate;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.ExchangeRateTolerance.Properties.CurrencyCode, "er");
            filter.Equal();
            filter.Constant(currencyId);
            filter.And();
            filter.Property(COMMEN.ExchangeRateTolerance.Properties.EndDate).IsNull();
            filter.And();
            filter.Property(COMMEN.ExchangeRateTolerance.Properties.InitialDate);
            filter.LessEqual();
            filter.Constant(DateTime.Now);

            COMMML.ExchangeRate exchangeRate = GetExchangeRateByCurrencyId(currencyId);
            currencyRate = exchangeRate.SellAmount;

            List<COMMEN.ExchangeRateTolerance> entityExchangeRates = new List<COMMEN.ExchangeRateTolerance>();

            BusinessCollection collectionExchangeRatesTolerance = DataFacadeManager.GetObjects(typeof(COMMEN.ExchangeRateTolerance), filter.GetPredicate());

            collectionExchangeRatesTolerance.ForEach(x => entityExchangeRates.Add((COMMEN.ExchangeRateTolerance)x));
            if (collectionExchangeRatesTolerance != null && collectionExchangeRatesTolerance.Count > 0)
            {
                COMMEN.ExchangeRateTolerance entityExchangeRateTolerance = entityExchangeRates.FirstOrDefault();
                decimal percentage = Convert.ToDecimal(entityExchangeRateTolerance.Percentage);

                decimal maximumValue = (1 + (percentage / 100)) * currencyRate;
                decimal minimumValue = (1 - (percentage / 100)) * currencyRate;
                return (newRate >= minimumValue && newRate <= maximumValue);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.CalculateExchangeRateTolerance");
            return false;
        }

        /// <summary> 
        /// Obtener un listado de tasas de cambio a partir de la moneda
        /// </summary>
        /// <param name="currencyId">Identificador de la moneda</param>
        /// <returns>Listado de tasas de cambio</returns>
        public COMMML.ExchangeRate GetExchangeRateByCurrencyId(int currencyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.ExchangeRate.Properties.CurrencyCode, typeof(COMMEN.ExchangeRate).Name);
            filter.Equal();
            filter.Constant(currencyId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.ExchangeRate), filter.GetPredicate()));

            List<COMMML.ExchangeRate> exchangeRatesList = ModelAssembler.CreateExchangeRates(businessCollection);

            COMMML.ExchangeRate exchangeRate = exchangeRatesList.OrderByDescending(p => p.RateDate).FirstOrDefault();
            return exchangeRate;
        }

        public List<COMMML.ExchangeRate> GetExchangeRates(DateTime? dateCumulus = null, int? CurrecyCode = null)
        {
            BusinessCollection businessCollection;
            List<COMMML.ExchangeRate> exchangeRates = new List<COMMML.ExchangeRate>();
            if (dateCumulus != null && CurrecyCode != null)
            {
                dateCumulus = dateCumulus.Value.AddDays(1);
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(COMMEN.ExchangeRate.Properties.RateDate)));
                select.AddSelectValue(new SelectValue(new Column(COMMEN.ExchangeRate.Properties.CurrencyCode)));
                select.AddSelectValue(new SelectValue(new Column(COMMEN.ExchangeRate.Properties.SellAmount)));
                select.AddSelectValue(new SelectValue(new Column(COMMEN.ExchangeRate.Properties.BuyAmount)));
                select.Table = new ClassNameTable(typeof(COMMEN.ExchangeRate));
                select.AddSortValue(new SortValue(new Column(COMMEN.ExchangeRate.Properties.RateDate), SortOrderType.Descending));

                ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
                where.Property(COMMEN.ExchangeRate.Properties.RateDate, typeof(COMMEN.ExchangeRate).Name);
                where.Less();
                where.Constant(dateCumulus);
                where.And();
                where.Property(COMMEN.ExchangeRate.Properties.CurrencyCode, typeof(COMMEN.ExchangeRate).Name);
                where.Equal();
                where.Constant(CurrecyCode);
                select.MaxRows = 1;
                select.Where = where.GetPredicate();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    while (reader.Read())
                    {
                        COMMML.ExchangeRate exchangeRate = new COMMML.ExchangeRate
                        {
                            RateDate = DateTime.Parse(reader["RateDate"].ToString()),
                            Currency = new COMMML.Currency
                            {
                                Id = int.Parse(reader["CurrencyCode"].ToString()),
                                Description = "test"
                            },
                            SellAmount = decimal.Parse(reader["SellAmount"].ToString()),
                            BuyAmount = decimal.Parse(reader["BuyAmount"].ToString()),
                        };
                        exchangeRates.Add(exchangeRate);
                    }
                }
            }
            else
            {
                businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.ExchangeRate)));
                exchangeRates = ModelAssembler.CreateExchangeRates(businessCollection);
                return exchangeRates;
            }
            return exchangeRates;
        }
    }
}
