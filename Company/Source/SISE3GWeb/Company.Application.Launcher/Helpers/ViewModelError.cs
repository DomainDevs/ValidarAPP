using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public class ViewModelError
    {
        public static string GetMessages(ICollection<ModelState> values)
        {
            StringBuilder sb = new StringBuilder();

            foreach (ModelState item in values.Where(x => x.Errors.Count > 0))
            {
                sb.Append(item.Errors[0].ErrorMessage).Append(", ");
            }

            return sb.ToString().Remove(sb.ToString().Length - 2);
        }
    }
}