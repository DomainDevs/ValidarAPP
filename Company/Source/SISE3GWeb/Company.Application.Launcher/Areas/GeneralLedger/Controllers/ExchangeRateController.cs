using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    [Authorize]
    [HandleError]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class ExchangeRateController : Controller
    {
        #region Public Methods

        /// <summary>
        /// ExchangeRate
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ExchangeRate()
        {
            try
            {
                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();


                return View("~/Areas/GeneralLedger/Views/ExchangeRate/ExchangeRate.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }              
        }

        /// <summary>
        /// Obtain the rate of change list
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetExchangeRate()
        {
            try
            {
                List<ExchangeRateDTO> exchangeRates = new List<ExchangeRateDTO>(); //DelegateService.commonService.GetExchangeRates();TODO LFREIRE no existe método en common

                var exchangeRatesOrder = (from item in exchangeRates orderby item.RateDate descending select item).ToList();
                

                List <object> exchangeRatesResponses = new List<object>();

                foreach (ExchangeRateDTO exchangeRate in exchangeRatesOrder)
                {
                    exchangeRatesResponses.Add(new
                    {
                        RateDate = exchangeRate.RateDate.Date.ToString(),
                        CurrencyCode = exchangeRate.Currency.Id,
                        CurrencyDescription = exchangeRate.Currency.Description,    
                        SellAmount = String.Format(new CultureInfo("en-US"), "{0:C4}", exchangeRate.SellAmount),                        
                        BuyAmount = String.Format(new CultureInfo("en-US"), "{0:C4}", exchangeRate.BuyAmount)
                    });
                }                
                return new UifTableResult(exchangeRatesResponses);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// Save the rate of change
        /// </summary>
        /// <param name="oExchangeRate">ExchangeRate</param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveExchangeRate(ExchangeRateDTO oExchangeRate)
        {            
            try
            {
                oExchangeRate.RateDate = Convert.ToDateTime(oExchangeRate.RateDate.Date.ToShortDateString());
                ExchangeRateDTO exchangeRate = new ExchangeRateDTO(); //DelegateService.commonService.SaveExchangeRate(oExchangeRate);TODO LFREIRE no existe método en common

                if (exchangeRate.Currency.Description == "DUPLICATED_OBJECT")
                {
                    return Json(new { success = false, result = Global.RecordAlreadyExists, exceptionType = "warning" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return new UifTableResult(exchangeRate);
                }
            }                       
            catch (Exception exception)
            {                
                    return Json(new { success = false, result = exception.Message, exceptionType = "danger"}, JsonRequestBehavior.AllowGet);         
            }
        }

        /// <summary>
        /// Update the rate of change
        /// </summary>
        /// <param name="oExchangeRate">ExchangeRate</param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateExchangeRate(ExchangeRateDTO oExchangeRate)
        {                        
            try
            {
                //ExchangeRate exchangeRate = DelegateService.commonService.UpdateExchangeRate(oExchangeRate);TODO LFREIRE no existe método en common

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Delete the rate of change
        /// </summary>
        /// <param name="rateDate">DateTime</param>
        /// <param name="currencyId">int</param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteExchangeRates(DateTime rateDate, int currencyId)
        {            
            ExchangeRateDTO oExchangeRateKey = new ExchangeRateDTO();
            oExchangeRateKey.RateDate = Convert.ToDateTime(rateDate);
            oExchangeRateKey.Currency = new CurrencyDTO() { Id = currencyId };
            try
            {                
                //DelegateService.commonService.DeleteExchangeRate(oExchangeRateKey);TODO LFREIRE no existe método en common
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {                
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}