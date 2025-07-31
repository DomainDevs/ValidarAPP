using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public class EnumsHelper
    {
        public static SelectList GetItems<T>()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            SelectListItem newItem = null;
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                newItem = new SelectListItem() { Value = ((int)item).ToString(), Text = (string)HttpContext.GetGlobalResourceObject("Language", item.ToString()) };
                list.Add(newItem);
            }
            return new SelectList(list, "Value", "Text");
        }

        public static string GetItemName<T>(object value)
        {
            string ItemName = Enum.GetName(typeof(T), value);
            ItemName = (string)HttpContext.GetGlobalResourceObject("Language", ItemName.ToString());
            return ItemName;
        }
    }
}