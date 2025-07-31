using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public class TextHelper
    {
        public static string replaceAccentMarks(string text)
        {
            text = text.Replace("á", "a");
            text = text.Replace("é", "e");
            text = text.Replace("í", "i");
            text = text.Replace("ó", "o");
            text = text.Replace("ú", "u");
            return text;
        }
    }
}