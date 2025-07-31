using System;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Grid
{
     [ModelBinder(typeof(GridModelBinder))]
    public class GridSettings
    {
        public bool IsSearch { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }

        public Filter Where { get; set; }


        /// <summary>
        /// IsDeclared
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool IsDeclared(string field)
        {

            if (Where != null)
            {
                foreach (Rule rule in Where.rules)
                {
                    if (rule.field == field)
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        /// <summary>
        /// GetFieldValue
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string GetFieldValue(string field)
        {
            if (Where != null)
            {
                foreach (Rule rule in Where.rules)
                {
                    if (rule.field == field)
                    {
                        return rule.data;
                    }

                }
            }

            return String.Empty;
        }
    }
}