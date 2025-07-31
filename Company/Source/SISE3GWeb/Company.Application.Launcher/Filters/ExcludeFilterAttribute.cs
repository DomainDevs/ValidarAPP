using System;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Filters
{
    public class ExcludeFilterAttribute : FilterAttribute
    {
        private Type filterType;

        public ExcludeFilterAttribute(Type filterType)
        {
            this.filterType = filterType;
        }

        public Type FilterType
        {
            get
            {
                return this.filterType;
            }
        }
    }
}