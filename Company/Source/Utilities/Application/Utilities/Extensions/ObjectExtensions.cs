using System;

namespace Sistran.Company.Application.Utilities.Extensions
{
    public static class ObjectExtensions
    {
        public static string GetString(this object source)
        {
            if (source != DBNull.Value && source != null)
            {
                return source.ToString();
            }
            return string.Empty;
        }
    }
}
