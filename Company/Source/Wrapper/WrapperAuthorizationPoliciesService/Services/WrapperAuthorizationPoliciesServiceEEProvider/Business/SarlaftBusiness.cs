
namespace Sistran.Company.Application.WrapperAuthorizationPoliciesServiceEEProvider.Business
{
    using Newtonsoft.Json;
    using Sistran.Company.Application.SarlaftApplicationServices.DTO;
    using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
    using Sistran.Core.Application.UniqueUserServices.Enums;
    using Sistran.Core.Application.UniqueUserServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SarlaftBusiness
    {
        public void CreateSarlaftAuthorization(int operationId)
        {
            TmpSarlaftOperationDTO operation = DelegateService.SarlaftApplicationService.GetCompanyTmpSarlaftOperation(operationId);
            string sarlaftId = string.Empty;
            string individualId = string.Empty;
            string message = string.Empty;
            string personType = string.Empty;
            string documentNumber = string.Empty;
            int documentType = 0;
            int userId = 0;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.SarlaftGeneral, operationId.ToString(), null, "Procesando");

                if (operation.TypeProccess.Contains("Sarlaft"))
                {
                    CustomerKnowledgeDTO customerKnowledgeDto = JsonConvert.DeserializeObject<CustomerKnowledgeDTO>(operation.Operation);
                    userId = customerKnowledgeDto.UserId;

                    if (operation.TypeProccess.Contains("Create"))
                    {
                        customerKnowledgeDto = DelegateService.SarlaftApplicationService.CreateSarlaft(customerKnowledgeDto, false);
                        message = Resources.Language.CreateSarlaftAuthoSuccess + " - " + customerKnowledgeDto.SarlaftDTO.Id;
                    }
                    else
                    {
                        customerKnowledgeDto = DelegateService.SarlaftApplicationService.UpdateSarlaft(customerKnowledgeDto, false);
                        message = Resources.Language.UpdateSarlaftAuthoSuccess + " - " + customerKnowledgeDto.SarlaftDTO.Id;
                    }

                    sarlaftId = customerKnowledgeDto.SarlaftDTO.Id.ToString();
                    personType = customerKnowledgeDto.SarlaftDTO.TypePerson.ToString();
                    documentType = customerKnowledgeDto.SarlaftDTO.TypeDocument;
                    documentNumber = customerKnowledgeDto.SarlaftDTO.DocumentNumber.ToString();
                    individualId = customerKnowledgeDto.SarlaftDTO.IndividualId.ToString();
                }

                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.SarlaftGeneral, operationId.ToString(), null, individualId);

                if (documentType == 2 && documentNumber.Length > 9)
                {
                    documentNumber = documentNumber.Substring(0, 9);
                }


                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = userId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.SarlaftPersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"TypePerson", personType},
                                {"DocumentNum", documentNumber},
                                {"SarlaftId", sarlaftId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.SarlaftGeneral, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreateSarlaftAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }
    }
}
