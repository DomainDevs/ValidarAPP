using System;
using System.Linq;

namespace Sistran.Core.Application.UIF2
{
    public class UserHelper
    {
        public static int GetUserIdLogOn(string user)
        {
            string[] tmp = user.Split('|');
            int id = -1;
            if (tmp.Count() > 0)
            {
                Int32.TryParse(tmp[1], out id);
            }
            return id;
        }
    }
}