namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Models
{
    public class ConceptDescriptionModelView
    {
        public int IdConcept { set; get; }

        public int IdEntity { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public string Value { set; get; }

        public int Order { set; get; }
    }
}