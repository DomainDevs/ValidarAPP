using Sistran.Core.Application.ModelServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public class PostEntity
    {
        public string EntityType { get; set; }

        private CRUD.ServiceClient.CRUDServiceClient _CRUDCliente = new CRUD.ServiceClient.CRUDServiceClient(System.Configuration.ConfigurationManager.AppSettings["crudservices"]);

        public CRUD.ServiceClient.CRUDServiceClient CRUDCliente
        {
            get { return _CRUDCliente; }
            set { _CRUDCliente = value; }
        }


        public Dictionary<string, string> Fields { get; set; }

        public StatusTypeService? Status { get; set; }
    }
}
