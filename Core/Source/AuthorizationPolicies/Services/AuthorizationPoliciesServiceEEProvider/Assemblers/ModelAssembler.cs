using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using System;
using System.Collections.Generic;
using APEntity = Sistran.Core.Application.AuthorizationPolicies.Entities;
using MRules = Sistran.Core.Application.RulesScriptsServices.Models;
//using modelsUtilities = Sistran.Core.Application.UtilitiesServices.Models;
using MUser = Sistran.Core.Application.UniqueUserServices.Models;


namespace Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.Assemblers
{
    public static class ModelAssembler
    {
        #region PoliciesAut

        public static List<Models.PoliciesAut> CreateListPolicies(List<APEntity.Policies> policies)
        {
            var models = new List<Models.PoliciesAut>();
            foreach (var policy in policies)
            {
                models.Add(CreatePolicies(policy));
            }
            return models;
        }

        public static Models.PoliciesAut CreatePolicies(APEntity.Policies policy)
        {
            return new Models.PoliciesAut
            {
                IdPolicies = policy.PoliciesId,
                IdHierarchyPolicy = policy.HierarchyPoliciesId,
                GroupPolicies = new Models.GroupPolicies { IdGroupPolicies = policy.GroupPoliciesId },
                Type = (Enums.TypePolicies)policy.TypePoliciesId,
                Description = policy.Description,
                Position = policy.Position,
                NumberAut = policy.NumberAut,
                Message = policy.Message,
                Enabled = policy.Enabled
            };
        }


        #endregion

        #region GroupPolicies

        public static List<Models.GroupPolicies> CreateListGroupPolicies(List<APEntity.GroupPolicies> groupPolicies)
        {
            var result = new List<Models.GroupPolicies>();
            foreach (var group in groupPolicies)
            {
                result.Add(CreateGroupPolicies(group));
            }

            return result;
        }

        public static Models.GroupPolicies CreateGroupPolicies(APEntity.GroupPolicies groupPolicies)
        {
            return new Models.GroupPolicies
            {
                IdGroupPolicies = groupPolicies.GroupPoliciesId,
                Module = new MUser.Module { Id = groupPolicies.ModuleId },
                SubModule = new MUser.SubModule { Id = groupPolicies.SubmoduleId },
                Description = groupPolicies.Description,
                Package = new MRules._Package { PackageId = groupPolicies.PackageId },
                Key = groupPolicies.Key
            };
        }

        #endregion

        #region ConceptDescription

        public static List<Models.ConceptDescription> CreateListConceptDescription(
            List<APEntity.ConceptDescription> concepts)
        {
            var conceptDescriptionsList = new List<Models.ConceptDescription>();

            foreach (var concept in concepts)
            {
                conceptDescriptionsList.Add(CreateConceptDescription(concept));
            }

            return conceptDescriptionsList;
        }

        public static Models.ConceptDescription CreateConceptDescription(APEntity.ConceptDescription concepts)
        {
            return new Models.ConceptDescription
            {
                Concept = new MRules._Concept
                {
                    ConceptId = concepts.ConceptId,
                    Entity = new MRules.Entity { EntityId = concepts.EntityId }
                },
                Order = concepts.Order
            };
        }

        #endregion

        #region AuthorizationRequest

        public static List<Models.AuthorizationRequest> CreateListAuthorizationRequest(
            List<APEntity.AuthorizationRequest> authorizationRequests)
        {
            var result = new List<Models.AuthorizationRequest>();

            foreach (var authorizationRequest in authorizationRequests)
            {
                result.Add(CreateAuthorizationRequest(authorizationRequest));
            }
            return result;
        }

        public static Models.AuthorizationRequest CreateAuthorizationRequest(
            APEntity.AuthorizationRequest authorizationRequest)
        {
            return new Models.AuthorizationRequest
            {
                AuthorizationRequestId = authorizationRequest.AuthorizationRequestId,
                Policies = new Models.PoliciesAut
                {
                    IdPolicies = authorizationRequest.PoliciesId,
                    Type = Enums.TypePolicies.Notification
                },
                Key = authorizationRequest.Key,
                Key2 = authorizationRequest.Key2,
                Description = authorizationRequest.Description,
                Status = (Enums.TypeStatus)authorizationRequest.StatusId,
                NumberAut = authorizationRequest.NumberAut,
                HierarchyRequest = new MUser.CoHierarchyAssociation { Id = authorizationRequest.HierarchyRequestId },
                DateRequest = authorizationRequest.DateRequest,
                AuthorizationAnswers = new List<Models.AuthorizationAnswer>(),
                UserRequest = new MUser.User { UserId = authorizationRequest.UserRequestId },
                DescriptionRequest = authorizationRequest.DescriptionRequest,
                NotificationUsers = new List<MUser.User>(),
                FunctionType = (TypeFunction)Enum.Parse(typeof(TypeFunction), authorizationRequest.FunctionId.ToString())
            };
        }


        #endregion

        #region AuthorizationAnswer

        public static List<Models.AuthorizationAnswer> CreateListAuthorizationAnswer(
            List<APEntity.AuthorizationAnswer> authorizationAnswer)
        {
            var result = new List<Models.AuthorizationAnswer>();

            foreach (var answer in authorizationAnswer)
            {
                result.Add(CreateAuthorizationAnswer(answer));
            }

            return result;
        }

        public static Models.AuthorizationAnswer CreateAuthorizationAnswer(
            APEntity.AuthorizationAnswer authorizationAnswer)
        {
            return new Models.AuthorizationAnswer
            {
                AuthorizationRequest = new Models.AuthorizationRequest
                {
                    AuthorizationRequestId = authorizationAnswer.AuthorizationRequestId,
                    Status = Enums.TypeStatus.Pending, 
                    FunctionType = Enums.TypeFunction.Individual
                },
                AuthorizationAnswerId = authorizationAnswer.AuthorizationAnswerId,
                Status = (Enums.TypeStatus)authorizationAnswer.StatusId,
                UserAnswer = new MUser.User { UserId = authorizationAnswer.UserAnswerId },
                DateAnswer = authorizationAnswer.DateAnswer,
                HierarchyAnswer = new MUser.CoHierarchyAssociation { Id = authorizationAnswer.HierarchyAnswerId },
                DescriptionAnswer = authorizationAnswer.DescriptionAnswer,
                Enabled = authorizationAnswer.Enabled,
                Required = authorizationAnswer.Required,
                RejectionCausesId = authorizationAnswer.RejectionCausesId
            };
        }

        #endregion

        #region TypePolicies

        public static List<Models.TypePolicies> CreateListTypePolicies(List<APEntity.TypePolicies> typePolicies)
        {
            var result = new List<Models.TypePolicies>();
            foreach (var type in typePolicies)
            {
                result.Add(CreateTypePolicies(type));
            }
            return result;
        }

        public static Models.TypePolicies CreateTypePolicies(APEntity.TypePolicies typePolicies)
        {
            return new Models.TypePolicies
            {
                Description = typePolicies.Description,
                IdTypePolicies = typePolicies.TypePoliciesId
            };
        }

        #endregion


    }
}
