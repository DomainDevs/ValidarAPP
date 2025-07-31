using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System;
using MOS = Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Product.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using UPV1 = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Text;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using crtEnum = Sistran.Core.Application.CommonService.Enums.CoveredRiskType;
using Newtonsoft.Json;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Helpers;
using System.Collections;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.ModelServices.Models.Product;
using AutoMapper;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Models.Param;
using System.Web.Http;
using System.IO;
using Sistran.Core.Application.ProductServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Controllers
{
    public class ProductController : Controller
    {
        private static ProductModelsView productModelsView = new ProductModelsView();

        private static List<Sistran.Core.Application.CommonService.Models.Parameter> parameters = new List<Sistran.Core.Application.CommonService.Models.Parameter>();

        public ActionResult Product()
        {
            try
            {
                productModelsView = new ProductModelsView();
                productModelsView.CurrentDate = DelegateService.commonService.GetDate();
                return View(productModelsView);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        #region Combos
        public UifSelectResult GetPrefixes()
        {
            try
            {
                List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();
                return new UifSelectResult(prefixes.OrderBy(x => x.Description));

            }
            catch (System.Exception)
            {

                return new UifSelectResult("");
            }
        }
        public UifSelectResult GetRiskTypeByPrefixId(int prefixId)
        {
            try
            {
                List<RiskType> riskType = DelegateService.underwritingService.GetRiskTypeByPrefixId(prefixId);
                return new UifSelectResult(riskType.OrderBy(x => x.Description));
            }
            catch (System.Exception)
            {

                return new UifSelectResult("");
            }
        }
        #endregion

        public string DateNow()
        {
            return DelegateService.commonService.GetDate().ToString("dd/MM/yyyy");
        }

        public PartialViewResult Currency()
        {
            return PartialView();
        }
        public PartialViewResult PartialViewPolicyType()
        {
            return PartialView();
        }
        public PartialViewResult PartialViewRiskType()
        {
            return PartialView();
        }
        ///// <summary>
        ///// Consulta el producto por Id
        ///// </summary>
        ///// <param name="productId">Identificador del producto </param>
        ///// <returns>Producto</returns>
        //public UifJsonResult GetProductsByProductId(int productId)
        //{
        //    try
        //    {
        //        CompanyProduct product = DelegateService.underwritingService.GetCompanyProductById(productId);
        //        if (product != null)
        //        {
        //            return new UifJsonResult(true, product);
        //        }
        //        else
        //        {
        //            return new UifJsonResult(false, App_GlobalResources.Language.NoExistentProduct);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return new UifJsonResult(false, App_GlobalResources.Language.NoExistentProduct);
        //    }


        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="productId"></param>
        ///// <returns></returns>
        //public UifJsonResult GetProductsByDeductibleByCOverage(int productId)
        //{
        //    try
        //    {
        //        CompanyProduct product = DelegateService.underwritingService.GetCompanyProductById(productId);
        //        if (product != null)
        //        {
        //            return new UifJsonResult(true, product);
        //        }
        //        else
        //        {
        //            return new UifJsonResult(false, App_GlobalResources.Language.NoExistentProduct);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return new UifJsonResult(false, App_GlobalResources.Language.NoExistentProduct);
        //    }


        //}

        /// <summary>
        /// Gets the policy types by product identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        public UifJsonResult GetPolicyTypesByProductId(int productId)
        {
            try
            {
                List<PolicyType> policyTypes = new List<PolicyType>();
                policyTypes = DelegateService.commonService.GetPolicyTypesByProductId(productId);
                if (policyTypes.Count > 0)
                {
                    return new UifJsonResult(true, policyTypes.OrderBy(x => x.Description));
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryPolicyTypesByProductId);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryPolicyTypesByProductId);
            }
        }
        /// <summary>
        /// Gets the policy types by product identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        public ActionResult GetPolicyTypesByPrefixId(int prefixId)
        {
            try
            {
                List<PolicyType> policyTypes = new List<PolicyType>();
                policyTypes = DelegateService.commonService.GetPolicyTypesByPrefixId(prefixId);
                if (policyTypes.Count > 0)
                {
                    return new UifSelectResult(policyTypes.OrderBy(x => x.Description), policyTypes.Where(b => b.IsDefault == true).Select(b => b.Id).FirstOrDefault());
                }
                else
                {
                    return new UifSelectResult("");
                }
            }
            catch (Exception)
            {
                return new UifSelectResult("");
            }
        }
        /// <summary>
        /// Dates the now.
        /// </summary>
        /// <returns></returns>

        #region Grabado Productos
        /// <summary>
        /// Saves the product.
        /// </summary>
        /// <param name="productModelsView">The product models view.</param>
        /// <returns></returns>
        /// [Bind(Exclude = "Id")]
        public ActionResult SaveProduct(ProductModelsView productModelsView)
        {
            try
            {
                if (productModelsView.Id == 0)
                {
                    ModelState.Remove("productModelsView.Id");
                }

                //if (productModelsView.ProductCoveredRisks[0].Id == (int)crtEnum.Vehicle && (productModelsView.Product2G == 0 || productModelsView.Product2G == null))
                //{
                //    return new UifJsonResult(false, App_GlobalResources.Language.ValidateMessageProduct2G);
                //}

                if (ModelState.IsValid)
                {
                    var simpleModel = ModelAssembler.CreateProductServiceModel(productModelsView);
                    var resultOperation = DelegateService.productParamService.SaveProduct(simpleModel);

                    productModelsView = ModelAssembler.CreateProductViewModel(resultOperation);
                    //CompanyProductParametrization product = ModelAssembler.CreateProduct(productModelsView);
                    //if (productModelsView.Currencies == null)
                    //{
                    //    product.Currencies = new List<Currency>();
                    //    Currency currency = new Currency { Id = productModelsView.Currency };
                    //    product.Currencies.Add(currency);
                    //}
                    //if (productModelsView.PolicyTypes == null)
                    //{
                    //    product.PolicyTypes = new List<PolicyType>();
                    //    Prefix Prefix = new Prefix { Id = product.Prefix.Id };
                    //    PolicyType policyType = new PolicyType { Id = productModelsView.PolicyType, Prefix = Prefix };
                    //    product.PolicyTypes.Add(policyType);
                    //}
                    //if (productModelsView.ProductCoveredRisks == null)
                    //{
                    //    product.ProductCoveredRisks = new List<MOS.CoveredRisk>();
                    //    MOS.CoveredRisk coveredRisk = new MOS.CoveredRisk { CoveredRiskType = (CoveredRiskType)productModelsView.RiskType, MaxRiskQuantity = productModelsView.MaximumNumberRisk };
                    //    product.ProductCoveredRisks.Add(coveredRisk);
                    //}
                    //if (product.Id > 0)
                    //{
                    //    try
                    //    {
                    //        List<MOS.TableResult> tableResult = product.ValidateModelProduct();//(List<MOS.TableResult>)ObjectHelper<MOS.Product>.Compare(product.ProductOld, product, new string[] { "ProductOld", "Product" });
                    //        product.TableResult = tableResult;
                    //        if (tableResult == null || tableResult.Count() == 0)
                    //        {
                    //            //return new UifJsonResult(true, product);
                    //        }
                    //        else
                    //        {
                    //            for (int i = 0; i < product.TableResult.Count; i++)
                    //            {
                    //                product.TableResult[i].UserId = SessionHelper.GetUserId();
                    //            }

                    //        }
                    //    }
                    //    catch
                    //    {

                    //    }
                    //}

                    //product = DelegateService.underwritingService.CreateCompanyFullProduct(product);
                    //product.CloneObject();
                    return new UifJsonResult(true, productModelsView);
                }
                else
                {
                    string errorMessage = GetErrorMessages();
                    return new UifJsonResult(false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveProduct);
                }
            }
        }
        #endregion

        #region Mensajes Errores
        private string GetErrorMessages()
        {
            int Id = 0;
            StringBuilder sb = new StringBuilder();
            foreach (ModelState item in ModelState.Values)
            {

                if (item.Errors.Count > 0)
                {
                    sb.Append(item.Errors[0].ErrorMessage).Append(", ");
                }
                Id = Id + 1;
            }
            return sb.ToString().Remove(sb.ToString().Length - 2);
        }
        #endregion

        #region Combos Perifericos
        public ActionResult CoverageGetRiskTypesByPrefixId(int prefixId)
        {
            try
            {
                List<RiskType> riskType = DelegateService.underwritingService.GetRiskTypeByPrefixId(prefixId);
                if (riskType != null)
                {
                    return new UifJsonResult(true, riskType.OrderBy(x => x.Description));
                }

                return new UifJsonResult(false, App_GlobalResources.Language.NoPaymentMethod);
            }
            catch (System.Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
        public ActionResult CoverageGetGroupCoveragesByRiskTypeId(int riskTypeId)
        {
            try
            {
                List<GroupCoverage> groupCoverage = DelegateService.underwritingService.GetGroupCoveragesByRiskTypeId(riskTypeId);
                return new UifSelectResult(groupCoverage.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult("");
            }
        }

        public ActionResult CoverageGetInsuredObjectsByPrefixId(int prefixId, string insuredObjectsAdd)
        {
            try
            {
                List<CompanyInsuredObject> insuredObjects = DelegateService.underwritingService.GetCompanyInsuredObjectByPrefixIdList(prefixId);

                if (insuredObjectsAdd.Length > 0)
                {
                    string[] idInsuredObjects = insuredObjectsAdd.Split(',');
                    insuredObjects = insuredObjects.Where(x => !idInsuredObjects.Any(y => Convert.ToInt32(y) == x.Id)).ToList();
                }

                return new UifSelectResult(insuredObjects.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifSelectResult(null);
            }
        }

        /// <summary>
        /// Gets the currencies.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCurrencies()
        {
            List<Currency> currencies = null;
            try
            {
                if (currencies == null)
                {
                    currencies = DelegateService.commonService.GetCurrencies();
                }
                return new UifJsonResult(true, currencies.OrderBy(x => x.Description));
            }
            catch (System.Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        /// Gets the currencies.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetProductCurrencies(int productId)
        {
            List<ProductCurrency> currencies = null;
            try
            {
                if (currencies == null)
                {
                    currencies = DelegateService.coreProductService.GetProductCurrencies(productId);
                }
                return new UifJsonResult(true, currencies.OrderBy(x => x.Id));
            }
            catch (System.Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }
        /// <summary>
        /// Gets the agents.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAgents(int productId, int[] currentAgents, int prefixId)
        {
            try
            {
                List<AgentDataTableView> responseAgents = new List<AgentDataTableView>();
                List<UPV1.Agent> Agents = new List<UPV1.Agent>();
                Agents = DelegateService.uniquePersonServiceV1.GetAgentsByPrefix(prefixId);
                foreach (UPV1.Agent item in Agents)
                {
                    if (item.DateDeclined > DateTime.MinValue)
                    {

                    }
                    else
                    {
                        if (currentAgents == null)
                        {
                            //AgentTypeData agentTypeData = new AgentTypeData();
                            //agentTypeData.Id = item.AgentType.Id;
                            //agentTypeData.Description = item.AgentType.Description;
                            //agentTypeData.SmallDescription = item.AgentType.SmallDescription;
                            AgentDataTableView itemResponse = new AgentDataTableView();
                            itemResponse.IndividualId = item.IndividualId;
                            itemResponse.ProductId = productId;
                            itemResponse.LockerId = 0;
                            //itemResponse.AgencyComiss = null;
                            itemResponse.FullName = item.FullName;
                            //itemResponse.AgentType = agentTypeData;
                            itemResponse.StatusTypeService = StatusTypeService.Create;
                            itemResponse.DataItem = "<div class=\"item\"><div class=\"display columns\"><div class=\"template\">" +
                        "<div class=\"success\"><strong>" + item.FullName + "</strong></div>" +
                        "<div><strong></strong></div>" +
                        "<div class=\"hidden\">" + 2 + "</div>" +
                        "</div><div class=\"toolbar buttons\"><div class=\"card-button edit-button\" onclick=\"return false\"><i class=\"fa fa-pencil\"></i></div><div class=\"card-button delete-button\" onclick=\"return false\"><i class=\"fa fa-trash\"></i></div></div></div></div>";
                            responseAgents.Add(itemResponse);
                        }
                        else
                        {
                            bool exist = currentAgents.Contains(item.IndividualId);
                            if (exist == false)
                            {
                                //AgentTypeData agentTypeData = new AgentTypeData();
                                //agentTypeData.Id = item.AgentType.Id;
                                //agentTypeData.Description = item.AgentType.Description;
                                //agentTypeData.SmallDescription = item.AgentType.SmallDescription;
                                AgentDataTableView itemResponse = new AgentDataTableView();
                                itemResponse.IndividualId = item.IndividualId;
                                itemResponse.ProductId = productId;
                                itemResponse.LockerId = 0;
                                //itemResponse.AgencyComiss = null;
                                itemResponse.FullName = item.FullName;
                                //itemResponse.AgentType = agentTypeData;
                                itemResponse.StatusTypeService = StatusTypeService.Create;
                                itemResponse.DataItem = "<div class=\"item\"><div class=\"display columns\"><div class=\"template\">" +
                            "<div class=\"success\"><strong>" + item.FullName + "</strong></div>" +
                            "<div><strong></strong></div>" +
                            "<div class=\"hidden\">" + 2 + "</div>" +
                            "</div><div class=\"toolbar buttons\"><div class=\"card-button edit-button\" onclick=\"return false\"><i class=\"fa fa-pencil\"></i></div><div class=\"card-button delete-button\" onclick=\"return false\"><i class=\"fa fa-trash\"></i></div></div></div></div>";
                                responseAgents.Add(itemResponse);
                            }
                        }

                    }
                }
                var jsonResult = Json(responseAgents, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }
        /// <summary>
        /// Obtiene toda la informacion de monedas para el control de Combo-Select.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public ActionResult GetCurrenciesSelect()
        {
            try
            {
                List<Currency> currencies = DelegateService.commonService.GetCurrencies();

                return new UifSelectResult(currencies.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult("");
            }
        }
        /// <summary>
        /// Obtiene toda la informacion de metodos de pago.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public ActionResult GetPaymentMethodSelect()
        {
            try
            {
                List<PaymentMethod> paymentMethod = DelegateService.commonService.GetPaymentMethods();
                if (paymentMethod != null)
                {
                    return new UifSelectResult(paymentMethod.OrderBy(x => x.Description));
                }

                return new UifSelectResult("");
            }
            catch (Exception)
            {
                return new UifSelectResult("");
            }
        }
        /// <summary>
        /// Obtener grupo de coberturas
        /// </summary>
        /// <param name=""></param>
        /// <returns>Coberturas</returns>
        public ActionResult GetAllGroupCoverages()
        {
            try
            {
                List<GroupCoverage> groupCoverage = DelegateService.underwritingService.GetAllGroupCoverages();
                if (groupCoverage.Count != 0)
                {
                    return new UifSelectResult(groupCoverage.OrderBy(x => x.Description));
                }

                return new UifSelectResult("");
            }

            catch (Exception)
            {
                return new UifSelectResult("");
            }
        }
        /// <summary>
        /// Obtener grupo de coberturas por tipo de riesgo
        /// </summary>
        /// <param name="riskTypeId">Id Tipo de Riesgo</param>
        /// <returns>Coberturas</returns>
        public ActionResult GetAllGroupCoveragesByRiskTypeId(int riskTypeId)
        {

            try
            {
                List<GroupCoverage> groupCoverage = DelegateService.underwritingService.GetAllGroupCoverages();

                if (groupCoverage != null)
                {
                    switch ((crtEnum)riskTypeId)
                    {
                        case crtEnum.Aeronavigation:
                            return new UifSelectResult(groupCoverage.Where(x => x.CoveredRiskType == crtEnum.Aeronavigation).OrderBy(x => x.Description));
                        case crtEnum.Vehicle:
                            return new UifSelectResult(groupCoverage.Where(x => x.CoveredRiskType == crtEnum.Vehicle).OrderBy(x => x.Description));
                        case crtEnum.Surety:
                            return new UifSelectResult(groupCoverage.Where(x => x.CoveredRiskType == crtEnum.Surety).OrderBy(x => x.Description));
                        case crtEnum.Transport:
                            return new UifSelectResult(groupCoverage.Where(x => x.CoveredRiskType == crtEnum.Transport).OrderBy(x => x.Description));
                        case crtEnum.Location:
                            return new UifSelectResult(groupCoverage.Where(x => x.CoveredRiskType == crtEnum.Location).OrderBy(x => x.Description));
                        default:
                            break;
                    }
                }

                return new UifSelectResult("");
            }

            catch (Exception)
            {
                return new UifSelectResult("");
            }
        }

        /// </summary>
        /// Obtener los Objetos de Seguro.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public ActionResult GetInsuredObjectByPrefixIdList(int prefixId)
        {
            try
            {
                List<CompanyInsuredObject> companyInsuredObjects = DelegateService.underwritingService.GetCompanyInsuredObjectByPrefixIdList(prefixId);

                if (companyInsuredObjects.Count != 0)
                {
                    return new UifJsonResult(true, companyInsuredObjects.OrderBy(x => x.Description));
                }

                return new UifJsonResult(false, App_GlobalResources.Language.NoInsuranceObjects);
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        /// <summary>
        /// Obtener Coberturas por Objecto del Seguro
        /// </summary>
        /// <param name="insuredObjectId">Id Objecto del Seguro</param>
        /// <returns>Coberturas</returns>
        public ActionResult GetCoveragesByInsuredObjectId(int insuredObjectId)
        {
            try
            {
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectId(insuredObjectId);
                if (coverages.Count != 0)
                {
                    return new UifSelectResult(coverages.OrderBy(x => x.Description));
                }

                return new UifSelectResult("");
            }

            catch (Exception)
            {
                return new UifSelectResult("");
            }
        }
        /// <summary>
        /// Obtener coberturas por plan tecnico
        /// </summary>
        /// <param name="insuredObjectId">Id Plan Tecnico</param>
        /// <returns>Coberturas</returns>
        public UifJsonResult GetCoveragesByTechnicalPlanId(int technicalPlanId, int insuredObjectId)
        {
            try
            {
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByTechnicalPlanId(technicalPlanId);

                if (coverages.Count != 0)
                {
                    return new UifJsonResult(true, coverages.Where(x => x.InsuredObject.Id == insuredObjectId));
                }

                return new UifJsonResult(true, null);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }
        /// <summary>
        /// Obtener coberturas principales por objeto de seguro
        /// </summary>
        /// <param name="insuredObjectId">Id Objecto del Seguro</param>
        /// <returns>Coberturas Principales</returns>
        public ActionResult GetCoveragesPrincipalByInsuredObjectId(int insuredObjectId)
        {
            try
            {
                List<CompanyCoverage> principalCoverages = DelegateService.underwritingService.GetCompanyCoveragesPrincipalByInsuredObjectId(insuredObjectId);

                if (principalCoverages.Count != 0)
                {
                    return new UifJsonResult(true, principalCoverages.OrderBy(x => x.Description));
                }

                return new UifJsonResult(true, "");
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }

        /// </summary>
        /// Obtiene toda la informacion de módulo habilitados.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public ActionResult GetPackageEnabled()
        {
            try
            {
                List<Package> packages = DelegateService.rulesEditorServices.GetPackages();
                if (packages.Count != 0)
                {
                    return new UifJsonResult(true, packages.Where(x => x.Disabled == false).OrderBy(x => x.Description));
                }

                return new UifJsonResult(false, App_GlobalResources.Language.NoModuleData);
            }

            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }
        /// </summary>
        /// Obtiene toda los reglas por nivel.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// Test CFB
        public ActionResult GetAllRuleSetsByLevelId()
        {

            try
            {

                List<Level> levels = new List<Level>();
                levels.Add(new Level { LevelId = (int)Sistran.Core.Application.RulesScriptsServices.Enums.Level.General });
                levels.Add(new Level { LevelId = (int)Sistran.Core.Application.RulesScriptsServices.Enums.Level.Risk });
                levels.Add(new Level { LevelId = (int)Sistran.Core.Application.RulesScriptsServices.Enums.Level.Coverage });


                List<RuleSet> ruleSetDTOs = DelegateService.rulesEditorServices.GetRuleSetByLevels(levels);

                if (ruleSetDTOs != null && ruleSetDTOs.Count != 0)
                {
                    List<RuleSet> level = new List<RuleSet>();
                    level = ruleSetDTOs.ToList();
                    //return new UifSelectResult(level.OrderBy(x => x.Description));
                    return new UifJsonResult(true, level.OrderBy(x => x.Description).ToList());
                }

                //return new UifSelectResult("");
                return new UifJsonResult(false, "");
            }

            catch (Exception)
            {
                //return new UifSelectResult("");
                return new UifJsonResult(false, "");
            }
        }

        /// </summary>
        /// Obtiene toda las guiones por nivel.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// Test CFB
        public ActionResult GetAllScriptByLevelId()
        {
            try
            {

                List<Level> levels = new List<Level>();
                levels.Add(new Level { LevelId = (int)Sistran.Core.Application.RulesScriptsServices.Enums.Level.General });
                levels.Add(new Level { LevelId = (int)Sistran.Core.Application.RulesScriptsServices.Enums.Level.Risk });
                levels.Add(new Level { LevelId = (int)Sistran.Core.Application.RulesScriptsServices.Enums.Level.Coverage });
                List<Script> ScriptsDTOs = DelegateService.scriptsService.GetScriptsByLevels(levels);

                if (ScriptsDTOs != null && ScriptsDTOs.Count != 0)
                {
                    List<Script> scripts = new List<Script>();
                    scripts = ScriptsDTOs.ToList();
                    return new UifJsonResult(true, scripts.OrderBy(x => x.Description).ToList());
                }

                return new UifJsonResult(false, "");
            }

            catch (Exception)
            {
                return new UifJsonResult(false, "");
            }
        }
        /// <summary>
        /// Obtener Planes Tecnicos por Tipo de Riesgo Cubierto
        /// </summary>
        /// <param name="coveredRiskTypeId">Tipo de Riesgo Cubierto</param>
        /// <returns>Planes Tecnicos</returns>
        public ActionResult GetTechnicalPlanByCoveredRiskTypeId(int coveredRiskTypeId, int insuredObjectId)
        {
            try
            {
                List<TechnicalPlan> technicalPlans = DelegateService.underwritingService.GetTechnicalPlanByCoveredRiskTypeIdInsuredObjectId(coveredRiskTypeId, insuredObjectId);
                return Json(technicalPlans.OrderBy(x => x.Description), JsonRequestBehavior.AllowGet);
            }

            catch (Exception)
            {
                return new UifJsonResult(false, "");
            }
        }
        //// Modulos Datos Adicionales
        public UifJsonResult GetAgenciesByAgentIdDescription(int agentId, string description, int prefixId)
        {
            try
            {
                List<CiaParamAgentServiceModel> productAgents = new List<CiaParamAgentServiceModel>();
                List<UPV1.Agency> agencies = DelegateService.uniquePersonServiceV1.GetAgenciesByAgentIdDescriptionIdPrefix(agentId, description, prefixId);


                if (agencies.Count == 1)
                {
                    if (agencies[0].Agent.DateDeclined > DateTime.MinValue)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryDischarged);
                    }
                    else if (agencies[0].DateDeclined > DateTime.MinValue)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorAgencyDischarged);
                    }
                }
                else
                {
                    for (int i = 0; i < agencies.Count(); i++)
                    {
                        if (agencies[i].DateDeclined != null)
                        {
                            agencies.RemoveAt(i);
                            i--;
                        }
                    }
                }
                MappingProductAgents(productAgents, agencies);
                return new UifJsonResult(true, productAgents);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        private static void MappingProductAgents(List<CiaParamAgentServiceModel> productAgents, List<UPV1.Agency> agencies)
        {

            foreach (var item in agencies)
            {
                CiaParamAgentServiceModel productAgentAdd = new CiaParamAgentServiceModel();
                productAgentAdd.FullName = item.Agent.FullName;
                productAgentAdd.StatusTypeService = StatusTypeService.Create;
                productAgentAdd.ProductId = 0;
                productAgentAdd.IndividualId = item.Agent.IndividualId;
                productAgentAdd.LockerId = item.Code;
                productAgents.Add(productAgentAdd);
            }
        }

        public ActionResult GetAgentByIndividualIdFullName(string description)
        {
            int codeAgent = 0;
            Int32.TryParse(description, out codeAgent);
            List<UPV1.Agent> agent = new List<UPV1.Agent>();
            try
            {
                if (codeAgent == 0)
                {
                    agent = DelegateService.uniquePersonServiceV1.GetAgentByIndividualIdFullName(0, description);
                }
                else
                {
                    agent = DelegateService.uniquePersonServiceV1.GetAgentByIndividualIdFullName(codeAgent, "");
                }

                if (agent.Count == 1)
                {
                    if (agent[0].DateDeclined > DateTime.MinValue)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryDischarged);
                    }
                    else
                    {
                        return new UifJsonResult(true, agent);
                    }
                }
                else
                {
                    return new UifJsonResult(true, agent);
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAgent);
            }

        }
        public ActionResult GetPaymentSchudeleByCurrencies(List<int> currencies)
        {
            try
            {
                if (currencies != null && currencies.Count > 0)
                {

                    List<ProductFinancialPlanModelsView> FinancialPlan = ModelAssembler.CreateProductFinancialPlansModelsView(DelegateService.productParamService.GetPaymentScheduleByCurrencies(currencies));
                    FinancialPlan = FinancialPlan.OrderBy(x => x.PaymentMethod.Id).ToList();

                    var jsonResult = Json(FinancialPlan, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return null;
            }

        }
        public ActionResult GetAgenciesByAgentId(int agentId)
        {
            try
            {
                List<UPV1.Agency> agencies = DelegateService.uniquePersonServiceV1.GetAgenciesByAgentId(agentId);
                if (agencies.Count == 1)
                {
                    return new UifSelectResult(agencies.OrderBy(x => x.FullName), agencies[0].Id);
                }
                return new UifSelectResult(agencies.OrderBy(x => x.FullName));
            }
            catch (Exception)
            {

                return new UifSelectResult("");
            }

        }
        public ActionResult CreateCopyProduct(CiaParamCopyProductServiceModel copyProduct)
        {
            try
            {
                if (copyProduct != null && copyProduct.Id != 0)
                {
                    copyProduct.Description = copyProduct.Description.TrimEnd().ToUpper();
                    copyProduct.SmallDescription = copyProduct.SmallDescription.ToUpper();
                    int ProductId = DelegateService.productParamService.CreateCopyProduct(copyProduct);
                    if (ProductId == 0)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ProductNotCopied);
                    }
                    else
                    {
                        return new UifJsonResult(true, ProductId);
                    }


                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSelectedProduct);
                }
            }
            catch (Exception ex)
            {

                return new UifJsonResult(false, ex.Message);
            }

        }

        public ActionResult CopyProduct()
        {
            try
            {
                CopyProductModelsView copyProductModelsView = new CopyProductModelsView();
                return PartialView(copyProductModelsView);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        public UifJsonResult GetProductByPrefixIdProductId(int prefixId, int productId)
        {
            try
            {
                CiaParamProductServiceModel product = new CiaParamProductServiceModel();
                product.StatusTypeService = StatusTypeService.Original;
                product.Description = productId.ToString();
                product.Prefix = new CiaParamPrefixServiceModel { Id = prefixId, StatusTypeService = StatusTypeService.Original };
                List<ProductModelsView> productParametrization = new List<ProductModelsView>();
                productParametrization = ModelAssembler.CreateProductsViewModel(DelegateService.productParamService.GetCiaProductsByProduct(product).OrderBy(t => t.Description).ToList());
                return new UifJsonResult(true, productParametrization);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryProductMain);
            }

        }

        public ActionResult GetCoverageProductByCoverageId(int coverageId)
        {
            try
            {
                CompanyCoverage coverage = DelegateService.underwritingService.GetCompanyCoverageProductByCoverageId(coverageId);
                if (coverage != null)
                {
                    return new UifJsonResult(true, coverage);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageSearchCoverage);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCoverage);
            }
        }
        /*Datos Adicionales*/
        // Modulos Datos Adicionales
        public ActionResult GetProductModulesAdditionalDataSelect()
        {
            try
            {
                List<AdditionalData> modulesAdditionalData = new List<AdditionalData>();
                modulesAdditionalData.Add(new AdditionalData() { Id = 1, Description = "Actividades" });
                modulesAdditionalData.Add(new AdditionalData() { Id = 2, Description = "Formas de impresión" });
                modulesAdditionalData.Add(new AdditionalData() { Id = 3, Description = "Limites RC" });

                return new UifSelectResult(modulesAdditionalData.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetActivities);
            }
        }
        // Formas
        public ActionResult GetForms()
        {
            try
            {
                List<Deductible> deductibles = new List<Deductible>();
                return new UifTableResult(deductibles);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetForms);
            }
        }

        public ActionResult SaveAdditionalData(List<CiaParamCommercialClassServiceModel> listRiskCommercialClass, List<CiaParamLimitsRCServiceModel> listCiaParamLimitsRCServiceModel, List<CiaParamDeductibleProductServiceModel> listCiaParamDeductibleProductServiceModel, List<CiaParamFormServiceModel> listCiaParamFormServiceModel, int productId)
        {
            try
            {
                if (listRiskCommercialClass != null)
                {
                    listRiskCommercialClass = listRiskCommercialClass.Where(x => x.StatusTypeService != StatusTypeService.Original).ToList();
                }
                else
                {
                    listRiskCommercialClass = new List<CiaParamCommercialClassServiceModel>();
                }
                if (listCiaParamLimitsRCServiceModel != null)
                {
                    listCiaParamLimitsRCServiceModel = listCiaParamLimitsRCServiceModel.Where(x => x.StatusTypeService != StatusTypeService.Original).ToList();
                }
                else
                {
                    listCiaParamLimitsRCServiceModel = new List<CiaParamLimitsRCServiceModel>();
                }
                if (listCiaParamDeductibleProductServiceModel != null)
                {
                    listCiaParamDeductibleProductServiceModel = listCiaParamDeductibleProductServiceModel.Where(x => x.StatusTypeService != StatusTypeService.Original).ToList();
                }
                else
                {
                    listCiaParamDeductibleProductServiceModel = new List<CiaParamDeductibleProductServiceModel>();
                }
                if (listCiaParamFormServiceModel != null)
                {
                    listCiaParamFormServiceModel = listCiaParamFormServiceModel.Where(x => x.StatusTypeService != StatusTypeService.Original).ToList();
                }
                else
                {
                    listCiaParamFormServiceModel = new List<CiaParamFormServiceModel>();
                }
                DelegateService.productParamService.SaveAdditionalData(listRiskCommercialClass, listCiaParamLimitsRCServiceModel, listCiaParamDeductibleProductServiceModel, listCiaParamFormServiceModel, productId);

                return new UifJsonResult(true, true);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveAdditionalData);
            }
        }

        // Actividades
        public ActionResult GetActivities()
        {
            try
            {
                List<RiskCommercialClass> riskCommercialClass = new List<RiskCommercialClass>();
                riskCommercialClass = DelegateService.underwritingService.GetRiskCommercialClass();

                return new UifTableResult(riskCommercialClass);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetActivities);
            }
        }
        //AdditionalData Limit RC
        public ActionResult GetLimitRc(List<PolicyType> policyTypes, int productId, int prefixCode)
        {
            try
            {
                List<int> lstPolicyTypes = new List<int>();
                if(policyTypes!=null)
                {
                    foreach (PolicyType itemPolicyType in policyTypes)
                    {
                        lstPolicyTypes.Add(itemPolicyType.Id);
                    }
                }
              
                List<CiaParamLimitsRCServiceModel> result = new List<CiaParamLimitsRCServiceModel>();
                result = DelegateService.productParamService.GetLimitsRc(lstPolicyTypes, productId, prefixCode);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRcLimits);
            }
        }
        //AdditionalData Deducibles por producto
        public ActionResult GetProductDeductiblesByPrefix(int productId, int prefixCode)
        {
            try
            {
                List<CiaParamDeductibleProductServiceModel> result = new List<CiaParamDeductibleProductServiceModel>();
                result = DelegateService.productParamService.GetProductDeductiblesByPrefix(productId, prefixCode);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDeductibles);
            }
        }

        //AdditionalData Activities
        public ActionResult GetRiskCommercialClass(int productId)
        {
            try
            {
                List<CiaParamCommercialClassServiceModel> result = new List<CiaParamCommercialClassServiceModel>();
                result = DelegateService.productParamService.GetRiskCommercialClass(productId);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetActivities);
            }
        }
        //AdditionalData ProductForms
        public ActionResult GetProductForm(int productId)
        {
            try
            {
                List<CiaParamFormServiceModel> result = new List<CiaParamFormServiceModel>();
                result = DelegateService.productParamService.GetProductForm(productId);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetForms);
            }
        }

        /// <summary>
        /// Funcion que obtiene los deducibles por Ramo
        /// </summary>
        /// <param name="prefixId"></param>
        /// <param name="band"></param>
        /// <returns>UifTableResult o UifJsonResult segun corresponda </returns>
        public ActionResult GetDeductiblesByPrefixId(int prefixId, int? band)
        {
            try
            {
                List<Deductible> deductibles = new List<Deductible>();
                deductibles = DelegateService.underwritingService.GetDeductiblesByPrefixId(prefixId);

                if (band == 0)
                {
                    return new UifTableResult(deductibles);
                }
                else
                {
                    return new UifJsonResult(true, deductibles);
                }

            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDeductibles);
            }
        }

        ///// <summary>
        ///// Funcion que obtiene los deducibles y su estado con relación al producto
        ///// </summary>
        ///// <param name="prefixId">Identificador del ramo comercial</param>
        ///// <param name="idProduct">Identificador del producto</param>
        ///// <returns>UifJsonResult con la información obtenida</returns>
        //public ActionResult GetDeductiblesByPrefixIdProductId(int prefixId, int idProduct)
        //{
        //    try
        //    {
        //        List<DeductibleUnit> deductibles = new List<DeductibleUnit>();
        //        deductibles = DelegateService.underwritingService.GetDeductiblesByPrefixIdProductId(prefixId, idProduct);
        //        return new UifJsonResult(true, deductibles);

        //    }
        //    catch (Exception)
        //    {
        //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDeductibles);
        //    }
        //}

        /// <summary>
        /// Funcion que obtiene los deducibles por producto, grupo de covertura y covertura
        /// </summary>
        /// <param name="productId">codigo producto</param>
        /// <param name="coverGroupId">id grupo de cobertura</param>
        /// <param name="CoverageID">id cobertura</param>
        /// <returns></returns>
        public ActionResult GetDeductiblesByProductIdByGroupCoverageBycoverageId(int productId, int coverGroupId, int CoverageID, int beneficiaryTypeCd, int lineBusinnessCd)
        {
            try
            {

                List<DeductByCoverProductModelView> deductibles = new List<DeductByCoverProductModelView>();
                deductibles = ModelAssembler.CreateDeductiblesByCoverProductModelView(DelegateService.productParamService.GetDeductiblesByProductId(productId, coverGroupId, CoverageID, beneficiaryTypeCd, lineBusinnessCd));
                return new UifJsonResult(true, deductibles);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDeductibles);
            }
        }

        //public ActionResult GetDeductiblesByProductByGroupCoverageByCoverageByBeneficiaryType(int productId, int groupCoverageId, int coverageId, int beneficiaryTypeId)
        //{
        //    try
        //    {
        //        List<Deductible> deductibles = DelegateService.underwritingService.GetDeductiblesByProductByGroupCoverageByCoverageByBeneficiaryType(productId, groupCoverageId, coverageId, beneficiaryTypeId);
        //        return new UifJsonResult(true, deductibles);
        //    }
        //    catch (Exception)
        //    {

        //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDeductibles);
        //    }
        //}

        /// <summary>
        /// Funcion que obtiene los deducibles por cobertura
        /// </summary>
        /// <param name="coverage"></param>
        /// <returns>List<Deductible></returns>
        public ActionResult GetDeductiblesByCoverage(int coverage)
        {
            try
            {
                List<Deductible> deductibles = new List<Deductible>();
                deductibles = DelegateService.underwritingService.GetDeductiblesByCoverageId(coverage);

                return new UifTableResult(deductibles);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDeductibles);
            }
        }

        #endregion
        //Validar coberturas a eliminar       
        public UifJsonResult ExistCoverageProductByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId, int coverageId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.underwritingService.ExistCoverageProductByProductIdGroupCoverageIdInsuredObjectId(productId, groupCoverageId, insuredObjectId, coverageId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        //Validar Objetos de seguro a eliminar       
        public UifJsonResult ExistInsuredObjectProductByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.underwritingService.ExistInsuredObjectProductByProductIdGroupCoverageIdInsuredObjectId(productId, groupCoverageId, insuredObjectId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
        public ActionResult GetParameterByDescription(string description)
        {
            try
            {
                if (parameters.Count == 0)
                {
                    parameters = GetParameters();
                }

                return new UifJsonResult(true, parameters.First(x => x.Description == description).Value);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchParameter);
            }
        }

        private List<Sistran.Core.Application.CommonService.Models.Parameter> GetParameters()
        {
            parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "Currency", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.Currency).NumberParameter.Value });

            parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "Country", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.Country).NumberParameter.Value });
            return parameters;
        }

        //public ActionResult GetProductsByPrefixIdByDescription(int prefixId, string description)
        //{
        //    List<CompanyProduct> product = new List<CompanyProduct>();
        //    try
        //    {
        //        product = DelegateService.underwritingService.GetCompanyProductsByPrefixIdByDescription(prefixId, description);
        //        return new UifJsonResult(true, product);
        //    }
        //    catch (Exception)
        //    {

        //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingAgents);
        //    }

        //}
        public UifJsonResult DetectProductChanges(ProductModelsView productOld, ProductModelsView product)
        {
            try
            {
                int hashPolicy = JsonConvert.SerializeObject(product).GetHashCode();
                int hashPolicyOld = JsonConvert.SerializeObject(productOld).GetHashCode();
                if (hashPolicy != hashPolicyOld)
                {
                    return new UifJsonResult(true, true);
                }
                else
                {
                    return new UifJsonResult(true, false);
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(true, App_GlobalResources.Language.ErrorDetectingChanges);
            }


        }
        #region Search Advanced
        public PartialViewResult AdvancedSearch()
        {
            return PartialView();
        }

        //public ActionResult GetProducts()
        //{
        //    try
        //    {

        //        List<CompanyProduct> products = new List<CompanyProduct>();
        //        products = DelegateService.underwritingService.GetCompanyAllProducts();
        //        return new UifJsonResult(true, products);
        //    }
        //    catch (Exception)
        //    {
        //        return new UifJsonResult(true, null);
        //    }

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ActionResult GetProductAdvancedSearch(CiaParamProductServiceModel product)
        {
            try
            {
                product.StatusTypeService = StatusTypeService.Original;
                if (product.Prefix == null || product.Prefix.Id == 0)
                {
                    ModelState.AddModelError("product.PrefixId", App_GlobalResources.Language.Prefix);
                    return PartialView(product);

                }
                if (product.Prefix != null)
                {
                    product.Prefix.StatusTypeService = StatusTypeService.Original;
                }
                List<ProductModelsView> products = new List<ProductModelsView>();
                products = ModelAssembler.CreateProductsViewModel(DelegateService.productParamService.GetCiaProductsByProduct(product).OrderBy(t => t.Description).ToList());
                return Json(products, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        public UifJsonResult GetMainProductByPrefixIdDescriptionProductId(string description, int prefixId, int productId)
        {
            try
            {
                CiaParamProductServiceModel product = new CiaParamProductServiceModel();
                product.StatusTypeService = StatusTypeService.Original;
                product.Description = productId.ToString();
                product.Prefix = new CiaParamPrefixServiceModel { Id = prefixId, StatusTypeService = StatusTypeService.Original };
                List<ProductModelsView> productParametrization = new List<ProductModelsView>();
                productParametrization = ModelAssembler.CreateProductsViewModel(DelegateService.productParamService.GetCiaProductsByProduct(product).OrderBy(t => t.Description).ToList());
                return new UifJsonResult(true, productParametrization);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryProductMain);
            }

        }

        public UifJsonResult GetProductAgentByProductId(int productId, int prefixId)
        {
            CiaParamSummaryAgentServiceModel summaryAgent = new CiaParamSummaryAgentServiceModel();
            try
            {
                summaryAgent = DelegateService.productParamService.GetProductAgentByProductId(productId, prefixId);
                return new UifJsonResult(true, summaryAgent);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingAgents);
            }

        }

        public UifJsonResult GetProductAgentByProductIdByIndividualId(int productId, int individualId)
        {
            List<CiaParamAgentServiceModel> productAgents = new List<CiaParamAgentServiceModel>();
            try
            {
                productAgents = DelegateService.productParamService.GetProductAgentByProductIdByIndividualId(productId, individualId);
                return new UifJsonResult(true, productAgents.OrderBy(x => x.FullName));
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingAgents);
            }

        }

        public UifJsonResult GetFinancialPlanByProductId(int productId)
        {
            List<FinancialPlan> financialPlans = new List<FinancialPlan>();
            try
            {
                financialPlans = DelegateService.underwritingService.GetFinancialPlanByProductId(productId);
                return new UifJsonResult(true, financialPlans);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryFinancialPlanByProductId);
            }
        }

        //public UifJsonResult GetCoveredRiskByProductId(int productId)
        //{
        //    CompanyProduct productCoverage = new CompanyProduct();
        //    try
        //    {
        //        productCoverage = DelegateService.underwritingService.GetCompanyProductById(productId);
        //        return new UifJsonResult(true, productCoverage);
        //    }
        //    catch (Exception)
        //    {

        //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCoveredRiskByProductId);
        //    }
        //}

        //public UifJsonResult GetCoveredProductById(int productId)
        //{
        //    CompanyProduct productCoverage = new CompanyProduct();
        //    try
        //    {
        //        productCoverage = DelegateService.underwritingService.GetCompanyCoveredProductById(productId);
        //        return new UifJsonResult(true, productCoverage);
        //    }
        //    catch (Exception)
        //    {

        //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCoveredRiskByProductId);
        //    }
        //}
        //public UifJsonResult GetDataAditionalByProductId(int productId)
        //{
        //    CompanyProduct productCoverage = new CompanyProduct();
        //    try
        //    {
        //        productCoverage = DelegateService.underwritingService.GetCompanyDataAditionalByProductId(productId);
        //        return new UifJsonResult(true, productCoverage);
        //    }
        //    catch (Exception)
        //    {

        //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryDataAditionalByProductId);
        //    }

        //}

        //public ActionResult GetIncentivesByProductIdByIndividualIdByAgentAgencyID(int productId, int indivicualId, int AgentAgencyId)
        //{
        //    try
        //    {
        //        List<CptProductIncentiveAgent> listCptProductIncentiveAgent = DelegateService.underwritingService.GetIncentivesByProductIdByIndividualIdByAgentAgencyID(productId, indivicualId, AgentAgencyId);
        //        return new UifJsonResult(true, listCptProductIncentiveAgent);
        //    }
        //    catch (Exception)
        //    {
        //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryIncentives);
        //    }

        //}

        #endregion
        #region exportar excel
        public ActionResult GenerateFileToProduct(int productId, string productDescription)
        {
            try
            {

                string urlFile = DelegateService.productParamService.GenerateFileToProduct(productId, string.Format("{0}_{1}_{2}", App_GlobalResources.Language.ButtonReport, productDescription, DateTime.Now.ToString("dd-MM-yyyy-ss-fffffff")));
                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorThereIsNoDataToExport);
                }
                else
                {
                    var filenamefromPath = urlFile.Split(new char[] { '\\' }).Last();
                    return new UifJsonResult(true, new { Url = this.Url.Action("ShowExcelFile", "Product") + "?url=" + urlFile, FileName = filenamefromPath });
                        //DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }

        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public FileResult ShowExcelFile(string url)
        {
            var pathToTheFile = url;
            var fileStream = new FileStream(pathToTheFile, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }


        #endregion

        #region BeneficiaryType

        public ActionResult GetBeneficiaryTypes()
        {
            try
            {
                List<CiaParamBeneficiaryTypeServiceModel> listBeneficiaryTypes = DelegateService.productParamService.GetBeneficiaryType();
                return new UifJsonResult(true, listBeneficiaryTypes.OrderBy(x => x.TinyDescription));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBeneficiaryTypes);
            }
        }

        #endregion
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="prefixId"></param>
        ///// <returns></returns>
        public ActionResult GetProducts2G(int prefixId)
        {
            try
            {
                List<CiaParamProduct2GServiceModel> products2G = new List<CiaParamProduct2GServiceModel>();
                products2G = DelegateService.productParamService.GetProduct2gByPrefix(prefixId);
                return new UifSelectResult(products2G);
            }
            catch (Exception)
            {
                return new UifSelectResult(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public ActionResult GetTypeOfAssistance(int prefixId)
        //{
        //    try
        //    {
        //        List<ProductAssistanceTypeModelsView> cptAssistanceType = ModelAssembler.CreateAssistanceTypesViewModel(DelegateService.productParamService.GetCiaAssistanceTypeByPrefix(prefixId));

        //        return new UifSelectResult(cptAssistanceType.OrderBy(y => y.Description));
        //    }
        //    catch (System.Exception)
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="prefixId"></param>
        ///// <returns></returns>
        //public ActionResult GetTypeOfAssistanceSelect(int prefixId)
        //{
        //    try
        //    {
        //        if (cptAssistanceType != null)
        //        {
        //            cptAssistanceType = DelegateService.underwritingService.GetAllcptAssistanceType();
        //        }
        //        return new UifSelectResult(cptAssistanceType.Where(x => x.PrefixCode == prefixId && x.Enabled == true).OrderBy(y => y.Description));
        //    }
        //    catch (Exception)
        //    {
        //        return new UifSelectResult("");
        //    }
        //}

        public ActionResult GenerateFileToProducts()
        {
            try
            {
                string urlFile = DelegateService.productParamService.GenerateFileToProducts(App_GlobalResources.Language.ListProducts);
                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }

        public ActionResult SaveAgents(List<CiaParamAgentServiceModel> listCiaParamAgentServiceModel, int productId)
        {
            try
            {
                productModelsView = ModelAssembler.CreateProductViewModel(DelegateService.productParamService.SaveAgents(listCiaParamAgentServiceModel, productId));



                return new UifJsonResult(true, productModelsView);
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.GetBaseException().Message);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveProduct);
                }
            }
        }

        public ActionResult SaveAllAgents(int prefixId, int productId, bool assigned)
        {
            try
            {
                string respuesta = DelegateService.productParamService.SaveAllAgents(prefixId, productId, assigned);
                return new UifJsonResult(true, respuesta);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveProduct);
            }
        }

        public ActionResult ValidatePolicyByProductId(int productId, int riskId, int coverId)
        {
            try
            {
                bool respuesta = DelegateService.productParamService.ValidatePolicyByProductId(productId, riskId, coverId);
                return new UifJsonResult(true, respuesta);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveProduct);
            }
        }

        public ActionResult GetParameterDecimalQty()
        {
            int DecimalQty = 0;
            try
            {
                
                DecimalQty = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DecimalQty"]);
                return new UifJsonResult(true, DecimalQty);
                
            }
            catch (Exception)
            {
                return new UifJsonResult(true, DecimalQty);
            }
        }

    }
}
