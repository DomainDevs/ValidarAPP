using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using TypePolicies = Sistran.Core.Application.AuthorizationPoliciesServices.Enums.TypePolicies;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Models
{
    public class AuthorizationRequestModelView
    {
        public int IdAuthorizationRequest { get; set; }

        [Required]
        public PoliciesAutModelView Policies { get; set; }

        [Required]
        public string Key { get; set; }

        public string Key2 { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public int NumberAut { get; set; }

        [Required]
        public int IdUserRequest { get; set; }

        [Required]
        public int IdHierarchyRequest { get; set; }

        [Required]
        public string DescriptionRequest { get; set; }

        [Required]
        public DateTime DateRequest { get; set; }

        public List<AuthorizationAnswerModelView> AuthorizationAnswers { get; set; }

        public List<int> NotificationUsers { set; get; }

        public int FunctionType { get; set; }



        #region Assemblers

        public static List<AuthorizationRequest> CreateListModel(List<AuthorizationRequestModelView> authorizationRequests)
        {
            var list = new List<AuthorizationRequest>();
            foreach (var request in authorizationRequests)
            {
                list.Add(CreateModel(request));
            }

            return list;
        }

        private static AuthorizationRequest CreateModel(AuthorizationRequestModelView request)
        {
            return new AuthorizationRequest
            {
                AuthorizationRequestId = request.IdAuthorizationRequest,
                Key = request.Key,
                Key2 = request.Key2 ?? "",
                Description = request.Description,
                Status = (TypeStatus)request.Status,
                NumberAut = request.NumberAut,
                UserRequest = new User { UserId = request.IdUserRequest },
                HierarchyRequest = new CoHierarchyAssociation { Id = request.IdHierarchyRequest },
                DescriptionRequest = request.DescriptionRequest,
                DateRequest = request.DateRequest,
                AuthorizationAnswers = AuthorizationAnswerModelView.CreateListModel(request.AuthorizationAnswers),
                NotificationUsers = request.NotificationUsers?.Select(x => new User { UserId = x }).ToList() ?? new List<User>(),
                Policies = new PoliciesAut
                {
                    IdPolicies = request.Policies.IdPolicies,
                    Description = request.Policies.Description,
                    Message = request.Policies.Message,
                    Type = TypePolicies.Notification
                },
                FunctionType = (TypeFunction)request.FunctionType
            };
        }

        #endregion
    }
}