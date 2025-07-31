// -----------------------------------------------------------------------
// <copyright file="QuotaDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using System.Collections.Generic;    
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;    
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.DataFacade;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using PRODEN = Sistran.Core.Application.Product.Entities;
    using Resources = UnderwritingParamService.EEProvider.Resources;
    using UTMO = Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Acceso a DB de Plan de Pago
    /// </summary>
    public class QuotaDAO
    {
        /// <summary>
        /// Acceso a DB para la creacion de la cuota
        /// </summary>
        /// <param name="quota">Cuota MOD-B</param>
        /// <returns>Modelo Result operacion creacion cuota</returns>
        public UTMO.Result<ParametrizationQuota, UTMO.ErrorModel> CreateQuota(ParametrizationQuota quota)
        {
            try
            {
                PRODEN.PaymentDistribution paymentDistributionEntity = EntityAssembler.CreatePaymentDistribution(quota);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(paymentDistributionEntity);
                ParametrizationQuota quotaResult = ModelAssembler.CreateParametrizationQuota(paymentDistributionEntity);
                return new UTMO.ResultValue<ParametrizationQuota, UTMO.ErrorModel>(quotaResult);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedCreatingQuotasErrorBD);
                return new UTMO.ResultError<ParametrizationQuota, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));               
            }
        }

        /// <summary>
        /// Acceso a DB para la creacion de los valores de las cuotas
        /// </summary>
        /// <param name="quota">Cuota MOD-B</param>
        /// <returns>Modelo Result operacion creacion cuota</returns>
        public UTMO.Result<ParametrizacionQuotaTypeComponent, UTMO.ErrorModel> CreateQuotaComponetType(ParametrizacionQuotaTypeComponent quota)
        {
            try
            {
                PRODEN.CoPaymentDistributionComponent paymentDistributionEntity = EntityAssembler.CreatePaymentDistributionComponent(quota);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(paymentDistributionEntity);
                ParametrizacionQuotaTypeComponent quotaResult = ModelAssembler.CreateParametrizationQuotaComponent(paymentDistributionEntity);
                return new UTMO.ResultValue<ParametrizacionQuotaTypeComponent, UTMO.ErrorModel>(quotaResult);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedCreatingQuotasErrorBD);
                return new UTMO.ResultError<ParametrizacionQuotaTypeComponent, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
