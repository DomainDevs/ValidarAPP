using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Security;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using ENUMSEC = Sistran.Core.Application.SecurityServices.EEProvider.Enums;

namespace Sistran.Core.Framework.UIF.Web.Controllers
{
    public class LayoutController : Controller
    {
        private static Dictionary<string, string> resourceGlb = new Dictionary<string, string>();
        private static Dictionary<string, string> resourceGlbPerson = new Dictionary<string, string>();
        private static Dictionary<string, string> resourceGlobal = new Dictionary<string, string>();
        //private static ConcurrentDictionary<int, KeyValuePair<string, IList<Sistran.Core.Framework.UIF2.Services.Security.Module>>> modulesBase = new ConcurrentDictionary<int, KeyValuePair<string, IList<Sistran.Core.Framework.UIF2.Services.Security.Module>>>();
        private static ConcurrentDictionary<int, Accesses> modulesBase = new ConcurrentDictionary<int, Accesses>();

        private static string languageResource;
        private ISecurityManager securityManager = new SecurityManager();

        public ActionResult SideBar()
        {
            ViewBag.Modules = securityManager.GetModules(User.Identity.Name);

            return View();
        }
        public ActionResult GetModules()
        {
            try
            {  
                var module = DelegateService.authorizationProviders.GetModulesAccess(User.Identity.Name);

                return Json(module, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                ViewBag.Modules = null;
                return View();
            }
        }
        /// <summary>
        ///Obtener Menu
        /// </summary>
        /// <returns></returns>
        public ActionResult SideBarR2()
        {
            //ViewBag.HashedUserName = AuthorizeUIF2.GetHash(User.Identity.Name);

            try
            {
                ViewBag.Modules = securityManager.GetModules(User.Identity.Name);
                if (User.Identity.Name == "" || User.Identity.Name == null)
                {
                    ViewBag.Modules = null;
                    return View();
                }
              
                var levelOperation = DelegateService.AuthenticationProviders.GetLevelOperation();
                ViewBag.LevelOpetarion = levelOperation;
               
             
                return View();
            }
            catch (TimeoutException)
            {
                return View();
            }
            catch (CommunicationException)
            {
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Servicio no Disponible");
                ViewBag.Modules = null;
                return View();
            }

            return View();
        }

        public JsonResult GetResources()
        {
            string userLanguage = System.Web.HttpContext.Current.Request.UserLanguages[0];
            CultureInfo cultureInfo = new CultureInfo(userLanguage.Substring(0, 2));
            if (resourceGlb.Count == 0 || languageResource != cultureInfo.Name)
            {
                string nameResoruces = "Sistran.Core.Framework.UIF.Web.App_GlobalResources.Language";
                System.Resources.ResourceManager rm = new System.Resources.ResourceManager(nameResoruces, this.GetType().Assembly);
                var entry = rm.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true).OfType<DictionaryEntry>(); resourceGlb = entry.Cast<DictionaryEntry>().ToDictionary(x => x.Key.ToString(), x => x.Value.ToString());

                languageResource = cultureInfo.Name;
            }
            return Json(resourceGlb, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGlobalResources()
        {
            string userLanguage = System.Web.HttpContext.Current.Request.UserLanguages[0];
            CultureInfo cultureInfo = new CultureInfo(userLanguage.Substring(0, 2));
            if (resourceGlobal.Count == 0 || languageResource != cultureInfo.Name)
            {
                string nameResoruces = "Sistran.Core.Framework.UIF.Web.App_GlobalResources.Global";
                System.Resources.ResourceManager rm = new System.Resources.ResourceManager(nameResoruces, this.GetType().Assembly);
                var entry = rm.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true).OfType<DictionaryEntry>(); resourceGlobal = entry.Cast<DictionaryEntry>().ToDictionary(x => x.Key.ToString(), x => x.Value.ToString());

                languageResource = cultureInfo.Name;
            }
            return Json(resourceGlobal, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSession()
        {
            int sessionTimeOut;
            try
            {
                sessionTimeOut = Convert.ToInt32(DelegateService.commonService.GetKeyApplication("SessionTimeOut"));
                return new UifJsonResult(true, sessionTimeOut);
            }
            catch (Exception ex)
            {

                return new UifJsonResult(false, ex.Message);
            }

        }
        public ActionResult GetSessionWarning()
        {
            int sessionWarning;
            try
            {
                sessionWarning = Convert.ToInt32(DelegateService.commonService.GetKeyApplication("SessionWarning"));
                return new UifJsonResult(true, sessionWarning);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        public ActionResult ForbiddenPage()
        {
            return View();
        }
        public JsonResult GetResourcesPerson()
        {
            string userLanguage = System.Web.HttpContext.Current.Request.UserLanguages[0];
            CultureInfo cultureInfo = new CultureInfo(userLanguage.Substring(0, 2));
            if (resourceGlbPerson.Count == 0 || languageResource != cultureInfo.Name)
            {
                string nameResoruces = "Sistran.Core.Framework.UIF.Web.App_GlobalResources.LanguagePerson";
                System.Resources.ResourceManager rm = new System.Resources.ResourceManager(nameResoruces, this.GetType().Assembly);
                var entry = rm.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true).OfType<DictionaryEntry>();
                resourceGlbPerson = entry.Cast<DictionaryEntry>().ToDictionary(x => x.Key.ToString(), x => x.Value.ToString());

                languageResource = cultureInfo.Name;
            }
            return Json(resourceGlbPerson, JsonRequestBehavior.AllowGet);
        }

        #region obtener accesos
        public static int GetAccessByUrl(string url)
        {
            return GetModules(url);
        }

        //public static int GetAccessByUrlWorkFlow(string url, int menu)
        //{
        //    return GetModulesWorkFlow(url, menu);
        //}


        //private static int GetModulesWorkFlow(string url, int menu)
        //{
        //    int accesId = -1;
        //    object obj = new object();
        //    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        //    if (modulesBase != null && modulesBase.Any() && modulesBase.ContainsKey(SessionHelper.GetUserId()) && modulesBase[SessionHelper.GetUserId()].modules != null)
        //    {
        //        int row = 0;
        //        string urlBase = modulesBase[SessionHelper.GetUserId()].LevelId + url;
        //        while (row < modulesBase[SessionHelper.GetUserId()].modules?.Count && accesId == -1)
        //        {
        //            try
        //            {
        //                if (accesId == -1)
        //                {
        //                    var module = modulesBase[SessionHelper.GetUserId()].modules[row];
        //                    if (module.Id == menu)
        //                    {
        //                        if (module.SubModules != null && module.SubModules.Any())
        //                        {
        //                            int row2 = 0;
        //                            while (row2 < module.SubModules?.Count && accesId == -1)
        //                            {
        //                                if (accesId == -1)
        //                                {
        //                                    var submodule = module.SubModules[row2];
        //                                    if (submodule.Path == urlBase)
        //                                    {
        //                                        lock (obj)
        //                                        {
        //                                            accesId = submodule.Id;
        //                                            break;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        if (submodule.SubModules != null && submodule.SubModules.Any())
        //                                        {
        //                                            for (int row3 = 0; row3 < submodule.SubModules?.Count; row3++)
        //                                            {
        //                                                var acceso = submodule.SubModules[row3];
        //                                                if (acceso.Path == urlBase)
        //                                                {
        //                                                    lock (obj)
        //                                                    {
        //                                                        accesId = acceso.Id;
        //                                                        break;
        //                                                    }
        //                                                }

        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                row2++;

        //                            }
        //                        }
        //                    }
        //                }

        //                row++;
        //            }
        //            catch (Exception)
        //            {
        //                Debug.WriteLine("Errro Obteniendo Accesos");
        //            }


        //        }//);
        //    }
        //    else
        //    {
        //        throw new Exception("No existen Modulos");
        //    }
        //    return accesId;

        //}

        private static int GetModules(string url)
        {
            int accesId = -1;
            object obj = new object();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            if (modulesBase != null && modulesBase.Any() && modulesBase.ContainsKey(SessionHelper.GetUserId()) && modulesBase[SessionHelper.GetUserId()].modules != null)
            {
                string urlBase = modulesBase[SessionHelper.GetUserId()].LevelId + modulesBase[SessionHelper.GetUserId()].UrlBase;
                for (int row = 0; row < modulesBase[SessionHelper.GetUserId()].modules?.Count; row++)
                {
                    try
                    {
                        var module = modulesBase[SessionHelper.GetUserId()].modules[row];
                        if (module.Path == urlBase)
                        {
                            lock (obj)
                            {
                                accesId = module.Id;
                                break;
                            }
                            //cancellationTokenSource.Cancel();
                            //loopState.Break();
                        }
                        else
                        {
                            if (module.SubModules != null && module.SubModules.Any())
                            {
                                var accesIdSubModule = LoadModules(module.SubModules, urlBase);
                                if (accesIdSubModule != -1)
                                {
                                    accesId = accesIdSubModule;
                                    break;
                                    //loopState.Break();
                                }

                            }
                        }
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Errro Obteniendo Accesos");
                    }


                }//);
            }           
            return accesId;

        }
        public static int LoadModules(List<Sistran.Core.Framework.UIF2.Services.Security.Module> module, string url, int accesId = -1)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            object objLock = new object();
            if (accesId == -1)
            {
                Parallel.For(0, module.Count, (row, loopState) =>
                {
                    try
                    {
                        var moduleBase = module[row];
                        if (moduleBase.Path == url)
                        {
                            lock (objLock)
                            {
                                accesId = moduleBase.Id;
                            }
                            cancellationTokenSource.Cancel();
                            loopState.Break();
                        }
                        else
                        {
                            if (moduleBase.SubModules != null && moduleBase.SubModules.Any())
                            {
                                var accesIdSubModule = LoadModules(moduleBase.SubModules, url);
                                accesId = accesIdSubModule;
                                if (accesIdSubModule != -1)
                                {
                                    loopState.Break();
                                }

                            }
                        }
                    }
                    catch (Exception)
                    {

                        Debug.WriteLine("Errro Obteniendo Accesos");
                    }
                });
                return accesId;
            }
            return accesId;
        }
        #region temporal
        private static int GetModulesAll(string url)
        {
            int accesId = -1;
            object obj = new object();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            string urlBase = modulesBase[SessionHelper.GetUserId()].LevelId + url;
            if (modulesBase != null && modulesBase.Any() && modulesBase[SessionHelper.GetUserId()].modules != null)
            {

                for (int row = 0; row < modulesBase[SessionHelper.GetUserId()].modules?.Count; row++)
                {
                    try
                    {
                        var module = modulesBase[SessionHelper.GetUserId()].modules[row];
                        if (module.Path == urlBase)
                        {
                            lock (obj)
                            {
                                accesId = module.Id;
                            }

                        }
                        else
                        {
                            if (module.SubModules != null && module.SubModules.Any())
                            {
                                var accesIdSubModule = LoadModules(module.SubModules, urlBase);
                                if (accesIdSubModule != -1)
                                {
                                    accesId = accesIdSubModule;
                                }

                            }
                        }
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Errro Obteniendo Accesos");
                    }


                }
            }
            else
            {
                throw new Exception("No existen Modulos");
            }
            return accesId;

        }
        #endregion
        #endregion

    }

    internal class Accesses
    {
        internal string LevelId { get; set; }

        internal string UrlBase { get; set; }

        internal IList<Sistran.Core.Framework.UIF2.Services.Security.Module> modules { get; set; }
    }
}
