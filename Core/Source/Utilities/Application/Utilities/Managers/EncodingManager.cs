using System.Text;

namespace Sistran.Core.Application.Utilities.Managers
{
    public static class EncodingManager
    {
        public static string RemoveAccent(this string value)
        {
            Encoding encoding = Encoding.GetEncoding("iso-8859-8");

            return encoding.GetString(Encoding.Convert(Encoding.UTF8, encoding, Encoding.UTF8.GetBytes(value.Trim())));
        }

        public static string RemoveMandatoryString(this string value)
        {
            const string mandatory = "(*)";
            if (string.IsNullOrEmpty(value))
                return value;
            return value.Replace(mandatory, "").Trim();
        }
    }
}