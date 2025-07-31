using Newtonsoft.Json;
using Sistran.Company.Application.ExternalProxyServices.Models;
using Sistran.Company.Application.ExternalProxyServicesEEProvider.Assemblers;
using Sistran.Core.Framework.Queues;
using System;
using System.Diagnostics;

namespace Sistran.Company.Application.ExternalProxyServicesEEProvider.Business
{
    public class BusinessLog
    {
        internal static void RegisterSuccessfulRequest(Stopwatch timer, string serviceName, string correlationId, params object[] parameters)
        {
            ExternalInformationLogDTO thirdPartiesLog = ModelAssembler.CreateExternalInformationLogDTO(timer, serviceName, correlationId, parameters);
            string jsonLog = JsonConvert.SerializeObject(thirdPartiesLog);
            PutOnQueue(jsonLog);
        }
        internal static void RegisterFailRequest(Stopwatch timer, string serviceName, string correlationId, string exceptionMessage, params object[] parameters)
        {
            ExternalInformationLogDTO thirdPartiesLog = ModelAssembler.CreateExternalInformationLogDTO(timer, serviceName, correlationId, parameters);
            thirdPartiesLog.ErrorMessage = exceptionMessage;
            thirdPartiesLog.SuccessInvoke = false;
            string jsonLog = JsonConvert.SerializeObject(thirdPartiesLog);
            PutOnQueue(jsonLog);
        }

        private static void PutOnQueue(string jsonLog)
        {
            try
            {
                    IQueue queue = new BaseQueueFactory().CreateQueue("ThirdPartiesLogQuee");
                queue.PutOnQueue(jsonLog);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
            }
        }
    }
}
