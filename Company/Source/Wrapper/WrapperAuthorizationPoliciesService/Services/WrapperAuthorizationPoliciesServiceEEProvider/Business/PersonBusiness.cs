namespace Sistran.Company.Application.WrapperAuthorizationPoliciesServiceEEProvider.Business
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Core.Application.AuthorizationPoliciesServices.Enums;
    using UniquePersonAplicationServices.DTOs;
    using Sistran.Core.Application.UniqueUserServices.Models;
    using Sistran.Core.Application.UniqueUserServices.Enums;
    using Sistran.Core.Application.OperationQuotaServices.DTOs.OperationQuota;
    using Sistran.Company.Application.UniquePersonServices.V1.Models;

    public class PersonBusiness
    {
        public void CreatePersonAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            string individualId = string.Empty;
            string message = string.Empty;
            int userId = 0;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonGeneral, operationId.ToString(), null, "Procesando");

                if (operation.ProcessType.Contains("Person"))
                {
                    PersonDTO personDto = JsonConvert.DeserializeObject<PersonDTO>(operation.Operation);
                    userId = personDto.UserId;

                    if (operation.ProcessType.Contains("Create"))
                    {
                        personDto = DelegateService.PersonAplicationService.CreateAplicationPerson(personDto, false);
                        message = Resources.Language.CreatePersonAuthoSuccess + " - " + personDto.Id;
                    }
                    else
                    {
                        personDto = DelegateService.PersonAplicationService.UpdateAplicationPerson(personDto, false);
                        message = Resources.Language.UpdatePersonAuthoSuccess + " - " + personDto.Id;
                    }

                    individualId = personDto.Id.ToString();
                }
                else if (operation.ProcessType.Contains("Company"))
                {
                    CompanyDTO companyDto = JsonConvert.DeserializeObject<CompanyDTO>(operation.Operation);
                    userId = companyDto.UserId;

                    if (operation.ProcessType.Contains("Create"))
                    {
                        companyDto = DelegateService.PersonAplicationService.CreateAplicationCompany(companyDto, false);
                        message = Resources.Language.CreateCompanyAuthoSuccess + " - " + companyDto.Id;
                    }
                    else
                    {
                        companyDto = DelegateService.PersonAplicationService.UpdateAplicationCompany(companyDto, false);
                        message = Resources.Language.UpdateCompanyAuthoSuccess + " - " + companyDto.Id;
                    }
                    individualId = companyDto.Id.ToString();
                }

                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonGeneral, operationId.ToString(), null, individualId);


                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = userId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", individualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonGeneral, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreatePersonAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreateInsuredAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            InsuredDTO insuredDto = JsonConvert.DeserializeObject<InsuredDTO>(operation.Operation);
            string message = string.Empty;
            int userId = insuredDto.UserId;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonInsured, operationId.ToString(), null, "Procesando");
                if (operation.ProcessType.Equals("Create"))
                {
                    insuredDto = DelegateService.PersonAplicationService.CreateAplicationInsured(insuredDto, false);
                    message = Resources.Language.CreateInsuredAutho + " - " + insuredDto.IndividualId;
                }
                else
                {
                    insuredDto = DelegateService.PersonAplicationService.UpdateAplicationInsured(insuredDto, false);
                    message = Resources.Language.UpdateInsuredAutho + " - " + insuredDto.IndividualId;
                }
                insuredDto.UserId = userId;
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonInsured, operationId.ToString(), null, insuredDto.Id.ToString());

                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = insuredDto.UserId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", insuredDto.IndividualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonInsured, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreateInsuredAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreateProviderAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            ProviderDTO providerDto = JsonConvert.DeserializeObject<ProviderDTO>(operation.Operation);
            int userId = providerDto.UserId;
            string message = string.Empty;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonProvider, operationId.ToString(), null, "Procesando");
                if (operation.ProcessType.Equals("Create"))
                {
                    providerDto = DelegateService.PersonAplicationService.CreateAplicationSupplier(providerDto, false);
                    message = Resources.Language.CreateProviderAutho + " - " + providerDto.IndividualId;
                }
                else
                {
                    providerDto = DelegateService.PersonAplicationService.UpdateAplicationSupplier(providerDto, false);
                    message = Resources.Language.UpdateProviderAutho + " - " + providerDto.IndividualId;
                }
                providerDto.UserId = userId;
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonProvider, operationId.ToString(), null, providerDto.IndividualId.ToString());

                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = providerDto.UserId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", providerDto.IndividualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonProvider, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreateProviderAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreateAgentAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            AgentDTO agentDto = JsonConvert.DeserializeObject<AgentDTO>(operation.Operation);
            int userId = agentDto.UserId;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonIntermediary, operationId.ToString(), null, "Procesando");
                agentDto = DelegateService.PersonAplicationService.ProcessAplicationAgent(agentDto, false);
                agentDto.UserId = userId;
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonIntermediary, operationId.ToString(), null, agentDto.IndividualId.ToString());

                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = agentDto.UserId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = Resources.Language.CreateAgentAutho + " - " + agentDto.IndividualId,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", agentDto.IndividualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonIntermediary, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreateAgentAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreateReInsurerAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            ReInsurerDTO reInsurerDto = JsonConvert.DeserializeObject<ReInsurerDTO>(operation.Operation);
            int userId = reInsurerDto.UserId;
            string message = string.Empty;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonReinsurer, operationId.ToString(), null, "Procesando");

                if (operation.ProcessType.Equals("Create"))
                {
                    reInsurerDto = DelegateService.PersonAplicationService.CreateAplicationReInsurer(reInsurerDto, false);
                    message = Resources.Language.CreateReInsurerAutho + " - " + reInsurerDto.IndividualId;
                }
                else
                {
                    reInsurerDto = DelegateService.PersonAplicationService.UpdateAplicationReInsurer(reInsurerDto, false);
                    message = Resources.Language.UpdateReInsurerAutho + " - " + reInsurerDto.IndividualId;
                }
                reInsurerDto.UserId = userId;
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonReinsurer, operationId.ToString(), null, reInsurerDto.IndividualId.ToString());

                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = reInsurerDto.UserId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", reInsurerDto.IndividualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);

            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonReinsurer, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreateAgentAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreateQuotaAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);

            dynamic operationObject = JsonConvert.DeserializeObject<dynamic>(operation.Operation);

            List<OperatingQuotaDTO> operatingQuotaDtos = JsonConvert.DeserializeObject<List<OperatingQuotaDTO>>(operationObject.ListOperatingQuotaDTO.ToString());
            List<OperatingQuotaEventDTO> operatingQuotaEventDTOs = JsonConvert.DeserializeObject<List<OperatingQuotaEventDTO>>(operationObject.operatingQuotaEventDTOs.ToString());
            int userId = operatingQuotaDtos.First().UserId;

            string message = string.Empty;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonOperatingQuota, operationId.ToString(), null, "Procesando");

                if (operation.ProcessType.Equals("Create"))
                {
                    operatingQuotaDtos = DelegateService.PersonAplicationService.CreateOperatingQuota(operatingQuotaDtos, operatingQuotaEventDTOs, false);
                    message = Resources.Language.CreateQuotaAutho + " - " + (int)operation.IndividualId;
                }
                else
                {
                    //operatingQuotaDtos = DelegateService.PersonAplicationService.UpdateOperatingQuota(operatingQuotaDtos, false);
                    //message = Resources.Language.UpdateQuotaAutho + " - " + operatingQuotaDtos.FirstOrDefault().IndividualId;
                }

                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonOperatingQuota, operationId.ToString(), null, operation.IndividualId.ToString());
                operatingQuotaDtos.First().UserId = userId;
                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = operatingQuotaDtos.FirstOrDefault().UserId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", (int)operation.IndividualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonOperatingQuota, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreateAgentAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreateTaxAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            List<IndividualTaxExeptionDTO> individualTaxExeptionDtos = JsonConvert.DeserializeObject<List<IndividualTaxExeptionDTO>>(operation.Operation);
            int userId = individualTaxExeptionDtos.First().UserId;
            string message = string.Empty;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonTaxes, operationId.ToString(), null, "Procesando");

                if (operation.ProcessType.Equals("Create"))
                {
                    individualTaxExeptionDtos = DelegateService.PersonAplicationService.CreateIndividualTax(individualTaxExeptionDtos, false);
                    message = Resources.Language.CreateTaxAutho + " - " + operation.IndividualId;
                }
                else
                {
                    individualTaxExeptionDtos = DelegateService.PersonAplicationService.UpdateIndividualTaxExeption(individualTaxExeptionDtos, false);
                    message = Resources.Language.UpdateTaxAutho + " - " + operation.IndividualId;
                }
                individualTaxExeptionDtos.First().UserId = userId;
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonTaxes, operationId.ToString(), null, operation.IndividualId.ToString());

                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = individualTaxExeptionDtos.FirstOrDefault().UserId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", operation.IndividualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonTaxes, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreateAgentAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreateCoInsuredAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            CompanyCoInsuredDTO coInsuredDto = JsonConvert.DeserializeObject<CompanyCoInsuredDTO>(operation.Operation);
            int userId = coInsuredDto.UserId;
            string message = string.Empty;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonCoinsurer, operationId.ToString(), null, "Procesando");

                if (operation.ProcessType.Equals("Create"))
                {
                    coInsuredDto = DelegateService.PersonAplicationService.CreateCompanyCoInsured(coInsuredDto, false);
                    message = Resources.Language.CreateCoInsuredAutho + " - " + coInsuredDto.IndividualId;
                }
                else
                {
                    coInsuredDto = DelegateService.PersonAplicationService.UpdateCompanyCoInsured(coInsuredDto, false);
                    message = Resources.Language.UpdateCoInsuredAutho + " - " + coInsuredDto.IndividualId;
                }
                coInsuredDto.UserId = userId;
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonCoinsurer, operationId.ToString(), null, coInsuredDto.IndividualId.ToString());

                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = coInsuredDto.UserId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", coInsuredDto.IndividualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonCoinsurer, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreateCoInsuredAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreateThirdAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            ThirdPartyDTO thirdPartyDto = JsonConvert.DeserializeObject<ThirdPartyDTO>(operation.Operation);
            int userId = thirdPartyDto.UserId;
            string message = string.Empty;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonThird, operationId.ToString(), null, "Procesando");

                if (operation.ProcessType.Equals("Create"))
                {
                    thirdPartyDto = DelegateService.PersonAplicationService.CreateAplicationThird(thirdPartyDto, false);
                    message = Resources.Language.CreateThirdAutho + " - " + thirdPartyDto.IndividualId;
                }
                else
                {
                    thirdPartyDto = DelegateService.PersonAplicationService.UpdateAplicationThird(thirdPartyDto, false);
                    message = Resources.Language.UpdateThirdAutho + " - " + thirdPartyDto.IndividualId;
                }
                thirdPartyDto.UserId = userId;
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonThird, operationId.ToString(), null, thirdPartyDto.IndividualId.ToString());

                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = thirdPartyDto.UserId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", thirdPartyDto.IndividualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonThird, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreateThirdAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreateEmployedAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            EmployeeDTO employeeDto = JsonConvert.DeserializeObject<EmployeeDTO>(operation.Operation);
            int userId = employeeDto.UserId;
            string message = string.Empty;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonEmployed, operationId.ToString(), null, "Procesando");

                if (operation.ProcessType.Equals("Create"))
                {
                    employeeDto = DelegateService.PersonAplicationService.CreateEmployee(employeeDto, false);
                    message = Resources.Language.CreateEmployedAutho + " - " + employeeDto.IndividualId;
                }
                else
                {
                    //employeeDto = DelegateService.PersonAplicationService.UpdateEmployee(employeeDto, false);
                    //message = Resources.Language.UpdateEmployedAutho + " - " + employeeDto.IndividualId;
                }
                employeeDto.UserId = userId;
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonEmployed, operationId.ToString(), null, employeeDto.IndividualId.ToString());

                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = employeeDto.UserId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", employeeDto.IndividualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonEmployed, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreateEmployedAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreatePersonalInformationAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            PersonInformationAndLabourDTO personInformationAndLabourDto = JsonConvert.DeserializeObject<PersonInformationAndLabourDTO>(operation.Operation);
            int userId = personInformationAndLabourDto.UserId;
            string message = string.Empty;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonPersonalInf, operationId.ToString(), null, "Procesando");

                if (operation.ProcessType.Equals("Create"))
                {
                    personInformationAndLabourDto = DelegateService.PersonAplicationService.CreateApplicationLabourPerson(personInformationAndLabourDto, personInformationAndLabourDto.IndividualId, false);
                    message = Resources.Language.CreatePersonalInformationAutho + " - " + personInformationAndLabourDto.IndividualId;
                }
                else
                {
                    personInformationAndLabourDto = DelegateService.PersonAplicationService.UpdateApplicationLabourPerson(personInformationAndLabourDto, false);
                    message = Resources.Language.UpdatePersonalInformationAutho + " - " + personInformationAndLabourDto.IndividualId;
                }
                personInformationAndLabourDto.UserId = userId;
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonPersonalInf, operationId.ToString(), null, personInformationAndLabourDto.IndividualId.ToString());

                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = personInformationAndLabourDto.UserId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", personInformationAndLabourDto.IndividualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonPersonalInf, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreatePersonalInformationAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreatePaymentMethodsAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            List<IndividualPaymentMethodDTO> individualPaymentMethodDTO = JsonConvert.DeserializeObject<List<IndividualPaymentMethodDTO>>(operation.Operation);
            int userId = individualPaymentMethodDTO.First().UserId;
            string message = string.Empty;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonPaymentMethods, operationId.ToString(), null, "Procesando");

                if (operation.ProcessType.Equals("Create"))
                {
                    individualPaymentMethodDTO = DelegateService.PersonAplicationService.CreateIndividualpaymentMethods(individualPaymentMethodDTO, (int)operation.IndividualId, false);
                    message = Resources.Language.CreatePaymentMethodsAutho + " - " + operation.IndividualId;
                }
                else
                {
                    //individualPaymentMethodDTO = DelegateService.PersonAplicationService.UpdateIndividualpaymentMethods(individualPaymentMethodDTO, false);
                    //message = Resources.Language.UpdatePaymentMethodsAutho + " - " + individualPaymentMethodDTO.FirstOrDefault().Id;
                }
                individualPaymentMethodDTO.First().UserId = userId;
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonPaymentMethods, operationId.ToString(), null, (operation.IndividualId).ToString());

                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = individualPaymentMethodDTO.FirstOrDefault().UserId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", operation.IndividualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonPaymentMethods, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreatePaymentMethodsAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreateBankTransfersAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            List<BankTransfersDTO> bankTransfersDTO = JsonConvert.DeserializeObject<List<BankTransfersDTO>>(operation.Operation);
            int userId = bankTransfersDTO.First().UserId;
            string message = string.Empty;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonBankTransfers, operationId.ToString(), null, "Procesando");

                if (operation.ProcessType.Equals("Create"))
                {
                    bankTransfersDTO = DelegateService.PersonAplicationService.CreateBankTransfers(bankTransfersDTO, false);
                    message = Resources.Language.CreateBankTransfersAutho + " - " + operation.IndividualId;
                }
                else
                {
                    bankTransfersDTO = DelegateService.PersonAplicationService.UpdateAplicationBankTransfers(bankTransfersDTO, false);
                    message = Resources.Language.UpdateBankTransfersAutho + " - " + operation.IndividualId;
                }
                bankTransfersDTO.First().UserId = userId;
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonBankTransfers, operationId.ToString(), null, operation.IndividualId.ToString());

                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = bankTransfersDTO.FirstOrDefault().UserId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", bankTransfersDTO.FirstOrDefault().IndividualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonBankTransfers, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreateBankTransfersAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreateConsortiatesAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            List<ConsorciatedDTO> consorciatedDto = JsonConvert.DeserializeObject<List<ConsorciatedDTO>>(operation.Operation);
            int userId = consorciatedDto.First().UserId;
            string message = string.Empty;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonConsortiates, operationId.ToString(), null, "Procesando");

                if (operation.ProcessType.Equals("Create"))
                {
                    consorciatedDto = DelegateService.PersonAplicationService.CreateConsortium(consorciatedDto, (int)operation.IndividualId, false);
                    message = Resources.Language.CreateConsortiatesAutho + " - " + operation.IndividualId;
                }
                else
                {
                    consorciatedDto = DelegateService.PersonAplicationService.UpdateConsortium(consorciatedDto, false);
                    message = Resources.Language.UpdateConsortiatesAutho + " - " + operation.IndividualId;
                }
                consorciatedDto.First().UserId = userId;
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonConsortiates, operationId.ToString(), null, operation.IndividualId.ToString());

                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = consorciatedDto.FirstOrDefault().UserId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", (int)operation.IndividualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonConsortiates, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreateConsortiatesAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreateBusinessNameAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            List<CompanyNameDTO> companyNameDto = JsonConvert.DeserializeObject<List<CompanyNameDTO>>(operation.Operation);
            int userId = companyNameDto.First().UserId;
            string message = string.Empty;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonBusinessName, operationId.ToString(), null, "Procesando");

                if (operation.ProcessType.Equals("Create"))
                {
                    companyNameDto = DelegateService.PersonAplicationService.CreateBusinessName(companyNameDto, false);
                    message = Resources.Language.CreateBusinessNameAutho + " - " + operation.IndividualId;
                }
                else
                {
                    companyNameDto = DelegateService.PersonAplicationService.UpdateAplicationBusinessName(companyNameDto, false);
                    message = Resources.Language.UpdateBusinessNameAutho + " - " + operation.IndividualId;
                }
                companyNameDto.First().UserId = userId;
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonBusinessName, operationId.ToString(), null, operation.IndividualId.ToString());

                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = companyNameDto.FirstOrDefault().UserId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId",operation.IndividualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonBusinessName, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreateBusinessNameAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreateGuaranteeAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            GuaranteeDto guaranteeDto = JsonConvert.DeserializeObject<GuaranteeDto>(operation.Operation);

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonGuarantees, operationId.ToString(), null, "Procesando");

                if (operation.ProcessType.Equals("Create"))
                {
                    guaranteeDto = DelegateService.PersonAplicationService.SaveApplicationInsuredGuarantee(guaranteeDto, false);

                    string message = Resources.Language.CreateGuaranteeAutho + " - " + guaranteeDto.Guarantee.InsuredGuarantee.IndividualId;

                    DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonGuarantees, operationId.ToString(), null, guaranteeDto.Guarantee.InsuredGuarantee.IndividualId.ToString());

                    NotificationUser notificationUser = new NotificationUser
                    {
                        UserId = guaranteeDto.UserId,
                        CreateDate = DateTime.Now,
                        NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                        Message = message,
                        Parameters = new Dictionary<string, object>
                        {
                            {"IndividualId", guaranteeDto.Guarantee.InsuredGuarantee.IndividualId},
                        }
                    };

                    DelegateService.UniqueUserService.CreateNotification(notificationUser);
                }
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonGuarantees, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreateGuaranteeAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }

        public void CreateBasicInfoAuthorization(int operationId)
        {
            PersonOperationDTO operation = DelegateService.PersonAplicationService.GetPersonOperation(operationId);
            string individualId = string.Empty;
            string message = string.Empty;
            int userId = 0;

            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonBasicInfo, operationId.ToString(), null, "Procesando");

                if (operation.ProcessType.Contains("Update Person Basic Info"))
                {
                    CompanyPerson personModel = JsonConvert.DeserializeObject<CompanyPerson>(operation.Operation);
                    userId = personModel.UserId;
                    personModel = DelegateService.PersonAplicationServiceV1.UpdateApplicationPersonBasicInfo(personModel, false);
                    message = Resources.Language.UpdatePersonAuthoSuccess + " - " + personModel.IndividualId;

                    individualId = personModel.IndividualId.ToString();
                }
                else if (operation.ProcessType.Contains("Update Company Basic Info"))
                {
                    CompanyCompany companyModel = JsonConvert.DeserializeObject<CompanyCompany>(operation.Operation);
                    userId = companyModel.UserId;

                    companyModel = DelegateService.PersonAplicationServiceV1.UpdateApplicationCompanyBasicInfo(companyModel, false);
                    message = Resources.Language.UpdateCompanyAuthoSuccess + " - " + companyModel.IndividualId;

                    individualId = companyModel.IndividualId.ToString();
                }

                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonBasicInfo, operationId.ToString(), null, individualId);


                NotificationUser notificationUser = new NotificationUser
                {
                    UserId = userId,
                    CreateDate = DateTime.Now,
                    NotificationType = new NotificationType { Type = NotificationTypes.PersonAccept },
                    Message = message,
                    Parameters = new Dictionary<string, object>
                            {
                                {"NotificationIndividualId", individualId},
                            }
                };

                DelegateService.UniqueUserService.CreateNotification(notificationUser);
            }
            catch (Exception ex)
            {
                DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.PersonBasicInfo, operationId.ToString(), null, "Error al procesar");
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Error (/Wrapper/CreateBasicInfoAuthorization) - Error: " + ex, EventLogEntryType.Information, 0, 1);
                }
            }
        }
    }
}