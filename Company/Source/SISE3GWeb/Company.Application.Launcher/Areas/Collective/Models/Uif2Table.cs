using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Collective.Models
{
    public class Uif2Table : JsonResult
    {
        public Uif2Table(object data)
        {          
            this.Data = new { aaData = data };
            this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            this.MaxJsonLength = int.MaxValue;           
        }
    }

}