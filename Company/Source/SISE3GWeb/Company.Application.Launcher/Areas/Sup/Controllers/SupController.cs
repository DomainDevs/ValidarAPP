using Sistran.Co.Previsora.Application.FullServices.Models;
using System;
using System.Collections.Generic;
using Sistran.Core.Framework.UIF.Web.Services;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using Sistran.Core.Framework.UIF.Web.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Sup.Controllers
{
    public class SupController : Controller
    {
        public ActionResult Sup()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetSupKey(string UserLogin)
        {
            try
            {
                List<Parameters> listParameter = new List<Parameters>();
                Parameters parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "USERSUP";
                parameter.Value = UserLogin;
                listParameter.Add(parameter);

                var ds = DelegateService.FullServices.GenericQuery("SUP.RANDOMKEY", listParameter);
                //ds = dr.GetData(listParameter, "SUP.RANDOMKEY|SISE3G");

                 List<Object> listKey = new List<Object>();

                string keyS = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string key = ds.Tables[0].Rows[0][0].ToString();
                        listKey.Add(new { SupKey = key });
                        keyS = key;
                    }
                }
                string encryptUserLogin = Helpers.EncryptHelper.EncryptSupLogin(UserLogin);
                var result = $"{ConfigurationManager.AppSettings["SupUrl"]}general/general?UserLogin={encryptUserLogin}&SUPKey={keyS}&IdApp=2";
                return new UifJsonResult(true, result);
            }
            catch(Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
            
        }
    }
}