using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.DAOs;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.BAF;
using MPolicies = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using MRules = Sistran.Core.Application.RulesScriptsServices.Models;
using Rules = Sistran.Core.Framework.Rules;
using SCREN = Sistran.Core.Application.Script.Entities;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider
{
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using Newtonsoft.Json;
    using Resources;
    using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
    using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
    using Sistran.Core.Application.UniqueUserServices.Enums;
    using Sistran.Core.Application.Utilities.Cache;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using Utilities.Helper;
    using TP = Sistran.Core.Application.Utilities.Utility;

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class AuthorizationPoliciesServiceEEProvider : IAuthorizationPoliciesServiceCore
    {
        #region Lanzado de politicas

        /// <summary>
        /// Realiza la validacion de las politicas
        /// </summary>
        /// <param name="idPackage">Id del package de politicas</param>
        /// <param name="key">llave filtro</param>
        /// <param name="parameters">Lista de facades</param>
        /// <param name="facadeType">nivel que se ejecuta</param>
        /// <returns>Lista de politicas infringidas</returns>
        public List<MPolicies.PoliciesAut> ValidateAuthorizationPolicies(int idPackage, object key, object parameter, FacadeType facadeType)
        {
            try
            {
                Rules.Facade facade = (Rules.Facade)parameter;

                ParametersDao parametersDao = new ParametersDao();
                int hierarchy = parametersDao.GetHierarchyByIdUser(idPackage, facade.GetConcept<int>(RuleConceptPolicies.UserId));

                PoliciesDao policiesDao = new PoliciesDao();
                List<int> ruleToValidate = policiesDao.GetRulesToValidate(idPackage, hierarchy, key.ToString(), facadeType);

                return this.ValidateAuthorizationPoliciesMassive(idPackage, key, parameter, facadeType, hierarchy, ruleToValidate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorValidateAuthorizationPolicies), ex);
            }
        }

        /// <summary>
        /// Realiza la validacion de las politicas
        /// </summary>
        /// <param name="idPackage">Id del package de politicas</param>
        /// <param name="key">llave filtro</param>
        /// <param name="parameter">Lista de facades</param>
        /// <param name="facadeType">nivel que se ejecuta</param>
        /// <returns>Lista de politicas infringidas</returns>
        public List<MPolicies.PoliciesAut> ValidateAuthorizationPoliciesMassive(int idPackage, object key, object parameter, FacadeType facadeType, int hierarchy, List<int> ruleToValidate)
        {
            try
            {
                Rules.Facade facade = (Rules.Facade)parameter;
                List<MPolicies.PoliciesAut> policiesList = new List<MPolicies.PoliciesAut>();
                PoliciesDao policiesDao = new PoliciesDao();

                List<Rules.Concept> concepts = new List<Rules.Concept>(facade.Concepts);
                int userId = facade.GetConcept<int>(RuleConceptPolicies.UserId);

                foreach (int eventId in ruleToValidate)
                {
                    facade.Concepts.Clear();
                    concepts.ForEach(x =>
                     {
                         if (x.IsStatic)
                         {
                             facade.SetConcept(new KeyValuePair<string, int>(x.Name, x.EntityId), x.Value);
                         }
                         else
                         {
                             facade.SetConcept(new KeyValuePair<int, int>(x.Id, x.EntityId), x.Value);
                         }
                     });


                    facade.SetConcept(RuleConceptPolicies.EventId, eventId);
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, false);

                    facade = RulesEngineDelegate.ExecuteRules(eventId, facade);

                    if (facade.GetConcept<bool>(RuleConceptPolicies.GenerateEvent))
                    {
                        policiesList.Add(new MPolicies.PoliciesAut
                        {
                            IdPolicies = eventId,
                            IdHierarchyPolicy = hierarchy,
                            IdHierarchyAut = facade.GetConcept<int?>(RuleConceptPolicies.Hierarchy) ?? -1,
                            ConceptsDescription = GetConceptsDescriptionValues(eventId, facade)
                        });
                    }
                }

                return policiesDao.GetPoliciesByRules(userId, policiesList.ToList());
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorValidateAuthorizationPolicies), ex);
            }
        }

        /// <summary>
        /// Obtiene los id de las reglas que se van a validar para politica
        /// </summary>
        /// <param name="idPackage">Id del paquete</param>
        /// <param name="hierarchy">Jeraquia del usuario que ejecuta</param>
        /// <param name="key">llave filtro</param>
        /// <param name="facadeType"></param>
        /// <returns>lista de id's de reglas</returns>
        public List<int> GetRulesToValidate(int idPackage, int hierarchy, string key, FacadeType facadeType)
        {
            try
            {
                PoliciesDao policiesDao = new PoliciesDao();
                return policiesDao.GetRulesToValidate(idPackage, hierarchy, key, facadeType);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetRulesToValidate), e);
            }
        }

        /// <summary>
        /// Consulta la jeraquia de un usuario
        /// </summary>
        /// <param name="idPackage">Id del paquete</param>
        /// <param name="idUser">id del usuario que ejecuta</param>
        /// <exception cref="BusinessException"></exception>
        /// <returns></returns>
        public int GetHierarchyByIdUser(int idPackage, int idUser)
        {
            try
            {
                ParametersDao parametersDao = new ParametersDao();
                return parametersDao.GetHierarchyByIdUser(idPackage, idUser);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetHierarchyByUser), e);
            }
        }

        /// <summary>
        /// obtiene los valores de los conceptos de la descripcion de la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="parameters">lista de facades</param>
        /// <returns></returns>
        private static List<MPolicies.ConceptDescription> GetConceptsDescriptionValues(int idPolicies, Rules.Facade facade)
        {
            try
            {
                ConceptDescriptionValueDao conceptDescriptionValueDao = new ConceptDescriptionValueDao();
                ConceptDescriptionDao conceptDescriptionDao = new ConceptDescriptionDao();
                List<MPolicies.ConceptDescription> conceptDescriptions = conceptDescriptionDao.GetConceptDescriptionsByIdPolicies(idPolicies);

                foreach (MPolicies.ConceptDescription conceptDescription in conceptDescriptions)
                {
                    object concept = conceptDescription.Concept.IsStatic ?
                            facade.GetConcept<object>(new KeyValuePair<string, int>(conceptDescription.Concept.ConceptName, conceptDescription.Concept.Entity.EntityId)) :
                            facade.GetConcept<object>(new KeyValuePair<int, int>(conceptDescription.Concept.ConceptId, conceptDescription.Concept.Entity.EntityId));

                    if (concept != null)
                    {
                        if (conceptDescription.ConceptDescriptionValue != null)
                        {
                            List<string> pars = new List<string>
                            {
                                concept.ToString()
                            };

                            if (conceptDescription.Concept.ConceptDependences.Any())
                            {

                                for (int i = 0; i < conceptDescription.Concept.ConceptDependences.Count(); i++)  // order
                                {
                                    object conceptDependence = conceptDescription.Concept.ConceptDependences[i].DependsConcept.IsStatic ?
                           facade.GetConcept<object>(new KeyValuePair<string, int>(conceptDescription.Concept.ConceptDependences[i].DependsConcept.ConceptName, conceptDescription.Concept.ConceptDependences[i].DependsConcept.Entity.EntityId)) :
                           facade.GetConcept<object>(new KeyValuePair<int, int>(conceptDescription.Concept.ConceptDependences[i].DependsConcept.ConceptId, conceptDescription.Concept.ConceptDependences[i].DependsConcept.Entity.EntityId));

                                    pars.Add(conceptDependence.ToString());
                                }
                            }
                            conceptDescription.Value = conceptDescriptionValueDao.GetValueConceptDescription(conceptDescription.ConceptDescriptionValue, pars.ToArray());
                        }
                        else
                        {
                            int entityId = int.Parse(EnumHelper.GetEnumParameterValue(FacadeType.RULE_FACADE_GENERAL).ToString());
                            Func<SCREN.Concept, bool> entityPredicate = c => c != null && c.ConceptName == "DocumentNumber" && c.EntityId == entityId;
                            SCREN.Concept conceptDocumentNumber = (SCREN.Concept)InProcCache.Instance.GetCurrent(RulesConstant.Entityconcept, entityPredicate);

                            if (Type.GetTypeCode(concept.GetType()) == TypeCode.Decimal && (conceptDocumentNumber.ConceptId != conceptDescription.Concept.ConceptId && conceptDocumentNumber.EntityId != conceptDescription.Concept.Entity.EntityId))
                            {
                                try
                                {
                                    decimal.TryParse(concept.ToString(), out decimal @out);
                                    conceptDescription.Value = @out.ToString("N2");
                                }
                                catch (Exception)
                                {
                                    conceptDescription.Value = concept.ToString();
                                }
                            }
                            else
                            {
                                conceptDescription.Value = concept.ToString();
                            }
                        }
                    }
                    else
                    {
                        conceptDescription.Value = string.Empty;
                    }

                    conceptDescription.Concept = null;
                    conceptDescription.ConceptDescriptionValue = null;
                    conceptDescription.Policies = null;
                }
                return conceptDescriptions;
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetConceptsDescriptionValues), e);
            }
        }


        #endregion

        #region UsuariosAutorizadores

        /// <summary>
        /// Obtiene la lista de usuarios autorizadores para la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="idHierarchy">id de la jerarquia</param>
        /// <returns>lista de usuarios autorizadores</returns>
        public List<MPolicies.UserAuthorization> GetUsersAutorizationByIdPoliciesIdHierarchy(int idPolicies, int? idHierarchy, List<Models.UserGroupModel> usersGroup = null)
        {
            try
            {
                UserAuthorizationDao authorizationDao = new UserAuthorizationDao();
                return authorizationDao.GetUsersAutorizationByIdPoliciesIdHierarchy(idPolicies, idHierarchy, usersGroup);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetUsersAutorization), e);
            }

        }

        /// <summary>
        /// Crea los usuarios autorizadores para la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="users">lista de usuarios</param>
        /// <param name="countMin">numero minimo de autorizadores</param>
        /// <returns></returns>
        public void CreateUsersAutorization(int idPolicies, List<MPolicies.UserAuthorization> users, int countMin)
        {
            try
            {
                UserAuthorizationDao authorizationDao = new UserAuthorizationDao();
                authorizationDao.CreateUsersAutorization(idPolicies, users, countMin);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorCreateUsersAutorization), e);
            }
        }

        #endregion

        #region UsuariosNotificadores

        /// <summary>
        /// Obtiene la lista de usuarios notificadores para la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="idHierarchy">id de la jerarquia</param>
        /// <returns>lista de usuarios notificadores</returns>
        public List<MPolicies.UserNotification> GetUsersNotificationByIdPoliciesIdHierarchy(int idPolicies, int? idHierarchy, List<Models.UserGroupModel> usersGroup = null)
        {
            try
            {
                UserNotificationDao notificationDao = new UserNotificationDao();
                return notificationDao.GetUsersNotificationByIdPoliciesIdHierarchy(idPolicies, idHierarchy, usersGroup);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetUsersNotificaticion), e);
            }
        }

        /// <summary>
        /// Crea los usuarios notificadores para la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="users">lista de usuarios</param>
        /// <returns></returns>
        public void CreateUsersNotification(int idPolicies, List<MPolicies.UserNotification> users)
        {
            try
            {
                UserNotificationDao userNotificationDao = new UserNotificationDao();
                userNotificationDao.CreateUsersNotification(idPolicies, users);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorCreateUsersNotification), e);
            }
        }

        #endregion

        #region SolicitudAutorizacion 

        /// <summary>
        /// realiza el guardado de las solicitudes de autorizacion 
        /// </summary>
        /// <param name="authorizationRequests">lista de solicitudes de autorizacion</param>
        /// <returns></returns>
        public void CreateAutorizationRequest(List<MPolicies.AuthorizationRequest> authorizationRequests)
        {
            try
            {
                List<EmailCriteria> emailsToSend = new List<EmailCriteria>();
                List<NotificationUser> notifications = new List<NotificationUser>();

                AuthorizationRequestDao authorizationRequestDao = new AuthorizationRequestDao();
                AuthorizationAnswerDao authorizationAnswerDao = new AuthorizationAnswerDao();

                DateTime now = DateTime.Now;

                foreach (MPolicies.AuthorizationRequest authorization in authorizationRequests)
                {
                    authorization.Description = string.IsNullOrEmpty(authorization.Description) ? "" : authorization.Description;
                    authorization.DateRequest = now;

                    string message = $"<h2>{authorization.Policies.Description}</h2>" +
                                  $"{authorization.Policies.Message}</br></br>" +
                                  $"Usuario: {authorization.UserRequest.AccountName}</br>" +
                                  $"Mensaje de solicitud: {authorization.DescriptionRequest}</br></br>" +
                                  $"{(string.IsNullOrEmpty(authorization.Description) ? "" : authorization.Description.Replace("|", "</br>"))}";

                    EmailCriteria emailAut = new EmailCriteria
                    {
                        Subject = "Solicitud de autorización de la politica - " + authorization.Policies.Description,
                        Message = message,
                        Addressed = new List<string>()
                    };
                    EmailCriteria emailNot = new EmailCriteria
                    {
                        Subject = "Notificacion de la politica - " + authorization.Policies.Description,
                        Message = message,
                        Addressed = new List<string>()
                    };

                    MPolicies.AuthorizationRequest authorizationTmp = authorizationRequestDao.CreateAuthorizationRequest(authorization);
                    foreach (MPolicies.AuthorizationAnswer answer in authorization.AuthorizationAnswers)
                    {
                        answer.AuthorizationRequest = new MPolicies.AuthorizationRequest { AuthorizationRequestId = authorizationTmp.AuthorizationRequestId };
                        answer.DateAnswer = null;
                        authorizationAnswerDao.CreateAuthorizationAnswer(answer);

                        emailAut.Addressed.Add(answer.UserAnswer.UserId.ToString());


                        NotificationUser notificationUser = new NotificationUser
                        {
                            UserId = answer.UserAnswer.UserId,
                            NotificationType = new NotificationType { Type = NotificationTypes.AutorizationPolicies },
                            Parameters = new Dictionary<string, object> { { "key", authorization.Key }, { "IdPolicies", authorization.Policies.IdPolicies }, { "FunctionType", authorization.FunctionType } },
                        };

                        switch (authorization.FunctionType)
                        {
                            case TypeFunction.Individual:
                                notificationUser.Message = $"Se ha generado la Politica - {authorization.Policies.Description} (Temporal {authorization.Key})";
                                break;

                            case TypeFunction.Massive:
                            case TypeFunction.Collective:
                                notificationUser.Message = $"Se ha generado la Politica - {authorization.Policies.Description} (Cargue {authorization.Key})";
                                break;
                            case TypeFunction.Claim:
                            case TypeFunction.PaymentRequest:
                            case TypeFunction.ClaimNotice:
                            case TypeFunction.ChargeRequest:
                                notificationUser.Message = $"Se ha generado la Politica - {authorization.Policies.Description} (Temporal {authorization.Key})";
                                break;
                            case TypeFunction.PersonGeneral:
                            case TypeFunction.PersonInsured:
                            case TypeFunction.PersonProvider:
                            case TypeFunction.PersonThird:
                            case TypeFunction.PersonIntermediary:
                            case TypeFunction.PersonEmployed:
                            case TypeFunction.PersonPersonalInf:
                            case TypeFunction.PersonPaymentMethods:
                            case TypeFunction.PersonGuarantees:
                            case TypeFunction.PersonOperatingQuota:
                            case TypeFunction.PersonTaxes:
                            case TypeFunction.PersonBankTransfers:
                            case TypeFunction.PersonReinsurer:
                            case TypeFunction.PersonCoinsurer:
                            case TypeFunction.PersonConsortiates:
                            case TypeFunction.PersonBusinessName:
                            case TypeFunction.PersonBasicInfo:
                                notificationUser.Message = $"Se ha generado la Politica - {authorization.Policies.Description} (Temporal {authorization.Key})";
                                break;

                            case TypeFunction.SarlaftGeneral:
                                notificationUser.Message = $"Se ha generado la Politica - {authorization.Policies.Description} (Temporal {authorization.Key})";
                                break;
                            case TypeFunction.AutomaticQuota:
                                notificationUser.Message = $"Se ha generado la cuota automática - {authorization.Policies.Description} (Temporal {authorization.Key})";
                                break;
                        }
                        if (!notifications.Any(x => x.UserId == notificationUser.UserId && x.Message == notificationUser.Message))
                        {
                            notifications.Add(notificationUser);
                        }
                    }

                    foreach (User user in authorization.NotificationUsers)
                    {
                        emailNot.Addressed.Add(user.UserId.ToString());

                        NotificationUser notificationUser = new NotificationUser
                        {
                            UserId = user.UserId,
                            NotificationType = new NotificationType { Type = NotificationTypes.NotificationPolicies },
                        };

                        switch (authorization.FunctionType)
                        {
                            case TypeFunction.Individual:
                                notificationUser.Message = $"Se ha generado la Politica - {authorization.Policies.Description} (Temporal {authorization.Key})";
                                break;

                            case TypeFunction.Massive:
                            case TypeFunction.Collective:
                                notificationUser.Message = $"Se ha generado la Politica - {authorization.Policies.Description} (Cargue {authorization.Key})";
                                break;
                            case TypeFunction.Claim:
                            case TypeFunction.PaymentRequest:
                            case TypeFunction.ClaimNotice:
                            case TypeFunction.ChargeRequest:
                                notificationUser.Message = $"Se ha generado la Politica - {authorization.Policies.Description} (Temporal {authorization.Key})";
                                break;

                            case TypeFunction.PersonGeneral:
                            case TypeFunction.PersonInsured:
                            case TypeFunction.PersonProvider:
                            case TypeFunction.PersonThird:
                            case TypeFunction.PersonIntermediary:
                            case TypeFunction.PersonEmployed:
                            case TypeFunction.PersonPersonalInf:
                            case TypeFunction.PersonPaymentMethods:
                            case TypeFunction.PersonGuarantees:
                            case TypeFunction.PersonOperatingQuota:
                            case TypeFunction.PersonTaxes:
                            case TypeFunction.PersonBankTransfers:
                            case TypeFunction.PersonReinsurer:
                            case TypeFunction.PersonCoinsurer:
                            case TypeFunction.PersonConsortiates:
                            case TypeFunction.PersonBusinessName:
                            case TypeFunction.PersonBasicInfo:
                                notificationUser.Message = $"Se ha generado la Politica - {authorization.Policies.Description} (Temporal {authorization.Key})";
                                break;

                            case TypeFunction.SarlaftGeneral:
                                notificationUser.Message = $"Se ha generado la Politica - {authorization.Policies.Description} (Temporal {authorization.Key})";
                                break;
                            case TypeFunction.AutomaticQuota:
                                notificationUser.Message = $"Se ha generado la cuota automática - {authorization.Policies.Description} (Temporal {authorization.Key})";
                                break;
                        }
                        if (!notifications.Any(x => x.UserId == notificationUser.UserId && x.Message == notificationUser.Message))
                        {
                            notifications.Add(notificationUser);
                        }
                    }

                    emailsToSend.Add(emailAut);
                    if (emailNot.Addressed.Count != 0)
                    {
                        emailsToSend.Add(emailNot);
                    }
                }

                this.SendEmails(emailsToSend);
                DelegateService.UniqueUserService.CreateNotifications(notifications);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorCreateAutorizationRequest), e);
            }
        }

        /// <summary>
        /// realiza el guardado de las solicitudes de autorizacion para masivos
        /// </summary>
        /// <param name="authorizationRequests">lista de solicitudes de autorizacion</param>
        /// <returns></returns>
        public void CreateMassiveAutorizationRequest(List<MPolicies.AuthorizationRequest> authorizationRequests)
        {
            try
            {
                AuthorizationRequestDao authorizationRequestDao = new AuthorizationRequestDao();
                authorizationRequestDao.CreateMassiveAutorizationRequest(authorizationRequests);

                TP.Task.Run(() =>
                {
                    try
                    {
                        User user = DelegateService.UniqueUserService.GetUserById(authorizationRequests.First().UserRequest.UserId);
                        ConcurrentBag<NotificationUser> notificationUsers = new ConcurrentBag<NotificationUser>();
                        ConcurrentBag<EmailCriteria> emailsToSend = new ConcurrentBag<EmailCriteria>();
                        var authorizationGroup = authorizationRequests.GroupBy(x => new { x.Policies.IdPolicies }).ToList();
                        ParallelHelper.ForEach(authorizationGroup, group =>
                        {
                            MPolicies.PoliciesAut policie = group.First().Policies;
                            MPolicies.AuthorizationRequest request = authorizationRequests.First(x => x.Policies.IdPolicies == policie.IdPolicies);
                            string description = string.Join("</br>", group.ToList().Select(x => x.Description));

                            List<User> users = group.First().AuthorizationAnswers.Select(y => y.UserAnswer).ToList();

                            emailsToSend.Add(new EmailCriteria
                            {
                                Subject = "Solicitud de autorización de las politicas  del " +
                                  request.DescriptionRequest,
                                Message = $"<h2>{policie.Description}</h2>" +
                                  $"{policie.Message}</br></br>" +
                                  $"Usuario: {(user.AccountName)  }</br>" +
                                  $"Descripción : {description}",
                                Addressed = users.Select(x => x.UserId.ToString()).ToList()
                            });

                            users.ForEach(x =>
                    {
                        notificationUsers.Add(new NotificationUser
                        {
                            UserId = x.UserId,
                            NotificationType =
                                new NotificationType { Type = NotificationTypes.AutorizationPolicies },
                            Parameters = new Dictionary<string, object>
                                            {
                                        {"key", request.Key},
                                        {"IdPolicies", policie.IdPolicies},
                                        {"FunctionId", request.FunctionType}
                                            },
                            Message =
                                $"Se ha generado la Politica - {policie.Description}, (Cargue {request.Key})"
                        });
                    });
                        });

                        DelegateService.UniqueUserService.CreateNotifications(notificationUsers.ToList());
                        this.SendEmails(emailsToSend.ToList());
                    }
                    catch (Exception)
                    {
                        //no hace nada
                    }
                    finally
                    {
                        DataFacadeManager.Dispose();
                    }
                });
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorCreateAutorizationRequest), e);
            }
        }

        private void CreateAuthorizationRequest(MPolicies.AuthorizationRequest authorization)
        {
            try
            {
                AuthorizationRequestDao authorizationRequestDao = new AuthorizationRequestDao();
                AuthorizationAnswerDao authorizationAnswerDao = new AuthorizationAnswerDao();
                MPolicies.AuthorizationRequest authorizationTmp = authorizationRequestDao.CreateAuthorizationRequest(authorization);
                foreach (MPolicies.AuthorizationAnswer answer in authorization.AuthorizationAnswers)
                {
                    answer.AuthorizationRequest = new MPolicies.AuthorizationRequest { AuthorizationRequestId = authorizationTmp.AuthorizationRequestId };
                    answer.DateAnswer = null;
                    authorizationAnswerDao.CreateAuthorizationAnswer(answer);
                }
            }
            catch (Exception e)
            {

                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorCreateAutorizationRequest), e);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }

        }


        /// <summary>
        /// Obtiene las solicitudes de autorizacion por la llave (key)
        /// </summary>
        /// <param name="key">llave de identificacion</param>
        /// <returns></returns>
        public List<MPolicies.AuthorizationRequest> GetAuthorizationRequestsByKey(string key)
        {
            try
            {
                AuthorizationRequestDao authorizationRequestDao = new AuthorizationRequestDao();
                List<MPolicies.AuthorizationRequest> auth = authorizationRequestDao.GetAuthorizationRequestsByKey(key);
                return auth;
            }
            catch (Exception e)
            {

                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetAuthorizationRequests), e);
            }
        }

        /// <summary>
        /// Obtiene las solicitudes de autorizacion por la llave (key)
        /// </summary>
        /// <param name="key">llave de identificacion</param>
        /// <param name="status">estado de la solicitud de autorizacion</param>
        /// <returns></returns>
        public List<MPolicies.AuthorizationRequest> GetAuthorizationRequestsByKeyStatus(string key, int status)
        {
            try
            {
                AuthorizationRequestDao authorizationRequestDao = new AuthorizationRequestDao();
                return authorizationRequestDao.GetAuthorizationRequestsByKeyStatus(key, status);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetAuthorizationRequests), e);
            }
        }

        /// <summary>
        /// Actualiza el Identificador del proceso, en los eventos asociados
        /// </summary>
        /// <param name="key">llave de identificacion 1</param>
        /// <param name="key2">llave de identificacion 2</param>
        /// <param name="processId">identificador del proceso</param>
        public void UpdateProcessIdByKeyKey2(Enums.TypeFunction typeFunction, string key, string key2, string processId)
        {
            try
            {
                AuthorizationRequestDao authorizationRequestDao = new AuthorizationRequestDao();
                authorizationRequestDao.UpdateProcessIdByKeyKey2(typeFunction, key, key2, processId);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorUpdateProcessId), e);
            }
        }

        /// <summary>
        /// Consulta las solicitudes de autorizacion por el usuario que solicita, fecha inicio/fin y los estados
        /// </summary>
        /// <param name="idUser">id usuario que solicita</param>
        /// <param name="status">lista de estados</param>
        /// <param name="dateInit">fecha inicial</param>
        /// <param name="dateEnd">fecha final</param>
        /// <returns></returns>
        public List<MPolicies.AuthorizationRequestGroup> GetAuthorizationRequestGroups(int idUser, List<int> status, DateTime dateInit, DateTime dateEnd)
        {
            try
            {
                return new AuthorizationRequestDao().GetAuthorizationRequestGroups(idUser, status, dateInit, dateEnd);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Resources.Errors.ErrorGetAuthorizationAnswes), e);
            }
        }


        /// <summary>
        /// Consulta las solicitudes de autorizacion pendientes 
        /// </summary>
        /// <param name="idUser">id usuario que solicita</param>
        /// <param name="status">lista de estados</param>
        /// <param name="dateInit">fecha inicial</param>
        /// <param name="dateEnd">fecha final</param>
        /// <returns></returns>
        public List<Models.AuthorizationRequestGroup> GetAuthorizationRequestPendingGroups(int groupPolicies, int policies, int idUser, int userAuthorization, DateTime dateInit, DateTime dateEnd)
        {
            try
            {
                return new AuthorizationRequestDao().GetAuthorizationRequestPendingGroups(groupPolicies, policies, idUser, userAuthorization, dateInit, dateEnd);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Resources.Errors.ErrorGetAuthorizationAnswes), e);
            }
        }

        public List<MPolicies.AuthorizationRequestGroup> GetDetailsAuthorizationRequestGroups(int idUser, string key, int policiesId)
        {
            try
            {
                return new AuthorizationRequestDao().GetDetailsAuthorizationRequestGroups(idUser, key, policiesId);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Resources.Errors.ErrorGetAuthorizationAnswes), e);
            }
        }
        #endregion

        #region AuthorizationAnswer

        ///  <summary>
        /// consulta las autorizacion de politicas segun el filtro
        ///  </summary>
        /// <param name="idGroup">id grupo de politica</param>
        /// <param name="idPolicies">id politica</param>
        /// <param name="idUser">Id del usuuario autorizador</param>
        /// <param name="status"> estado de la politica</param>
        /// <param name="dateInit">  fecha inicial</param>
        /// <param name="dateEnd"> fecha final</param>
        /// <param name="sort">  like nombre de la politica</param>
        public List<MPolicies.AuthorizationAnswerGroup> GetAuthorizationAnswersByFilter(int? idGroup, int? idPolicies, int idUser, int status, DateTime? dateInit, DateTime? dateEnd, string sort)
        {
            try
            {
                AuthorizationAnswerDao authorizationAnswerDao = new AuthorizationAnswerDao();
                return authorizationAnswerDao.GetAuthorizationAnswersByFilter(idGroup, idPolicies, idUser, status, dateInit, dateEnd, sort);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetAuthorizationAnswes), e);
            }
        }

        /// <summary>
        /// Actualiza Enabled =0
        /// </summary>
        /// <param name="key1">key1</param>
        /// <param name="key2">key2</param>
        public void UpdateAuthorizationAnswer(string key1, string key2)
        {
            try
            {
                AuthorizationAnswerDao authorizationAnswerDao = new AuthorizationAnswerDao();
                authorizationAnswerDao.UpdateAuthorizationAnswer(key1, key2);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetAuthorizationAnswes), e);
            }
        }

        public void UpdateAuthorizationAnswersByAuthorizationRequests(List<AuthorizationRequest> authorizationRequests, string userName)
        {
            try
            {
                AuthorizationAnswerDao authorizationAnswerDao = new AuthorizationAnswerDao();
                authorizationAnswerDao.UpdateAuthorizationAnswersByAuthorizationRequests(authorizationRequests, userName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAuthorizationAnswes), ex);
            }
        }

        ///  <summary>
        /// consulta las autorizacion de politicas que han sido reasignadas por el usuario
        ///  </summary>
        /// <param name="idGroup">id grupo de politica</param>
        /// <param name="idPolicies">id politica</param>
        /// <param name="idUser">Id del usuuario autorizador</param>
        /// <param name="dateInit">  fecha inicial</param>
        /// <param name="dateEnd"> fecha final</param>
        /// <param name="sort">  like nombre de la politica</param>
        public List<Models.AuthorizationAnswerGroup> GetAuthorizationAnswersReasignByFilter(int? idGroup, int? idPolicies, int idUser, DateTime? dateInit, DateTime? dateEnd, string sort)
        {
            try
            {
                AuthorizationAnswerDao authorizationAnswerDao = new AuthorizationAnswerDao();
                return authorizationAnswerDao.GetAuthorizationAnswersReasignByFilter(idGroup, idPolicies, idUser, dateInit, dateEnd, sort);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetAuthorizationAnswes), e);
            }
        }

        public List<string> GetAuthorizationAnswerDescriptions(int idPolicies, int idUser, int status, string key)
        {
            try
            {
                AuthorizationAnswerDao authorizationAnswerDao = new AuthorizationAnswerDao();
                return authorizationAnswerDao.GetAuthorizationAnswerDescriptions(idPolicies, idUser, status, key);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetAuthorizationAnswes), e);
            }
        }

        /// <summary>
        /// consultar las jerarquias superiores parametrizadas a la politica
        /// </summary>
        /// <param name="policiesId">id de la politica</param>
        /// <param name="hierarchyId">jerarquia del usuario actual</param>
        /// <param name="userId">id del usuario actual</param>
        /// <returns>lista de las jerarquias autorizadoras</returns>
        public List<CoHierarchyAssociation> GetAuthorizationHierarchy(int policiesId, int hierarchyId, int userId)
        {
            try
            {
                AuthorizationAnswerDao authorizationAnswerDao = new AuthorizationAnswerDao();
                return authorizationAnswerDao.GetAuthorizationHierarchy(policiesId, hierarchyId, userId);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetAuthorizationHierarchy), e);
            }
        }

        /// <summary>
        /// consultar los usuarios autorizadores de la politica en esa jerarquia
        /// </summary>
        /// <param name="autorizatioAnswerId">id de la autorizacion</param>
        /// <param name="hierarchyId">jerarquia autorizadora</param>
        /// <returns>lista usuarios autorizadores de la jerarquia</returns>
        public List<User> GetUsersAuthorizationHierarchy(int autorizatioAnswerId, int hierarchyId)
        {
            try
            {
                AuthorizationAnswerDao authorizationAnswerDao = new AuthorizationAnswerDao();
                return authorizationAnswerDao.GetUsersAuthorizationHierarchy(autorizatioAnswerId, hierarchyId);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetUsersAuthorizationHierarchy), e);
            }
        }

        /// <summary>
        /// Realiza la autorizacion de la politicas pendientes
        /// </summary>
        /// <param name="policiesId"></param>
        /// <param name="userAnswerId"></param>
        /// <param name="key"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public void AcceptAuthorization(int policiesId, int userAnswerId, string key, string reason, List<int> policiesToAccept, TypeFunction functionType)
        {
            try
            {
                EmailCriteria email = new EmailCriteria();

                AuthorizationAnswerDao authorizationAnswerDao = new AuthorizationAnswerDao();
                List<MPolicies.AuthorizationRequest> requests = authorizationAnswerDao.AcceptAuthorization(policiesId, userAnswerId, key, reason, ref email, policiesToAccept, functionType);
                this.SendEmails(new List<EmailCriteria> { email });

                if (requests.Any())
                {
                    TP.Task.Run(() =>
                    {
                        AuthorizationFunctionDao authorizationFunctionDao = new AuthorizationFunctionDao();
                        MPolicies.AuthorizationFunction function = authorizationFunctionDao.GetFunctionByIdFunction(requests.First().FunctionType);
                        this.ExecuteFuctions(requests, function);
                    });
                }
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorAcceptAuthorization), e);
            }
        }


        /// <summary>
        /// Realiza el rechazo de las autorizaciones, de forma automatica rechaza las pendientes
        /// actualiza le request de la autorizacion
        /// </summary>
        /// <param name="answers">lista de autorizaciones</param>
        public void RejectAuthorization(int policiesId, int userAnswerId, string key, string reason, int idRejection, List<int> policiesToReject, TypeFunction functionType)
        {
            try
            {
                EmailCriteria email = new EmailCriteria();

                AuthorizationAnswerDao authorizationAnswerDao = new AuthorizationAnswerDao();
                authorizationAnswerDao.RejectAuthorization(policiesId, userAnswerId, key, reason, idRejection, ref email, policiesToReject, functionType);
                this.SendEmails(new List<EmailCriteria> { email });
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorRejectAuthorization), e);
            }
        }

        public List<MPolicies.AuthorizationAnswer> GetAuthorizationAnswersByRequestId(int requestId)
        {
            try
            {
                return new AuthorizationAnswerDao().GetAuthorizationAnswersByRequestId(requestId);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Resources.Errors.ErrorGetAuthorizationAnswes), e);
            }
        }

        public List<string> GetAuthorizationAnswerDescription(int idPolicies, string key)
        {
            try
            {
                AuthorizationAnswerDao authorizationAnswerDao = new AuthorizationAnswerDao();
                return authorizationAnswerDao.GetAuthorizationAnswerDescriptions(idPolicies, key);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetAuthorizationAnswes), e);
            }
        }

        #endregion

        #region IssueWithPolicies
        public List<Models.IssueWithPolicies> GetIssueWithPolicies(int? temporalId, int userId)
        {
            try
            {
                AuthorizationRequestDao authorizationRequestDao = new AuthorizationRequestDao();
                return authorizationRequestDao.GetIssueWithPolicies(temporalId, userId);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Resources.Errors.ErrorGetGroupsPolicies), e);
            }
        }


        #endregion

        #region GroupPolicies

        /// <summary>
        /// obtiene los grupos de politicas
        /// </summary>
        /// <returns></returns>
        public List<MPolicies.GroupPolicies> GetGroupsPolicies()
        {
            try
            {
                GroupPoliciesDao groupPoliciesDao = new GroupPoliciesDao();
                return groupPoliciesDao.GetGroupsPolicies();
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetGroupsPolicies), e);
            }
        }

        /// <summary>
        /// Obtienes los usuarios autorizadores
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsersAuthorization(int groupId, int policiesId)
        {
            try
            {
                AuthorizationAnswerDao authorizationAnswerDao = new AuthorizationAnswerDao();
                return authorizationAnswerDao.GetUsersAuthorization(groupId, policiesId);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetGroupsPolicies), e);
            }
        }

        /// <summary>
        /// Elimina un grupo de Politicas
        /// </summary>
        /// <param name="groupPoliciesId"></param>
        public int DeleteGroupPolicies(int groupPoliciesId)
        {
            try
            {
                int delete;
                GroupPoliciesDao groupPoliciesDao = new GroupPoliciesDao();
                List<Models.PoliciesAut> GroupPolice = groupPoliciesDao.ValidGroupPolicies(groupPoliciesId);
                if (GroupPolice.Count == 0)
                {
                    groupPoliciesDao.DeleteGroupPolicies(groupPoliciesId);
                    delete = 1;
                    return delete;
                }
                delete = 0;
                return delete;
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorDeleteGroupPolicies), e);
            }
        }

        /// <summary>
        /// Crea un grupo de Politicas a partir del modelo
        /// </summary>
        /// <param name="GroupPolicies"></param>
        public void CreateGroupPolicies(MPolicies.GroupPolicies groupPolicy)
        {
            try
            {
                GroupPoliciesDao groupPoliciesDao = new GroupPoliciesDao();
                groupPoliciesDao.CreateGroupPolicies(groupPolicy);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorCreateGroupPolicies), e);
            }
        }

        /// <summary>
        /// Actualiza un grupo de Politicas a partir del modelo
        /// </summary>
        /// <param name="GroupPolicies"></param>
        public void UpdateGroupPolicies(MPolicies.GroupPolicies groupPolicy)
        {
            try
            {
                GroupPoliciesDao groupPoliciesDao = new GroupPoliciesDao();
                groupPoliciesDao.UpdateGroupPolicies(groupPolicy);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorUpdateGroupPolicies), e);
            }
        }

        /// <summary>
        /// Obtiene lista de riesgos por ramo
        /// </summary>
        /// <param name="groupPoliciesId"></param>
        public List<Models.CoveredRisk> GetCoveredRiskByPrefix(int Prefix)
        {
            try
            {
                GroupPoliciesDao groupPoliciesDao = new GroupPoliciesDao();
                return groupPoliciesDao.GetCoveredRiskType(Prefix);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorDeleteGroupPolicies), e);
            }
        }

        /// <summary>
        /// Obtiene lista de riesgos por ramo
        /// </summary>
        /// <param name="groupPoliciesId"></param>
        public List<Models.GroupPolicies> GetGroupPoliciesByDescription(string description, int module, int subModule, string prefix)
        {
            try
            {
                GroupPoliciesDao groupPoliciesDao = new GroupPoliciesDao();
                return groupPoliciesDao.GetGroupsPoliciesByDescription(description, module, subModule, prefix);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorDeleteGroupPolicies), e);
            }
        }

        #endregion

        #region Policies

        /// <summary>
        /// Realiza el filtro de las politicas
        /// </summary>
        /// <param name="groupPolicyId">Id grupo de politicas</param>
        /// <param name="typePolicyId">tipo de la politica</param>
        /// <param name="levelId">nivel de la politica</param>
        /// <param name="name">Nombre de la politica</param>
        /// <param name="message">mensaja de la politica</param>
        /// <param name="enable">si la politica esta habilitada</param>
        /// <returns></returns>
        public List<MPolicies.PoliciesAut> GetPoliciesByFilter(int? groupPolicyId, int? typePolicyId, int? levelId, string name, string message, bool enable)
        {
            try
            {
                PoliciesDao policiesDao = new PoliciesDao();
                return policiesDao.GetPoliciesByFilter(groupPolicyId, typePolicyId, levelId, name, message, enable);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetPolicies), e);
            }
        }

        /// <summary>
        /// obtiene las de politicas del grupo 
        /// </summary>
        /// <param name="idGroup">id del grupo de politicas</param>
        /// <returns></returns>
        public List<MPolicies.PoliciesAut> GetPoliciesByIdGroup(int idGroup)
        {
            try
            {
                PoliciesDao policiesDao = new PoliciesDao();
                return policiesDao.GetPoliciesByIdGroup(idGroup);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetPolicies), e);
            }
        }

        /// <summary>
        /// Obtiene las politicas con su respectiva regla segun el filtro
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <param name="idGroup">id del grupo</param>
        /// <param name="type">tipo de politica</param>
        /// <param name="position">posicion de la politica</param>
        /// <param name="filter">filtro tipo like</param>
        /// <returns></returns>
        public List<MPolicies.PoliciesAut> GetRulesPoliciesByFilter(int? idPackage, int idGroup, int? type, int? position, string filter)
        {
            try
            {
                PoliciesDao policiesDao = new PoliciesDao();
                return policiesDao.GetRulesPoliciesByFilter(idPackage, idGroup, type, position, filter);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetPolicies), e);
            }
        }


        /// <summary>
        /// Realiza la creacion de una politica con su respactiva regla
        /// </summary>
        /// <param name="policies">politica a crear</param>
        /// <param name="ruleSet">regla a crear</param>
        public void CreateRulePolicies(MPolicies.PoliciesAut policies, MRules._RuleSet ruleSet)
        {
            try
            {
                PoliciesDao policiesDao = new PoliciesDao();
                policiesDao.CreateRulePolicies(policies, ruleSet);

            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorCreateRulePolicies), e);
            }
        }

        /// <summary>
        /// Realiza el proceso de importar la regla de la politica
        /// </summary>
        /// <param name="policies">politica a importar</param>
        /// <returns>Politica importada</returns>
        public Models.PoliciesAut ImportRulePolicies(Models.PoliciesAut policies)
        {
            try
            {
                PoliciesDao policiesDao = new PoliciesDao();
                return policiesDao.ImportRulePolicies(policies);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, string.Empty), e);
            }
        }

        /// <summary>
        /// Realiza la modificacion de una politica con su respactiva regla
        /// </summary>
        /// <param name="policies">politica a modificar</param>
        /// <param name="ruleSet">regla a modificar</param>
        public void UpdateRulePolicies(MPolicies.PoliciesAut policies, MRules._RuleSet ruleSet)
        {
            try
            {
                PoliciesDao policiesDao = new PoliciesDao();
                policiesDao.UpdateRulePolicies(policies, ruleSet);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorUpdateRulePolicies), e);
            }
        }

        /// <summary>
        /// guarda la politica regla
        /// </summary>
        /// <param name="policies">politica a guardar</param>
        /// <param name="idHierarchy">id de la jerarquia</param>
        /// <param name="idHierarchyDt">id de la tabla de decision</param>
        /// <returns></returns>
        public void UpdateRulesPolicies(MPolicies.PoliciesAut policies, int? idHierarchyDt)
        {
            try
            {
                PoliciesDao policiesDao = new PoliciesDao();
                policiesDao.UpdateRulesPolicies(policies, idHierarchyDt);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorUpdateRulePolicies), e);
            }
        }

        /// <summary>
        /// Elimina una politica y su respectiva regla
        /// </summary>
        /// <param name="idPolicies"></param>
        public void DeleteRulePolicies(int idPolicies)
        {
            try
            {
                PoliciesDao policiesDao = new PoliciesDao();
                policiesDao.DeleteRulePolicies(idPolicies);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorDeletePolicyInUse), e);
            }
        }

        /// <summary>
        /// Elimina una politica y su respectiva regla
        /// </summary>
        /// <param name="idPolicies"></param>
        public string GenerateFileToPolicies(List<string> policiesList, string fileName)
        {
            try
            {
                PoliciesDao policiesDao = new PoliciesDao();
                return policiesDao.GenerateFileToPolicies(policiesList, fileName);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGenerateFileToPolicies), e);
            }
        }

        #endregion

        #region Tipo de politicas

        /// <summary>
        /// Obtierne los tipos de politicas 
        /// </summary>
        /// <returns></returns>
        public List<MPolicies.TypePolicies> GetTypePolicies()
        {
            try
            {
                PoliciesDao policiesDao = new PoliciesDao();
                return policiesDao.GetTypePolicies();

            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetTypePolicies), e);
            }
        }


        #endregion

        #region Nivel del grupo de politicas
        /// <summary>
        /// Obtiene los niveles asociados al grupo de politicas
        /// </summary>
        /// <returns></returns>
        public Dictionary<int[], string> GetLevelsByIdGroupPolicies(int idGroupPolicies, int? level)
        {
            try
            {
                PoliciesDao policiesDao = new PoliciesDao();
                return policiesDao.GetLevelsByIdGroupPolicies(idGroupPolicies, level);

            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetLevels), e);
            }
        }


        #endregion

        #region Package

        /// <summary>
        /// consulta los paquetes asociados a politicas
        /// </summary>
        /// <returns></returns>
        public List<MRules.Package> GetPackagePolicies()
        {
            try
            {
                ParametersDao parametersDao = new ParametersDao();
                return parametersDao.GetPackagePolicies();
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetPackagePolicies), e);
            }
        }

        #endregion

        #region Reasign

        /// <summary>
        /// Reasigna una politica
        /// </summary>
        /// <param name="policiesId"></param>
        /// <param name="userAnswerId"></param>
        /// <param name="key"></param>
        /// <param name="hierarchyId"></param>
        /// <param name="userReasignId"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public void ReasignAuthorizationAnswer(int policiesId, int userAnswerId, string key, int hierarchyId, int userReasignId, string reason, List<int> policiesToReassign, int userReassigning, TypeFunction functionType)
        {
            try
            {
                ReasignDao reasignDao = new ReasignDao();
                reasignDao.ReasignAuthorizationAnswer(policiesId, userAnswerId, key, hierarchyId, userReasignId, reason, policiesToReassign, userReassigning, functionType);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorReasignAuthorizationAnswer), e);
            }
        }

        /// <summary>
        /// Obtiene el historias de las autorizaciones reasignadas
        /// </summary>
        /// <param name="policiesId"></param>
        /// <param name="userAnswerId"></param>
        /// <param name="key"></param>
        /// <param name="userId">usuario de la consulta</param>
        /// <returns></returns>
        public List<MPolicies.Reasign> GetHistoryReasign(int policiesId, int userAnswerId, string key)
        {
            try
            {
                ReasignDao reasignDao = new ReasignDao();
                return reasignDao.GetHistoryReasign(policiesId, userAnswerId, key);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetHistoryReasign), e);
            }
        }

        /// <summary>
        /// Obtiene las jerarquias para la pantalla de reasignacion
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<CoHierarchyAssociation> GetHierarchyByGroupPolicies(int groupId)
        {
            try
            {
                ReasignDao reasignDao = new ReasignDao();
                return reasignDao.GetHierarchyByGroupPolicies(groupId);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetHistoryReasign), e);
            }
        }

        #endregion

        #region Concepts Description  

        /// <summary>
        /// Obtiene los conceptos asignados a la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <returns></returns>
        public List<MPolicies.ConceptDescription> GetConceptDescriptionsByIdPolicies(int idPolicies)
        {
            try
            {
                ConceptDescriptionDao conceptDescriptionDao = new ConceptDescriptionDao();
                return conceptDescriptionDao.GetConceptDescriptionsByIdPolicies(idPolicies);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetConceptDescriptionsByIdPolicies), e);
            }
        }

        /// <summary>
        /// Guarda los conceptos asociados a la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="conceptDescriptions">lista de conceptos</param>
        /// <returns></returns>
        public void SaveConceptDescriptions(int idPolicies, List<MPolicies.ConceptDescription> conceptDescriptions)
        {
            try
            {
                ConceptDescriptionDao conceptDescriptionDao = new ConceptDescriptionDao();
                conceptDescriptionDao.SaveConceptDescriptions(idPolicies, conceptDescriptions);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorSaveConceptDescriptionsByIdPolicies), e);
            }
        }

        #endregion

        #region Emails
        /// <summary>
        /// Realiza el armado de los correos y los envia a los respectivos usuarios
        /// </summary>
        /// <param name="emails">lista de los emails</param>
        private async void SendEmails(List<EmailCriteria> emails)
        {
            try
            {
                Dictionary<int, string> usersEmail = new Dictionary<int, string>();

                for (int i = 0; i < emails.Count; i++)
                {
                    EmailCriteria email = emails[i];
                    for (int j = 0; j < email.Addressed.Count; j++)
                    {
                        string user = email.Addressed[j];
                        int idUser = Convert.ToInt32(user);

                        if (usersEmail.ContainsKey(idUser))
                        {
                            emails[i].Addressed[j] = usersEmail[idUser];
                        }
                        else
                        {
                            var person = DelegateService.UniqueUserService.GetPersonByUserId(idUser);
                            if (person.Emails.Any())
                            {
                                string strAddress = person.Emails[0].Description;
                                emails[i].Addressed[j] = strAddress;
                                usersEmail.Add(idUser, strAddress);
                            }
                            else
                            {
                                emails[i].Addressed[j] = string.Empty;
                            }
                        }
                    }
                }
                emails.ForEach(e => e.Addressed = e.Addressed.Where(a => !string.IsNullOrEmpty(a)).ToList());
                await DelegateService.utilitiesServiceCore.SendEmailsAsync(emails);
            }
            catch
            {
                // ignored
                //throw new BusinessException("Error en ExecuteFuctions", e);
            }
        }

        /// <summary>
        /// Método que envía correos. Se crea para poderlo llamar desde el Launcher.
        /// </summary>
        /// <param name="objEmailCriteria"></param>
        public async void SendEmail(EmailCriteria objEmailCriteria)
        {
            await DelegateService.utilitiesServiceCore.SendEmailAsync(objEmailCriteria);
        }

        #endregion

        #region Ejecucion de las funciones

        private void ExecuteFuctions(List<MPolicies.AuthorizationRequest> requests, MPolicies.AuthorizationFunction function)
        {
            try
            {

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                string urlApi = DelegateService.CommonService.GetKeyApplication("UrlApi");
                string apiPath = DelegateService.CommonService.GetKeyApplication("ApiPath") ?? string.Empty;

                parameters.Clear();

                switch (requests.First().FunctionType)
                {
                    case TypeFunction.Individual:
                        parameters.Add("temporalId", requests.First().Key);
                        DelegateService.CommonService.GetAsyncHelper(urlApi, $"{apiPath}/{function.Type}/{function.Method}", parameters);
                        break;
                    case TypeFunction.Massive:
                        IEnumerable<string> listTemporal = requests.Select(x => x.Key2);
                        string temporals = JsonConvert.SerializeObject(listTemporal);
                        DelegateService.CommonService.PostAsyncHelper(urlApi, $"{apiPath}/{function.Type}/{function.Method}/{requests.First().Key}", temporals);
                        break;
                    case TypeFunction.Collective:
                        string loadId = requests.First().Key;
                        string temporalId = requests.First().Key2.Split('|')[0];

                        IEnumerable<int> listRisks = requests.Where(x => x.Key2.Contains("|"))?.Select(x => int.Parse(x.Key2.Split('|')[1]));
                        string risks = JsonConvert.SerializeObject(listRisks);
                        DelegateService.CommonService.PostAsyncHelper(urlApi, $"{apiPath}/{function.Type}/{function.Method}/{loadId}/{temporalId}", risks);
                        break;
                    case TypeFunction.Claim:
                        parameters.Add("claimTemporalId", requests.First().Key);
                        DelegateService.CommonService.GetAsyncHelper(urlApi, $"{apiPath}/{function.Type}/{function.Method}", parameters);
                        break;
                    case TypeFunction.PaymentRequest:
                        parameters.Add("paymentRequestTemporalId", requests.First().Key);
                        DelegateService.CommonService.GetAsyncHelper(urlApi, $"{apiPath}/{function.Type}/{function.Method}", parameters);
                        break;
                    case TypeFunction.ChargeRequest:
                        parameters.Add("chargeRequestTemporalId", requests.First().Key);
                        DelegateService.CommonService.GetAsyncHelper(urlApi, $"{apiPath}/{function.Type}/{function.Method}", parameters);
                        break;
                    case TypeFunction.ClaimNotice:
                        parameters.Add("claimNoticeTemporalId", requests.First().Key);
                        DelegateService.CommonService.GetAsyncHelper(urlApi, $"{apiPath}/{function.Type}/{function.Method}", parameters);
                        break;
                    case TypeFunction.PersonGeneral:
                    case TypeFunction.PersonInsured:
                    case TypeFunction.PersonProvider:
                    case TypeFunction.PersonThird:
                    case TypeFunction.PersonIntermediary:
                    case TypeFunction.PersonEmployed:
                    case TypeFunction.PersonPersonalInf:
                    case TypeFunction.PersonPaymentMethods:
                    case TypeFunction.PersonGuarantees:
                    case TypeFunction.PersonOperatingQuota:
                    case TypeFunction.PersonTaxes:
                    case TypeFunction.PersonBankTransfers:
                    case TypeFunction.PersonReinsurer:
                    case TypeFunction.PersonCoinsurer:
                    case TypeFunction.PersonConsortiates:
                    case TypeFunction.PersonBusinessName:
                    case TypeFunction.PersonBasicInfo:
                        parameters.Add("operationId", requests.First().Key);
                        DelegateService.CommonService.GetAsyncHelper(urlApi, $"{apiPath}/{function.Type}/{function.Method}", parameters);
                        break;

                    case TypeFunction.SarlaftGeneral:
                        parameters.Add("operationId", requests.First().Key);
                        DelegateService.CommonService.GetAsyncHelper(urlApi, $"{apiPath}/{function.Type}/{function.Method}", parameters);
                        break;
                    case TypeFunction.AutomaticQuota:
                        parameters.Add("id", requests.First().Key);
                        DelegateService.CommonService.GetAsyncHelper(urlApi, $"{apiPath}/{function.Type}/{function.Method}", parameters);
                        break;
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format(" Error en ExecuteFuctions: {0}", ex.Message));

            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        public List<MPolicies.PoliciesAut> ValidateInfringementPolicies(List<MPolicies.PoliciesAut> infringementPolicies)
        {
            try
            {
                PoliciesDao policiesDao = new PoliciesDao();
                return policiesDao.ValidateInfringementPolicies(infringementPolicies);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region notificaciones
        /// <summary>
        /// Metodo que elimina la informacion de notificaciones de una temporal
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteNotificationByTemporalId(int id, int functionId)
        {
            try
            {
                PoliciesDao policiesDao = new PoliciesDao();
                return policiesDao.DeleteNotificationByTemporalId(id, functionId);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGetUsersAutorization), e);
            }

        }
        #endregion


    }
}