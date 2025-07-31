
namespace Sistran.Company.Application.WrapperAuthorizationPoliciesServiceEEProvider.Extentions
{
    public static class ObjectExtentions
    {
        public static string GetJson(this object o)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(o);
        }
    }
}
