using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public static class ExceptionHelper
    {
        public static string GetMessage(string message)
        {
            string[] messages = message.Split('|');
            return messages[0];
        }
    }
}