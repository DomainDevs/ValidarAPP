using APEntity = Sistran.Core.Application.AuthorizationPolicies.Entities;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.Assemblers
{
    public static class EntityAssembler
    {
        #region AuthorizationRequest
        public static APEntity.AuthorizationRequest CreateAuthorizationRequest(Models.AuthorizationRequest authorization)
        {
            return new APEntity.AuthorizationRequest
            {
                PoliciesId = authorization.Policies.IdPolicies,
                Key = authorization.Key,
                Key2 = authorization.Key2,
                Description = authorization.Description,
                StatusId = (int)authorization.Status,
                NumberAut = authorization.NumberAut,
                UserRequestId = authorization.UserRequest.UserId,
                HierarchyRequestId = authorization.HierarchyRequest.Id,
                DescriptionRequest = authorization.DescriptionRequest,
                DateRequest = authorization.DateRequest,
                FunctionId = (int)authorization.FunctionType
            };
        }
        #endregion

        #region AuthorizationAnswer
        public static APEntity.AuthorizationAnswer CreateAuthorizationAnswer(Models.AuthorizationAnswer answer)
        {
            return new APEntity.AuthorizationAnswer
            {
                AuthorizationRequestId = answer.AuthorizationRequest.AuthorizationRequestId,
                StatusId = (int)answer.Status,
                UserAnswerId = answer.UserAnswer.UserId,
                HierarchyAnswerId = answer.HierarchyAnswer.Id,
                DescriptionAnswer = answer.DescriptionAnswer,
                Required = answer.Required,
                Enabled = answer.Enabled,
                DateAnswer = answer.DateAnswer,
                RejectionCausesId = answer.RejectionCausesId
            };
        }
        #endregion


        #region Reasign
        public static APEntity.Reasign CreateReasign(Models.Reasign reasign)
        {
            return new APEntity.Reasign
            {
                AuthorizationAnswerId = reasign.AuthorizationAnswer.AuthorizationAnswerId,
                UserAnswerId = reasign.UserAnswer.UserId,
                HierarchyAnswerId = reasign.HierarchyAnswer.Id,
                UserReasignId = reasign.UserReasign.UserId,
                HierarchyReasignId = reasign.HierarchyReasign.Id,
                DescriptionReasign = reasign.DescriptionReasign,
                DateReasign = reasign.DateReasign
            };
        }
        #endregion

        #region User Autohorization
        public static APEntity.UserAuthorization CreateUserAuthorization(Models.UserAuthorization userAuthorization)
        {
            return new APEntity.UserAuthorization(userAuthorization.Policies.IdPolicies, userAuthorization.User.UserId, userAuthorization.Hierarchy.Id)
            {
                Default = userAuthorization.Default,
                Required = userAuthorization.Required
            };
        }
        #endregion

        #region User Notification
        public static APEntity.UserNotification CreateUserNotification(Models.UserNotification userNotification)
        {
            return new APEntity.UserNotification(userNotification.Policies.IdPolicies, userNotification.User.UserId, userNotification.Hierarchy.Id)
            {
                Default = userNotification.Default
            };
        }
        #endregion

        #region ConceptDescription
        public static APEntity.ConceptDescription CreateConceptDescription(Models.ConceptDescription conceptDescription)
        {
            return new APEntity.ConceptDescription(conceptDescription.Policies.IdPolicies, conceptDescription.Concept.ConceptId, conceptDescription.Concept.Entity.EntityId)
            {
                Order = conceptDescription.Order
            };
        }



        #endregion
    }
}
