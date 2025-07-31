using System;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Managers
{
    public class HttpContextManager
    {
        private static HttpContextBase m_context;

        public static HttpContextBase Current
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }
                else
                {
                    return new HttpContextWrapper(HttpContext.Current);
                }
            }
        }

        public static void SetCurrentContext(HttpContextBase context)
        {
            m_context = context;
        }
    }
}