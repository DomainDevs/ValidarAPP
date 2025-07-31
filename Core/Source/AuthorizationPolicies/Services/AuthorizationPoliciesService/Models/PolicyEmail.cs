using Sistran.Core.Application.Extensions;
namespace Sistran.Core.Application.AuthorizationPoliciesServices.Models
{
    public class PolicyEmail : Extension
    {
        public int PolicyId { get; set; }
        public string Keys { get; set; }
        public int UserId { get; set; }
    }
}
