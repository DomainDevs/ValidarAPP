using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Company.Application.WrapperServices.EEProvider.Assemblers;
using Sistran.Company.Application.WrapperServices.EEProvider.DAOs;
using Sistran.Company.Application.WrapperServices.Models;
using Sistran.Core.Application.Cache.CacheBusinessService.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;

namespace Sistran.Company.Application.WrapperServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class WrapperServiceEEProvider : IWrapperService
    {


        /// <summary>
        /// Cotizar Póliza Autos
        /// </summary>
        /// <param name="requestQuoteVehicle">Datos Cotización</param>
        /// <returns>Cotización</returns>
        public QuoteVehicleResponse QuoteVehicle(QuoteVehicleRequest requestQuoteVehicle)
        {
            QuoteVehicleResponse quoteVehicleResponse = new QuoteVehicleResponse();

            try
            {
                QuoteVehicleDAO quoteVehicleDAO = new QuoteVehicleDAO();
                quoteVehicleResponse = quoteVehicleDAO.QuoteVehicle(requestQuoteVehicle);
            }
            catch (BusinessException)
            {
                quoteVehicleResponse.ErrorMessage = Sistran.Company.Application.WrapperServices.EEProvider.Properties.Resources.ErrorQuoteVehicle;
            }
            catch (ValidationException ex)
            {
                quoteVehicleResponse.ErrorMessage = Sistran.Company.Application.WrapperServices.EEProvider.Properties.Resources.ResourceManager.GetString(ex.Message);

                if (string.IsNullOrEmpty(quoteVehicleResponse.ErrorMessage))
                {
                    quoteVehicleResponse.ErrorMessage = ex.Message;
                }
            }

            return quoteVehicleResponse;
        }

        /// <summary>
        /// Tarifar Póliza Autos sin reglas
        /// </summary>
        /// <param name="quoteVehiclePolicyRequest">Datos temporal</param>
        /// <returns>Tarifación</returns>
        public QuoteVehiclePolicyResponse Quotate(QuoteVehiclePolicyRequest quoteVehiclePolicyRequest)
        {
            QuoteVehiclePolicyResponse response = new QuoteVehiclePolicyResponse();

            try
            {
                response.Message = "OK";
            }
            catch (Exception ex)
            {
                response.Message = ex.ToString();
                throw new BusinessException(ex.Message, ex);
            }

            return response;
        }

        /// <summary>
        /// Consulta la información de la Póliza en Temporales y la persiste
        /// </summary>
        /// <param name="quoteVehiclePolicyRequest">modelo de poliza de vehiculos</param>
        /// <returns>bool: estado de la transacción</returns>
        public bool CreateQuotation(QuoteVehiclePolicyRequest quoteVehiclePolicyRequest)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
               
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.WrapperServices.EEProvider.CreateQuotation");

            return true;
        }

        public string QuotateMassive(int massiveLoadId)
        {
            try
            {
                return "OK";
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Policy CreatePolicy(int temporalId)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

            switch (companyPolicy.Product.CoveredRisk.SubCoveredRiskType.Value)
            {
                case SubCoveredRiskType.Vehicle:
                    List<CompanyVehicle> vehicles = DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(companyPolicy.Id);
                    companyPolicy = DelegateService.vehicleService.CreateEndorsement(companyPolicy, vehicles);
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    List<CompanyTplRisk> thirdPartyLiabilities = DelegateService.thirdPartyLiabilityService.GetThirdPartyLiabilitiesByTemporalId(companyPolicy.Id);
                    companyPolicy = DelegateService.thirdPartyLiabilityService.CreateEndorsement(companyPolicy, thirdPartyLiabilities);
                    break;
                case SubCoveredRiskType.Property:
                    List<CompanyPropertyRisk> companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByTemporalId(companyPolicy.Id);
                    companyPolicy = DelegateService.propertyService.CreateEndorsement(companyPolicy, companyPropertyRisks);
                    break;
                case SubCoveredRiskType.Liability:
                    List<CompanyLiabilityRisk> companyLiabilityRisks = DelegateService.liabilityService.GetCompanyLiabilitiesByTemporalId(companyPolicy.Id);
                    companyPolicy = DelegateService.liabilityService.CreateEndorsement(companyPolicy, companyLiabilityRisks);
                    break;
                case SubCoveredRiskType.Surety:
                    List<CompanyContract> companySurety = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyPolicy.Id);
                    companyPolicy = DelegateService.suretyService.CreateEndorsement(companyPolicy, companySurety);
                    break;
                case SubCoveredRiskType.JudicialSurety:
                    List<CompanyJudgement> companyJudgements = DelegateService.judicialSuretyService.GetCompanyJudgementsByTemporalId(companyPolicy.Id);
                    companyPolicy = DelegateService.judicialSuretyService.CreateEndorsement(companyPolicy, companyJudgements);
                    break;
            }
            var imapper = ModelAssembler.CreateMapCompanyPolicy();

            Policy policy = new Policy
            {
                DocumentNumber = companyPolicy.DocumentNumber,

                Prefix = imapper.Map<CompanyPrefix, Prefix>(companyPolicy.Prefix),
                Branch = imapper.Map<CompanyBranch, Branch>(companyPolicy.Branch),
                Endorsement = imapper.Map<CompanyEndorsement, Endorsement>(companyPolicy.Endorsement)
            };

            return policy;
        }
        /// <summary>
        /// Publicar y notificar la verison 
        /// </summary>
        /// <param name="HistoryVersionJson">Datos Version publicada</param>
        /// <returns></returns>
        public void loadCacheByJson(string HistoryVersionJson)
        {
            try
            {
                DelegateService.cacheBusinessService.LoadCacheByJson(HistoryVersionJson);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }
        /// <summary>
        /// Guardar y notificar un cambio al servicio del cache 
        /// </summary>
        /// <returns>VersionHistory</returns>
        public VersionHistory CreateVersionHistoryExternal(int userId)
        {
            try
            {
                return DelegateService.cacheBusinessService.CreateVersionHistory(userId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            catch (Exception)
            {
                throw new BusinessException("Error");//Errors.ErrorWithoutAccessCacheServices);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        /// <summary>
		/// Listado de maquinas con la lista de reglas actuales
		/// </summary>
		/// <returns>NodeRulessetEsatus</returns>
		public List<NodeRulesetStatus> GetNodeRulSet()
        {
            try
            {
                return DelegateService.cacheBusinessService.GetNodeRulSet();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            catch (Exception)
            {
                throw new BusinessException("Error");//Errors.ErrorWithoutAccessCacheServices);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        /// <summary>
        /// Obtiene la lista las ultimas versiones Publicadas
        /// </summary>
        /// <returns>NodeRulessetEsatus</returns>
        public List<VersionHistory> GetVersionHistory(int count)
        {
            try
            {
                return DelegateService.cacheBusinessService.GetVersionHistory(count);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            catch (Exception)
            {
                throw new BusinessException("Error"); //Errors.ErrorWithoutAccessCacheServices
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        /// <summary>
        /// Validación Estado Caché
        /// </summary>
        /// <returns>CacheStatus</returns>
        public CacheStatus GetCacheStatus()
        {
            try
            {
                VersionHistory versionHistory = DelegateService.cacheBusinessService.GetVersionHistory(1).FirstOrDefault();
                List<NodeRulesetStatus> nodesRulesetStatus = DelegateService.cacheBusinessService.GetNodeRulSet();
                List<Node> nodes = new List<Node>();

                if (versionHistory == null)
                {
                    return new CacheStatus
                    {
                        Status = 1,
                        Description = "Error" // Errors.MSG_GetNodeStatusNotVersion
                    };
                }

                if (nodesRulesetStatus == null || nodesRulesetStatus.Count() == 0)
                {
                    return new CacheStatus
                    {
                        Status = 1,
                        Description = "Error" // Errors.MSG_GetNodeStatusNotNodes
                    };
                }

                foreach (var nodeRulesetStatus in nodesRulesetStatus)
                {
                    Node node = new Node();
                    node.Hostname = nodeRulesetStatus.Node;
                    node.Version = new Guid(nodeRulesetStatus.Guid);
                    node.StartDateLoad = nodeRulesetStatus.CreationDate;
                    node.EndDateLoad = nodeRulesetStatus.FinishDate;
                    nodes.Add(node);
                }


                if (nodesRulesetStatus.Where(x => x.Guid == versionHistory.Guid).Count() == nodesRulesetStatus.Count()
                && nodesRulesetStatus.Where(x => x.FinishDate != null).Count() == nodesRulesetStatus.Count())
                {
                    return new CacheStatus
                    {
                        Status = 1,
                        Description = "Error", // Errors.MSG_GetNodeStatus,
                        Version = new Guid(versionHistory.Guid),
                        VersionDatetime = DateTime.Now,
                        Nodes = nodes
                    };
                }
                else
                {
                    return new CacheStatus
                    {
                        Status = 0,
                        Description = "Error", //Errors.Fail_GetNodeStatus,
                        Version = new Guid(versionHistory.Guid),
                        VersionDatetime = DateTime.Now,
                        Nodes = nodes
                    };
                }
            }
            catch (Exception)
            {
                return new CacheStatus
                {
                    Status = 0,
                    Description = "Error" //Errors.Error_GetNodeStatus 
                };
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        public void AuditData(object body)
        {
            try
            {
                DelegateService.auditService.AuditData(body);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("AuditData", ex.StackTrace, EventLogEntryType.Error);
                throw;
            }
        }

        public void ReloadListRisksCache(string userName)
        {
            try
            {
                DelegateService.listRiskBusinessService.LoadOnMemoryListRisks(userName);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error en {nameof(ReloadListRisksCache)}", ex);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }
    }
}