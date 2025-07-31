using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public static class EnumerableHelper
    {
        public static bool AnyOrDefault<T>(this IEnumerable<T> output,Func<T,bool> fun)
        {
            return output?.Any(fun) ?? false;
        }
    }
}