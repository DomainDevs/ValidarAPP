using Newtonsoft.Json;
using Sistran.Company.Application.OperationQuotaServices.DTOs;
using Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.UniqueUserServices.Enums;
using Sistran.Core.Application.UniqueUserServices.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.WrapperAuthorizationPoliciesServiceEEProvider.Business
{
    public class AutomaticQuotaBusiness
    {
        public void CreateAutomaticQuotaAuthorization(int id)
        {
            List<AutomaticQuotaOperationDTO> operation = DelegateService.operationQuotaCompanyService.GetAutomaticQuotaOperation(id);
            
            try
            {
                if (operation != null)
                {
                    AutomaticQuota automaticQuota = JsonConvert.DeserializeObject<AutomaticQuota>(operation.FirstOrDefault().Operation);
                    AutomaticQuotaDTO automaticDto = new AutomaticQuotaDTO();
                    DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.AutomaticQuota, id.ToString(), null, "Procesando");
                    automaticDto.AutomaticQuotaId = id;
                    automaticDto = DelegateService.operationQuotaCompanyService.SaveAutomaticQuotaGeneral(automaticDto, false);

                    string notification;
                    if (automaticDto != null)
                    {
                        notification = "Se creó la cuota automática para el individual id" + automaticQuota.IndividualId;

                        DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.AutomaticQuota, id.ToString(), null, null);

                        NotificationUser notificationUser = new NotificationUser
                        {
                            UserId = automaticQuota.ElaboratedId,
                            CreateDate = DateTime.Now,
                            NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                            Message = notification,
                            Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", automaticQuota.IndividualId},
                            }
                        };

                        DelegateService.UniqueUserService.CreateNotification(notificationUser);
                    }
                    
                }

                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(
                        "Error (/Api/AuthorizationFunctionAutomaticQuotaApi/CreateAutomaticQuota) - Temporal no encontrado: " +
                        id, EventLogEntryType.Information, 0, 1);
                }
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Individual, id.ToString(), null, "Error al emitir");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(
                        "Error (/Api/AuthorizationFunctionApi/CreatePolicyAuthorization) - Error: " + ex,
                        EventLogEntryType.Information, 0, 1);
                }
            }
        }
    }
}
