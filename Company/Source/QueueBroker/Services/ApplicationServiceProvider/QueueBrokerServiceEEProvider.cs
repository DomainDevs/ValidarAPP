using Sistran.Company.Application.QueueBrokerService;
using Sistran.Company.Application.QueueBrokerServiceEEProvider.Business;
using Sistran.Company.Application.QueueBrokerServiceEEProvider.Resources;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;

namespace Sistran.Company.Application.QueueBrokerServiceEEProvider
{
    public class QueueBrokerServiceEEProvider : IQueueBrokerService
    {
        public void CreateFasecoldaValue(string businessCollection)
        {
            try
            {
                QueueBrokerFasecoldaBusiness queueBrokerBusiness = new QueueBrokerFasecoldaBusiness();
                queueBrokerBusiness.CreateFasecoldaValue(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSavePricesFasecolda), ex);
            }
        }

        public void CreateFasecoldaCode(string businessCollection)
        {
            try
            {
                QueueBrokerFasecoldaBusiness queueBrokerBusiness = new QueueBrokerFasecoldaBusiness();
                queueBrokerBusiness.CreateFasecoldaCode(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSaveCodesFasecolda), ex);
            }
        }

        public void CreateListRisk(string businessCollection)
        {
            try
            {
                QueueBrokerFasecoldaBusiness queueBrokerBusiness = new QueueBrokerFasecoldaBusiness();
                queueBrokerBusiness.CreateListRisk(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSaveCodesFasecolda), ex);
            }
        }

        public void CreateListRiskOfac(string businessCollection)
        {
            try
            {
                QueueBrokerFasecoldaBusiness queueBrokerBusiness = new QueueBrokerFasecoldaBusiness();
                queueBrokerBusiness.CreateListRiskOfac(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSaveCodesFasecolda), ex);
            }
        }
        public void RecordListRisk(string businessCollection)
        {
            try
            {
                QueueBrokerFasecoldaBusiness queueBrokerBusiness = new QueueBrokerFasecoldaBusiness();
                queueBrokerBusiness.RecordListRisk(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSaveCodesFasecolda), ex);
            }
        }

        public void RecordListRiskOfac(string businessCollection)
        {
            try
            {
                QueueBrokerFasecoldaBusiness queueBrokerBusiness = new QueueBrokerFasecoldaBusiness();
                queueBrokerBusiness.RecordListRiskOfac(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSaveCodesFasecolda), ex);
            }
        }
    }
}