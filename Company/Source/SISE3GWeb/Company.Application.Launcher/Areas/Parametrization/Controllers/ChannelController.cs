// -----------------------------------------------------------------------
// <copyright file="ChannelController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Sistran.Company.Application.ProductServices.Models;
    using Sistran.Company.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.CommonService.Enums;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.CommonService;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using Sistran.Core.Application.UniquePersonService.Models;
    using Sistran.Core.Application.UniqueUserServices.Models;
    using Sistran.Core.Application.Vehicles.Models;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Helpers;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using CPEM = Sistran.Core.Application.UniquePersonService.Models;
    using UNMO = Sistran.Core.Application.UnderwritingServices.Models;
    using VECO = Sistran.Core.Application.Vehicles.VehicleServices.Models;
    using modelsProductServicesCompany = Sistran.Company.Application.ProductServices.Models;

    /// <summary>
    /// Contiene los procedimientos del controlador Channel
    /// </summary>
    public class ChannelController : Controller
    {
        /// <summary>
        /// Lista de países
        /// </summary>
        private static List<Country> countries = new List<Country>();

        /// <summary>
        /// Lista de productos
        /// </summary>
        private static List<modelsProductServicesCompany.CompanyProduct> products = new List<modelsProductServicesCompany.CompanyProduct>();
         
        /// <summary>
        /// Lista de parametros
        /// </summary>
        private static List<Sistran.Core.Application.CommonService.Models.Parameter> parameters = new List<Sistran.Core.Application.CommonService.Models.Parameter>();

        /// <summary>
        /// Una lista de canales
        /// </summary>
        private List<ServiceQuotationSource>  serviceQuotationSource = new List<ServiceQuotationSource>();
        
        /// <summary>
        /// Metodo Channel que retorna una vista
        /// </summary>
        /// <returns>Una vista</returns>
        public ActionResult Channel()
        {
            return this.View();
        }

        /// <summary>
        /// Método para retornar una vista parcial
        /// </summary>
        /// <returns>Una vista parcial</returns>
        [HttpGet]
        public ActionResult ChannelAdvancedSearch()
        {
            return View();
        }

        /// <summary>
        /// Metodo ValuesDefault que retorna una vista
        /// </summary>
        /// <returns>Una vista</returns>
        public ActionResult ValuesDefault()
        {
            return this.View("ValuesDefault");
        }

        /// <summary>
        /// Obtiene la lista de sucursales
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public ActionResult GetChannels()
        {
            try
            {
                List<ChannelViewModel> serviceQuotationSourceList = ModelAssembler.GetServiceQuotationSource(DelegateService.commonService.GetServiceQuotationSources());
                return new UifJsonResult(true, serviceQuotationSourceList.ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetChannels);
            }
        }

        /// <summary>
        /// Realiza los procesos del CRUD para los canales
        /// </summary>
        /// <param name="listAdded">Lista de serviceQuotationSources(canales) para ser agregados</param>
        /// <param name="listModified">Lista de serviceQuotationSources(canales) para ser modificados</param>
        /// <param name="listDeleted">Lista de serviceQuotationSources(canales) para ser eliminados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados</returns>
        public ActionResult SaveServiceQuotationSources(List<ChannelViewModel> listAdded, List<ChannelViewModel> listModified, List<ChannelViewModel> listDeleted)
        {
            try
            {
                ParametrizationResponse<ServiceQuotationSource> channelResponse = Services.DelegateService.commonService.SaveServiceQuotationSources(
                    ModelAssembler.CreateServiceQuotationSource(listAdded), 
                    ModelAssembler.CreateServiceQuotationSource(listModified), 
                    ModelAssembler.CreateServiceQuotationSource(listDeleted));
                
                string added = string.Empty;
                string edited = string.Empty;
                string deleted = string.Empty;
                string message;

                if (!string.IsNullOrEmpty(channelResponse.ErrorAdded))
                {
                    channelResponse.ErrorAdded = App_GlobalResources.Language.ResourceManager.GetString(channelResponse.ErrorAdded);
                }

                if (!string.IsNullOrEmpty(channelResponse.ErrorModify))
                {
                    channelResponse.ErrorModify = App_GlobalResources.Language.ResourceManager.GetString(channelResponse.ErrorModify);
                }

                if (!string.IsNullOrEmpty(channelResponse.ErrorDeleted))
                {
                    channelResponse.ErrorDeleted = App_GlobalResources.Language.ResourceManager.GetString(channelResponse.ErrorDeleted);
                }

                if (channelResponse.TotalAdded > 0)
                {
                    added = App_GlobalResources.Language.ReturnSaveAddedChannels;
                }
                else
                {
                    channelResponse.TotalAdded = null;
                }

                if (channelResponse.TotalModify > 0)
                {
                    edited = App_GlobalResources.Language.ReturnSaveEditedChannels;
                }
                else
                {
                    channelResponse.TotalModify = null;
                }

                if (channelResponse.TotalDeleted > 0)
                {
                    deleted = App_GlobalResources.Language.ReturnSaveDeletedChannels;
                }
                else
                {
                    channelResponse.TotalDeleted = null;
                }

                message = string.Format(
                    added + edited + deleted + "{3}{4}{5}",
                    channelResponse.TotalAdded.ToString() ?? string.Empty,
                    channelResponse.TotalModify.ToString() ?? string.Empty,
                    channelResponse.TotalDeleted.ToString() ?? string.Empty,
                    channelResponse.ErrorAdded ?? string.Empty,
                    channelResponse.ErrorModify ?? string.Empty,
                    channelResponse.ErrorDeleted ?? string.Empty);
                var result = ModelAssembler.GetServiceQuotationSource(channelResponse.ReturnedList.OrderBy(x => x.Description).ToList());
                
                return new UifJsonResult(true, new { message = message, data = result });
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveChannel);
            }
        }

        /// <summary>
        /// Obtiene sucursales
        /// </summary>
        public void GetListChannels()
        {
            if (this.serviceQuotationSource.Count == 0)
            {
                this.serviceQuotationSource = DelegateService.commonService.GetServiceQuotationSources().ToList();
            }
        }

        /// <summary>
        /// Metodo GenerateFileToExport que genera archivo excel y lo retorna
        /// </summary>
        /// <returns>Un Excel de canales</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                this.GetListChannels();
                string urlFile = DelegateService.commonService.GenerateFileToServiceQuotationSource(this.serviceQuotationSource, App_GlobalResources.Language.FileNameChannel);
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
        
        /// <summary>
        /// Obtiene la lista de parametros de un canal
        /// </summary>
        /// <param name="sourceCode">Codigo de un canal</param>
        /// <returns>Lista de parametros por canal</returns>
        public ActionResult GetServiceQuotationParameterBySourceCode(int sourceCode)
        {
            ValuesDefault valuesDefault = DelegateService.commonService.GetValuesDefaultBySourceCode(sourceCode);
            if (valuesDefault.ProspectName != null)
            {
                return new UifJsonResult(true, valuesDefault);
            }
            else
            {
                return new UifJsonResult(false, valuesDefault);
            }
        }

        /// <summary>
        /// Método para obtener países
        /// </summary>
        /// <returns>Un lista de países</returns>
        public ActionResult GetCountries()
        {
            try
            {
                if (countries.Count == 0)
                {
                    countries = DelegateService.commonService.GetCountries();
                }

                var list = countries.Select(item => new { item.Id, item.Description }).ToList();
                return new UifJsonResult(true, list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, new List<Country>());
            }
        }

        /// <summary>
        /// Funcion para obtener las ciudades por id de país
        /// </summary>
        /// <param name="countryId">Id del país</param>
        /// <returns>Lista de departamentos por país</returns>
        public ActionResult GetStatesByCountryId(int countryId = 0)
        {
            try
            {
                Country country = (from c in countries where c.Id == countryId select c).FirstOrDefault();

                if (country != null)
                {
                    var list = country.States.Select(item => new { item.Id, item.Description }).ToList();
                    return new UifJsonResult(true, list.OrderBy(x => x.Description));
                }
                else
                {
                    return new UifJsonResult(false, new List<State>());
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingDepartments);
            }
        }

        /// <summary>
        /// Funcion para obtener los grupos de coberturas
        /// </summary>
        /// <param name="productId">Id del producto</param>
        /// <returns>Lista de grupos de coberturas</returns>
        public ActionResult GetGroupCoverages(int productId)
        {
            try
            {
                List<UNMO.GroupCoverage> groupCoverages = DelegateService.underwritingService.GetProductCoverageGroupRiskByProductId(productId);
                return new UifJsonResult(true, groupCoverages.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, null);
            }
        }

        /// <summary>
        /// Método para obtener los Estado Civil
        /// </summary>
        /// <returns>Un lista los Estado Civil</returns>
        public ActionResult GetMaritalStatus()
        {
            try
            {
                List<CPEM.MaritalStatus> maritalStatus = new List<CPEM.MaritalStatus>();
                maritalStatus = DelegateService.uniquePersonService.GetMaritalStatus();

                return new UifJsonResult(true, maritalStatus.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetMaritalStatus);
            }
        }

        /// <summary>
        /// Método para obtener productos
        /// </summary>
        /// <param name="agentId">Id del agente</param>
        /// <param name="prefixId">Id del ramo</param>
        /// <returns>Lista de productos</returns>
        public ActionResult GetProductsByAgentIdPrefixId(int agentId, int prefixId)
        {
            List<modelsProductServicesCompany.CompanyProduct> products = DelegateService.underwritingService.GetCompanyProductsByAgentIdPrefixId(agentId, prefixId, false);
            if (products.Count > 0)
            {
                return new UifJsonResult(true, products.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, App_GlobalResources.Language.MessageAgentWithoutProduct);
            }
        }

        /// <summary>
        /// Método para obtener las agencias
        /// </summary>
        /// <param name="agentId">Id del agente</param>
        /// <param name="description">Descripción del agente</param>
        /// <returns>Lista de agencias</returns>
        public ActionResult GetAgenciesByAgentIdDescription(int agentId, string description)
        {
            List<Agency> agencies = DelegateService.uniquePersonService.GetAgenciesByAgentIdDescription(agentId, description);

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
                else
                {
                    return new UifJsonResult(true, agencies);
                }
            }
            else
            {
                return new UifJsonResult(true, agencies);
            }
        }

        /// <summary>
        /// Consulta las agencias dado el id de un intermediario
        /// </summary>
        /// <param name="agentId">Identificador del intermediario</param>
        /// <returns>Agencias del intermediario</returns>
        public ActionResult GetAgenciesByAgentId(int agentId)
        {
            List<Agency> agencies = DelegateService.uniquePersonService.GetAgenciesByAgentId(agentId);
            return new UifJsonResult(true, agencies.OrderBy(x => x.FullName).ToList());
        }

        /// <summary>
        /// Consulta los tipos de pólizas por producto
        /// </summary>
        /// <param name="productId"> Identificador del producto </param>
        /// <returns> Listado de pólizas dado el identificador de un producto </returns>
        public ActionResult GetPolicyTypesByProductId(int productId)
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
                    return new UifJsonResult(false, App_GlobalResources.Language.NoPolicyTypesFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyType);
            }
        }

        /// <summary>
        /// Método para obtener los limites RC
        /// </summary>
        /// <param name="prefixId">Id del ramo</param>
        /// <param name="productId">Id del producto</param>
        /// <param name="policyTypeId">Id de tipo de pólizas</param>
        /// <returns>lista de limites RC</returns>
        public ActionResult GetLimitsRcByPrefixIdProductIdPolicyTypeId(int prefixId, int productId, int policyTypeId)
        {
            try
            {
                List<UNMO.LimitRc> limitsRc = DelegateService.underwritingService.GetLimitsRcByPrefixIdProductIdPolicyTypeId(prefixId, productId, policyTypeId);
                return new UifJsonResult(true, limitsRc.OrderBy(x => x.LimitSum).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchLimitRC);
            }
        }

        /// <summary>
        /// Método para obtener sucursales
        /// </summary>
        /// <returns>Un lista de sucursales</returns>
        public ActionResult GetBranches()
        {
            try
            {
                List<Branch> branches = DelegateService.uniqueUserService.GetBranchesByUserId(SessionHelper.GetUserId());
                return new UifJsonResult(true, branches.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingBranch);
            }
        }

        /// <summary>
        /// Método para obtener tipos de uso
        /// </summary>
        /// <returns>Un lista de tipos de uso</returns>
        public ActionResult GetUses()
        {   
            List<VECO.Use> uses = DelegateService.vehicleService.GetUses();
            return new UifJsonResult(true, uses.OrderBy(x => x.Description).ToList());
        }

        /// <summary>
        /// Método para obtener tipos de carrocería
        /// </summary>
        /// <returns>Un lista de tipos de carrocería</returns>
        public UifJsonResult GetColors()
        {
            List<Color> colors = DelegateService.vehicleService.GetColors();
            return new UifJsonResult(true, colors.OrderBy(x => x.Description).ToList());
        }

        /// <summary>
        /// Método para obtener países
        /// </summary>
        /// <returns>Un lista de países</returns>
        public ActionResult GetBodies()
        {
            List<Body> bodies = DelegateService.vehicleService.GetBodies();
            return new UifJsonResult(true, bodies.OrderBy(x => x.Description));
        }

        /// <summary>
        /// Método para obtener tipos de trabajadore
        /// </summary>
        /// <returns>Un lista de tipos de trabajadore</returns>
        public ActionResult GetWorkerTypes()
        {
            List<WorkerType> workerTypes = DelegateService.commonService.GetWorkerTypes();
            return new UifJsonResult(true, workerTypes.OrderBy(x => x.Description).ToList());
        }

        /// <summary>
        /// Método para obtener un usuario
        /// </summary>
        /// <param name="userId">Id del usuario</param>
        /// <returns>Un usuario</returns>
        public ActionResult GetUserById(int userId)
        {
            User user = DelegateService.uniqueUserService.GetUserById(userId);
            return new UifJsonResult(true, user);
        }

        /// <summary>
        /// Método para obtener las monedas
        /// </summary>
        /// <param name="productId">Id del producto</param>
        /// <returns>Lista de monedas</returns>
        public ActionResult GetCurrenciesByProductId(int productId)
        {
            try
            {
                if (parameters.Count == 0)
                {
                    parameters = this.GetParameters();
                }

                List<Currency> currencies = DelegateService.underwritingService.GetCurrenciesByProductId(productId);
                return new UifJsonResult(true, currencies.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCurrency);
            }
        }

        /// <summary>
        /// Método para obtener los parametros
        /// </summary>
        /// <returns>Lista de parametros</returns>
        private List<Sistran.Core.Application.CommonService.Models.Parameter> GetParameters()
        {
            parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "Currency", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.Currency).NumberParameter.Value });

            parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "BusinessType", Value = BusinessType.CompanyPercentage });

            parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "Country", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.Country).NumberParameter.Value });

            return parameters;
        }
    }
}