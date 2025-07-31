using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.QuotationServices.EEProvider.DAOs;
using Sistran.Core.Application.QuotationServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.QuotationServices.EEProvider
{
    /// <summary>
    /// QuotationServiceEEProviderCore.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class QuotationServiceEEProviderCore : IQuotationServiceCore
    {

        /// <summary>
        /// Obtener Cotización
        /// </summary>
        /// <param name="quotationId">Id Cotización</param>
        /// <param name="version">Versión</param>
        /// <returns>Cotización</returns>
        public List<Policy> GetPoliciesByQuotationIdVersionPrefixId(int quotationId, int version, int prefixId, int branchId)
        {
            try
            {
                QuotationDAO quotationDAO = new QuotationDAO();
                return quotationDAO.GetPoliciesByQuotationIdVersionPrefixId(quotationId, version, prefixId, branchId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear Nueva Versión De Una Cotización
        /// </summary>
        /// <param name="quotationId">Id Cotización</param>
        /// <returns>Cotización</returns>
        public Policy CreateNewVersionQuotation(int operationId)
        {
            try
            {
                QuotationDAO quotationDAO = new QuotationDAO();
                return quotationDAO.CreateNewVersionQuotation(operationId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Convertir Cotización en Temporal
        /// </summary>
        /// <param name="quotationId">Id Cotización</param>
        /// <param name="version">Versión</param>
        /// <returns>Temporal</returns>
        public Policy CreateTemporalFromQuotation(int operationId)
        {
            try
            {
                QuotationDAO quotationDAO = new QuotationDAO();
                return quotationDAO.CreateTemporalFromQuotation(operationId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Cotización de busqueda avanzada
        /// </summary>
        /// <param name="policy"></param>
        /// <returns>Cotizaciones</returns>
        public List<Policy> GetPoliciesByPolicy(Policy policy)
        {
            try
            {
                QuotationDAO quotationDAO = new QuotationDAO();
                return quotationDAO.GetPoliciesByPolicy(policy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Cotizacion de la Poliza
        /// </summary>
        /// <param name="quotationId">Numero de Cotizacion</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">Error in GetQuotationByQuotationId</exception>
        public Policy GetPolicyByQuotationId(int quotationId, int prefixCd, int branchCd)
        {
            try
            {
                QuotationDAO policy = new QuotationDAO();
                return policy.GetPolicyByQuotationId(quotationId, prefixCd, branchCd);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        #region ConditionTextPrecatalogued
        
        public List<ConditionTextModel> GetconditionTextPrecatalogued()
        {
            try
            {
                QuotationDAO textPrecatalogued = new QuotationDAO();
                return textPrecatalogued.GetConditionText();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public ExcelFileServiceModel GenerateFileToText(string fileName)
        {
            try
            {
                QuotationDAO textPrecatalogued = new QuotationDAO();
                List<ConditionTextModel> list = GetconditionTextPrecatalogued();
                return new ExcelFileServiceModel { FileData = textPrecatalogued.GenerateFile(list, fileName) };
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<ConditionTextModel> GetConditionTexts(string nameConditionText)
        {
            try
            {
                QuotationDAO textPrecatalogued = new QuotationDAO();
                return textPrecatalogued.GetConditionTexts(nameConditionText);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

    }
}
