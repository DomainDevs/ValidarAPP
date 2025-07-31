using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Models
{
    public class AuthorizationAnswerModelView
    {
        public int IdAuthorizationAnswer { get; set; }

        public AuthorizationRequestModelView AuthorizationRequest { get; set; }

        [Required]
        public int IdUserAnswer { get; set; }

        [Required]
        public int IdHierarchyAnswer { get; set; }

        public string DescriptionAnswer { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public bool Required { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public DateTime DateAnswer { get; set; }

        #region Assemblers
        public static List<AuthorizationAnswer> CreateListModel(List<AuthorizationAnswerModelView> authorizationAnswers)
        {
            var list = new List<AuthorizationAnswer>();

            foreach (var answer in authorizationAnswers)
            {
                list.Add(CreateModel(answer));
            }

            return list;
        }

        private static AuthorizationAnswer CreateModel(AuthorizationAnswerModelView answer)
        {
            return new AuthorizationAnswer
            {
                AuthorizationAnswerId = answer.IdAuthorizationAnswer,
                AuthorizationRequest = answer.AuthorizationRequest != null ? new AuthorizationRequest { AuthorizationRequestId = answer.AuthorizationRequest.IdAuthorizationRequest } : null,
                Status = (TypeStatus)answer.Status,
                UserAnswer = new User { UserId = answer.IdUserAnswer },
                HierarchyAnswer = new CoHierarchyAssociation { Id = answer.IdHierarchyAnswer },
                DescriptionAnswer = answer.DescriptionAnswer,
                Required = answer.Required,
                Enabled = answer.Enabled,
                DateAnswer = answer.DateAnswer
            };
        }

        #endregion
    }
}