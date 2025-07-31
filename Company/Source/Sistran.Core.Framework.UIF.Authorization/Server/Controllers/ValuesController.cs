using Sistran.Core.Framework.UIF.Authorization.Server.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sistran.Core.Framework.UIF.Authorization.Server.Controllers
{
    [SistranAuthorizationFilter]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "Respuesta del Back" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "Respuesta con parametro";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {

        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
