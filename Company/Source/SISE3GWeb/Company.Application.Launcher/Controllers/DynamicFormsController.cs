using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sistran.Core.Framework.UIF.Web.Controllers
{
    [Authorize]
    public class DynamicFormsController : ApiController
    {
        [HttpGet, HttpPost, HttpPut, HttpDelete]
        public async Task<IHttpActionResult> Execute(string url)
        {
            string endPoint = ConfigurationManager.AppSettings["formulariosdinamicos"];
            if (string.IsNullOrWhiteSpace(endPoint))
            {
                return BadRequest("No CRUD service endpoint configured.");   // Also sets the task state to "RanToCompletion"                
            }
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(endPoint);

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(Request.Method, new Uri(endPoint));
            if (Request.Method != HttpMethod.Get)
            {
                httpRequestMessage.Content = Request.Content;
            }

            var response = await httpClient.SendAsync(httpRequestMessage);

            var responseContent = await response.Content.ReadAsAsync<object>();

            return Ok(responseContent);
        }
    }
}
