using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.QuotationServices.EEProvider.Assemblers;
using Sistran.Company.Application.QuotationServices.EEProvider.Business;
using Sistran.Company.Application.QuotationServices.EEProvider.DAOs;
using Sistran.Company.Application.QuotationServices.EEProvider.Resources;
using Sistran.Company.Application.QuotationServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.QuotationServices;
using Sistran.Core.Application.QuotationServices.EEProvider;
using Sistran.Core.Application.QuotationServices.EEProvider.DAOs;
using Sistran.Core.Application.QuotationServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Services.UtilitiesServices.Models;
using QuotationEntitiesCore = Sistran.Core.Application.Quotation.Entities;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using ErrorQuotion = Sistran.Core.Application.ModelServices.Enums;
using System.Linq;
using Sistran.Core.Application.UniqueUserServices.Models;

namespace Sistran.Company.Application.QuotationServices.EEProvider
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class QuotationServiceEEProvider : QuotationServiceEEProviderCore, IQuotationService
    {
        /// <summary>
        /// Cotizar póliza autos
        /// </summary>
        /// <param name="Vehicle">Datos vehiculo</param>
        /// <returns>Cotización</returns>
        public CompanyVehicle QuoteVehicle(CompanyVehicle companyVehicle)
        {
            try
            {
                QuoteVehicleDAO quoteDAO = new QuoteVehicleDAO();
                return quoteDAO.QuoteVehicle(companyVehicle);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetType().ToString() + "|" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener cotización por identificador
        /// </summary>
        /// <param name="quotationId">Identificador</param>
        /// <returns>Cotización</returns>
        public CompanyVehicle GetCompanyVehicleByQuotationId(int quotationId)
        {
            QuoteVehicleDAO quoteDAO = new QuoteVehicleDAO();
            return quoteDAO.GetCompanyVehicleByQuotationId(quotationId);
        }

        public CompanyPropertyRisk GetCompanyPropertyByQuotationId(int quotationId)
        {
            QuotePropertyDAO quoteDAO = new QuotePropertyDAO();
            return quoteDAO.GetCompanyPropertyByQuotationId(quotationId);
        }

        public List<CompanyPropertyRisk> GetCompanyPropertyByCompanyPolicy(CompanyPropertyRisk temporal)
        {
            QuotePropertyDAO quoteDAO = new QuotePropertyDAO();
            return quoteDAO.GetCompanyPropertyByCompanyPolicy(temporal);
        }
        /// <summary>
        /// Obtiene todos los amparos
        /// </summary>
        /// <returns></returns>
        public List<Peril> GetPerils()
        {
            var perilDAO = new PerilDAO(DataFacadeManager.Instance.GetDataFacade());
            return perilDAO.GetPerils();
        }

        /// <summary>
        /// Obtiene los amparos por descripcion
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public List<Peril> GetPerilsByDescription(string description)
        {
            try
            {
                var perilDAO = new PerilDAO(DataFacadeManager.Instance.GetDataFacade());
                return perilDAO.GetPerilByDescription(description);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetType().ToString() + "|" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Crea una lista de amparos
        /// </summary>
        /// <param name="perils"></param>
        /// <returns></returns>
        public ParametrizationResponse<Peril> SavePerils(List<Peril> perilsAdded, List<Peril> perilsModify, List<Peril> perilsDeleted)
        {
            try
            {
                var perilDAO = new PerilDAO(DataFacadeManager.Instance.GetDataFacade());
                return perilDAO.SavePerils(perilsAdded, perilsModify, perilsDeleted);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetClausesByLevelsConditionLevelId", ex);
            }
        }

        public List<Peril> GetPerilsByLineBusinessId(int lineBusinessId)
        {
            var perilDAO = new PerilDAO(DataFacadeManager.Instance.GetDataFacade());
            return perilDAO.GetPerilsByLineBusinessId(lineBusinessId);
        }

        public PerilLineBusiness GetPerilsByLineBusinessAssigned(int lineBusinessId)
        {
            var perilDAO = new PerilDAO(DataFacadeManager.Instance.GetDataFacade());
            return perilDAO.GetPerilsByLineBusinessAssigned(lineBusinessId);
        }

        /// <summary>
        /// Genera archivo excel amparos
        /// </summary>
        /// <param name="peril"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToPeril(List<Peril> peril, string fileName)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.GenerateFileToPeril(peril, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyPolicy> GetCompanyPoliciesByQuotationIdVersionPrefixId(int quotationId, int version, int prefixId, int branchId)
        {
            QuoteBusiness bis = new QuoteBusiness();
            return bis.GetCompanyPoliciesByQuotationIdVersionPrefixId(quotationId, version, prefixId, branchId);
        }

        public CompanyPropertyRisk QuoteProperty(CompanyPropertyRisk companyProperty)
        {
            try
            {
                QuotePropertyDAO quotePropertyDAO = new QuotePropertyDAO();
                return quotePropertyDAO.QuoteProperty(companyProperty);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorQuotate), ex);
            }
        }

        public CompanyPropertyRisk GetCompanyPropertyRiskByTemporalId(int temporalId)
        {
            try
            {
                QuotePropertyDAO quotePropertyDAO = new QuotePropertyDAO();
                return quotePropertyDAO.GetCompanyPropertyRiskByTemporalId(temporalId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRisk), ex);
            }
        }
        public List<TextPretacalogued> GetTextPretacaloguedDto()
        {
            var text = DelegateService.quotationService.GetconditionTextPrecatalogued();
            var resul = ModelAssembler.CreateText(text);
            return resul;
        }
      
        public void ExecuteOpertaionTextPrecatalogued(List<TextPretacalogued> textPrecatalogueds)
        {
            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        QuotationDAO quotationDAO = new QuotationDAO(); 
                        foreach (var itemTextPretacalogued in textPrecatalogueds)
                        {
                            var conditionText = EntityAssembler.CreateConditionText(itemTextPretacalogued);
                            var textPrecataloguedAssembler = EntityAssembler.CreateConditionTextLevel(itemTextPretacalogued);

                            if (itemTextPretacalogued.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create)
                            {
                                quotationDAO.CreateConditionText(conditionText);
                                textPrecataloguedAssembler.ConditionTextId = conditionText.ConditionTextId;
                                quotationDAO.CreateConditionTextLevel(textPrecataloguedAssembler);
                            }
                            
                            if (itemTextPretacalogued.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update)
                            {
                                quotationDAO.UpdateConditionText(conditionText);
                                quotationDAO.UpdateConditionTextLevel(textPrecataloguedAssembler);                                
                            }
                            if (itemTextPretacalogued.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                            {
                                quotationDAO.DeleteConditionTextLevel(textPrecataloguedAssembler);
                                quotationDAO.DeleteConditionText(conditionText);                                
                            }
                        }
                        transaction.Complete();
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        throw new BusinessException(ex.Message);
                    }

                }
            }

        }

        public CompanyPolicy CreateCompanyTemporalFromQuotation(int operationId)
        {
            try
            {

                QuotePropertyDAO quotePropertyDAO = new QuotePropertyDAO();
                return quotePropertyDAO.CreateCompanyTemporalFromQuotation(operationId);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener Temporal de cotización", ex);
            }
        }
		/// <summary>
        /// Obtiene el agente asociado al usaurio
        /// </summary>
        /// <param name="TempId"></param>
        /// <returns></returns>
        public UserAgency GetDefaultAgentByUserId(int idUser)
        {
            UserAgency agencies = DelegateService.uniqueUserService.GetAgenciesByAgentIdDescriptionUserId(0, "", idUser).FirstOrDefault();
            return agencies;
        }

        public List<TextPretacalogued> GetTextPretacaloguedByDescription(string textPre)
        {
            var text = DelegateService.quotationService.GetConditionTexts(textPre);
            var resul = ModelAssembler.CreateText(text);
            return resul;
        }

        #region SearchQuotation
        /// <summary>
        /// Obtener busqueda de cotización por persona
        /// </summary>
        /// <param name="TempId"></param>
        /// <returns></returns>
        public List<CompanyQuotationVehicleSearch> CompanyQuotationVehicleSearches(int tempId)
        {
            try
            {
                QuoteVehicleDAO quoteDAO = new QuoteVehicleDAO();
                return quoteDAO.GetQuotationVehicleSearch(tempId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetType().ToString() + "|" + ex.Message, ex);
            }
        }
        #endregion

        #region EmailQuotation

        public void UpdateCompanyPrintedDateByTempId(int tempId)
        {
            try
            {
                QuoteVehicleDAO quoteDAO = new QuoteVehicleDAO();
                quoteDAO.UpdatePrintedDateByTempId(tempId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.GetType().ToString() + "|" + ex.Message, ex);
            }
        }
        #endregion
    }
}