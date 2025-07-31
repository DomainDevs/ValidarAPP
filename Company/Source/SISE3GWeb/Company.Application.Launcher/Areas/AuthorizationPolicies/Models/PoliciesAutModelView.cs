using System.Collections.Generic;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Models
{
    public class PoliciesAutModelView
    {
        public int IdPolicies { set; get; }
        public GroupPoliciesModelView GroupPolicies { set; get; }
        public TypePolicies Type { set; get; }
        public string Description { set; get; }
        public int Position { set; get; }
        public int IdHierarchyPolicy { set; get; }
        public int IdHierarchyAut { set; get; }
        public int NumbreAut { set; get; }
        public string Message { set; get; }
        public List<ConceptDescriptionModelView> ConceptsDescription { set; get; }
    }
}