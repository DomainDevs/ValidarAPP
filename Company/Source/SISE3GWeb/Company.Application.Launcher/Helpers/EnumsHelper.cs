using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    /// <summary>
    /// Enums
    /// </summary>
    public class EnumsHelper
    {
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static SelectList GetItems<T>()
        {
            try
            {
                if (typeof(T).IsEnum)
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
                else
                {
                    throw new Exception(App_GlobalResources.Language.ObjectNotEnum);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetItemName<T>(object value)
        {
            string ItemName = Enum.GetName(typeof(T), value);
            if (ItemName != null)
            {
                ItemName = (string)HttpContext.GetGlobalResourceObject("Language", ItemName.ToString());
            }
            return ItemName;
        }

        public static string GetName(string name)
        {
            string ItemName;
            ItemName = (string)HttpContext.GetGlobalResourceObject("Language", name.Trim());
            if (string.IsNullOrEmpty(ItemName))
                ItemName = name;
            return ItemName;
        }

    }
}