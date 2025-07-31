using Sistran.Core.Framework;
using System;

namespace Sistran.Core.Application.Utilities.Managers
{
    public class ExceptionManager
    {
        public static string GetMessage(Exception ex, string errorMessage)

        {
            if (ex.GetType() == typeof(ValidationException))
            {
                return ex.Message;
            }
            else
            {
                return errorMessage + "|" + ex.ToString();
            }
        }
    }
}