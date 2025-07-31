using PersonService;
using PersonServiceEEProvider;
using System;
using System.Web.Http;

namespace APIPersonService.Controllers
{
    public class PersonController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetPerson2gByDocumentNumber(string documentNumber, bool company)
        {
            IPersonService personServices = null;
            try
            {
                personServices = PersonOperations.GetPersonOperations();
                return Ok(personServices.GetPerson2gByDocumentNumber(documentNumber, company));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpGet]
        public IHttpActionResult GetPerson2gByPersonId(int personId, bool company)
        {
            IPersonService personServices = null;
            try
            {
                personServices = PersonOperations.GetPersonOperations();
                return Ok(personServices.GetPerson2gByPersonId(personId, company));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetCompany2gByCompanyId(int companyId, bool company)
        {
            IPersonService personServices = null;
            try
            {
                personServices = PersonOperations.GetPersonOperations();
                return Ok(personServices.GetCompany2gByCompanyId(companyId, company));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public IHttpActionResult GetProvider2g(int personId)
        {
            IPersonService personServices = null;
            try
            {
                personServices = PersonOperations.GetPersonOperations();
                return Ok(personServices.GetProvider2g(personId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public IHttpActionResult GetBankTransfer2g(int transferPersonId)
        {
            IPersonService personServices = null;
            try
            {
                personServices = PersonOperations.GetPersonOperations();
                return Ok(personServices.GetBankTransfer2g(transferPersonId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public IHttpActionResult GetTax2g(int taxPersonId)
        {
            IPersonService personServices = null;
            try
            {
                personServices = PersonOperations.GetPersonOperations();
                return Ok(personServices.GetTax2g(taxPersonId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
